namespace SimpleMathService
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public interface IAdditionOperations
    {

        [OperationContract]
        [WebGet]
        int AddTwoNumbers(int valueA, int valueB);
    }
}
