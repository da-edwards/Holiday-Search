using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidaySearch.Search
{
    public class SearchParameters
    {
        public string Accomodation { get; set; }

        public IList<string> AllSearchFields
        {
            get
            {
                return new List<string>
                {
                    Accomodation,
                    Location
                };
            }
        }

        public IList<DateTime> Dates { get; set; }

        public string Location { get; set; }
    }
}
