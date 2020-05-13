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
    String sendJson = jsonfyTempRange(mac, minTemp, maxTemp);
    webSocket.sendTXT(getWsClient(mac), sendJson);
  });

  //Subscribe to "/cup/locate" and display received message to Serial
  mqttClient.subscribe("/cup/locate", [](const String & payload) {
    String received = payload;
    USE_SERIAL.println(received);
    DynamicJsonDocument json = toJsonCloud(received);
    //Find the corresponding edge
    String mac = json["Id"];
    String sendJson = jsonfyLocate(mac);
    webSocket.sendTXT(getWsClient(mac), sendJson);
  });

    //Subscribe to "/cup/benchmark" for benchmark
  mqttClient.subscribe("/cup/benchmark", [](const String & payload) {
    String received = payload;
    USE_SERIAL.println(received);
    DynamicJsonDocument json = toJsonCloud(received);
    //Find the corresponding edge
    String mac = json["Id"];
    String ticks = json["StartTicks"];
    String sendJson = jsonfyBenchmarkToEdge(mac,ticks);
    webSocket.sendTXT(getWsClient(mac), sendJson);
  });

}

int getWsClient(String mac) {
  // Find the connected client with the given mac-address
  for (int i = 0; i < sizeof(connectedEdges); i++) {
    if (connectedEdges[i] == mac) {
      return i;
    }
  }
}
