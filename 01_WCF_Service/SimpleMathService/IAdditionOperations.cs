using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SimpleMathService
{
    [ServiceContract]
    public interface IAdditionOperations
    {

        [OperationContract]
        int AddTwoNumbers(int valueA, int valueB);
    }
}
