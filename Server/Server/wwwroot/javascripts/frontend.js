var amountOfCards = 0;
var host = "http://localhost:80"
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
        `    <ul class="list-group list-group-flush">\n ` +
        `        <li class="list-group-item">Max-Temp: ${cup.maxtemp}</li>\n ` +
        `         <li class="list-group-item">Min-Temp: ${cup.mintemp}</li>\n ` +
        `    </ul>\n` +
        '  </div>\n' +
        '  <p style="font-family: \'Comic Sans MS\';font-size: 10px;text-align: center">Made by: JFD</p>\n' +
        '</div>\n'
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
        `    <ul class="list-group list-group-flush">\n ` +
        `        <li class="list-group-item">Max-Temp: ${cup.maxtemp}</li>\n ` +
        `         <li class="list-group-item">Min-Temp: ${cup.mintemp}</li>\n ` +
        `    </ul>\n` +
        `    <a href="${host}/config.html?id=${cup.mac}" class="btn btn-primary" style="margin-top: 1rem">Configure</a>` +
        '  </div>\n' +
        '  <p style="font-family: \'Comic Sans MS\';font-size: 10px;text-align: center">Made by: JFD</p>\n' +
        '</div>\n'
    amountOfCards += 1;
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

