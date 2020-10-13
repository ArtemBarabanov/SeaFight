using SeaFightInterface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFightInterface.Presenter
{
    abstract class GameSession
    {
        protected GameState Game;
        protected GamePrepareHelper GameHelper;
        protected IView _iView;

        public GameSession(IView _iView) 
        {
            this._iView = _iView;
            Game = new GameState();
            GameHelper = new GamePrepareHelper(_iView, Game);
            RegisterGameHelperEvents();
        }

        private void RegisterGameHelperEvents()
        {
            GameHelper.ArrowHorizontal += GameHelper_ArrowHorizontal;
            GameHelper.ArrowVertical += GameHelper_ArrowVertical;
            GameHelper.BackTransperent += GameHelper_BackTransperent;
            GameHelper.ChangeColorBad += GameHelper_ChangeColorBad;
            GameHelper.ChangeColorGood += GameHelper_ChangeColorGood;
            GameHelper.ChangeFourDeckCount += GameHelper_ChangeFourDeckCount;
            GameHelper.ChangeOneDeckCount += GameHelper_ChangeOneDeckCount;
            GameHelper.ChangeTwoDeckCount += GameHelper_ChangeTwoDeckCount;
            GameHelper.ChangeThreeDeckCount += GameHelper_ChangeThreeDeckCount;
            GameHelper.PlacePlayerShip += GameHelper_PlacePlayerShip;
            GameHelper.HideStartButton += GameHelper_HideStartButton;
            GameHelper.ShipTypeChanged += GameHelper_ShipTypeChanged;
            GameHelper.ShowStartButton += GameHelper_ShowStartButton;
            #if DEBUG
            GameHelper.PlaceCompShip += GameHelper_PlaceCompShip;
            #endif

            _iView.OpponentFieldClickEvent += _iView_OpponentFieldClickEvent;
            _iView.AutoGenerateShipsEvent += _iView_AutoGenerateShipsEvent;
            _iView.AbortGameEvent += _iView_AbortGameEvent;
            _iView.MouseInEvent += _iView_MouseInEvent;
            _iView.MouseOutEvent += _iView_MouseOutEvent;
            _iView.PlayerFieldClickEvent += _iView_PlayerFieldClickEvent;
            _iView.ChangeShipTypeEvent += _iView_ChangeShipTypeEvent;
            _iView.ShipDirectionEvent += _iView_ShipDirectionEvent;
            _iView.StartGameEvent += _iView_StartGameEvent;
            _iView.AutoGenerateShipsEvent += _iView_AutoGenerateShipsEvent;
            _iView.MoveFinished += _iView_MoveFinished;
        }

        #if DEBUG
        private void GameHelper_PlaceCompShip(List<(int, int)> obj)
        {
            _iView.PlaceCompShip(obj);
        }
        #endif
        protected void GameHelper_ShowStartButton()
        {
            _iView.ShowStartButton();
        }

        protected void GameHelper_ShipTypeChanged(int type)
        {
            _iView.ChangeShipType(type);
        }

        protected void GameHelper_HideStartButton()
        {
            _iView.HideStartButton();
        }

        protected void GameHelper_PlacePlayerShip(List<(int, int)> ship)
        {
            _iView.PlacePlayerShip(ship);
        }

        protected void GameHelper_ChangeThreeDeckCount(int count)
        {
            _iView.ChangeThreeDeckCount(count);
        }

        protected void GameHelper_ChangeTwoDeckCount(int count)
        {
            _iView.ChangeTwoDeckCount(count);
        }

        protected void GameHelper_ChangeOneDeckCount(int count)
        {
            _iView.ChangeOneDeckCount(count);
        }

        protected void GameHelper_ChangeFourDeckCount(int count)
        {
            _iView.ChangeFourDeckCount(count);
        }

        protected void GameHelper_ChangeColorGood(List<(int, int)> ship)
        {
            _iView.ChangeColorGood(ship);
        }

        protected void GameHelper_ChangeColorBad(List<(int, int)> ship)
        {
            _iView.ChangeColorBad(ship);
        }

        protected void GameHelper_BackTransperent(List<(int, int)> ship)
        {
            _iView.BackTransperent(ship);
        }

        protected void GameHelper_ArrowVertical()
        {
            _iView.ArrowVertical();
        }

        protected void GameHelper_ArrowHorizontal()
        {
            _iView.ArrowHorizontal();
        }

        #region Подготовка игры
        //Обработка нажатия на кнопку Лень
        protected void _iView_AutoGenerateShipsEvent()
        {
            GameHelper.AutoPlayerShips(Game);
        }

        //Обработка нажатия на кнопку Направление
        protected void _iView_ShipDirectionEvent(object sender, EventArgs e)
        {
            GameHelper.ChangeShipOrientation();
        }

        //Обработка нажатия на тип корабля
        protected void _iView_ChangeShipTypeEvent(int type)
        {
            GameHelper.ChangeShipType(type);
        }

        //Обработка клика по полю игрока
        protected void _iView_PlayerFieldClickEvent(int x, int y)
        {
            GameHelper.PlayerFieldClick(Game, x, y);
        }

        //Обработка выхода курсора мыши
        protected void _iView_MouseOutEvent()
        {
            GameHelper.MouseOut(Game);
        }

        //Обработка входа курсора мыши
        protected void _iView_MouseInEvent(int x, int y)
        {
            GameHelper.MouseIn(Game, x, y);
        }

        abstract protected void _iView_MoveFinished(int x, int y);
        abstract protected void _iView_AbortGameEvent();
        abstract protected void _iView_StartGameEvent(object sender, EventArgs e);
        abstract protected void _iView_OpponentFieldClickEvent(int x, int y);
        #endregion
    }
}
