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
            Comprobante.XSLT_CadenaOriginal = "";

            try
            {
                Comprobante cfdi = new Comprobante();

                //Llenar datos del CFDI
                cfdi.Emisor.Nombre = "";

                //Establecer certificado del emisor
                cfdi.UbicacionCertificado = "archivo.cer";
                cfdi.UbicacionClavePrivada = "archivo.key";
                cfdi.ContrasenaClavePrivada = "contraseña de la clave privada";

                //Establecer Cuenta de Timbrado Induxsoft (CTI) y contraseña
                cfdi.CuentaTimbradoInduxsoft = "xipova";
                cfdi.ContrasenaCuentaTimbradoInduxsoft = "123456";
                
                var res = cfdi.Timbrar();

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
