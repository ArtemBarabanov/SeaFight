using SeaFightInterface.Model;
using SeaFightInterface.Presenter;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SeaFightInterface
{
    class GameSessionNet: GameSession
    {
        string GameSessionID;
        HubUtility hub;

        public GameSessionNet(IView _iView, string gameSessionID) : base(_iView)
        {
            GameSessionID = gameSessionID;            
            hub = HubUtility.Instance;
            RegisterHubEvents();
        }

        private void RegisterHubEvents()
        {
            hub.BeginNetGameEvent += Hub_BeginNetGameEvent;
            hub.DecreaseOpponentShipCountEvent += Hub_DecreaseOpponentShipCountEvent;
            hub.DecreasePlayerShipCountEvent += Hub_DecreasePlayerShipCountEvent;
            hub.MarkMyTurnEvent += Hub_MarkMyTurnEvent;
            hub.MarkOpponentDestroyedShipEvent += Hub_MarkOpponentDestroyedShipEvent;
            hub.MarkOpponentTurnEvent += Hub_MarkOpponentTurnEvent;
            hub.MarkPlayerDestroyedShipEvent += Hub_MarkPlayerDestroyedShipEvent;
            hub.NetMyVictoryEvent += Hub_NetMyVictoryEvent;
            hub.NetOpponentAbortGameEvent += Hub_NetOpponentAbortGameEvent;
            hub.NetOpponentVictoryEvent += Hub_NetOpponentVictoryEvent;
            hub.OpponentTargetHitEvent += Hub_OpponentTargetHitEvent;
            hub.OpponentTargetMissEvent += Hub_OpponentTargetMissEvent;
            hub.PlayerTargetHitEvent += Hub_PlayerTargetHitEvent;
            hub.PlayerTargetMissEvent += Hub_PlayerTargetMissEvent;
        }

        private void DeregisterHubEvents()
        {
            hub.BeginNetGameEvent -= Hub_BeginNetGameEvent;
            hub.DecreaseOpponentShipCountEvent -= Hub_DecreaseOpponentShipCountEvent;
            hub.DecreasePlayerShipCountEvent -= Hub_DecreasePlayerShipCountEvent;
            hub.MarkMyTurnEvent -= Hub_MarkMyTurnEvent;
            hub.MarkOpponentDestroyedShipEvent -= Hub_MarkOpponentDestroyedShipEvent;
            hub.MarkOpponentTurnEvent -= Hub_MarkOpponentTurnEvent;
            hub.MarkPlayerDestroyedShipEvent -= Hub_MarkPlayerDestroyedShipEvent;
            hub.NetMyVictoryEvent -= Hub_NetMyVictoryEvent;
            hub.NetOpponentAbortGameEvent -= Hub_NetOpponentAbortGameEvent;
            hub.NetOpponentVictoryEvent -= Hub_NetOpponentVictoryEvent;
            hub.OpponentTargetHitEvent -= Hub_OpponentTargetHitEvent;
            hub.OpponentTargetMissEvent -= Hub_OpponentTargetMissEvent;
            hub.PlayerTargetHitEvent -= Hub_PlayerTargetHitEvent;
            hub.PlayerTargetMissEvent -= Hub_PlayerTargetMissEvent;
        }

        private void Hub_PlayerTargetMissEvent((int, int) destinantionPoint)
        {
            _iView.PlayerTargetMiss(destinantionPoint);
        }

        private void Hub_PlayerTargetHitEvent((int, int) destinantionPoint)
        {
            _iView.PlayerTargetHit(destinantionPoint);
        }

        private void Hub_OpponentTargetMissEvent((int, int) destinantionPoint)
        {
            _iView.OpponentTargetMiss(destinantionPoint);
        }

        private void Hub_OpponentTargetHitEvent((int, int) destinantionPoint)
        {
            _iView.OpponentTargetHit(destinantionPoint);
        }

        private void Hub_NetMyVictoryEvent()
        {
            _iView.NetMyVictory();
        }

        private void Hub_NetOpponentVictoryEvent()
        {
            _iView.NetOpponentVictory();
        }

        private void Hub_NetOpponentAbortGameEvent(string name)
        {
            _iView.NetOpponentAbortGame(name);
        }

        private void Hub_MarkPlayerDestroyedShipEvent(string ship)
        {
            _iView.MarkPlayerDestroyedShip(ListDecks(ship));
        }

        private void Hub_MarkOpponentTurnEvent()
        {
            _iView.MarkOpponentTurn();
        }

        private void Hub_MarkOpponentDestroyedShipEvent(string ship)
        {
            _iView.MarkOpponentDestroyedShip(ListDecks(ship));
        }

        private void Hub_MarkMyTurnEvent()
        {
            _iView.MarkMyTurn();
        }

        private void Hub_DecreasePlayerShipCountEvent(int deckCount, int liveShips)
        {
            _iView.DecreasePlayerShipCount(deckCount, liveShips);
        }

        private void Hub_DecreaseOpponentShipCountEvent(int deckCount, int liveShips)
        {
            _iView.DecreaseOpponentShipCount(deckCount, liveShips);
        }

        private void Hub_BeginNetGameEvent(string whoIsFirstId, string whoIsFirstName)
        {
            _iView.BeginNetGame(whoIsFirstName);
            if (whoIsFirstId == hub.hubConnection.ConnectionId)
            {
                _iView.MarkMyTurn();
            }
            else
            {
                _iView.MarkOpponentTurn();
            }
        }

        private List<(int, int)> ListDecks(string ship)
        {
            List<(int, int)> decks = new List<(int, int)>();

            foreach (string d in ship.Split('-'))
            {
                decks.Add((int.Parse(d[0].ToString()), int.Parse(d[1].ToString())));
            }

            return decks;
        }

        protected override void _iView_AbortGameEvent()
        {
            DeregisterHubEvents();
            hub.chat.Invoke("NetAbortGameEvent");
        }

        //Обработка нажатия на кнопку В бой!
        protected override void _iView_StartGameEvent(object sender, EventArgs e)
        {
            string ships = "";

            foreach (Ship s in Game.GetPlayerShips())
            {
                if (s.decks.Count == 1)
                {
                    ships += "1:";
                    foreach (Deck d in s.decks)
                    {
                        ships += $"{d.X}{d.Y}";
                    }
                    ships += "|";
                }
                else if (s.decks.Count == 2)
                {
                    ships += "2:";
                    foreach (Deck d in s.decks)
                    {
                        ships += $"{d.X}{d.Y}-";
                    }
                    ships += "|";
                }
                else if (s.decks.Count == 3)
                {
                    ships += "3:";
                    foreach (Deck d in s.decks)
                    {
                        ships += $"{d.X}{d.Y}-";
                    }
                    ships += "|";
                }
                else if (s.decks.Count == 4)
                {
                    ships += "4:";
                    foreach (Deck d in s.decks)
                    {
                        ships += $"{d.X}{d.Y}-";
                    }
                    ships += "|";
                }
            }

            hub.chat.Invoke("ReadyToStart", GameSessionID, hub.hubConnection.ConnectionId, ships);
        }

        protected override void _iView_MoveFinished(int x, int y)
        {
            hub.chat.Invoke("CompletingTurn", GameSessionID, hub.hubConnection.ConnectionId, x.ToString(), y.ToString());
        }

        protected override void _iView_OpponentFieldClickEvent(int x, int y)
        {
            hub.chat.Invoke("Move", GameSessionID, hub.hubConnection.ConnectionId, x.ToString(), y.ToString());
        }
    }
}

