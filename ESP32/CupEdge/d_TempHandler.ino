int numberOfTempSamples = 0;
int total_temperature = 0;
int currentTemperature = 0;
void processCollectTemp() {
  int temperature = 0.0;
  temperature = analogRead(39);
  total_temperature = total_temperature + temperature;
  numberOfTempSamples = numberOfTempSamples + 1;

  if (numberOfTempSamples == 100) {
    temperature = total_temperature / numberOfTempSamples;
    String json = jsonfyTemp(macaddres, temperature);
    currentTemperature = temperature; // Update the current temperature
    //Serial.println(json);
    numberOfTempSamples = 0;
    total_temperature = 0.0;
    webSocket.sendTXT(json);
  }
}


void monitorTemp() {
 
  if (currentTemperature >= ComfortMinTemp && currentTemperature <= ComfortMaxTemp) {
    Serial.println("Temperature is violated");
  }
}
