using System;
using System.Data;
using System.Data.SqlClient;

namespace dll.clases
{
    class BaseDatos
    {
        public String _error = String.Empty;

        public BaseDatos()
        {




        }

        public Boolean ExecuteProcedure(String conexion, String store, SqlParameter[] parameters, out DataTable dt, int timeOut)
        {
            Boolean boolProcess = true;
            _error = String.Empty;
            dt = new DataTable();
            SqlConnection con = new SqlConnection(conexion);
            try
            {
                using (SqlCommand cmd = new SqlCommand(store, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = timeOut;
                    foreach (SqlParameter param in parameters)
                    {
                        if (param != null)
                        {
                            cmd.Parameters.Add(param);
                        }

                    }
                    con.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }

                    //cmd.ExecuteNonQuery();
                    try
                    {
                        con.Close();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                _error = ex.ToString();
                try
                {
                    con.Close();
                }
                catch (Exception)
                {
                }
                throw;
            }

            return boolProcess;
        }

        public Boolean ExecuteProcedure(String conexion, String store, out DataTable dt, int timeOut)
        {
            Boolean boolProcess = true;
            _error = String.Empty;
            dt = new DataTable();
            SqlConnection con = new SqlConnection(conexion);
            try
            {
                using (SqlCommand cmd = new SqlCommand(store, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = timeOut;
                    con.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }

                    //cmd.ExecuteNonQuery();
                    try
                    {
                        con.Close();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                _error = ex.ToString();
                try
                {
                    con.Close();
                }
                catch (Exception)
                {
                }
                throw;
            }

            return boolProcess;
        }


    }
}
