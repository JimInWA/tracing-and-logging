namespace SimpleMathService
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    /// <summary>
    /// IAdditionOperations interface
    /// Sample web service used for the tracing and logging example
    /// </summary>
    [ServiceContract]
    public interface IAdditionOperations
    {
        /// <summary>
        /// Sample web service method that has a deliberate bug in it for the purposes of walking through the tracing and logging example
        /// </summary>
        /// <param name="valueA"></param>
        /// <param name="valueB"></param>
        /// <returns></returns>
        [OperationContract]
        [WebGet]
        int AddTwoNumbers(int valueA, int valueB);
    }
}
