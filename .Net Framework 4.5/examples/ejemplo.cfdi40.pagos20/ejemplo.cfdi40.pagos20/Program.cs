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
            //1- agregar el componente induxsoft.cfdi.v40.dll
            //Establecer su clave de licencia

            Comprobante.NIC = "Su NIC (Número de Identificación de Cliente)";
            Comprobante.Licencia = "Su clave de licencia";
            //Establecer la ruta del certificado,clave y contraseña del certificado
            string cerfile = @"..\..\..\..\..\..\Recursos\CSD-Pruebas\RFC-PAC-SC\Personas Fisicas\FIEL_CACX7605101P8_20190528152826\cacx7605101p8.cer"; ;
            string keyfile = @"..\..\..\..\..\..\Recursos\CSD-Pruebas\RFC-PAC-SC\Personas Fisicas\FIEL_CACX7605101P8_20190528152826\Claveprivada_FIEL_CACX7605101P8_20190528_152826.key"; ;
            string cerpwd = "12345678a";

            // Establecer ubicación de los XSLT del SAT
            string xslt= @"..\..\..\..\..\..\Recursos\xslt\cadenaoriginal.xslt";
            Comprobante.XSLT_CadenaOriginal = xslt;

            try
            {
                //instanciar la clase del componente induxsoft.cfdi.v40.dll
                //para obtener sus clases y atributos
                Comprobante cfdi = new Comprobante();
                //establecer la ubicación del certificado a los attributos del comprobante
                cfdi.UbicacionCertificado = cerfile;
                cfdi.UbicacionClavePrivada = keyfile;
                cfdi.ContrasenaClavePrivada = cerpwd;
                cfdi.CuentaTimbradoInduxsoft = "xipova";
                cfdi.ContrasenaCuentaTimbradoInduxsoft = "123456";
                //Llenar datos del CFDI
                cfdi.Serie = "F";
                cfdi.Folio = "12948";
                cfdi.Fecha = DateTime.Now;
                cfdi.NoCertificado = "00001000000504485894";
                cfdi.SetAttribute("SubTotal", "0");
                cfdi.Moneda = "XXX";
                cfdi.TipoDeComprobante = "P";
                cfdi.LugarExpedicion = "29039";
                cfdi.Exportacion = "01";
                cfdi.SetAttribute("Total", "0");

                //Llenar datos del emisor
                cfdi.Emisor.Rfc = "CACX7605101P8";
                cfdi.Emisor.RegimenFiscal = "601";
                cfdi.Emisor.Nombre = "XOCHILT CASAS CHAVEZ";

                //Llenar datos del receptor
                cfdi.Receptor.Rfc = "JES900109Q90";
                cfdi.Receptor.Nombre = "JIMENEZ ESTRADA SALAS A A";
                cfdi.Receptor.UsoCFDI = "G01";
                cfdi.Receptor.RegimenFiscalReceptor = "601";
                cfdi.Receptor.DomicilioFiscalReceptor = "29039";

                //Llenar datos del conceptos
                Concepto c = cfdi.CreateElement<Concepto>();
                c.ClaveProdServ = "84111506";
                //de esta forma se agrega valor a los atributos de tipo decimal para no hacer la conversión
                c.SetAttribute("Cantidad", "1.000000"); 
                c.ClaveUnidad = "ACT";
                c.Descripcion = "Pago";
                c.ObjetoImp = "01";
                c.SetAttribute("ValorUnitario", "0");
                c.SetAttribute("Importe", "0");
                cfdi.Conceptos.Add(c); //agregamos el nodo concepto a comprobante

                //se crea el nodo pagos
                Pagos pagos20 = new Pagos();
                //agregamos el espacio de nombre de pagos20 con su respectiva schemaLocation
                pagos20.SetAttribute("xmlns:pago20", "http://www.sat.gob.mx/Pagos20");
                pagos20.SetAttribute("xsi:schemaLocation", "http://www.sat.gob.mx/Pagos20 http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos20.xsd");
                
                //creamos el nodo Totales y agregamos valor a sus atributos
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
                //colocar el uuid del comprobante a relacionar
                doctoRel.IdDocumento = "";
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
                
                //para timbrar solo llamamos al método Timbrar del comprobante
                var res = cfdi.Timbrar();
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
