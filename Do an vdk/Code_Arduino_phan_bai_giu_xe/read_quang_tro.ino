int quangtro = A0; //Thiết đặt chân analog đọc quang trở
int quangtro1 = A1; //Thiết đặt chân analog đọc quang trở
int quangtro2= A2;
int quangtro3= A3;
int khuA = 8;
int khuB = 7;
int khuC = 6;
int khuD = 5; 
int laser = 10;
void setup() {
  // Khởi tạo cộng Serial 9600
  
  Serial.begin(9600);
//  for(int i = 2; i < 7; i++){
    pinMode(khuA,OUTPUT);
    pinMode(khuB,OUTPUT);
    pinMode(khuC,OUTPUT);
    pinMode(khuD,OUTPUT);
    pinMode(laser,OUTPUT);
//  }
}

void loop() {
  int giatriQuangtro = analogRead(quangtro);// đọc giá trị quang trở khu a
  int giatriQuangtro1 = analogRead(quangtro1);// đọc giá trị quang trở khu b
  int giatriQuangtro2 = analogRead(quangtro2);// đọc giá trị quang trở khu c
  int giatriQuangtro3 = analogRead(quangtro3);// đọc giá trị quang trở khu d
   Serial.println(giatriQuangtro1);
  Serial.println(giatriQuangtro2);
  // bat den laser, dua vao dien ap cao
  delay(1000);
  digitalWrite(laser,HIGH);
  if(giatriQuangtro > 100){
    digitalWrite(khuA,HIGH);
  }else{
    digitalWrite(khuA,LOW);
  }
   if(giatriQuangtro1 > 100){
    digitalWrite(khuB,HIGH);
  }else{
    digitalWrite(khuB,LOW);
  }
   if(giatriQuangtro2 > 100){
    digitalWrite(khuC,HIGH);
  }else{
    digitalWrite(khuC,LOW);
  }
   if(giatriQuangtro3 > 100){
    digitalWrite(khuD,HIGH);
  }else{
    digitalWrite(khuD,LOW);
  }

}
