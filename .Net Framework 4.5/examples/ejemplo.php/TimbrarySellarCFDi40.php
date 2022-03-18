<?php
$url = "https://factudesk.api.induxsoft.net/comprobantes/sellar-y-timbrar/";
$data = array(
	"cti" => "xipova",
	"pwd" => "123456",
	"idd"=>"", //
	"ncer"=>"",
    "nb64" => false, //si se establece en true el xml debe ser una cadena en formato json y si es false en base64
    //en el xml debe colocar los espacios de nombres
    "xml" => base64_encode('<?xml version="1.0" encoding="UTF-8"?>
<cfdi:Comprobante Version="4.0"
                  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                  xmlns:cfdi="http://www.sat.gob.mx/cfd/4"
                  xmlns:divisas="http://www.sat.gob.mx/divisas"
                  xmlns:donat="http://www.sat.gob.mx/donat"
                  xmlns:notariospublicos="http://www.sat.gob.mx/notariospublicos"
                  xmlns:implocal="http://www.sat.gob.mx/implocal"
                  xmlns:ine="http://www.sat.gob.mx/ine"
                  xmlns:cartaporte20="http://www.sat.gob.mx/CartaPorte20"
                  xsi:schemaLocation="http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd http://www.sat.gob.mx/divisas http://www.sat.gob.mx/sitio_internet/cfd/divisas/divisas.xsd http://www.sat.gob.mx/donat http://www.sat.gob.mx/sitio_internet/cfd/donat/donat11.xsd http://www.sat.gob.mx/notariospublicos http://www.sat.gob.mx/sitio_internet/cfd/notariospublicos/notariospublicos.xsd http://www.sat.gob.mx/implocal http://www.sat.gob.mx/sitio_internet/cfd/implocal/implocal.xsd http://www.sat.gob.mx/ine http://www.sat.gob.mx/sitio_internet/cfd/ine/ine11.xsd http://www.sat.gob.mx/CartaPorte20 http://www.sat.gob.mx/sitio_internet/cfd/CartaPorte/CartaPorte20.xsd"
                  Certificado=""
                  NoCertificado=""
                  Serie="F"
                  Folio="001"
                  Fecha="2022-03-16T17:42:47"
                  FormaPago="99"
                  CondicionesDePago="PAGO EN PARCIALIDADES O DIFERIDO"
                  TipoDeComprobante="I"
                  Moneda="MXN"
                  LugarExpedicion="29000"
                  Exportacion="01"
                  SubTotal="5.00"
                  Descuento="0.00"
                  Total="5.80"
                  MetodoPago="PPD"
                  Sello="">
	<cfdi:Emisor Nombre="EMISOR DE PRUEBAS"
	             Rfc="AAA010101AAA"
	             RegimenFiscal="601"/>
	<cfdi:Receptor Nombre="RECEPTOR DE PRUEBAS"
	               DomicilioFiscalReceptor="29000"
	               Rfc="RECE8002036T7"
	               UsoCFDI="G01"
	               RegimenFiscalReceptor="601"/>
	<cfdi:Conceptos>
		<cfdi:Concepto ClaveProdServ="01010101"
		               NoIdentificacion="PR02"
		               Cantidad="1.000000"
		               ClaveUnidad="H87"
		               Unidad="PZA"
		               Descripcion="BOCINAS PARA PC DE ESCRITORIO"
		               Importe="5.000000"
		               ValorUnitario="5.000000"
		               Descuento="0.00"
		               ObjetoImp="02">
			<cfdi:Impuestos>
				<cfdi:Traslados>
					<cfdi:Traslado TasaOCuota="0.160000"
					               Base="5.000000"
					               Importe="0.800000"
					               Impuesto="002"
					               TipoFactor="Tasa"/>
				</cfdi:Traslados>
			</cfdi:Impuestos>
		</cfdi:Concepto>
	</cfdi:Conceptos>
	<cfdi:Impuestos TotalImpuestosTrasladados="0.80">
		<cfdi:Traslados>
			<cfdi:Traslado Base="5.00"
			               Importe="0.80"
			               TipoFactor="Tasa"
			               TasaOCuota="0.160000"
			               Impuesto="002"/>
		</cfdi:Traslados>
	</cfdi:Impuestos>
</cfdi:Comprobante>')
);

$ch = curl_init($url);
//codifica en formato json los parametros a enviar
$jsonData = json_encode($data);
 //codigo para enviar los datos por metodo post y colocar los headers
curl_setopt($ch, CURLOPT_POSTFIELDS, $jsonData);
curl_setopt($ch, CURLOPT_HTTPHEADER, array('Content-Type:application/json'));
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
 //este codigo es por si el error certificate problem: unable to get local issuer certificate
curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, 0);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, 0);
	//*********************/
//ejecuta la llamada al web service
$result = curl_exec($ch);
//condicion para ver si durante la llamada del servcicio ocurrio un error
if(curl_errno($ch)){echo curl_error($ch);}

//para obtener los valores de data,que viene de la respuesta
 $res=json_decode($result,true);
 $data=$res["data"];
 //************
//imprimir la respuesta del web service
echo $result;
curl_close($ch);
?>
