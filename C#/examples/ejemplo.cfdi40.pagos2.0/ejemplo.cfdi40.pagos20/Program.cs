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
            /*Establecer su clave de licencia

            Comprobante.NIC = "Su NIC (Número de Identificación de Cliente)";
            Comprobante.Licencia = "Su clave de licencia";
            */

            //Establecer la ruta del certificado,clave y contraseña del certificado

            string cerfile = @"..\..\..\..\..\..\Recursos\CSD-Pruebas\RFC-PAC-SC\Personas Fisicas\FIEL_CACX7605101P8_20190528152826\1\30001000000400002335.cer"; 
            string keyfile = @"..\..\..\..\..\..\Recursos\CSD-Pruebas\RFC-PAC-SC\Personas Fisicas\FIEL_CACX7605101P8_20190528152826\1\CSD_XOCHILT_CASAS_CHAVEZ_CACX7605101P8_20190528_173544.key"; 
            string cerpwd = "12345678a";



            // Establecer ubicación de los XSLT del SAT
            string xslt = @"..\..\..\..\..\..\Recursos\xslt40\cadenaoriginal.xslt";
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
                //Establecer Cuenta de Timbrado Induxsoft (CTI) y contraseña
                cfdi.CuentaTimbradoInduxsoft = "xipova";// Cuenta de pruebas, no obtiene un TFD válido
                cfdi.ContrasenaCuentaTimbradoInduxsoft = "123456";
                //Llenar datos del CFDI
                cfdi.Serie = "F";
                cfdi.Folio = "12948";
                cfdi.Moneda = "XXX";
                cfdi.Fecha = DateTime.Now;
                cfdi.NoCertificado = "30001000000400002335";
                cfdi.SubTotal=0;
                cfdi.TipoDeComprobante = "P";
                cfdi.LugarExpedicion = "29039";
                cfdi.Exportacion = "01";
                cfdi.Total=0;

                //Llenar datos del emisor
                cfdi.Emisor.Rfc = "CACX7605101P8";
                cfdi.Emisor.RegimenFiscal = "601";
                cfdi.Emisor.Nombre = "XOCHILT CASAS CHAVEZ";

                //Llenar datos del receptor
                cfdi.Receptor.Rfc = "LAMP931125IX9";
                cfdi.Receptor.Nombre = "PABLO LARA AGUILAR";
                cfdi.Receptor.UsoCFDI = "G01";
                cfdi.Receptor.RegimenFiscalReceptor = "601";
                cfdi.Receptor.DomicilioFiscalReceptor = "29039";

                //Llenar datos del conceptos
                Concepto c = cfdi.CreateElement<Concepto>();
                c.ClaveProdServ = "84111506";
                //de esta forma se agrega valor a los atributos de tipo decimal para no hacer la conversión
                c.Cantidad= 1.000000m;
                c.ClaveUnidad = "ACT";
                c.Descripcion = "Pago";
                c.ObjetoImp = "01";
                c.ValorUnitario=0;
                c.Importe=0;
                cfdi.Conceptos.Add(c); //agregamos el nodo concepto a comprobante

                //se crea el nodo pagos
                Pagos pagos20 = new Pagos();
                //creamos el nodo Totales y agregamos valor a sus atributos
                pagos20.Totales.MontoTotalPagos=1.00m;

                //creamos el nodo pago
                Pago pago = new Pago();
                pago.MonedaP = "MXN";
                //colocamos valor a los attributos de pago
                pago.TipoCambioP=1;
                pago.NumOperacion = "1033";
                pago.Monto=1.00m;
                pago.FormaDePagoP = "01";
                pago.FechaPago = DateTime.Now;

                //agregar el docto relacionado
                DoctoRelacionado doctoRel = pago.CreateElement<DoctoRelacionado>();
                //doctoRel.EquivalenciaDR = 1;
                doctoRel.MonedaDR = "MXN";
                doctoRel.EquivalenciaDR=1;
                doctoRel.Folio = "01";
                doctoRel.ImpSaldoInsoluto=0;
                doctoRel.ImpPagado=1.00m;
                doctoRel.ImpSaldoAnt=1.00m;
                doctoRel.NumParcialidad = 1;
                //comprobante relacionado
                //colocar el uuid del comprobante a relacionar
                doctoRel.IdDocumento = "XXXX760ba62af4094b5caeea4213a816bca7";
                doctoRel.ObjetoImpDR = "02";

                //estos nodos deben existir solamente si el objetoImpDr es 02 de lo contrario no
                //agregamos el nodo retencionDr
                RetencionDR RetencionDr = new RetencionDR();
                RetencionDr.BaseDR = 2.0m;
                RetencionDr.ImpuestoDR = "002";
                RetencionDr.TipoFactorDR = "Tasa";
                RetencionDr.TasaOCuotaDR = 0.16m;
                RetencionDr.ImporteDR = 0.16m;

                //Creamos el nodo trasladoDr y sus atributos
                TrasladoDR TrasladoDr = new TrasladoDR();
                TrasladoDr.BaseDR = 2.0m;
                TrasladoDr.ImpuestoDR = "002";
                TrasladoDr.TipoFactorDR = "Tasa";
                TrasladoDr.TasaOCuotaDR = 0.16m;
                TrasladoDr.ImporteDR = 0.16m;
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

                /*
                Para únicamente sellar, invoque al método Sellar, requerirá haber indicado una clave de licencia.
                cfdi.Sellar();
                */
                //El método timbrar puede invocarse sin establecer una clave de licencia
                var res = cfdi.Timbrar();
                //guardar el xml
                string archivo = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), res["uuid"].ToString() + ".xml");

                System.IO.File.WriteAllText(archivo, Encoding.UTF8.GetString(Convert.FromBase64String(res["xml"].ToString())));
                System.Diagnostics.Process.Start(archivo);
                Console.WriteLine("El UUID del comprobante timbrado es: " + res["uuid"].ToString());
                Console.WriteLine("El CFDI está en: " + archivo);
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
