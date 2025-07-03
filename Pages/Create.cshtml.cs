using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Shere_Market.Pages
{
    public class Create : PageModel
    {
        [BindProperty]
        public string name { get; set; } = "";
        [BindProperty]
        public int quantity { get; set; }
        [BindProperty]
        public decimal brokerage { get; set; }
        [BindProperty]
        public decimal price { get; set; }
        [BindProperty]
        public string tradeType { get; set; } = "";
        [BindProperty]
        public string createdAt { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
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
                    string sql = "INSERT INTO Shares (name,quantity,brokerage,price,balance,tradeType,createdAt)" + "VALUES (@name, @quantity, @brokerage,@price, @balance, @tradeType, GETDATE())";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.Parameters.AddWithValue("@brokerage", brokerage);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@balance", balance);
                        command.Parameters.AddWithValue("@tradeType", tradeType);

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