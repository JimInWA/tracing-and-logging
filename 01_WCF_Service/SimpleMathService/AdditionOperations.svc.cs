namespace SimpleMathService
{
    public class AdditionOperations : IAdditionOperations
    {
        public int AddTwoNumbers(int valueA, int valueB)
        {
            // simple error that could have been caught with a unit test, here just for the proof of concept
            var result = valueA + valueB - 1;
            return result;
        }
    }
}
