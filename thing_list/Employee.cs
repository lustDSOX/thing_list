using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thing_list
{
    public class Employee
    {
        public int id { get; set; }
        public string surname { get; set; }
        public string name { get; set; } 
        public string patronymic { get; set; }

        public List<Thing> Things{ get; set; } = new();
        public List<Taken_things> Taken_Things { get; set; } = new();
        public Employee() { }

        public Employee(string surname, string name, string patronymic)
        {
            this.name = name;
            this.surname = surname;
            this.patronymic = patronymic;
        }

    }
}
