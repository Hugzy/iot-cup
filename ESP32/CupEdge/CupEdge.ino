/*
 * Represent a Websocket client
 *  
 *
 */
#include <Arduino.h>
#include <WiFi.h>

#include <WebSocketsClient.h>
#include <ArduinoJson.h>

#include <Thread.h>
#include <StaticThreadController.h>

// Wifi const
const char* ssid = "PV_WLan";
const char* password =  "H5@dA56aQ";
String macaddres;

// Websocket
WebSocketsClient webSocket;

//Json Capacity
const size_t jsonCap_send = JSON_OBJECT_SIZE(4) + 80;
const size_t jsonCap_Receive= JSON_OBJECT_SIZE(4) + 80;

// Thread
Thread* tempCollectThread = new Thread();
Thread* tempMonitorThread = new Thread();
Thread* locaterThread = new Thread();

StaticThreadController<3> controll (tempCollectThread, tempMonitorThread, locaterThread);

#define USE_SERIAL Serial

int ComfortMinTemp = 0;
int ComfortMaxTemp = 0;

int LED_LOCATE = 32;
const int buzzer = 27;  // Buzzer pin

bool locateCup = false;


void loop() {
  webSocket.loop();
  controll.run();
  
}
