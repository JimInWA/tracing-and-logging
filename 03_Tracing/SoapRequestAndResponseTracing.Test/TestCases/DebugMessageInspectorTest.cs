namespace SoapRequestAndResponseTracing.Test.TestCases
{
    using System;
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Xml;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SoapRequestAndResponseTracing;
    using SoapRequestAndResponseTracing.Test.Framework;


    /// <summary>
    /// DebugMessageInspector tests
    /// </summary>
    [TestClass]
    public class DebugMessageInspectorTest : UnitTestBaseClass
    {
        /// <summary>
        /// DebugMessageInspector_BeforeSendRequest_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageInspector")]
        public void DebugMessageInspector_BeforeSendRequest_Success()
        {
            // Arrange
            var messageText = File.ReadAllText(InspectorSampleRequestFullPath);

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Default, "*", XmlTextReader.Create(new StringReader(messageText)));

            // ToDo: introduce FakeItEasy so we convert this into a unit test
            // ToDo: Autofac with FakeItEasy

            // Act
            var myHelper = new Helper();
            var myLogger = new Logger();
            var myDebugMessageInspector = new DebugMessageInspector(myHelper, myLogger);
            IClientChannel channel = null;
            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in App.config is true, then the value should be logged to the DB
                var actualMessage = myDebugMessageInspector.BeforeSendRequest(ref expectedMessage, channel);
            }
            catch (Exception ex)
            {
                Assert.Fail("{0}", ex);
            }
        }

        /// <summary>
        /// DebugMessageInspector_StartLoggingTheRequest_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageInspector")]
        public void DebugMessageInspector_StartLoggingTheRequest_Success()
        {
            // Arrange
            var messageText = File.ReadAllText(InspectorSampleRequestFullPath);

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Default, "*", XmlTextReader.Create(new StringReader(messageText)));

            // ToDo: introduce FakeItEasy so we convert this into a unit test
            // ToDo: Autofac with FakeItEasy

            // Act
            var myHelper = new Helper();
            var myLogger = new Logger();
            var myDebugMessageInspector = new DebugMessageInspector(myHelper, myLogger);
            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in App.config is true, then the value should be logged to the DB
                myDebugMessageInspector.StartLoggingTheRequest(expectedMessage);
            }
            catch (Exception ex)
            {
                Assert.Fail("{0}", ex);
            }
        }

        /// <summary>
        /// DebugMessageInspector_AfterReceiveReply_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageInspector")]
        public void DebugMessageInspector_AfterReceiveReply_Success()
        {
            // Arrange
            var messageText = File.ReadAllText(InspectorSampleReplyFullPath);

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Default, "*", XmlTextReader.Create(new StringReader(messageText)));

            // ToDo: introduce FakeItEasy so we convert this into a unit test
            // ToDo: Autofac with FakeItEasy

            // Act
            var myHelper = new Helper();
            var myLogger = new Logger();
            var myDebugMessageInspector = new DebugMessageInspector(myHelper, myLogger);
            object myCorrelationState = null;
            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in App.config is true, then the value should be logged to the DB
                myDebugMessageInspector.AfterReceiveReply(ref expectedMessage, myCorrelationState);
            }
            catch (Exception ex)
            {
                Assert.Fail("{0}", ex);
            }
        }

        /// <summary>
        /// DebugMessageInspector_StartLoggingTheReply_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageInspector")]
        public void DebugMessageInspector_StartLoggingTheReply_Success()
        {
            // Arrange
            var messageText = File.ReadAllText(InspectorSampleReplyFullPath);

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Default, "*", XmlTextReader.Create(new StringReader(messageText)));

            // ToDo: introduce FakeItEasy so we convert this into a unit test
            // ToDo: Autofac with FakeItEasy

            // Act
            var myHelper = new Helper();
            var myLogger = new Logger();
            var myDebugMessageInspector = new DebugMessageInspector(myHelper, myLogger);
            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in App.config is true, then the value should be logged to the DB
                myDebugMessageInspector.StartLoggingTheReply(expectedMessage);
            }
            catch (Exception ex)
            {
                Assert.Fail("{0}", ex);
            }
        }
    }
}
