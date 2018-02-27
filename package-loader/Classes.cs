using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace package_loader
{
    [XmlRoot("Job")]
    public class Job
    {
        [XmlElement("Package")]
        public List<Package> Packages { get; set; }
    }

    public class Package
    {
        [XmlElement("NSLID")]
        public string NSLID { get; set; }
        [XmlElement("Sequence")]
        public string Sequence { get; set; }
        [XmlElement("LogicalPage")]
        public string LogicalPage { get; set; }
        [XmlElement("DivertCode")]
        public string DivertCode { get; set; }
        [XmlElement("EnclosureCode")]
        public string EnclosureCode { get; set; }
    }
}
