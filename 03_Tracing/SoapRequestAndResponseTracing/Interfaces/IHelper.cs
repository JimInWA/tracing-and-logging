namespace SoapRequestAndResponseTracing.Interfaces
{
    using System;
    using System.Xml;

    /// <summary>
    /// IHelper interface
    /// General utility methods to reduce code duplication
    /// </summary>
    public interface IHelper
    {
        /// <summary>
        /// ShouldLogSoapRequestsAndResponses method stub - reads the config to determine if we should log or not
        /// </summary>
        /// <returns></returns>
        bool ShouldLogSoapRequestsAndResponses();

        /// <summary>
        /// ProvideUrnFromHeaderMessageId method - gives the URN from the Header.MessageId;
        /// if not valid, returns an empty GUID (all zeroes)
        /// </summary>
        /// <returns></returns>
        Guid ProvideUrnFromHeaderMessageId(UniqueId messageId);

        /// <summary>
        /// ProvideUrnFromHeaderRelatesTo method - gives the URN from the Header.RelatesTo;
        /// if not valid, returns an empty GUID (all zeroes)
        /// </summary>
        /// <returns></returns>
        Guid ProvideUrnFromHeaderRelatesTo(UniqueId relatesTo);
    }
}
