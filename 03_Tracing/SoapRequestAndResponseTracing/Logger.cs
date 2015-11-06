namespace SoapRequestAndResponseTracing
{
    using SoapRequestAndResponseTracing.Interfaces;
    using System;
    using System.IO;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Xml;

    public class Logger : ILogger
    {
        public Message Log(string sourceType, string stepName, Message message)
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

            var myMessageVersion = message.Version;

            // read the contents of the message here and update as required; eg sign the message

            // Log the contents of xmlDoc
            LogToFile(sourceType, stepName, uri, myXmlDocument);

            // as the message is forward reading then we need to recreate it before moving on 
            message = RebuildMessageFromXmlDocument(myXmlDocument, myMessageVersion);

            return message;
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

        private void LogToFile(string sourceType, string stepName, string uri, XmlDocument xmlDocument)
        {
            // ToDo: Get rid of the magic strings
            var folderName = @"c:\Temp";
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
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

            using (System.IO.StreamWriter fileAppender = File.AppendText(pathString))
            {
                var headerLine = new StringBuilder();
                headerLine.AppendFormat("(UTC) {0} {1} - {2} - {3}", DateTime.UtcNow.ToLongDateString(), DateTime.UtcNow.ToLongTimeString(), sourceType, stepName);
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

        private Message RebuildMessageFromXmlDocument(XmlDocument xmlDocument, MessageVersion myMessageVersion)
        {
            // as the message is forward reading then we need to recreate it before moving on 
            var myMemoryStream = new MemoryStream();
            var myXmlWriter = XmlWriter.Create(myMemoryStream);
            myMemoryStream = new MemoryStream();
            xmlDocument.Save(myMemoryStream);
            myMemoryStream.Position = 0;
            var reader = XmlReader.Create(myMemoryStream);
            var message = Message.CreateMessage(reader, int.MaxValue, myMessageVersion);
            message.Properties.CopyProperties(message.Properties);

            return message;
        }
    }
}
