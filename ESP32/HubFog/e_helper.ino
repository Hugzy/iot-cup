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
  doc["temp"] = temp;

  String stringfied;
  serializeJson(doc, stringfied);
  return stringfied;
}

DynamicJsonDocument toJson(String json) {
  
  DynamicJsonDocument doc(jsonCap_ReceiveEdge);
  deserializeJson(doc, json);
  return doc;
}
