
void setup() {
  setupSoundLight();
  setupSerial();
  setupWifi();
  setupWebsocket();
  setupThreads();

}

void setupSoundLight() {
  pinMode(buzzer, OUTPUT);
  pinMode (LED_LOCATE, OUTPUT);
  digitalWrite(LED_LOCATE, LOW);
}

void setupThreads() {
  tempCollectThread->onRun(processCollectTemp);
  tempCollectThread->setInterval(100);

  tempMonitorThread->onRun(monitorTemp);
  tempMonitorThread->setInterval(1000);

  locaterThread->onRun(processLocate);
  locaterThread->setInterval(1000);
}

void setupSerial() {
  USE_SERIAL.begin(115200);
  //Serial.setDebugOutput(true);
  USE_SERIAL.setDebugOutput(true);

  USE_SERIAL.println();
  USE_SERIAL.println();
  USE_SERIAL.println();

  for (uint8_t t = 4; t > 0; t--) {
    USE_SERIAL.printf("[SETUP] BOOT WAIT %d...\n", t);
    USE_SERIAL.flush();
    delay(1000);
  }
}

void setupWifi() {

  WiFi.begin(ssid, password);

  //WiFi.disconnect();
  while ( WiFi.status() != WL_CONNECTED) {
    delay(100);
  }
  Serial.println(WiFi.localIP());
  macaddres = WiFi.macAddress();
}
