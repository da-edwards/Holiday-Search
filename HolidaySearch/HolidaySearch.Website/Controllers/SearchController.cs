using HolidaySearch.IOC;
using HolidaySearch.Search;
using HolidaySearch.Search.Repositories;
using HolidaySearch.Website.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HolidaySearch.Website.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearch _searchRepository;

        public SearchController()
        {
            _searchRepository = Waiter.GetInstance<ISearch>();
        }

        /// <summary>
        /// Returns the search form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new BasicSearchViewModel
            {
                Nights = 1,
                StartDate = DateTime.Now.AddDays(1)
            };

            return View(viewModel);
        }

        /// <summary>
        /// Returns the search results
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Results(BasicSearchViewModel model)
        {
            var viewModel = new SearchResultsViewModel
            {
                BasicSearchViewModel = model
            };

            var resultList = new List<SearchResultViewModel>();

            // Get the search results
            var searchResults = _searchRepository.Search(
                new SearchParameters
                {
                    Accomodation = model.SearchTerm,
                    Dates = model.StartDate == default(DateTime) ? null : new List<DateTime> { model.StartDate },
                    NumberOfNights = model.Nights,
                    UseCombinedSearchFields = true
                });

            // If not null, get them
            if (searchResults != null)
            {
                foreach (var searchResult in searchResults)
                {
                    resultList.Add(new SearchResultViewModel
                    {
                        AccommodationName = searchResult.AccommodationName,
                        Id = searchResult.Id
                    });
                }
            }

            viewModel.SearchResults = resultList.AsEnumerable();

            return View(viewModel);
        }
    }
}