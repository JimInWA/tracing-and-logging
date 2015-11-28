using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoapRequestAndResponseTracing.Test.Framework
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Provides helper methods for the tests
    /// </summary>
    public class TestHelper
    {
        /// <summary>
        /// GetAppSettingsKey will return the string value of a particular AppSettings key
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public string GetAppSettingsKey(string keyName)
        {
            var result = (string.IsNullOrWhiteSpace(keyName)) ? null : ConfigurationManager.AppSettings[keyName];

            return result;
        }

        /// <summary>
        /// BuildSqlSelectStatement - for the tests that need to verify data written to the SampleLogging Database, SoapRequestAndResponseTracingBase table 
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="isRequest"></param>
        /// <param name="isResponse"></param>
        /// <param name="urn"></param>
        /// <param name="methodName"></param>
        /// <param name="messageTextFull"></param>
        /// <returns></returns>
        public string BuildSqlSelectStatement(string applicationName, bool isRequest, bool isResponse, Guid urn, string methodName, string messageTextFull)
        {
            var isRequestBit = (isRequest) ? 1 : 0;
            var isResponseBit = (isResponse) ? 1 : 0;

            var sql = string.Format("select * from dbo.SoapRequestAndResponseTracingBase where ApplicationName = '{0}' and IsRequest = {1} and IsReply = {2} and URN_UUID = '{3}' and URL = '{4}' and SoapRequestOrResponseXml = '{5}'", applicationName, isRequestBit, isResponseBit, urn, methodName, messageTextFull);
            return sql;
        }

        /// <summary>
        /// BuildSqlDeleteStatement - for the tests that need to verify data written to the SampleLogging Database, SoapRequestAndResponseTracingBase table 
        /// </summary>
        /// <param name="rowIdValue"></param>
        /// <returns></returns>
        public string BuildSqlDeleteStatement(long rowIdValue)
        {
            var sql = string.Format("delete from dbo.SoapRequestAndResponseTracingBase where ID = {0}", rowIdValue);
            return sql;
        }

        /// <summary>
        /// ExecuteSqlSelectStatement - for the tests that need to verify data written to the SampleLogging Database
        /// </summary>
        /// <param name="sqlSelectStatement"></param>
        /// <param name="expectedRowCount"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public long ExecuteSqlSelectStatement(string sqlSelectStatement, int expectedRowCount)
        {
            long rowIdValue = 0;
            var connection = ConfigurationManager.ConnectionStrings["SampleLoggingConnectionString"];

            using (var conn = new SqlConnection(connection.ConnectionString))
            {
                var cmd = new SqlCommand()
                {
                    CommandText = sqlSelectStatement,
                    CommandType = CommandType.Text,
                    Connection = conn,
                };

                conn.Open();

                var reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    Assert.Fail("For the following select statement, there weren't any rows {0}{0}{1}", Environment.NewLine, sqlSelectStatement);
                }

                var localRowCount = 0;
                while (reader.Read())
                {
                    localRowCount++;
                    rowIdValue = reader.GetInt64(0);
                }

                if (localRowCount != expectedRowCount)
                {
                    Assert.Fail("For the following select statement, there {0} rows while we expected {1} rows {2}{2}{3}", localRowCount, expectedRowCount, Environment.NewLine, sqlSelectStatement);
                }

            }

            return rowIdValue;
        }

        /// <summary>
        /// ExecuteSqlDeleteStatement - for the tests that need to verify data written to the SampleLogging Database
        /// </summary>
        /// <param name="sqlDeleteStatement"></param>
        /// <param name="expectedRowCount"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public void ExecuteSqlDeleteStatement(string sqlDeleteStatement, int expectedRowCount)
        {
            var connection = ConfigurationManager.ConnectionStrings["SampleLoggingConnectionString"];

            using (var conn = new SqlConnection(connection.ConnectionString))
            {
                var cmd = new SqlCommand()
                {
                    CommandText = sqlDeleteStatement,
                    CommandType = CommandType.Text,
                    Connection = conn,
                };

                conn.Open();

                var localRowCount = cmd.ExecuteNonQuery();

                if (localRowCount == 0)
                {
                    Assert.Fail("For the following delete statement, there weren't any rows deleted {0}{0}{1}", Environment.NewLine, sqlDeleteStatement);
                }

                if (localRowCount != expectedRowCount)
                {
                    Assert.Fail("For the following delete statement, there {0} rows while we expected {1} rows to be deleted {2}{2}{3}", localRowCount, expectedRowCount, Environment.NewLine, sqlDeleteStatement);
                }
            }
        }
    }
}
