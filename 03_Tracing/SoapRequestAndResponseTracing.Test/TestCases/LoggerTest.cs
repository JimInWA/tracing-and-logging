namespace SoapRequestAndResponseTracing.Test.TestCases
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using System.ServiceModel.Channels;
    using System.Xml;
    using SoapRequestAndResponseTracing;
    using SoapRequestAndResponseTracing.Test.Framework;

    /// <summary>
    /// Logger tests
    /// </summary>
    [TestClass]
    public class LoggerTest : UnitTestBaseClass
    {
        #region Test attributes

        /// <summary>
        /// LoggerForTests is the Logger used for the tests
        /// </summary>
        public Logger LoggerForTests;

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
            LoggerForTests = new Logger();
            TestHelperForTests = new TestHelper();
        }
        #endregion

        /// <summary>
        /// Logger_Log_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("Logger")]
        [DeploymentItem(@"TestData\Logger_01_SampleRequest_JustInnerXmlOfBody.txt", "TestData")]
        [DeploymentItem(@"TestData\Logger_01_SampleRequest.txt", "TestData")]
        public void Logger_Log_Success()
        {
            // Arrange
            const string methodName = "Logger_Log_Success";
            var uniqueId = new UniqueId(Urn);
            var messageTextJustInnerXmlOfBody = File.ReadAllText(LoggerSampleRequestJustInnerXmlOfBodyFullPath);
            var messageTextFull = File.ReadAllText(LoggerSampleRequestFullPath).Replace("Method_Name", methodName).Replace("urn:uuid:00000000-0000-0000-0000-000000000000", uniqueId.ToString());
            var xmlReader = XmlReader.Create(new StringReader(messageTextJustInnerXmlOfBody));

            const string sourceType = "WCF Server Side";
            const string incomingRequestText = "incoming request";
            var result = false;

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Soap11WSAddressing10, methodName, xmlReader);

            // Because this is the incoming request, set MessageId
            expectedMessage.Headers.MessageId = uniqueId;

            // Act
            try
            {
                result = LoggerForTests.Log(sourceType, incomingRequestText, Urn, expectedMessage);
            }
            catch (Exception ex)
            {
                Assert.Fail("{0}", ex);
            }

            Assert.IsTrue(result, "Log method returned false");

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
    }
}
