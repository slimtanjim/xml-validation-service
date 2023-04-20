using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace xml_valid_service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public bool ValidateXml(string xmlUrl, string xsdUrl)
        {
            // Download XML and XSD files
            string xmlContent = DownloadFile(xmlUrl);
            string xsdContent = DownloadFile(xsdUrl);

            // Load XML and XSD files
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(null, XmlReader.Create(new StringReader(xsdContent)));
            XmlReader xmlReader = XmlReader.Create(new StringReader(xmlContent), settings);

            // Validate XML against XSD
            bool isValid = true;
            try
            {
                while (xmlReader.Read()) { }
            }
            catch (XmlException ex)
            {
                isValid = false;
            }

            return isValid;
        }

        public string QueryXml(string xmlUrl, string pathExpr)
        {
            // Download XML file
            string xmlContent = DownloadFile(xmlUrl);

            // Load XML file
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            // Evaluate path expression and get result
            XmlNodeList resultNodes = xmlDoc.SelectNodes(pathExpr);
            string result = "";
            foreach (XmlNode node in resultNodes)
            {
                result += node.OuterXml + "\n";
            }

            return result;
        }

        private string DownloadFile(string url)
        {
            //refresh string content
            string content = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        content = reader.ReadToEnd();
                    }
                }
            }
            return content;
        }
    }
}
