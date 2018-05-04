using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace LehmanSystemTransfer
{
    class DBConnectionFactory
    {
        /// <summary>
        /// Connection string
        /// </summary>
        static SqlConnection conn = new SqlConnection();
        static string connString = @"Data Source=DESKTOP-KDSSJNM\SQLEXPRESS;Initial Catalog=testDB; Integrated Security=true; Pooling=false;";

        /// <summary>
        ///ConnectionString commission to local DB;
        ///Return Value: IDBConnClass;
        ///Using: IDBConn Class;
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetConn()
        {
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn = new SqlConnection(connString);
                conn.Open();
            }
            return conn;
        }

        /// <summary>
        ///ConnectionString decommission to local DB;
        ///Return Value: IDBConnClass;
        ///Using: IDBConn Class;
        /// </summary>
        /// <returns></returns>
        public static IDbConnection EndConn()
        {
            if (conn.State != System.Data.ConnectionState.Closed)
            {
                conn.Close();
            }
            return conn;
        }
    }
}
