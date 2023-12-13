/*
 UF 2 Actividad 2 - Sergi Formatjer 
 Funcional 
    Hay 4 Divs, cada uno con una utiliadad difeente
    1. Crear un Robot, con nombre, modelo y fecha de fabricacion
    2. Selector de Robots disponibles/creados.
        Con btotones para: Agregar una tarea a la lista. Ver Datos del robot y cambiarle el nombre al robot.
    3. Lista de tareas en cola del robot actual. Cada tarea lleva un boton para ejecutarla
    4. Lista de tareas ejecutadas, con un timestamp de cuando fue realiazada. 

*/



class Robot 
{
    constructor(id, model, manufacturedDate) 
    {
        this.Name = id;
        this.Model = model;
        this.ManufacturedDate = manufacturedDate;
        this.Tasks = [] ; 
        this.TasksLog = new Map() ; // https://www.freecodecamp.org/news/how-to-use-javascript-collections-map-and-set/
    }

    sayHi() // devuelve los datos del robot. Al dar al boton Ver Robot
    {
        const robotBox = document.getElementById('robot-details');
        let robotData = `<h3>${this.Name}</h3>
                        <p>Modelo: ${this.Model}</p>
                        <p>Fecha de Fabricacion: ${this.ManufacturedDate}</p>`;

        robotBox.innerHTML = robotData;
    }

    addTask() // añade la tarea del robot a la lista
    {
        const task = prompt("Introduce la tarea:");
        if (task) 
        {
            this.Tasks.push(task);
            this.updateTaskList();
        }
    }

    updateTaskList() // actualiza la lista de tareas del robot
    {
        const robotTaskList = document.getElementById('robot-task-list');
        robotTaskList.innerHTML = "";

        // por cada tarea. Tenemos la tarea y posicion e la lista
        this.Tasks.forEach((task, index) => {
            const listItem = document.createElement('li'); // creamos una fila en el html
            listItem.classList.add('task-item');  // le asignamos una clase 
    
            const taskContent = document.createElement('span'); // en la fila habra texto + boton
            taskContent.textContent = `${index + 1}. ${task}`; // orden + tarea -> 1. Hacer la cama
            
    
            const executeButton = document.createElement('button'); // crea yun boton que llamará al metodo
            executeButton.textContent = 'Hacer tarea';              // de la clase para ejecutar dicha tarea
            
            executeButton.addEventListener('click', () => this.executeTask(index));

            listItem.appendChild(taskContent); // añade los 2 objectos a la fila
            listItem.appendChild(executeButton);

            robotTaskList.appendChild(listItem); // añade la fila a la lista
        });
    }
    
    updateTaskLog() // igual que Task-List pero para el Task-Log
    {
        const taskLogList = document.getElementById('task-log-list');
        taskLogList.innerHTML = "";

        let index = 1;
        this.TasksLog.forEach((task, timestamp) =>{
            const listItem = document.createElement('li');
            listItem.classList.add('task-item'); 

            const taskContent = document.createElement('span');
            taskContent.textContent = `${timestamp} - ${index}. ${task}.`;
            listItem.appendChild(taskContent);
            taskLogList.appendChild(listItem);
            index++;
        });
    }
    

    executeTask(index) // ejecuta la tarea. @Params es el indice en Array de la tarea
    {
        if (index >= 0 && index < this.Tasks.length) // index dentro de rango
        {
            const taskToExecute = this.Tasks[index]; // pilla la tarea
            alert(`${this.Name} hace la tarea: ${taskToExecute}`); // alerta
            // quita de la lista tareas
            this.Tasks.splice(index, 1); // Array.splice(ArrayIndex, Qty To Delete, Items to Add..N)

            let timestamp = new Date();
            timestamp = timestamp.toLocaleTimeString();
            this.TasksLog.set(timestamp, taskToExecute); // añade tarea a tareas realizadas con Timestamp

            this.updateTaskList(); // actualiza ambas
            this.updateTaskLog();
        } 
        else 
        {
            alert("Número de tarea no válido");
        }
    }
}


// Funciones para el programa e interactuar con el html

let robotList = []; // instancias de los robots
let currentRobot = null; // robot sobre el que se trabja
const robotNameTask = document.getElementById("current-robot-task");
const robotNameTaskLog = document.getElementById("current-robot-log");

const createRobot = () => {    // recoge los datos
    const nameInput = document.getElementById('name');
    const modelInput = document.getElementById('model');
    const manufacturedDateInput = document.getElementById('manufactured-date');

    const name = nameInput.value;
    const model = modelInput.value;
    const manufacturedDate = manufacturedDateInput.value;

    const newRobot = new Robot(name, model, manufacturedDate); // instancia
    robotList.push(newRobot); // añade a lista

    updateRobotSelector(); // input selector para mostarr ese nuevo robot.

    setCurrentRobot(robotList.length - 1); // nuevo robot creado es el robot sobre el que se trabaja
    // reset variables
    nameInput.value = '';
    modelInput.value = '';
    manufacturedDateInput.value = '';
}

const updateRobotSelector = () => { // actualiza el selector del inout  
    const selector = document.getElementById('robot-selector');
    selector.innerHTML = "";
    // muestra el selector con los nombres de robot pero sus valores son el index en la lista
    robotList.forEach((robot, index) => { 
        const option = document.createElement('option');
        option.value = index;
        option.text = robot.Name;
        selector.add(option);
    });
};


const showRobot = () => currentRobot.sayHi(); // para el boton ver robot



const setCurrentRobot = (index) => { // importante. Selecciona sobre que instancia de robot se trabajara   
    // seleciona al robot y actualiza la lista de tareas
    currentRobot = robotList[index];
    currentRobot.updateTaskList();
    currentRobot.updateTaskLog();
    
    // Tambien actualiza con el nombre del robbot cada div
    robotNameTask.innerText = currentRobot.Name;
    robotNameTaskLog.innerText = currentRobot.Name;
};


const addTask = () => {  // al boton Agregar Tarea
    currentRobot ? currentRobot.addTask() // ejecuta el metodo AddTask del robbot
                 : alert("Selecciona un robot para añadirle una tarea.");
};

const changeRobotName = () => {  // cambia el nombre a la instancia
    if (currentRobot) 
    {
        const newName = prompt("Nuevo nombre para el robot:");
        if (newName) 
        {
            currentRobot.Name = newName;
            updateRobotSelector();
            robotNameTask.innerText = currentRobot.Name;
            robotNameTaskLog.innerText = currentRobot.Name;
        }
    } 
    else 
    {
        alert("Selecciona un robot para cambiarle el nombre");
    }
};



