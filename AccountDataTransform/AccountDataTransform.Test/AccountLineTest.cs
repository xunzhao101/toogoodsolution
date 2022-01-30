using AccountDataTransform.Library;
using System.Collections.Generic;
using Xunit;

namespace AccountDataTransform.Test
{
    public class AccountLineTest
    {
        private EnumFieldProcessor accTypeProcessor;
        private IList<string> srdCurrentcyList = new List<string>() { "CD", "US" };
        private IList<string> targetCurrencyList = new List<string>() { "CAD", "USD" };
        private string sampleLine = "123|ABC,3323421,2,01-01-2018, CD";
        public AccountLineTest()
        {
            IList<string> srdRRSPList = new List<string>() { "1", "2", "3", "4" };
            IList<string> targetRRSPList = new List<string>() { "Trading", "RRSP", "RESP", "Fund" };
            accTypeProcessor = new EnumFieldProcessor(srdRRSPList, targetRRSPList, "Type", 2);
        }
        [Fact]
        public void CanReturnCorrectValueForAccountType()
        {
            string[] values = "123|ABC,3323421,2,01-01-2018, CD".Split(new char[] { ',' });
            var ret = accTypeProcessor.ConvertField(values[accTypeProcessor.SourceFieldIndex]);
            Assert.Equal("RRSP", ret);

            values = "123|ABC,3323421,1,01-01-2018, CD".Split(new char[] { ',' });
            ret = accTypeProcessor.ConvertField(values[accTypeProcessor.SourceFieldIndex]);
            Assert.Equal("Trading", ret);
            values = "123|ABC,3323421,3,01-01-2018, CD".Split(new char[] { ',' });
            ret = accTypeProcessor.ConvertField(values[accTypeProcessor.SourceFieldIndex]);
            Assert.Equal("RESP", ret);
            values = "123|ABC,3323421,4,01-01-2018, CD".Split(new char[] { ',' });
            ret = accTypeProcessor.ConvertField(values[accTypeProcessor.SourceFieldIndex]);
            Assert.Equal("Fund", ret);
        }
    }
}
