using AccountDataTransform.Library;
using AccountDataTransform.Library.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;

namespace AccountDataTransform.Test
{
    public class DataTransformerTest
    {
        public DataTransformerTest()
        {

        }
        [Fact]
        public void MissingFieldProcessorWillNotCreateDataTransfor()
        {
            IList<IFieldProcessor> processorList = new List<IFieldProcessor>();
            processorList.Add(new IdAccountNoProcessor(TargetFieldConstant.AccountCode, 0));
            processorList.Add(new RequiredFieldProcessor(TargetFieldConstant.Name, 1));
            IList<string> srdRRSPList = new List<string>() { "1", "2", "3", "4" };
            IList<string> targetRRSPList = new List<string>() { "Trading", "RRSP", "RESP", "Fund" };
            processorList.Add(new EnumFieldProcessor(srdRRSPList, targetRRSPList, TargetFieldConstant.Type, 2));
            //processorList.Add(new OptionFieldProcessor(TargetFieldConstant.OpenDate, 3, (int)EnumDataTypes.DateTime));
            IList<string> srdCurrentcyList = new List<string>() { "CD", "US" };
            IList<string> targetCurrencyList = new List<string>() { "CAD", "USD" };
            processorList.Add(new EnumFieldProcessor(srdCurrentcyList, targetCurrencyList, TargetFieldConstant.Currency, 4));
            string[] targetFieldArray = new string[] { TargetFieldConstant.AccountCode, TargetFieldConstant.Name, TargetFieldConstant.Type, TargetFieldConstant.OpenDate, TargetFieldConstant.Currency };

            Assert.Throws<ArgumentNullException>(() =>
            {
                DataTransformer transform = new DataTransformer(processorList, targetFieldArray);
            });
        }

    }
}
