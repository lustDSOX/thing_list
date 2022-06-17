using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thing_list
{
    public class Tag
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Thing> Things { get; set; } = new();
        //public List<Tag_thing> Tag_Things { get; set; } = new();

        public Tag() { }

        public Tag(string name)
        {
            this.name = name;
        }
    }
}
