using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolidaySearch.Website.ViewModels
{
    public class BasicSearchViewModel
    {
        public short Nights { get; set; }

        public string SearchTerm { get; set; }

        public DateTime StartDate { get; set; }
    }
}