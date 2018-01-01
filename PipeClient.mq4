//+------------------------------------------------------------------+
//|                  PipeClient for CyberCortex.mq4                  |
//+------------------------------------------------------------------+
#property copyright "Romanov A."
#property link      "atellmer@gmail.com"
#property version   "1.00"
#property strict
#include <Files\FilePipe.mqh>
CFilePipe PipeClient;
//+------------------------------------------------------------------+
//| Script program start function                                    |
//+------------------------------------------------------------------+
int _patternLength=4;

double _dataToServer[];
double _predict=0;
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
int init()
  {
   ArrayResize(_dataToServer,_patternLength);
   Alert("Клиент Создан");
   return(0);
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
int deinit()
  {
   PipeClient.Close();
   return (0);
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
void OnStart()
  {
   for(int i=0; i<_patternLength; i++)
     {
      _dataToServer[i]=Close[i];
     }

   SendData(_dataToServer);
   _predict=GetPredict();
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
void Connect()
  {
   while(!IsStopped())
     {
      if(PipeClient.Open("\\\\.\\pipe\\MQL4.Server",FILE_READ|FILE_WRITE|FILE_BIN)!=INVALID_HANDLE)
        {
         break;
        }
      Sleep(500);
     }
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
void SendToServer(double value)
  {
   Alert("Клиент: ",value);
   if(!PipeClient.WriteDouble(value))
     {
      Alert("Не удалось отправить double!");
      return;
     }
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
double ReadFromServer()
  {
   double value=0;
   if(!PipeClient.ReadDouble(value))
     {
      Alert("Не удалось прочитать double!");
     }
   Alert("Сервер: ",value);
   return (value);
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
void SendData(double &array[])
  {
   for(int i=0; i<_patternLength; i++)
     {
      Connect();
      SendToServer(array[i]);
      ReadFromServer();
     }
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
double GetPredict()
  {
   Connect();
   SendToServer(0);
   return ReadFromServer();
  }
//+------------------------------------------------------------------+
