using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace MyWonderfulApp.Core.DataAccess
{
    public static class DataAccess
    {
        /// <summary>
        /// Incapsula i dati di una connessione e tiene traccia del fatto che siamo 
        /// o non siamo in una transazione globale.
        /// </summary>
        internal class ConnectionData : IDisposable
        {
            public string ConnectionString { get; set; }

            private bool IsWeakReference { get; set; }

            private bool IsCommittedOrRolledBack = false;
            private bool IsEnlistedInNhibernateTransaction = false;

            private DbTransaction _transaction;
            public DbTransaction Transaction
            {
                get
                {
                    if (IsEnlistedInNhibernateTransaction)
                    {
                        throw new NotSupportedException("Some code is trying to enlist into a transaction when transaction is not available");
                    }
                    return _transaction;
                }
            }

            private DbConnection _connection;
            public DbConnection Connection
            {
                get
                {
                    if (IsEnlistedInNhibernateTransaction)
                    {
                        throw new NotSupportedException("Some code is trying to enlist into a connection when transaction is not available");
                    }
                    return _connection;
                }
            }

            internal static ConnectionData CreateConnectionData(string connectionString)
            {
                return new ConnectionData(connectionString, false);
            }

            internal static ConnectionData CreateWeakConnectionData(ConnectionData data)
            {
                var conn = new ConnectionData(data.ConnectionString, true);
                conn._connection = data._connection;
                conn._transaction = data._transaction;
                conn.IsEnlistedInNhibernateTransaction = data.IsEnlistedInNhibernateTransaction;
                return conn;
            }

            private ConnectionData(string connectionString, bool isWeakReference)
            {
                ConnectionString = connectionString;
                IsWeakReference = isWeakReference;
            }

            internal void Commit()
            {

                if (IsWeakReference)
                {
                    return;
                }

                IsCommittedOrRolledBack = true;
                if (!IsEnlistedInNhibernateTransaction && _transaction != null)
                {
                    _transaction.Commit();
                }
            }

            internal void Rollback()
            {
                if (IsWeakReference)
                {
                    return;
                }

                IsCommittedOrRolledBack = true;
                if (!IsEnlistedInNhibernateTransaction && _transaction != null)
                {
                    _transaction.Rollback();
                }
            }

            public void Dispose()
            {
                if (IsWeakReference || IsEnlistedInNhibernateTransaction)
                {
                    return;
                }

                using (Connection)
                using (Transaction)
                {
                    if (!IsCommittedOrRolledBack)
                    {
                        //We are disposing and a transation is still active, rollback everythin
                        Transaction.Rollback();
                    }
                }
            }


            /// <summary>
            /// preso un comando lo enlista alla connessione e transazione correnti.
            /// </summary>
            /// <param name="dbCommand"></param>
            internal void EnlistCommand(DbCommand dbCommand)
            {
                //la connessione è già stata creata da qualcun'altro, per
                //questa ragione enlistiamo il comando e basta
                if (_connection != null)
                {
                    dbCommand.Connection = _connection;
                    dbCommand.Transaction = _transaction;
                    return;
                }

                _connection = new SqlConnection();
                _connection.ConnectionString = ConnectionString;
                _connection.Open();
                _transaction = _connection.BeginTransaction();
                dbCommand.Connection = _connection;
                dbCommand.Transaction = Transaction;
            }
        }

        //#endregion

        #region Static Initialization

        /// <summary>
        /// Set connection string before using the class if you want to use a default
        /// connection string.
        /// </summary>
        public static string ConnectionString { get; private set; }

        public static void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #endregion

        #region Execution core

        private static ConnectionData CreateConnection(string connection)
        {
            return ConnectionData.CreateConnectionData(connection ?? ConnectionString);
        }

        /// <summary>
        /// Execute a sqlquery. This is the basic place where the query takes really place
        /// </summary>
        /// <param name="q">Query to be executed</param>
        /// <param name="executionCore">Function to execute</param>
        /// <param name="connection">Connection used to create </param>
        /// <param name="logException">If false it will not log the exception,
        /// exception will be always rethrow</param>
        public static void Execute(
            SqlQuery q,
            Action executionCore,
            bool logException = true)
        {
            using (ConnectionData connectionData = CreateConnection(q.Connection))
            {
                try
                {
                    using (q.Command)
                    {
                        connectionData.EnlistCommand(q.Command);
                        q.Command.CommandText = q.query.ToString();
                        Log.Logger.Debug(DumpCommand(q.Command));
                        executionCore();
                        //Now handle output parameters if any
                        if (q.OutputParamCount > 0)
                        {
                            foreach (KeyValuePair<string, OutputParameter> parameter in q.OutputParameters)
                            {
                                parameter.Value.Value = q.Command.Parameters[parameter.Value.Name].Value;
                            }
                        }
                    }
                    connectionData.Commit();
                }
                catch (Exception ex)
                {
                    if (logException)
                    {
                        Log.Error(ex, "Could not execute Query {Query}:", DumpCommand(q.Command));
                    }
                    connectionData.Rollback();
                    throw;
                }
            }
        }

        private static object GetValueFromParameter(DbParameter parameter)
        {
            if (parameter == null || parameter.Value == null || parameter.Value == DBNull.Value)
            {
                return "NULL";
            }
            return parameter.Value;
        }

        public static string DumpCommand(DbCommand command)
        {
            if (command == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Data Access Dumper:");

            if (command.CommandType == CommandType.StoredProcedure)
            {
                sb.Append("EXEC " + command.CommandText + " ");
                foreach (DbParameter parameter in command.Parameters)
                {
                    if (parameter.DbType == DbType.String)
                    {
                        sb.AppendFormat("{0}='{1}', ", parameter.ParameterName, GetValueFromParameter(parameter));
                    }
                    else
                    {
                        sb.AppendFormat("{0}={1}, ", parameter.ParameterName, GetValueFromParameter(parameter));
                    }
                }
                if (command.Parameters.Count > 0)
                {
                    sb.Length -= 2;
                }
            }
            else
            {
                foreach (DbParameter parameter in command.Parameters)
                {
                    sb.AppendFormat("DECLARE {0} {2} = {1}\n",
                        parameter.ParameterName,
                        parameter.Value,
                       GetDeclarationTypeFromDbType(parameter.DbType));
                }
                sb.AppendLine(command.CommandText);
            }
            return sb.ToString();
        }

        private static string GetDeclarationTypeFromDbType(DbType type)
        {
            switch (type)
            {
                case DbType.Int32: return "INT";
                case DbType.Int16: return "SMALLINT";
                case DbType.Int64: return "BIGINT";
                case DbType.String: return "varchar(max)";
            }
            return type.ToString();
        }

        /// <summary>
        /// This is the core execution function, it accept a simple functor that will accept a sqlcommand
        /// the command is created in the core of the function so it really care of all the standard
        /// burden of creating connection, creating transaction and enlist command into a transaction.
        /// </summary>
        /// <param name="functionToExecute">The delegates that really executes the command.</param>
        /// <param name="connection"></param>
        public static void Execute(Action<DbCommand> functionToExecute)
        {
            using (ConnectionData connectionData = CreateConnection(null))
            {
                DbCommand command = null;
                try
                {
                    using (command = new SqlCommand())
                    {
                        command.CommandTimeout = 120;
                        command.CommandType = CommandType.Text;
                        connectionData.EnlistCommand(command);
                        functionToExecute(command);
                    }
                    connectionData.Commit();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Could not execute Query {Query}:", DumpCommand(command));
                    connectionData.Rollback();
                    throw;
                }
            }
        }

        #endregion

        #region helper function

        /// <summary>
        /// This function Execute a command, it accepts a function with no parameter that
        /// Prepare a command to be executed. It internally use the 
        ///
        /// function that really executes the code.
        /// </summary>
        /// <typeparam name="T">return parameter type, it reflect the return type
        /// of the delegates</typeparam>
        /// <param name="functionToExecute">The function that prepares the command that should
        /// be executed with execute scalar.</param>
        /// <returns></returns>
        public static T ExecuteScalar<T>(Action<DbCommand> functionToExecute)
        {
            T result = default;
            Execute((command) =>
            {
                functionToExecute(command);
                object o = command.ExecuteScalar();
                //result = (T)o; //execute scalar mi ritorna un decimal...che non posso castare
                result = (T)Convert.ChangeType(o, typeof(T));
            });
            return result;
        }

        public static List<T> ExecuteGetEntity<T>(Action<DbCommand> functionToExecute, Func<IDataRecord, T> select)
        {
            List<T> retvalue = new List<T>();
            Execute((c) =>
            {
                functionToExecute(c);
                using (IDataReader dr = c.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        retvalue.Add(select(dr));
                    }
                }
            });
            return retvalue;
        }

        /// <summary>
        /// Execute a command with no result.
        /// </summary>
        /// <param name="functionToExecute"></param>
        public static int ExecuteNonQuery(Action<DbCommand> functionToExecute)
        {
            int result = -1;
            Execute((command) =>
            {
                functionToExecute(command);
                result = command.ExecuteNonQuery();
            });
            return result;
        }

        /// <summary>
        /// This is the function that permits to use a datareader without any risk
        /// to forget datareader open.
        /// </summary>
        /// <param name="commandPrepareFunction">The delegate should accepts 3 parameter, 
        /// the command to configure, a factory to create parameters, and finally another
        /// delegate of a function that returns the datareader.</param>
        public static void ExecuteReader(
            Action<DbCommand, Func<IDataReader>> commandPrepareFunction)
        {
            Execute((command) =>
            {
                //The code to execute only assures that the eventually created datareader would be
                //closed in a finally block.
                IDataReader dr = null;
                try
                {
                    commandPrepareFunction(command,
                        () =>
                        {
                            dr = command.ExecuteReader();
                            return dr;
                        });
                }
                finally
                {
                    dr?.Dispose();
                }
            });
        }

        public static void FillDataset(
            DataTable table,
            Action<DbCommand> commandPrepareFunction)
        {
            Execute(
                (command) =>
                {
                    commandPrepareFunction(command);
                    using (DbDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = command;
                        da.Fill(table);
                    }
                });
        }

        public static void ExecuteDataset<T>(
            string tableName,
            Action<DbCommand, Func<T>> commandPrepareFunction)
            where T : DataSet, new()
        {
            Execute((command) =>
            {
                //The code to execute only assures that the eventually created datareader would be
                //closed in a finally block.
                using (T ds = new T())
                {
                    commandPrepareFunction(command,
                        () =>
                        {
                            using (DbDataAdapter da = new SqlDataAdapter())
                            {
                                da.SelectCommand = command;
                                da.Fill(ds, tableName);
                            }
                            return ds;
                        });
                }
            });
        }

        /// <summary>
        /// This is the function that permits to use a datareader without any risk
        /// to forget datareader open.
        /// </summary>
        /// <param name="commandPrepareFunction"></param>
        public static void ExecuteDataset(
            Action<DbCommand, Func<DataSet>> commandPrepareFunction)
        {
            Execute((command) =>
            {
                //The code to execute only assures that the eventually created datareader would be
                //closed in a finally block.
                using (DataSet ds = new DataSet())
                {
                    commandPrepareFunction(command,
                        () =>
                        {
                            using (DbDataAdapter da = new SqlDataAdapter())
                            {
                                da.SelectCommand = command;
                                da.Fill(ds);
                            }
                            return ds;
                        });
                }
            });
        }

        #endregion

        #region Command filler and helpers

        /// <summary>
        /// Add a parameter to a command.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="factory"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddParameterToCommand(
            DbCommand command,
            DbProviderFactory factory,
            DbType type,
            string name,
            object value)
        {
            DbParameter param = factory.CreateParameter();
            param.DbType = type;
            param.ParameterName = GetParameterName(command, name);
            param.Value = value;

            command.Parameters.Add(param);
        }

        public static string GetParameterName(DbCommand command, string parameterName)
        {
            return string.Format("@{0}", parameterName);
        }

        #endregion

        #region FluentInterface

        public static SqlQuery CreateQuery(string s)
        {
            return new SqlQuery(s, CommandType.Text);
        }

        /// <summary>
        /// Access database with a special readonly user.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static SqlQuery CreateReadonlyQuery(string s)
        {
            var query = new SqlQuery(s, CommandType.Text);
            query.Connection = MyWonderfulAppConfiguration.Instance.ReadonlyConnectionString;
            return query;
        }

        #endregion
    }
}
