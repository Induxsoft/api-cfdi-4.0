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


           /*Establecer su clave de licencia*/

           //Comprobante.NIC = "Su NIC (Número de Identificación de Cliente)";
          // Comprobante.Licencia = "Su clave de licencia";


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
                cfdi.NoCertificado = "00001000000504485894";
                cfdi.CondicionesDePago = "Inmediato";
                cfdi.SetAttribute("SubTotal", "1.16");
                cfdi.Moneda = "MXN";
                cfdi.TipoDeComprobante = "I";
                cfdi.MetodoPago = "PUE";
                cfdi.Exportacion = "01";
                cfdi.SetAttribute("Total", "1.16");
                cfdi.SetAttribute("LugarExpedicion", "29039");

                //Se crea un concepto
                Concepto c = cfdi.CreateElement<Concepto>();

                //Datos del concepto
                c.SetAttribute("Descuento", "0.00");
                c.SetAttribute("ObjetoImp", "02");
                c.SetAttribute("Importe", "1.00");
                c.SetAttribute("ValorUnitario", "1.00");
                c.SetAttribute("Descripcion ", "OBJETO DE PRUEBAS");
                c.SetAttribute("Unidad ", "pza");
                c.SetAttribute("Cantidad", "1");
                c.SetAttribute("NoIdentificacion", "E5C86D3FA5");
                c.SetAttribute("ClaveUnidad", "H87");
                c.SetAttribute("ClaveProdServ", "01010101");

                // Se agrega el concepto como nodo al nodo conceptos
                cfdi.Conceptos.Add(c);
                
                //Se crean los impuestos
                Traslado t = cfdi.CreateElement<Traslado>();
                //Nodo de impuesto
                t.SetAttribute("importe  ", "0.16");
                t.SetAttribute("Base ", "1.00");
                t.SetAttribute("TasaOCuota ", "0.160000");
                t.SetAttribute("TipoFactor ", "Tasa");
                t.SetAttribute("Impuesto ", "002");
                
                Impuestos i = cfdi.CreateElement<Impuestos>();
                i.SetAttribute("TotalImpuestosTrasladados","0.16");
                i.SetAttribute("TotalImpuestosRetenidos", "0.00");

                //Establecer certificado del emisor
                //Ruta establecida por defecto
                cfdi.UbicacionCertificado = @"..\..\..\..\..\..\Recursos\CSD-Pruebas\RFC-PAC-SC\Personas Fisicas\FIEL_CACX7605101P8_20190528152826\cacx7605101p8.cer";
                cfdi.UbicacionClavePrivada = @"..\..\..\..\..\..\Recursos\CSD-Pruebas\RFC-PAC-SC\Personas Fisicas\FIEL_CACX7605101P8_20190528152826\Claveprivada_FIEL_CACX7605101P8_20190528_152826.key";
                cfdi.ContrasenaClavePrivada = "12345678a";

                //Establecer Cuenta de Timbrado Induxsoft (CTI) y contraseña
                cfdi.CuentaTimbradoInduxsoft = "xipova";
                cfdi.ContrasenaCuentaTimbradoInduxsoft = "123456";
                
                var res = cfdi.Timbrar();
                string xm64 = res["xml"].ToString();
                System.IO.File.WriteAllText("C:/temp/CFDi40.xml", xm64);

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
