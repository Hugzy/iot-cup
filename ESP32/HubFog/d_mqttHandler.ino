void onConnectionEstablished()
{
  //Subscribe to "/cup/temprange" and display received message to Serial
  mqttClient.subscribe("/cup/temprange", [](const String & payload) {
    String received = payload;
    USE_SERIAL.println(received);
    DynamicJsonDocument json = toJsonCloud(received);
    //Find the corresponding edge
    String mac = json["Id"];
    int minTemp = json["MinTemp"];
    int maxTemp = json["MaxTemp"];
    
    for (int i = 0; i < sizeof(connectedEdges); i++) {
      if (connectedEdges[i] == mac) {
        String sendJson = jsonfyTempRange(mac, minTemp, maxTemp);
        webSocket.sendTXT(i, sendJson);
        break;
      }
    }
  });

}
