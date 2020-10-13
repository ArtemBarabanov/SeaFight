using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFightInterface.Presenter
{
    class StartPresenter
    {
        IStartView _iStartView;
        HubUtility hub;
        string Name;

        public StartPresenter(IStartView _iStartView)
        {
            this._iStartView = _iStartView;
            _iStartView.StartCompGameEvent += _iStartView_StartCompGameEvent;
            _iStartView.ConnectEvent += _iStartView_ConnectEvent;
            _iStartView.DisconnectEvent += _iStartView_DisconnectEvent;
            _iStartView.SendMessageEvent += _iStartView_SendMessageEvent;
            _iStartView.OfferGameEvent += _iStartView_OfferGameEvent;
            _iStartView.AnswerOfferEvent += _iStartView_AnswerOfferEvent;
            _iStartView.StartNetGameEvent += _iStartView_StartNetGameEvent;
        }

        private void _iStartView_StartCompGameEvent(IView gameForm)
        {
            GameSession GameSession = new GameSessionComp(gameForm);
        }

        private void _iStartView_DisconnectEvent()
        {
            if (hub != null && hub.hubConnection.State == ConnectionState.Connected)
            {
                hub.Disconnect();
                DeregisterHubEvents();
            }
        }

        private void _iStartView_StartNetGameEvent(IView gameForm, string idFrom, string idTo, string sessionID)
        {
            GameSession GameSession = new GameSessionNet(gameForm, sessionID);
        }

        private void _iStartView_AnswerOfferEvent(string idFrom, string answer)
        {
            hub.chat.Invoke("AnswerOffer", $"{idFrom}", $"{hub.hubConnection.ConnectionId}", $"{answer}");
        }

        private void _iStartView_OfferGameEvent(string idTo)
        {
            if (idTo != hub.hubConnection.ConnectionId)
            {
                hub.chat.Invoke("OfferGame", $"{hub.hubConnection.ConnectionId}", $"{idTo}");
            }
            else
            {
                _iStartView.NameMatched();
            }
        }

        private void _iStartView_SendMessageEvent(string name, string message)
        {
            hub.chat.Invoke("SendMessage", $"{name}", $"{message}");
        }

        private void _iStartView_ConnectEvent(string name)
        {
            Task.Run(() =>
            {
                Name = name;
                hub = HubUtility.Instance;
                RegisterHubEvents();
                hub.Connect();
            });
        }

        private void RegisterHubEvents()
        {
            hub.OnConnected += Hub_OnConnected;
            hub.OnError += Hub_OnError;
            hub.AnswerOfferingEvent += Hub_AnswerOfferingEvent;
            hub.DenyOfferingHeEvent += Hub_DenyOfferingHeEvent;
            hub.DenyOfferingYouEvent += Hub_DenyOfferingYouEvent;
            hub.OfferToYouEvent += Hub_OfferToYouEvent;
            hub.ShowPlayersEvent += Hub_ShowPlayersEvent;
            hub.UpdateMessageListEvent += Hub_UpdateMessageListEvent;
        }

        private void Hub_OnError()
        {
            _iStartView.ErrorConnection();
        }

        private void Hub_OnConnected()
        {
            _iStartView.ActivateNetPanel();
            hub.chat.Invoke("Register", $"{Name}");
        }

        private void Hub_UpdateMessageListEvent(string name, string message)
        {
            _iStartView.UpdateMessageList(name, message);
        }

        private void Hub_ShowPlayersEvent(List<string> players, string myName)
        {
            _iStartView.ShowPlayers(players, myName);
        }

        private void Hub_OfferToYouEvent(string nameFrom, string idFrom, string nameTo)
        {
            _iStartView.OfferToYou(nameFrom, idFrom, nameTo);
        }

        private void Hub_DenyOfferingYouEvent()
        {
            _iStartView.DenyOfferingYou();
        }

        private void Hub_DenyOfferingHeEvent()
        {
            _iStartView.DenyOfferingHe();
        }

        private void Hub_AnswerOfferingEvent(string nameFrom, string nameTo, string answer, string sessionID)
        {
            _iStartView.AnswerOffering(nameFrom, nameTo, answer, sessionID);
        }

        private void DeregisterHubEvents()
        {
            hub.AnswerOfferingEvent -= Hub_AnswerOfferingEvent;
            hub.DenyOfferingHeEvent -= Hub_DenyOfferingHeEvent;
            hub.DenyOfferingYouEvent -= Hub_DenyOfferingYouEvent;
            hub.OfferToYouEvent -= Hub_OfferToYouEvent;
            hub.ShowPlayersEvent -= Hub_ShowPlayersEvent;
            hub.UpdateMessageListEvent -= Hub_UpdateMessageListEvent;
        }
    }
}
