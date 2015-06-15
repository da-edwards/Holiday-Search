using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidaySearch.Search
{
    public class SearchResult
    {
        public string AccommodationName { get; internal set; }

        public long Id { get; internal set; }

        public string Location { get; internal set; }

        public short Rating { get; internal set; }
    }
}
