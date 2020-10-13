using SeaFightInterface.Presenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNet.SignalR.Client;
using System.Threading;

namespace SeaFightInterface
{
    public partial class StartForm : Form, IStartView
    {
        CancellationTokenSource CtsConnected;
        CancellationTokenSource CtsError;
        CancellationTokenSource CtsCancel;
        SynchronizationContext UIContext;        
        IAnimation _iAnimation;
        ISound _iSound;

        public StartForm(ISound _iSound, IAnimation _iAnimation)
        {
            InitializeComponent();
            UIContext = SynchronizationContext.Current;
            CreateStartScreen();
            this._iSound = _iSound;
            this._iAnimation = _iAnimation;
        }

        //Кнопка Игра с компьютером
        private void CompGame_Click(object sender, EventArgs e)
        {
            GameForm gameForm = new GameForm(_iSound, _iAnimation);
            gameForm.Owner = this;
            gameForm.Show();
            Hide();
            StartCompGameEvent(gameForm);
        }

        //Кнопка Игра по сети
        private void NetGame_Click(object sender, EventArgs e)
        {
            CreateChatScreen();
        }

        //Обработка закрытия формы
        private void StartForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DisconnectEvent?.Invoke();
        }
    }
}
