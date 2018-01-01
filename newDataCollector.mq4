//+------------------------------------------------------------------+
//|                                             newDataCollector.mq4 |
//|                                                          Romanov |
//|                                               atellmer@gmail.com |
//+------------------------------------------------------------------+
#property copyright "Romanov"
#property link      "atellmer@gmail.com"
#property version   "1.00"
#property strict
//+------------------------------------------------------------------+
//| Script program start function                                    |
//+------------------------------------------------------------------+

int _lengthPattern=32;
double _threshold = 0.5;

double _commonSamples[1][32];
double _samples[1][32];
double _testSamples[1][32];

double _commonAnswers[1];
double _answers[1];
double _testAnswers[1];

string _nameFileForPatterns= "patterns.csv";
string _nameFileForAnswers = "answers.csv";
string _nameFileForTestPatterns= "test_patterns.csv";
string _nameFileForTestAnswers = "test_answers.csv";
int _countSamples=0;
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+

void OnStart()
  {
   FindTrigger();
   SavePatterns(_nameFileForPatterns,_commonSamples);
   SaveAnswers(_nameFileForAnswers,_commonAnswers);
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
void FindTrigger()
  {
   double priceNow=0;
   double priceThen=0;
   int StartIndexForSearch=0;
   int StartIndexForAnalyse=0;
   double increment=0;
   datetime time=0;

   StartIndexForSearch=ConvertTimeframeToDay();

   for(int i=StartIndexForSearch-1;i>0;i--)
     {
      increment=0;
      priceThen=iClose(NULL,1440,i);
      priceNow=iClose(NULL,1440,i+1);

      if(priceNow!=0)
         increment=(priceThen-priceNow)/priceNow*100;
      //Alert("Изменение цен по дням %: ",increment);

      if(MathAbs(increment)>=_threshold)
        {
         if(increment>0)
           {
            _commonAnswers[_countSamples]=1;
           }
         if(increment<0)
           {
            _commonAnswers[_countSamples]=2;
           }

         time=iTime(NULL,1440,i);
         StartIndexForAnalyse=iBarShift(NULL,_Period,time);

         //Alert("-----------------------------");
         //Alert("Изменение цен %: ",increment);
         //Alert("Индекс бара изменения: ",StartIndexForAnalyse);
         //Alert("Индекс дневного бара изменения: ",i);

         int k=StartIndexForAnalyse;

         for (int j=0; j<_lengthPattern; j=j+4)
         {         
         _commonSamples[_countSamples][j] = (Open[k]-Open[k+1])/Open[k+1]*100;
         _commonSamples[_countSamples][j+1] = (High[k]-High[k+1])/High[k+1]*100;
         _commonSamples[_countSamples][j+2] = (Low[k]-Low[k+1])/Low[k+1]*100;
         _commonSamples[_countSamples][j+3] = (Close[k]-Close[k+1])/Close[k+1]*100;         
         k++;
         }
         
         _countSamples++;

         if(_countSamples==ArrayRange(_commonSamples,0))
           {
            ArrayResize(_commonSamples,ArrayRange(_commonSamples,0)+1);
           }
         if(_countSamples==ArrayRange(_commonAnswers,0))
           {
            ArrayResize(_commonAnswers,ArrayRange(_commonAnswers,0)+1);
           }
        }
        else
        {
         _commonAnswers[_countSamples]=3;          

         time=iTime(NULL,1440,i);
         StartIndexForAnalyse=iBarShift(NULL,_Period,time);

         int k=StartIndexForAnalyse;

         for (int j=0; j<_lengthPattern; j=j+4)
         {         
         _commonSamples[_countSamples][j] = (Open[k]-Open[k+1])/Open[k+1]*100;
         _commonSamples[_countSamples][j+1] = (High[k]-High[k+1])/High[k+1]*100;
         _commonSamples[_countSamples][j+2] = (Low[k]-Low[k+1])/Low[k+1]*100;
         _commonSamples[_countSamples][j+3] = (Close[k]-Close[k+1])/Close[k+1]*100;         
         k++;
         }
         
         _countSamples++;

         if(_countSamples==ArrayRange(_commonSamples,0))
           {
            ArrayResize(_commonSamples,ArrayRange(_commonSamples,0)+1);
           }
         if(_countSamples==ArrayRange(_commonAnswers,0))
           {
            ArrayResize(_commonAnswers,ArrayRange(_commonAnswers,0)+1);
           }
        }       
     }
   Print("Найден ",_countSamples," сэмпл");
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
int ConvertTimeframeToDay()
  {
   int index=0;

   if(_Period==1)
     {
      index=Bars/1440;
     }
   if(_Period==5)
     {
      index=Bars/288;
     }
   if(_Period==15)
     {
      index=Bars/96;
     }
   if(_Period==30)
     {
      index=Bars/48;
     }
   if(_Period==60)
     {
      index=Bars/24;
     }
   if(_Period==240)
     {
      index=Bars/6;
     }
   if(_Period==1440)
     {
      index=Bars;
     }
   return (index);
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
void SavePatterns(string nameFile,double &data[][])
  {
   int fileHandle=FileOpen(nameFile,FILE_CSV|FILE_WRITE);

   if(fileHandle!=INVALID_HANDLE)
     {
      for(int i=0;i<ArrayRange(data,0)-1; i++)
        {
         FileWrite(fileHandle,data[i][0],data[i][1],data[i][2],data[i][3],data[i][4],data[i][5],data[i][6],data[i][7],
         data[i][8],data[i][9],data[i][10],data[i][11],data[i][12],data[i][13],data[i][14],data[i][15],
         data[i][9],data[i][10],data[i][11],data[i][12],data[i][13],data[i][14],data[i][15],data[i][16],
         data[i][10],data[i][11],data[i][12],data[i][13],data[i][14],data[i][15],data[i][16],data[i][17],
         data[i][18],data[i][19],data[i][20],data[i][21],data[i][22],data[i][23],data[i][24],data[i][25],
         data[i][26],data[i][27],data[i][28],data[i][29],data[i][30],data[i][31]);
        }
      FileClose(fileHandle);
      Print("Данные успешно записаны");
     }
   else
     {
      Print("Операция FileOpen неудачна, ошибка ",GetLastError());
     }
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
void SaveAnswers(string nameFile,double &data[])
  {
   int fileHandle=FileOpen(nameFile,FILE_CSV|FILE_WRITE);

   if(fileHandle!=INVALID_HANDLE)
     {
      for(int i=0;i<ArrayRange(data,0)-1;i++)
        {
         FileWrite(fileHandle,data[i]);
        }
      FileClose(fileHandle);
      Print("Данные успешно записаны");
     }
   else
     {
      Print("Операция FileOpen неудачна, ошибка ",GetLastError());
     }
  }
//+------------------------------------------------------------------+
