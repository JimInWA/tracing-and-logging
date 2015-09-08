using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SimpleMathService
{
    public class Service1 : IAdditionOperations
    {
        public int AddTwoNumbers(int valueA, int valueB)
        {
            // simple error that could have been caught with a unit test, here just for the proof of concept
            var result = valueA + valueB - 1;
            return result;
        }
    }
}
