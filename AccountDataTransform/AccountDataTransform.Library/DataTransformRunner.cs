using AccountDataTransform.Library.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDataTransform.Library
{
    public class DataTransformRunner
    {
        private readonly string filePath;
        private readonly IDataTransformer transformer;
        private readonly bool withHeader;
        public DataTransformResult TransformResult { get; private set; }
        public DataValidationResult ValidationResult { get; private set; }

        public DataTransformRunner(IDataTransformer transformer, string filePath, bool withHeader=false)
        {
            this.filePath = filePath;
            this.transformer = transformer;
            this.withHeader = withHeader;
        }

        public void Execute()
        {
            IList<string> lines = File.ReadAllLines(filePath).ToList();
            if (withHeader)
            {
                lines.RemoveAt(0);
            }

            ValidationResult = transformer.ValidateData(lines);
            if(ValidationResult.SucceedLineCount>0)
            {
                TransformResult = transformer.Transform(ValidationResult.ValidatedLines);
            }
        }

        public IList<string> GetResult()
        {
           return transformer.GetResult();
        }
    }
}
