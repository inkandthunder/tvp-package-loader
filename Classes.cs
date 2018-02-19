using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace package_loader
{
    public class Packages
    {
        public List<Package> Package { get; set; }
    }

    public class Package
    {
        public string NslId { get; set; }
        public string SequenceNumber { get; set; }
        public string LogicalPage { get; set; }
        public string DivertCode { get; set; }
        public string EnclosureCode { get; set; }
    }
}
