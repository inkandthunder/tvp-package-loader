using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.Xml;

namespace package_loader
{

    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                log.Info("No arguments passed. Exiting Application.");
            }
            else
            {
                string xmlfile = args[0];	
                string extension = xmlfile.Substring(xmlfile.LastIndexOf('.') + 1);
                if (extension.ToLower() == "xml")
                {
                    log.Info("Begin processing on " + args[0]);
                    Job Job = DeserializeXMLFileToObject<Job>(xmlfile);
                    for (var i = 0; i < Job.Packages.Count; i++)
                    {
                        Console.WriteLine("Sequence: " + Job.Packages[i].Sequence);
                        Console.WriteLine("LogPage: " + Job.Packages[i].LogicalPage);
                        Console.WriteLine();
                    }

                       // Console.WriteLine(Job.Packages[3].Sequence);
                }
                else
                {
                    log.Debug("Invalid filetype. Please pass an XML file");
                }
                Console.ReadKey();
            }
        }

        public static Job DeserializeXMLFileToObject<Job>(string path)
        {
            Job returnObject = default(Job);
            if (string.IsNullOrEmpty(path)) return default(Job);

            try
            {
                StreamReader xmlStream = new StreamReader(path);
                XmlSerializer serializer = new XmlSerializer(typeof(Job));
                returnObject = (Job)serializer.Deserialize(xmlStream);
            }
            catch (Exception ex)
            {
                log.Error("XML Deserialization failed", ex);
            }
            return returnObject;
        }



    }
}
