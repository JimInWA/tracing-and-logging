namespace SoapRequestAndResponseTracing
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Xml;
    using SoapRequestAndResponseTracing.Interfaces;


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
        /// <param name="urnUuid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public void Log(string sourceType, string stepName, Guid urnUuid, Message message)
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

            // Log the contents of xmlDoc to the database
            LogToDatabase(sourceType, stepName, urnUuid, uri, myXmlDocument);
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

        private void LogToFile(string sourceType, string stepName, Guid urnUuid, string uri, XmlDocument xmlDocument)
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
                    headerLine.AppendFormat("(UTC) {0} {1} - {2} - {3} - {4}", DateTime.UtcNow.ToLongDateString(),
                        DateTime.UtcNow.ToLongTimeString(), sourceType, stepName, urnUuid);
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

                // ToDo: Log the issue to the event log
            }
        }

        private void LogToDatabase(string sourceType, string stepName, Guid urnUuid, string uri, XmlDocument xmlDocument)
        {
            try
            {
                // ToDo: Get rid of the magic strings
                var applicationName = "Unknown";

                // ToDo: Get rid of the magic strings
                var configSetting = ConfigurationManager.AppSettings["SoapRequestsAndResponsesApplicationName"];
                if (!string.IsNullOrWhiteSpace(configSetting))
                {
                    applicationName = configSetting;
                }

                // ToDo: Get rid of the magic strings
                var connection = ConfigurationManager.ConnectionStrings["SampleLoggingConnectionString"];

                using (var conn = new SqlConnection(connection.ConnectionString))
                {
                    var isRequest = false;
                    var isReply = false;

                    var comparisonText = stepName.ToString().ToLower();

                    if (comparisonText.Contains("request"))
                    {
                        isRequest = true;
                    }
                    else if (comparisonText.Contains("reply"))
                    {
                        isReply = true;
                    }

                    const string insertAndScopeIdentityStatement = "insert into dbo.SoapRequestAndResponseTracingBase (CreatedDateTimeUtc, ApplicationName, IsRequest, IsReply, URN_UUID, URL, SoapRequestOrResponseXml) values (@CreatedDateTimeUtc, @ApplicationName, @IsRequest, @IsReply, @URN_UUID, @URL, @SoapRequestOrResponseXml);SELECT CAST(scope_identity() AS bigint)";

                    var paramCreatedDateTimeUtc = new SqlParameter("CreatedDateTimeUtc", DateTime.UtcNow );
                    var paramApplicationName = new SqlParameter("ApplicationName", applicationName);
                    var paramIsRequest = new SqlParameter("IsRequest", isRequest);
                    var paramIsReply = new SqlParameter("IsReply", isReply);
                    var paramUrnUuid = new SqlParameter("URN_UUID", urnUuid);
                    var paramUrl = new SqlParameter("URL", uri);
                    var paramSoapRequestOrResponseXml = new SqlParameter("SoapRequestOrResponseXml", xmlDocument.InnerXml);

                    var command = new SqlCommand(insertAndScopeIdentityStatement, conn)
                    {
                        CommandType = CommandType.Text
                    };

                    command.Parameters.Add(paramCreatedDateTimeUtc);
                    command.Parameters.Add(paramApplicationName);
                    command.Parameters.Add(paramIsRequest);
                    command.Parameters.Add(paramIsReply);
                    command.Parameters.Add(paramUrnUuid);
                    command.Parameters.Add(paramUrl);
                    command.Parameters.Add(paramSoapRequestOrResponseXml);

                    conn.Open();

                    // ExecuteScalar is returning a value due to the command having a select statement after the insert statement
                    var result = (long)command.ExecuteScalar();
                }            
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                // When an exception happens, log to the file
                LogToFile(sourceType, stepName, urnUuid, uri, xmlDocument);

                // ToDo: Log the issue to the event log
            }
        }
    }
}
