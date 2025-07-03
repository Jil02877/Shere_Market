using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Shere_Market.Pages
{
    public class Edit : PageModel
    {
        [BindProperty]
        public int id { get; set; }
        [BindProperty]
        public string name { get; set; } = "";
        [BindProperty]
        public int quantity { get; set; }
        [BindProperty]
        public decimal brokerage { get; set; }
        [BindProperty]
        public decimal price { get; set; }
        [BindProperty]
        public decimal balance { get; set; }
        [BindProperty]
        public string tradeType { get; set; } = "";
        [BindProperty]
        public string createdAt { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        public void OnGet(int id)
        {
            string connectionString = "Server=JILPATEL28\\SQLEXPRESS02;Database=Stocks;Trusted_Connection=True;TrustServerCertificate=Yes;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Shares WHERE id = @id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            this.id = reader.GetInt32(0);
                            name = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            quantity = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                            brokerage = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3);
                            price = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4);
                            balance = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5);
                            tradeType = reader.IsDBNull(6) ? "" : reader.GetString(6);
                            createdAt = reader.IsDBNull(7) ? "" : reader.GetDateTime(7).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            Response.Redirect("/Index");
                        }
                    }
                }
            }
        }
        public void OnPost()
        {
            try
            {
                decimal balance = 0;

                if (tradeType.ToLower() == "buy")
                {
                    balance = (price + brokerage) * quantity;
                }
                else if (tradeType.ToLower() == "sell")
                {
                    balance = (price - brokerage) * quantity;
                }
                else
                {
                    ErrorMessage = "Invalid trade type. Please select either 'Buy' or 'Sell'.";
                    return;
                }
                string connectionString = "Server=JILPATEL28\\SQLEXPRESS02;Database=Stocks;Trusted_Connection=True;TrustServerCertificate=Yes;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Shares SET name=@name,quantity=@quantity,brokerage=@brokerage,price=@price,balance=@balance,tradeType=@tradeType WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.Parameters.AddWithValue("@brokerage", brokerage);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@balance", balance);
                        command.Parameters.AddWithValue("@tradeType", tradeType);
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = "Error: " + ex.Message;
            }
            Response.Redirect("/Index");
        }
    }
}