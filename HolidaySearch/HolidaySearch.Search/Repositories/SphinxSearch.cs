using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidaySearch.Search.Repositories
{
    /// <summary>
    /// Sphinx search class
    /// </summary>
    public class SphinxSearch : ISearch
    {
        /// <summary>
        /// Returns the days since the millenium.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        /// TODO: move this to an extension method
        private int DaysSinceMillenium(DateTime date)
        {
            return (date.Date - new DateTime(2000, 1, 1).Date).Days;
        }

        /// <summary>
        /// Searches using the specified search parameters.
        /// </summary>
        /// <param name="searchParameters">The search parameters.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">SphinxIndexName</exception>
        public IEnumerable<SearchResult> Search(SearchParameters searchParameters)
        {
            var indexName = ConfigurationManager.AppSettings["SphinxIndexName"];

            if (string.IsNullOrEmpty(indexName))
            {
                throw new ArgumentNullException("SphinxIndexName");
            }

            var searchQuery = "SELECT * FROM " + indexName + " WHERE ";
            var firstClause = true;

            // Add dates
            if (searchParameters.Dates.Any())
            {
                foreach (var date in searchParameters.Dates)
                {
                    searchQuery += (firstClause ? string.Empty : " AND ") + "availability = " + DaysSinceMillenium(date);
                    firstClause = false;
                }
            }

            // Add free text
            if (!string.IsNullOrEmpty(searchParameters.Accomodation) || !string.IsNullOrEmpty(searchParameters.Location))
            {
                searchQuery += (firstClause ? string.Empty : " AND ") + "MATCH('";

                var addSpace = false;   // do we want to add a space to the next search parameter

                // Accomodation name
                if (!string.IsNullOrEmpty(searchParameters.Accomodation))
                {
                    searchQuery += "@accomodation_name " + searchParameters.Accomodation;
                    addSpace = true;
                }

                // Location name
                if (!string.IsNullOrEmpty(searchParameters.Location))
                {
                    searchQuery += (addSpace ? " " : string.Empty) + "@location_name " + searchParameters.Location;
                }

                searchQuery += "')";
            }
            
            var searchResults = new List<SearchResult>();

            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Sphinx"].ConnectionString))
            {
                connection.Open();

                var cmd = new MySqlCommand(searchQuery, connection);
                
                var r = cmd.ExecuteReader();

                while (r.Read())
                {
                    searchResults.Add(new SearchResult
                    {
                        Id = r.GetInt64(0),
                        AccommodationName = r.GetString(1)
                    });
                }
            }

            ////using (ConnectionBase connection = new PersistentTcpConnection(
            ////    ConfigurationManager.AppSettings["SphinxHost"],
            ////    Convert.ToInt32(ConfigurationManager.AppSettings["SphinxPort"])))
            ////{
                

            ////    ////SearchQuery query = new SearchQuery(searchTerm);
            ////    ////////query.AttributeFilters.Add("availability", new List<int> { 9391 }, true);
            ////    ////query.AttributeFilters.Add(new Sphinx.Client.Commands.Attributes.Filters.AttributeFilterBase
            ////    ////    {
            ////    ////        FilterType = AttributeFilterType.RangeInt32,
            ////    ////        Exclude = true,
            ////    ////        Name = "availability"
            ////    ////    });

            ////    ////query.MatchMode = MatchMode.Extended2;

            ////    ////query.Indexes.Add(ConfigurationManager.AppSettings["SphinxIndexName"]);
            ////    ////query.Limit = Convert.ToInt32(ConfigurationManager.AppSettings["SphinxPageLimit"]);

            ////    ////SearchCommand command = new SearchCommand(connection);
            ////    ////command.QueryList.Add(query);

            ////    ////try
            ////    ////{
            ////    ////    command.Execute();

            ////    ////    foreach (SearchQueryResult result in command.Result.QueryResults)
            ////    ////    {
            ////    ////        foreach (Match match in result.Matches)
            ////    ////        {
            ////    ////            searchResults.Add(new SearchResult
            ////    ////            {
            ////    ////                AccommodationName = match.AttributesValues["s_accommodation_name"].GetValue().ToString(),
            ////    ////                Id = match.DocumentId
            ////    ////            });
            ////    ////        }
            ////    ////    }
            ////    ////}
            ////    ////catch (Sphinx.Client.Common.QueryErrorException)
            ////    ////{
            ////    ////    throw;
            ////    ////    //TODO: log
            ////    ////}
                
            ////}

            return searchResults.AsEnumerable();
        }
    }
}
