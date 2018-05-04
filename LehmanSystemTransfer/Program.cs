using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LehmanSystemTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            FTP2DBServices NewFTPService = new FTP2DBServices();
            NewFTPService.CheckUpdate();
        }
    }
}
