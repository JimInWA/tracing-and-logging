namespace SoapRequestAndResponseTracing
{
    using SoapRequestAndResponseTracing.Interfaces;
    using System.Configuration;

    public class Helper : IHelper
    {
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

    }
}
