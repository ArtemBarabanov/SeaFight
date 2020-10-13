using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using SeaFightServer.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SeaFightServer
{
    public class GameHub: Hub
    {
        //Подключения
        static ConcurrentDictionary<string, Player> Connections = new ConcurrentDictionary<string, Player>();
        //Сессии
        static List<GameSession> Sessions = new List<GameSession>();

        #region События игровой сессии
        private void SessionEventRegistration(GameSession session) 
        {
            session.EveryOneReadyEvent += Session_EveryOneReadyEvent;
            session.WinEvent += Session_WinEvent;
            session.MyHitEvent += Session_MyHitEvent;
            session.MyMissEvent += Session_MyMissEvent;
            session.OpponentHitEvent += Session_OpponentHitEvent;
            session.OpponentMissEvent += Session_OpponentMissEvent;
            session.MyShipDestroyedEvent += Session_MyShipDestroyedEvent;
            session.OpponentShipDestroyedEvent += Session_OpponentShipDestroyedEvent;
        }

        private void SessionEventDeregistration(GameSession session)
        {
            session.EveryOneReadyEvent -= Session_EveryOneReadyEvent;
            session.WinEvent -= Session_WinEvent;
            session.MyHitEvent -= Session_MyHitEvent;
            session.MyMissEvent -= Session_MyMissEvent;
            session.OpponentHitEvent -= Session_OpponentHitEvent;
            session.OpponentMissEvent -= Session_OpponentMissEvent;
            session.MyShipDestroyedEvent -= Session_MyShipDestroyedEvent;
            session.OpponentShipDestroyedEvent -= Session_OpponentShipDestroyedEvent;
        }

        /// <summary>
        /// Уничтожение корабля игрока
        /// </summary>
        /// <param name="id"></param>
        /// <param name="opponentName"></param>
        /// <param name="ship"></param>
        /// <param name="deckCount"></param>
        /// <param name="liveShips"></param>
        private void Session_MyShipDestroyedEvent(string id, string opponentName, string ship, string deckCount, string liveShips)
        {
            Clients.Client(id).myShipDestroyed(ship, deckCount, liveShips);
        }

        /// <summary>
        /// Уничтожение корабля противника
        /// </summary>
        /// <param name="id"></param>
        /// <param name="opponentName"></param>
        /// <param name="ship"></param>
        /// <param name="deckCount"></param>
        /// <param name="liveShips"></param>
        private void Session_OpponentShipDestroyedEvent(string id, string opponentName, string ship, string deckCount, string liveShips)
        {
            Clients.Client(id).opponentShipDestroyed(ship, deckCount, liveShips);
        }

        /// <summary>
        /// Промах противника
        /// </summary>
        /// <param name="id"></param>
        /// <param name="destinationPoint"></param>
        private void Session_OpponentMissEvent(string id, string destinationPoint)
        {
            Clients.Client(id).opponentMiss(destinationPoint);
        }

        /// <summary>
        /// Попадание противника
        /// </summary>
        /// <param name="id"></param>
        /// <param name="destinationPoint"></param>
        private void Session_OpponentHitEvent(string id, string destinationPoint)
        {
            Clients.Client(id).opponentHit(destinationPoint);
        }

        /// <summary>
        /// Промах игрока
        /// </summary>
        /// <param name="id"></param>
        /// <param name="destinationPoint"></param>
        private void Session_MyMissEvent(string id, string destinationPoint)
        {
            Clients.Client(id).myMiss(destinationPoint);
        }

        /// <summary>
        /// Попадание игрока
        /// </summary>
        /// <param name="id"></param>
        /// <param name="destinationPoint"></param>
        private void Session_MyHitEvent(string id, string destinationPoint)
        {
            Clients.Client(id).myHit(destinationPoint);
        }

        //Все готовы к игре
        private void Session_EveryOneReadyEvent(object sender, EventArgs e)
        {
            GameSession Session = sender as GameSession;
            Clients.Clients(new List<string> { Session.Players[0].Id, Session.Players[1].Id }).startGame(Session.WhoFirstId, Session.WhoFirstName);
        }
        #endregion

        #region Обработка сообщений клиента
        //Отправка сообщений в чат
        public void SendMessage(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

        //Предложение игры
        public void OfferGame(string idFrom, string idTo)
        {
            int imBusy = (from s in Sessions from p in s.Players where p.Id == idFrom select p).Count();
            if (imBusy != 0)
            {
                Clients.Client(Connections[idFrom].Id).denyOfferingYouBusy();
            }
            else
            {
                int OpponentBusy = (from s in Sessions from p in s.Players where p.Id == idTo select p).Count();
                if (OpponentBusy == 0)
                {
                    Clients.Client(idTo).gameOffering(Connections[idFrom].Name, idFrom, Connections[idTo].Name);
                }
                else
                {
                    Clients.Client(idFrom).denyOfferingHeBusy();
                }
            }
        }

        //Ход
        public void Move(string sessionID, string id, string x, string y)
        {
            GameSession Session = Sessions.Find(f => f.ID == sessionID);
            Session.Move(id, x, y);
        }

        //Проверка корабля на уничтожение
        public void CompletingTurn(string sessionID, string connectionId, string x, string y)
        {
            GameSession Session = Sessions.Find(f => f.ID == sessionID);
            Session.CompletingTurn(connectionId, x, y);
        }

        //Ответ на предложение
        public void AnswerOffer(string idFrom, string idTo, string answer)
        {
            if (answer == "yes")
            {
                Player player1 = Connections[idFrom];
                Player player2 = Connections[idTo];
                player1.IsBusy = true;
                player2.IsBusy = true;

                GameSession session = new GameSession(Guid.NewGuid().ToString(), new List<Player> { player1, player2 });
                SessionEventRegistration(session);
                Sessions.Add(session);
                Clients.Clients(new List<string> { idTo, idFrom }).answerOffering(player1.Name, player2.Name, answer, session.ID);
                SendPlayers();
            }
            else
            {
                Clients.Client(idFrom).answerOffering(Connections[idTo].Name, Connections[idFrom].Name, answer, null, null);
            }
        }

        //Игрок готов к игре
        public void ReadyToStart(string sessionID, string id, string ships)
        {
            GameSession Session = Sessions.Find(f => f.ID == sessionID);
            Session.AddShip(id, ships);
        }

        //Прерывание игры
        public void NetAbortGameEvent()
        {
            string id = Context.ConnectionId;
            GameSession Session = (from s in Sessions from p in s.Players where p.Id == id select s).FirstOrDefault();
            AbortGame(id, Session);
        }

        private void Session_WinEvent(object sender, EventArgs e)
        {
            GameSession session = sender as GameSession;
            Clients.Clients(new List<string> { session.Players[0].Id, session.Players[1].Id }).victory(session.VictoryId);
            session.EveryOneReadyEvent -= Session_EveryOneReadyEvent;
            session.WinEvent -= Session_WinEvent;
            session.MyHitEvent -= Session_MyHitEvent;
            session.MyMissEvent -= Session_MyMissEvent;
            session.OpponentHitEvent -= Session_OpponentHitEvent;
            session.OpponentMissEvent -= Session_OpponentMissEvent;
            session.MyShipDestroyedEvent -= Session_MyShipDestroyedEvent;
            session.OpponentShipDestroyedEvent -= Session_OpponentShipDestroyedEvent;

            foreach (Player p in session.Players) 
            {
                p.IsBusy = false;
                p.Quit();
            }
            Sessions.Remove(session);

            SendPlayers();
        }

        //Регистрация пользователя
        public void Register(string name)
        {
            string id = Context.ConnectionId;
            Connections.TryAdd(id, new Player(name, id));
            SendPlayers();
        }

        //Отправка игроков
        public void SendPlayers()
        {
            List<string> answer = new List<string>();
            foreach (Player player in Connections.Values)
            {
                answer.Add($"{player.Name}|{player.Id}|{player.IsBusy}");
            }
            Clients.All.getPlayers(answer);
        }

        private void AbortGame(string id, GameSession Session) 
        {
            Connections[id].IsBusy = false;
            Connections[id].Quit();
            Player opponent = (from s in Sessions from p in s.Players where p.Id != id select p).FirstOrDefault();
            Player sender = (from s in Sessions from p in s.Players where p.Id == id select p).FirstOrDefault();
            if (opponent != null)
            {
                opponent.IsBusy = false;
                opponent.Quit();
            }

            Clients.Client(opponent.Id).opponentAbortGame(sender.Name);
            SessionEventDeregistration(Session);
            Sessions.Remove(Session);
            SendPlayers();
        }

        //Выход пользователя
        public void Disconnect()
        {
            string id = Context.ConnectionId;
            GameSession Session = (from s in Sessions from p in s.Players where p.Id == id select s).FirstOrDefault();

            if (Session != null)
            {
                AbortGame(id, Session);
            }
            else
            {
                Player s;
                Connections.TryRemove(id, out s);
                SendPlayers();
            }
        }
        #endregion

        //Завершение работы с пользователем
        public override Task OnDisconnected(bool stopCalled)
        {
            Disconnect();
            return base.OnDisconnected(stopCalled);
        }
    }
}