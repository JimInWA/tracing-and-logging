namespace SoapRequestAndResponseTracing.Test.TestCases
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO;
    using System.ServiceModel;
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
            var messageText = File.ReadAllText(DispatcherSampleReplyFullPath);

            // Create the Message
            var expectedMessage = Message.CreateMessage(MessageVersion.Default, "*", XmlTextReader.Create(new StringReader(messageText)));

            // ToDo: introduce FakeItEasy so we convert this into a unit test
            // ToDo: Autofac with FakeItEasy

            // Act
            const string sourceType = "WCF Server Side";
            const string incomingRequestText = "incoming request";
            var urn = Guid.NewGuid();
            var myLogger = new Logger();
            var result = false;

            try
            {
                result = myLogger.Log(sourceType, incomingRequestText, urn, expectedMessage);
            }
            catch (Exception ex)
            {
                Assert.Fail("{0}", ex);
            }
        }
    }
}
