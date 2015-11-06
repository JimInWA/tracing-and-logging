using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleMathClient.ViewModels
{
    public class HomeViewModel
    {
        public int InputA { get; private set; }
        public int InputB { get; private set; }
        public int Result { get; private set; }

        public void AddTwoNumbers(int inputA, int inputB)
        {
            InputA = inputA;
            InputB = inputB;

            var additionOperationProxy = new AdditionOperationsClient();

            Result = additionOperationProxy.AddTwoNumbers(inputA, inputB);
        }
    }
}