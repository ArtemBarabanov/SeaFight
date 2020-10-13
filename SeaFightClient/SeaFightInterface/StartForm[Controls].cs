using System;
using System.Drawing;
using System.Windows.Forms;

namespace SeaFightInterface
{
    partial class StartForm
    {
        Label NameLabel;
        Label MessageLabel;
        Button NetGame;
        Button CompGame;
        Button Register;
        Button SendMessage;
        Button Back;
        TextBox Message;
        TextBox NameBox;
        TextBox ChatBox;
        TextBox StatusBox;
        DataGridView PlayersGrid;

        bool regButtonState;
        bool IsShrinking;

        /// <summary>
        /// Создание стартового окна
        /// </summary>
        private void CreateStartScreen()
        {
            //Кнопка игры по сети
            NetGame = new Button()
            {
                Text = "Игра по сети",
                Location = new Point(40, 20),
                Size = new Size(300, 50)
            };
            NetGame.Click += NetGame_Click;
            Controls.Add(NetGame);

            //Кнопка игры с компьютером
            CompGame = new Button()
            {
                Text = "Игра с компьютером",
                Location = new Point(40, 90),
                Size = new Size(300, 50)
            };
            CompGame.Click += CompGame_Click;
            Controls.Add(CompGame);
        }

        /// <summary>
        /// Изменение стартового окна, если выбрана сетевая игра
        /// </summary>
        private void CreateChatControls()
        {
            //TextBox статуса подключения
            StatusBox = new TextBox()
            {
                Text = "Статус",
                Size = new Size(90, 20),
                Location = new Point(290, 320),
                Enabled = false,
                BackColor = Color.White
            };

            //Label для поля ввода имени
            NameLabel = new Label()
            {
                Text = "Имя игрока",
                Location = new Point(300, 235)
            };

            //Кнопка Регистрация/Выход
            Register = new Button()
            {
                Text = "Регистрация",
                Size = new Size(90, 20),
                Location = new Point(290, 290)
            };
            Register.Click += Register_Click;

            //Кнопка Назад
            Back = new Button()
            {
                Text = "<< Назад",
                Size = new Size(90, 20),
                Location = new Point(290, 350)
            };
            Back.Click += Back_Click;

            //Кнопка отправить сообщение
            SendMessage = new Button()
            {
                Text = "Отправить сообщение",
                Size = new Size(385, 20),
                Location = new Point(0, 190),
                Enabled = false
            };
            SendMessage.Click += SendMessage_Click;

            //TextBox ввода имени
            NameBox = new TextBox()
            {
                Size = new Size(90, 20),
                Location = new Point(290, 260),
            };
            NameBox.Click += NameBox_Click;

            //TextBox отображения сообщений чата
            ChatBox = new TextBox()
            {
                Size = new Size(385, 150),
                Multiline = true,
                Enabled = false,
                ScrollBars = ScrollBars.Vertical
            };

            //Label для поля ввода сообщений
            MessageLabel = new Label()
            {
                Text = "Ваше сообщение",
                Location = new Point(0, 150)
            };

            //TextBox ввода сообщений
            Message = new TextBox()
            {
                Size = new Size(390, 20),
                Location = new Point(0, 170),
                Enabled = false,
                
            };
            Message.Click += Message_Click;

            //DataGrid с зарегистрированными игроками
            PlayersGrid = new DataGridView()
            {
                Size = new Size(230, 150),
                Location = new Point(0, 230),
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };
            PlayersGrid.Columns.Add("Players", "Игроки");
            PlayersGrid.Columns.Add("Id", "Id");
            PlayersGrid.Columns["Id"].Visible = false;
            PlayersGrid.CellMouseDoubleClick += PlayersGrid_CellMouseDoubleClick;

            Controls.Add(StatusBox);
            Controls.Add(NameLabel);
            Controls.Add(Register);
            Controls.Add(Back);
            Controls.Add(NameBox);
            Controls.Add(ChatBox);
            Controls.Add(Message);
            Controls.Add(MessageLabel);
            Controls.Add(SendMessage);
            Controls.Add(PlayersGrid);
        }

        private void Message_Click(object sender, EventArgs e)
        {
            Message.BackColor = Color.White;
        }

        private void NameBox_Click(object sender, EventArgs e)
        {
            NameBox.BackColor = Color.White;
        }

        private void DestroyStartScreen()
        {
            NetGame.Dispose();
            CompGame.Dispose();
            NetGame = null;
            CompGame = null;
        }

        private void DestroyChatScreen()
        {
            StatusBox.Dispose();
            Register.Dispose();
            Back.Dispose();
            NameBox.Dispose();
            ChatBox.Dispose();
            Message.Dispose();
            SendMessage.Dispose();
            PlayersGrid.Dispose();
            StatusBox = null;
            Register = null;
            Back = null;
            NameBox = null;
            ChatBox = null;
            Message = null;
            SendMessage = null;
            PlayersGrid = null;
        }

        private void CreateChatScreen()
        {
            DestroyStartScreen();
            WindowSizeTimer.Enabled = true;
            CreateChatControls();
        }

        private void WindowSizeTimer_Tick(object sender, EventArgs e)
        {
            if (!IsShrinking)
            {
                if (Height < 420)
                {
                    Height += 5;
                }
                else
                {
                    WindowSizeTimer.Stop();
                    IsShrinking = true;
                }
            }
            else
            {
                if (Height > 205)
                {
                    Height -= 5;
                }
                else
                {
                    WindowSizeTimer.Stop();
                    IsShrinking = false;
                }
            }
        }

        private void ActivateChatPanel() 
        {
            SendMessage.Enabled = true;
            Message.Enabled = true;
            NameBox.Enabled = false;
            Register.Text = "Выход";
            regButtonState = true;
        }

        private void DeactivateChatPanel()
        {
            SendMessage.Enabled = false;
            Message.Enabled = false;
            NameBox.Enabled = true;
            Register.Text = "Регистрация";
            regButtonState = false;
        }
    }
}
