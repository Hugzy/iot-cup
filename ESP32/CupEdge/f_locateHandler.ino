
void processLocate() {
  if (locateCup) {
    digitalWrite(LED_LOCATE, HIGH);
    SoundLocate();
    digitalWrite(LED_LOCATE, LOW);
    locateCup = false;
  }

}
