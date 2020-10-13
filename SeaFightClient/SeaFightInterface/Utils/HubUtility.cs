using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFightInterface.Presenter
{
    //Класс с реализацией Singletone
    class HubUtility
    {
        HubUtility() 
        {
            #if DEBUG
            hubConnection = new HubConnection("http://localhost:50073");
            #else   
            hubConnection = new HubConnection("http://seafight.somee.com");
            #endif
            chat = hubConnection.CreateHubProxy("GameHub");
            RegisterHubEvents();
            hubConnection.StateChanged += HubConnection_StateChanged;
        }

        private void HubConnection_StateChanged(StateChange obj)
        {
            if (obj.NewState == ConnectionState.Connected)
            {
                OnConnected();
            }
        }

        public HubConnection hubConnection { get; private set; }
        public IHubProxy chat { get; private set; }

        static readonly Lazy<HubUtility> instance = new Lazy<HubUtility>(() => new HubUtility());
        public static HubUtility Instance
        { 
            get 
            { 
                return instance.Value; 
            } 
        }

        public event Action OnConnected;
        public event Action OnError;
        public event Action<List<string>, string> ShowPlayersEvent;
        public event Action<string, string> UpdateMessageListEvent;
        public event Action<string, string, string> OfferToYouEvent;
        public event Action<string, string, string, string> AnswerOfferingEvent;
        public event Action DenyOfferingYouEvent;
        public event Action DenyOfferingHeEvent;


        public event Action NetMyVictoryEvent;
        public event Action NetOpponentVictoryEvent;
        public event Action<string> NetOpponentAbortGameEvent;
        public event Action<string, string> BeginNetGameEvent;
        public event Action<(int, int)> PlayerTargetHitEvent;
        public event Action<(int, int)> PlayerTargetMissEvent;
        public event Action<(int, int)> OpponentTargetHitEvent;
        public event Action<(int, int)> OpponentTargetMissEvent;
        public event Action<int, int> DecreasePlayerShipCountEvent;
        public event Action<int, int> DecreaseOpponentShipCountEvent;
        public event Action<string> MarkPlayerDestroyedShipEvent;
        public event Action<string> MarkOpponentDestroyedShipEvent;
        public event Action MarkMyTurnEvent;
        public event Action MarkOpponentTurnEvent;

        public void Connect()
        {
            try
            {
                hubConnection.Start().Wait();
            }
            catch
            {
                OnError();
            }
        }

        public void Disconnect()
        {
            if (hubConnection.State == ConnectionState.Connected)
            {
                hubConnection.Stop();               
            }
        }

        /// <summary>
        /// Подписка на события хаба сервера
        /// </summary>
        private void RegisterHubEvents()
        {
            chat.On<string, string>("broadcastMessage", (name, message) => { UpdateMessageListEvent(name, message); });
            chat.On<List<string>>("getPlayers", (players) => { ShowPlayersEvent(players, hubConnection.ConnectionId); });
            chat.On<string, string, string>("gameOffering", (nameFrom, idFrom, nameTo) => { OfferToYouEvent(nameFrom, idFrom, nameTo); });
            chat.On<string, string, string, string>("answerOffering", (nameFrom, nameTo, answer, sessionID) => { AnswerOfferingEvent(nameFrom, nameTo, answer, sessionID); });
            chat.On("denyOfferingHeBusy", () => { DenyOfferingHeEvent(); });
            chat.On("denyOfferingYouBusy", () => { DenyOfferingYouEvent(); });

            chat.On<string, string>("startGame", (whoIsFirstId, whoIsFirstName) => { BeginNetGameEvent(whoIsFirstId, whoIsFirstName); });
            chat.On<string>("opponentHit", (destinantionPoint) => { OpponentTargetHitEvent((int.Parse(destinantionPoint[0].ToString()), int.Parse(destinantionPoint[1].ToString()))); MarkOpponentTurnEvent(); });
            chat.On<string>("opponentMiss", (destinantionPoint) => { OpponentTargetMissEvent((int.Parse(destinantionPoint[0].ToString()), int.Parse(destinantionPoint[1].ToString()))); MarkMyTurnEvent(); });
            chat.On<string>("myHit", (destinantionPoint) => { PlayerTargetHitEvent((int.Parse(destinantionPoint[0].ToString()), int.Parse(destinantionPoint[1].ToString()))); MarkMyTurnEvent(); });
            chat.On<string>("myMiss", (destinantionPoint) => { PlayerTargetMissEvent((int.Parse(destinantionPoint[0].ToString()), int.Parse(destinantionPoint[1].ToString()))); MarkOpponentTurnEvent(); });
            chat.On<string, string, string>("myShipDestroyed", (ship, deckCount, liveShips) => { DecreasePlayerShipCountEvent(int.Parse(deckCount), int.Parse(liveShips)); MarkPlayerDestroyedShipEvent(ship); });
            chat.On<string, string, string>("opponentShipDestroyed", (ship, deckCount, liveShips) => { DecreaseOpponentShipCountEvent(int.Parse(deckCount), int.Parse(liveShips)); MarkOpponentDestroyedShipEvent(ship); });
            chat.On<string>("opponentAbortGame", (name) => { NetOpponentAbortGameEvent(name); });
            chat.On<string>("victory", (victoryId) => { if (victoryId == hubConnection.ConnectionId) { NetMyVictoryEvent(); } else { NetOpponentVictoryEvent(); } });
        }
    }
}
