using AccountDataTransform.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDataTransform.Library
{
    /// <summary>
    /// The class to handle extracting the target value from the value in source file
    /// 
    /// </summary>
    /// <example>
    /// In Source file: 123|ABC
    /// In Target file: ABC
    /// </example>
    public class IdAccountNoProcessor : IFieldProcessor
    {
        public string TargetField { get; }
        public int SourceFieldIndex { get; }

        public int? DataType { get; }

        /// <summary>
        /// Constructor to create an IdAccountNoProcessor object
        /// </summary>
        /// <param name="targetField">Field name of value in target file</param>
        /// <param name="srcFieldIndex">the index of value in the line of source CSV file.</param>
        public IdAccountNoProcessor(string targetField, int srcFieldIndex, int? dataType=null)
        {
            TargetField = targetField;
            SourceFieldIndex = srcFieldIndex;
            DataType = dataType;

        }
        /// <summary>
        /// Extract the value from value in source file
        /// </summary>
        /// <param name="fieldValue">the value in source file</param>
        /// <returns>Extracted value</returns>
        public string ConvertField(string fieldValue)
        {
            string[] idAcountPair = fieldValue.Split(new char[] { '|' });
            if (idAcountPair.Length == 2)
                return idAcountPair[1];
            else
                throw new Exception("Not valid input value");
        }
        /// <summary>
        /// Validate the value of source file is in right format.
        /// </summary>
        /// <param name="fieldValue">The value in source file</param>
        /// <returns>
        /// True: fieldValue is in right format.
        /// False: fieldvalue is not in right format.
        /// </returns>
        public bool ValidateField(string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue))
                return false;
            string[] idAcountPair = fieldValue.Split(new char[] { '|' });
            if (idAcountPair.Length != 2)
                return false;
            return true;
        }
    }
}
