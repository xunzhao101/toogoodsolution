using AccountDataTransform.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDataTransform.Library
{
    /// <summary>
    /// DataTransformer class use a List of IFieldProcessor objects transforem a source data file into correct format.
    /// An instance of a DataTransformer will process one specific format of data file.
    /// </summary>
    public class DataTransformer : IDataTransformer
    {
        private readonly Dictionary<string, IFieldProcessor> fieldProcessors=new Dictionary<string, IFieldProcessor>();
        //private readonly string[] targetFieldArray = new string[] { TargetFieldConstant.AccountCode, TargetFieldConstant.Name, TargetFieldConstant.Type, TargetFieldConstant.OpenDate, TargetFieldConstant.Currency };
        private readonly string[] targetFieldArray;
        private DataValidationResult validationResult;
        public DataValidationResult ValidationResult
        {
            get { return validationResult; }
        }
        private DataTransformResult transformResult;
        public  DataTransformResult TransformResult
        {
            get { return transformResult; }
        }
        /// <summary>
        /// using processors and the fields to create a DataTransform object.
        /// </summary>
        /// <param name="processors">
        ///     the list of IFieldProcessor object to process values in source data file. The number of 
        ///     IFieldProcessor of processors should match the columns requrited by target file, which is 
        ///     specified by the targetFields array.
        /// </param>
        /// <param name="targetFields">
        ///     the fields included in the result file.
        /// </param>
        public DataTransformer(IList<IFieldProcessor> processors, string[] targetFields)
        {
            if (processors == null || processors.Count==0|| targetFields == null || targetFields.Length == 0)
                throw new ArgumentNullException("Invalid argumments for DataTransformer");
            if(processors.Count!=targetFields.Length)
            {
                throw new ArgumentNullException("Field processor number is not equal to required fields count");
            }

            foreach (IFieldProcessor processor in processors)
            {
                fieldProcessors.Add(processor.TargetField, processor);
            }
            targetFieldArray = targetFields;
            string message = string.Empty;
            bool missingFieldConverter = false;
            foreach (string targetField in targetFieldArray)
            {
                if (!fieldProcessors.ContainsKey(targetField))
                {
                    missingFieldConverter = true;
                    message += "Missing field processor for field:" + targetField + "\n";
                }
            }
            if (missingFieldConverter)
            {
                throw new Exception(message);
            }
        }

        public DataTransformResult Transform(IList<string> lines)
        {
            DataTransformResult result = new DataTransformResult();
            IList<string> errorList = new List<string>();
            IList<AccountStandardDTO> succeedList = new List<AccountStandardDTO>();
            for (int i = 0; i < lines.Count(); i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                    continue;
                AccountStandardDTO record = ConvertOneLine(lines[i]);
                if (record != null)
                {
                    succeedList.Add(record);
                }
                else
                {
                    errorList.Add("invalid line " + i + ":" + lines[i]);
                }
            }
            result.InvalidLineCount = errorList.Count();
            result.SucceedLineCount = succeedList.Count();
            result.TotalLineCount = result.InvalidLineCount + result.SucceedLineCount;
            result.resultList = succeedList;
            result.ErrorLines = errorList;
            this.transformResult = result;
            return result;
        }

        public DataValidationResult ValidateData(IList<string> lines)
        {
            DataValidationResult result = new DataValidationResult();

            IList<string> errorList = new List<string>();
            IList<string> succeedList = new List<string>();
            for (int i = 0; i < lines.Count(); i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                    continue;
                if(ValidateOneLine(lines[i]))
                {
                    succeedList.Add(lines[i]);
                }
                else
                {
                    errorList.Add(lines[i]);
                }
            }
            result.ErrorLineCount = errorList.Count();
            result.SucceedLineCount = succeedList.Count();
            result.TotalLineCount = result.ErrorLineCount + result.SucceedLineCount;
            result.ValidatedLines = succeedList;
            result.ErrorLines = errorList;
            this.validationResult = result;
            return result;
        }

        private bool ValidateOneLine(string line)
        {
            string[] values = line.Split(new char[] { ',' });
            for (int i = 0; i < targetFieldArray.Length; i++)
            {
                IFieldProcessor processor = fieldProcessors[targetFieldArray[i]];
                if (!processor.ValidateField(values[processor.SourceFieldIndex]))
                    return false;
            }
            return true;
        }

        private AccountStandardDTO ConvertOneLine(string line)
        {
            AccountStandardDTO ret = new AccountStandardDTO();
            string[] values = line.Split(new char[] { ',' });
            for (int i = 0; i < targetFieldArray.Length; i++)
            {
                IFieldProcessor processor = null;
                switch (targetFieldArray[i])
                {
                    case TargetFieldConstant.AccountCode:
                        processor = fieldProcessors[targetFieldArray[i]];
                        ret.AccountCode = processor.ConvertField(values[processor.SourceFieldIndex]);
                        break;
                    case TargetFieldConstant.Name:
                        processor = fieldProcessors[targetFieldArray[i]];
                        ret.Name = processor.ConvertField(values[processor.SourceFieldIndex]);
                        break;
                    case TargetFieldConstant.Type:
                        processor = fieldProcessors[targetFieldArray[i]];
                        ret.Type = processor.ConvertField(values[processor.SourceFieldIndex]);
                        break;
                    case TargetFieldConstant.OpenDate:
                        processor = fieldProcessors[targetFieldArray[i]];
                        ret.OpenDate =processor.ConvertField(values[processor.SourceFieldIndex]);
                        break;
                    case TargetFieldConstant.Currency:
                        processor = fieldProcessors[targetFieldArray[i]];
                        ret.Currency = processor.ConvertField(values[processor.SourceFieldIndex]);
                        break;

                }
            }
            
            return ret;
        }

        public IList<string> GetResult()
        {
            if (fieldProcessors.ContainsKey(TargetFieldConstant.OpenDate))
                return GetResultWithOpenDate();
            else
                return GetResultWithoutOpenDate();
        }

        private IList<string> GetResultWithOpenDate()
        {
            
            IList<string> retList = new List<string>();
            retList.Add("AccountCode,Name,Type,Open Date,Currency");
            if (transformResult != null)
            {
                for (int i = 0; i < transformResult.resultList.Count(); i++)
                {
                    retList.Add(transformResult.resultList[i].ToString());
                }
            }
            return retList;
        }

        private IList<string> GetResultWithoutOpenDate()
        {
            IList<string> retList = new List<string>();
            retList.Add("AccountCode,Name,Type,Currency");
            if (transformResult != null)
            {
                for (int i = 0; i < transformResult.resultList.Count(); i++)
                {
                    retList.Add(transformResult.resultList[i].ToString(false));
                }
            }
            return retList;
        }
    }
}
