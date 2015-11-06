namespace SimpleMathService
{
    /// <summary>
    /// Sample web service used for the tracing and logging example
    /// </summary>
    public class AdditionOperations : IAdditionOperations
    {
        /// <summary>
        /// AdditionOperations service - implements IAdditionOperations interface
        /// Sample web service method that has a deliberate bug in it for the purposes of walking through the tracing and logging example
        /// </summary>
        /// <param name="valueA"></param>
        /// <param name="valueB"></param>
        /// <returns></returns>
        public int AddTwoNumbers(int valueA, int valueB)
        {
            // simple error that could have been caught with a unit test, here just for the proof of concept
            var result = valueA + valueB - 1;
            return result;
        }
    }
}
