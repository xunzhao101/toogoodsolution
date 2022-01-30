using AccountDataTransform.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDataTransform.Library
{
    public class RequiredFieldProcessor : IFieldProcessor
    {
        public string TargetField { get; }
        public int SourceFieldIndex { get; }

        public int? DataType { get; }

        public RequiredFieldProcessor(string targetField, int srcFieldIndex, int? dataType=null)
        {
            TargetField = targetField;
            SourceFieldIndex = srcFieldIndex;
            DataType = dataType;
        }
        public string ConvertField(string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue))
                throw new Exception("field value is empty");
            return fieldValue;
        }

        public bool ValidateField(string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue))
                return false;
            else
                return true;
        }
    }
}
