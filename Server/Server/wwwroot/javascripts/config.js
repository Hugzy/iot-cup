const urlParams = new URLSearchParams(window.location.search);
const cupMacAddress = urlParams.get('id');
const host = "http://localhost:80"

class Cup {
    constructor(mac, name, mintemp, maxtemp, connected) {
        this.mac = mac;
        this.name = name;
        this.mintemp = mintemp;
        this.maxtemp = maxtemp;
        this.connected = connected;
    }
}

function connectedCupElementCreator(cup) {
    var mainDiv = document.getElementById('main');
    mainDiv.innerHTML += `<form action="${host}/api/cup/update/${cup.mac}" method="post">\n` +
        '<div class="form-group">\n' +
        '<label for="InputName">Cup Name</label>\n' +
        '<input type="text" class="form-control" name="InputName" id="InputName" placeholder="Enter new name">\n' +
        '</div>\n' +
        '<div class="form-row">\n' +
        '<div class="col">\n' +
        '<label for="MaxTemp">Max Temp</label>\n' +
        `<input type="number"  class="form-control" name="MaxTemp" id="MaxTemp" value="${cup.maxtemp}">\n` +
        '</div>\n' +
        '<div class="col">\n' +
        '<label for="MinTemp">Min Temp</label>\n' +
        `<input type="number" class="form-control"  name="MinTemp"  id="MinTemp" value="${cup.mintemp}">\n` +
        '</div>\n' +
        '</div>\n' +
        `<button type="submit"  class="btn btn-primary" style="margin-top: 1rem">Submit</button>\n` +
        '</form>\n'
}

function disconnectedCupElementCreator(cup) {
    var mainDiv = document.getElementById('main');
}


async function fetchInitialCup() {
    var resp = await fetch(host + '/api/cup/' + cupMacAddress);
    resp.json().then(cup => {
        console.log(cup)
        var mac = cup.id;
        var name = cup.displayName;
        var mintemp = cup.minTemp;
        var maxtemp = cup.maxTemp;
        var connected = cup.connected;
        var cupObj = new Cup(mac, name, mintemp, maxtemp, connected);
        if (cup.connected) {
            connectedCupElementCreator(cupObj);
        } else {
            disconnectedCupElementCreator(cupObj);
        }
    });
}

fetchInitialCup();
