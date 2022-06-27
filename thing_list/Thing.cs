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
        public DateTime? date { get; set; }
        public List<Tag> Tags { get; set; } = new();
        public List<Employee> Employees { get; set; } = new();
        public List<Taken_things> Taken_Things { get; set; } = new();

        public Thing(string name, string number, int count)
        {
            this.name = name;
            this.number = number;
            this.count = count;           
        }

    }
}
