/*
   Represent a MQTT Client and a Websocket server
*/

#include <Arduino.h>

#include <WiFi.h>

#include <WebSocketsServer.h>
#include <ArduinoJson.h>
#include "EspMQTTClient.h"



// Wifi const
const char* ssid = "PV_WLan";
const char* password =  "H5@dA56aQ";

// MQTT
const char* brokerHost =  "167.172.184.103";

WebSocketsServer webSocket = WebSocketsServer(81);

//Json Capacity
const size_t jsonCap_SendMac = JSON_OBJECT_SIZE(4) + 80;
const size_t jsonCap_Send = JSON_OBJECT_SIZE(4) + 80;
const size_t jsonCap_ReceiveEdge = JSON_OBJECT_SIZE(4) + 80;
const size_t jsonCap_ReceiveCloud = JSON_OBJECT_SIZE(4) + 80;

#define USE_SERIAL Serial
String connectedEdges[10];

EspMQTTClient mqttClient(
  ssid,
  password,
  brokerHost,  // MQTT Broker server ip
  "MQTTUsername",   // Can be omitted if not needed
  "MQTTPassword",   // Can be omitted if not needed
  "TestClient",     // Client name that uniquely identify your device
  1883              // The MQTT port, default to 1883. this line can be omitted
);


void loop() {
  mqttClient.loop();
  webSocket.loop();

}
