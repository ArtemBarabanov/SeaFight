using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaFightInterface
{
    class RadioPictureBox: PictureBox
    {
        public bool Checked { get; set; }
        public int Decks { get; set; }
    }
}
