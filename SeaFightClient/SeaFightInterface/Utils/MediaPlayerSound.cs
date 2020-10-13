using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace SeaFightInterface
{
    class MediaPlayerSound: ISound
    {
        SoundPlayer shot;
        SoundPlayer pluk;
        SoundPlayer explosion;
        SoundPlayer rynda;
        SoundPlayer seaAndGulls;

        public void Explosion()
        {
            using (explosion = new SoundPlayer(Properties.Resources.Взрыв))
            {
                explosion.Play();
            }
        }

        public void Pluk()
        {
            using (pluk = new SoundPlayer(Properties.Resources.Плюх))
            {
                pluk.Play();
            }
        }

        public void Shot()
        {
            using (shot = new SoundPlayer(Properties.Resources.Выстрел))
            {
                shot.Play();
            }
        }

        public void Rynda()
        {
            using (rynda = new SoundPlayer(Properties.Resources.Рында))
            {
                rynda.Play();
            }
        }

        public void SeaAndGulls()
        {
            using (seaAndGulls = new SoundPlayer(Properties.Resources.seaAndGull))
            {
                seaAndGulls.Play();
            }
        }

        public void StopSeaAndGulls()
        {
            if (seaAndGulls != null)
            {
                seaAndGulls.Stop();
                seaAndGulls.Dispose();
                seaAndGulls = null;
            }
        }
        //MediaPlayer rynda = new MediaPlayer();

        //MediaPlayer shot = new MediaPlayer();
        //MediaPlayer pluk = new MediaPlayer();
        //MediaPlayer explosion = new MediaPlayer();

        //MediaPlayer seaAndGulls = new MediaPlayer();

        //public void Explosion()
        //{
        //    explosion.Open(new Uri("Взрыв.wav", UriKind.Relative));
        //    explosion.Play();
        //}

        //public void Pluk()
        //{
        //    pluk.Open(new Uri(@"C:\Users\Администратор\Documents\Visual Studio 2017\Projects\SeaFightInterface\SeaFightInterface\Resources\Плюх.wav", UriKind.Relative));
        //    pluk.Play();
        //}

        //public void Shot()
        //{
        //    shot.Open(new Uri(Environment.CurrentDirectory + "Выстрел.wav", UriKind.Relative));
        //    shot.Play();
        //}

        //public void Rynda()
        //{
        //    rynda.MediaFailed += Rynda_MediaFailed;
        //    rynda.Open(new Uri($@"C:\Users\Администратор\Documents\Visual Studio 2017\Projects\SeaFightInterface\SeaFightInterface\Resources\Рында.wav", UriKind.RelativeOrAbsolute));
        //    rynda.Play();
        //}

        //private void Rynda_MediaFailed(object sender, ExceptionEventArgs e)
        //{
        //    MessageBox.Show(e.ErrorException.Message);
        //}

        //public void SeaAndGulls()
        //{
        //    seaAndGulls.Open(new Uri(@"C:\Users\Администратор\Documents\Visual Studio 2017\Projects\SeaFightInterface\SeaFightInterface\Resources\Чайка.wav", UriKind.Relative));
        //    seaAndGulls.Play();
        //}

        //public void StopSeaAndGulls()
        //{
        //    if (seaAndGulls != null)
        //    {
        //        seaAndGulls.Stop();
        //        seaAndGulls = null;
        //    }
        //}
    }
}
