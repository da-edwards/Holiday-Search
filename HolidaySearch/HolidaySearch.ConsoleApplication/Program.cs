using HolidaySearch.IOC;
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
        private static readonly ISearch _searchRepository;
        
        static Program()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For<ISearch>().Use<SphinxSearch>();
            });

            _searchRepository = Waiter.GetInstance<ISearch>();
        }

        static void Main()
        {
            var searchResults = _searchRepository.Search(
                new SearchParameters
                {
                    Accomodation = "dave",
                    Dates = new List<DateTime> { DateTime.Now.AddDays(1) },
                    UseCombinedSearchFields = true
                }).ToList();

            if (!searchResults.Any())
            {
                Console.WriteLine("No results found");

                Environment.Exit(0);
            }

            foreach (var searchResult in searchResults)
            {
                Console.WriteLine(searchResult.AccommodationName);
            }
        }
    }
}
