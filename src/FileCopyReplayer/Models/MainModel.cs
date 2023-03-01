using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCopyReplayer.Models
{
    public class MainModel
    {

        public double OrigWidth { get; set; }
        public double OrigHeight { get; set; }

        public List<string> SourceFileList { get; set; }


        public string DestinationPath { get; set; }

    }
}
