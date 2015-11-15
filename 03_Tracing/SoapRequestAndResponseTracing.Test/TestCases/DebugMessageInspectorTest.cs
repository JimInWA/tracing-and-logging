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
    /// UnitTest1
    /// </summary>
    [TestClass]
    public class DebugMessageInspectorTest : UnitTestBaseClass
    {
        private const string _inspectorSampleRequest = @"TestData\DebugMessageInspector_01_SampleRequest.txt";
        private const string _inspectorSampleReply = @"TestData\DebugMessageInspector_02_SampleReply.txt";

        private string _inspectorSampleRequestFullPath;

        /// <summary>
        /// InspectorSampleRequestFullPath - get the full path of the request
        /// </summary>
        public string InspectorSampleRequestFullPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_inspectorSampleRequestFullPath))
                {
                    // Get the current assembly location from the public property in the base class
                    _inspectorSampleRequestFullPath = Path.Combine(AssemblyDirectory.Replace(" ", "%"), _inspectorSampleRequest);

                    if (!File.Exists(_inspectorSampleRequestFullPath))
                    {
                        Assert.Fail("File location \"{0}\" does not exist");
                    }
                }

                return _inspectorSampleRequestFullPath;
            }
        }

        private string _inspectorSampleReplyFullPath;

        /// <summary>
        /// InspectorSampleReplyFullPath - get the full path of the reply
        /// </summary>
        public string InspectorSampleReplyFullPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_inspectorSampleReplyFullPath))
                {
                    // Get the current assembly location from the public property in the base class
                    _inspectorSampleReplyFullPath = Path.Combine(AssemblyDirectory.Replace(" ", "%"), _inspectorSampleReply);

                    if (!File.Exists(_inspectorSampleReplyFullPath))
                    {
                        Assert.Fail("File location \"{0}\" does not exist");
                    }
                }

                return _inspectorSampleReplyFullPath;
            }
        }

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

            // ToDo: get the assembly path and then read the file from the TestData folder
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

            // ToDo: get the assembly path and then read the file from the TestData folder
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

            // ToDo: get the assembly path and then read the file from the TestData folder
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

            // ToDo: get the assembly path and then read the file from the TestData folder
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
