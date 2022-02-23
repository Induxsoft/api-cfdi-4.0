using induxsoft.cfdi.v40;
using induxsoft.cfdi.v40.Complementos.pago20;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace ejemplo.cfdi40.pagos20
{
    class Program
    {
        static void Main(string[] args)
        {
            //Establecer su clave de licencia

           Comprobante.NIC = "201373";
            Comprobante.Licencia = "YzNty5tiKeZqOgAv2bumxA==";
            string cerfile = @"\\192.168.0.14\Data\FactuDesk\Schemas\z_IDS1402204R4@uvav2\files\storage\d680a0cba69d4d3853ee98fd62f5c0fc.idgroup";
            string keyfile = @"\\192.168.0.14\Data\FactuDesk\Schemas\z_IDS1402204R4@uvav2\files\storage\161381b56035453cf1332e6a83d487c7.idgroup";
            string cerpwd = "Istocolmo123=";

            // Establecer ubicación de los XSLT del SAT
            string xslt = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\GitHub\cfdi-4.0-api/Recursos/xslt/cadenaoriginal.xslt";
            Comprobante.XSLT_CadenaOriginal = xslt;

            try
            {
                Comprobante cfdi = new Comprobante();
                cfdi.UbicacionCertificado = cerfile;
                cfdi.UbicacionClavePrivada = keyfile;
                cfdi.ContrasenaClavePrivada = cerpwd;
                cfdi.CuentaTimbradoInduxsoft = "201652";
                cfdi.ContrasenaCuentaTimbradoInduxsoft = "123456";
                //Llenar datos del CFDI
                cfdi.Serie = "F";
                cfdi.Folio = "12948";
                cfdi.Fecha = DateTime.Now;
                cfdi.NoCertificado = "30001000000400002335";
                cfdi.SetAttribute("SubTotal", "0");
                cfdi.Moneda = "XXX";
                cfdi.TipoDeComprobante = "P";
                cfdi.LugarExpedicion = "29039";
                cfdi.Exportacion = "01";
                cfdi.SetAttribute("Total", "0");

                //emisor
                cfdi.Emisor.Rfc = "IDS1402204R4";
                cfdi.Emisor.RegimenFiscal = "601";
                cfdi.Emisor.Nombre = "INDUXSOFT DATA SERVICES S DE RL DE CV";

                //receptor
                cfdi.Receptor.Rfc = "SIC150828HX3";
                cfdi.Receptor.Nombre = "SOLUCIONES DE IMAGEN Y CALIDAD EMPRESARIAL DE MEXICO";
                cfdi.Receptor.UsoCFDI = "G01";
                cfdi.Receptor.RegimenFiscalReceptor = "601";
                cfdi.Receptor.DomicilioFiscalReceptor = "29039";

                //conceptos
                Concepto c = cfdi.CreateElement<Concepto>();

                c.ClaveProdServ = "84111506";
                c.SetAttribute("Cantidad", "1.000000");
                c.ClaveUnidad = "ACT";
                c.Descripcion = "Pago";
                c.ObjetoImp = "01";
                c.SetAttribute("ValorUnitario", "0");
                c.SetAttribute("Importe", "0.00");
                cfdi.Conceptos.Add(c); //agregamos el nodo concepto a comprobante
                //se crea el nodo pago20,pago
                Pagos pagos20 = new Pagos();
                //agregamos el espacio de nombre de pagos con su respectiva schemaLocation
                pagos20.SetAttribute("xmlns:pago20", "http://www.sat.gob.mx/Pagos20");
                pagos20.SetAttribute("xsi:schemaLocation", "http://www.sat.gob.mx/Pagos20 http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos20.xsd");
               
                //colocamos valor a los attributos del nodo pagos
                pagos20.Totales.SetAttribute("MontoTotalPagos", "1.00");
                //creamos el nodo pago
                Pago pago = new Pago();
                pago.MonedaP = "MXN";
                //colocamos valor a los attributos de pago
                pago.SetAttribute("TipoCambioP", "1");
                pago.NumOperacion = "1033";
                pago.SetAttribute("Monto", "1.00");

                pago.FormaDePagoP = "01";
                pago.FechaPago = DateTime.Now;

                //agregar el docto relacionado
                DoctoRelacionado doctoRel = pago.CreateElement<DoctoRelacionado>();
                //doctoRel.EquivalenciaDR = 1;
                doctoRel.MonedaDR = "MXN";
                doctoRel.SetAttribute("EquivalenciaDR", "1");
                doctoRel.Folio = "01";
                doctoRel.SetAttribute("ImpSaldoInsoluto", "0");
                doctoRel.SetAttribute("ImpPagado", "1.00");
                doctoRel.SetAttribute("ImpSaldoAnt", "1.00");
                doctoRel.NumParcialidad = 1;
                //comprobante relacionado
                doctoRel.IdDocumento = "ba0f42cb-0c26-4db7-9e54-7617d2abb761";

                doctoRel.ObjetoImpDR = "02";
                //estos nodos deben existir solamente si el objetoImpDr es 02 de lo contrario no
                //agregamos el nodo retencionDr
                RetencionDR RetencionDr = new RetencionDR();
                RetencionDr.BaseDR = Convert.ToDecimal("2.0");
                RetencionDr.ImpuestoDR = "002";
                RetencionDr.TipoFactorDR = "Tasa";
                RetencionDr.TasaOCuotaDR = Convert.ToDecimal("0.16");
                RetencionDr.ImporteDR = Convert.ToDecimal("0.16");
                //Creamos el nodo trasladoDr y sus atributos
                TrasladoDR TrasladoDr = new TrasladoDR();
                TrasladoDr.BaseDR = Convert.ToDecimal("2.0");
                TrasladoDr.ImpuestoDR = "002";
                TrasladoDr.TipoFactorDR = "Tasa";
                TrasladoDr.TasaOCuotaDR = Convert.ToDecimal("0.16");
                TrasladoDr.ImporteDR = Convert.ToDecimal("0.16");
                //agregamos los impuesto al DoctoRelacionado
                doctoRel.ImpuestosDR.RetencionesDR.Add(RetencionDr);
                doctoRel.ImpuestosDR.TrasladosDR.Add(TrasladoDr);
                //agregamos el nodo DoctoRelacionado a pago
                pago.Add(doctoRel);
                //agregamos el nodo pago a pagos
                pagos20.Add(pago);

                //agregamos el nodo pagos y sus componentes ya agregados al comprobante cfdi
                cfdi.Complemento.Add(pagos20);
                
                var res = cfdi.Timbrar();
                string v= Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\GitHub\cfdi-4.0-api\.Net Framework 4.5\examples\ejemplo.cfdi40.pagos20\" + res["uuid"].ToString()+".xml";
                System.IO.File.WriteAllText(v,cfdi.toXml());
                Console.WriteLine("El UUID del comprobante timbrado es: " + res["uuid"].ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine("E R R O R !!!");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("");
            Console.WriteLine("Presione ENTER para terminar");
            Console.ReadLine();
        }
    }
    
}
