using AccountDataTransform.Library;
using System;
using Xunit;

namespace AccountDataTransform.Test
{

    public class OptionFieldProcessorTest
    {
       
        public OptionFieldProcessorTest()
        {
           
        }
        [Fact]
        public void ValidateInvalidFieldValueReturnFalse()
        {
            OptionFieldProcessor processor = new OptionFieldProcessor("OpenDate", 1, (int)EnumDataTypes.DateTime);
           
            var ret = processor.ValidateField("2021-02-30");
            Assert.False(ret);
            ret = processor.ValidateField("2021-02-");
            Assert.False(ret);
            ret = processor.ValidateField("2001-04-31");
            Assert.False(ret);
        }

        [Fact]
        public void ValidateValidFieldValueReturnTrue()
        {
            OptionFieldProcessor processor = new OptionFieldProcessor("OpenDate", 1, (int)EnumDataTypes.DateTime);
            bool ret = processor.ValidateField("");
            Assert.True(ret);
            ret = processor.ValidateField("2021-02-28");
            Assert.True(ret);
            ret = processor.ValidateField("2021-1-1");
            Assert.True(ret);
            ret = processor.ValidateField("2001-12-31");
            Assert.True(ret);
        }
    }
}
