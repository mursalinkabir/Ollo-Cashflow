using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Ollo_Cashflow.Models;

namespace Ollo_Cashflow.Core.Gateway
{
    public class ReportGateway
    {
        private string connectionString =
            WebConfigurationManager.ConnectionStrings["CFDBConnection"].ConnectionString;

        SqlConnection connection = new SqlConnection();
        public bool ProcessReportData()
        {
           bool isBankdataprocessed= ProcessBankData();
           bool isUnclearedChequeProcessed = ProcessUnclearCheque();
           return isBankdataprocessed;
        }

        private bool ProcessUnclearCheque()
        {
            connection.ConnectionString = connectionString;
            DateTime now = DateTime.Now;
            string cperiod = now.Year.ToString() + "-" + now.Month.ToString();
            //getting data from one table 
            string query1 = @"SELECT Z_Cheques.ChequeNo, R_BankStat.CheckNo, Z_Cheques.CheckDate, Z_Cheques.Amount, Z_Cheques.Counterparty, Z_Cheques.Source_Name, R_BankStat.CashSource
FROM R_BankStat RIGHT JOIN Z_Cheques ON (R_BankStat.CashSource = Z_Cheques.Source_Name) AND (R_BankStat.CheckNo = Z_Cheques.ChequeNo)
WHERE (((R_BankStat.CheckNo) Is Null))
ORDER BY Z_Cheques.ChequeNo, Z_Cheques.CheckDate;";
            SqlCommand command1 = new SqlCommand(query1, connection);

            
            connection.Open();
            SqlDataReader reader = command1.ExecuteReader();

            List<Report> UnclearedChequeReportList = new List<Report>();


            while (reader.Read())
            {
                Report report = new Report();

                report.Source = reader["Source_Name"].ToString();
                report.Amount = Convert.ToDouble(reader["Amount"].ToString());
                report.Period = reader["CheckDate"].ToString();
                report.DataType = "UC";

                UnclearedChequeReportList.Add(report);
            }
            reader.Close();
            connection.Close();

            int rowAffected = 0;
            foreach (Report report in UnclearedChequeReportList)
            {



                string query2 = @"INSERT INTO TreasuryReport(Source,Amount,DType,SourcePeriod,CurrentPeriod) VALUES(@Source,@Amount,@DType,@SourcePeriod,@CPeriod)";
                SqlCommand command2 = new SqlCommand(query2, connection);


                command2.Parameters.Clear();
                command2.Parameters.Add("Source", SqlDbType.NVarChar);
                command2.Parameters["Source"].Value = report.Source;
                command2.Parameters.Add("Amount", SqlDbType.Float);
                command2.Parameters["Amount"].Value = report.Amount;

                command2.Parameters.Add("DType", SqlDbType.NVarChar);
                command2.Parameters["DType"].Value = report.DataType;
                command2.Parameters.Add("SourcePeriod", SqlDbType.NVarChar);
                command2.Parameters["SourcePeriod"].Value = report.Period;
                command2.Parameters.Add("CPeriod", SqlDbType.NVarChar);
                command2.Parameters["CPeriod"].Value = cperiod;


                connection.Open();
                rowAffected = rowAffected + command2.ExecuteNonQuery();
                connection.Close();
            }



            if (rowAffected > 0)
            {
                return true;
            }
            return false;


           
        }

        private bool ProcessBankData()
        {

            connection.ConnectionString = connectionString;
            DateTime now = DateTime.Now;
            string cperiod = now.Year.ToString() + "-" + now.Month.ToString();
            //getting data from one table 
            string query1 = @"SELECT CashSource,Closing,Period FROM B_BankTB where Period =@CPeriod AND CashSource NOT IN('TDS','VDS');";
            SqlCommand command1 = new SqlCommand(query1, connection);

            command1.Parameters.Clear();
            command1.Parameters.Add("CPeriod", SqlDbType.NVarChar);
            command1.Parameters["CPeriod"].Value = "2016-08";
            connection.Open();
            SqlDataReader reader = command1.ExecuteReader();

            List<Report> BankReportList = new List<Report>();


            while (reader.Read())
            {
                Report report = new Report();

                report.Source = reader["CashSource"].ToString();
                report.Amount = Convert.ToDouble(reader["Closing"].ToString());
                report.Period = reader["Period"].ToString();
                report.DataType = "BB";

                BankReportList.Add(report);
            }
            reader.Close();
            connection.Close();




            int rowAffected=0;
            foreach (Report report in BankReportList)
            {



                string query2 = @"INSERT INTO TreasuryReport(Source,Amount,DType,SourcePeriod,CurrentPeriod) VALUES(@Source,@Amount,@DType,@SourcePeriod,@CPeriod)";
                SqlCommand command2 = new SqlCommand(query2, connection);
                

                command2.Parameters.Clear();
                command2.Parameters.Add("Source", SqlDbType.NVarChar);
                command2.Parameters["Source"].Value = report.Source;
                command2.Parameters.Add("Amount", SqlDbType.Float);
                command2.Parameters["Amount"].Value = report.Amount;

                command2.Parameters.Add("DType", SqlDbType.NVarChar);
                command2.Parameters["DType"].Value = report.DataType;
                command2.Parameters.Add("SourcePeriod", SqlDbType.NVarChar);
                command2.Parameters["SourcePeriod"].Value = report.Period;
                command2.Parameters.Add("CPeriod", SqlDbType.NVarChar);
                command2.Parameters["CPeriod"].Value = cperiod;


                connection.Open();
                 rowAffected =rowAffected+ command2.ExecuteNonQuery();
                connection.Close();
            }
          
            
            
            if (rowAffected>0)
            {
                return true;
            }
            return false;
        }
    }
}