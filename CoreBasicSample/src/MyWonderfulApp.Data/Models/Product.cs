using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MyWonderfulApp.Data.Models
{
    public class Product
    {
        public Int32 ProductId { get; set; }

        public String ProductName { get; set; }

        public Int32 SupplierId { get; set; }

        public Int32 CategoryId { get; set; }

        public Boolean Discontinued { get; set; }

        public static Product Builder(IDataRecord arg)
        {
            return new Product()
            {
                ProductId = (Int32)arg["ProductID"],
                ProductName = arg["ProductName"] as String,
                SupplierId = (Int32)arg["SupplierId"],
                CategoryId = (Int32)arg["CategoryId"],
                Discontinued = (Boolean)arg["Discontinued"],
            };
        }
    }
}
