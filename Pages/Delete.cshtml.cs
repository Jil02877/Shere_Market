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
    public class Delete : PageModel
    {
        public void OnGet(int id)
        {
            deleteData(id);
            Response.Redirect("/Index");
        }
        public void OnPost(int id)
        {
            deleteData(id);
            Response.Redirect("/Index");
        }

        private void deleteData(int id)
        {
            try
            {
                string connectionString = "Server=JILPATEL28\\SQLEXPRESS02;Database=Stocks;Trusted_Connection=True;TrustServerCertificate=Yes;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "DELETE FROM Shares WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception ex)
            {
                 Console.WriteLine("Cannot delete customer: " + ex.Message);
            }
        }
    }
}