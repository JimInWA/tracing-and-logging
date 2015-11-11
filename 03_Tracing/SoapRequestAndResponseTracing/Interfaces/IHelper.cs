using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoapRequestAndResponseTracing.Interfaces
{
    /// <summary>
    /// IHelper interface (internal)
    /// General utility methods to reduce code duplication
    /// </summary>
    internal interface IHelper
    {
        /// <summary>
        /// ShouldLogSoapRequestsAndResponses method - reads the config to determine if we should log or not
        /// </summary>
        /// <returns></returns>
        bool ShouldLogSoapRequestsAndResponses();

        /// <summary>
        /// StripFormattingFromHeaderMessageId method - strips the colons and dashes from the Header's MesssageId 
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        string StripFormattingFromHeaderMessageId(string messageId);

        /// <summary>
        /// StripFormattingFromHeaderRelatesTo method - strips the colons and dashes from the Header's RelatesTo
        /// </summary>
        /// <param name="relatesTo"></param>
        /// <returns></returns>
        string StripFormattingFromHeaderRelatesTo(string relatesTo);
    }
}
