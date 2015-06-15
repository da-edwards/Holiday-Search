﻿using HolidaySearch.IOC;
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

        [HttpGet]
        public ActionResult Results(BasicSearchViewModel model)
        {
            var viewModel = new SearchResultsViewModel
            {
                BasicSearchViewModel = model
            };

            ISearch searchRepository = Waiter.GetInstance<ISearch>();

            var resultList = new List<SearchResultViewModel>();

            foreach (var searchResult in searchRepository.Search(model.SearchTerm))
            {
                resultList.Add(new SearchResultViewModel
                {
                    AccommodationName = searchResult.AccommodationName,
                    Id = searchResult.Id
                });
            }

            viewModel.SearchResults = resultList.AsEnumerable();

            return View(viewModel);
        }
    }
}