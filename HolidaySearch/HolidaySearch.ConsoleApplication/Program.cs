using HolidaySearch.Search;
using HolidaySearch.Search.Repositories;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidaySearch.ConsoleApplication
{
    class Program
    {
        private static ISearch _searchRepository;
        
        static Program()
        {
            ObjectFactory.Initialize(x =>
                {
                    x.For<ISearch>().Use<SphinxSearch>();
                });

            _searchRepository = ObjectFactory.GetInstance<ISearch>();
        }

        static void Main()
        {
            var searchResults = _searchRepository.Search("Dave").ToList();

            Console.WriteLine(searchResults.First().Age);
        }
    }
}
