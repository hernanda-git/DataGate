# DataGate

A Simplified SQL Transaction for C# using System.Data.SqlClient version=4.6.1.2

you can use it as a class library for example:

using DataGate;

namespace YourProject{
    public class YourController
    {
      string sqlCmd = "SELECT * FROM [dbo].[Transaction]";
      using (SqlConnection con = new SqlConnection(YourConectionString))
      {
        SqlDataReader reader = (DataGate.Transact.ExecuteReader(sqlCmd, con));
      }
    }
}
