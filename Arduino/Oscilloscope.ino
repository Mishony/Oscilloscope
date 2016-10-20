/*
PINOUT:
   RST - LCD RST
   15 - SCK  -> LCD SCK
   16 - MOSI -> LCD SDA
   10 - LCD CS
   7 - LCD DC (A0)
   5 - PWM out
   4 (A6) - Resistor meter

   3 - SCL
   2 - SDA
   
   A0 - channel 1
   A1 - channel 2
   A2 - channel 3
   A3 - Vcc measure
*/

#include <Wire.h>
#include <Adafruit_INA219.h>

#include <SPI.h>
#include <PDQ_GFX.h>  
enum
{
  ST7735_INITB      = 0,        // 1.8" (128x160) ST7735B chipset (only one type)
  ST7735_INITR_GREENTAB   = 1,        // 1.8" (128x160) ST7735R chipset with green tab (same as ST7735_INITR_18GREENTAB)
  ST7735_INITR_REDTAB   = 2,        // 1.8" (128x160) ST7735R chipset with red tab (same as ST7735_INITR_18REDTAB)
  ST7735_INITR_BLACKTAB   = 3,        // 1.8" (128x160) ST7735S chipset with black tab (same as ST7735_INITR_18BLACKTAB)
  ST7735_INITR_144GREENTAB    = 4,        // 1.4" (128x128) ST7735R chipset with green tab
  ST7735_INITR_18GREENTAB   = ST7735_INITR_GREENTAB,  // 1.8" (128x160) ST7735R chipset with green tab
  ST7735_INITR_18REDTAB   = ST7735_INITR_REDTAB,    // 1.8" (128x160) ST7735R chipset with red tab
  ST7735_INITR_18BLACKTAB   = ST7735_INITR_BLACKTAB,  // 1.8" (128x160) ST7735S chipset with black tab
};
#define ST7735_CHIPSET    ST7735_INITR_BLACKTAB
#define  ST7735_CS_PIN   10
#define  ST7735_DC_PIN   7
#define  ST7735_SAVE_SPCR  0
#include <PDQ_ST7735.h>

static const int TXT_SIZE = 2;

static const int SCREEN_WIDTH = 128;
static const int SCREEN_HEIGHT = 160;
static const int CHAR_WIDTH =  6 * TXT_SIZE;
static const int CHAR_HEIGHT = 8 * TXT_SIZE;

static const int CHARS_X = SCREEN_WIDTH / CHAR_WIDTH;
static const int CHARS_Y = SCREEN_HEIGHT / CHAR_HEIGHT;

PDQ_ST7735 display;
Adafruit_INA219 sensor(0x40);

const unsigned char PS_16 = (1 << ADPS2);
const unsigned char PS_32 = (1 << ADPS2) | (1 << ADPS0);
const unsigned char PS_64 = (1 << ADPS2) | (1 << ADPS1);
const unsigned char PS_128 = (1 << ADPS2) | (1 << ADPS1) | (1 << ADPS0);

char adc_rate = '1';

void SetADCRate(char rate)
{
  adc_rate = rate;
  byte val;
  if (rate == '0') val = PS_16;
  else if (rate == '1') val = PS_32;
  else if (rate == '2') val = PS_64;
  else val = PS_128;
  ADCSRA &= ~PS_128;
  ADCSRA |= val;
}

#define VREF 2.56
#define R1 3715
#define R2 3950
#define DARK 0xA514 //0xC618 //0x7BEF
static const byte CHAN_PIN[4] = { A0, A1, A2, A3 };
static const char *CHAN_NAME[4] = { "CH1", "CH2", "CH3", "VCC" };
static const uint16_t CHAN_CLR[4] = { ST7735_GREEN, ST7735_RED, ST7735_BLUE, ST7735_WHITE };
static const float CHAN_DIV[4] = { 10.722, 11.0, 1.96, 1.955 };

char pwm_val = '0';

void setup() {
  Serial.begin(921600);
  SetADCRate('1');
  analogReference(INTERNAL);

  pinMode(5, OUTPUT);
  digitalWrite(5, LOW);

  sensor.begin();

  display.initR(ST7735_CHIPSET);
  //display.setRotation(3);
  display.fillScreen(ST7735_BLACK);

//  for (int i = 0; i < 255; ++i) {
//    display.print((char) i);
//    if ((i % 16) == 15) display.println();
//  }
//  return;
  
  display.setTextSize(TXT_SIZE);   

  display.drawFastHLine(0, 33, SCREEN_WIDTH, ST7735_WHITE & DARK);
  for (int i = 1; i < 3; ++i ) 
    display.drawFastVLine(1 + 42 * i, 0, 32, ST7735_WHITE & DARK);
  display.drawFastHLine(0, 39 + 3 * CHAR_HEIGHT, SCREEN_WIDTH, ST7735_WHITE & DARK);  
  display.drawFastHLine(0, 45 + 4 * CHAR_HEIGHT, SCREEN_WIDTH, ST7735_WHITE & DARK);  
}

#define BUFSIZE 32
unsigned int buf[42];

char mode = '0';
char last_mode = mode;

unsigned long last_recv = 0;

int chan = 0;

void loop() {    
  if (Serial.available())
    last_recv = micros();
  while (Serial.available()) {
    char cmd = Serial.read();
    if (cmd == '?') {
      Serial.write("OSCL");
      Serial.write(adc_rate);
      Serial.write(pwm_val);
      continue;
    }
    if (cmd == 'R') {
      char rate = Serial.read();
      SetADCRate(rate);
      continue;
    }
    if (cmd == 'P') {
      pwm_val = Serial.read();
      if (pwm_val <= '0')
        digitalWrite(5, LOW);
      else if (pwm_val <= '9')
        analogWrite(5, (pwm_val - '0') * 255 / 10);
      continue;
    }
    if (cmd < '0' || cmd > '5') continue;
    mode = cmd;
  }

  if (pwm_val > '9')
    digitalWrite(5, !digitalRead(5));
    
  if (micros() - last_recv > 500000)
    mode = '0';

//  if (last_mode != mode) {
//    last_mode = mode;
//    if (mode == '0' || mode == '5')
//      analogReference(DEFAULT);
//    else
//      analogReference(INTERNAL);
//  }

  if (mode == '0') {
    uint16_t clr;
    int xofs = 3;
    int yofs = 38;
    if (chan == 3) {
      yofs += 6;
      float VCC = analogRead(CHAN_PIN[3]) * CHAN_DIV[3] * VREF / 1023.0;
      display.setCursor(xofs, 3 * CHAR_HEIGHT + yofs);
      clr = CHAN_CLR[3];
      display.setTextColor(clr, ST7735_BLACK);  
      display.print("VCC:"); display.print(VCC); display.print("V");

      yofs += 4;
      float busvoltage = sensor.getBusVoltage_V();
      float current_mA = sensor.getCurrent_mA();
      //float shuntvoltage = sensor.getShuntVoltage_mV();
      //float loadvoltage = busvoltage + (shuntvoltage / 1000);
      
      display.setCursor(xofs, 4 * CHAR_HEIGHT + yofs);
      clr = ST7735_YELLOW;
      if (busvoltage < 1.5)
        clr &= DARK;
      display.setTextColor(clr, ST7735_BLACK);  
      display.print("VIN:"); display.print(busvoltage); display.print("V");
      if (busvoltage < 10) display.print(' ');
      
      display.setCursor(xofs, 5 * CHAR_HEIGHT + yofs);
      clr = ST7735_CYAN;
      if (current_mA < 1)
        clr &= DARK;
      display.setTextColor(clr, ST7735_BLACK);  
      display.print("CUR:");
      if (current_mA < 1000) {
        int mA = (int) current_mA;
        display.print(mA);
        display.print("mA");
        if (mA < 100) display.print(' ');
        if (mA < 10) display.print(' ');
      } else {
        display.print(current_mA / 1000);
        display.print("A");
      }

      display.setCursor(xofs, 6 * CHAR_HEIGHT + yofs);
      unsigned long uiRes = 0;
      for (int i = 0; i < 32; ++i) {
        uiRes += analogRead(A6);
        delayMicroseconds(100);
      }
      float fRes = uiRes / 32.0;
      if (fRes > 5) {
        clr = ST7735_WHITE;
        display.setTextColor(clr, ST7735_BLACK);
        float v = fRes * VREF / 1023;
        long R = VCC * R2 / v - R1 - R2;
        display.print("RES:");
        if (R < 1000) {
          if (R < 0) R = 0;
          R = R / 10 * 10;
          display.print(R);
          display.print((char) 233);
          if (R < 100) display.print(' ');
          if (R < 10) display.print(' ');
          display.print(' ');  
        } else if (R < 10000) {
          display.print(R / 1000);
          display.print('.');
          display.print((R / 100) % 10);
          display.print('K');
          display.print(' ');
        } else if (R < 1000000) {
          display.print(R / 1000);
          display.print("K");
          display.print(' ');
        } else {
          display.print(R / 1000000.0);
          display.print('M');
        }
      } else {
        clr = ST7735_MAGENTA;
        if (current_mA < 1)
          clr &= DARK;
        display.setTextColor(clr, ST7735_BLACK);
        display.print("PWR:");
        float power_mW = busvoltage * current_mA;
        if (power_mW < 1000) {
          int mW = (int) power_mW;
          display.print(mW);
          display.print("mW");
          if (mW < 100) display.print(' ');
          if (mW < 10) display.print(' ');
        } else {
          display.print(power_mW / 1000);
          display.print("W");
        }
      }
      
      chan = 0;
      return;
    }
    
    byte pin = CHAN_PIN[chan];
    analogRead(pin);
    delayMicroseconds(100); 
    buf[0] = analogRead(pin);
    unsigned int uiMin = buf[0];
    unsigned int uiMax = buf[0];
    unsigned int uiSum = buf[0];
    int idxMin = 0;
    bool bEdgeFound = false;
    for (int i = 1; i < 42; ++i) {
      unsigned int ui = analogRead(pin);
      buf[i] = ui;
      if (ui < uiMin) { uiMin = ui; idxMin = i; }
      if (ui > uiMax) uiMax = ui;
      uiSum += ui;
      if (!bEdgeFound && ui > uiMin + 150) {
        bEdgeFound = true;
        buf[0] = buf[i - 1];
        buf[1] = ui;
        i = 1;
        uiMax = ui;
        uiSum = buf[0] + ui;;
      }
      delayMicroseconds(100); 
    }
    float vAvg = (uiSum / 42.0) * CHAN_DIV[chan] * VREF / 1023.0;

    display.setCursor(xofs, chan * CHAR_HEIGHT + yofs);
    clr = CHAN_CLR[chan];
    if (vAvg < 0.25)
      clr &= DARK;
    display.setTextColor(clr, ST7735_BLACK);
    display.print(CHAN_NAME[chan]); display.print(":"); display.print(vAvg);
    display.print("V");
    if (vAvg < 10) display.print(' ');

    unsigned int yPrev;
    uiMin = 0;
    uiMax = 1023;
    for (int i = 0; i < 42; ++i) {
      unsigned int y = 32 - (buf[i] - uiMin) * 32 / (uiMax - uiMin);
      if (!i) {
        yPrev = y;
        continue;
      }
      unsigned int y1, y2;
      if (y < yPrev) {
        y1 = y;
        y2 = yPrev;
      } else {
        y1 = yPrev;
        y2 = y;
      }
      yPrev = y;
      unsigned int x = 1 + 42 * chan + i;
      display.drawFastVLine(x, 0, y1, ST7735_BLACK);
      display.drawFastVLine(x, y1, y2 - y1 + 1, CHAN_CLR[chan]);
      display.drawFastVLine(x, y2 + 1, 32 - y2, ST7735_BLACK);
    }
    
    ++chan;
    return;
  }

  byte i = 0; 
  //unsigned long tmStart = micros();
  if (mode == '1') {
    while (i < BUFSIZE) {
      unsigned int val1 = analogRead(A0);
      buf[i++] = val1; 
    } 
  } else if (mode == '2') {
    while (i < BUFSIZE) {
      unsigned int val1 = analogRead(A1);
      buf[i++] = val1; 
    } 
  } else if (mode == '3') {
    while (i < BUFSIZE) {
      unsigned int val1 = analogRead(A0);
      unsigned int val2 = analogRead(A1); 
      buf[i++] = val1; 
      buf[i++] = val2; 
    }    
  } else if (mode == '4') {
    while (i < BUFSIZE) {
      unsigned int val1 = analogRead(A0); 
      unsigned int val2 = analogRead(A1);
      buf[i++] = (val2 - val1);
    }    
  } else if (mode == '5') {
    while (i < BUFSIZE) {
      unsigned int val1 = analogRead(A2);
      buf[i++] = val1;
    }
  }
  //unsigned long tm = micros() - tmStart;
  Serial.write((byte *) buf, BUFSIZE * 2);

//  Serial.println();
//  Serial.println(tm); 
}
