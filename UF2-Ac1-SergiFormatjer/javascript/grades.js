// Notas Exámen


let operation = document.getElementById('submit-operation'); // boton calcular
operation.addEventListener('click', Calculate);

let totalNumber =  0; // inicia varibales
let totalPct =  0;
let totalResult = undefined;


function Calculate() 
{
    
    if (!totalResult)
    {
        totalResult = {};
    }

    let number = parseFloat(document.getElementById('number').value); // los valores pasados por el user
    let percentage = parseFloat(document.getElementById('percentage').value);

    result = CalculateThePct(number, percentage).toFixed(2);

    let resultText = `${number} * ${percentage}% = ${result}`;

    let resultBox = document.getElementById('operations-container'); // box operaciones
    let totalContainer = document.getElementById('total-result-container'); // box resultado final

    let operation = document.createElement('p'); // crea elemento, asigna los valores y añade el 'p' al documento
    operation.innerText = resultText;
    resultBox.appendChild(operation);

    if (CheckMaxPercentage(totalPct + percentage)) // para que NO se pueda añadir mas del 100%
    {
        totalNumber = true ? 
            totalNumber += parseFloat(result) : // ya tiene valores. los sumamos
            totalNumber = parseFloat(result); // primer valor es = al resultado.

        totalPct = true ? // igual para el pct
            totalPct += percentage : 
            totalPct = percentage;

        
        totalResult = `${totalNumber.toFixed(2)} - ${totalPct}%`;
    }

    else // pct es as grande que 100
    {
        let errorMessage = `Lo siento, el porcentaje no puede ser superior a 100%\nActual: ${totalPct}`;
        alert(errorMessage);
        let errorBox = document.createElement('p');
        errorBox.innerText = errorMessage;
        totalContainer.appendChild(errorBox);    
    }

    
    
    let totalText = totalResult; // container del texto = resultado
    
    let totalBox = document.createElement('p'); // mostramos los valores
    totalBox.innerText = totalText;
    totalContainer.innerText = '';
    totalContainer.appendChild(totalBox);


    function CalculateThePct(number, percentage) // helper para calcular pct
{
    let result = (number * percentage) / 100 ;
    return result;  
}

function CheckMaxPercentage(totalPct) // helper para pct < 100
{
    const maxPercentage = 100;

    if (totalPct > maxPercentage)
    {
        return false;
    }  

    return true;
}
    
}







