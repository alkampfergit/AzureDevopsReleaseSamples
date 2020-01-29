using MyWonderfulApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWonderfulApp.Service.Models.V1
{
public class GetCustomer2Dto
{
    public CustomerId CustomerId { get; set; }

    public Int32 OtherParam { get; set; }
}
}
