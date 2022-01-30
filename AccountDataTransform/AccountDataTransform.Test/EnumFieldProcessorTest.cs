using AccountDataTransform.Library;
using System;
using System.Collections.Generic;
using Xunit;

namespace AccountDataTransform.Test
{
    public class EnumFieldProcessorTest
    {
        private EnumFieldProcessor processor;
        public EnumFieldProcessorTest()
        {
            IList<string> srdRRSPList = new List<string>() { "1", "2", "3", "4" };
            IList<string> targetRRSPList = new List<string>() { "Trading", "RRSP", "RESP", "Fund" };
            processor = new EnumFieldProcessor(srdRRSPList, targetRRSPList,"Type",2);
        }
        [Fact]
        public void CanReturnCorrectValueForAccountType()
        {
            string[] values = "123|ABC,3323421,2,01-01-2018, CD".Split(new char[] { ',' });
            var ret = processor.ConvertField(values[processor.SourceFieldIndex]);
            Assert.Equal("RRSP", ret);

            values = "123|ABC,3323421,1,01-01-2018, CD".Split(new char[] { ',' });
            ret = processor.ConvertField(values[processor.SourceFieldIndex]);
            Assert.Equal("Trading", ret);
            values = "123|ABC,3323421,3,01-01-2018, CD".Split(new char[] { ',' });
            ret = processor.ConvertField(values[processor.SourceFieldIndex]);
            Assert.Equal("RESP", ret);
            values = "123|ABC,3323421,4,01-01-2018, CD".Split(new char[] { ',' });
            ret = processor.ConvertField(values[processor.SourceFieldIndex]);
            Assert.Equal("Fund", ret);
        }

        [Fact]
        public void CanReturnCorrectValueForCurrency()
        {
            IList<string> srdCurrentcyList = new List<string>() { "CD", "US" };
            IList<string> targetCurrencyList = new List<string>() { "CAD", "USD" };
            processor = new EnumFieldProcessor(srdCurrentcyList, targetCurrencyList, "Currency", 4);
            string[] values = "123|ABC,3323421,2,01-01-2018,CD".Split(new char[] { ',' });
            var ret = processor.ConvertField(values[processor.SourceFieldIndex]);
            Assert.Equal("CAD", ret);

            values = "123|ABC,3323421,1,01-01-2018,US".Split(new char[] { ',' });
            ret = processor.ConvertField(values[processor.SourceFieldIndex]);
            Assert.Equal("USD", ret);
        }
    }
}
