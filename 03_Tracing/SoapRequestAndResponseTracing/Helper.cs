namespace SoapRequestAndResponseTracing
{
    using SoapRequestAndResponseTracing.Interfaces;
    using System;
    using System.Configuration;
    using System.Xml;

    /// <summary>
    /// Helper class - implements IHelper interface
    /// General utility methods to reduce code duplication
    /// </summary>
    public class Helper : IHelper
    {
        private const string _appSettingSoapRequestsAndResponsesShouldLog = "SoapRequestsAndResponsesShouldLog";

        /// <summary>
        /// ShouldLogSoapRequestsAndResponses method - reads the config to determine if we should log or not
        /// </summary>
        /// <returns></returns>
        public bool ShouldLogSoapRequestsAndResponses()
        {
            var shouldLogSoapRequestsAndResponses = false;

            var configSetting = ConfigurationManager.AppSettings[_appSettingSoapRequestsAndResponsesShouldLog];
            if (!string.IsNullOrWhiteSpace(configSetting))
            {
                bool.TryParse(configSetting, out shouldLogSoapRequestsAndResponses);
            }

            return shouldLogSoapRequestsAndResponses;
        }

        /// <summary>
        /// ProvideUrnFromHeaderMessageId method - gives the URN from the Header.MessageId;
        /// if not valid, returns an empty GUID (all zeroes)
        /// </summary>
        /// <returns></returns>
        public Guid ProvideUrnFromHeaderMessageId(UniqueId messageId)
        {
            var urn = ProvideUrnFromHeaderFields(messageId);

            return urn;
        }

        /// <summary>
        /// ProvideUrnFromHeaderRelatesTo method - gives the URN from the Header.RelatesTo;
        /// if not valid, returns an empty GUID (all zeroes)
        /// </summary>
        /// <returns></returns>
        public Guid ProvideUrnFromHeaderRelatesTo(UniqueId relatesTo)
        {
            var urn = ProvideUrnFromHeaderFields(relatesTo);

            return urn;
        }

        private Guid ProvideUrnFromHeaderFields(UniqueId headerField)
        {
            var urn = new Guid();

            if (headerField != null)
            {
                var possibleUrnAsString = StripFormattingFromHeaderFields(headerField.ToString());
                Guid.TryParseExact(possibleUrnAsString, "D", out urn);
            }
            return urn;            
        }

        private string StripFormattingFromHeaderFields(string headerField)
        {
            var result = headerField.Replace(":", " ").Replace("urn uuid ", "");
            return result;
        }
    }
}
