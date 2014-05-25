using ListExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ListExtensionTests
{
    [TestClass]
    public class ListExtensions
    {
        [TestMethod]
        public void ReduceShouldWork_ForLists()
        {
            var elem1 = new { x = 1, y = new List<string> { "a", "b" } };
            var elem2 = new { x = 1, y = new List<string> { "c", "d" } };

            var result = elem1.ReduceCollectionProperties(elem2);

            Assert.AreEqual(1, result.x);
            Assert.IsNotNull(result.y);
            Assert.AreEqual(4, result.y.Count);
            Assert.AreEqual("a", result.y.FirstOrDefault());
            Assert.AreEqual("d", result.y.LastOrDefault());
        }

        [TestMethod]
        public void ReduceShouldWork_ForArrays()
        {
            var elem1 = new ArrayItem { x = 1, y = new[] { "a", "b" } };
            var elem2 = new ArrayItem { x = 1, y = new[] { "c", "d" } };

            var result = elem1.ReduceCollectionProperties(elem2);

            Assert.AreEqual(1, result.x);
            Assert.IsNotNull(result.y);
            Assert.AreEqual(4, result.y.Length);
            Assert.AreEqual("a", result.y.FirstOrDefault());
            Assert.AreEqual("d", result.y.LastOrDefault());
        }

        [TestMethod]
        public void ReduceShouldWork_ForDictionaries()
        {
            var elem1 = new DictionaryItem { x = 1, y = new Dictionary<string, string> { { "a", "b" } } };
            var elem2 = new DictionaryItem { x = 1, y = new Dictionary<string, string> { { "c", "d" } } };

            var result = elem1.ReduceCollectionProperties(elem2);

            Assert.AreEqual(1, result.x);
            Assert.IsNotNull(result.y);
            Assert.AreEqual(2, result.y.Count);
            Assert.AreEqual("a", result.y.Keys.FirstOrDefault());
            Assert.AreEqual("d", result.y.Values.LastOrDefault());
        }

        class ArrayItem
        {
            public int x { get; set; }
            public string[] y { get; set; }
        }

        class DictionaryItem
        {
            public int x { get; set; }
            public Dictionary<string, string> y { get; set; }
        }
    }
}