namespace SoapRequestAndResponseTracing.Test.Framework
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// UnitTestBaseClass 
    /// </summary>
    public class UnitTestBaseClass
    {
        /// <summary>
        /// AssemblyDirectory - return the assembly directory
        /// </summary>
        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private const string _dispatcherSampleRequest = @"TestData\DebugMessageDispatcher_01_SampleRequest.txt";
        private const string _dispatcherSampleRequestJustInnerXmlOfBody = @"TestData\DebugMessageDispatcher_01_SampleRequest_JustInnerXmlOfBody.txt";

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
                        Assert.Fail("File location \"{0}\" does not exist", _dispatcherSampleRequestFullPath);
                    }
                }

                return _dispatcherSampleRequestFullPath;
            }
        }

        private string _dispatcherSampleRequestJustInnerXmlOfBodyFullPath;

        /// <summary>
        /// DispatcherSampleRequestJustInnerXmlOfBodyFullPath - get the full path of the request
        /// </summary>
        public string DispatcherSampleRequestJustInnerXmlOfBodyFullPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_dispatcherSampleRequestJustInnerXmlOfBodyFullPath))
                {
                    // Get the current assembly location from the public property in the base class
                    _dispatcherSampleRequestJustInnerXmlOfBodyFullPath = Path.Combine(AssemblyDirectory.Replace(" ", "%"), _dispatcherSampleRequestJustInnerXmlOfBody);

                    if (!File.Exists(_dispatcherSampleRequestJustInnerXmlOfBodyFullPath))
                    {
                        Assert.Fail("File location \"{0}\" does not exist", _dispatcherSampleRequestJustInnerXmlOfBodyFullPath);
                    }
                }

                return _dispatcherSampleRequestJustInnerXmlOfBodyFullPath;
            }
        }

        private const string _dispatcherSampleReply = @"TestData\DebugMessageDispatcher_02_SampleReply.txt";

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
                        Assert.Fail("File location \"{0}\" does not exist", _dispatcherSampleReplyFullPath);
                    }
                }

                return _dispatcherSampleReplyFullPath;
            }
        }

        private const string _dispatcherSampleReplyJustInnerXmlOfBody = @"TestData\DebugMessageDispatcher_02_SampleReply_JustInnerXmlOfBody.txt";

        private string _dispatcherSampleReplyJustInnerXmlOfBodyFullPath;

        /// <summary>
        /// DispatcherSampleReplyJustInnerXmlOfBodyFullPath - get the full path of the reply
        /// </summary>
        public string DispatcherSampleReplyJustInnerXmlOfBodyFullPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_dispatcherSampleReplyJustInnerXmlOfBodyFullPath))
                {
                    // Get the current assembly location from the public property in the base class
                    _dispatcherSampleReplyJustInnerXmlOfBodyFullPath = Path.Combine(AssemblyDirectory.Replace(" ", "%"), _dispatcherSampleReplyJustInnerXmlOfBody);

                    if (!File.Exists(_dispatcherSampleReplyJustInnerXmlOfBodyFullPath))
                    {
                        Assert.Fail("File location \"{0}\" does not exist", _dispatcherSampleReplyJustInnerXmlOfBodyFullPath);
                    }
                }

                return _dispatcherSampleReplyJustInnerXmlOfBodyFullPath;
            }
        }

        private const string _inspectorSampleRequest = @"TestData\DebugMessageInspector_01_SampleRequest.txt";

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
                        Assert.Fail("File location \"{0}\" does not exist", _inspectorSampleRequestFullPath);
                    }
                }

                return _inspectorSampleRequestFullPath;
            }
        }

        private const string _inspectorSampleRequestJustInnerXmlOfBody = @"TestData\DebugMessageInspector_01_SampleRequest_JustInnerXmlOfBody.txt";

        private string _inspectorSampleRequestJustInnerXmlOfBodyFullPath;

        /// <summary>
        /// InspectorSampleRequestFullPath - get the full path of the request
        /// </summary>
        public string InspectorSampleRequestJustInnerXmlOfBodyFullPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_inspectorSampleRequestJustInnerXmlOfBodyFullPath))
                {
                    // Get the current assembly location from the public property in the base class
                    _inspectorSampleRequestJustInnerXmlOfBodyFullPath = Path.Combine(AssemblyDirectory.Replace(" ", "%"), _inspectorSampleRequestJustInnerXmlOfBody);

                    if (!File.Exists(_inspectorSampleRequestJustInnerXmlOfBodyFullPath))
                    {
                        Assert.Fail("File location \"{0}\" does not exist", _inspectorSampleRequestJustInnerXmlOfBodyFullPath);
                    }
                }

                return _inspectorSampleRequestJustInnerXmlOfBodyFullPath;
            }
        }

        private const string _inspectorSampleReply = @"TestData\DebugMessageInspector_02_SampleReply.txt";

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
                        Assert.Fail("File location \"{0}\" does not exist", _inspectorSampleReplyFullPath);
                    }
                }

                return _inspectorSampleReplyFullPath;
            }
        }

        private const string _inspectorSampleReplyJustInnerXmlOfBody = @"TestData\DebugMessageInspector_02_SampleReply_JustInnerXmlOfBody.txt";

        private string _inspectorSampleReplyJustInnerXmlOfBodyFullPath;

        /// <summary>
        /// InspectorSampleReplyFullPath - get the full path of the reply
        /// </summary>
        public string InspectorSampleReplyJustInnerXmlOfBodyFullPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_inspectorSampleReplyJustInnerXmlOfBodyFullPath))
                {
                    // Get the current assembly location from the public property in the base class
                    _inspectorSampleReplyJustInnerXmlOfBodyFullPath = Path.Combine(AssemblyDirectory.Replace(" ", "%"), _inspectorSampleReplyJustInnerXmlOfBody);

                    if (!File.Exists(_inspectorSampleReplyJustInnerXmlOfBodyFullPath))
                    {
                        Assert.Fail("File location \"{0}\" does not exist", _inspectorSampleReplyJustInnerXmlOfBodyFullPath);
                    }
                }

                return _inspectorSampleReplyJustInnerXmlOfBodyFullPath;
            }
        }

        private const string _loggerSampleRequest = @"TestData\Logger_01_SampleRequest.txt";

        private string _loggerSampleRequestFullPath;

        /// <summary>
        /// LoggerSampleRequestFullPath - get the full path of the request
        /// </summary>
        public string LoggerSampleRequestFullPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_loggerSampleRequestFullPath))
                {
                    // Get the current assembly location from the public property in the base class
                    _loggerSampleRequestFullPath = Path.Combine(AssemblyDirectory.Replace(" ", "%"), _loggerSampleRequest);

                    if (!File.Exists(_loggerSampleRequestFullPath))
                    {
                        Assert.Fail("File location \"{0}\" does not exist", _loggerSampleRequestFullPath);
                    }
                }

                return _loggerSampleRequestFullPath;
            }
        }

        private const string _loggerSampleRequestJustInnerXmlOfBody = @"TestData\Logger_01_SampleRequest_JustInnerXmlOfBody.txt";

        private string _loggerSampleRequestJustInnerXmlOfBodyFullPath;

        /// <summary>
        /// LoggerSampleRequestJustInnerXmlOfBodyFullPath - get the full path of the request
        /// </summary>
        public string LoggerSampleRequestJustInnerXmlOfBodyFullPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_loggerSampleRequestJustInnerXmlOfBodyFullPath))
                {
                    // Get the current assembly location from the public property in the base class
                    _loggerSampleRequestJustInnerXmlOfBodyFullPath = Path.Combine(AssemblyDirectory.Replace(" ", "%"), _loggerSampleRequestJustInnerXmlOfBody);

                    if (!File.Exists(_loggerSampleRequestJustInnerXmlOfBodyFullPath))
                    {
                        Assert.Fail("File location \"{0}\" does not exist", _loggerSampleRequestJustInnerXmlOfBody);
                    }
                }

                return _loggerSampleRequestJustInnerXmlOfBodyFullPath;
            }
        }
    }
}
