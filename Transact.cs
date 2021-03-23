using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Text;
using DataGate.Model;
using Newtonsoft.Json;

namespace DataGate
{
    public sealed class Transact
    {
        public static int ExecuteScalar(string CommandString, SqlConnection Connection)
        {
            int result = 0;
            StringBuilder stringBuilder = new StringBuilder(Common.BeginTrans() + CommandString + Common.ExecuteScalar());
            try { using (SqlCommand sqlCommand = new SqlCommand(stringBuilder.ToString(), Connection)) result = (int)RuntimeHelpers.GetObjectValue(sqlCommand.ExecuteScalar()); } catch { }
            return result;
        }

        public static int ExecuteScalar(string CommandString, SqlConnection Connection, List<SqlParameter> parameters = null)
        {
            int result = 0;
            StringBuilder stringBuilder = new StringBuilder(Common.BeginTrans() + CommandString + Common.ExecuteScalar());
            try
            {
                if (parameters.Equals(null))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(stringBuilder.ToString(), Connection))
                    {
                        sqlCommand.Parameters.AddRange(parameters.ToArray());
                        result = (int)RuntimeHelpers.GetObjectValue(sqlCommand.ExecuteScalar());
                    }
                }
                else
                {
                    using (SqlCommand sqlCommand = new SqlCommand(stringBuilder.ToString(), Connection))
                    {
                        result = (int)RuntimeHelpers.GetObjectValue(sqlCommand.ExecuteScalar());
                    }
                }
            }
            catch { }
            return result;
        }

        public static bool ExecuteNonQuery(string CommandString, SqlConnection Connection)
        {
            bool result = true;
            StringBuilder stringBuilder = new StringBuilder(Common.BeginTrans() + CommandString + Common.CommitTrans());
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(stringBuilder.ToString(), Connection))
                {
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch { result = false; }
            return result;
        }

        public static bool ExecuteNonQuery(string CommandString, SqlConnection Connection, List<SqlParameter> parameters = null)
        {
            bool result = true;
            StringBuilder stringBuilder = new StringBuilder(Common.BeginTrans() + CommandString + Common.CommitTrans());
            try
            {
                if (parameters.Equals(null))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(stringBuilder.ToString(), Connection))
                    {
                        sqlCommand.Parameters.AddRange(parameters.ToArray());
                        sqlCommand.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (SqlCommand sqlCommand = new SqlCommand(stringBuilder.ToString(), Connection))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch { result = false; }
            return result;
        }

        public static SqlDataReader ExecuteReader(string CommandString, SqlConnection Connection)
        {
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(CommandString, Connection))
                {
                    return sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch { return (SqlDataReader)null; }
        }

        public static bool CheckExist(string TblName, string Column, string Condition, SqlConnection Connection)
        {
            bool result = false;
            string cmdText = $"SELECT ISNULL(COUNT({Column}),0) AS COUNT FROM {TblName} {Condition};";
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(cmdText, Connection))
                {
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
                    if (sqlDataReader.Read())
                    {
                        if (Convert.ToInt32(sqlDataReader["COUNT"].ToString()) > 0)
                            result = true;
                    }
                }
            }
            catch { result = true; }
            return result;
        }

        public static DataSet DataSet(string CommandString, SqlConnection Connection)
        {
            DataSet DS = new DataSet();
            Common.FillDataSet(DS, CommandString, Connection);
            return DS;
        }

        public static int UpdateTable(string TableName, string UpdateSyntax, string ParamSyntax, SqlConnection Connection)
        {
            int result = 0;
            try
            {
                using (SqlConnection connection = Connection)
                {
                    using (SqlCommand sqlCommand = new SqlCommand($"UPDATE {TableName} SET {UpdateSyntax} WHERE {ParamSyntax}", connection))
                    {
                        result = sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch { }
            return result;
        }

        public static string MapSingleRecord(string TableName, string Conditions, SqlConnection Connection, string ColumnName = "*")
        {
            string result = string.Empty;
            try
            {
                using (SqlConnection connection = Connection)
                {
                    using (SqlCommand sqlCommand = new SqlCommand($"SELECT TOP 1 {ColumnName} FROM {TableName} WITH (NOLOCK) WHERE {Conditions}", connection))
                    {
                        SqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
                        if (reader.Read())
                        {
                            DataTable table = new DataTable();
                            table.Load((IDataReader)reader);

                            if (table.Rows.Count > 0)
                                result = JsonConvert.SerializeObject((object)table.Rows[0].Table);
                        }
                    }
                }
            }
            catch { }
            return result;
        }
    }
}
