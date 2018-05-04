using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

///Extensions
using Dapper;
using System.Data.SqlClient;
using LehmanSystemTransfer.Models;

namespace LehmanSystemTransfer
{
    class FTP2DBServices
    {
        /// <summary>
        /// Variables
        /// </summary>
        string directory = "C:/Users/Sim Kim Wee/Google Drive/Fincast/BLOOMBERG";
        List<BloomBergRawDataDto> DictionList = new List<BloomBergRawDataDto>();

        /// <summary>
        /// Checks for selected movements of files in a directory
        /// </summary>
        /// <returns></returns>
        public void CheckUpdate()
        {

            try
            {
                // Create a new FileSystemWatcher and set its properties.
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = directory;

                /* Watch for changes in LastAccess and LastWrite times, and
                the renaming of files or directories. */
                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;

                // Files to take note if actions have been applied to those.
                watcher.Filter = "*.csv";

                // Add event handlers.
                watcher.Created += new FileSystemEventHandler(OnCreated);

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                Console.WriteLine("Type 'Quit' to quit the sample.");
                while (Console.ReadLine() != "Quit") ;
            }
            catch (Exception ex) {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }

        }

        /// <summary>
        /// OnCreated Event Handler
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void OnCreated(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("File Created: {0}", Path.GetFileName(e.FullPath));
            //New instance created due to static implementation with this method itself.
            FTP2DBServices FTPSDBervice = new FTP2DBServices();
            FTPSDBervice.DataRaw2Dictionary(e.FullPath);
            FTPSDBervice.FileRecordCreation(e.FullPath);
            FTPSDBervice.Dictionary2System();
            FTPSDBervice.DataBaseFiltering();
        }

        /// <summary>
        /// Conversion of xml/csv to DataSet
        /// </summary>
        private void DataRaw2Dictionary(string pathName)
        {
            if (File.Exists(pathName)) {
                var csv = File.ReadAllText(pathName);
                var lines = csv.Split('\n');
                foreach (var line in lines) {
                    var cols = line.Split(',');
                    var bloomBergRawData = new BloomBergRawDataDto
                    {
                        PrcDate = cols[0],
                        FullName = cols[1],
                        idIndex = cols[2],
                        NumIssuesB = cols[3],
                        NumIssuesF = cols[4],
                        Currency = cols[5],
                        RetType = cols[6],
                        Coupon = cols[7],
                        Maturity = cols[8],
                        Quality = cols[9],
                        Price = cols[10],
                        MarketValue = cols[11],
                        YieldWorst = cols[12],
                        YieldMaturity = cols[13],
                        DurationModF = cols[14],
                        DurationModWorst = cols[15],
                        DurationWorstF = cols[16],
                        Convexity = cols[17],
                        OAS = cols[18],
                        DurationModB = cols[19],
                        RetDaily = cols[20],
                        RetMTD = cols[21],
                        Ret3M = cols[22],
                        Ret6M = cols[23],
                        Ret12M = cols[24],
                        RetYTD = cols[25],
                        RetMTDPrice = cols[26],
                        RetMTDCoupon = cols[27],
                        RetMTDPaydown = cols[28],
                        RetMTDCurrency = cols[29],
                        ExcessRet = cols[30],
                        ExcessRet3M = cols[31],
                        ExcessRet6M = cols[32],
                        ExcessRet12M = cols[33],
                        ExcessRetYTD = cols[34],
                        RetInception = cols[35],
                        IndexValue = cols[36]
                    };
                    DictionList.Add(bloomBergRawData);
                }
            }   
            Console.WriteLine("Data Transfer Completed");
        }

        /// <summary>
        /// Algorithm to appebd data from List<List<string>> to Database
        /// </summary>
        private void Dictionary2System()
        {
            //Algorithm to Copy to DB
            using (var conn = DBConnectionFactory.GetConn())
            {
                int item = 0;
                while (item < DictionList.Count)
                {
                    BloomBergRawDataDto currentItem = DictionList[item];
                    using (var tra = conn.BeginTransaction())
                    {
                        try
                        {
                            var ResultList = conn.Execute(
                                @"INSERT INTO dbo.BloomBergRawData (
                                    PrcDate,FullName,idIndex,
                                    NumIssuesB,NumIssuesF,Currency,
                                    RetType,Coupon,Maturity,Quality,
                                    Price,MarketValue,YieldWorst,
                                    YieldMaturity,DurationModF,DurationModWorst,
                                    DurationWorstF,Convexity,OAS,DurationModB,
                                    RetDaily,RetMTD,Ret3M,Ret6M,Ret12M,
                                    RetYTD,RetMTDPrice,RetMTDCoupon,RetMTDPaydown,
                                    RetMTDCurrency,ExcessRet,ExcessRet3M,
                                    ExcessRet6M,ExcessRet12M,ExcessRetYTD,RetInception,IndexValue) 
                                    VALUES 
                                    (@PrcDate,@FullName,@idIndex,
                                    @NumIssuesB,@NumIssuesF,@Currency,@RetType,
                                    @Coupon,@Maturity,@Quality,@Price,
                                    @MarketValue,@YieldWorst,
                                    @YieldMaturity,@DurationModF,
                                    @DurationModWorst,@DurationWorstF,@Convexity,
                                    @OAS,@DurationModB,@RetDaily,
                                    @RetMTD,@Ret3M,@Ret6M,@Ret12M,
                                    @RetYTD,@RetMTDPrice,
                                    @RetMTDCoupon,@RetMTDPaydown,
                                    @RetMTDCurrency,@ExcessRet,@ExcessRet3M,
                                    @ExcessRet6M,@ExcessRet12M,
                                    @ExcessRetYTD,@RetInception,@IndexValue);",
                            new
                            {
                                PrcDate = currentItem.PrcDate,
                                FullName = currentItem.FullName,
                                idIndex = currentItem.idIndex,
                                NumIssuesB = currentItem.NumIssuesB,
                                NumIssuesF = currentItem.NumIssuesF,
                                Currency = currentItem.Currency,
                                RetType = currentItem.RetType,
                                Coupon = currentItem.Coupon,
                                Maturity = currentItem.Maturity,
                                Quality = currentItem.Quality,
                                Price = currentItem.Price,
                                MarketValue = currentItem.MarketValue,
                                YieldWorst = currentItem.YieldWorst,
                                YieldMaturity = currentItem.YieldMaturity,
                                DurationModF = currentItem.DurationModF,
                                DurationModWorst = currentItem.DurationModWorst,
                                DurationWorstF = currentItem.DurationWorstF,
                                Convexity = currentItem.Convexity,
                                OAS = currentItem.OAS,
                                DurationModB = currentItem.DurationModB,
                                RetDaily = currentItem.RetDaily,
                                RetMTD = currentItem.RetMTD,
                                Ret3M = currentItem.Ret3M,
                                Ret6M = currentItem.Ret6M,
                                Ret12M = currentItem.Ret12M,
                                RetYTD = currentItem.RetYTD,
                                RetMTDPrice = currentItem.RetMTDPrice,
                                RetMTDCoupon = currentItem.RetMTDCoupon,
                                RetMTDPaydown = currentItem.RetMTDPaydown,
                                RetMTDCurrency = currentItem.RetMTDCurrency,
                                ExcessRet = currentItem.ExcessRet,
                                ExcessRet3M = currentItem.ExcessRet3M,
                                ExcessRet6M = currentItem.ExcessRet6M,
                                ExcessRet12M = currentItem.ExcessRet12M,
                                ExcessRetYTD = currentItem.ExcessRetYTD,
                                RetInception = currentItem.RetInception,
                                IndexValue = currentItem.IndexValue
                            }, tra);
                            tra.Commit();
                        }
                        catch (Exception ex)
                        {
                            if (ex is SqlException)
                            {
                                tra.Rollback();
                            }
                        }
                    }
                    item++;
                    Console.WriteLine("Updating: " + item);
                }
                DBConnectionFactory.EndConn();
                Console.WriteLine("Pass DataBase Transfer Function");
            }
        }

        /// <summary>
        /// DataBaseFiltering via Dapper Stored Procedure
        /// </summary>
        private void DataBaseFiltering()
        {
            //Established DB connections
            using (var conn = DBConnectionFactory.GetConn())
            {
                //Run Stored procedure for filter
                var affectedRows1 = conn.Execute("SP_FILTERHedgedAUD",
                commandType: CommandType.StoredProcedure);

                var affectedRows2 = conn.Execute("SP_FILTERUnhedgedAUD",
                commandType: CommandType.StoredProcedure);

                var affectedRows3 = conn.Execute("SP_FILTERUnhedgedOthers",
                commandType: CommandType.StoredProcedure);
            }
            DBConnectionFactory.EndConn();
            Console.WriteLine("Pass DataBase Transfer Function");
        }

        private void FileRecordCreation(string pathName)
        {
            //Algorithm to Copy to DB
            using (var conn = DBConnectionFactory.GetConn())
            {
                using (var tra = conn.BeginTransaction())
                {
                    try
                    {
                        var sql = "INSERT INTO [dbo].[FileRecord] ([DateStampUTC],[Description]) VALUES (GETutcDATE(), @Desc)";
                        conn.Query<FileRecord>(sql, new { Desc = Path.GetFileName(pathName)}, tra);
                        tra.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (ex is SqlException)
                        {
                            tra.Rollback();
                            throw ex;
                        }
                    }
                }
                conn.Close();
            }
        }
    }
}

