using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataGate.Model
{
    public class Common
    {
        public static string BeginTrans() => "SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL, XACT_ABORT ON\r\nSET NUMERIC_ROUNDABORT OFF\r\nBEGIN TRANSACTION\r\n";
        public static string CommitTrans() => "\r\nIF @@ERROR <> 0\r\nBEGIN\r\nIF @@TRANCOUNT = 1\r\nROLLBACK TRANSACTION\r\nCOMMIT TRANSACTION\r\nEND\r\nELSE\r\nBEGIN\r\nIF @@TRANCOUNT = 1\r\nBEGIN\r\nCOMMIT TRANSACTION\r\nEND\r\nEND\r\n";
        public static string ExecuteScalar() => "\r\n\r\nDECLARE @ID AS INT\r\nSET @ID = 0\r\nIF @@ERROR <> 0\r\nBEGIN\r\nIF @@TRANCOUNT = 1\r\nROLLBACK TRANSACTION\r\nCOMMIT TRANSACTION\r\nEND\r\nELSE\r\nBEGIN\r\nIF @@TRANCOUNT = 1\r\nBEGIN\r\nCOMMIT TRANSACTION\r\nSELECT @ID = CAST(SCOPE_IDENTITY() AS INT)\r\nEND\r\nEND\r\nSELECT @ID AS InsertID\r\n";
        public static void FillDataSet(DataSet DS, string CommandString, SqlConnection Connection)
        {
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(CommandString, Connection))
            {
                try
                {
                    sqlDataAdapter.Fill(DS);
                }
                catch
                {
                    DS = (DataSet)null;
                }
            }
        }
    }
}
