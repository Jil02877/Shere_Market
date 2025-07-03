using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Shere_Market.Pages;

public class IndexModel : PageModel
{
    public List<shareInfo> SharesList { get; set; } = [];
    public List<shareInfo> SearchResults { get; set; } = [];

    public List<shareInfo> SharesToDiaplay => SearchResults != null
    && SearchResults.Count > 0 ? SearchResults : SharesList;
    public void OnGet()
    {
        try
        {
            string connectionString = "Server=JILPATEL28\\SQLEXPRESS02;Database=Stocks;Trusted_Connection=True;TrustServerCertificate=Yes;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Shares ORDER BY Id DESC";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            shareInfo shareinfo = new shareInfo();
                            shareinfo.id = reader.GetInt32(0);
                            shareinfo.name = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            shareinfo.quantity = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                            shareinfo.brokerage = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3);
                            shareinfo.price = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4);
                            shareinfo.balance = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5);
                            shareinfo.tradeType = reader.IsDBNull(6) ? "" : reader.GetString(6);
                            shareinfo.createdAt = reader.IsDBNull(7) ? "" : reader.GetDateTime(7).ToString("MM/dd/yyyy");

                            SharesList.Add(shareinfo);
                        }

                    }
                }
            }

        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public void OnPost(string SearchPhrase, string TradeTypeFilter)
    {
        search(SearchPhrase, TradeTypeFilter);

    }

    private void search(string SearchPhrase, string TradeTypeFilter)
    {
        try
        {
            string connectionString = "Server=JILPATEL28\\SQLEXPRESS02;Database=Stocks;Trusted_Connection=True;TrustServerCertificate=Yes;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Shares WHERE 1 = 1";

                if (!string.IsNullOrEmpty(SearchPhrase) && !string.IsNullOrEmpty(TradeTypeFilter))
                {
                    sql += " AND (name LIKE @SearchPhrase OR tradeType = @TradeTypeFilter)";
                }
                else if (!string.IsNullOrEmpty(SearchPhrase))
                {
                    sql += " AND name LIKE @SearchPhrase";
                }
                else if (!string.IsNullOrEmpty(TradeTypeFilter))
                {
                    sql += " AND tradeType = @TradeTypeFilter";
                }

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (!string.IsNullOrEmpty(SearchPhrase))
                        command.Parameters.AddWithValue("@SearchPhrase", "%" + SearchPhrase + "%");

                    if (!string.IsNullOrEmpty(TradeTypeFilter))
                        command.Parameters.AddWithValue("@TradeTypeFilter", TradeTypeFilter);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            shareInfo shareinfo = new shareInfo
                            {
                                id = reader.GetInt32(0),
                                name = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                quantity = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                                brokerage = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                                balance = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                                price = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5),
                                tradeType = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                createdAt = reader.IsDBNull(7) ? "" : reader.GetDateTime(7).ToString("MM/dd/yyyy")
                            };
                            SharesList.Add(shareinfo);
                        }
                    }
                }
            }

        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Cannot search customer: " + ex.Message);
        }
    }
    public class shareInfo
    {
        public int id { get; set; }
        public string name { get; set; } = "";
        public int quantity { get; set; }
        public decimal brokerage { get; set; }
        public decimal balance { get; set; }
        public decimal price { get; set; }
        public string tradeType { get; set; } = "";
        public string createdAt { get; set; } = "";
    }
}
