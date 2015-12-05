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

        /// <summary>
        /// TestHelperForTests is the TestHelper used for the tests
        /// </summary>
        public TestHelper TestHelperForTests;

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
            TestHelperForTests = new TestHelper();            
        }
        #endregion

        
        /// <summary>
        /// DebugMessageDispatcher_AfterReceiveRequest_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageDispatcher")]
        [DeploymentItem(@"TestData\DebugMessageDispatcher_01_SampleRequest_JustInnerXmlOfBody.txt", "TestData")]
        [DeploymentItem(@"TestData\DebugMessageDispatcher_01_SampleRequest.txt", "TestData")]
        public void DebugMessageDispatcher_AfterReceiveRequest_Success()
        {
            // Arrange
            const string methodName = "DebugMessageDispatcher_AfterReceiveRequest_Success";
            var uniqueId = new UniqueId(Urn);
            IClientChannel channel = null;
            InstanceContext context = null;
            var messageTextJustInnerXmlOfBody = File.ReadAllText(DispatcherSampleRequestJustInnerXmlOfBodyFullPath);
            var messageTextFull = File.ReadAllText(DispatcherSampleRequestFullPath).Replace("Method_Name", methodName).Replace("urn:uuid:00000000-0000-0000-0000-000000000000", uniqueId.ToString());
            var xmlReader = XmlReader.Create(new StringReader(messageTextJustInnerXmlOfBody));

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

            // if you want to inspect the value logged in the table, stop the test at this point

            var applicationName = TestHelperForTests.GetAppSettingsKey("SoapRequestsAndResponsesApplicationName");
            const bool isRequest = true;
            const bool isResponse = false;
            var sqlSelectStatement = TestHelperForTests.BuildSqlSelectStatement(applicationName, isRequest, isResponse, Urn, methodName, messageTextFull);
            const int expectedRowCount = 1;
            var rowIdValue = TestHelperForTests.ExecuteSqlSelectStatement(sqlSelectStatement, expectedRowCount);
            var sqlDeleteStatement = TestHelperForTests.BuildSqlDeleteStatement(rowIdValue);
            TestHelperForTests.ExecuteSqlDeleteStatement(sqlDeleteStatement, expectedRowCount);
        }

        /// <summary>
        /// DebugMessageDispatcher_StartLoggingTheRequest_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageDispatcher")]
        [DeploymentItem(@"TestData\DebugMessageDispatcher_01_SampleRequest_JustInnerXmlOfBody.txt", "TestData")]
        [DeploymentItem(@"TestData\DebugMessageDispatcher_01_SampleRequest.txt", "TestData")]
        public void DebugMessageDispatcher_StartLoggingTheRequest_Success()
        {
            // Arrange
            const string methodName = "DebugMessageDispatcher_StartLoggingTheRequest_Success";
            var uniqueId = new UniqueId(Urn);
            var messageTextJustInnerXmlOfBody = File.ReadAllText(DispatcherSampleRequestJustInnerXmlOfBodyFullPath);
            var messageTextFull = File.ReadAllText(DispatcherSampleRequestFullPath).Replace("Method_Name", methodName).Replace("urn:uuid:00000000-0000-0000-0000-000000000000", uniqueId.ToString());
            var xmlReader = XmlReader.Create(new StringReader(messageTextJustInnerXmlOfBody));

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

            // if you want to inspect the value logged in the table, stop the test at this point

            var applicationName = TestHelperForTests.GetAppSettingsKey("SoapRequestsAndResponsesApplicationName");
            const bool isRequest = true;
            const bool isResponse = false;
            var sqlSelectStatement = TestHelperForTests.BuildSqlSelectStatement(applicationName, isRequest, isResponse, Urn, methodName, messageTextFull);
            const int expectedRowCount = 1;
            var rowIdValue = TestHelperForTests.ExecuteSqlSelectStatement(sqlSelectStatement, expectedRowCount);
            var sqlDeleteStatement = TestHelperForTests.BuildSqlDeleteStatement(rowIdValue);
            TestHelperForTests.ExecuteSqlDeleteStatement(sqlDeleteStatement, expectedRowCount);
        }

        /// <summary>
        /// DebugMessageDispatcher_BeforeSendReply_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageDispatcher")]
        [DeploymentItem(@"TestData\DebugMessageDispatcher_02_SampleReply_JustInnerXmlOfBody.txt", "TestData")]
        [DeploymentItem(@"TestData\DebugMessageDispatcher_02_SampleReply.txt", "TestData")]
        public void DebugMessageDispatcher_BeforeSendReply_Success()
        {
            // Arrange
            const string methodName = "DebugMessageDispatcher_BeforeSendReply_Success";
            var uniqueId = new UniqueId(Urn);
            object myCorrelationState = null;
            var messageTextJustInnerXmlOfBody = File.ReadAllText(DispatcherSampleReplyJustInnerXmlOfBodyFullPath);
            var messageTextFull = File.ReadAllText(DispatcherSampleReplyFullPath).Replace("Method_Name", methodName).Replace("urn:uuid:00000000-0000-0000-0000-000000000000", uniqueId.ToString());
            var xmlReader = XmlReader.Create(new StringReader(messageTextJustInnerXmlOfBody));

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

            // if you want to inspect the value logged in the table, stop the test at this point

            var applicationName = TestHelperForTests.GetAppSettingsKey("SoapRequestsAndResponsesApplicationName");
            const bool isRequest = false;
            const bool isResponse = true;
            var sqlSelectStatement = TestHelperForTests.BuildSqlSelectStatement(applicationName, isRequest, isResponse, Urn, methodName, messageTextFull);
            const int expectedRowCount = 1;
            var rowIdValue = TestHelperForTests.ExecuteSqlSelectStatement(sqlSelectStatement, expectedRowCount);
            var sqlDeleteStatement = TestHelperForTests.BuildSqlDeleteStatement(rowIdValue);
            TestHelperForTests.ExecuteSqlDeleteStatement(sqlDeleteStatement, expectedRowCount);
        }

        /// <summary>
        /// DebugMessageDispatcher_StartLoggingTheReply_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageDispatcher")]
        [DeploymentItem(@"TestData\DebugMessageDispatcher_02_SampleReply_JustInnerXmlOfBody.txt", "TestData")]
        [DeploymentItem(@"TestData\DebugMessageDispatcher_02_SampleReply.txt", "TestData")]
        public void DebugMessageDispatcher_StartLoggingTheReply_Success()
        {
            // Arrange
            const string methodName = "DebugMessageDispatcher_StartLoggingTheReply_Success";
            var uniqueId = new UniqueId(Urn);
            var messageTextJustInnerXmlOfBody = File.ReadAllText(DispatcherSampleReplyJustInnerXmlOfBodyFullPath);
            var messageTextFull = File.ReadAllText(DispatcherSampleReplyFullPath).Replace("Method_Name", methodName).Replace("urn:uuid:00000000-0000-0000-0000-000000000000", uniqueId.ToString());
            var xmlReader = XmlReader.Create(new StringReader(messageTextJustInnerXmlOfBody));

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

            // if you want to inspect the value logged in the table, stop the test at this point

            var applicationName = TestHelperForTests.GetAppSettingsKey("SoapRequestsAndResponsesApplicationName");
            const bool isRequest = false;
            const bool isResponse = true;
            var sqlSelectStatement = TestHelperForTests.BuildSqlSelectStatement(applicationName, isRequest, isResponse, Urn, methodName, messageTextFull);
            const int expectedRowCount = 1;
            var rowIdValue = TestHelperForTests.ExecuteSqlSelectStatement(sqlSelectStatement, expectedRowCount);
            var sqlDeleteStatement = TestHelperForTests.BuildSqlDeleteStatement(rowIdValue);
            TestHelperForTests.ExecuteSqlDeleteStatement(sqlDeleteStatement, expectedRowCount);
        }
    }
}
