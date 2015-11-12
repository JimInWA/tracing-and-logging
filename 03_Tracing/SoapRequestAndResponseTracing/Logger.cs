using System.Configuration;

namespace SoapRequestAndResponseTracing
{
    using SoapRequestAndResponseTracing.Interfaces;
    using System;
    using System.IO;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Logger class - implements ILogger interface
    /// Methods for logging the request and response
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        /// Log method - orchestrates the logging of the request or response
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="stepName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public void Log(string sourceType, StringBuilder stepName, Message message)
        {
            var uri = string.Empty;
            if (message.Headers.To != null)
            {
                // Server side, incoming request
                uri = message.Headers.To.AbsoluteUri;
            }
            else if (message.Headers.Action != null)
            {
                // Client side, outgoing request
                uri = message.Headers.Action;
            }
            
            // read the message into an XmlDocument as then you can work with the contents 
            var myXmlDocument = ReadMessageIntoXmlDocument(message);

            // Log the contents of xmlDoc
            LogToFile(sourceType, stepName, uri, myXmlDocument);
        }

        private XmlDocument ReadMessageIntoXmlDocument(Message message)
        {
            // read the message into an XmlDocument as then you can work with the contents 
            // Message is a forward reading class only so once read that's it. 
            var myMemoryStream = new MemoryStream();
            var myXmlWriter = XmlWriter.Create(myMemoryStream);
            message.WriteMessage(myXmlWriter);
            myXmlWriter.Flush();
            myMemoryStream.Position = 0;
            var myXmlDocument = new XmlDocument();
            myXmlDocument.PreserveWhitespace = true;
            myXmlDocument.Load(myMemoryStream);

            return myXmlDocument;
        }

        private void LogToFile(string sourceType, StringBuilder stepName, string uri, XmlDocument xmlDocument)
        {
            try
            {
                // ToDo: Get rid of the magic strings
                var folderName = @"c:\Temp";
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }

                // ToDo: Get rid of the magic strings
                var configSetting = ConfigurationManager.AppSettings["SoapRequestsAndResponsesFolder"];
                if (!string.IsNullOrWhiteSpace(configSetting))
                {
                    folderName = configSetting;
                }

                var fileName = string.Empty;

                // ToDo: Get rid of the magic strings
                switch (sourceType)
                {
                    case "WCF Server Side":
                        fileName = "WCF_Server_Side_Log_File.log";
                        break;

                    case "MVC Client Side":
                        fileName = "MVC_Client_Side_Log_File.log";
                        break;

                    default:
                        fileName = "Unknown_Log_File.log";
                        break;
                }

                var pathString = Path.Combine(folderName, fileName);

                if (!System.IO.File.Exists(pathString))
                {
                    using (var myFile = File.Create(pathString))
                    {
                        // putting the using statement to close the file after creation
                        // this is to avoid the "The process cannot access the file '[my file path here]' because it is being used by another process." error 
                    }

                }

                using (var fileAppender = File.AppendText(pathString))
                {
                    var headerLine = new StringBuilder();
                    headerLine.AppendFormat("(UTC) {0} {1} - {2} - {3}", DateTime.UtcNow.ToLongDateString(),
                        DateTime.UtcNow.ToLongTimeString(), sourceType, stepName);
                    if (!string.IsNullOrWhiteSpace(uri))
                    {
                        headerLine.AppendFormat(" - {0}", uri);
                    }
                    fileAppender.WriteLine("{0}", headerLine);
                    fileAppender.WriteLine("{0}", xmlDocument.InnerXml);
                    fileAppender.WriteLine("");
                    fileAppender.WriteLine("");
                }
            }
            catch
            {
                // empty catch block for right now
            }
        }
    }
}
