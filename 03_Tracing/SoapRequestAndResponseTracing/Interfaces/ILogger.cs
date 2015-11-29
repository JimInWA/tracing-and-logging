namespace SoapRequestAndResponseTracing.Interfaces
{
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using System;
    using System.ServiceModel.Channels;

    /// <summary>
    /// ILogger interface
    /// Methods for logging the request and response
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Write a new log entry to the default category (uses Microsoft.Practices.EnterpriseLibrary.Logging)
        /// </summary>
        /// <param name="message">Message body to log. Value from ToString() method from message object</param>
        void Write(object message);
        
        /// <summary>
        /// Log method stub - orchestrates the logging of the request or response (using inline SQL statements)
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="stepName"></param>
        /// <param name="urnUuid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Log(string sourceType, string stepName, Guid urnUuid, Message message);
    }
}
