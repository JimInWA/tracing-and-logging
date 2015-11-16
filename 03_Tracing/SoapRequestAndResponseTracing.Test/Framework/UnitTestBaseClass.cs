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

    }
}
