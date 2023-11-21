// sandwatch simulation


let doStuff = document.getElementById('submit-value'); // boton del input-group
doStuff.addEventListener("click", doSandWatch);


function doSandWatch() 
{
    let inputNumber = document.getElementById('input-number').value; // numero dado por el user

    const symbol = "⌛";


    let canvas = document.getElementById('display-sandwatch');
    canvas.innerHTML = ''; // vacia el div


    for (let i = inputNumber; i >= 0; i--) 
    { 
        let symbols = [symbol.repeat(i)]; // [*,*,*,*] (InputNumber)
        let line = document.createElement('p'); // nuevo parrafo
        let toPrint = symbols.join(" "); // hace la array una str con espacios
        line.innerText = toPrint; // la str al parrafo
        canvas.appendChild(line); // el parrafo al div
        symbols.pop(); // quitamos el ultimo elemento de la array
    }

    for (let i = 2; i <= inputNumber; i++) 
    {
        let symbols = [symbol.repeat(i)];
        let line = document.createElement('p'); 
        let toPrint = symbols.join(" "); 
        line.innerText = toPrint; 
        canvas.appendChild(line); 
        symbols.push(symbol); // añade un *simbolo* mas

    }
}