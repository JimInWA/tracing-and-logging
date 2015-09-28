using System.IO;
using System.ServiceModel.Channels;
using System.Xml;

namespace SoapRequestAndResponseTracing
{
    public class MessageReader
    {
        public void Read(MessageBuffer messageBuffer)
        {
            var memoryStream = new MemoryStream();
            messageBuffer.WriteMessage(memoryStream);
            memoryStream.Position = 0;
            var xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(memoryStream);
        }
    }
}
