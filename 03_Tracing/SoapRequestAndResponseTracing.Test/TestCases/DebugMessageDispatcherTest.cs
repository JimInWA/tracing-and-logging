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
    /// DebugMessageDispatcher tests
    /// </summary>
    [TestClass]
    public class DebugMessageDispatcherTest : UnitTestBaseClass
    {
        /// <summary>
        /// DebugMessageDispatcher_BeforeSendRequest_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageDispatcher")]
        public void DebugMessageDispatcher_BeforeSendRequest_Success()
        {
            // Arrange
            var messageText = File.ReadAllText(DispatcherSampleRequestFullPath);

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Default, "*", XmlTextReader.Create(new StringReader(messageText)));

            // ToDo: introduce FakeItEasy so we convert this into a unit test
            // ToDo: Autofac with FakeItEasy

            // Act
            var myHelper = new Helper();
            var myLogger = new Logger();
            var myDebugMessageDispatcher = new DebugMessageDispatcher(myHelper, myLogger);
            IClientChannel channel = null;
            InstanceContext context = null;
            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in test project App.config is true, then the value should be logged to the DB
                // However, since we are using TPL (Task Parallel Library) Task.Factory.StartNew, the below call will finish before the item is
                // actually logged to the DB
                var actualMessage = myDebugMessageDispatcher.AfterReceiveRequest(ref expectedMessage, channel, context);
            }
            catch (Exception ex)
            {
                Assert.Fail("{0}", ex);
            }
        }

        /// <summary>
        /// DebugMessageDispatcher_StartLoggingTheRequest_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageDispatcher")]
        public void DebugMessageDispatcher_StartLoggingTheRequest_Success()
        {
            // Arrange
            var messageText = File.ReadAllText(DispatcherSampleRequestFullPath);

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Default, "*", XmlTextReader.Create(new StringReader(messageText)));

            // ToDo: introduce FakeItEasy so we convert this into a unit test
            // ToDo: Autofac with FakeItEasy

            // Act
            var myHelper = new Helper();
            var myLogger = new Logger();
            var myDebugMessageDispatcher = new DebugMessageDispatcher(myHelper, myLogger);
            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in test project App.config is true, then the value should be logged to the DB
                // However, since we are using TPL (Task Parallel Library) Task.Factory.StartNew, the below call will finish before the item is
                // actually logged to the DB
                myDebugMessageDispatcher.StartLoggingTheRequest(expectedMessage);
            }
            catch (Exception ex)
            {
                Assert.Fail("{0}", ex);
            }
        }

        /// <summary>
        /// DebugMessageDispatcher_AfterReceiveReply_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageDispatcher")]
        public void DebugMessageDispatcher_AfterReceiveReply_Success()
        {
            // Arrange
            var messageText = File.ReadAllText(DispatcherSampleReplyFullPath);

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Default, "*", XmlTextReader.Create(new StringReader(messageText)));

            // ToDo: introduce FakeItEasy so we convert this into a unit test
            // ToDo: Autofac with FakeItEasy

            // Act
            var myHelper = new Helper();
            var myLogger = new Logger();
            var myDebugMessageDispatcher = new DebugMessageDispatcher(myHelper, myLogger);
            object myCorrelationState = null;
            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in test project App.config is true, then the value should be logged to the DB
                // However, since we are using TPL (Task Parallel Library) Task.Factory.StartNew, the below call will finish before the item is
                // actually logged to the DB
                myDebugMessageDispatcher.BeforeSendReply(ref expectedMessage, myCorrelationState);
            }
            catch(Exception ex)
            {
                Assert.Fail("{0}", ex);
            }
        }

        /// <summary>
        /// DebugMessageDispatcher_StartLoggingTheReply_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageDispatcher")]
        public void DebugMessageDispatcher_StartLoggingTheReply_Success()
        {
            // Arrange
            var messageText = File.ReadAllText(DispatcherSampleReplyFullPath);

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Default, "*", XmlTextReader.Create(new StringReader(messageText)));

            // ToDo: introduce FakeItEasy so we convert this into a unit test
            // ToDo: Autofac with FakeItEasy

            // Act
            var myHelper = new Helper();
            var myLogger = new Logger();
            var myDebugMessageDispatcher = new DebugMessageDispatcher(myHelper, myLogger);
            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in test project App.config is true, then the value should be logged to the DB
                // However, since we are using TPL (Task Parallel Library) Task.Factory.StartNew, the below call will finish before the item is
                // actually logged to the DB
                myDebugMessageDispatcher.StartLoggingTheReply(expectedMessage);
            }
            catch (Exception ex)
            {
                Assert.Fail("{0}", ex);
            }
        }
    }
}
