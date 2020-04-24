String jsonfyMacadress(String mac) {
  DynamicJsonDocument doc(jsonCap_send);
  doc["action"] = "id";
  doc["mac"] = mac;

  String stringfied;
  serializeJson(doc, stringfied);
  return stringfied;
}


String jsonfyTemp(String mac, int temp ) {
  DynamicJsonDocument doc(jsonCap_send);
  doc["action"] = "temp";
  doc["Id"] = mac;
  doc["temp"] = temp;

  String stringfied;
  serializeJson(doc, stringfied);
  return stringfied;
}
