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
    console.log('Out ' + name + '  ' + timestamp);
    timestamp = timestamp.replace('T', ' ').replace('Z', '').substring(0, timestamp.length-5);
    var mainDiv = document.getElementById('main');
    if(amountOfCards%5 === 0){
        mainDiv.innerHTML += '<div class="w-100 d-none d-xl-block"><!-- wrap every 5 on xl--></div>'
    }
    mainDiv.innerHTML += '<div class="card border-danger mb-3" style="width: 15rem;">\n' +
        '  <div class="card-body">\n' +
        `    <h5 class="card-title">${cup.mac}</h5>\n` +
        '    <div>\n' +
        `    <p class="card-text" style="float: left; margin-right: 2rem">${timestamp}</p>\n` +
        '      <span class="badge badge-danger" style="float: left; margin-top: 5px">Disconnected</span>\n' +
        '    </div>\n' +
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
        `    <h5 class="card-title">${cup.mac}</h5>\n` +
        '    <div>\n' +
        `    <p class="card-text" style="float: left; margin-right: 2rem">${cup.name}</p>\n` +
        '      <span class="badge badge-primary" style="float: left; margin-top: 5px">Connected</span>\n' +
        '    </div>\n' +
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

