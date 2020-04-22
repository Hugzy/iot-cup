void webSocketEvent(WStype_t type, uint8_t * payload, size_t length) {

  switch (type) {
    case WStype_DISCONNECTED:
      USE_SERIAL.printf("[WSc] Disconnected!\n");
      break;
    case WStype_CONNECTED:
      USE_SERIAL.printf("[WSc] Connected to url: %s\n", payload);

      // send message to server when Connected
      Serial.println(macaddres);
      //String jmacaddres = jsonfyMacadress(macaddres);
      webSocket.sendTXT(macaddres);
      break;
    case WStype_TEXT:
      USE_SERIAL.printf("[WSc] get text: %s\n", payload);

      // send message to server
      // webSocket.sendTXT("message here");
      break;
    case WStype_BIN: break;
    case WStype_ERROR: break;
    case WStype_FRAGMENT_TEXT_START: break;
    case WStype_FRAGMENT_BIN_START: break;
    case WStype_FRAGMENT: break;
    case WStype_FRAGMENT_FIN:
      break;
  }

}

void setupWebsocket() {
  // server address, port and URL
  webSocket.begin("192.168.1.135", 81, "/");

  // event handler
  webSocket.onEvent(webSocketEvent);

  // use HTTP Basic Authorization this is optional remove if not needed
  //webSocket.setAuthorization("user", "Password");

  // try ever 5000 again if connection has failed
  webSocket.setReconnectInterval(5000);
}
