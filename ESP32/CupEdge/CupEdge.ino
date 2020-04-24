/*
 * Represent a Websocket client
 *  
 *
 */
#include <Arduino.h>
#include <WiFi.h>

#include <WebSocketsClient.h>
#include <ArduinoJson.h>


// Wifi const
const char* ssid = "PV_WLan";
const char* password =  "H5@dA56aQ";
String macaddres;

// Websocket
WebSocketsClient webSocket;



//Json Capacity
const size_t jsonCap_Connected = JSON_OBJECT_SIZE(2);


#define USE_SERIAL Serial



void loop() {
  webSocket.loop();
}
