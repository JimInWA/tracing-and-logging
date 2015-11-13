namespace SoapRequestAndResponseTracing.Interfaces
{
    using System;
    using System.ServiceModel.Channels;

    /// <summary>
    /// ILogger interface
    /// Methods for logging the request and response
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log method - orchestrates the logging of the request or response
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="stepName"></param>
        /// <param name="urnUuid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        void Log(string sourceType, string stepName, Guid urnUuid, Message message);
    }
}
