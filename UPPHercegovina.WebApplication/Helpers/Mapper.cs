using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UPPHercegovina.WebApplication.Models;
using UPPHercegovina.WebApplication.Extensions;
using UPPHercegovina.WebApplication.Abstractions;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;
using System.Text;

namespace UPPHercegovina.WebApplication.Helpers
{
    public class Mapper
    {
        internal static T MapTo<T, V>(V item) 
        {
            var serializer = new DataContractSerializer(typeof(V));

            var sb = new StringBuilder();
            var textWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(textWriter);
            serializer.WriteObject(xmlWriter, item);

            var deserializer = new DataContractSerializer(typeof(T));

            T result = (T)deserializer.ReadObject(new XmlTextReader(new StringReader(sb.ToString())));

            return result;
        }


    }
}