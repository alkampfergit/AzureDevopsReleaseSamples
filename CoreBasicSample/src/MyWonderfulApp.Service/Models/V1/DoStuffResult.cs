using System;

namespace MyWonderfulApp.Service.Models.V1
{
    public class DoStuffResult
    {
        public DoStuffResult(int result, string greet)
        {
            Result = result;
            Greet = greet;
        }

        public Int32 Result { get; private set; }

        public String Greet { get; private set; }
    }
}
