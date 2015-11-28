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
            var messageText = File.ReadAllText(DispatcherSampleReplyFullPath).Replace("urn:uuid:00000000-0000-0000-0000-000000000000", string.Format("urn:uuid:{0}",Urn));
            var xmlReader = XmlReader.Create(new StringReader(messageText));

            const string sourceType = "WCF Server Side";
            const string incomingRequestText = "incoming request";
            var result = false;

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Default, methodName, xmlReader);

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

        }
    }
}
