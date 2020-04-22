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
const size_t jsonCap_Connected = JSON_OBJECT_SIZE(1);


#define USE_SERIAL Serial


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
