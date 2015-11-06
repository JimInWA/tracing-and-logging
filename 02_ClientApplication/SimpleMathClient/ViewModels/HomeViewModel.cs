namespace SimpleMathClient.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// HomeViewModel class
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// InputA public property (only get is public)
        /// </summary>
        public int InputA { get; private set; }

        /// <summary>
        /// InputB public property (only get is public)
        /// </summary>
        public int InputB { get; private set; }

        /// <summary>
        /// Result public property (only get is public)
        /// </summary>
        public int Result { get; private set; }

        /// <summary>
        /// AddTwoNumbers method - calls the web service and then modifies the value in Result
        /// </summary>
        /// <param name="inputA"></param>
        /// <param name="inputB"></param>
        public void AddTwoNumbers(int inputA, int inputB)
        {
            InputA = inputA;
            InputB = inputB;

            var additionOperationProxy = new AdditionOperationsClient();

            Result = additionOperationProxy.AddTwoNumbers(inputA, inputB);
        }
    }
}