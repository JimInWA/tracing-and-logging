namespace SoapRequestAndResponseTracing.Interfaces
{
    using System;
    using System.ServiceModel.Channels;

    /// <summary>
    /// IMessageLogging interface
    /// Methods exposed for external callers to start the process of logging the request and response
    /// </summary>
    public interface IMessageLogging
    {
        /// <summary>
        /// StartLoggingTheRequest method stub - starts the process of logging the request
        /// </summary>
        /// <param name="requestCopyForLogging"></param>
        /// <returns></returns>
        bool StartLoggingTheRequest(Message requestCopyForLogging);

        /// <summary>
        /// StartLoggingTheReply method stub - starts the process of logging the reply
        /// </summary>
        /// <param name="replyCopyForLogging"></param>
        /// <returns></returns>
        bool StartLoggingTheReply(Message replyCopyForLogging);

    }
}
