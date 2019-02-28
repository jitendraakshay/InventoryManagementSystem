using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DomainEntities;
using DomainInterface;
using System.Globalization;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace DomainRepository
{
    public class DashboardRepo : IDashboardRepo
    {
        IOptions<ReadConfig> connectionString;
        public DashboardRepo(IOptions<ReadConfig> _connectionString)
        {
            connectionString = _connectionString;
        }

        //public DashboardStatistics DashboardStatisticsGet()
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
        //        {

        //            DashboardStatistics dashboardStats = SqlMapper.Query<DashboardStatistics>(connection, "[dbo].[usp_DashboardStatisticsGetSingle]", commandType: CommandType.StoredProcedure).FirstOrDefault();
        //            return dashboardStats;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public List<TicketStatistics> TicketStatisticsGet()
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
        //        {
        //            List<TicketStatistics> ticketStats = SqlMapper.Query<TicketStatistics>(connection, "[dbo].[usp_TicketStatisticsGetAll]", commandType: CommandType.StoredProcedure).ToList() ;
        //            return ticketStats;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public List<TicketStatistics> TicketStatisticsGetByFiscalYearCode(string fiscalYearCode)
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
        //        {
        //            DynamicParameters param = new DynamicParameters();
        //            param.Add("@FiscalYearCode", fiscalYearCode);
        //            List<TicketStatistics> ticketStats = SqlMapper.Query<TicketStatistics>(connection, "[dbo].[usp_TicketStatisticsGetByFiscalYearCode]", param, commandType: CommandType.StoredProcedure).ToList();
        //            return ticketStats;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        //public TicketStatistics GetDashboardCollectionStatus(string fiscalYearCode)
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
        //        {
        //            List<TicketStatistics> ticketStats=new List<TicketStatistics>();
        //            DynamicParameters param = new DynamicParameters();
        //            DynamicParameters param1 = new DynamicParameters();
        //            param.Add("@FiscalYearCode", fiscalYearCode);
        //            //List<TicketStatistics> ticketStats = SqlMapper.Query<TicketStatistics>(connection, "[dbo].[usp_TicketStatisticsGetByFiscalYearCode]", param, commandType: CommandType.StoredProcedure).ToList();
        //            ticketStats = SqlMapper.Query<TicketStatistics>(connection, "[dbo].[usp_TicketStatisticsGetByFiscalYearCode]", param, commandType: CommandType.StoredProcedure).ToList();
        //            decimal TicketSold = 0;
        //            decimal NetCollection = 0;
        //            decimal GrossCollection = 0;
        //            decimal TotalTax = 0;
        //            decimal VAT = 0;
        //            decimal FDF = 0;
        //            decimal SLET = 0;
        //            foreach (TicketStatistics list in ticketStats)
        //            {
        //                TicketSold = TicketSold+list.TicketCounts;
        //                NetCollection = NetCollection+list.NetCollection;
        //                GrossCollection = GrossCollection+list.GrossCollection;
        //                TotalTax = TotalTax+list.TotalTax;
        //                VAT = VAT + list.VAT;
        //                FDF = FDF + list.FDF;
        //                SLET = SLET + list.SLET;
        //            }

        //            TicketStatistics objStat = new TicketStatistics();
        //            objStat.TotalTax = Convert.ToDecimal(String.Format("{0:0.00}", TotalTax)); 
        //            objStat.NetCollection = Convert.ToDecimal(String.Format("{0:0.00}", NetCollection)); 
        //            objStat.GrossCollection = Convert.ToDecimal(String.Format("{0:0.00}", GrossCollection)); 
        //            objStat.TicketSold = Convert.ToDecimal(String.Format("{0:0.00}", TicketSold)); 
        //            objStat.VAT = Convert.ToDecimal(String.Format("{0:0.00}", VAT)); 
        //            objStat.SLET = Convert.ToDecimal(String.Format("{0:0.00}", SLET)); 
        //            objStat.FDF = Convert.ToDecimal(String.Format("{0:0.00}", FDF)); 

        //            //NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        //            //display the physical address of the first nic in the array,
        //            //which should correspond to our mac address
        //            //string x= nics[0].GetPhysicalAddress().ToString();



        //            //foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        //            //{
        //            //    foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
        //            //    {
                            
        //            //            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        //            //            {
        //            //                string y=ip.Address.ToString();
        //            //            }
                            
        //            //    }
        //            //}

        //            return objStat;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}
        //public List<BoxOfficeCollection> GetBoxOfficeCollection()
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString.Value.DefaultConnection))
        //        {
        //            List<BoxOfficeCollection> collection = new List<BoxOfficeCollection>();
        //            List<BoxOfficeCollection> boxoffice = new List<BoxOfficeCollection>();
        //            collection = SqlMapper.Query<BoxOfficeCollection>(connection, "Report.usp_GetMoviewiseNetTotal", commandType: CommandType.StoredProcedure).ToList();
        //            foreach(BoxOfficeCollection data in collection)
        //            {
        //                string sum =  data.NetTotal.ToString("#,##0");
        //                data.Total = sum;
        //                boxoffice.Add(data);
        //            }
        //            return boxoffice;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}
    }
}
