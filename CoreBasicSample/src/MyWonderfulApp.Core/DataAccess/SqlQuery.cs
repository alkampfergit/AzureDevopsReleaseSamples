using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyWonderfulApp.Core.DataAccess
{
    public class SqlQuery
    {
        #region Properties and constructor

        internal DbCommand Command { get; set; }

        private Dictionary<string, OutputParameter> outputParameters;

        internal Dictionary<string, OutputParameter> OutputParameters
        {
            get { return outputParameters ?? (outputParameters = new Dictionary<string, OutputParameter>()); }
        }

        internal int OutputParamCount
        {
            get { return outputParameters == null ? 0 : outputParameters.Count; }
        }

        internal StringBuilder query = new StringBuilder();

        internal SqlQuery(string query, CommandType cmdType)
        {
            Command = new SqlCommand();
            Command.CommandType = cmdType;
            Command.CommandTimeout = 1200;
            this.query.Append(query);
        }

        public SqlQuery AppendToQuery(string queryFragment)
        {
            query.Append(queryFragment);
            return this;
        }

        public SqlQuery AppendLineToQuery(string queryFragment)
        {
            query.Append("\n");
            query.Append(queryFragment);
            return this;
        }

        /// <summary>
        /// Lot of the time the caller add dynamic comma separated value, so it needs
        /// to remove the last comma or the last charachter.
        /// </summary>
        /// <param name="charToTrim"></param>
        /// <returns></returns>
        public SqlQuery TrimCharFromEnd(char charToTrim)
        {
            int newLength = query.Length;
            while (charToTrim == query[--newLength])
            {
                ;
            }

            query.Length = newLength + 1;
            return this;
        }

        /// <summary>
        /// Lot of the time the caller add dynamic comma separated value, so it needs
        /// to remove the last comma or the last charachter.
        /// </summary>
        /// <param name="charToTrim"></param>
        /// <returns></returns>
        public SqlQuery TrimCharsFromEnd(int numOfCharToRemove)
        {
            query.Length -= numOfCharToRemove;
            return this;
        }

        #endregion

        #region Fluent

        public SqlQuery SetTimeout(int timeoutInSeconds)
        {
            Command.CommandTimeout = timeoutInSeconds;
            return this;
        }

        public SqlQuery FormatQuery(string baseQuery, params object[] paramList)
        {
            query.Length = 0;
            query.AppendFormat(baseQuery, paramList);
            return this;
        }

        #endregion

        #region Executor functions

        public T ExecuteScalar<T>()
        {
            T result = default;
            DataAccess.Execute(this, () =>
            {
                var tempres = Command.ExecuteScalar();
                if (tempres == null || tempres == DBNull.Value)
                {
                    result = default;
                }
                else
                {
                    result = (T)tempres;
                }
            });

            return result;
        }

        /// <summary>
        /// Execute query with a reader-like semantic.
        /// </summary>
        /// <param name="action"></param>
        /// <returns>True se la query è andata a buon fine, false se sono avvenute eccezioni all'interno della query.</returns>
        public void ExecuteReader(Action<IDataReader> action)
        {
            DataAccess.Execute(this, () =>
            {
                using (DbDataReader dr = Command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        action(dr);
                    }
                }
            });
        }

        public void ExecuteReaderMaxRecord(
            int maxRecordsToFetch,
            Action<IDataReader> action)
        {
            DataAccess.Execute(this, () =>
            {
                using (DbDataReader dr = Command.ExecuteReader())
                {
                    while (dr.Read() && maxRecordsToFetch-- >= 0)
                    {
                        action(dr);
                    }
                }
            });
        }

        public void ExecuteGetSchema(Action<DataTable> action)
        {
            DataAccess.Execute(this, () =>
            {
                using (DbDataReader dr = Command.ExecuteReader(CommandBehavior.KeyInfo))
                {
                    var schema = dr.GetSchemaTable();
                    action(schema);
                }
            });
        }

        public List<T> ExecuteBuildEntities<T>(Func<IDataRecord, T> entityBuilder)
        {
            return ExecuteBuildEntities(entityBuilder, false);
        }

        public List<T> ExecuteBuildEntities<T>(Func<IDataRecord, T> entityBuilder, bool returnNullListOnError)
        {
            List<T> retvalue = new List<T>();
            DataAccess.Execute(this, () =>
            {
                try
                {
                    using (DbDataReader dr = Command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            retvalue.Add(entityBuilder(dr));
                        }
                    }
                }
                catch (Exception ex)
                {
                    //logger.Error("Error executing " + DataAccess.DumpCommand(Command), ex);
                    retvalue = null;
                }
            });
            return retvalue;
        }

        public T ExecuteBuildSingleEntity<T>(Func<IDataReader, T> entityBuilder) where T : class
        {
            T retvalue = null;
            DataAccess.Execute(this, () =>
            {
                using (DbDataReader dr = Command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        retvalue = entityBuilder(dr);
                    }
                }
            });
            return retvalue;
        }

        /// <summary>
        /// Permette di restituire i dati come lista di tipi base, restituisce solamente i dati
        /// della prima colonna del resultset.
        /// </summary>
        /// <typeparam name="T">Può essere solamente un tipo base, tipo intero, double etc.</typeparam>
        /// <returns></returns>
        public List<T> ExecuteList<T>()
        {
            List<T> retvalue = new List<T>();
            DataAccess.Execute(this, () =>
            {
                using (DbDataReader dr = Command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        retvalue.Add(dr[0] == null || dr[0] == DBNull.Value ? default : (T)dr[0]);
                    }
                }
            });
            return retvalue;
        }

        /// <summary>
        /// Esegue la query, ma se la query da eccezione oppure torna un null torna il parametro che 
        /// indica il default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ExecuteScalarWithDefault<T>(T defaultValue)
        {
            T result = defaultValue;
            DataAccess.Execute(this, () =>
            {
                try
                {
                    object obj = Command.ExecuteScalar();
                    if (obj != DBNull.Value)
                    {
                        result = (T)(obj ?? default(T));
                    }
                    else
                    {
                        //logger.Warn("DbNull returned for query " + query);
                    }
                }
                catch (Exception)
                {
                    result = defaultValue;
                }
            });
            return result;
        }

        public int ExecuteNonQuery(bool logException = true)
        {
            int result = 0;
            DataAccess.Execute(
                this,
                () => result = Command.ExecuteNonQuery(),
                logException);
            return result;
        }

        public void FillDataTable(DataTable dt)
        {
            DataAccess.Execute(this, () =>
            {
                using (DbDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = Command;
                    da.Fill(dt);
                }
            });
        }

        public void FillDataset(DataSet ds, string tableName)
        {
            DataAccess.Execute(this, () =>
            {
                using (DbDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = Command;
                    da.Fill(ds, tableName);
                }
            });
        }

        #endregion

        #region PArameter Settings

        public SqlQuery SetStringParam(string parameterName, string value)
        {
            SetParam(parameterName, value, DbType.String);
            return this;
        }

        public SqlQuery SetList(string parameterName, IEnumerable paramList)
        {
            SetParam(parameterName, string.Join(",", paramList.OfType<object>()), DbType.String);
            return this;
        }

        public SqlQuery SetInt64Param(string parameterName, long? value)
        {
            SetParam(parameterName, value, DbType.Int64);
            return this;
        }

        public SqlQuery SetInt32Param(string parameterName, int? value)
        {
            SetParam(parameterName, value, DbType.Int32);
            return this;
        }

        public SqlQuery SetInt32ParamWithNullValue(string parameterName, int value, int nullValue)
        {
            if (value != nullValue)
            {
                SetParam(parameterName, value, DbType.Int32);
            }

            return this;
        }

        public SqlQuery SetInt16Param(string parameterName, short value)
        {
            SetParam(parameterName, value, DbType.Int16);
            return this;
        }

        public SqlQuery SetInt8Param(string parameterName, byte value)
        {
            SetParam(parameterName, value, DbType.Byte);
            return this;
        }

        public SqlQuery SetSingleParam(string parameterName, float? value)
        {
            SetParam(parameterName, value, DbType.Single);
            return this;
        }

        public SqlQuery SetDoubleParam(string parameterName, double? value)
        {
            SetParam(parameterName, value, DbType.Single);
            return this;
        }

        public SqlQuery SetBooleanParam(string parameterName, bool? value)
        {
            SetParam(parameterName, value, DbType.Boolean);
            return this;
        }

        public SqlQuery SetGuidParam(string parameterName, Guid value)
        {
            SetParam(parameterName, value, DbType.Guid);
            return this;
        }

        public SqlQuery SetBooleanParam(string parameterName, bool value)
        {
            SetParam(parameterName, value, DbType.Boolean);
            return this;
        }

        public SqlQuery SetDateTimeParam(string parameterName, DateTime value)
        {
            SetParam(parameterName, value, DbType.DateTime);
            return this;
        }

        public SqlQuery SetDateTimeParam(string parameterName, DateTime? value)
        {
            SetParam(parameterName, value, DbType.DateTime);
            return this;
        }

        public SqlQuery SetFloatParam(string parameterName, float value)
        {
            SetParam(parameterName, value, DbType.Single);
            return this;
        }

        public SqlQuery SetParam(string commandName, object value, DbType? type = null)
        {
            string paramName = DataAccess.GetParameterName(Command, commandName);
            if (Command.CommandType == CommandType.Text)
            {
                query.Replace("{" + commandName + "}", paramName);
            }

            DbParameter param = new SqlParameter();
            if (type != null)
            {
                param.DbType = type.Value;
            }

            param.ParameterName = paramName;
            param.Value = value ?? DBNull.Value;
            Command.Parameters.Add(param);
            return this;
        }

        public string SetOutParam(string commandName, DbType type)
        {
            string paramName = DataAccess.GetParameterName(Command, commandName);
            if (Command.CommandType == CommandType.Text)
            {
                query.Replace("{" + commandName + "}", paramName);
            }

            DbParameter param = new SqlParameter();
            param.DbType = type;
            param.ParameterName = paramName;
            param.Direction = ParameterDirection.Output;
            Command.Parameters.Add(param);
            return paramName;
        }

        #endregion

        #region OutputParameter

        public SqlQuery SetInt32OutParam(string paramName)
        {
            string pname = SetOutParam(paramName, DbType.Int32);
            OutputParameters.Add(paramName, new OutputParameter(pname, typeof(int)));
            return this;
        }

        public SqlQuery SetInt64OutParam(string paramName)
        {
            string pname = SetOutParam(paramName, DbType.Int64);
            OutputParameters.Add(paramName, new OutputParameter(pname, typeof(long)));
            return this;
        }

        public T GetOutParam<T>(string paramName)
        {
            return (T)outputParameters[paramName].Value;
        }

        #endregion

        #region OrmLike

        /// <summary>
        /// Idrata una entità dove ogni nome di prorpietà però deve essere presente nel dareader corrispondente.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> Hydrate<T>() where T : new()
        {
            var properties = typeof(T).GetProperties();

            List<T> retvalue = new List<T>();
            DataAccess.Execute(this, () =>
            {
                HashSet<PropertyInfo> availableFields = null;
                using (DbDataReader dr = Command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (availableFields == null)
                        {
                            availableFields = new HashSet<PropertyInfo>();
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                var fieldName = dr.GetName(i);
                                var property = properties.FirstOrDefault(p => p.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
                                if (property != null)
                                {
                                    availableFields.Add(property);
                                }
                            }
                        }
                        retvalue.Add(Hydrater<T>(dr, availableFields));
                    }
                }
            });
            return retvalue;
        }

        public T HydrateSingle<T>() where T : class, new()
        {
            var properties = typeof(T).GetProperties();
            T entity = null;
            DataAccess.Execute(this, () =>
            {
                HashSet<PropertyInfo> availableFields = null;
                using (DbDataReader dr = Command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (availableFields == null)
                        {
                            availableFields = new HashSet<PropertyInfo>();
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                var fieldName = dr.GetName(i);
                                var property = properties.FirstOrDefault(p => p.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
                                if (property != null)
                                {
                                    availableFields.Add(property);
                                }
                            }
                        }
                        entity = Hydrater<T>(dr, availableFields);
                    }
                }
            });

            return entity;
        }

        private T Hydrater<T>(DbDataReader dr, HashSet<PropertyInfo> availableFields) where T : new()
        {
            T instance = new T();
            foreach (var property in availableFields)
            {
                if (dr[property.Name] != DBNull.Value)
                {
                    property.SetValue(instance, dr[property.Name], new object[] { });
                }
            }
            return instance;
        }

        #endregion
    }
}
