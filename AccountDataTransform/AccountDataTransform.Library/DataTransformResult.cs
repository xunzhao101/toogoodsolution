using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDataTransform.Library
{
    public class DataTransformResult
    {
        public int ErrorNo { get; internal set; }
        public int TotalLineCount { get; set; }
        public int SucceedLineCount { get; set; }
        public int InvalidLineCount { get; set; }
        public IList<AccountStandardDTO> resultList { get; set; }
        public IList<string> ErrorLines { get; set; }
    }
}
