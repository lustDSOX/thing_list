using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thing_list
{
    public class Tag_thing
    {
        public int id_thing { get; set; }
        public int id_tag { get; set; }

        public Tag_thing(int id_tag, int id_thing)
        {
            this.id_tag = id_tag;
            this.id_thing = id_thing;
        }
    }
}
