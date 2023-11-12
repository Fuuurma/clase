


//activar listener del boton de baja de todas las personas
var botonesModi = document.querySelectorAll('.modificar');
//activar listener de los botones de modificación de persona
botonesModi.forEach(function(item) 
    {
        item.onclick = function()
        {
            trasladarDatos(this.getAttribute('nif'));
        } 
    });
// función para trasladar los datos de la fila seleccionada al formulario oculto
function trasladarDatos(nif) 
{   
    
    let button = document.querySelector(`[nif='${nif}']`);
    let tr = button.closest('tr');
    
    // Retrieve values from the specific row
    let name = tr.querySelector('.nombre').value;
    let address = tr.querySelector('.direccion').value;

    console.log('nif:', nif);
    console.log('name:', name);
    console.log('address:', address);
    
    document.querySelector('[name=nifModi]').value = nif;
    document.querySelector('[name=nombreModi]').value = name;
    document.querySelector('[name=direccionModi]').value = address ;
   

    console.log('nifModi:', document.querySelector('[name=nifModi]').value);
    console.log('nombreModi:', document.querySelector('[name=nombreModi]').value);
    console.log('direccionModi:', document.querySelector('[name=direccionModi]').value);

    // document.querySelector('#formularioModi').submit();

    setTimeout(() => {
        document.querySelector('#formularioModi').submit()
    }, 1000);

}

