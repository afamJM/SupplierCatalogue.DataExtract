// <copyright file="BaseDataAccess.cs" company="Hitched Ltd">
// Copyright (c) Hitched Ltd. All rights reserved.
// </copyright>

namespace SupplierCatalogue.DataExtract.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Text;

    /// <summary>
    /// Base acccess class for SQL Server operations
    /// </summary>
    public class BaseDataAccess
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataAccess"/> class.
        /// </summary>
        public BaseDataAccess()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataAccess"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string for the data source</param>
        public BaseDataAccess(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// Gets or sets the connection string for the data source
        /// </summary>
        protected string ConnectionString { get; set; }

        /// <summary>
        /// Fetch the DB command for the data source
        /// </summary>
        /// <param name="connection">The data source connection string</param>
        /// <param name="commandText">The data source command text</param>
        /// <param name="commandType">The data source command type</param>
        /// <returns>The data source command object</returns>
        protected DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType)
        {
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection)
            {
                CommandType = commandType,
            };
            return command;
        }

        /// <summary>
        /// Fetch a given SQL parameter object for the data source
        /// </summary>
        /// <param name="parameter">The required parameter</param>
        /// <param name="value">The value for the parameter</param>
        /// <returns>A data source parameter object</returns>
        protected SqlParameter GetParameter(string parameter, object value)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, value ?? DBNull.Value)
            {
                Direction = ParameterDirection.Input,
            };
            return parameterObject;
        }

        /// <summary>
        /// Fetch a given parameter object for the data source
        /// </summary>
        /// <param name="parameter">The required parameter</param>
        /// <param name="type">The required parameter type</param>
        /// <param name="value">The value for the parameter</param>
        /// <param name="parameterDirection">The direction for the parameter</param>
        /// <returns>A data source parameter object</returns>
        protected SqlParameter GetParameterOut(string parameter, SqlDbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.InputOutput)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, type);

            if (type == SqlDbType.NVarChar || type == SqlDbType.VarChar || type == SqlDbType.NText || type == SqlDbType.Text)
            {
                parameterObject.Size = -1;
            }

            parameterObject.Direction = parameterDirection;

            if (value != null)
            {
                parameterObject.Value = value;
            }
            else
            {
                parameterObject.Value = DBNull.Value;
            }

            return parameterObject;
        }

        /// <summary>
        /// Execute a non query fetch against the data source
        /// </summary>
        /// <param name="procedureName">The data script to execute</param>
        /// <param name="parameters">The parameters for the execution script</param>
        /// <param name="commandType">The type of command to run</param>
        /// <returns>The non query return value</returns>
        protected int ExecuteNonQuery(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            int returnValue = -1;

            try
            {
                using (SqlConnection connection = this.GetConnection())
                {
                    DbCommand cmd = this.GetCommand(connection, procedureName, commandType);

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    returnValue = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.Write("Failed to ExecuteNonQuery for " + procedureName + "Exception : " + ex.Message);
                throw;
            }

            return returnValue;
        }

        /// <summary>
        /// Execute a scalar fetch against the data source
        /// </summary>
        /// <param name="procedureName">The data script to execute</param>
        /// <param name="parameters">The parameters for the execution script</param>
        /// <param name="commandType">The type of command to run</param>
        /// <returns>The scalar value</returns>
        protected object ExecuteScalar(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            object returnValue = null;

            try
            {
                using (DbConnection connection = this.GetConnection())
                {
                    DbCommand cmd = this.GetCommand(connection, procedureName, commandType);

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    StringBuilder returnString = new StringBuilder();
                    returnString.Append(cmd.ExecuteScalar());

                    returnValue = returnString;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Execute Scalar for {/s} : {/s}", procedureName, ex.Message);
                throw;
            }

            return returnValue;
        }

        /// <summary>
        /// Fect the data reader object for the data source
        /// </summary>
        /// <param name="procedureName">The data script to execute</param>
        /// <param name="parameters">The parameters for the execution script</param>
        /// <param name="commandType">The type of command to run</param>
        /// <returns>The data source reader</returns>
        protected DbDataReader GetDataReader(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            DbDataReader ds;

            try
            {
                DbConnection connection = this.GetConnection();
                {
                    DbCommand cmd = this.GetCommand(connection, procedureName, commandType);
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    ds = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to GetDataReader for " + procedureName + " : " + ex.Message);
                throw;
            }

            return ds;
        }

        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }
    }
}
