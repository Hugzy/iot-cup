String jsonfyMacadress(String mac) {

  DynamicJsonDocument doc(jsonCap_SendMac);

  doc["Id"] = mac;
  String jsonfied;
  serializeJson(doc, jsonfied);
  return jsonfied;
}

String jsonfyTemp(String mac, int temp ) {
  DynamicJsonDocument doc(jsonCap_Send);
  doc["Id"] = mac;
  doc["Temp"] = temp;

  String stringfied;
  serializeJson(doc, stringfied);
  return stringfied;
}


String jsonfyLocate(String mac) {
  DynamicJsonDocument doc(jsonCap_Send);
  doc["Action"] = "locate";
  doc["Id"] = mac;

  String stringfied;
  serializeJson(doc, stringfied);
  return stringfied;
}


String jsonfyTempRange(String mac, int minTemp, int maxTemp ) {
  DynamicJsonDocument doc(jsonCap_Send);
  doc["Action"] = "tempConfig";
  doc["Id"] = mac;
  doc["MinTemp"] = minTemp;
  doc["MaxTemp"] = maxTemp;

  String stringfied;
  serializeJson(doc, stringfied);
  return stringfied;
}

String jsonfyBenchmarkToEdge(String mac, String ticks) {
  DynamicJsonDocument doc(jsonCap_Send);
  doc["Action"] = "benchmark";
  doc["Id"] = mac;
  doc["StartTicks"] = ticks;
  String stringfied;
  serializeJson(doc, stringfied);
  return stringfied;
}

String jsonfyBenchmarkToCloud(String mac, String ticks) {
  DynamicJsonDocument doc(jsonCap_Send);
  doc["Id"] = mac;
  doc["StartTicks"] = ticks;
  String stringfied;
  serializeJson(doc, stringfied);
  return stringfied;
}

DynamicJsonDocument toJsonEdge(String json) {
  
  DynamicJsonDocument doc(jsonCap_ReceiveEdge);
  deserializeJson(doc, json);
  return doc;
}

DynamicJsonDocument toJsonCloud(String json) {
  
  DynamicJsonDocument doc(jsonCap_ReceiveCloud);
  deserializeJson(doc, json);
  return doc;
}
