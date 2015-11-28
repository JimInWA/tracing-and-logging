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
        #region Test attributes

        /// <summary>
        /// DebugMessageInspectorForTests is the DebugMessageInspector used for the tests
        /// </summary>
        public DebugMessageInspector DebugMessageInspectorForTests;

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
            DebugMessageInspectorForTests = new DebugMessageInspector(myHelper, myLogger);
            TestHelperForTests = new TestHelper();
        }
        #endregion

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
            const string methodName = "DebugMessageInspector_BeforeSendRequest_Success";
            var uniqueId = new UniqueId(Urn);
            var messageTextJustInnerXmlOfBody = File.ReadAllText(InspectorSampleRequestJustInnerXmlOfBodyFullPath);
            var messageTextFull = File.ReadAllText(InspectorSampleRequestFullPath).Replace("Method_Name", methodName).Replace("urn:uuid:00000000-0000-0000-0000-000000000000", uniqueId.ToString());
            var xmlReader = XmlReader.Create(new StringReader(messageTextJustInnerXmlOfBody));

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Default, methodName, xmlReader);

            // Because this is the outgoing request, set MessageId
            expectedMessage.Headers.MessageId = uniqueId;

            // Act
            IClientChannel channel = null;
            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in App.config is true, then the value should be logged to the DB
                var actualMessage = DebugMessageInspectorForTests.BeforeSendRequest(ref expectedMessage, channel);
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
        /// DebugMessageInspector_StartLoggingTheRequest_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageInspector")]
        public void DebugMessageInspector_StartLoggingTheRequest_Success()
        {
            // Arrange
            const string methodName = "DebugMessageInspector_StartLoggingTheRequest_Success";
            var uniqueId = new UniqueId(Urn);
            var messageTextJustInnerXmlOfBody = File.ReadAllText(InspectorSampleRequestJustInnerXmlOfBodyFullPath);
            var messageTextFull = File.ReadAllText(InspectorSampleRequestFullPath).Replace("Method_Name", methodName).Replace("urn:uuid:00000000-0000-0000-0000-000000000000", uniqueId.ToString());
            var xmlReader = XmlReader.Create(new StringReader(messageTextJustInnerXmlOfBody));

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Default, methodName, xmlReader);

            // Because this is the outgoing request, set MessageId
            expectedMessage.Headers.MessageId = uniqueId;

            // Act
            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in App.config is true, then the value should be logged to the DB
                DebugMessageInspectorForTests.StartLoggingTheRequest(expectedMessage);
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
        /// DebugMessageInspector_AfterReceiveReply_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("DebugMessageInspector")]
        public void DebugMessageInspector_AfterReceiveReply_Success()
        {
            // Arrange
            const string methodName = "DebugMessageInspector_AfterReceiveReply_Success";
            var uniqueId = new UniqueId(Urn);
            var messageTextJustInnerXmlOfBody = File.ReadAllText(InspectorSampleReplyJustInnerXmlOfBodyFullPath);
            var messageTextFull = File.ReadAllText(InspectorSampleReplyFullPath).Replace("Method_Name", methodName).Replace("urn:uuid:00000000-0000-0000-0000-000000000000", uniqueId.ToString());
            var xmlReader = XmlReader.Create(new StringReader(messageTextJustInnerXmlOfBody));

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Default, methodName, xmlReader);

            // Because this is the incoming reply, set RelatesTo
            expectedMessage.Headers.RelatesTo = uniqueId;

            // Act
            object myCorrelationState = null;
            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in App.config is true, then the value should be logged to the DB
                DebugMessageInspectorForTests.AfterReceiveReply(ref expectedMessage, myCorrelationState);
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
            const string methodName = "DebugMessageInspector_StartLoggingTheReply_Success";
            var uniqueId = new UniqueId(Urn);
            var messageTextJustInnerXmlOfBody = File.ReadAllText(InspectorSampleReplyJustInnerXmlOfBodyFullPath);
            var messageTextFull = File.ReadAllText(InspectorSampleReplyFullPath).Replace("Method_Name", methodName).Replace("urn:uuid:00000000-0000-0000-0000-000000000000", uniqueId.ToString());
            var xmlReader = XmlReader.Create(new StringReader(messageTextJustInnerXmlOfBody));

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Default, methodName, xmlReader);

            // Because this is the incoming reply, set RelatesTo
            expectedMessage.Headers.RelatesTo = uniqueId;

            // Act
            try
            {
                // Note: If the SoapRequestsAndResponsesShouldLog in App.config is true, then the value should be logged to the DB
                DebugMessageInspectorForTests.StartLoggingTheReply(expectedMessage);
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
