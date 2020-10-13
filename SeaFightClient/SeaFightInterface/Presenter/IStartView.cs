using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFightInterface.Presenter
{
    public interface IStartView
    {
        event Action<IView> StartCompGameEvent;
        event Action<IView, string, string, string> StartNetGameEvent;
        event Action<string> ConnectEvent;
        event Action DisconnectEvent;
        event Action<string, string> SendMessageEvent;
        event Action<string> OfferGameEvent;
        event Action<string, string> AnswerOfferEvent;

        void ActivateNetPanel();
        void ErrorConnection();
        void ShowPlayers(List<string> players, string myName);
        void UpdateMessageList(string name, string message);
        void OfferToYou(string nameFrom, string idFrom, string nameTo);
        void AnswerOffering(string nameFrom, string nameTo, string answer, string sessionID);
        void DenyOfferingYou();
        void DenyOfferingHe();
        void NameMatched();
    }
}
