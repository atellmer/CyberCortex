//+------------------------------------------------------------------+
//|                Робот-шаблон для обмена данными с CyberCortex.mq4 |
//|                        Copyright 2015, MetaQuotes Software Corp. |
//|                                              http://www.mql5.com |
//+------------------------------------------------------------------+
#property copyright "Copyright 2015"
#property link      ""

//Данные, помещенные внутрь блоков из символов "/" - это код необходимый
//для обмена данными с CyberCortex. Весь остальной код - обычный шаблон
//робота для торговли, найденный на просторах интернета.
//Вы можете использовать как этот шаблон, так и любой другой. 
//Необходимо дишь скопировать код для обмена с CyberCortex и вставить в 
//своего робота. 

//////////////////////////////1/////////////////////////////////////////
////////////////////////////////////////////////////////////////////////
#include <Files\FilePipe.mqh> 
CFilePipe PipeClient;
double _patternLength=32;            //Длина примера (как в CyberCortex)
          //Длина примера задается в зависимости от количества признаков
     //входного вектора, который Вы собираетесь отправлять в CyberCortex
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
//| Функция инициализации робота                                                        |
//+-------------------------------------------------------------------------------------+
int init()
  {
   FatalError=False;
   Tick=MarketInfo(Symbol(),MODE_TICKSIZE);                            // минимальный тик    
   Spread=ND(MarketInfo(Symbol(),MODE_SPREAD)*Point);                    // текущий спрэд
   StopLevel=ND(MarketInfo(Symbol(),MODE_STOPLEVEL)*Point);     // текущий уровень стопов
   FreezeLevel=ND(MarketInfo(Symbol(),MODE_FREEZELEVEL)*Point);      // уровень заморозки
   MinLot=MarketInfo(Symbol(),MODE_MINLOT);     // минимальный разрешенный объем сделки
   MaxLot=MarketInfo(Symbol(),MODE_MAXLOT);   // максимальный разрешенный объем сделки
   LotStep=MarketInfo(Symbol(),MODE_LOTSTEP);              // шаг приращения объема сделк          
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
//| Расчет объема сделки, исходя из свободных средств.                                  |
//+-------------------------------------------------------------------------------------+
double GetLots()
  {
   if(Lots==0)                                       // Если значение Lots равно нулю, то
     {                                               // ..рассчитаем объем по имеющимся..
      // ..свободным средствам
      double lot=(PercentOfDepo/100)*AccountFreeMargin()/
                 MarketInfo(Symbol(),MODE_MARGINREQUIRED);
      return(LotRound(lot));
     }
   else                                              // Если параметр не равен нулю, то..
   return(LotRound(Lots));                                // ..просто вернем его значение
  }
//+-------------------------------------------------------------------------------------+
//| Проверка объема на корректность и округление                                        |
//+-------------------------------------------------------------------------------------+
double LotRound(double L)
  {
   return(MathRound(MathMin(MathMax(L, MinLot), MaxLot)/LotStep)*LotStep);
  }
//+-------------------------------------------------------------------------------------+
//| Приведение значений к точности одного пункта                                        |
//+-------------------------------------------------------------------------------------+
double ND(double AA)
  {
   return(NormalizeDouble(AA, Digits));
  }
//+-------------------------------------------------------------------------------------+
//| Расшифровка сообщения об ошибке                                                     |
//+-------------------------------------------------------------------------------------+
string ErrorToString(int Error)
  {
   switch(Error)
     {
      case 2: return("зафиксирована общая ошибка, обратитесь в техподдержку.");
      case 5: return("у вас старая версия терминала, обновите ее.");
      case 6: return("нет связи с сервером, попробуйте перезагрузить терминал.");
      case 64: return("счет заблокирован, обратитесь в техподдержку.");
      case 132: return("рынок закрыт.");
      case 133: return("торговля запрещена.");
      case 149: return("запрещено локирование.");
     }
   return(0);
  }
//+-------------------------------------------------------------------------------------+
//| Ожидание торгового потока. Если поток свободен, то результат True, иначе - False    |
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
// Redefinition - при True доопределять параметры до минимально допустимых
//                при False - возвращать ошибку
  {
// - 1 - == Проверка достаточности свободных средств ====================================
   if(AccountFreeMarginCheck(Symbol(),OP_BUY,Lot)<=0 || GetLastError()==134)
     {
      if(!FreeMarginAlert)
        {
         Print("Недостаточно средств для открытия позиции. Free Margin = ",
               AccountFreeMargin());
         FreeMarginAlert=True;
        }
      return(5);
     }
   FreeMarginAlert=False;

// - 2 - == Корректировка значений Price, SL и TP или возврат ошибки ====================   
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
// - 3 - == Открытие ордера с ожиданием торгового потока ================================  
   if(WaitForTradeContext()) // ожидание освобождения торгового потока
     {
      Comment("Отправлен запрос на открытие ордера ",S," ...");
      int ticket=OrderSend(Symbol(),type,Lot,Price,3,
                           SL,TP,NULL,MagicNumber,0,Red);// открытие позиции
      // Попытка открытия позиции завершилась неудачей
      if(ticket<0)
        {
         int Error= GetLastError();
         if(Error == 2|| Error == 5|| Error == 6|| Error == 64
            || Error==132 || Error==133 || Error==149) // список фатальных ошибок
           {
            Comment("Фатальная ошибка при открытии позиции т. к. "+
                    ErrorToString(Error)+" Советник отключен!");
            FatalError=True;
           }
         else
            Comment("Ошибка открытия позиции ",S,": ",Error);      // нефатальная ошибка
         return(1);
        }
      // ---------------------------------------------

      // Удачное открытие позиции   
      Comment("Позиция ",S," открыта успешно!");
      return(0);
      // ------------------------
     }
   else
     {
      Comment("Время ожидания освобождения торгового потока истекло!");
      return(1);
     }
  }
//+-------------------------------------------------------------------------------------+
//| Приведение значений к точности одного тика                                          |
//+-------------------------------------------------------------------------------------+
double NP(double AA)
  {
   return(MathRound(AA/Tick)*Tick);
  }
//+-------------------------------------------------------------------------------------+
//| Функция поиска своих ордеров.                                                       |
//+-------------------------------------------------------------------------------------+
void FindOrders()
  {
// - 1 - == Инициализация переменных перед поиском ======================================
   int total=OrdersTotal()-1;
   Ticket=-1;
   Type=-1;
// - 2 - == Непосредственно поиск =======================================================
   for(int i = total; i >= 0; i--)                // Используется весь список ордеров
      if(OrderSelect(i, SELECT_BY_POS))           // Убедимся, что ордер выбран
         if(OrderMagicNumber() == MagicNumber &&  // Позиция открыта экспертом, который
            OrderSymbol()==Symbol()  &&           // ..прикреплен к текущей паре
            OrderType()<2)
           {
            Ticket=OrderTicket();                // Запишем данные позиции
            Type=OrderType();
           }
  }
//+-------------------------------------------------------------------------------------+
//| Закрытие заданного рыночного ордера                                                 |
//+-------------------------------------------------------------------------------------+
bool CloseDeal(int ticket)
  {
   if(OrderSelect(ticket,SELECT_BY_TICKET) && // Существует ордер с заданным.. 
      OrderCloseTime()==0) // ..тикетом и ордер не закрыт
      if(WaitForTradeContext()) // Свободен ли торговый поток?
        {
         if(OrderType()==OP_BUY) // Если следует закрыть длинную.. 
            double Price=MarketInfo(Symbol(),MODE_BID);      // ..сделку, то применяется..
         // ..цена BID
         else                                           // Если следует закрыть короткую.. 
         Price=MarketInfo(Symbol(),MODE_ASK);         // ..сделку, то применяется цена ASK
         if(!OrderClose(OrderTicket(),OrderLots(),NP(Price),3)) // Если сделку не..
            return(False);                            // ..удалось закрыть, то результат..
         // функции - False
        }
   else
      return(false);
   return(True);                                       // Можно открывать следующую сделку
  }
//+-------------------------------------------------------------------------------------+
//| Открытие позиций                                                                    |
//+-------------------------------------------------------------------------------------+
bool Trade()
  {
   FindOrders();                                                   // Найдем свои позиции
                                                                   // - 1 - == Открытие длинной позиции 
   if(Signal>0 && Type!=OP_BUY) // Активен сигнал покупки и..
     {                                                    // ..отсутствует длинная позиция
      if(Type==OP_SELL) // Существует короткая позиция
         if(!CloseDeal(Ticket)) // Закроем позицию
            return(false);                                     // При неудаче вернем false
      if(OpenOrderCorrect(OP_BUY,GetLots(),NP(Ask),0,0)!=0)
         return(False);
     }

// - 2 - == Открытие короткой позиции ===================================================
   if(Signal<0 && Type!=OP_SELL) // Активен сигнал продажи и..
     {                                                  // ..отсутствует короткая позиция
      if(Type==OP_BUY) // Существует длинная позиция
         if(!CloseDeal(Ticket)) // Закроем позицию
            return(false);                                    // При неудаче вернем false  
      if(OpenOrderCorrect(OP_SELL,GetLots(),NP(Bid),0,0)!=0)
         return(False);


     }
   return(0);
   return(True);
  }
//+------------------------------------------------------------------+
//|      Функция отслеживания открытия нового бара                   |
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

   if(fNewBar())     //вызываем Функцию отслеживания открытия нового бара
     {
      predict=GetMySignal();      //вызываем функцию связи с CyberCortex
             //далее в зависимости от количества классов создаем условия
                                              //(в примере ниже 3 класса)
//если от CyberCortex пришла "1" и "1" соответствует покупке, то покупаем
      if(predict==1) 
        {
         Signal=1; 
        }
//если от CyberCortex пришла "2" и "2" соответствует продаже, то продаем
      if(predict==2)
        {
         Signal=-1;
        }
//если от CyberCortex пришла "3" и "3" соответствует "не знаю", то не 
//делаем ничего
      if(predict==3)
        {
         Signal=0;
        }
     }
//////////////////////////////4/////////////////////////////////////////
////////////////////////////////////////////////////////////////////////

   if(Signal!=0) // Если есть сигнал
      if(!Trade()) return(0);                                // Открытие/закрытие позиций
// - 1 - == Можно ли работать эксперту? =================================================
   if(!Activate || FatalError) return(0);

// - 2 - == Контроль открытия нового бара ===============================================
   if(LastBar==Time[0]) // Если на текущем баре уже были..
      return(0);                                    // ..произведены необходимые действия,
// ..то прерываем работу до следующего 
//..тика
// - 3 - == Слежение за изменением рыночного окружения ==================================
   if(!IsTesting())
     {
      Tick=MarketInfo(Symbol(),MODE_TICKSIZE);                         // минимальный тик    
      Spread=ND(MarketInfo(Symbol(),MODE_SPREAD)*Point);                 // текущий спред
      StopLevel=ND(MarketInfo(Symbol(),MODE_STOPLEVEL)*Point);   //текущий уровень стопов
      FreezeLevel=ND(MarketInfo(Symbol(),MODE_FREEZELEVEL)*Point);   // уровень заморозки
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

   //здесь показан пример загрузки приращений цен Open, High, Low, Close
   //для 8 последних баров (32/4 = 8)
   //эта функция  - просто пример, Вы сделаете свою, в зависимости от того, 
   //что будете подавать в CyberCortex (приращения цен, объем, дельты, 
   //время, индикаторы, и прочее.)
   for(int j=0; j<_patternLength; j=j+4)
     {
      _dataToServer[j]=(Open[k]-Open[k+1])/Open[k+1]*100;
      _dataToServer[j+1] = (High[k]-High[k+1])/High[k+1]*100;
      _dataToServer[j+2] = (Low[k]-Low[k+1])/Low[k+1]*100;
      _dataToServer[j+3] = (Close[k]-Close[k+1])/Close[k+1]*100;
      k++;
     }

  //после того как загрузили данные, отправляем их в CyberCortex
  //и получаем тут же ответ
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
      //имя сервера "\\\\.\\pipe\\MT4.Server" менять нельзя!
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
//Print("Клиент: ",value);
   if(!PipeClient.WriteDouble(value))
     {
      //Print("Не удалось отправить double!");
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
      //Print("Не удалось прочитать double!");
     }
//Print("Сервер: ",value);
   return (value);
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
double SendDataAndGetPredict(double &array[])
  {
   double message=0; //сообщение о нажатии кнопки "Cтоп" в CyberCortex
   for(int i=0; i<_patternLength; i++)
     {
      Connect();
      SendToServer(array[i]);
      message=ReadFromServer();
      if(message==-12345)
        {
         Print("Сервер закрыт");
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
