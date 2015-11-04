namespace SimpleMathService
{
    using System.ServiceModel;

    [ServiceContract]
    public interface IAdditionOperations
    {

        [OperationContract]
        int AddTwoNumbers(int valueA, int valueB);
    }
}
