<?php 
//ejemplo
$url = "https://factudesk.api.induxsoft.net/comprobantes/descargar/";
$data=array(
	//"ids"=>"ids válido",
	"uid"=>"uid (usuario)",
	"pwd"=>"pwd (contraseña)",
	"doc"=>"uuid a descargar", //uuid a descargar
	"tpo"=>"", //parámetro opcional, si se omite se toma que se trata de un cfdi
	"res"=>"ziplnk",//tipo de respuesta, puede cambiar el tipo de respuesta que guste, de acuerdo a la descripción del github sobre el parámetro res 
	"pln"=>"" //parámetro opcional.
	);
//esto es igual a un formulario html 
$opciones = array(
    "http" => array(
        "header" => "Content-type: application/x-www-form-urlencoded\r\n",
        "method" => "POST",
        "content" => http_build_query($data), # Agregar el contenido definido antes
    ),
);
# Preparar petición
$contexto = stream_context_create($opciones);

//*****para ver el flujo durante la invocacion
 $flujo = fopen($url, 'r', false, $contexto);
 stream_set_blocking($flujo,false);
 //***************

//*******************respuesas en formato json
 $resultado = file_get_contents($url, false, $contexto);
 // $res=json_decode($resultado);
$data=json_decode($resultado, true); 
echo json_encode($data);
if ($data["success"] == false) {
    echo "Error:".$data["message"];
    exit;
}

# si fue existoso
if($data["success"]==true){
	echo "<br>";
	echo "<br>";
	echo "<label>Puede descargar el zip dando click en el boton </label>";
	echo "<a href='".$data["data"]["link"]."'><button style='background:green;'>Descargar</button></a>";
}
//este codigo para descargar automáticamente sin el botón
 // header("Location:".$data["data"]["link"]);
//*************************
 ?>