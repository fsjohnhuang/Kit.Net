using lpp.CollectionHelper;
using lpp.LogHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace lpp.DBHelper.MSSQL
{
    public class MSSQLInstance : IDBInstance
    {
        private SqlConnection m_Conn;

        public DbConnection Conn
        {
            get { return m_Conn; }
        }

        public DbCommand Cmd
        {
            get { return new SqlCommand(); }
        }

        public MSSQLInstance(string connStr)
        {
            m_Conn = new SqlConnection(connStr);
        }

        public char ParamPreffix
        {
            get { return '@'; }
        }

        public DbDataReader ExecReader(string sql, List<ParamInfo> paramInfos, CommandBehavior cmdBehavior)
        {
            SqlDataReader reader = null;
            if (string.IsNullOrEmpty(sql)) return reader;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                if (paramInfos != null) foreach (ParamInfo paramInfo in paramInfos)
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = ParamPreffix + paramInfo.Name;
                    param.Value = paramInfo.Value;
                    if (paramInfo.DBType.HasValue)
                    {
                        param.DbType = paramInfo.DBType.Value;
                    }
                    cmd.Parameters.Add(param);
                }
                cmd.Connection = m_Conn;
                if (m_Conn.State != ConnectionState.Open)
                {
                    m_Conn.Open();
                }

                try
                {
                    reader = cmd.ExecuteReader(cmdBehavior);
                }
                catch (Exception ex)
                {
                    Logger.WriteEx2LogFile(ex);
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                        reader = null;
                    }
                }

                return reader;
            }
        }

        public object ExecScalar(string sql, List<ParamInfo> paramInfos)
        {
            object obj = null;
            if (string.IsNullOrEmpty(sql)) return obj;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;

                if (paramInfos != null) foreach (ParamInfo paramInfo in paramInfos)
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = ParamPreffix + paramInfo.Name;
                    param.Value = paramInfo.Value;
                    if (paramInfo.DBType.HasValue)
                    {
                        param.DbType = paramInfo.DBType.Value;
                    }
                    cmd.Parameters.Add(param);
                }
                cmd.Connection = m_Conn;
                if (m_Conn.State != ConnectionState.Open)
                {
                    m_Conn.Open();
                }

                try
                {
                    obj = cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Logger.WriteEx2LogFile(ex);
                }
                finally
                {
                    if (m_Conn.State != ConnectionState.Closed)
                    {
                        m_Conn.Close();
                    }
                }

                return obj;
            }
        }

        public int ExecNonQuery(string sql, List<ParamInfo> paramInfos)
        {
            int count = 0;
            if (string.IsNullOrEmpty(sql)) return count;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                if (paramInfos != null) foreach (ParamInfo paramInfo in paramInfos)
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = ParamPreffix + paramInfo.Name;
                    param.Value = paramInfo.Value;
                    if (paramInfo.DBType.HasValue)
                    {
                        param.DbType = paramInfo.DBType.Value;
                    }
                    cmd.Parameters.Add(param);
                }
                cmd.Connection = m_Conn;
                if (m_Conn.State != ConnectionState.Open)
                {
                    m_Conn.Open();
                }

                try
                {
                    count = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.WriteEx2LogFile(ex);
                }
                finally
                {
                    if (m_Conn.State != ConnectionState.Closed)
                    {
                        m_Conn.Close();
                    }
                }

                return count;
            }
        }

        public void AddRecs(DataTable dt)
        {
            SqlBulkCopy bulkCopy = new SqlBulkCopy(m_Conn);
            bulkCopy.DestinationTableName = dt.TableName;
            bulkCopy.BatchSize = 5000;
            foreach (DataColumn col in dt.Columns)
            {
                bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(col.ColumnName, col.ColumnName));
            }

            try
            {
                if (m_Conn.State != ConnectionState.Open)
                {
                    m_Conn.Open();
                }

                bulkCopy.WriteToServer(dt);
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
            finally
            {
                if (m_Conn.State != ConnectionState.Closed)
                {
                    m_Conn.Close();
                }
            }
        }

        public DataTable ExecFill(string sql, List<ParamInfo> paramInfos)
        {
            DataTable dt = null;
            if (string.IsNullOrEmpty(sql)) return dt;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                if (paramInfos != null) foreach (ParamInfo paramInfo in paramInfos)
                    {
                        SqlParameter param = new SqlParameter();
                        param.ParameterName = ParamPreffix + paramInfo.Name;
                        param.Value = paramInfo.Value;
                        if (paramInfo.DBType.HasValue)
                        {
                            param.DbType = paramInfo.DBType.Value;
                        }
                        cmd.Parameters.Add(param);
                    }
                cmd.Connection = m_Conn;

                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    Logger.WriteEx2LogFile(ex);
                    dt = null;
                }
                finally
                {
                    if (m_Conn.State != ConnectionState.Closed)
                    {
                        m_Conn.Close();
                    }
                }

                return dt;
            }
        }
    }
}
