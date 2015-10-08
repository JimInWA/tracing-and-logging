namespace SoapRequestAndResponseTracing.Test.TestCases
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Xml;

    using SoapRequestAndResponseTracing;

    [TestClass]
    public class DebugMessageInspectorTest
    {
        [TestMethod]
        // Note: At this point, you need to have manually run the web service in order to run this test
        public void TestMethod1()
        {
            // Arrange
            var request = Message.CreateMessage(MessageVersion.Soap11, "http://schemas.xmlsoap.org/soap/envelope/", "<AddTwoNumbers xmlns=\"http://tempuri.org/\"><valueA>3</valueA><valueB>3</valueB></AddTwoNumbers>");

            //Step1: create a binding with just HTTP
            var binding = new CustomBinding();
            binding.Elements.Add(new TcpTransportBindingElement());
            
            //Step2: use the binding to build the channel factory
            var bContext = new BindingContext(binding, new BindingParameterCollection());
            var factory = binding.BuildChannelFactory<IOutputChannel>(bContext);

            //open the channel factory
            factory.Open();

            //Step3: use the channel factory to create a channel
            var channel = factory.CreateChannel(new EndpointAddress("http://localhost:8080/channelapp"));

            // Create SUT / MUT (System under test / method under test)
            var myDebugMessageInspector = new DebugMessageInspector();

            // Act
            var result = myDebugMessageInspector.BeforeSendRequest(ref request, (IClientChannel)channel);
        }

        [TestMethod]
        // Note: At this point, you need to have manually run the web service in order to run this test
        public void TestMethod2()
        {
            //Step1: Create a binding with just HTTP.
            BindingElement[] bindingElements = new BindingElement[2];
            bindingElements[0] = new TextMessageEncodingBindingElement();
            bindingElements[1] = new HttpTransportBindingElement();
            CustomBinding binding = new CustomBinding(bindingElements);

            //Step2: Use the binding to build the channel factory.
            IChannelFactory<IRequestChannel> factory =
            binding.BuildChannelFactory<IRequestChannel>(
                             new BindingParameterCollection());
            //Open the channel factory.
            factory.Open();

            //Step3: Use the channel factory to create a channel.
            IRequestChannel channel = factory.CreateChannel(
               new EndpointAddress("http://localhost:13048/AdditionOperations.svc"));
            channel.Open();

            //Step4: Create a message.
            //Message requestmessage = Message.CreateMessage(
            //    binding.MessageVersion,
            //    "http://contoso.com/someaction",
            //     "This is the body data");

            string requestAsString = @"
                <AddTwoNumbers>
                  <valueA>3</valueA>
                  <valueB>3</valueB>
                </AddTwoNumbers>";

            XmlDocument requestAsXML = new XmlDocument();
            requestAsXML.LoadXml(requestAsString);

            Message requestmessage = Message.CreateMessage(MessageVersion.Soap11, "http://schemas.xmlsoap.org/soap/envelope/", requestAsXML);

            //Send message.
            Message replymessage = channel.Request(requestmessage);
            Console.WriteLine("Reply message received");
            Console.WriteLine("Reply action: {0}",
                                  replymessage.Headers.Action);
            string data = replymessage.GetBody<string>();
            Console.WriteLine("Reply content: {0}", data);

            //Step5: Do not forget to close the message.
            replymessage.Close();
            //Do not forget to close the channel.
            channel.Close();
            //Do not forget to close the factory.
            factory.Close();
        }
    }            
}
