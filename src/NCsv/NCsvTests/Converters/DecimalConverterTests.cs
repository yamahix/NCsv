﻿using NCsv;
using NCsv.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace CsvSerializerTests.Converters
{
    [TestClass]
    public class DecimalConverterTests
    {
        [TestMethod]
        public void ConvertToCsvItemTest()
        {
            var c = new DecimalConverter();
            Assert.AreEqual("1000", c.ConvertToCsvItem(CreateConvertToCsvItemContext(1000m)));
            Assert.AreEqual("1,000", c.ConvertToCsvItem(CreateConvertToCsvItemContext(1000m, "FormattedValue")));
        }

        [TestMethod]
        public void TryConvertToObjectItemTest()
        {
            Assert.AreEqual(1000m, ConvertToObjectItem("1000"));
            Assert.AreEqual(1000m, ConvertToObjectItem("1,000"));
        }

        [TestMethod]
        public void TryConvertToObjectItemFailureTest()
        {
            var c = new DecimalConverter();
            var context = CreateConvertToObjectItemContext("x");
            Assert.IsFalse(c.TryConvertToObjectItem(context, out object? _, out string message));
            Assert.AreEqual(CsvConfig.Current.ValidationMessage.GetNumericConvertError(context), message);
        }

        [TestMethod]
        public void TryConvertToObjectItemRequireTest()
        {
            var c = new DecimalConverter();
            var context = CreateConvertToObjectItemContext(string.Empty);
            Assert.IsFalse(c.TryConvertToObjectItem(context, out object? _, out string message));
            Assert.AreEqual(CsvConfig.Current.ValidationMessage.GetRequiredError(context), message);
        }

        private decimal? ConvertToObjectItem(string csvItem)
        {
            var c = new DecimalConverter();
            Assert.IsTrue(c.TryConvertToObjectItem(CreateConvertToObjectItemContext(csvItem), out object? result, out string _));
            return (decimal?)result;
        }

        private ConvertToCsvItemContext CreateConvertToCsvItemContext(object? objectItem, string name = nameof(Foo.Value))
        {
            var p = GetProperty(name);
            return new ConvertToCsvItemContext(p, p.Name, objectItem);
        }

        private ConvertToObjectItemContext CreateConvertToObjectItemContext(string csvItem, string name = nameof(Foo.Value))
        {
            var p = GetProperty(name);
            return new ConvertToObjectItemContext(p, p.Name, 1, csvItem);
        }

        private CsvProperty GetProperty(string name)
        {
            return new CsvProperty(typeof(Foo), name);
        }

        private class Foo
        {
            public decimal Value { get; set; }

            [CsvFormat("#,0")]
            public decimal FormattedValue { get; set; }
        }
    }
}
