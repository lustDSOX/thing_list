using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thing_list
{
    public class Location
    {
        public int id { get; set; }
        public string name { get; set; }

        public Location() { }

        public Location(string name)
        {
            this.name = name;
        }

    }
}
