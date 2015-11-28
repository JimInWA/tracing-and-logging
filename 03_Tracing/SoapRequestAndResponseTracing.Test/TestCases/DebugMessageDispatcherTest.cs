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
        #region Test attributes

        /// <summary>
        /// DebugMessageDispatcherForTests is the DebugMessageDispatcher used for the tests
        /// </summary>
        public DebugMessageDispatcher DebugMessageDispatcherForTests;

        /// <summary>
        /// Urn is a Guid used for each test
        /// </summary>
        public Guid Urn;

        #endregion

        #region Additional test attributes
        /// <summary>
        /// Use TestInitialize to run code before running each test
        /// </summary>
        [TestInitialize()]
        public void MyTestInitialize()
        {
            Urn = Guid.NewGuid(); 
            var myHelper = new Helper();
            var myLogger = new Logger();
            DebugMessageDispatcherForTests = new DebugMessageDispatcher(myHelper, myLogger);
        }
        #endregion

        
        /// <summary>
        /// DebugMessageDispatcher_AfterReceiveRequest_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageDispatcher")]
        public void DebugMessageDispatcher_AfterReceiveRequest_Success()
        {
            // Arrange
            const string methodName = "DebugMessageDispatcher_AfterReceiveRequest_Success";
            var uniqueId = new UniqueId(Urn);
            IClientChannel channel = null;
            InstanceContext context = null;
            var messageText = File.ReadAllText(DispatcherSampleRequestFullPath).Replace("urn:uuid:00000000-0000-0000-0000-000000000000", uniqueId.ToString());
            var xmlReader = XmlReader.Create(new StringReader(messageText));

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Soap11WSAddressing10, methodName, xmlReader);

            // Because this is the incoming request, set MessageId
            expectedMessage.Headers.MessageId = uniqueId;

            // Act
            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in test project App.config is true, then the value should be logged to the DB
                // However, since we are using TPL (Task Parallel Library) Task.Factory.StartNew, the below call will finish before the item is
                // actually logged to the DB
                var actualMessage = DebugMessageDispatcherForTests.AfterReceiveRequest(ref expectedMessage, channel, context);
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
            const string methodName = "DebugMessageDispatcher_StartLoggingTheRequest_Success";
            var uniqueId = new UniqueId(Urn);
            var messageText = File.ReadAllText(DispatcherSampleRequestFullPath).Replace("urn:uuid:00000000-0000-0000-0000-000000000000", uniqueId.ToString());
            var xmlReader = XmlReader.Create(new StringReader(messageText));

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Soap11WSAddressing10, methodName, xmlReader);

            // Because this is the incoming request, set MessageId
            expectedMessage.Headers.MessageId = uniqueId;

            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in test project App.config is true, then the value should be logged to the DB
                // However, since we are using TPL (Task Parallel Library) Task.Factory.StartNew, the below call will finish before the item is
                // actually logged to the DB
                DebugMessageDispatcherForTests.StartLoggingTheRequest(expectedMessage);
            }
            catch (Exception ex)
            {
                Assert.Fail("{0}", ex);
            }
        }

        /// <summary>
        /// DebugMessageDispatcher_BeforeSendReply_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageDispatcher")]
        public void DebugMessageDispatcher_BeforeSendReply_Success()
        {
            // Arrange
            const string methodName = "DebugMessageDispatcher_BeforeSendReply_Success";
            var uniqueId = new UniqueId(Urn);
            object myCorrelationState = null;
            var messageText = File.ReadAllText(DispatcherSampleReplyFullPath).Replace("urn:uuid:00000000-0000-0000-0000-000000000000", uniqueId.ToString());
            var xmlReader = XmlReader.Create(new StringReader(messageText));

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Soap11WSAddressing10, methodName, xmlReader);

            // Because this is the outgoing reply, set RelatesTo
            expectedMessage.Headers.RelatesTo = uniqueId;

            // Act
            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in test project App.config is true, then the value should be logged to the DB
                // However, since we are using TPL (Task Parallel Library) Task.Factory.StartNew, the below call will finish before the item is
                // actually logged to the DB
                DebugMessageDispatcherForTests.BeforeSendReply(ref expectedMessage, myCorrelationState);
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
            const string methodName = "DebugMessageDispatcher_StartLoggingTheReply_Success";
            var uniqueId = new UniqueId(Urn);
            var messageText = File.ReadAllText(DispatcherSampleReplyFullPath).Replace("urn:uuid:00000000-0000-0000-0000-000000000000", uniqueId.ToString());
            var xmlReader = XmlReader.Create(new StringReader(messageText));

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Soap11WSAddressing10, methodName, xmlReader);

            // Because this is the outgoing reply, set RelatesTo
            expectedMessage.Headers.RelatesTo = uniqueId;

            // Act
            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in test project App.config is true, then the value should be logged to the DB
                // However, since we are using TPL (Task Parallel Library) Task.Factory.StartNew, the below call will finish before the item is
                // actually logged to the DB
                DebugMessageDispatcherForTests.StartLoggingTheReply(expectedMessage);
            }
            catch (Exception ex)
            {
                Assert.Fail("{0}", ex);
            }
        }
    }
}
