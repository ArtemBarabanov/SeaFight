using SeaFightInterface.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaFightInterface
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MediaPlayerSound sound = new MediaPlayerSound();
            Animation animation = new Animation();
            StartForm startForm = new StartForm(sound, animation);
            StartPresenter start = new StartPresenter(startForm);
            Application.Run(startForm);
        }
    }
}
