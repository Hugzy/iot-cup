String jsonfyMacadress(String mac) {
  DynamicJsonDocument doc(jsonCap_send);
  doc["Action"] = "Id";
  doc["Mac"] = mac;

  String stringfied;
  serializeJson(doc, stringfied);
  return stringfied;
}


String jsonfyTemp(String mac, int temp ) {
  DynamicJsonDocument doc(jsonCap_send);
  doc["Action"] = "Temp";
  doc["Id"] = mac;
  doc["Temp"] = temp;

  String stringfied;
  serializeJson(doc, stringfied);
  return stringfied;
}

DynamicJsonDocument toJson(String json) {
  
  DynamicJsonDocument doc(jsonCap_Receive);
  deserializeJson(doc, json);
  return doc;
}
