/*
   Represent a MQTT Client and a Websocket server
*/

#include <Arduino.h>

#include <WiFi.h>

#include <WebSocketsServer.h>
#include <ArduinoJson.h>
// Wifi const
const char* ssid = "PV_WLan";
const char* password =  "H5@dA56aQ";



WebSocketsServer webSocket = WebSocketsServer(81);

//Json Capacity
const size_t jsonCap_Connected = JSON_OBJECT_SIZE(1);


#define USE_SERIAL Serial




void loop() {
  webSocket.loop();
}
