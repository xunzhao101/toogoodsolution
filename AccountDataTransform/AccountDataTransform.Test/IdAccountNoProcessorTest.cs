using AccountDataTransform.Library;
using System;
using Xunit;

namespace AccountDataTransform.Test
{

    public class IdAccountNoProcessorTest
    {
        private IdAccountNoProcessor processor;
        public IdAccountNoProcessorTest()
        {
            processor = new IdAccountNoProcessor("AccountCode", 0);
        }
        [Fact]
        public void ValidateInvalidFieldReturnFalse()
        {
            bool ret = processor.ValidateField("");
            Assert.False(ret);
            ret = processor.ValidateField("123");
            Assert.False(ret);
            ret = processor.ValidateField("123,abc");
            Assert.False(ret);
            ret = processor.ValidateField("123|abc|test");
            Assert.False(ret);
        }
        [Fact]
        public void ValidateInvalidFieldReturnTrue()
        {
            bool ret = processor.ValidateField("1|Abc");
            Assert.True(ret);
            ret = processor.ValidateField("121|Abc");
            Assert.True(ret);
            ret = processor.ValidateField("333|Abc");
            Assert.True(ret);
           
        }

        [Fact]
        public void TransformInvalidFieldThrowException()
        {
            Assert.Throws<Exception>(() => processor.ConvertField(""));
            Assert.Throws<Exception>(() => processor.ConvertField("123"));
            Assert.Throws<Exception>(() => processor.ConvertField("123,abc"));
            Assert.Throws<Exception>(() => processor.ConvertField("123|abc|test"));
        }

        [Fact]
        public void TransformValidFieldReturnCorrectValue()
        {
            var ret= processor.ConvertField("123|abc");
            Assert.Equal("abc",ret);
        }
    }
}