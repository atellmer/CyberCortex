//+------------------------------------------------------------------+
//|                �����-������ ��� ������ ������� � CyberCortex.mq4 |
//|                        Copyright 2015, MetaQuotes Software Corp. |
//|                                              http://www.mql5.com |
//+------------------------------------------------------------------+
#property copyright "Copyright 2015"
#property link      ""

//������, ���������� ������ ������ �� �������� "/" - ��� ��� �����������
//��� ������ ������� � CyberCortex. ���� ��������� ��� - ������� ������
//������ ��� ��������, ��������� �� ��������� ���������.
//�� ������ ������������ ��� ���� ������, ��� � ����� ������. 
//���������� ���� ����������� ��� ��� ������ � CyberCortex � �������� � 
//������ ������. 

//////////////////////////////1/////////////////////////////////////////
////////////////////////////////////////////////////////////////////////
#include <Files\FilePipe.mqh> 
CFilePipe PipeClient;
double _patternLength=32;            //����� ������� (��� � CyberCortex)
          //����� ������� �������� � ����������� �� ���������� ���������
     //�������� �������, ������� �� ����������� ���������� � CyberCortex
double _dataToServer[];
//////////////////////////////1/////////////////////////////////////////
////////////////////////////////////////////////////////////////////////
double    Lots=0.1;
double    PercentOfDepo=2.0;
int       MagicNumber=1;
int Signal,Ticket,Type;
bool Activate,FreeMarginAlert,FatalError;
datetime LastBar,LastSignal;
double Tick,Spread,StopLevel,MinLot,MaxLot,LotStep,FreezeLevel,sl;
//+-------------------------------------------------------------------------------------+
//| ������� ������������� ������                                                        |
//+-------------------------------------------------------------------------------------+
int init()
  {
   FatalError=False;
   Tick=MarketInfo(Symbol(),MODE_TICKSIZE);                            // ����������� ���    
   Spread=ND(MarketInfo(Symbol(),MODE_SPREAD)*Point);                    // ������� �����
   StopLevel=ND(MarketInfo(Symbol(),MODE_STOPLEVEL)*Point);     // ������� ������� ������
   FreezeLevel=ND(MarketInfo(Symbol(),MODE_FREEZELEVEL)*Point);      // ������� ���������
   MinLot=MarketInfo(Symbol(),MODE_MINLOT);     // ����������� ����������� ����� ������
   MaxLot=MarketInfo(Symbol(),MODE_MAXLOT);   // ������������ ����������� ����� ������
   LotStep=MarketInfo(Symbol(),MODE_LOTSTEP);              // ��� ���������� ������ �����          
   LastBar=0;
   LastSignal=0;
   Activate=True;

//////////////////////////////2/////////////////////////////////////////
////////////////////////////////////////////////////////////////////////
   ArrayResize(_dataToServer,_patternLength);
//////////////////////////////2/////////////////////////////////////////
////////////////////////////////////////////////////////////////////////

   return(0);
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
int deinit()
  {
//////////////////////////////3/////////////////////////////////////////
////////////////////////////////////////////////////////////////////////
   PipeClient.Close();
//////////////////////////////3/////////////////////////////////////////
////////////////////////////////////////////////////////////////////////
   return(0);
  }
//+-------------------------------------------------------------------------------------+
//| ������ ������ ������, ������ �� ��������� �������.                                  |
//+-------------------------------------------------------------------------------------+
double GetLots()
  {
   if(Lots==0)                                       // ���� �������� Lots ����� ����, ��
     {                                               // ..���������� ����� �� ���������..
      // ..��������� ���������
      double lot=(PercentOfDepo/100)*AccountFreeMargin()/
                 MarketInfo(Symbol(),MODE_MARGINREQUIRED);
      return(LotRound(lot));
     }
   else                                              // ���� �������� �� ����� ����, ��..
   return(LotRound(Lots));                                // ..������ ������ ��� ��������
  }
//+-------------------------------------------------------------------------------------+
//| �������� ������ �� ������������ � ����������                                        |
//+-------------------------------------------------------------------------------------+
double LotRound(double L)
  {
   return(MathRound(MathMin(MathMax(L, MinLot), MaxLot)/LotStep)*LotStep);
  }
//+-------------------------------------------------------------------------------------+
//| ���������� �������� � �������� ������ ������                                        |
//+-------------------------------------------------------------------------------------+
double ND(double AA)
  {
   return(NormalizeDouble(AA, Digits));
  }
//+-------------------------------------------------------------------------------------+
//| ����������� ��������� �� ������                                                     |
//+-------------------------------------------------------------------------------------+
string ErrorToString(int Error)
  {
   switch(Error)
     {
      case 2: return("������������� ����� ������, ���������� � ������������.");
      case 5: return("� ��� ������ ������ ���������, �������� ��.");
      case 6: return("��� ����� � ��������, ���������� ������������� ��������.");
      case 64: return("���� ������������, ���������� � ������������.");
      case 132: return("����� ������.");
      case 133: return("�������� ���������.");
      case 149: return("��������� �����������.");
     }
   return(0);
  }
//+-------------------------------------------------------------------------------------+
//| �������� ��������� ������. ���� ����� ��������, �� ��������� True, ����� - False    |
//+-------------------------------------------------------------------------------------+  
bool WaitForTradeContext()
  {
   int P=0;
   while(IsTradeContextBusy() && P<5)
     {
      P++;
      Sleep(1000);
     }
   if(P==5)
      return(False);
   return(True);
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
int OpenOrderCorrect(int type,double Lot,double Price,double SL,double TP,
                     bool Redefinition=True)
// Redefinition - ��� True ������������ ��������� �� ���������� ����������
//                ��� False - ���������� ������
  {
// - 1 - == �������� ������������� ��������� ������� ====================================
   if(AccountFreeMarginCheck(Symbol(),OP_BUY,Lot)<=0 || GetLastError()==134)
     {
      if(!FreeMarginAlert)
        {
         Print("������������ ������� ��� �������� �������. Free Margin = ",
               AccountFreeMargin());
         FreeMarginAlert=True;
        }
      return(5);
     }
   FreeMarginAlert=False;

// - 2 - == ������������� �������� Price, SL � TP ��� ������� ������ ====================   
   RefreshRates();
   switch(type)
     {
      case OP_BUY:
         string S="BUY";
         if(MathAbs(Price-Ask)/Point>3)
            if(Redefinition) Price=ND(Ask);
         else              return(2);
         if(ND(TP-Bid)<=StopLevel && TP!=0)
            if(Redefinition) TP=ND(Bid+StopLevel+Tick);
         else              return(4);
         if(ND(Bid-SL)<=StopLevel)
            if(Redefinition) SL=ND(Bid-StopLevel-Tick);
         else              return(3);
         break;
      case OP_SELL:
         S="SELL";
         if(MathAbs(Price-Bid)/Point>3)
            if(Redefinition) Price=ND(Bid);
         else              return(2);
         if(ND(Ask-TP)<=StopLevel)
            if(Redefinition) TP=ND(Ask-StopLevel-Tick);
         else              return(4);
         if(ND(SL-Ask)<=StopLevel && SL!=0)
            if(Redefinition) SL=ND(Ask+StopLevel+Tick);
         else              return(3);
         break;
      case OP_BUYSTOP:
         S="BUYSTOP";
         if(ND(Price-Ask)<=StopLevel)
            if(Redefinition) Price=ND(Ask+StopLevel+Tick);
         else              return(2);
         if(ND(TP-Price)<=StopLevel && TP!=0)
            if(Redefinition) TP=ND(Price+StopLevel+Tick);
         else              return(4);
         if(ND(Price-SL)<=StopLevel)
            if(Redefinition) SL=ND(Price-StopLevel-Tick);
         else              return(3);
         break;
      case OP_SELLSTOP:
         S="SELLSTOP";
         if(ND(Bid-Price)<=StopLevel)
            if(Redefinition) Price=ND(Bid-StopLevel-Tick);
         else              return(2);
         if(ND(Price-TP)<=StopLevel)
            if(Redefinition) TP=ND(Price-StopLevel-Tick);
         else              return(4);
         if(ND(SL-Price)<=StopLevel && SL!=0)
            if(Redefinition) SL=ND(Price+StopLevel+Tick);
         else              return(3);
         break;
      case OP_BUYLIMIT:
         S="BUYLIMIT";
         if(ND(Ask-Price)<=StopLevel)
            if(Redefinition) Price=ND(Ask-StopLevel-Tick);
         else              return(2);
         if(ND(TP-Price)<=StopLevel && TP!=0)
            if(Redefinition) TP=ND(Price+StopLevel+Tick);
         else              return(4);
         if(ND(Price-SL)<=StopLevel)
            if(Redefinition) SL=ND(Price-StopLevel-Tick);
         else              return(3);
         break;
      case OP_SELLLIMIT:
         S="SELLLIMIT";
         if(ND(Price-Bid)<=StopLevel)
            if(Redefinition) Price=ND(Bid+StopLevel+Tick);
         else              return(2);
         if(ND(Price-TP)<=StopLevel)
            if(Redefinition) TP=ND(Price-StopLevel-Tick);
         else              return(4);
         if(ND(SL-Price)<=StopLevel && SL!=0)
            if(Redefinition) SL=ND(Price+StopLevel+Tick);
         else              return(3);
         break;
     }
// - 3 - == �������� ������ � ��������� ��������� ������ ================================  
   if(WaitForTradeContext()) // �������� ������������ ��������� ������
     {
      Comment("��������� ������ �� �������� ������ ",S," ...");
      int ticket=OrderSend(Symbol(),type,Lot,Price,3,
                           SL,TP,NULL,MagicNumber,0,Red);// �������� �������
      // ������� �������� ������� ����������� ��������
      if(ticket<0)
        {
         int Error= GetLastError();
         if(Error == 2|| Error == 5|| Error == 6|| Error == 64
            || Error==132 || Error==133 || Error==149) // ������ ��������� ������
           {
            Comment("��������� ������ ��� �������� ������� �. �. "+
                    ErrorToString(Error)+" �������� ��������!");
            FatalError=True;
           }
         else
            Comment("������ �������� ������� ",S,": ",Error);      // ����������� ������
         return(1);
        }
      // ---------------------------------------------

      // ������� �������� �������   
      Comment("������� ",S," ������� �������!");
      return(0);
      // ------------------------
     }
   else
     {
      Comment("����� �������� ������������ ��������� ������ �������!");
      return(1);
     }
  }
//+-------------------------------------------------------------------------------------+
//| ���������� �������� � �������� ������ ����                                          |
//+-------------------------------------------------------------------------------------+
double NP(double AA)
  {
   return(MathRound(AA/Tick)*Tick);
  }
//+-------------------------------------------------------------------------------------+
//| ������� ������ ����� �������.                                                       |
//+-------------------------------------------------------------------------------------+
void FindOrders()
  {
// - 1 - == ������������� ���������� ����� ������� ======================================
   int total=OrdersTotal()-1;
   Ticket=-1;
   Type=-1;
// - 2 - == ��������������� ����� =======================================================
   for(int i = total; i >= 0; i--)                // ������������ ���� ������ �������
      if(OrderSelect(i, SELECT_BY_POS))           // ��������, ��� ����� ������
         if(OrderMagicNumber() == MagicNumber &&  // ������� ������� ���������, �������
            OrderSymbol()==Symbol()  &&           // ..���������� � ������� ����
            OrderType()<2)
           {
            Ticket=OrderTicket();                // ������� ������ �������
            Type=OrderType();
           }
  }
//+-------------------------------------------------------------------------------------+
//| �������� ��������� ��������� ������                                                 |
//+-------------------------------------------------------------------------------------+
bool CloseDeal(int ticket)
  {
   if(OrderSelect(ticket,SELECT_BY_TICKET) && // ���������� ����� � ��������.. 
      OrderCloseTime()==0) // ..������� � ����� �� ������
      if(WaitForTradeContext()) // �������� �� �������� �����?
        {
         if(OrderType()==OP_BUY) // ���� ������� ������� �������.. 
            double Price=MarketInfo(Symbol(),MODE_BID);      // ..������, �� �����������..
         // ..���� BID
         else                                           // ���� ������� ������� ��������.. 
         Price=MarketInfo(Symbol(),MODE_ASK);         // ..������, �� ����������� ���� ASK
         if(!OrderClose(OrderTicket(),OrderLots(),NP(Price),3)) // ���� ������ ��..
            return(False);                            // ..������� �������, �� ���������..
         // ������� - False
        }
   else
      return(false);
   return(True);                                       // ����� ��������� ��������� ������
  }
//+-------------------------------------------------------------------------------------+
//| �������� �������                                                                    |
//+-------------------------------------------------------------------------------------+
bool Trade()
  {
   FindOrders();                                                   // ������ ���� �������
                                                                   // - 1 - == �������� ������� ������� 
   if(Signal>0 && Type!=OP_BUY) // ������� ������ ������� �..
     {                                                    // ..����������� ������� �������
      if(Type==OP_SELL) // ���������� �������� �������
         if(!CloseDeal(Ticket)) // ������� �������
            return(false);                                     // ��� ������� ������ false
      if(OpenOrderCorrect(OP_BUY,GetLots(),NP(Ask),0,0)!=0)
         return(False);
     }

// - 2 - == �������� �������� ������� ===================================================
   if(Signal<0 && Type!=OP_SELL) // ������� ������ ������� �..
     {                                                  // ..����������� �������� �������
      if(Type==OP_BUY) // ���������� ������� �������
         if(!CloseDeal(Ticket)) // ������� �������
            return(false);                                    // ��� ������� ������ false  
      if(OpenOrderCorrect(OP_SELL,GetLots(),NP(Bid),0,0)!=0)
         return(False);


     }
   return(0);
   return(True);
  }
//+------------------------------------------------------------------+
//|      ������� ������������ �������� ������ ����                   |
//+------------------------------------------------------------------+
bool fNewBar()
  {
   static datetime NewTime=0;
   if(NewTime!=Time[0])
     {
      if(NewTime==0)
        {
         NewTime=Time[0];
         return(false);
        }
      NewTime=Time[0];
      return(true);
     }
   return(false);
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
int start()
  {
//////////////////////////////4/////////////////////////////////////////
////////////////////////////////////////////////////////////////////////
   int predict=0;

   if(fNewBar())     //�������� ������� ������������ �������� ������ ����
     {
      predict=GetMySignal();      //�������� ������� ����� � CyberCortex
             //����� � ����������� �� ���������� ������� ������� �������
                                              //(� ������� ���� 3 ������)
//���� �� CyberCortex ������ "1" � "1" ������������� �������, �� ��������
      if(predict==1) 
        {
         Signal=1; 
        }
//���� �� CyberCortex ������ "2" � "2" ������������� �������, �� �������
      if(predict==2)
        {
         Signal=-1;
        }
//���� �� CyberCortex ������ "3" � "3" ������������� "�� ����", �� �� 
//������ ������
      if(predict==3)
        {
         Signal=0;
        }
     }
//////////////////////////////4/////////////////////////////////////////
////////////////////////////////////////////////////////////////////////

   if(Signal!=0) // ���� ���� ������
      if(!Trade()) return(0);                                // ��������/�������� �������
// - 1 - == ����� �� �������� ��������? =================================================
   if(!Activate || FatalError) return(0);

// - 2 - == �������� �������� ������ ���� ===============================================
   if(LastBar==Time[0]) // ���� �� ������� ���� ��� ����..
      return(0);                                    // ..����������� ����������� ��������,
// ..�� ��������� ������ �� ���������� 
//..����
// - 3 - == �������� �� ���������� ��������� ��������� ==================================
   if(!IsTesting())
     {
      Tick=MarketInfo(Symbol(),MODE_TICKSIZE);                         // ����������� ���    
      Spread=ND(MarketInfo(Symbol(),MODE_SPREAD)*Point);                 // ������� �����
      StopLevel=ND(MarketInfo(Symbol(),MODE_STOPLEVEL)*Point);   //������� ������� ������
      FreezeLevel=ND(MarketInfo(Symbol(),MODE_FREEZELEVEL)*Point);   // ������� ���������
     }
   LastBar=Time[0];
   return(0);
  }
//////////////////////////////5/////////////////////////////////////////
////////////////////////////////////////////////////////////////////////
int GetMySignal()
  {
   double predict=0;

   int k=1;

   //����� ������� ������ �������� ���������� ��� Open, High, Low, Close
   //��� 8 ��������� ����� (32/4 = 8)
   //��� �������  - ������ ������, �� �������� ����, � ����������� �� ����, 
   //��� ������ �������� � CyberCortex (���������� ���, �����, ������, 
   //�����, ����������, � ������.)
   for(int j=0; j<_patternLength; j=j+4)
     {
      _dataToServer[j]=(Open[k]-Open[k+1])/Open[k+1]*100;
      _dataToServer[j+1] = (High[k]-High[k+1])/High[k+1]*100;
      _dataToServer[j+2] = (Low[k]-Low[k+1])/Low[k+1]*100;
      _dataToServer[j+3] = (Close[k]-Close[k+1])/Close[k+1]*100;
      k++;
     }

  //����� ���� ��� ��������� ������, ���������� �� � CyberCortex
  //� �������� ��� �� �����
   predict=SendDataAndGetPredict(_dataToServer);

   return (predict);
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
void Connect()
  {
   while(!IsStopped())
     {
      //��� ������� "\\\\.\\pipe\\MT4.Server" ������ ������!
      if(PipeClient.Open("\\\\.\\pipe\\MT4.Server",FILE_READ|FILE_WRITE|FILE_BIN)!=INVALID_HANDLE)
        {
         break;
        }
     }
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
void SendToServer(double value)
  {
//Print("������: ",value);
   if(!PipeClient.WriteDouble(value))
     {
      //Print("�� ������� ��������� double!");
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
      //Print("�� ������� ��������� double!");
     }
//Print("������: ",value);
   return (value);
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
double SendDataAndGetPredict(double &array[])
  {
   double message=0; //��������� � ������� ������ "C���" � CyberCortex
   for(int i=0; i<_patternLength; i++)
     {
      Connect();
      SendToServer(array[i]);
      message=ReadFromServer();
      if(message==-12345)
        {
         Print("������ ������");
         return(0);
        }
     }
   return (GetPredict());
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
//////////////////////////////5/////////////////////////////////////////
////////////////////////////////////////////////////////////////////////
