using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDataTransform.Library
{
    public class AccountStandardDTO
    {
        public string AccountCode { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string OpenDate{get;set;}
        public string Currency { get; set; }
        public string ToString(bool withOpenDate=true)
        {
            if(withOpenDate)
            {
                return AccountCode + "," + Name + "," + Type + "," + OpenDate + "," + Currency;
            }
            else
            {
                return AccountCode + "," + Name + "," + Type + "," + "," + Currency;
            }
        }

    }
}
