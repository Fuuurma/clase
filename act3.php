

<?php

//inicializar variable de sesión 
session_start();

if (isset($_SESSION['people'])) 
{   //si existe la variable de sesión substituyo el contenido del array
    $people = $_SESSION['people'];
    ksort($people);
} 
else 
{   //array para guardar las personas
    $people = [];
}

//inicialización de variables
$name = $address = $nif = $name_modified = $address_modified = $nif_modified = null;

function show_error($message) 
{
    echo "<p>$message</p>";
}

//ALTA DE PERSONA
function add_person($nif, $name, $address, &$people) 
{
    foreach ($people as $person) 
	{   //validar que el nif no exista en la base de datos
        if ($person['nif'] === $nif) 
		{
            show_error("NIF: $nif ya existe.");
			return;
        }
    }

    //guardar la persona en el array
    $person = [
        'nif' => $nif,
        'nombre' => $name,
        'direccion' => $address
    ];
    $people[] = $person;
    $_SESSION['people'] = $people;

	//mensaje de alta efectuada
    echo "$name fue añadido.";
}

//BAJA DE LA PERSONA SELECCIONADA EN LA TABLA
	function delete_person($nif, &$people) 
{
    foreach ($people as $key => &$person) 
	{   //recuperar y validar el nif
        if ($person['nif'] === $nif) 
		{
            $person_to_delete = $people[$key];
			//borrar la fila del array
            unset($people[$key]);
            $_SESSION['people'] = $people;
	 		//mensaje de baja efectuada
            echo "{$person_to_delete['nombre']} fué eliminado.";
            return;
        }
    }

    show_error("NIF: $nif no fué ecnontrado.");
}

function modify_person($nif, $new_name, $new_address, &$people)
{
    $person_modified = false;

    foreach ($people as &$person) 
    {
        if ($person['nif'] === $nif) 
        {
            if (!empty($new_name)) 
            {
                $person['nombre'] = ucfirst(strtolower($new_name));
            }
            if (!empty($new_address)) 
            {
                $person['direccion'] = ucfirst(strtolower($new_address));
            }
            $person_modified = true;
            echo "Person modified: $new_name "; // aplies
        }
    }

    if ($person_modified == true) // Entra, aplica
    {
        echo "Persona con NIF: $nif fue modificada.";
        $_SESSION['people'] = $people;

    } 
    else 
    {
        show_error("No se encontró a la persona con NIF: $nif.");
    }
}


// Handle form submissions
if ($_SERVER['REQUEST_METHOD'] === 'POST') 
{
    if (isset($_POST['alta'])) 
	{
		//recuperar los datos sin espacios en blanco -trim()-, 1a Mayus y resto Minusculas
        $nif = trim($_POST["nif"]);
        $name = ucfirst(strtolower(trim($_POST["nombre"])));
        $address = ucfirst(strtolower(trim($_POST["direccion"])));

		//validar datos obligatorios
        if (!empty($nif) && !empty($name) && !empty($address)) 
		{
            add_person($nif, $name, $address, $people);
			//limpiar el formulario
			$name = $address = $nif = null;
        } 
		else 
		{
            show_error("Se deben completar todos los campos.");
        }
    }

	//BAJA DE TODAS LAS PERSONAS
    if (isset($_POST['bajas'])) 
	{
	// Limpiar el array de personas
		$people = [];
        $_SESSION['people'] = $people;
        echo "Lista eliminada.";
		$name = $address = $nif = null;

    }

	//BAJA DE PERSONA INDIVIDUAL
    if (isset($_POST['baja'])) 
	{
        $nif = trim($_POST["nif"]);

        if (!empty($nif)) 
		{
            delete_person($nif, $people);
			$name = $address = $nif = null;

        }   
		else 
		{
            show_error("No se encontró el NIF.");
        }
    }


	if (isset($_POST['nombreModi']) || (isset($_POST['direccionModi']))) 
    {
		$nif_modified = trim($_POST["nifModi"]);
		$name_modified = ucfirst(strtolower(trim($_POST["nombreModi"])));
		$address_modified = ucfirst(strtolower(trim($_POST["direccionModi"])));
	
		if ($nif_modified) 
        {
			modify_person($nif_modified, $name_modified, $address_modified, $people);
		} 
        else 
        {
			show_error("No se proporcionó el NIF para la modificación.");
		}
	}
	
}
			
			
			

	
		

	
			

	//MODIFICACION DE LA PERSONA SELECCIONADA
	
		//recuperar los datos sin espacios en blanco -trim()-
						
		//validar datos
			
		//validar que el nif no exista en la base de datos
			
		//guardamos el nombre y dirección en minúsculas con la primera letra en mayúsculas
			
		//modificar la persona en el array
			
			

	//CONSULTA DE PERSONAS

	//ordenar el array por nif
	
	//confeccionar la tabla con las personas del array
	

	//volcar el contenido del array en la variable de sesión

?>

<html>
<head>
	<title>PLA03</title>
	<meta charset='UTF-8'>
	<link rel="stylesheet" type="text/css" href="css/estilos.css">
</head>
<body>
	<div class='container'>
		<h1 class='centrar'>PLA03: MANTENIMIENTO PERSONAS</h1>
		<form method='post' action='#'>
			<label>NIF</label>
			<input type='text' name='nif' value='<?php echo $nif; ?>'><br>
			<label>Nombre</label>
			<input type='text' name='nombre' value='<?php echo $name; ?>'><br>
			<label>Dirección</label>
			<input type='text' name="direccion" value="<?php echo $address; ?>"><br>
			<input type='submit' name='alta' value='alta persona'>
			<span></span>
		</form><br><br>

		<table>
			<tr><th>NIF</th><th>Nombre</th><th>Dirección</th><th>Eliminar</th></tr>
			<?php
foreach ($people as $person ) 
{
    echo "<tr>";
    echo "<td>{$person['nif']}</td>";
    echo "<td><input type='text' class='nombre' value='{$person['nombre']}'></td>";
    echo "<td><input type='text' class='direccion' value='{$person['direccion']}'></td>";
    echo "<td>";
    echo "<form method='post' action='#'>";
    echo "<input type='hidden' name='nif' value='{$person['nif']}'>"; // NIF de la persona
    echo "<input type='submit' name='baja' value='Eliminar'>";
    echo "<input type='button' name='modificar' value='Modificar' class='modificar' onclick='trasladarDatos()' nif='{$person['nif']}'>";
    echo "</form>";
    echo "</td>";
    echo "</tr>";
}
?>

   

		</table><br>
		<form method='post' action='#' id='formularioBaja'>
			<input type='submit' value='baja personas' name='bajas'>
		</form>
		<!--FORMULARIO OCULTO PARA LA MODIFICACION-->
		<form method='post' action='#' id='formularioModi'>
            <input type='hidden' name='nifModi'>
            <input type='hidden' name='nombreModi'>
            <input type='hidden' name="direccionModi">
            <input type='hidden' name='modificar'>
        </form>
	</div>
	<script type="text/javascript" src='js/scripts.js'></script>
</body>
</html>

