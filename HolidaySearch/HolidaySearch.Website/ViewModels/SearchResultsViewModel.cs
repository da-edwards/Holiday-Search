using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolidaySearch.Website.ViewModels
{
    public class SearchResultsViewModel
    {
        public BasicSearchViewModel BasicSearchViewModel { get; set; }

        public IEnumerable<SearchResultViewModel> SearchResults { get; set; }
    }
}