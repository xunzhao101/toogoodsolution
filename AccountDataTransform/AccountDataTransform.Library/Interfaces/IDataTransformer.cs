using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDataTransform.Library.Interfaces
{
    public interface IDataTransformer
    {
        DataValidationResult ValidateData(IList<string> lines);
        DataTransformResult Transform(IList<string> lines);
        IList<string> GetResult();
    }
}
