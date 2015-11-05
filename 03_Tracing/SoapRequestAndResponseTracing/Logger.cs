namespace SoapRequestAndResponseTracing
{
    using SoapRequestAndResponseTracing.Interfaces;
    using System.IO;
    using System.ServiceModel.Channels;
    using System.Xml;

    public class Logger : ILogger
    {
        public Message Log(Message message)
        {
            // read the message into an XmlDocument as then you can work with the contents 
            // Message is a forward reading class only so once read that's it. 
            var ms = new MemoryStream();
            var writer = XmlWriter.Create(ms);
            message.WriteMessage(writer);
            writer.Flush();
            ms.Position = 0;
            var xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(ms);

            // read the contents of the message here and update as required; eg sign the message

            // ToDo: This is where we would actually log the contents of xmlDoc

            // as the message is forward reading then we need to recreate it before moving on 
            ms = new MemoryStream();
            xmlDoc.Save(ms);
            ms.Position = 0;
            var reader = XmlReader.Create(ms);
            var newMessage = Message.CreateMessage(reader, int.MaxValue, message.Version);
            newMessage.Properties.CopyProperties(message.Properties);
            message = newMessage;
            return message;
        }
    }
}
