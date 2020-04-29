
void processLocate() {
  if (locateCup) {
    digitalWrite(LED_LOCATE, HIGH);
    for (int i = 250; i > 0 ; --i)
    {
      digitalWrite(buzzer, HIGH);
      delay(1);
      digitalWrite(buzzer, LOW);
      delay(1);
    }
    digitalWrite(LED_LOCATE, LOW);
    locateCup = false;
  }

}
