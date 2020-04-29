
void SoundLocate() {
  for (int i = 250; i > 0 ; --i)
  {
    digitalWrite(buzzer, HIGH);
    delay(1);
    digitalWrite(buzzer, LOW);
    delay(1);
  }
}

void SoundTempViolation() {
  for (int i = 200; i > 0 ; --i)
  {
    digitalWrite(buzzer, HIGH);
    delay(2);
    digitalWrite(buzzer, LOW);
    delay(1);
  }
}

void SoundTempComfort() {
  for (int i = 250; i > 0 ; --i)
  {
    digitalWrite(buzzer, HIGH);
    delay(2);
    digitalWrite(buzzer, LOW);
    delay(2);
  }
}
