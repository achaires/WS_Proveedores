using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;

namespace dll.clases
{
    public class PROC_ServicioOrdenesPendientesConfirmar
    {
        public int id { get; set; } = 0;
        public DateTime fecha { get; set; } = DateTime.Now;
        public string clave { get; set; } = "";

        public PROC_ServicioOrdenesPendientesConfirmar()
        {
            id = 0;
            clave = "";
            fecha = DateTime.Now;
        }

        public RespuestaFormato Insert()
        {
            RespuestaFormato res = new RespuestaFormato();
            try
            {
                DataAccess da = new DataAccess();

                var dt = new System.Data.DataTable();
                var errores = "";
                if (da.INS_PROC_ServicioOrdenesPendientesConfirmar(this, out dt, out errores))
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

        public static PROC_ServicioOrdenesPendientesConfirmar Get()
        {
            PROC_ServicioOrdenesPendientesConfirmar res = new PROC_ServicioOrdenesPendientesConfirmar();
            try
            {
                DataAccess da = new DataAccess();

                var dt = new System.Data.DataTable();
                var errores = "";
                if (da.cons_PROC_ServicioOrdenesPendientesConfirmar_Hoy(out dt, out errores))
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int idx = 0;
                            var row = dt.Rows[i];
                            var item = new PROC_ServicioOrdenesPendientesConfirmar();
                            item.id = Int32.Parse(row[idx].ToString()); idx++;
                            item.fecha = DateTime.Parse(row[idx].ToString()); idx++;
                            item.clave = row[idx].ToString(); idx++;
                            res = item;
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
                res = new PROC_ServicioOrdenesPendientesConfirmar();
            }
            finally
            {
                //con.Close();
            }
            return res;
        }
    }

    public class GenReporte
    {
       

       // static Dictionary<int, ReportDocument> reportes = new Dictionary<int, ReportDocument>(50);

        /*
        public Boolean consultaFacturas() {
            DataAccess da = new DataAccess();
            Boolean boolProcess = true;
            DataTable dt = new DataTable();
            String msgError = String.Empty;

            String fechaEjecucion = DateTime.Now.ToString("yyyyMMddHHmmss");

            try
            {
                if (!da.consultaFactPteEnvio(out dt, out msgError))
                {
                    boolProcess = false;
                }
                else {
                    if (dt.Rows.Count > 0)
                    {
                        deleteFilesInDirectory();
                        int i = 0;
                        System.Threading.Tasks.Parallel.ForEach(dt.AsEnumerable(), (DataRows) =>
                        {
                            GenerarReporte(DataRows, Interlocked.Increment(ref i));

                        });

                    }
                }
            }
            catch (Exception ex )
            {
                boolProcess = false;
                msgError = ex.ToString();
            }

            if (!boolProcess)
            {
                File.WriteAllText(ConfigurationManager.AppSettings["pathLogs"].ToString() + fechaEjecucion + ".txt", "Error " + msgError);
            }

            return boolProcess;
        }

        public void deleteFilesInDirectory()
        {

            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(ConfigurationManager.AppSettings["pathTemp"].ToString());

                foreach (FileInfo file in di.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception)
                    {

                    }

                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private Boolean GenerarReporte(DataRow D_ROW, int numTH) {
            DataAccess da = new DataAccess();
            Funciones fn = new Funciones();
            StringBuilder sb_Proceso = new StringBuilder();
            Boolean boolProcess = true;
            
            String msgError = String.Empty;
           

            int estatusEnvio = 1;

            String idCfdi = String.Empty;
            String idFactOracle = String.Empty;
            //String fechaEmision = String.Empty;
            String RfcEmisor = String.Empty;
            String RfcReceptor = String.Empty;
            // String serie = String.Empty;
            //String folio = String.Empty;
            //String fechaTimbrado = String.Empty;
            String tipoCompNegocio = String.Empty;
            String UUID = String.Empty;
            String str_TSTXML = String.Empty;
            String XML_Timbrado = String.Empty;
            String nombreReceptor = String.Empty;
            String emailsReceptores = String.Empty;
            String nombreReporte = String.Empty;
            byte[] imgQR = null;
            Stream rptStream = null;

            String orgID = String.Empty;
            String nombreEmisor = String.Empty;
            String emisorEmail = String.Empty;

            try
            {
                idCfdi = D_ROW["idCfdi"].ToString();
                idFactOracle = D_ROW["idFactOracle"].ToString();
                //fechaEmision = D_ROW["fechaEmision"].ToString();
                tipoCompNegocio = D_ROW["tipoComprobanteNegocio"].ToString();
                RfcEmisor = D_ROW["rfcEmisor"].ToString();
                RfcReceptor = D_ROW["rfcReceptor"].ToString();
                //serie = D_ROW["serie"].ToString();
                //folio = D_ROW["folio"].ToString();
                //fechaTimbrado = D_ROW["fechaTimbrado"].ToString();
                UUID = D_ROW["uuid"].ToString();
                str_TSTXML = D_ROW["xmlTST"].ToString();
                imgQR = (byte[]) D_ROW["imageQR"];
                XML_Timbrado = D_ROW["xmlTimbrado"].ToString();
                nombreReceptor = D_ROW["nombreReceptor"].ToString();
                emailsReceptores = D_ROW["correosReceptores"].ToString();
                nombreReporte = D_ROW["nombreReporte"].ToString();

                orgID = D_ROW["orgID"].ToString();
                nombreEmisor = D_ROW["nombreEmisor"].ToString();

                rptStream = null;


                fn.addError(0, "Procesando Envio de comprobante idCfdi: " + idCfdi + ", idFactOracle: " + idFactOracle, ref sb_Proceso);

                if (!da.obtenerEmailOrgID(Int64.Parse(orgID), out emisorEmail, out msgError))
                {
                    fn.addError(2, "Error al obtener correo de Salida OrgId: " + msgError, ref sb_Proceso);
                }

                if (nombreReporte.Trim().Equals(String.Empty))
                {
                    estatusEnvio = 2;
                    boolProcess = false;
                    fn.addError(2, "No existe Reporte para este comprobante", ref sb_Proceso);
                }

                if (boolProcess)
                {
                    if (!GenRepFactura(str_TSTXML, imgQR, nombreReporte, numTH, out rptStream, out msgError))
                    {
                         boolProcess = false;
                        Console.WriteLine("Error en el idCfdi: " + idCfdi.ToString());
                        fn.addError(2, "Error al generar el PDF de Factura: " + msgError, ref sb_Proceso);
                    }
                }

                if (emailsReceptores.Trim().Equals(String.Empty))
                {
                    estatusEnvio = 4;
                    fn.addError(2, "No existen Receptores para este comprobante", ref sb_Proceso);
                }

                if (estatusEnvio != 4 && boolProcess == true)
                {
                    try
                    {
                        Email em = new Email();
                        if (!em.enviaCorreoCfdi(orgID, idCfdi, tipoCompNegocio, idFactOracle, UUID, rptStream, emailsReceptores, XML_Timbrado, nombreReceptor, nombreEmisor, emisorEmail, idCfdi, out msgError))
                        {
                            estatusEnvio = 3;
                            boolProcess = false;
                            fn.addError(2, "Error al enviar Comprobante: " + msgError, ref sb_Proceso);
                        }
                    }
                    catch (Exception ex)
                    {
                        estatusEnvio = 3;
                        boolProcess = false;
                        fn.addError(2, "Excepcion al enviar Comprobante: " + ex.Message, ref sb_Proceso);
                    }
                    
                }



            }
            catch (Exception ex)
            {
                estatusEnvio = 2;
                boolProcess = false;
                fn.addError(2, "Excepcion al enviar Comprobante: " + ex.Message, ref sb_Proceso);
            }

            try
            {
                
                if (!da.upd_EnvioFactura(Int64.Parse(idCfdi), estatusEnvio, fn.ToByteArray(rptStream), out msgError))
                {

                    boolProcess = false;
                    fn.addError(2, "Error al actualizar estatus factura: " + msgError, ref sb_Proceso);
                }
            }
            catch (Exception)
            {
                
            }

            try
            {
                if (!boolProcess)
                {
                  da.ins_LogError(Int64.Parse(idCfdi), sb_Proceso.ToString(),out msgError);
                    try
                    {
                        String pathLog = ConfigurationManager.AppSettings["pathLogs"].ToString()  + numTH.ToString() + "_" + UUID + "_" + nombreReporte;
                        File.WriteAllText("" ,sb_Proceso.ToString());
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
            catch (Exception)
            {
                
            }

            return boolProcess;
        }

        private Boolean GenRepFactura(String xmlTST, Byte[] imgQR, String nombreReporte, int numTH, out Stream rptStream, out String msgError)
        {
            Boolean boolProcess = true;
            rptStream = null;
            msgError = String.Empty;
            try
            {

                if (!nombreReporte.Equals(String.Empty))
                {
                    String pathReporte = ConfigurationManager.AppSettings["pathReportes"].ToString() + @"Reportes\TEMP_RPT\" + numTH.ToString() + "_" + nombreReporte;
                    String pathXSD = ConfigurationManager.AppSettings["pathReportes"].ToString() + @"XSD\TST.xsd";


                    if (!File.Exists(pathReporte))
                    {
                        String pathReporteBase = ConfigurationManager.AppSettings["pathReportes"].ToString() + @"Reportes\" + nombreReporte;
                        try
                        {
                            File.Copy(pathReporteBase, pathReporte);
                        }
                        catch (Exception)
                        {
                        }
                       
                    }

                    if (File.Exists(pathReporte))
                    {
                        if (File.Exists(pathXSD))
                        {
                            DataSet DS_TST = new DataSet();

                            DS_TST.ReadXmlSchema(pathXSD);

                            System.IO.StringReader xmlSR = new System.IO.StringReader(xmlTST);

                            DS_TST.ReadXml(xmlSR, XmlReadMode.IgnoreSchema);


                            foreach (DataTable dtXML in DS_TST.Tables)
                            {
                                if (dtXML.TableName.ToUpper().Equals("PROVAUTORIZADO"))
                                {
                                    dtXML.Rows[0]["imgQR"] = imgQR;
                                }
                            }


                            ReportDocument reporte = new ReportDocument();

                            reporte.Load(pathReporte);
                            reporte.SetDataSource(DS_TST);


                            rptStream = reporte.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            DS_TST.Dispose();

                            reporte.Close();
                            //reporte.Load(null);
                            reporte.Dispose();
                            
                           


                        }
                        else
                        {
                            boolProcess = false;
                            msgError = "El archivo de XSDTST  (" + pathXSD + "), no Existe.";
                        }
                    }
                    else
                    {
                        boolProcess = false;
                        msgError = "El archivo de reporte (" + pathReporte + "), no Existe y no se pudo crear.";
                    }


                }
                else
                {
                    boolProcess = false;
                    msgError = "No se tiene un reporte valido para generar. Favor de verificar las tablas de reportes.";
                }



            }
            catch (Exception ex)
            {
                boolProcess = false;
                msgError = ex.ToString();
            }

            return boolProcess;
        }
        */
    }
}
