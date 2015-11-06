using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoapRequestAndResponseTracing.Interfaces
{
    public interface IHelper
    {
        bool ShouldLogSoapRequestsAndResponses();
    }
}
