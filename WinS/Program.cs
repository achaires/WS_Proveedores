using dll.clases;
using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace WinS
{
    class Program
    {
        Thread findDocsThread;

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }

        public void iniciaBusqueda()
        {
            try
            {
                findDocsThread = new Thread(documentosPendientes);
                findDocsThread.Start();
            }
            catch (Exception)
            {
                
            }
        }

        private void documentosPendientes()
        {
            EmailTmp config_email = new EmailTmp();
            Funciones fn = new Funciones();
            int port = 0;
            string host = "";
            string username = "";
            string password = "";
            bool ssl = false;
            string hosturl = "";

            Int32.TryParse(ConfigurationManager.AppSettings["SMTPPort"].ToString(), out port);
            host = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            username = ConfigurationManager.AppSettings["SMTPUsername"].ToString();
            password = ConfigurationManager.AppSettings["SMTPPassword"].ToString();
            Boolean.TryParse(ConfigurationManager.AppSettings["SMTPSSL"].ToString(), out ssl);

            config_email.host = fn.Desencriptar(host);
            config_email.port = port;
            config_email.username = fn.Desencriptar(username);
            config_email.password = fn.Desencriptar(password);
            config_email.ssl = ssl;

            hosturl = ConfigurationManager.AppSettings["hosturl"].ToString();

            int timeout = int.Parse(ConfigurationManager.AppSettings["refreshDocs"].ToString());
            
            do
            {
                try
                {
                    DateTime dt1 = new DateTime();

                    dt1 = DateTime.Now;

                    /*if ((dt1.Hour >= 20 && dt1.Hour <= 23) ||
                        (dt1.Hour >= 4 && dt1.Hour <= 7))*/
                    if (dt1.Hour >= 8 && dt1.Hour <= 19)
                    {

                        RespuestaFormato res = proc_PlanAccionCorrectiva.RevisarPorVencerBatch(config_email, hosturl);

                    }

                    /*------ORDENES POR CONFIRMAR------*/
                    int horaNotificacion = 8;
                    Int32.TryParse(ConfigurationManager.AppSettings["horaNotificacion"].ToString(), out horaNotificacion);
                    if (dt1.Hour == horaNotificacion)
                    {
                        var porConfirmarHoy = PROC_ServicioOrdenesPendientesConfirmar.Get();
                        if (porConfirmarHoy.id <= 0)
                        {
                            porConfirmarHoy.fecha = DateTime.Now.Date;
                            porConfirmarHoy.clave = "clave_servicio_" + fn.Encriptar(DateTime.Now.ToString("yyyy-MM-dd"));
                            var ins = porConfirmarHoy.Insert();
                            if (ins.flag != false)
                            {
                                int diasPorConfirmar = 75;
                                Int32.TryParse(ConfigurationManager.AppSettings["diasPorConfirmar"].ToString(), out diasPorConfirmar);
                                Console.WriteLine("consultar servicio");

                                var servicioConfirmarcion = Utility.OrdenesCompraPendientesConfirmar(diasPorConfirmar);
                            }
                        }
                    }
                    /*------ORDENES POR CONFIRMAR------*/
                    Thread.Sleep(timeout);

                   
                }
                catch (Exception ex)
                {
                    try
                    {
                        //File.WriteAllText(ConfigurationManager.AppSettings["pathLogs"].ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + "_ServiceError.txt", "Error " + ex.ToString());
                    }
                    catch (Exception)
                    {
                        
                    }
                }
                    

            } while (true);
        }
    }
}
