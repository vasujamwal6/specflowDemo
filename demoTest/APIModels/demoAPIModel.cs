using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoTest.APIModels
{
    public class demoAPIModel
    {
        
            public string id { get; set; }
            public string name { get; set; }
            public Data data { get; set; }
        

        public class Data
        {
            public int capactity { get; set; }
            public string color { get; set; }
        }

    }
}
