﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCsv;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using NotVisualBasic.FileIO;
using NCsv.Validations;

namespace NCsv.Tests
{
    [TestClass()]
    public class CsvSerializerTests
    {
        [TestMethod()]
        public void SerializeTest()
        {
            var examples = CreateExamples();

            var sut = new CsvSerializer<Example>
            {
                HasHeader = true
            };

            var csv = sut.Serialize(examples);
            Assert.AreEqual(CreateExamplesCsv(), csv);
        }

        [TestMethod()]
        public void SerializeWriterTest()
        {
            var examples = CreateExamples();

            var sut = new CsvSerializer<Example>
            {
                HasHeader = true
            };

            var writer = new StringWriter();
            sut.Serialize(writer, examples);

            Assert.AreEqual(CreateExamplesCsv(), writer.ToString());
        }

        [TestMethod()]
        public void DeserializeTest()
        {
            var sut = new CsvSerializer<Example>
            {
                HasHeader = true
            };

            var examples = sut.Deserialize(CreateExamplesCsv());
            CollectionAssert.AreEqual(CreateExamples(), examples);
        }

        [TestMethod()]
        public void DeserializeReaderTest()
        {
            var sut = new CsvSerializer<Example>
            {
                HasHeader = true
            };

            var reader = new StringReader(CreateExamplesCsv());
            var examples = sut.Deserialize(reader);
            CollectionAssert.AreEqual(CreateExamples(), examples);
        }

        [TestMethod()]
        public void DeserializeErrorTest()
        {
            var sut = new CsvSerializer<ForDesirializeError>();
            Assert.ThrowsException<CsvValidationException>(() => sut.Deserialize("x"));
        }

        [TestMethod()]
        public void ParseErrorTest()
        {
            var sut = new CsvSerializer<Example>();
            Assert.ThrowsException<CsvParseException>(() => sut.Deserialize("\"foo\",\"ba\"r\""));
        }


        [TestMethod()]
        public void GetErrorsTest()
        {
            var sut = new CsvSerializer<ForDesirializeError>();
            var errors = sut.GetErrors("1000,2000\r\nx,y");

            Assert.AreEqual(4, errors.Count);

            var errorLine1 = errors[0];
            Assert.AreEqual(1, errorLine1.Context.LineNumber);
            Assert.AreEqual(CsvMessages.GetPrecisionError(errorLine1.Context, 3), errorLine1.ErrorMessage);

            var errorLine2 = errors[3];
            Assert.AreEqual(2, errorLine2.Context.LineNumber);
            Assert.AreEqual(CsvMessages.GetNumericConvertError(errorLine2.Context), errorLine2.ErrorMessage);
        }

        private Example[] CreateExamples()
        {
            return new Example[]
            {
                new Example()
                {
                    StringValue = "\"\"foo",
                    DecimalValue = 123456,
                    DateTimeValue = new DateTime(2020, 1, 1),
                    BooleanValue = true,
                    IntValue = 100,
                    NullableDecimalValue = 1000,
                    NullableDateTimeValue = new DateTime(2020, 1, 2),
                    NullableIntValue = 111,
                    SeparateIndex = "bar",
                    ValueObject = new ValueObject("abc"),
                    DoubleValue = 10.123,
                    NullableDoubleValue = 111.111,
                    ShortValue = 100,
                    NullableShortValue = 200,
                    LongValue = 10000,
                    NullableLongValue = 20000,
                    FloatValue = 1.1f,
                    NullableFloatValue = 1.2f,
                },
                new Example()
                {
                    StringValue = "foo",
                    DecimalValue = 123456,
                    DateTimeValue = new DateTime(2020, 1, 1),
                    BooleanValue = true,
                    IntValue = 100,
                    NullableDecimalValue = null,
                    NullableDateTimeValue = null,
                    NullableIntValue = null,
                    SeparateIndex = "bar",
                    ValueObject = new ValueObject("xyz"),
                    DoubleValue = 10.123,
                    NullableDoubleValue = 111.111,
                    ShortValue = 100,
                    NullableShortValue = 200,
                    LongValue = 10000,
                    NullableLongValue = 20000,
                    FloatValue = 1.1f,
                    NullableFloatValue = 1.2f,
                },
            };
        }

        private string CreateExamplesCsv()
        {
            return
@"CustomName,DecimalValue,DateTimeValue,BooleanValue,IntValue,NullableDecimalValue,NullableDateTimeValue,NullableIntValue,,SeparateIndex,ValueObject,DoubleValue,NullableDoubleValue,ShortValue,NullableShortValue,LongValue,NullableLongValue,FloatValue,NullableFloatValue
""""""""""foo"",""123,456"",2020/01/01,True,100,1000,2020/01/02,111,,bar,abc,10.123,111.111,100,200,10000,20000,1.1,1.2
foo,""123,456"",2020/01/01,True,100,,,,,bar,xyz,10.123,111.111,100,200,10000,20000,1.1,1.2
";
        }

        private class ForDesirializeError
        {
            [CsvColumn(0)]
            [CsvNumber(3, 0)]
            public int Value1 { get; set; }

            [CsvColumn(1)]
            [CsvNumber(3, 0)]
            public int Value2 { get; set; }
        }
    }
}