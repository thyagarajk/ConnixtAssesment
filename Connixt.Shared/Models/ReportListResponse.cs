using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connixt.Shared.Models
{
    public class ReportListResponse
    {
        public int Total { get; set; }
        public List<ReportRow> Rows { get; set; } = new();
    }
}
