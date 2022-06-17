using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thing_list
{
    public class Thing
    {
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public int count { get; set; }
        public int id_location { get; set; }
        public List<Tag> Tags { get; set; } = new();
        //public List<Tag_thing> Tag_Things { get; set; } = new();

        public Thing() { }

        public Thing(string name, string number, int count, int id_location)
        {
            this.name = name;
            this.number = number;
            this.count = count;
            this.id_location = id_location;
        }

    }
}
