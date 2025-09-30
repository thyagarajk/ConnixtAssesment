using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Connixt.Shared.Models
{
    public class ReportRow
    {
        [XmlElement("ZHOSTID")]
        public string? ZHOSTID { get; set; }

        [XmlElement("ZDIDDATANUM")]
        public string? ZDIDDATANUM { get; set; }

        [XmlElement("ZDIDDATAST")]
        public string? ZDIDDATAST { get; set; }

        [XmlElement("ZDIDDESC")]
        public string? ZDIDDESC { get; set; }
    }
}
