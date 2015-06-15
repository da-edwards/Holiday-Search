using HolidaySearch.IOC;
using HolidaySearch.Search;
using HolidaySearch.Search.Repositories;
using NUnit.Framework;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidaySearch.UnitTests
{
    public class TestSphinxSearch : ISearch
    {
        public IEnumerable<SearchResult> Search(string searchTerm)
        {
            List<SearchResult> results = new List<SearchResult>();

            results.Add(new SearchResult { AccommodationName = "one", Id = 1 });

            return results.AsEnumerable();
        }
    }


    [TestFixture]
    public class Class1
    {
        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For<ISearch>().Use<TestSphinxSearch>();
            });
        }

        [Test]
        public void TestResults()
        {
            var search = Waiter.GetInstance<ISearch>();

            Assert.AreEqual(1, search.Search("anything").Count());
        }
    }
}
