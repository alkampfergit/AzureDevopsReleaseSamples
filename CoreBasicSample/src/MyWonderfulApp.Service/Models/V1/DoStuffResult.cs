using System;

namespace MyWonderfulApp.Service.Models.V1
{
    public class DoStuffResult
    {
        public DoStuffResult(int result, string greet, String sampleConfigurationValue)
        {
            Result = result;
            Greet = greet;
            SampleConfigurationValue = sampleConfigurationValue;
        }

        public Int32 Result { get; private set; }

        public String Greet { get; private set; }

        public String SampleConfigurationValue { get; set; }
    }
}
