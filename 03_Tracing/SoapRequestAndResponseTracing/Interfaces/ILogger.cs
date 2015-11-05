﻿namespace SoapRequestAndResponseTracing.Interfaces
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public interface ILogger
    {
        Message Log(string sourceType, string stepName, Message message);
    }
}
