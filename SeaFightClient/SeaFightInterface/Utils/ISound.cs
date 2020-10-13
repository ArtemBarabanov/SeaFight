using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFightInterface
{
    public interface ISound
    {
        void Explosion();
        void SeaAndGulls();
        void StopSeaAndGulls();
        void Shot();
        void Pluk();
        void Rynda();
    }
}
