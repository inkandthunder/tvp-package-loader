using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Threading.Tasks;

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
                    ReadXml(xmlfile);
                }
                else
                {
                    log.Debug("Invalid filetype. Please pass an XML file");
                }
                Console.ReadKey();
            }

        }

        static void ReadXml(string xml)
        {

        }
    }
}
