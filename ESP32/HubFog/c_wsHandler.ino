void webSocketEvent(uint8_t num, WStype_t type, uint8_t * payload, size_t length) {

  switch (type) {
    case WStype_DISCONNECTED: {
        USE_SERIAL.printf("[%u] Disconnected!\n", num);
        String mac = connectedEdges[num];
        String jsonmac = jsonfyMacadress(mac);
        Serial.println(jsonmac);
        mqttClient.publish("/cup/disconnect", jsonmac );
        connectedEdges[num] = "";
        break;
      }
    case WStype_CONNECTED:
      {
        IPAddress ip = webSocket.remoteIP(num);
        USE_SERIAL.printf("[%u] Connected from %d.%d.%d.%d url: %s\n", num, ip[0], ip[1], ip[2], ip[3], payload);
      }
      break;
    case WStype_TEXT: {
        if (mqttClient.isMqttConnected()) {
          String received = reinterpret_cast<const char*>(payload);
          DynamicJsonDocument json = toJsonEdge(received);
          String action = json["Action"];
          if (action == "Id") {
            String mac = json["Mac"];
            String jsonmac = jsonfyMacadress(mac);
            Serial.println(jsonmac);
            connectedEdges[num] = mac;
            mqttClient.publish("/cup/connect", jsonmac);
          } else if (action == "Temp") {
            String mac = json["Id"];
            int temp = json["Temp"];
            String sendJson = jsonfyTemp(mac, temp);
            Serial.println(sendJson);
            mqttClient.publish("/cup/temperature", sendJson);
          }

        }

        break;
      }
  }
}

void setupWebsocket() {
  webSocket.begin();
  webSocket.enableHeartbeat(15000, 3000, 2);
  webSocket.onEvent(webSocketEvent);

}
