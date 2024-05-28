using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;

namespace IDS.DataAccess
{
    public class SqlServer : AbstractDataAccess
    {
        public CommandType CommandType
        {
            get { return DbCommand.CommandType; }
            set
            {
                DbCommand.CommandType = value;
            }
        }

        public string CommandText
        {
            get { return DbCommand.CommandText; }
            set
            {
                DbCommand.CommandText = value;
            }
        }

        public SqlServer()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            DbConnection = new SqlConnection(ConnectionString);
            DbCommand = new SqlCommand();
            DbCommand.Connection = DbConnection;
        }

        public SqlServer(string mConnectionString)
        {
            ConnectionString = mConnectionString;
            DbConnection = new SqlConnection(ConnectionString);
            DbCommand = new SqlCommand();
            DbCommand.Connection = DbConnection;
        }
        
        public void AddParameter(SqlParameter param)
        {
            DbCommand.Parameters.Add(param);
        }

        public void AddParameterRange(SqlParameter[] param)
        {
            if (param != null && param.Count() > 0)
            {
                for (int i = 0; i < param.Count(); i++)
                {
                    DbCommand.Parameters.Add(param[i]);
                }
            }
        }

        public void AddParameter(string paramName, SqlDbType sqlDbType, object value)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = paramName;
            param.SqlDbType = sqlDbType;
            param.Value = value;

            if (DbCommand.Parameters.Contains(paramName))
                return;
            DbCommand.Parameters.Add(param);
        }

        public void AddParameter(string paramName, SqlDbType sqlDbType, object value, bool isNullable)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = paramName;
            param.SqlDbType = sqlDbType;
            param.IsNullable = isNullable;
            param.Value = value;

            if (DbCommand.Parameters.Contains(paramName))
                return;
            DbCommand.Parameters.Add(param);
        }

        public void AddParameter(string paramName, SqlDbType sqlDbType, bool isNullable, int size, ParameterDirection paramDirection)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = paramName;
            param.SqlDbType = sqlDbType;
            param.Size = size;
            param.IsNullable = isNullable;
            param.Direction = paramDirection;

            if (DbCommand.Parameters.Contains(paramName))
                return;
            DbCommand.Parameters.Add(param);
        }

        public void AddParameter(string paramName, SqlDbType sqlDbType, int size, ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = paramName;
            param.SqlDbType = sqlDbType;
            param.Size = size;
            param.Direction = direction;

            if (DbCommand.Parameters.Contains(paramName))
                return;
            DbCommand.Parameters.Add(param);
        }

        public void ClearParameter()
        {
            DbCommand.Parameters.Clear();
        }

        public override void Open()
        {
            if (DbConnection.State != ConnectionState.Open)
                DbConnection.Open();
        }

        public override void BeginTransaction()
        {
            if (DbConnection.State == ConnectionState.Open)
                Transaction = DbConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
            DbCommand.Transaction = Transaction;
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (DbConnection.State == ConnectionState.Open)
                Transaction = DbConnection.BeginTransaction(isolationLevel);
            DbCommand.Transaction = Transaction;
        }

        public override void CommitTransaction()
        {
            if (Transaction != null)
                Transaction.Commit();
        }

        public override void RollbackTransaction()
        {
            if (Transaction != null)
                Transaction.Rollback();
        }

        public int ExecuteNonQuery()
        {
            int result = 0;

            try
            {
                Open();
                if (Transaction == null)
                    throw new Exception("Transaction has not been set yet");

                DbCommand.Connection = DbConnection;
                result = DbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Transaction == null)
                    if (DbConnection.State != ConnectionState.Closed)
                        Close();
                DbCommand.Parameters.Clear();
            }

            return result;
        }

        public int ExecuteNonQueryWithParamOutput()
        {
            int result = 0;

            try
            {
                Open();
                if (Transaction == null)
                    throw new Exception("Transaction has not been set yet");

                DbCommand.Connection = DbConnection;
                result = DbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Transaction == null)
                    if (DbConnection.State != ConnectionState.Closed)
                        Close();
            }

            return result;
        }

        public void ExecuteReader()
        {
            try
            {
                DbDataReader = DbCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbCommand.Parameters.Clear();
            }
        }

        public object ExecuteScalar()
        {
            object result = null;
            try
            {
                Open();
                result = DbCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Transaction == null)
                    if (DbConnection.State != ConnectionState.Closed)
                        Close();
                DbCommand.Parameters.Clear();
            }

            return result;
        }

        public override DataSet GetDataSet()
        {
            DataSet = new DataSet();
            try
            {
                if (DbDataAdapter == null)
                    DbDataAdapter = new SqlDataAdapter();

                DbDataAdapter.SelectCommand = DbCommand;
                DbDataAdapter.Fill(DataSet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbCommand.Parameters.Clear();
            }

            return DataSet;
        }

        public override DataSet GetDataSet(string dataSetName)
        {
            DataSet = new DataSet();
            try
            {
                if (DbDataAdapter == null)
                    DbDataAdapter = new SqlDataAdapter();

                DbDataAdapter.SelectCommand = DbCommand;
                DbDataAdapter.Fill(DataSet);
                DataSet.DataSetName = dataSetName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbCommand.Parameters.Clear();
            }

            return DataSet;
        }

        public override DataTable GetDataTable()
        {
            DataSet = new DataSet();
            try
            {
                if (DbDataAdapter == null)
                {
                    DbDataAdapter = new SqlDataAdapter();
                }

                DbDataAdapter.SelectCommand = DbCommand;
                DbDataAdapter.Fill(DataSet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbCommand.Parameters.Clear();
            }

            return DataSet.Tables[0];
        }

        public override DataTable GetDataTable(string dataTableName)
        {
            DataSet = new DataSet();
            try
            {
                if (DbDataAdapter == null)
                    DbDataAdapter = new SqlDataAdapter();
                DbDataAdapter.SelectCommand = DbCommand;
                DbDataAdapter.Fill(DataSet);
                DataSet.Tables[0].TableName = dataTableName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbCommand.Parameters.Clear();
            }

            return DataSet.Tables[dataTableName];
        }

        public void ExecuteReader(CommandBehavior behavior)
        {
            try
            {
                DbDataReader = DbCommand.ExecuteReader(behavior);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbCommand.Parameters.Clear();
            }
        }

        public string GetServerName()
        {
            return (this.DbConnection as SqlConnection).DataSource;
        }

        public string GetDatabaseName()
        {
            return (this.DbConnection as SqlConnection).Database;
        }
    }
}