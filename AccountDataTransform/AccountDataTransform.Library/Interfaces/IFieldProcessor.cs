using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDataTransform.Library.Interfaces
{
    public interface IFieldProcessor
    {
        string TargetField { get;  }
        int SourceFieldIndex { get; }

        int? DataType { get; }
        bool ValidateField(string fieldValue);
        string ConvertField(string fieldValue);

    }
}
