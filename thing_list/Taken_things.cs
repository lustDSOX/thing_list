using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thing_list
{
    public class Taken_things
    {
        public int id_thing { get; set; }
        public Thing thing { get; set; }
        public int id_employee { get; set; }
        public Employee employee { get; set; }
        public DateTime? date { get; set; }
        public string? comm { get; set; }
        public string count { get; set; }
        public Taken_things() { }
    }
}
