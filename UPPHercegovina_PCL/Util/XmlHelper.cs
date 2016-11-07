using System.Xml.Linq;
using System.Xml.Serialization;

namespace TV.Util
{
    public class XmlHelper
    {
        internal static T Deserialize<T>(XElement xdoc)
        {
            XmlSerializer xmlser = new XmlSerializer(typeof(T));
            return (T)xmlser.Deserialize(xdoc.CreateReader());
        }
    }
}