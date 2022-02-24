using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Usar el espacio de nombres de Induxsoft CFDI 4.0
using induxsoft.cfdi.v40;

namespace ejemplo.cfdi40.nomina
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
            Comprobante.XSLT_CadenaOriginal = @"..\..\..\..\..\..\Recursos\xslt\cadenaoriginal.xslt";

            try
            {
                //Se crea el comprobante
                Comprobante cfdi = new Comprobante();

                //Llenar datos del CFDI
                cfdi.Serie = "F";
                cfdi.Folio = "12948";
                cfdi.Fecha = DateTime.Now;
                cfdi.NoCertificado = "30001000000400002300";
                cfdi.Moneda = "MXN";
                cfdi.SubTotal = 3.3m;
                cfdi.Descuento = 0.6m;
                cfdi.TipoDeComprobante = "N";
                cfdi.MetodoPago = "PUE";
                cfdi.LugarExpedicion = "29039";
                cfdi.Exportacion = "01";
                cfdi.Total = 2.7m;

                //Datos del emisor
                cfdi.Emisor.Rfc = "CACX7605101P8";
                cfdi.Emisor.RegimenFiscal = "601";
                cfdi.Emisor.Nombre = "XOCHILT CASAS CHAVEZ";

                //Datos del receptor
                cfdi.Receptor.Rfc = "LAMP931125IX9";
                cfdi.Receptor.Nombre = "PABLO LARA AGUILAR";
                cfdi.Receptor.UsoCFDI = "CN01";
                cfdi.Receptor.RegimenFiscalReceptor = "605";
                cfdi.Receptor.DomicilioFiscalReceptor = "29010";

                //Datos del Concepto
                Concepto c = cfdi.CreateElement<Concepto>();
                c.ClaveProdServ = "84111505";
                c.Cantidad = 1;
                c.ClaveUnidad = "ACT";
                c.Descripcion = "Pago de nómina";
                c.ObjetoImp = "01";
                c.ValorUnitario = 3.3m;
                c.Importe = 3.3m;
                c.Descuento = 0.6m;
                
                //Se agrega el concepto como nodo al nodo conceptos
                cfdi.Conceptos.Add(c);

                //Complemento de Nómina
                induxsoft.cfdi.v40.Complementos.nomina12.Nomina nomina = new induxsoft.cfdi.v40.Complementos.nomina12.Nomina();
                nomina.FechaInicialPago = new DateTime(2021, 01, 01);
                nomina.FechaFinalPago = new DateTime(2023, 01, 01);
                nomina.FechaPago = new DateTime(2021, 01, 10);
                nomina.NumDiasPagados = 15;
                nomina.TipoNomina = "O";

                //Datos del emisor de la nómina
                nomina.Emisor.RegistroPatronal = "1234567890";

                //Datos del receptor de la nómina
                nomina.Receptor.Curp = "LAMP931125MCSRML03";
                nomina.Receptor.TipoContrato = "01";
                nomina.Receptor.TipoRegimen = "02";
                nomina.Receptor.NumEmpleado = "10";
                nomina.Receptor.PeriodicidadPago = "04";
                nomina.Receptor.ClaveEntFed = "CHP";
                nomina.Receptor.NumSeguridadSocial = "1234567890";
                nomina.Receptor.FechaInicioRelLaboral = new DateTime(2020,01,16);
                nomina.Receptor.Antigüedad = "P56W";
                nomina.Receptor.RiesgoPuesto = "1";
                nomina.Receptor.SalarioDiarioIntegrado = 0.20m;

                //Datos de persepción del nodo percepciones de la nómina
                induxsoft.cfdi.v40.Complementos.nomina12.Percepcion percepcion = new induxsoft.cfdi.v40.Complementos.nomina12.Percepcion();
                percepcion.Clave = "001";
                percepcion.Concepto = "Sueldo Ordinario";
                percepcion.ImporteExento = 0;
                percepcion.ImporteGravado = 3.3m;
                percepcion.TipoPercepcion = "001";

                //Se agrega la persepción como nodo al nodo persepciones de la nómina
                nomina.Percepciones.Add(percepcion);

                //Datos de percepciones de la nómina
                nomina.Percepciones.TotalExento = 0;
                nomina.Percepciones.TotalGravado = 3.3m;
                nomina.Percepciones.TotalSueldos = 3.3m;

                nomina.TotalPercepciones = 3.3m;

                //Datos de deducciones de la nómina
                nomina.Deducciones.TotalOtrasDeducciones = 0.6m;
                induxsoft.cfdi.v40.Complementos.nomina12.Deduccion deduccion = new induxsoft.cfdi.v40.Complementos.nomina12.Deduccion();
                deduccion.Clave = "IVAT";
                deduccion.Concepto = "Invalidez y Vida a Cargo del Trabajador";
                deduccion.Importe = 0.6m;
                deduccion.TipoDeduccion = "004";

                //Se agrega la deducción como nodo al nodo deducciones de la nómina
                nomina.Deducciones.Add(deduccion);
                nomina.TotalDeducciones = 0.6m;

                //Datos de OtroPago
                induxsoft.cfdi.v40.Complementos.nomina12.OtroPago otroPago = new induxsoft.cfdi.v40.Complementos.nomina12.OtroPago();
                otroPago.Clave = "SUBPE";
                otroPago.Concepto = "Subsidio para el Empleo";
                otroPago.Importe = 0.00m;
                otroPago.TipoOtroPago = "002";

                //Datos de SudsidioAlEmpleo
                otroPago.SubsidioAlEmpleo.SubsidioCausado = 0.10m;

                //Se agrega el OtroPago como nodo al nodo OtrosPagos de la nómina
                nomina.OtrosPagos.Add(otroPago);

                nomina.TotalOtrosPagos = 0;

                //Se agrega el Complemento de nómina al comprobante 
                cfdi.Complemento.Add(nomina);

                //Establecer certificado del emisor
                cfdi.UbicacionCertificado = @"..\..\..\..\..\..\Recursos\CSD-Pruebas\RFC-PAC-SC\Personas Fisicas\FIEL_CACX7605101P8_20190528152826\cacx7605101p8.cer";
                cfdi.UbicacionClavePrivada = @"..\..\..\..\..\..\Recursos\CSD-Pruebas\RFC-PAC-SC\Personas Fisicas\FIEL_CACX7605101P8_20190528152826\Claveprivada_FIEL_CACX7605101P8_20190528_152826.key";
                cfdi.ContrasenaClavePrivada = "12345678a";

                //Establecer Cuenta de Timbrado Induxsoft (CTI) y contraseña
                cfdi.CuentaTimbradoInduxsoft = "xipova";    // Cuenta de pruebas, no obtiene un TFD válido
                cfdi.ContrasenaCuentaTimbradoInduxsoft = "123456";

                /*
                Para únicamente sellar, invoque al método Sellar, requerirá haber indicado una clave de licencia.
                cfdi.Sellar();
                */

                // El método Timbrar() realiza el sellado y obtención del TFD del SAT; 
                // puede invocarse sin establecer una clave de licencia

                var res = cfdi.Timbrar();


                string archivo = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), res["uuid"].ToString()+".xml");

                System.IO.File.WriteAllText(archivo, Encoding.UTF8.GetString(Convert.FromBase64String(res["xml"].ToString())));

                //Intentar abrir el xml con el programa predeterminado
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
