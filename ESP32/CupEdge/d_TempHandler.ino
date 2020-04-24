
void processTemp(){
  I = I + 1;
  delay(1000);
  if(I % 10){
    String json = jsonfyTemp(macaddres,100);
    Serial.println(json);
    webSocket.sendTXT(json);
  }
  
}
