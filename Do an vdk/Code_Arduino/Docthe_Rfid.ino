#include <LiquidCrystal_I2C.h>
#include <SPI.h>
#include <Servo.h>
#include <MFRC522.h>
#include <Wire.h> 

LiquidCrystal_I2C lcd(0x27,20,4);  // set the LCD address to 0x27 for a 16 chars and 2 line display

#define RST_PIN         9
#define SS_PIN          10

int quangtro = A0;
// khai bao bien 

String inputString = "";         // a string to hold incoming data
boolean stringComplete = false;  // whether the string is complete
String commandString = "";
int pos = 0;
int cuoi = 0;
Servo myservo;
Servo myservoOut;
MFRC522 mfrc522(SS_PIN, RST_PIN);

void setup() {

Serial.begin(9600);
SPI.begin();
pinMode(quangtro, INPUT);
mfrc522.PCD_Init();
myservo.attach(6);// dieu kieu servo bang cach nap vao chan 7
myservoOut.attach(4);
//Serial.println("Khởi tạo thành công, đang chờ đọc thẻ…");
 lcd.init();                      // initialize the lcd 
 lcd.backlight();
 lcd.clear();
}
void loop() {
  //cap dien cho chan laser
// doc chi so cua quang tro

myservoOut.write(90);
if(stringComplete)
{
  stringComplete = false;
  getCommand();
//  switch(message)
  if(commandString.equals("1"))
  {
    lcd.clear();
    String text = "";
    text = getTextToPrint();
    printText(text);
    cuoi = 90;
    controllServoOpen(myservo,90);
    delay(5000);
    printText("Your welcome!");
    controllServoCllose(myservo,90);
  }
  else if(commandString.equals("2"))
  {
    lcd.clear();
    String text = "";
    text = getTextToPrint();
    printText(text); 
    controllServoOpen(myservoOut,90);
    delay(5000);
    controllServoCllose(myservoOut,90);   
  } 
  else if(commandString.equals("3"))
  {
    lcd.clear();
    String text = getTextToPrint();
    printMessenge(text);
      
  } 
  inputString = "";
}else{
  if ( ! mfrc522.PICC_IsNewCardPresent()) {

return;

}

if ( ! mfrc522.PICC_ReadCardSerial()) {

return;

}

for (byte i = 0; i < mfrc522.uid.size; i++) {

      Serial.print(mfrc522.uid.uidByte[i], HEX);

}
int value = analogRead(quangtro);//lưu giá trị cảm biến vào biến value
if(value > 300){
  delay(100);
  if(analogRead(quangtro) > 300){
    Serial.print("A");
  }
  }
  Serial.println("");

mfrc522.PICC_HaltA();
mfrc522.PCD_StopCrypto1();
delay(5000);
}

}
// ham lay text o vi tri thu 1 ->5
void getCommand()
{
  if(inputString.length()>0)
  {
     commandString = inputString.substring(1,2);
  }
}
//cat chuoi tu vi tri thu 5 den het
String getTextToPrint()
{
  String value = "";
  int firstClosingBracket = inputString.indexOf(':');
   value = inputString.substring(firstClosingBracket+1,inputString.length()-1);
  return value;
}

// ham in text  thong tin khach hang len lcd
void printText(String text)
{
  lcd.clear();
  lcd.setCursor(0,0);
    if(text.length()<16)
    {
      lcd.print(text);
    }else
    { int firstClosingBracket = text.indexOf(',');
      lcd.print(text.substring(0,firstClosingBracket));
      lcd.setCursor(0,1);
      lcd.print(text.substring(firstClosingBracket+1,text.length()));
    }
}
void printMessenge(String text)
{
  lcd.clear();
  lcd.setCursor(0,0);
    if(text.length()<16)
    {
      lcd.print(text);
    }else
    { 
      lcd.print(text.substring(0,16));
      lcd.setCursor(0,1);
      lcd.print(text.substring(16,text.length()));
    }
}
// ham in text messege thong bao cho khach hang len lcd
// ham dieu khien servo ra cong
void controllServoCllose(Servo isserVo, int cuoi1){
    for(int pos1 = 0 ; pos1 < cuoi1; pos1 += 1)  // cho servo quay từ 0->179 độ
  {                                  // mỗi bước của vòng lặp tăng 1 độ
    isserVo.write(pos1);              // xuất tọa độ ra cho servo
    delay(15);                       // đợi 15 ms cho servo quay đến góc đó rồi tới bước tiếp theo
  } 
}
// ham dieu khien servo dong cong o servo output
void controllServoOpen(Servo isserVo,int cuoi1){
  for(int pos1 = cuoi1 ; pos1 >= 0; pos1 -= 1)  // cho servo quay từ 0->179 độ
  {                                  // mỗi bước của vòng lặp tăng 1 độ
    isserVo.write(pos1);              // xuất tọa độ ra cho servo
    delay(15);                       // đợi 15 ms cho servo quay đến góc đó rồi tới bước tiếp theo
  } 
}


//Hàm serialEvent() sẽ được gọi khi nào có tín hiệu từ cổng Serial. 
void serialEvent() {
 // Serial.flush();
  while (Serial.available()) {
     delay(10);
    // get the new byte:
    char inChar = (char)Serial.read();
    // add it to the inputString:
    inputString += inChar;
    // if the incoming character is a newline, set a flag
    // so the main loop can do something about it:
    if (inChar == '|') {
      break;
    }
  }
   stringComplete = true;
   Serial.flush();
}
