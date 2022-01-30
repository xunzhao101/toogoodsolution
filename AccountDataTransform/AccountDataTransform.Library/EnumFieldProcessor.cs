using AccountDataTransform.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDataTransform.Library
{
    /// <summary>
    /// EnumFieldProcessor handle one to one match transformation of field value
    /// </summary>
    /// <example>
    /// value in sourc file: C is converted to CAD, and U is converted to USD.
    /// </example>
    public class EnumFieldProcessor : IFieldProcessor
    {

        private readonly Dictionary<string, string> dict = new Dictionary<string, string>();

        public string TargetField { get;  }
        public int SourceFieldIndex { get;  }

        public int? DataType { get; }

        public EnumFieldProcessor(IList<string> srcList, IList<string> targetList, string targetField, int sourceFieldIndex, int? dataType=null)
        {
            if (srcList == null || targetList == null)
                throw new Exception("Incorrect parameters");
            if (srcList.Count() != targetList.Count())
                throw new Exception("Source list length don't match target list length");
            for(int i=0;i<srcList.Count;i++)
            {
                dict.Add(srcList[i], targetList[i]);
            }
            TargetField = targetField;
            SourceFieldIndex = sourceFieldIndex;
            DataType = dataType;
        }
        public string ConvertField(string fieldValue)
        {
            string trimedValue = fieldValue.Trim();
            if (!dict.ContainsKey(trimedValue))
                throw new Exception("Invalid value");
            return dict[trimedValue];
        }

        public bool ValidateField(string fieldValue)
        {
            if (dict.ContainsKey(fieldValue.Trim()))
                return true;
            else
                return false;
        }
    }
}
