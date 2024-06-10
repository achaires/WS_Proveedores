using dll.clases;
using System.Threading;
using System.Configuration;
using System;

namespace Consola
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.documentosPendientes();
        }

        public void documentosPendientes()
        {            
            EmailTmp config_email = new EmailTmp();
            Funciones fn = new Funciones();

            string a1 = fn.Encriptar("aaa");
            string a2 = fn.Encriptar("aaa");
            string a3 = fn.Encriptar("aaa");
            ///+string abc = fn.Encriptar("Data Source=SRGISMTY2-0726;Initial Catalog = GIS_ProvRecibe;User ID=netdesa;Password=1#Desa$Net$2018;Trusted_Connection=False");
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

            hosturl = ConfigurationManager.AppSettings["hosturl"].ToString();

            config_email.host = fn.Desencriptar(host);
            config_email.port = port;
            config_email.username = fn.Desencriptar(username);
            config_email.password = fn.Desencriptar(password);
            config_email.ssl = ssl;

            int timeout = int.Parse(ConfigurationManager.AppSettings["refreshDocs"].ToString());
            do
            {
                try
                {
                    DateTime dt1 = new DateTime();
                    DateTime dt2 = new DateTime();
                    dt1 = DateTime.Now;
                    Console.WriteLine("INICIO: " + dt1.ToString());
                    //---------

                    //RespuestaFormato res = proc_PlanAccionCorrectiva.RevisarPorVencerBatch(config_email, hosturl);

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

                    //---------
                    dt2 = DateTime.Now;
                    Console.WriteLine("FIN: " + dt2.ToString());
                    Console.WriteLine("Tiempo Total(Segundos): " + (dt2 - dt1).TotalSeconds.ToString());
                    Console.WriteLine("-------------------------------------------------------------------------");
                    Thread.Sleep(timeout);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: " + ex.ToString());
                }
                  
            } while (true);
           
          

        }
    }
}
