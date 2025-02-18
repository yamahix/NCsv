﻿using System;
using System.Collections.Generic;
using System.Text;
using NCsv;
using NCsv.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace CsvSerializerTests.Converters
{
    [TestClass]
    public class StringConverterTests
    {
        [TestMethod]
        public void ConvertToCsvItemTest()
        {
            var sut = new StringConverter();
            Assert.AreEqual("abc", sut.ConvertToCsvItem(CreateConvertToCsvItemContext("abc")));
        }

        [TestMethod]
        public void TryConvertToObjectItemTest()
        {
            Assert.AreEqual("abc", ConvertToObjectItem("abc"));
        }

        private string? ConvertToObjectItem(string csvItem)
        {
            var sut = new StringConverter();
            Assert.IsTrue(sut.TryConvertToObjectItem(CreateConvertToObjectItemContext(csvItem), out object? result, out string _));
            return (string?)result;
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
            public string Value { get; set; } = string.Empty;
        }
    }
}
