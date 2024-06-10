using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace dll.clases
{
    class DataAccess
    {
        BaseDatos bd = new BaseDatos();
        String conexionBD = String.Empty;
        Funciones fn = new Funciones();

        public DataAccess()
        {

            try
            {
                string str_con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
                string decrypt = fn.Desencriptar(str_con);
                conexionBD = decrypt;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public Boolean Cons_proc_PlanAccionCorrectiva_PorVencer(out DataTable dt, out String msgError)
        {
            bool boolProcess = true;
            dt = new DataTable();
            msgError = string.Empty;

            try
            {
                SqlParameter[] @params = new SqlParameter[0];

                int i = 0;
                //@params[0] = new SqlParameter("@id", id);

                i++;
                if (!bd.ExecuteProcedure(conexionBD, "Cons_proc_PlanAccionCorrectiva_PorVencerV2", @params, out dt, 1000))
                {
                    boolProcess = false;
                    msgError = "BD Error: " + bd._error.ToString();
                }
                else
                {
                    if (dt.Rows.Count < 1)
                    {
                        boolProcess = false;
                        msgError = "BD Row Error: " + dt.Rows[0][1].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                boolProcess = false;
                msgError = "BD Exception: " + ex.ToString();
            }
            return boolProcess;
        }

        public Boolean Upd_proc_PlanAccionCorrectiva_NotificacionEnviada(int id, out DataTable dt, out String msgError)
        {
            bool boolProcess = true;
            dt = new DataTable();
            msgError = string.Empty;

            try
            {
                SqlParameter[] @params = new SqlParameter[2];

                int i = 0;
                @params[0] = new SqlParameter("@id", id);

                i++;
                if (!bd.ExecuteProcedure(conexionBD, "Upd_proc_PlanAccionCorrectiva_NotificacionEnviada", @params, out dt, 1000))
                {
                    boolProcess = false;
                    msgError = bd._error.ToString();
                }
                else
                {
                    if (!dt.Rows[0][0].ToString().Equals("0"))
                    {
                        boolProcess = false;
                        msgError = dt.Rows[0][1].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                boolProcess = false;
                msgError = ex.ToString();
            }
            return boolProcess;
        }

        public Boolean CONS_QA_Accesos(out DataTable dt, out String msgError)
        {
            bool boolProcess = true;
            dt = new DataTable();
            msgError = string.Empty;

            try
            {
                SqlParameter[] @params = new SqlParameter[0];

                int i = 0;

                i++;
                if (!bd.ExecuteProcedure(conexionBD, "CONS_QA_Accesos", @params, out dt, 1000))
                {
                    boolProcess = false;
                    msgError = bd._error.ToString();
                }
                else
                {
                    if (dt.Rows.Count < 1)
                    {
                        boolProcess = false;
                        msgError = "No hay datos a mostrar";
                    }
                }

            }
            catch (Exception ex)
            {
                boolProcess = false;
                msgError = ex.ToString();
            }
            return boolProcess;
        }

        //-----------------------------------------
        public Boolean cons_PROC_ServicioOrdenesPendientesConfirmar_Hoy(out DataTable dt, out String msgError)
        {
            bool boolProcess = true;
            dt = new DataTable();
            msgError = string.Empty;

            try
            {
                SqlParameter[] @params = new SqlParameter[0];

                int i = 0;
                //@params[0] = new SqlParameter("@id", id);

                i++;
                if (!bd.ExecuteProcedure(conexionBD, "cons_PROC_ServicioOrdenesPendientesConfirmar_Hoy", @params, out dt, 1000))
                {
                    boolProcess = false;
                    msgError = "BD Error: " + bd._error.ToString();
                }
                else
                {
                    if (dt.Rows.Count < 1)
                    {
                        boolProcess = false;
                        msgError = "BD Row Error: " + dt.Rows[0][1].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                boolProcess = false;
                msgError = "BD Exception: " + ex.ToString();
            }
            return boolProcess;
        }

        public Boolean INS_PROC_ServicioOrdenesPendientesConfirmar(PROC_ServicioOrdenesPendientesConfirmar modelo, out DataTable dt, out String msgError)
        {
            bool boolProcess = true;
            dt = new DataTable();
            msgError = string.Empty;

            try
            {
                SqlParameter[] @params = new SqlParameter[2];

                int i = 0;
                @params[0] = new SqlParameter("@clave", modelo.clave);

                i++;
                if (!bd.ExecuteProcedure(conexionBD, "INS_PROC_ServicioOrdenesPendientesConfirmar", @params, out dt, 1000))
                {
                    boolProcess = false;
                    msgError = bd._error.ToString();
                }
                else
                {
                    if (!dt.Rows[0][0].ToString().Equals("0"))
                    {
                        boolProcess = false;
                        msgError = dt.Rows[0][1].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                boolProcess = false;
                msgError = ex.ToString();
            }
            return boolProcess;
        }
    }
}
