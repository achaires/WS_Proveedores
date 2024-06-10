using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dll.clases
{
    public class BatchFolio
    {
        public string folio { get; set; } = "";
        public List<proc_PlanAccionCorrectiva> lista { get; set; } = new List<proc_PlanAccionCorrectiva>();

        public static List<BatchFolio> FiltrarListado(List<proc_PlanAccionCorrectiva> lista)
        {
            List<BatchFolio> res = new List<BatchFolio>();
            try
            {
                List<string> folios = new List<string>();

                folios = lista.Select(i => i.Folio).Distinct().ToList();

                foreach(string folio in folios)
                {
                    BatchFolio item = new BatchFolio();
                    item.folio = folio;
                    item.lista = lista.Where(i => i.Folio == folio).ToList();
                    if(item.lista.Count > 0)
                    {
                        res.Add(item);
                    }
                }

            }
            catch (Exception ex)
            {
               res = new List<BatchFolio>();
            }
            finally
            {
                //con.Close();
            }
            return res;
        }


    }
    public class proc_PlanAccionCorrectiva
    {
        public int Id { get; set; }
        public string AccionCorrectiva { get; set; }
        public int AccionCorrectivaTipoId { get; set; }
        public string AccionCorrectivaTipoDescripcion { get; set; }
        public int AccionCorrectivaEstatusId { get; set; }
        public string AccionCorrectivaEstatusDescripcion { get; set; }
        public string Pregunta1 { get; set; }
        public string Pregunta2 { get; set; }
        public string Pregunta3 { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public int RequiereEvidencia { get; set; }
        public int Evidencia { get; set; }
        public string Usuario { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioEmail { get; set; }
        public string Comentario { get; set; }
        public string Folio { get; set; }
        public string UUID { get; set; }
        public DateTime FechaNotificacion { get; set; }
        public string compania { get; set; }

        public proc_PlanAccionCorrectiva()
        {
            Id = 0;
            AccionCorrectiva = "";
            AccionCorrectivaTipoId = 0;
            AccionCorrectivaEstatusId = 0;
            AccionCorrectivaTipoDescripcion = "";
            AccionCorrectivaEstatusDescripcion = "";
            Pregunta1 = "";
            Pregunta2 = "";
            Pregunta3 = "";
            RequiereEvidencia = -1;
            Evidencia = 0;
            FechaCreacion = DateTime.Parse("1969-01-01");
            FechaActualizacion = DateTime.Parse("1969-01-01");
            FechaNotificacion = DateTime.Parse("1969-01-01");
            Usuario = "";
            UsuarioNombre = "";
            UsuarioEmail = "";
            Comentario = "";
            UUID = "";
            Folio = "";
            compania = "";
        }

        public RespuestaFormato ActualizarFecha()
        {
            RespuestaFormato res = new RespuestaFormato();
            try
            {
                DataAccess da = new DataAccess();

                var dt = new System.Data.DataTable();
                var errores = "";
                if (da.Upd_proc_PlanAccionCorrectiva_NotificacionEnviada(this.Id, out dt, out errores))
                {
                    if (dt.Rows.Count > 0)
                    {
                        var row = dt.Rows[0];
                        int id = 0;
                        Int32.TryParse(row[3].ToString(), out id);
                        if (id > 0)
                        {
                            res.flag = true;
                            res.data_int = id;
                        }
                    }
                }
                else
                {
                    res.description = "Ocurrió un error.";
                    res.errors.Add(errores);
                }


            }
            catch (Exception ex)
            {
                res.errors.Add(ex.Message);
                res.description = "Ocurrió un error.";
            }
            finally
            {
                //con.Close();
            }
            return res;
        }

        public static List<proc_PlanAccionCorrectiva> Get()
        {
            List<proc_PlanAccionCorrectiva> res = new List<proc_PlanAccionCorrectiva>();
            try
            {
                DataAccess da = new DataAccess();

                var dt = new System.Data.DataTable();
                var errores = "";
                if (da.Cons_proc_PlanAccionCorrectiva_PorVencer(out dt, out errores))
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int idx = 0;
                            var row = dt.Rows[i];
                            var item = new proc_PlanAccionCorrectiva();
                            item.Id = Int32.Parse(row[idx].ToString()); idx++;
                            item.Folio = row[idx].ToString(); idx++;
                            item.AccionCorrectivaTipoId = Int32.Parse(row[idx].ToString()); idx++;
                            item.AccionCorrectivaTipoDescripcion = row[idx].ToString(); idx++;
                            item.Pregunta1 = row[idx].ToString(); idx++;
                            item.Pregunta2 = row[idx].ToString(); idx++;
                            item.Pregunta3 = row[idx].ToString(); idx++;
                            item.RequiereEvidencia = Int32.Parse(row[idx].ToString()); idx++;
                            item.FechaCreacion = DateTime.Parse(row[idx].ToString()); idx++;
                            item.FechaActualizacion = DateTime.Parse(row[idx].ToString()); idx++;
                            item.Usuario = row[idx].ToString(); idx++;
                            item.UsuarioNombre = row[idx].ToString(); idx++;
                            item.UsuarioEmail = row[idx].ToString(); idx++;
                            item.AccionCorrectivaEstatusId = Int32.Parse(row[idx].ToString()); idx++;
                            item.AccionCorrectivaEstatusDescripcion = row[idx].ToString(); idx++;
                            item.Comentario = row[idx].ToString(); idx++;
                            item.FechaNotificacion = DateTime.Parse(row[idx].ToString()); idx++;
                            item.compania = row[idx].ToString(); idx++;
                            res.Add(item);
                        }
                    }
                }
                else
                {
                    //
                }


            }
            catch (Exception ex)
            {
                res = new List<proc_PlanAccionCorrectiva>();
            }
            finally
            {
                //con.Close();
            }
            return res;
        }



        public static RespuestaFormato RevisarPorVencer(EmailTmp config, string hosturl)
        {
            RespuestaFormato res = new RespuestaFormato();
            try
            {
                string errores = "";
                var pendientes = proc_PlanAccionCorrectiva.Get();
                if(pendientes.Count > 0)
                {
                    foreach (proc_PlanAccionCorrectiva accion in pendientes)
                    {
                        var email = new EmailTmp();
                        email.port = config.port;
                        email.ssl = config.ssl;
                        email.host = config.host;
                        email.password = config.password;
                        email.username = config.username;

                        email.subject = "Recordatorio de vencimiento de actividades. Folio " + accion.Folio;
                        email.mensaje = "<!DOCTYPE html>" +
                        "<html>" +
                        "<head>" +
                        "<link href='https://fonts.googleapis.com/css2?family=Lato:wght@300;400;700;900&display=swap' rel='stylesheet'><title></title>" +
                        "<style type='text/css'>body,body * {font-family: 'Lato', sans-serif;}</style>" +
                        "</head>" +
                        "<body>" +
                        "<div style='border-top: 25px solid #003e74; padding: 20px 20px; border-radius: 7px; box-shadow: 0px 2px 2px 2px #ddd; width: 450px;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;border-left: 1px solid #ddd;'>" +
                        "<img src='https://www.gis.com.mx/wp-content/themes/GIS/images/gislogo1.png' style='width: 100px;'>" +
                        "<br>" +
                        "<h1 style='font-size: 15px; font-weight: 400;'>Hola, te informamos sobre el próximo <b>vencimiento</b> de un registro con dentro del folio <b>" + accion.Folio + "</b></h1>" +
                        "<br>" +
                        "<p style='font-size: 14px;'><b>Folio:</b> " + accion.Folio + "</p>" +
                        "<p style='font-size: 14px;'><b>Tipo de acción:</b> " + accion.AccionCorrectivaTipoDescripcion + "</p>" +
                        "<p style='font-size: 14px;'><b>Actividad:</b> " + accion.Pregunta1 + "</p>" +
                        "<p style='font-size: 14px;'><b>Responsable:</b> " + accion.Pregunta2 + "</p>" +
                        "<p style='font-size: 14px;'><b>Fecha de compromiso:</b> " + accion.Pregunta3 + "</p>" +
                        "<br>" +
                        "<p style='font-size: 14px;'>Para ver el detalle del registro, da click en el siguiente <a href='" + hosturl + "/AccountLogin'>enlace</a>.</p>" +
                        "<br>" +
                        "<small><i>No respondas a este mensaje, ha sido generado automáticamente.</i></small>" +
                        "</div>" +
                        "</body>" +
                        "</html>";
                        email.to = "alejandro.chairesg@gmail.com";// modelo.email;
                        email.from = "noreply@portalproveedores.com";
                        var emailSended = Utility.enviaEmailSoftdepot(0, email);
                        if(emailSended.flag== true)
                        {
                            var actualizarFecha = accion.ActualizarFecha();
                            if(actualizarFecha.flag != true)
                            {
                                errores += "No se pudo actualizar la fecha del registro " + accion.Id + " del folio" + accion.Folio;
                            }
                        }
                        else
                        {
                            errores += "No se pudo enviar la notificación del registro " + accion.Id + " del folio" + accion.Folio;
                        }
                    }
                }
                else
                {
                    //
                }
            }
            catch (Exception ex)
            {
                res.errors.Add(ex.Message);
                res.description = "Ocurrió un error.";
            }
            finally
            {
                //con.Close();
            }
            return res;
        }

        public static RespuestaFormato RevisarPorVencerBatch(EmailTmp config, string hosturl)
        {
            RespuestaFormato res = new RespuestaFormato();
            try
            {
                string errores = "";
                var pendientes = proc_PlanAccionCorrectiva.Get();
                var _qa = Utility.GetQA();
                if (pendientes.Count > 0)
                {
                    List<BatchFolio> folios = BatchFolio.FiltrarListado(pendientes);
                    foreach (BatchFolio folio in folios)
                    {
                        var email = new EmailTmp();
                        email.port = config.port;
                        email.ssl = config.ssl;
                        email.host = config.host;
                        email.password = config.password;
                        email.username = config.username;

                        email.subject = "Recordatorio de vencimiento de acciones - Folio " + folio.folio;

                        //
                        string tabla_datos = "";
                        string compania = "";
                        string email_proveedor = "";
                        //List<string> email_proveedor = new List<string>();
                        foreach (proc_PlanAccionCorrectiva item in folio.lista)
                        {
                            tabla_datos += "<tr>" +
                                "<td style='width: 100px; text-align: left;'>" + item.AccionCorrectivaTipoDescripcion + "</td>" +
                                "<td style='width: 275px; text-align: left;'>" + item.Pregunta1 + "</td>" +
                                "<td style='width: 275px; text-align: center;'>" + item.Pregunta2 + "</td>" +
                                "<td style='width: 150px; text-align: center;'>" + DateTime.Parse(item.Pregunta3).ToString("dd-MMM-yyyy", new CultureInfo("es-ES")).Replace(".", "").ToUpper() + "</td>" +
                                "</tr>";
                            email_proveedor = item.UsuarioEmail;
                            compania = item.compania;
                        }
                        //
                        email.mensaje = "<!DOCTYPE html>" +
                        "<html>" +
                        "<head>" +
                        "<link href='https://fonts.googleapis.com/css2?family=Lato:wght@300;400;700;900&display=swap' rel='stylesheet'><title></title>" +
                        "<style type='text/css'>body,body * {font-family: 'Lato', sans-serif;} table { width: 100%; border-left: 1px solid #003e74; border-right: 1px solid #003e74;} tr td { text-align: left; font-size: 12px; } tbody tr td{ border-bottom: 1px solid #003e74; padding: 5px 5px;} thead th { text-align: left; background-color: #003e74; color: #fff; font-size: 14px; padding: 5px 5px; }</style>" +
                        "</head>" +
                        "<body>" +
                        "<div style='border-top: 25px solid #003e74; padding: 20px 20px; border-radius: 7px; box-shadow: 0px 2px 2px 2px #ddd; width: 900px;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;border-left: 1px solid #ddd;'>" +
                        "<img src='https://www.gis.com.mx/wp-content/themes/GIS/images/gislogo1.png' style='width: 100px;'>" +
                        "<br>" +
                        "<h1 style='font-size: 15px; font-weight: 400;'>Estimado proveedor: " + compania + "</h1>" +
                        "<p style='font-size: 15px; font-weight: 400;'>Por este medio se le notifica sobre el vencimiento de fecha para la conclusión de acciones aceptadas para el folio " + folio.folio + ".</p>" +
                        "<br>" +
                        "<table cellspacing='0'>" +
                        "<thead>" +
                        "<tr>" +
                        "<th style='width: 100px; text-align: left;'>Acción</th>" +
                        "<th style='width: 275px; text-align: left;'>¿Qué?</th>" +
                        "<th style='width: 275px; text-align: center;'>¿Quién?</th>" +
                        "<th style='width: 150px; text-align: center;'>¿Cuando?</th>" +
                        "</tr>" +
                        "</thead>" +
                        "<tbody>" +
                        tabla_datos +
                        "</tbody>" +
                        "</table>" +
                        "<p style='font-size: 14px;'>Agradecemos su atención inmediata.</p>" +
                        "<br>" +
                        "<p style='font-size: 14px;'>Para ver el detalle del registro, click en el siguiente <a href='" + hosturl + "/Account/Login'>enlace</a>.</p>" +
                        "<br>" +
                        "<small><i>Por favor no respondas a este mensaje, ha sido generado automáticamente </i></small>" +
                        "</div>" +
                        "</body>" +
                        "</html>";
                        //email.cc = "a.chaires@softdepot.mx";// modelo.email;
                        email.to = String.Join(",", _qa) + "," + email_proveedor;// "ruby.gonzalez@gis.com.mx,luis.morenom@gis.com.mx,adolfo.aranda@gis.com.mx";// modelo.email;
                        email.from = "noreply@portalproveedores.com";
                        var emailSended = Utility.enviaEmail(email);
                        if (emailSended.flag == true)
                        {

                            foreach (proc_PlanAccionCorrectiva accion in folio.lista)
                            {
                                var actualizarFecha = accion.ActualizarFecha();
                                if (actualizarFecha.flag != true)
                                {
                                    errores += "No se pudo actualizar la fecha del registro " + accion.Id + " del folio" + accion.Folio;
                                }
                            }
                        }
                        else
                        {
                            errores += "No se pudo enviar la notificación del folio " + folio.folio;
                        }
                        string Z = "";
                    }
                }
                else
                {
                    //
                }
            }
            catch (Exception ex)
            {
                res.errors.Add(ex.Message);
                res.description = "Ocurrió un error.";
            }
            finally
            {
                //con.Close();
            }
            return res;
        }
    }
}
