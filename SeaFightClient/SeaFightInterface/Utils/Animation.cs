using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;

namespace SeaFightInterface
{
    class Animation : IAnimation
    {
        System.Windows.Forms.Timer FrameTimer = new System.Windows.Forms.Timer();
        ResourceManager imageResources = new ResourceManager(typeof(Properties.Resources));
        IEnumerator<(double, double)> FlyPath;
        PictureBox pctr;

        public Animation() 
        {
            FrameTimer.Tick += FrameTimer_Tick;
            FrameTimer.Interval = 100;
        }

        private void FrameTimer_Tick(object sender, EventArgs e)
        {
            if (FlyPath.MoveNext())
            {
                pctr.Location = new Point((int)FlyPath.Current.Item1, (int)FlyPath.Current.Item2);
            }
        }

        public void Explosion(object obj)
        {
            PictureBox pctr = obj as PictureBox;

            for (int i = 1; i < 15; i++)
            {
                pctr.Image = (Image)imageResources.GetObject($@"newExpl{i}");
                Thread.Sleep(50);
            }
            for (int i = 14; i > 0; i--)
            {
                pctr.Image = (Image)imageResources.GetObject($@"newExpl{i}");
                Thread.Sleep(50);
            }
        }

        public void StartSmoke(object obj)
        {
            PictureBox pctr = obj as PictureBox;

            for (int i = 1; i < 7; i++)
            {
                pctr.Image = (Image)imageResources.GetObject($@"newHit{i}");
                Thread.Sleep(50);
            }
        }

        public void WaterSplash(object obj)
        {
            PictureBox pctr = obj as PictureBox;

            for (int i = 1; i < 14; i++)
            {
                pctr.Image = (Image)imageResources.GetObject($@"miss{i}");
                Thread.Sleep(50);
            }
        }
    }
}
