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
            var firstClause = true; // do we want to add an AND?

            // Add dates
            if (searchParameters.Dates != null && searchParameters.Dates.Any())
            {
                foreach (var date in searchParameters.Dates)
                {
                    searchQuery += (firstClause ? string.Empty : " AND ") + "availability = " + DaysSinceMillenium(date);
                    firstClause = false;
                }
            }

            // Add minimum rating
            if (searchParameters.MinimumRating.HasValue)
            {
                searchQuery += (firstClause ? string.Empty : " AND ") + "rating >= " + searchParameters.MinimumRating.Value;
                firstClause = false;
            }

            // Add free text
            if (!string.IsNullOrEmpty(searchParameters.Accomodation) || !string.IsNullOrEmpty(searchParameters.Location))
            {
                searchQuery += (firstClause ? string.Empty : " AND ") + "MATCH('";

                // One search box mode
                if (searchParameters.UseCombinedSearchFields)
                {
                    searchQuery += string.Join(" ", searchParameters.CombinedSearchFields).Trim();
                }
                else // Multiple search box mode
                {
                    var addSpace = false;   // do we want to add a space to the next search parameter

                    // Accomodation name
                    if (!string.IsNullOrEmpty(searchParameters.Accomodation))
                    {
                        searchQuery += "@accomodation_name " + searchParameters.Accomodation;
                        addSpace = true;
                    }

                    // Location
                    if (!string.IsNullOrEmpty(searchParameters.Location))
                    {
                        searchQuery += (addSpace ? " " : string.Empty) + "@location_name " + searchParameters.Location;
                    }
                }

                searchQuery += "')";
                firstClause = false;
            }

            if (firstClause)
            {
                return null;
            }

            var searchResults = new List<SearchResult>();

            // Connect to sphinx
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Sphinx"].ConnectionString))
            {
                connection.Open();

                var cmd = new MySqlCommand(searchQuery, connection);
                
                var r = cmd.ExecuteReader();

                // Get results
                while (r.Read())
                {
                    searchResults.Add(new SearchResult
                    {
                        Id = r.GetInt64(0),
                        AccommodationName = r.GetString(1),
                        Location = r.GetString(2),
                        Rating = r.GetInt16(3)
                    });
                }
            }

            return searchResults.AsEnumerable();
        }
    }
}
