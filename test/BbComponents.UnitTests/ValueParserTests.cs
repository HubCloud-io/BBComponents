using BBComponents.Helpers;
using BBComponents.Enums;
using NUnit.Framework;
using System;
using System.Globalization;

namespace BbComponents.UnitTests
{
    [TestFixture]
    public class ValueParserTests
    {
        [Test]
        public void TryParse_GuidStringGuid_ReturnGuid()
        {
            var valueStr = "0BA805EA-C453-461E-910C-BC79D5C0EDFC";
            var parseResult = ValueParser.TryParse<Guid>(valueStr);

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(Guid.Parse(valueStr), parseResult.Value);
        }

        [Test]
        public void TryParse_GuidStringNonGuid_NotParsed()
        {
            var valueStr = "non guid";
            var parseResult = ValueParser.TryParse<Guid>(valueStr);

            Assert.IsFalse(parseResult.IsParsed);

        }

        [Test]
        public void TryParse_ByteStringByte_ReturnByte()
        {
            var parseResult = ValueParser.TryParse<byte>("1");

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(1, parseResult.Value);
        }

        [Test]
        public void TryParse_ByteStringNonByte_NotParse()
        {
            var parseResult = ValueParser.TryParse<byte>("str");

            Assert.IsFalse(parseResult.IsParsed);
        }

        [Test]
        public void TryParse_IntStringInt_ReturnInt()
        {
            var parseResult = ValueParser.TryParse<int>("1");

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(1, parseResult.Value);
        }

        [Test]
        public void TryParse_IntStringIntWithGroupSeparator_ReturnInt()
        {
            var parseResult = ValueParser.TryParse<int>("1 000");

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(1000, parseResult.Value);
        }

        [Test]
        public void TryParse_IntStringIntWithoutGroupSeparator_ReturnInt()
        {
            var parseResult = ValueParser.TryParse<int>("1000");

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(1000, parseResult.Value);
        }

        [Test]
        public void TryParse_IntStringNonInt_NotParse()
        {
            var parseResult = ValueParser.TryParse<int>("str");

            Assert.IsFalse(parseResult.IsParsed);
        }

        [Test]
        public void TryParse_LongStringIntWithGroupSeparator_ReturnLong()
        {
            var parseResult = ValueParser.TryParse<long>("1 000");

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(1000F, parseResult.Value);
        }

        [Test]
        public void TryParse_LongStringIntWithoutGroupSeparator_ReturnLong()
        {
            var parseResult = ValueParser.TryParse<long>("1000");

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(1000F, parseResult.Value);
        }

        [Test]
        public void TryParse_LongStringNonLong_NotParse()
        {
            var parseResult = ValueParser.TryParse<long>("str");

            Assert.IsFalse(parseResult.IsParsed);
        }

        [Test]
        public void TryParse_DecimalStringIntWithGroupSeparator_ReturnDecimal()
        {
            var parseResult = ValueParser.TryParse<decimal>("1 000.23");

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(1000.23M, parseResult.Value);
        }

        [Test]
        public void TryParse_DecimalStringIntWithGroupSeparatorAndCustomDecimalSeparator_ReturnDecimal()
        {

            var nfi = new NumberFormatInfo
            {
                NumberGroupSeparator = " ",
                NumberDecimalSeparator = ",",
                NumberDecimalDigits = 3
            };

            var parseResult = ValueParser.TryParse<decimal>("1 000,234", nfi);

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(1000.234M, parseResult.Value);
        }

        [Test]
        public void TryParse_DecimalStringIntWithoutGroupSeparator_ReturnDecimal()
        {
            var parseResult = ValueParser.TryParse<decimal>("1000");

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(1000M, parseResult.Value);
        }

        [Test]
        public void TryParse_DecimalStringNonDecimal_NotParse()
        {
            var parseResult = ValueParser.TryParse<decimal>("str");

            Assert.IsFalse(parseResult.IsParsed);
        }

        [Test]
        public void TryParse_DoubleStringIntWithGroupSeparator_ReturnDouble()
        {
            var parseResult = ValueParser.TryParse<double>("1 000.23");

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(1000.23D, parseResult.Value);
        }

        [Test]
        public void TryParse_DoubleStringIntWithGroupSeparatorAndCustomDecimalSeparator_ReturnDouble()
        {

            var nfi = new NumberFormatInfo
            {
                NumberGroupSeparator = " ",
                NumberDecimalSeparator = ",",
                NumberDecimalDigits = 3
            };

            var parseResult = ValueParser.TryParse<double>("1 000,234", nfi);

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(1000.234D, parseResult.Value);
        }

        [Test]
        public void TryParse_DoubleStringIntWithoutGroupSeparator_ReturnDouble()
        {
            var parseResult = ValueParser.TryParse<double>("1000");

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(1000M, parseResult.Value);
        }

        [Test]
        public void TryParse_DoubleStringNonNubmer_NotParse()
        {
            var parseResult = ValueParser.TryParse<double>("str");

            Assert.IsFalse(parseResult.IsParsed);
        }

        [Test]
        public void TryParse_FloatStringIntWithGroupSeparator_ReturnFloat()
        {
            var parseResult = ValueParser.TryParse<float>("1 000.23");

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(1000.23F, parseResult.Value);
        }

        [Test]
        public void TryParse_FloatStringIntWithGroupSeparatorAndCustomDecimalSeparator_ReturnFloat()
        {

            var nfi = new NumberFormatInfo
            {
                NumberGroupSeparator = " ",
                NumberDecimalSeparator = ",",
                NumberDecimalDigits = 3
            };

            var parseResult = ValueParser.TryParse<float>("1 000,234", nfi);

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(1000.234F, parseResult.Value);
        }

        [Test]
        public void TryParse_FloatStringIntWithoutGroupSeparator_ReturnFloat()
        {
            var parseResult = ValueParser.TryParse<float>("1000");

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(1000F, parseResult.Value);
        }

        [Test]
        public void TryParse_FloatStringNonNumber_NotParse()
        {
            var parseResult = ValueParser.TryParse<float>("str");

            Assert.IsFalse(parseResult.IsParsed);
        }

        [Test]
        public void TryParse_EnumStringValidName_ReturnEnumValue()
        {
            var parseResult = ValueParser.TryParse<BootstrapColors>("Primary");

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(BootstrapColors.Primary, parseResult.Value);
        }

        [Test]
        public void TryParse_EnumStringValidNameWrongCase_ReturnEnumValue()
        {
            var parseResult = ValueParser.TryParse<BootstrapColors>("pRimarY");

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(BootstrapColors.Primary, parseResult.Value);
        }

        [Test]
        public void TryParse_EnumStringNotExistingName_NotParsed()
        {
            var parseResult = ValueParser.TryParse<BootstrapColors>("NonExisting");

            Assert.IsFalse(parseResult.IsParsed);
        }


        [Test]
        public void TryParse_String_ReturnString()
        {
            var parseResult = ValueParser.TryParse<string>("string");

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual("string", parseResult.Value);

        }

        [Test]
        public void TryParse_Null_ReturnNull()
        {
            var parseResult = ValueParser.TryParse<string>(null);

            Assert.IsTrue(parseResult.IsParsed);
            Assert.AreEqual(null, parseResult.Value);

        }


    }
}
