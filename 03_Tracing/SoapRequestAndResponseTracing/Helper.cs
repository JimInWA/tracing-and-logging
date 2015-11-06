namespace SoapRequestAndResponseTracing
{
    using SoapRequestAndResponseTracing.Interfaces;
    using System.Configuration;

    /// <summary>
    /// Helper class - implements IHelper interface
    /// General utility methods to reduce code duplication
    /// </summary>
    public class Helper : IHelper
    {
        /// <summary>
        /// ShouldLogSoapRequestsAndResponses method - reads the config to determine if we should log or not
        /// </summary>
        /// <returns></returns>
        public bool ShouldLogSoapRequestsAndResponses()
        {
            var shouldLogSoapRequestsAndResponses = false;

            // ToDo: Get rid of the magic strings
            var configSetting = ConfigurationManager.AppSettings["ShouldLogSoapRequestsAndResponses"];
            if (!string.IsNullOrWhiteSpace(configSetting))
            {
                bool.TryParse(configSetting, out shouldLogSoapRequestsAndResponses);
            }

            return shouldLogSoapRequestsAndResponses;
        }

        /// <summary>
        /// StripFormattingFromHeaderMessageId method - strips the colons and dashes from the Header's MesssageId 
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public string StripFormattingFromHeaderMessageId(string messageId)
        {
            var result = StripFormattingFromHeaderFields(messageId);
            return result;
        }

        /// <summary>
        /// StripFormattingFromHeaderRelatesTo method - strips the colons and dashes from the Header's RelatesTo
        /// </summary>
        /// <param name="relatesTo"></param>
        /// <returns></returns>
        public string StripFormattingFromHeaderRelatesTo(string relatesTo)
        {
            var result = StripFormattingFromHeaderFields(relatesTo);
            return result;
        }

        private string StripFormattingFromHeaderFields(string headerField)
        {
            var result = headerField.Replace(":", " ").Replace("-", "");
            return result;
        }
    }
}
