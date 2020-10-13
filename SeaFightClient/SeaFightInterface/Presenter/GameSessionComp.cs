using SeaFightInterface.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFightInterface.Presenter
{
    class GameSessionComp:GameSession
    {
        public GameSessionComp(IView _iView) :base(_iView)
        {
            RegisterGameEvents();
        }

        private void RegisterGameEvents() 
        {
            Game.OpponentShipDestroyed += Game_OpponentShipDestroyed;
            Game.PlayerShipDestroyed += Game_PlayerShipDestroyed;
            Game.OpponentShipHit += Game_OpponentShipHit;
            Game.OpponentShipMiss += Game_OpponentShipMiss;
            Game.PlayerShipHit += Game_PlayerShipHit;
            Game.PlayerShipMiss += Game_PlayerShipMiss;
            Game.VictoryHappened += Game_VictoryHappened;
            Game.DecreaseOpponentShipCount += Game_DecreaseOpponentShipCount;
            Game.DecreasePlayerShipCount += Game_DecreasePlayerShipCount;
            Game.FirstChosen += Game_FirstChosen;
            Game.MarkMyTurnEvent += Game_MarkMyTurnEvent;
            Game.MarkOpponentTurnEvent += Game_MarkOpponentTurnEvent;
        }

        private void Game_MarkOpponentTurnEvent()
        {
            _iView.MarkOpponentTurn();
        }

        private void Game_MarkMyTurnEvent()
        {
            _iView.MarkMyTurn();
        }

        private void Game_FirstChosen(Participants first)
        {
            _iView.BeginGame(first);
        }

        private void Game_DecreasePlayerShipCount(int deck, int value)
        {
            _iView.DecreasePlayerShipCount(deck, value);
        }

        private void Game_DecreaseOpponentShipCount(int deck, int value)
        {
            _iView.DecreaseOpponentShipCount(deck, value);
        }

        private void Game_VictoryHappened(Participants victorious)
        {
            _iView.Victory(victorious);
        }

        private void Game_PlayerShipMiss((int, int) destinationPoint)
        {
            _iView.PlayerTargetMiss(destinationPoint);
        }

        private void Game_PlayerShipHit((int, int) destinationPoint)
        {
            _iView.PlayerTargetHit(destinationPoint);
        }

        private void Game_OpponentShipMiss((int, int) destinationPoint)
        {
            _iView.OpponentTargetMiss(destinationPoint);
        }

        private void Game_OpponentShipHit((int, int) destinationPoint)
        {
            _iView.OpponentTargetHit(destinationPoint);
        }

        private void Game_PlayerShipDestroyed(List<(int, int)> decks)
        {
            _iView.MarkPlayerDestroyedShip(decks);
        }

        private void Game_OpponentShipDestroyed(List<(int, int)> decks)
        {
            _iView.MarkOpponentDestroyedShip(decks);
        }

        protected override void _iView_StartGameEvent(object sender, EventArgs e)
        {
            GameHelper.AutoOpponentShips(Game);
            Game.StartGame();
        }

        protected override void _iView_OpponentFieldClickEvent(int x, int y)
        {
            Game.Turn(x, y);
        }

        protected override void _iView_MoveFinished(int x, int y)
        {
            Game.CompletingTurn(x, y);
        }

        //TO DO
        protected override void _iView_AbortGameEvent()
        {
            
        }
    }
}
