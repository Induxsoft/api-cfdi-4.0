using induxsoft.cfdi.v40;
using induxsoft.cfdi.v40.Complementos.cartaporte20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ejemplo.cfdi40.cartaporte20
{
    class Program
    {
        static void Main(string[] args)
        {
            Comprobante.NIC = "201373";
            Comprobante.Licencia = "YzNty5tiKeZqOgAv2bumxA==";
            //string cerfile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+ @"\GitHub\cfdi-4.0-api\Recursos\CSD-Pruebas\RFC-PAC-SC\Personas Fisicas\FIEL_LIÑI920228KS8_20190614170612\liñi920228ks8.cer";
            //string keyfile=  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+ @"\GitHub\cfdi-4.0-api\Recursos\CSD-Pruebas\RFC-PAC-SC\Personas Fisicas\FIEL_LIÑI920228KS8_20190614170612\Claveprivada_FIEL_LIÑI920228KS8_20190614_170612.key";
            //string cerpwd= "12345678a";
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
                cfdi.FormaPago = "01";
                cfdi.NoCertificado = "30001000000400002335";
                cfdi.CondicionesDePago = "Inmediato";
                
                
                cfdi.Moneda = "MXN";
                cfdi.SetAttribute("SubTotal", "1.00");
                cfdi.TipoDeComprobante = "I";
                cfdi.MetodoPago = "PUE";
                cfdi.LugarExpedicion = "29039";
                cfdi.Exportacion = "01";
                cfdi.SetAttribute("Total", "1.51");

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
                c.ClaveProdServ = "10101500";
                c.SetAttribute("Cantidad", "1");
                c.ClaveUnidad = "H87";
                c.Descripcion = "Prueba 1";
                c.ObjetoImp = "02";
                c.SetAttribute("ValorUnitario", "1.00");
                c.SetAttribute("Importe", "1.00");
                TrasladoConcepto trasladoConcepto = new TrasladoConcepto();
                trasladoConcepto.Base = 1.30m;
                trasladoConcepto.Impuesto = "002";
                trasladoConcepto.TipoFactor = "Tasa";
                trasladoConcepto.TasaOCuota = 0.160000m;
                trasladoConcepto.SetAttribute("Importe", "0.21");

                TrasladoConcepto trasladoConcepto1 = new TrasladoConcepto();
                trasladoConcepto1.Base = 1.00m;
                trasladoConcepto1.Impuesto = "003";
                trasladoConcepto1.TipoFactor = "Tasa";
                trasladoConcepto1.TasaOCuota = 0.300000m;
                trasladoConcepto1.SetAttribute("Importe", "0.30");

                c.ImpuestosConcepto.TransladosConcepto.Add(trasladoConcepto);
                c.ImpuestosConcepto.TransladosConcepto.Add(trasladoConcepto1);

                cfdi.Conceptos.Add(c);



                cfdi.Impuestos.SetAttribute("TotalImpuestosRetenidos","0.00");
                cfdi.Impuestos.SetAttribute("TotalImpuestosTrasladados","0.51");

                Traslado traslado = cfdi.CreateElement<Traslado>();
                traslado.Base = 1.30m;
                traslado.Impuesto = "002";
                traslado.TipoFactor = "Tasa";
                traslado.TasaOCuota = 0.160000m;
                traslado.SetAttribute("Importe", "0.21");
                cfdi.Impuestos.Traslados.Add(traslado);

                Traslado traslado1 = cfdi.CreateElement<Traslado>();
                traslado1.Base = 1.00m;
                traslado1.Impuesto = "003";
                traslado1.TipoFactor = "Tasa";
                traslado1.TasaOCuota = 0.300000m;
                traslado1.SetAttribute("Importe", "0.30");
                cfdi.Impuestos.Traslados.Add(traslado1);


                //cartaporte20
                CartaPorte cartaPorte = new CartaPorte();
                cartaPorte.TotalDistRec = Convert.ToDecimal("0.1");
                cartaPorte.TranspInternac = "No";

                Ubicacion ubicacionOrigen = new Ubicacion();
                ubicacionOrigen.TipoUbicacion = "Origen";
                ubicacionOrigen.SetAttribute("FechaHoraSalidaLlegada","2021-12-07T12:50:30");
                ubicacionOrigen.NombreRemitenteDestinatario = "Miguel angel morales cruz";
                ubicacionOrigen.RFCRemitenteDestinatario = "IDS1402204R4";
                cartaPorte.Ubicaciones.Add(ubicacionOrigen);



                Ubicacion ubicacionDestino = new Ubicacion();
                ubicacionDestino.TipoUbicacion = "Destino";
                ubicacionDestino.SetAttribute("FechaHoraSalidaLlegada", "2021-12-07T12:50:30");
                ubicacionDestino.NombreRemitenteDestinatario = "Martin de la cruz malles";
                ubicacionDestino.RFCRemitenteDestinatario = "SIC150828HX3";
                cartaPorte.Ubicaciones.Add(ubicacionDestino);

                cartaPorte.Mercancias.NumTotalMercancias = 1;
                cartaPorte.Mercancias.Mercancia.ClaveSTCC = "000001";
                cartaPorte.Mercancias.Mercancia.BienesTransp = "10101500";
                cartaPorte.Mercancias.Mercancia.PesoEnKg = Convert.ToDecimal("10");
                cartaPorte.Mercancias.Mercancia.Descripcion = "saaa";
                cartaPorte.Mercancias.Mercancia.Cantidad = 1;
                cartaPorte.Mercancias.Mercancia.MaterialPeligroso = "No";
                cartaPorte.Mercancias.PesoBrutoTotal = 10;
                cartaPorte.Mercancias.UnidadPeso = "KGM";
                cartaPorte.Mercancias.Mercancia.ClaveUnidad = "30";

                TiposFigura tiposFigura = new TiposFigura();
                tiposFigura.TipoFigura = "01";
                tiposFigura.SetAttribute("RFCFigura","LAMP800229876");
                tiposFigura.NombreFigura = "Candelario Hernandez jaurez";
                cartaPorte.FiguraTransporte.Add(tiposFigura);
                cfdi.Complemento.Add(cartaPorte);


                System.IO.File.WriteAllText(@"C:\Users\Dev\Documents\GitHub\cfdi-4.0-api\.Net Framework 4.5\examples\ejemplo.cfdi40.cartaporte20\3.xml", cfdi.toXml());
                var res = cfdi.Timbrar();
                string v = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\GitHub\cfdi-4.0-api\.Net Framework 4.5\examples\ejemplo.cfdi40.pagos20\" + res["uuid"].ToString() + ".xml";
                System.IO.File.WriteAllText(v, cfdi.toXml());
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
