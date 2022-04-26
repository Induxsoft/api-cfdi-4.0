<?php
$url = "https://factudesk.api.induxsoft.net/comprobantes/sellar-y-timbrar/";
$data = array(
    "cti" => "xipova",
    "pwd" => "123456",
		"idd"=>"",
		"ncer"=>"",
    "nb64" => false, //si se establece en true el xml debe ser una cadena en formato json y si es false en base64
    //en el xml debe colocar los espacios de nombres
    "xml" => base64_encode('<cfdi:Comprobante xmlns:cfdi="http://www.sat.gob.mx/cfd/4"
                  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                  xmlns:pago20="http://www.sat.gob.mx/Pagos20"
                  xsi:schemaLocation="http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd  http://www.sat.gob.mx/iedu http://www.sat.gob.mx/sitio_internet/cfd/iedu/iedu.xsd http://www.sat.gob.mx/Pagos20 http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos20.xsd"
                  xmlns:iedu="http://www.sat.gob.mx/iedu"
                  LugarExpedicion="29000"
                  TipoDeComprobante="P"
                  Total="0"
                  Moneda="XXX"
                  SubTotal="0"
                  Certificado=""
                  NoCertificado=""
                  Sello=""
                  Fecha="2022-03-07T17:23:34"
                  Folio="1028"
                  Serie="pago1"
                  Version="4.0"
                  Exportacion="01">
	<cfdi:Emisor RegimenFiscal="601"
	             Nombre="EMISOR DE PRUEBA"
	             Rfc="AAA010101AAA"/>
	<cfdi:Receptor Nombre="MARCO SARAOZ JUAREZ"
	               Rfc="MACO7010034T6"
	               UsoCFDI="CP01"
	               RegimenFiscalReceptor="605"
	               DomicilioFiscalReceptor="29000"/>
	<cfdi:Conceptos>
		<cfdi:Concepto Importe="0.00"
		               ValorUnitario="0"
		               Descripcion="Pago"
		               Cantidad="1"
		               ClaveUnidad="ACT"
		               ClaveProdServ="84111506"
		               ObjetoImp="01"/>
	</cfdi:Conceptos>
	<cfdi:Complemento>
		<pago20:Pagos Version="2.0">
			<pago20:Totales MontoTotalPagos="1.00"/>
			<pago20:Pago TipoCambioP="1"
			             NumOperacion="pago1-1028"
			             Monto="1.00"
			             MonedaP="MXN"
			             FormaDePagoP="01"
			             FechaPago="2022-03-07T17:23:34">
				<pago20:DoctoRelacionado EquivalenciaDR="1"
				                         Folio="12216"
				                         ImpSaldoInsoluto="1099.00"
				                         ImpPagado="1.00"
				                         ImpSaldoAnt="1100.00"
				                         NumParcialidad="3"
				                         MonedaDR="MXN"
				                         IdDocumento="XXXX860cb39eff034d0a9e9940a8a5a92c29"
				                         ObjetoImpDR="02">
					<pago20:ImpuestosDR>
						<pago20:TrasladosDR>
							<pago20:TrasladoDR BaseDR="1.00"
							                   ImpuestoDR="002"
							                   TipoFactorDR="Tasa"
							                   TasaOCuotaDR="0.16"
							                   ImporteDR="0.16"/>
						</pago20:TrasladosDR>
					</pago20:ImpuestosDR>
				</pago20:DoctoRelacionado>
			</pago20:Pago>
		</pago20:Pagos>
		</cfdi:Complemento>
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
  //condición para ver si durante la llamada del servcicio ocurrio un error
	if(curl_errno($ch)){echo curl_error($ch);}
	
 //para obtener los valores de data,que viene de la respuesta
 $res=json_decode($result,true);
 $data=$res["data"];

	//imprimir la respuesta del web service
	echo $result;
curl_close($ch);
?>
