﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IAdditionOperations")]
public interface IAdditionOperations
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAdditionOperations/AddTwoNumbers", ReplyAction="http://tempuri.org/IAdditionOperations/AddTwoNumbersResponse")]
    int AddTwoNumbers(int valueA, int valueB);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAdditionOperations/AddTwoNumbers", ReplyAction="http://tempuri.org/IAdditionOperations/AddTwoNumbersResponse")]
    System.Threading.Tasks.Task<int> AddTwoNumbersAsync(int valueA, int valueB);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IAdditionOperationsChannel : IAdditionOperations, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class AdditionOperationsClient : System.ServiceModel.ClientBase<IAdditionOperations>, IAdditionOperations
{
    
    public AdditionOperationsClient()
    {
    }
    
    public AdditionOperationsClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public AdditionOperationsClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public AdditionOperationsClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public AdditionOperationsClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public int AddTwoNumbers(int valueA, int valueB)
    {
        return base.Channel.AddTwoNumbers(valueA, valueB);
    }
    
    public System.Threading.Tasks.Task<int> AddTwoNumbersAsync(int valueA, int valueB)
    {
        return base.Channel.AddTwoNumbersAsync(valueA, valueB);
    }
}
