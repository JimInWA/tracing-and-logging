namespace SoapRequestAndResponseTracing.Test.TestCases
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
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
        }
        #endregion

        /// <summary>
        /// Logger_Log_Success
        /// </summary>
        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("HappyPath")]
        [TestCategory("Logger")]
        public void Logger_Log_Success()
        {
            // Arrange
            const string methodName = "Logger_Log_Success";
            var uniqueId = new UniqueId(Urn);
            var messageText = File.ReadAllText(DispatcherSampleRequestFullPathJustInnerXmlOfBody);
            var xmlReader = XmlReader.Create(new StringReader(messageText));

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

            var applicationName = "Tests";
            var isRequest = true;
            var isResponse = false;
            var sql = BuildSqlStatement(applicationName, isRequest, isResponse, Urn, methodName, messageText);

        }

        private string BuildSqlStatement(string applicationName, bool isRequest, bool isResponse, Guid urn, string methodName, string messageText)
        {
            var isRequestBit = (isRequest) ? 1 : 0;
            var isResponseBit = (isResponse) ? 1 : 0;

            var soap = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><s:Envelope xmlns:a=\"http://www.w3.org/2005/08/addressing\" xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Header><a:Action s:mustUnderstand=\"1\">{0}</a:Action><a:MessageID>urn:uuid:{1}</a:MessageID></s:Header><s:Body>{2}</s:Body></s:Envelope>", methodName, urn, messageText);

            var sql = string.Format("select * from dbo.SoapRequestAndResponseTracingBase where ApplicationName = '{0}' and IsRequest = {1} and IsReply = {2} and URN_UUID = '{3}' and URL = '{4}' and SoapRequestOrResponseXml = '{5}'", applicationName, isRequestBit, isResponseBit, urn, methodName, soap);
            return sql;
        }
    }
}
