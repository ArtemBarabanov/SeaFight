using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeaFightInterface
{
    public interface IAnimation
    {
        void Explosion(object pctr);
        void WaterSplash(object pctr);
        void StartSmoke(object pctr);
    }
}
