using MessengerHealth.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MessengerHealth.Data
{
    public static class DataAccess
    {
        public static List<ServiceOperation> GetServiceOperationsList()
        {
            List<ServiceOperation> serviceOperationList = new List<ServiceOperation>();
            string connection = ConfigurationManager.AppSettings["connectionString"];
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string queryString = @"SELECT 
                                        MAX(ARICREATEDTTM) LastOperationDate, 
	                                    ARISERVICECODE Service,
                                        IIF(ARIOPERATIONCODE like 'Resp%', 'Resp', 'Send') Operation
                                    FROM 
                                        ARCHIVEITEM
                                    WHERE
                                        ARICREATEDTTM >= DATEADD(DAY, -1, GETDATE()) 
                                        AND ARISTATUS = 'OK'
                                        AND ARISERVICECODE IS NOT NULL
                                        GROUP BY ARISERVICECODE, IIF(ARIOPERATIONCODE like 'Resp%', 'Resp', 'Send')";

                SqlCommand oCmd = new SqlCommand(queryString, con);

                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        ServiceOperation service = new ServiceOperation();
                        service.LastOperationDate = Convert.ToDateTime(oReader["LastOperationDate"]);
                        service.Service = oReader["Service"].ToString();
                        service.Operation = oReader["Operation"].ToString();
                        serviceOperationList.Add(service);
                    }
                    con.Close();
                }
            }
            return serviceOperationList;
        }
    }
}