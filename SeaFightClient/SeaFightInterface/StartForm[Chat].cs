using SeaFightInterface.Presenter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaFightInterface
{
    partial class StartForm
    {
        //Кнопка Назад
        private void Back_Click(object sender, EventArgs e)
        {
            CtsCancel?.Cancel();
            DeactivateChatPanel();
            DestroyChatScreen();
            DisconnectEvent?.Invoke();
            WindowSizeTimer.Enabled = true;
            CreateStartScreen();
        }

        //Двойной клик по строке DataGridView
        private void PlayersGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string id = PlayersGrid.Rows[e.RowIndex].Cells["Id"].Value.ToString();
            OfferGameEvent(id);
        }

        //Кнопка Отправить
        private void SendMessage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Message.Text))
            {
                SendMessageEvent(NameBox.Text, Message.Text);
            }
            else 
            {
                Message.BackColor = Color.LightCoral;
                MessageBox.Show("Введите текст сообщения!");
            }
        }

        //Кнопка Регистрация/Выход
        private void Register_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(NameBox.Text))
            {
                if (!regButtonState)
                {
                    Register.Enabled = false;
                    CtsConnected = new CancellationTokenSource();
                    CtsError = new CancellationTokenSource();
                    CtsCancel = new CancellationTokenSource();
                    ConnectEvent(NameBox.Text);
                    ChangeText();
                }
                else
                {
                    CtsCancel?.Cancel();
                    StatusBox.Text = "Статус";
                    StatusBox.BackColor = Color.White;
                    PlayersGrid.Rows.Clear();
                    DeactivateChatPanel();
                    DisconnectEvent?.Invoke();
                }
            }
            else
            {
                NameBox.BackColor = Color.LightCoral;
                MessageBox.Show("Введите имя для регистрации!");
            }
        }
        //Изменение статуса подключения
        public async void ChangeText() 
        {
            for (int i = 1; i < 4; i++)
            {
                if (CtsError.IsCancellationRequested)
                {
                    CtsError.Dispose();
                    CtsError = null;
                    StatusBox.Text = "Ошибка!";
                    StatusBox.BackColor = Color.LightCoral;
                    MessageBox.Show("Ошибка сервера!");
                    break;
                }
                else if (CtsConnected.IsCancellationRequested)
                {
                    CtsConnected.Dispose();
                    CtsConnected = null;
                    StatusBox.Text = "Подключено!";
                    StatusBox.BackColor = Color.LightGreen;
                    break;
                }
                else if (CtsCancel.IsCancellationRequested)
                {
                    CtsCancel.Dispose();
                    CtsCancel = null;
                    break;
                }
                else 
                {
                    StatusBox.BackColor = Color.White;
                    StatusBox.Text = "Подключаем" + new string('.', i);
                    await Task.Delay(500);
                    if (i == 3)
                    {
                        i = 0;
                    }
                }
            }
        }

        #region Реализация интерфейса IStartView
        public event Action<string> ConnectEvent;
        public event Action DisconnectEvent;
        public event Action<string, string> SendMessageEvent;
        public event Action<string> OfferGameEvent;
        public event Action<string, string> AnswerOfferEvent;
        public event Action<IView, string, string, string> StartNetGameEvent;
        public event Action<IView> StartCompGameEvent;

        public void ShowPlayers(List<string> players, string id)
        {
            PlayersGrid.Invoke(new Action(() => FilterDataGrid(players, id)));
        }

        public void ActivateNetPanel()
        { 
            CtsConnected?.Cancel();
            UIContext.Post(new SendOrPostCallback(o => { Register.Enabled = true; ActivateChatPanel(); }), null);
        }

        public void ErrorConnection()
        {
            CtsError?.Cancel();
        }

        private void FilterDataGrid(List<string> players, string id)
        {
            PlayersGrid.Rows.Clear();

            for (int i = 0; i < players.Count; i++)
            {
                string[] temp = players[i].Split('|');

                PlayersGrid.Rows.Add(temp);

                if (temp[1] == id)
                {
                    PlayersGrid.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                    PlayersGrid.Rows[i].Cells[0].Value += " - это Вы";
                }
                else
                {
                    if (temp[2] == "True")
                    {
                        PlayersGrid.Rows[i].DefaultCellStyle.BackColor = Color.LightCoral;
                        PlayersGrid.Rows[i].Cells[0].Value += " - играет";
                    }
                    else
                    {
                        PlayersGrid.Rows[i].Cells[0].Value += " - свободен";
                    }
                }
            }
        }

        public void UpdateMessageList(string name, string message)
        {
            UIContext.Post(new SendOrPostCallback(o => { ChatBox.Text += $"{name} - {message}" + Environment.NewLine; Message.Text = ""; }), null);
        }

        public void OfferToYou(string nameFrom, string idFrom, string nameTo)
        {
            UIContext.Post(new SendOrPostCallback(o =>
            {
                if (MessageBox.Show($"Вам предлагает сыграть {nameFrom}!", "Предложение!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    AnswerOfferEvent(idFrom, "yes");
                }
                else
                {
                    AnswerOfferEvent(idFrom, "no");
                }
            }), null);
        }

        public void AnswerOffering(string nameFrom, string nameTo, string answer, string sessionID)
        {
            UIContext.Post(new SendOrPostCallback(o =>
            {
                if (answer == "yes")
                {
                    MessageBox.Show($"Игра начинается!"); GameForm gameForm = new GameForm(_iSound, _iAnimation); gameForm.Show(); StartNetGameEvent(gameForm, nameFrom, nameTo, sessionID);
                }
                else
                {
                    MessageBox.Show($"{nameFrom} отказался(-лась) играть!");
                }
            }), null);
        }

        public void DenyOfferingHe()
        {
            MessageBox.Show("Данный игрок уже играет!");
        }

        public void DenyOfferingYou()
        {
            MessageBox.Show("Вы уже играете!");
        }

        public void NameMatched()
        {
            MessageBox.Show("Нельзя выбрать себя!");
        }
        #endregion
    }
}
