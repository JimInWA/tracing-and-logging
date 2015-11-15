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
    public class DebugMessageDispatcherTest : UnitTestBaseClass
    {
        private const string _dispatcherSampleRequest = @"TestData\DebugMessageDispatcher_01_SampleRequest.txt";
        private const string _dispatcherSampleReply = @"TestData\DebugMessageDispatcher_02_SampleReply.txt";

        private string _dispatcherSampleRequestFullPath;

        /// <summary>
        /// DispatcherSampleRequestFullPath - get the full path of the request
        /// </summary>
        public string DispatcherSampleRequestFullPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_dispatcherSampleRequestFullPath))
                {
                    // Get the current assembly location from the public property in the base class
                    _dispatcherSampleRequestFullPath = Path.Combine(AssemblyDirectory.Replace(" ", "%"), _dispatcherSampleRequest);

                    if (!File.Exists(_dispatcherSampleRequestFullPath))
                    {
                        Assert.Fail("File location \"{0}\" does not exist");
                    }
                }

                return _dispatcherSampleRequestFullPath;
            }
        }

        private string _dispatcherSampleReplyFullPath;

        /// <summary>
        /// DispatcherSampleReplyFullPath - get the full path of the reply
        /// </summary>
        public string DispatcherSampleReplyFullPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_dispatcherSampleReplyFullPath))
                {
                    // Get the current assembly location from the public property in the base class
                    _dispatcherSampleReplyFullPath = Path.Combine(AssemblyDirectory.Replace(" ", "%"), _dispatcherSampleReply);

                    if (!File.Exists(_dispatcherSampleReplyFullPath))
                    {
                        Assert.Fail("File location \"{0}\" does not exist");
                    }
                }

                return _dispatcherSampleReplyFullPath;
            }
        }

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

            // ToDo: get the assembly path and then read the file from the TestData folder
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

            // ToDo: get the assembly path and then read the file from the TestData folder
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

            // ToDo: get the assembly path and then read the file from the TestData folder
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

            // ToDo: get the assembly path and then read the file from the TestData folder
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
