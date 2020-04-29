var amountOfCards = 0;
const host = "http://localhost:80"

                    // Create a <li> node


class Cup {
    constructor(mac,name,mintemp,maxtemp,connected){
        this.mac = mac;
        this.name = name;
        this.mintemp = mintemp;
        this.maxtemp = maxtemp;
        this.connected = connected;
    }
}


function disconnectedCupElementCreator(cup){
    var mainDiv = document.getElementById('main');

    if(amountOfCards%5 === 0){
        mainDiv.innerHTML += '<div class="w-100 d-none d-xl-block"><!-- wrap every 5 on xl--></div>'
    }
    mainDiv.innerHTML += '<div class="card border-danger mb-3" style="width: 15rem;">\n' +
        '  <div class="card-body">\n' +
        `    <h5 class="card-title">${cup.name}</h5>\n` +
        '    <div>\n' +
        `    <p class="card-text">Mac-Address: ${cup.mac}</p>\n` +
        '      <span class="badge badge-danger">Disconnected</span>\n' +
        '    </div>\n' +
        `    <ul id="devices-info-${cup.mac}" class="list-group list-group-flush">\n ` +
        `        <li id="current-temp-element-${cup.mac}" class="list-group-item">Current-Temp: undefined °C</li>\n` +
        `        <li class="list-group-item">Max-Temp: ${cup.maxtemp} °C</li>\n ` +
        `        <li class="list-group-item">Min-Temp: ${cup.mintemp} °C</li>\n ` +
        `    </ul>\n` +
        '<div></div>' +
        '  </div>\n' +
        '  <p style="font-family: \'Helvetica Neue\';font-size: 12px;text-align: center">Made by: JFD</p>\n' +
        '</div>\n'

    fetchTemperature(cup);

    amountOfCards += 1;
}

function connectedCupElementCreator(cup){
    var mainDiv = document.getElementById('main');
    if(amountOfCards%5 === 0){
        mainDiv.innerHTML += '<div class="w-100 d-none d-xl-block"><!-- wrap every 5 on xl--></div>'
    }
    mainDiv.innerHTML += '<div class="card border-primary mb-3" style="width: 15rem;">\n' +
        '  <div class="card-body">\n' +
        `    <h5 class="card-title">${cup.name}</h5>\n` +
        '    <div>\n' +
        `    <p class="card-text">Mac-Address: ${cup.mac}</p>\n` +
        '      <span class="badge badge-primary">Connected</span>\n' +
        '    </div>\n' +
        `    <ul id="devices-info-${cup.mac}" class="list-group list-group-flush">\n ` +
        `        <li id="current-temp-element-${cup.mac}" class="list-group-item">Current-Temp: undefined °C</li>\n` +
        `        <li class="list-group-item">Max-Temp: ${cup.maxtemp} °C</li>\n ` +
        `        <li class="list-group-item">Min-Temp: ${cup.mintemp} °C</li>\n ` +
        `    </ul>\n` +
        `    <a href="${host}/config.html?id=${cup.mac}" class="btn btn-primary" style="margin-top: 1rem">Configure</a>` +
        `    <button type="button" class="btn btn-primary" onclick="locateCup('${cup.mac}')" style="margin-top: 1rem">Locate Cup</button>` +
        '  </div>\n' +
        '  <p style="font-family: \'Helvetica Neue\';font-size: 12px;text-align: center">Made by: JFD</p>\n' +
        '</div>\n'

    setInterval(function(){ fetchTemperature(cup) }, 1000);

    amountOfCards += 1;
}

function locateCup(id){
    fetch(host+'/api/cup/locate/'+id,{method: 'POST'})
}

function clearMainDiv() {
    var mainDiv = document.getElementById('main');
    mainDiv.innerHTML = '';
    amountOfCards = 0;
}

async function connect() {
    var resp = await fetch(host+'/api/cup');
    resp.json().then(cups =>{
        for (i = 0; i < cups.length; i++){
            var mac = cups[i].id;
            var name = cups[i].displayName;
            var mintemp = cups[i].minTemp;
            var maxtemp = cups[i].maxTemp;
            var connected = cups[i].connected;
            if(name === null){
                name = 'Name not set';
            }
            var cup = new Cup(mac,name,mintemp,maxtemp,connected);
            if(cup.connected){
                connectedCupElementCreator(cup);
            } else {
                disconnectedCupElementCreator(cup);
            }
        }
    })
}

connect();

function fetchTemperature(cup) {
    fetch(host + '/api/cup/' + cup.mac + '/temperature')
        .then(response => {return response.json()})
        .then(json => {return json[0]})
        .then(jsonElem => {
            var oldChild = document.getElementById(`current-temp-element-${cup.mac}`);
            document.getElementById(`devices-info-${cup.mac}`).removeChild(oldChild);
            var newChild =  document.createElement("LI");
            newChild.id = `current-temp-element-${cup.mac}`;
            newChild.className = "list-group-item";
            var currentTempNode = document.createTextNode(`Current-Temp: ${jsonElem.temp.toFixed(2)} °C`);
            newChild.appendChild(currentTempNode);
            document.getElementById(`devices-info-${cup.mac}`).appendChild(newChild)
        });
}

