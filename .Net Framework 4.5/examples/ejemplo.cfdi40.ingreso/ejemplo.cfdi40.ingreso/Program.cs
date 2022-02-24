using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Usar el espacio de nombres de Induxsoft CFDI 4.0
using induxsoft.cfdi.v40;

namespace ejemplo.cfdi40.ingreso
{
    class Program
    {
        static void Main(string[] args)
        {


           /*Establecer su clave de licencia

           Comprobante.NIC = "Su NIC (Número de Identificación de Cliente)";
           Comprobante.Licencia = "Su clave de licencia";
            
            */

            // Establecer ubicación de los XSLT del SAT
            //Ruta establecida por defecto 
            Comprobante.XSLT_CadenaOriginal = @"..\..\..\..\..\..\Recursos\xslt\cadenaoriginal.xslt";

            try
            {
                //Se crea el comprobante
                Comprobante cfdi = new Comprobante();

                //Se llenan los datos del CFDI

                //Datos de emisor
                cfdi.Emisor.Nombre = "XOCHILT CASAS CHAVEZ";
                cfdi.Emisor.RegimenFiscal = "601";
                cfdi.Emisor.Rfc = "CACX7605101P8";

                //Datos de receptor
                cfdi.Receptor.Nombre = "JIMENEZ ESTRADA SALAS A A";
                cfdi.Receptor.Rfc = "JES900109Q90";
                cfdi.Receptor.DomicilioFiscalReceptor = "29039";
                cfdi.Receptor.RegimenFiscalReceptor = "603";
                cfdi.Receptor.UsoCFDI = "G01";

                 //Datos del comprobante
                cfdi.Serie = "RAH";
                cfdi.Folio = "118";
                cfdi.Fecha = DateTime.Now;
                cfdi.FormaPago = "01";
                cfdi.NoCertificado = "30001000000400002300";
                cfdi.CondicionesDePago = "Inmediato";
                cfdi.MetodoPago = "PUE";
                cfdi.Moneda = "MXN";
                cfdi.TipoDeComprobante = "I";
                cfdi.Total = 1.16M;
                cfdi.Descuento = 0;
                cfdi.SubTotal = 1;
                cfdi.Exportacion = "01";
                cfdi.LugarExpedicion =  "29039";

                //Se crea un concepto
                Concepto c = cfdi.CreateElement<Concepto>();

                //Datos del concepto
                c.Descuento = 0;
                c.ObjetoImp="02";
                c.Importe= 1;
                c.ValorUnitario= 1;
                c.Descripcion= "Concepto de desarrollo";
                c.Unidad= "pza";
                c.Cantidad= 1;
                c.NoIdentificacion= "E5C86D3FA5";
                c.ClaveUnidad="H87";
                c.ClaveProdServ="01010101";

                //Se crea el impuesto del concepto
                TrasladoConcepto tc = cfdi.CreateElement<TrasladoConcepto>();
                tc.Importe = 0.16M;
                tc.Base = 1;
                tc.TasaOCuota = 0.160000M;
                tc.TipoFactor = "Tasa";
                tc.Impuesto = "002";

                // se agrega el impuesto al concepto
                c.ImpuestosConcepto.TransladosConcepto.Add(tc);

                //Se agrega el concepto al nodo Conceptos
                cfdi.Conceptos.Add(c);



                //Se crean los impuestos
                Traslado t = cfdi.CreateElement<Traslado>();
                //Nodo de impuesto
                t.Importe = 0.16M;
                t.Base = 1;
                t.TasaOCuota = 0.160000M;
                t.TipoFactor = "Tasa";
                t.Impuesto = "002";

                //Se agregan el impuesto trasladado al los impestos
                cfdi.Impuestos.Traslados.Add(t);

                //Datos de totales de impuesto
                cfdi.Impuestos.TotalImpuestosTrasladados = 0.16M;


                //Establecer certificado del emisor
                //Ruta establecida por defecto
                cfdi.UbicacionCertificado = @"..\..\..\..\..\..\Recursos\CSD-Pruebas\RFC-PAC-SC\Personas Fisicas\FIEL_CACX7605101P8_20190528152826\cacx7605101p8.cer";
                cfdi.UbicacionClavePrivada = @"..\..\..\..\..\..\Recursos\CSD-Pruebas\RFC-PAC-SC\Personas Fisicas\FIEL_CACX7605101P8_20190528152826\Claveprivada_FIEL_CACX7605101P8_20190528_152826.key";
                cfdi.ContrasenaClavePrivada = "12345678a";

                //Establecer Cuenta de Timbrado Induxsoft (CTI) y contraseña
                cfdi.CuentaTimbradoInduxsoft = "xipova";
                cfdi.ContrasenaCuentaTimbradoInduxsoft = "123456";
                /*
                    Para únicamente sellar, invoque al método Sellar, requerirá haber indicado una clave de licencia.
                    cfdi.Sellar();
                */

                //  El método timbrar puede invocarse sin establecer una clave de licencia
                var res = cfdi.Timbrar();


                string xml = Encoding.UTF8.GetString(Convert.FromBase64String(res["xml"].ToString()));
                System.IO.File.WriteAllText( res["uuid"].ToString() + ".xml", xml);
                //

                Console.WriteLine("El UUID del comprobante timbrado es: " + res["uuid"].ToString());

            }
            catch(Exception ex)
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
