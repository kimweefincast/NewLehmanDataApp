using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LehmanSystemTransfer.Models
{
    class FileRecord
    {
        public Guid ID { get; set; }
        public DateTime DateStamp { get; set; }
        public string Description { get; set; }
    }
}
