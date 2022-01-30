using AccountDataTransform.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDataTransform.Library
{
    /// <summary>
    /// A implmentation of IFieldProcessor to handle an optional field value.
    /// </summary>
    public class OptionFieldProcessor : IFieldProcessor
    {
        public string TargetField { get; }
        public int SourceFieldIndex { get; }

        public int? DataType { get; }

        public OptionFieldProcessor(string targetField, int srcFieldIndex, int? dataType=null)
        {
            TargetField = targetField;
            SourceFieldIndex = srcFieldIndex;
            DataType = dataType;

        }
        public string ConvertField(string fieldValue)
        {
            
            return fieldValue;
        }

        public bool ValidateField(string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue))
                return true;
            if(DataType!=null)
            {
                switch (DataType)
                {
                    case (int)EnumDataTypes.Integer:
                        int ret;
                        if (!Int32.TryParse(fieldValue, out ret))
                            return false;
                        break;
                    case (int)EnumDataTypes.DateTime:
                        DateTime dateRet;
                        if (!DateTime.TryParse(fieldValue, out dateRet))
                            return false;
                        if (dateRet.Year <= 1900)
                            return false;
                        break;
                }
            }
            return true;
        }
    }
}
