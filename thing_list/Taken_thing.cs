using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thing_list
{
    public class Taken_thing
    {
        public int id_thing { get; set; }
        public int id_employee { get; set; }
        public string date { get; set; }

        public Taken_thing() { }

        public Taken_thing(int id_thing, int id_employee, string date)
        {
            this.id_thing = id_thing;
            this.id_employee = id_employee;
            this.date = date;
        }
    }
}
