using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;
using System.Data;
using System.IO;
using lpp.LogHelper;

namespace lpp.DBHelper.SQLite
{
    public class SQLiteInstance : IDBInstance
    {
        private SQLiteConnection m_Conn;

        public DbConnection Conn
        {
            get { return m_Conn; }
        }

        public DbCommand Cmd 
        {
            get { return new SQLiteCommand(); }
        }

        public char ParamPreffix
        {
            get { return '@'; }
        }

        public SQLiteInstance(string fileOrConnStr)
        {
            m_Conn = new SQLiteConnection();

            bool existsFile = File.Exists(fileOrConnStr); 
            if (existsFile)
            {
                SQLiteConnectionStringBuilder connStrBuilder = new SQLiteConnectionStringBuilder();
                connStrBuilder.DataSource = fileOrConnStr;
                m_Conn.ConnectionString = connStrBuilder.ToString();
            }
            else
            {
                m_Conn.ConnectionString = fileOrConnStr;
            }
        }

        public DbDataReader ExecReader(string sql, List<ParamInfo> paramInfos, System.Data.CommandBehavior cmdBehavior)
        {
            SQLiteDataReader reader = null;
            if (string.IsNullOrEmpty(sql)) return reader;

            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                if (paramInfos != null) foreach (ParamInfo paramInfo in paramInfos)
                    {
                        SQLiteParameter param = new SQLiteParameter();
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

            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;

                if (paramInfos != null) foreach (ParamInfo paramInfo in paramInfos)
                    {
                        SQLiteParameter param = new SQLiteParameter();
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

            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                if (paramInfos != null) foreach (ParamInfo paramInfo in paramInfos)
                    {
                        SQLiteParameter param = new SQLiteParameter();
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

        public void AddRecs(System.Data.DataTable dt)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable ExecFill(string sql, List<ParamInfo> paramInfos)
        {
            DataTable dt = null;
            if (string.IsNullOrEmpty(sql)) return dt;

            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                if (paramInfos != null) foreach (ParamInfo paramInfo in paramInfos)
                    {
                        SQLiteParameter param = new SQLiteParameter();
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
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
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
