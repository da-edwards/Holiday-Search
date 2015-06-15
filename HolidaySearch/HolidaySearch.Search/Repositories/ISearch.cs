using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidaySearch.Search.Repositories
{
    public interface ISearch
    {
        IEnumerable<SearchResult> Search(SearchParameters searchParameters);
    }
}
