using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinesauce_ROM_Corrupter.Changelog
{
    class ChangeItem
    {
        public string version { get; set; }
        public List<Feature> featurelist { get; set; }
    }
}
