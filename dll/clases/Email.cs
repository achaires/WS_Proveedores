using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;



namespace dll.clases
{
    class Email
    {
        Funciones fn = new Funciones();
        DataAccess da = new DataAccess();

        public bool enviaCorreoCfdi(String orgID, String idCfdi, String tipoCompNegocio, String idFactOracle, String UUID, 
            Stream streamPDF, String Destinatarios, String strXML, String nombreReceptor, String nombreEmisor,
         String emisorEmail, String idFactura, out String msgError)
        {
            Boolean boolProcess = true;
            msgError = string.Empty;
            /*String msgEnvia = String.Empty;
            String SubjectStr = String.Empty;
            Boolean appPruebas = false;

           //Destinatarios = Destinatarios + ";rodrigo.mireles.ext@gis.com.mx";

            try
            {
                if (ConfigurationManager.AppSettings["appPruebas"].ToUpper().ToString().Equals("TRUE"))
                {
                    appPruebas = true;
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (appPruebas)
                {
                    SubjectStr = "[PRUEBAS] - ";
                }

                msgEnvia = formatoHtmlCfdi().ToString();

                msgEnvia = msgEnvia.Replace("@NOMBRERECEPTOR", nombreReceptor);
                msgEnvia = msgEnvia.Replace("@NOMBREEMISOR", nombreEmisor);
                msgEnvia = msgEnvia.Replace("@EMAIL", emisorEmail);

                if (tipoCompNegocio.Equals("MI") || tipoCompNegocio.Equals("SI") || tipoCompNegocio.Equals("CI") || tipoCompNegocio.Equals("SB") ||
                    tipoCompNegocio.Equals("PP") || tipoCompNegocio.Equals("TS") || tipoCompNegocio.Equals("VE") || tipoCompNegocio.Equals("AC") ||
                    tipoCompNegocio.Equals("AS"))
                {
                    SubjectStr = SubjectStr + "Factura # " + idFactOracle;
                }

                if (tipoCompNegocio.Equals("IP"))
                {
                    SubjectStr = SubjectStr + "Recibo Electrónico de Pago # " + idFactOracle;
                }

                if (tipoCompNegocio.Equals("CM") || tipoCompNegocio.Equals("CC") || tipoCompNegocio.Equals("MR") || tipoCompNegocio.Equals("AP"))
                {
                    SubjectStr = SubjectStr + "Nota de Crédito # " + idFactOracle;
                }

                if (tipoCompNegocio.Equals("MD") || tipoCompNegocio.Equals("CD") || tipoCompNegocio.Equals("NC"))
                {
                    SubjectStr = SubjectStr + "Nota de Debito # " + idFactOracle;
                }
                

                AlternateView htmlView =
     AlternateView.CreateAlternateViewFromString(msgEnvia,
                             Encoding.UTF8,
                             MediaTypeNames.Text.Html);


                MailMessage msg = new MailMessage();

                Boolean ExistenReceptores = false;
                String correosRechazo = String.Empty;

                if (Destinatarios.Length > 0)
                {
                    String[] strCorreos = Destinatarios.Split(';');
                    foreach (String correo in strCorreos)
                    {
                        String tmp_Correo = correo.Trim();
                        if (!tmp_Correo.Equals(String.Empty))
                        {
                            if (fn.validaExpRegEx(tmp_Correo, Encoding.UTF8.GetString(Convert.FromBase64String("XlthLXpBLVowLTkuISMkJSYnKisvPT9eX2B7fH1+LV0rQFthLXpBLVowLTldKD86W2EtekEtWjAtOS1dezAsNjF9W2EtekEtWjAtOV0pPyg/OlwuW2EtekEtWjAtOV0oPzpbYS16QS1aMC05LV17MCw2MX1bYS16QS1aMC05XSk/KSok"))))
                            {
                                ExistenReceptores = true;
                                msg.To.Add(new MailAddress(tmp_Correo));
                            }
                            else
                            {
                                correosRechazo = correosRechazo + tmp_Correo + ";";
                            }
                        }
                        
                    }

                }

                String LogoName = "logoGIS.png";
                String correoSend = "relay.ap@gis.com.mx";
                String correoPswSend = "1#4ppSmsn$2";

                if (orgID == "173")
                {
                    LogoName = "Vitromex.png";
                }
                if (orgID == "190")
                {
                    LogoName = "Evercast.png";
                }
                if (orgID == "81")
                {
                    LogoName = "Cinsa.png";
                }
                if (orgID == "133")
                {
                    LogoName = "Cifunsa.png";
                }
                if (orgID == "134")
                {
                    LogoName = "Tisamatic.jpg";
                }
                if (orgID == "151")
                {
                    LogoName = "Calorex.jpg";//???
                }
                if (orgID == "430")
                {
                    LogoName = "Fluida.jpg";
                }
                if (orgID == "88")
                {
                    LogoName = "Fisso.jpg";
                }

                if (orgID == "831" || orgID == "832" || orgID == "833" || orgID == "851" ||
                    orgID == "852" || orgID == "151" || orgID == "430" || orgID == "72" || orgID == "73")
                {
                    correoSend = "relay.at@gis.com.mx";
                    correoPswSend = "1#4ppSmsn$2";
                    LogoName = "logoAriston.png";
                }

                //if (emisorEmail.Length > 0)
                //{
                //    msg.From = new MailAddress(emisorEmail, "Notificaciones CFDI");
                //}
                //else
                //{
                //    msg.From = new MailAddress("noreply@gis.com.mx", "Notificaciones CFDI");
                //}

                msg.From = new MailAddress(correoSend, "Notificaciones CFDI");

                msg.Subject = SubjectStr;


               // msg.Sender = new MailAddress("soluciones.net@gis.com.mx", "Notificaciones CFDI");

                if (streamPDF != null)
                {
                    Attachment filePDF = new Attachment(streamPDF, UUID.ToString() + ".pdf");

                    msg.Attachments.Add(filePDF);
                   
                }
                else {
                    boolProcess = false;
                    msgError = "No se recibio pdf para envio por correo, PDF NULL";
                }

                if (strXML.Length > 0)
                {
                    Stream streamXML = fn.GenerateStreamFromString(strXML);

                    if (streamXML != null)
                    {
                        Attachment fileXML = new Attachment(streamXML, UUID.ToString() + ".xml");
                        msg.Attachments.Add(fileXML);
                    }
                }


                // msg.AlternateViews.Add(plainView);

                try
                {
                    
                    LinkedResource img = new LinkedResource(ConfigurationManager.AppSettings["pathReportes"].ToString() + @"Reportes\" + LogoName, MediaTypeNames.Image.Jpeg);
                    img.ContentId = "imagen";

                    htmlView.LinkedResources.Add(img);

                }
                catch (Exception )
                {

                }

                msg.AlternateViews.Add(htmlView);
                


                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = false;
                client.Port = 25;
                client.Host = "correo.gis.com.mx";
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = false;
                
                
                if (ExistenReceptores)
                {
                    client.Send(msg);
                }
                else {
                    msgError = "No existen receptores validos para realizar envío.";
                    boolProcess = false;
                }

                try
                {
                    if (correosRechazo != String.Empty)
                    {
                        da.ins_LogError(Int64.Parse(idCfdi), "Receptores con estructura invalida: " + correosRechazo, out msgError);
                    }
                }
                catch (Exception)
                {

                }


            }
            catch (Exception ex)
            {
                boolProcess = false;
                msgError = ex.ToString();
            }

            */
            return boolProcess;
        }
        
        
        /*
        private StringBuilder formatoHtmlCfdi()
        {
            StringBuilder msgReturn = new StringBuilder();
            try
            {
                msgReturn.AppendLine("<html xmlns:a=\"urn:oracle:b2b:X12/V4010/850\" xmlns:xp20=\"http://www.oracle.com/XSL/Transform/java/oracle.tip.pc.services.functions.Xpath20\">");
                msgReturn.AppendLine("<head>");
                msgReturn.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head>");
                msgReturn.AppendLine("<body>");
                msgReturn.AppendLine("<style type=\"text/css\">");
                msgReturn.AppendLine(".encabezado{color: #FFFFFF;background-color: #003366;}");
                msgReturn.AppendLine("table.estilo, th.estilo, td.estilo {border: 1px solid black;border-collapse: collapse;border-color: #003366;}");
                msgReturn.AppendLine(".centrar{text-align: center;}");
                msgReturn.AppendLine(".col-md-5{display: inline-block;text-align: right;}");
                //msgReturn.AppendLine(".col-md-4{display: inline-block;width: 400px;margin: 5px 0px 5px 5px;text-align: right;}");
                msgReturn.AppendLine(".col-md-4{display: inline-block;width: '400px;margin: 5px 0px 5px 5px;text-align: right;}");
                msgReturn.AppendLine(".row{width: 100%;}");
                msgReturn.AppendLine("</style>");

                msgReturn.AppendLine("<table align=\"center\" width=\"100%\">");
                msgReturn.AppendLine("<tr>");
                msgReturn.AppendLine("<td class=\"centrar\" width=\"25%\"><img width=\"200px\" src=\"cid:imagen\">");
                msgReturn.AppendLine("</td>");
                msgReturn.AppendLine("<td class=\"centrar\" width=\"50%\"><font face=\"Garamond\" color=\"#003366\" size=\"10\">Facturación</font>");
                msgReturn.AppendLine("</td>");
                msgReturn.AppendLine("<td class=\"centrar\" width=\"30%\">");
                msgReturn.AppendLine("</td>");
                msgReturn.AppendLine("</tr>");
                msgReturn.AppendLine("</table>");
                msgReturn.AppendLine("<hr width=\"100%\" color=\"#003366\" size=\"2\"><br><br>");
                msgReturn.AppendLine("<br>");



                //msgReturn.AppendLine("");
                msgReturn.AppendLine("<p>Estimado Cliente: <b>@NOMBRERECEPTOR</b></p>");
                msgReturn.AppendLine("<p>Le notificamos que la empresa: <b>@NOMBREEMISOR</b> le ha enviado su Comprobante Fiscal Digital a trav&eacute;s del env&iacute;o autom&aacute;tico de correos del Sistema de Factura Electr&oacute;nica de TI GIS.</p>");
                msgReturn.AppendLine("<p>Adjunto encontrar&aacute; el archivo XML con su respectiva representaci&oacute;n impresa.</p>");
                msgReturn.AppendLine("<p>Para cualquier duda favor de contactar a su ejecutivo.");
                //msgReturn.AppendLine("<p>Para cualquier duda favor de contactar al siguiente correo electr&oacute;nico: <a href=\"mailto: @EMAIL\">@EMAIL</a></p>");
                msgReturn.AppendLine("<p>Saludos Cordiales,</p>");
                msgReturn.AppendLine("<p><b>@NOMBREEMISOR</b></p>");


                msgReturn.AppendLine("</body>");
                msgReturn.AppendLine("</html>");

            }
            catch (Exception)
            {
                // msgReturn.AppendLine(ex.ToString());
            }
            return msgReturn;
        }
        */
    }
}
