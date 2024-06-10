using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dll.clases
{
    public class Utility
    {
        public static RespuestaFormato enviaEmail(EmailTmp email)
        {
            RespuestaFormato res = new RespuestaFormato();
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(email.host);

                mail.From = new MailAddress(email.username, "Portal Proveedores GIS / Carta Porte");
                //mail.To.Add(email.to);
                if (email.to != "")
                {
                    var tos = email.to.Split(',').ToList();
                    foreach (string to in tos)
                    {
                        var copy = new MailAddress(to);
                        mail.To.Add(copy);
                    }
                }

                //mail.To.Add(email.to);

                mail.Subject = email.subject;
                mail.Body = email.mensaje;
                if (email.cc != "")
                {
                    var ccs = email.cc.Split(',').ToList();
                    foreach (string cci in ccs)
                    {
                        var copy = new MailAddress(cci, "CC");
                        mail.Bcc.Add(copy);
                    }
                }
                foreach (var f in email.files)
                {
                    Attachment attachment = new Attachment(f);
                    mail.Attachments.Add(attachment);
                }
                mail.IsBodyHtml = true;
                //mail.ReplyToList.Add(new MailAddress("a.chaires@softdepot.mx", "reply-to"));

                SmtpServer.Port = email.port;
                SmtpServer.UseDefaultCredentials = true;
                SmtpServer.Credentials = new System.Net.NetworkCredential(email.username, email.password);
                SmtpServer.EnableSsl = true;
                SmtpServer.EnableSsl = email.ssl;
                //ACTIVAR CORREOS
                SmtpServer.Send(mail);
                res.flag = true;
            }
            catch (Exception ex)
            {
                res.flag = false;
                res.errors.Add(ex.Message);
            }

            return res;
        }
        public static List<string> GetQA()
        {
            List<string> res = new List<string>();
            try
            {
                DataAccess da = new DataAccess();

                var dt = new System.Data.DataTable();
                var errores = "";
                if (da.CONS_QA_Accesos(out dt, out errores))
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int idx = 0;
                            var row = dt.Rows[i];
                            res.Add(row[idx].ToString());
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
                res = new List<string>();
            }
            finally
            {
                //con.Close();
            }
            return res;
        }
        public static RespuestaFormato enviaEmailSoftdepot(int tipo, EmailTmp email)
        {
            RespuestaFormato res = new RespuestaFormato();
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("a.chaires@softdepot.mx", "Pruebas Correo Softdepot");
                mail.To.Add(email.to);
                mail.Subject = email.subject;
                mail.Body = email.mensaje;
                if (email.cc != "")
                {
                    var ccs = email.cc.Split(',').ToList();
                    foreach (string cci in ccs)
                    {
                        var copy = new MailAddress(cci, "CC");
                        mail.Bcc.Add(copy);
                    }
                }
                foreach (var f in email.files)
                {
                    Attachment attachment = new Attachment(f);
                    mail.Attachments.Add(attachment);
                }
                mail.IsBodyHtml = true;
                mail.ReplyToList.Add(new MailAddress("a.chaires@softdepot.mx", "reply-to"));

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = true;
                SmtpServer.Credentials = new System.Net.NetworkCredential("a.chaires@softdepot.mx", "hwyvirofpbnpyzcb");
                SmtpServer.EnableSsl = true;
                //ACTIVAR CORREOS
                SmtpServer.Send(mail);
                res.flag = true;
            }
            catch (Exception ex)
            {
                res.flag = false;
                res.errors.Add(ex.Message);
            }

            return res;
        }

        /*--------------------------*/
        public static RespuestaFormato OrdenesCompraPendientesConfirmar(int dias)
        {
            RespuestaFormato res = new RespuestaFormato();
            try
            {
                string sended_url = ConfigurationManager.AppSettings["hosturl"].ToString() + "API/OrdenesCompraPendientesConfirmar";
                HttpWebRequest request = WebRequest.Create(sended_url) as HttpWebRequest;
                request.ContentType = "application/json; charset=utf-8";

                request.Method = "POST";
                string responseStr = "";
                IR_OC_PorConfirmar_Params req = new IR_OC_PorConfirmar_Params();
                req.dias = dias;

                using (var sw = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(req);
                    sw.Write(json);
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls12
                | SecurityProtocolType.Ssl3;
                //optional
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Stream stream = response.GetResponseStream();

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    responseStr = sr.ReadToEnd();
                }

                var responseJSON = JObject.Parse(responseStr);

                var serialized = JsonConvert.SerializeObject(responseJSON);

                RespuestaFormato _res = JsonConvert.DeserializeObject<RespuestaFormato>(serialized);
                res = _res;
            }
            catch (Exception ex)
            {
                res.flag = false;
                res.description = ex.Message;
            }
            finally
            {
            }

            return res;
        }
    }

    public class IR_OC_PorConfirmar_Params
    {
        public int dias { get; set; }

        public IR_OC_PorConfirmar_Params()
        {
            dias = 0;
        }
    }

    public class RespuestaFormato
    {
        public bool flag { get; set; }
        public string description { get; set; }
        public List<string> errors { get; set; }
        public ArrayList content { get; set; }
        public int data_int { get; set; }
        public string data_string { get; set; }
        public string info { get; set; }
        public decimal data_decimal { get; set; }
        public string data_string1 { get; set; }
        public string data_string2 { get; set; }
        public string data_string3 { get; set; }

        public RespuestaFormato()
        {
            flag = false;
            description = "No se ha ejecutado";
            errors = new List<string>();
            content = new ArrayList();
            data_int = 0;
            info = "";
            data_decimal = 0;
            data_string = "";
            data_string1 = "";
            data_string2 = "";
            data_string3 = "";
        }
    }

    public class EmailTmp
    {
        public string to { get; set; }
        public string cc { get; set; }
        public string from { get; set; }
        public string subject { get; set; }
        public string email { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string puesto { get; set; }
        public string mensaje { get; set; }
        public string fecha_nacimiento { get; set; }
        public string sexo { get; set; }
        public string estado_civil { get; set; }
        public string domicilio { get; set; }
        public string marca { get; set; }
        public string version { get; set; }
        public string anho { get; set; }
        public string detalles { get; set; }
        public string calendario { get; set; }
        public string tema { get; set; }
        public string auto { get; set; }
        public string host { get; set; }
        public int port { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool ssl { get; set; }
        public List<string> files { get; set; }

        public EmailTmp()
        {
            host = "";
            port = 0;
            username = "";
            password = "";
            ssl = true;
            to = "";
            from = "";
            email = "";
            nombre = "";
            telefono = "";
            puesto = "";
            mensaje = "";
            fecha_nacimiento = "";
            sexo = "";
            estado_civil = "";
            domicilio = "";
            marca = "";
            version = "";
            anho = "";
            detalles = "";
            calendario = "";
            tema = "";
            auto = "";
            cc = "";
            files = new List<string>();
        }
    }
}



