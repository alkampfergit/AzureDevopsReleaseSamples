using MyWonderfulApp.Data.Models;
using System;

namespace MyWonderfulApp.Service.Models.V1
{
    public class GetCustomer2Dto
    {
        public CustomerId CustomerId { get; set; }

        public Int32 OtherParam { get; set; }
    }
}
