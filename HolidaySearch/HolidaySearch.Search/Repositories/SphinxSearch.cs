using Sphinx.Client.Commands.Search;
using Sphinx.Client.Connections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidaySearch.Search.Repositories
{
    public class SphinxSearch : ISearch
    {
        public IEnumerable<SearchResult> Search(string searchTerm)
        {
            List<SearchResult> searchResults = new List<SearchResult>();

            using (ConnectionBase connection = new PersistentTcpConnection(
                ConfigurationManager.AppSettings["SphinxHost"],
                Convert.ToInt32(ConfigurationManager.AppSettings["SphinxPort"])))
            {
                SearchQuery query = new SearchQuery(searchTerm);

                query.MatchMode = MatchMode.Extended2;

                query.Indexes.Add(ConfigurationManager.AppSettings["SphinxIndexName"]);
                query.Limit = Convert.ToInt32(ConfigurationManager.AppSettings["SphinxPageLimit"]);

                SearchCommand command = new SearchCommand(connection);
                command.QueryList.Add(query);

                command.Execute();

                foreach (SearchQueryResult result in command.Result.QueryResults)
                {
                    foreach (Match match in result.Matches)
                    {
                        searchResults.Add(new SearchResult
                        {
                            Age = Int32.Parse(match.AttributesValues["age"].GetValue().ToString()),
                            Id = match.DocumentId
                        });
                    }
                }
            }

            return searchResults.AsEnumerable();
        }
    }
}
