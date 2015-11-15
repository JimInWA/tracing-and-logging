namespace SoapRequestAndResponseTracing.Test.Framework
{
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
    }
}
