#define SENSOR_LEFT_A_PIN 13
#define SENSOR_LEFT_B_PIN 12
#define SENSOR_RIGHT_A_PIN 8
#define SENSOR_RIGHT_B_PIN 9

#define MIN_MICROS 70000
#define MAX_MICROS 500000
#define LEFT_CW_THRESHOLD 0.45f
#define RIGHT_CW_THRESHOLD 0.45f

void setup() {
  Serial.begin(9600);

  pinMode(SENSOR_LEFT_A_PIN, INPUT_PULLUP);
  pinMode(SENSOR_LEFT_B_PIN, INPUT_PULLUP);

  pinMode(SENSOR_RIGHT_A_PIN, INPUT_PULLUP);
  pinMode(SENSOR_RIGHT_B_PIN, INPUT_PULLUP);

  Serial.println("startup done");
}

unsigned long lastleftATime = 0;
unsigned long lastleftBTime = 0;
bool leftPinA = false;
bool leftPinB = false;
unsigned long lastrightATime = 0;
unsigned long lastrightBTime = 0;
bool rightPinA = false;
bool rightPinB = false;

void loop() {
  unsigned long now = micros();

  if (!digitalRead(SENSOR_LEFT_A_PIN)) {
    if (!leftPinA) {
      leftPinA = true;
      
      unsigned long diff = now - lastleftATime;
      unsigned long bdiff = now - lastleftBTime;
      if (diff > MIN_MICROS && diff < MAX_MICROS) {
        unsigned long cw_threshold = (unsigned long)((float)diff * LEFT_CW_THRESHOLD);
        bool cw = bdiff > cw_threshold;
        // Serial.print("left:");
        // if (cw) Serial.print("-");
        // Serial.println(diff);
        // Serial.print(d);
        // Serial.print(" ");
        // print_dir(diff, bdiff, cw);
        Serial.print("left:");
        if (cw) Serial.print("-");
        Serial.println(diff);
      }
      lastleftATime = now;
    }
  }
  else {
    leftPinA = false;
  }

  if (!digitalRead(SENSOR_LEFT_B_PIN)) {
    if (!leftPinB) {
      leftPinB = true;
      lastleftBTime = now;
    }
  }
  else {
    leftPinB = false;
  }

  if (!digitalRead(SENSOR_RIGHT_A_PIN)) {
    if (!rightPinA) {
      rightPinA = true;
      
      unsigned long diff = now - lastrightATime;
      unsigned long bdiff = now - lastrightBTime;
      if (diff > MIN_MICROS && diff < MAX_MICROS) {
        unsigned long cw_threshold = (unsigned long)((float)diff * RIGHT_CW_THRESHOLD);
        bool cw = bdiff > cw_threshold;
        Serial.print("right:");
        if (cw) Serial.print("-");
        Serial.println(diff);
        // print_dir(diff, bdiff, bdiff > cw_threshold);
      }
      lastrightATime = now;
    }
  }
  else {
    rightPinA = false;
  }

  if (!digitalRead(SENSOR_RIGHT_B_PIN)) {
    if (!rightPinB) {
      rightPinB = true;
      lastrightBTime = now;
    }
  }
  else {
    rightPinB = false;
  }
}




void print_dir(unsigned long m, unsigned long b, bool c) {
  // if (c) Serial.print("-");
  // Serial.println(m);
  Serial.print("speed ");
  Serial.print((unsigned long)m);
  Serial.print(" diff ");
  Serial.print((unsigned long)b);
  Serial.print(c ? " cw" : " ccw");
  Serial.println();
}
