﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoTest.APIModels
{
    public class postBodyModel
    {

        
            public string name { get; set; }
            public Data data { get; set; }
        

        public class Data
        {
            public string color { get; set; }
            public int capacityGB { get; set; }
        }

    }
}
