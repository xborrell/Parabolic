using System;
using System.Numerics;

namespace Parabolic
{
    public static class Ecuacion
    {
        public static float AtraccioEntreDosMasas(float masa1EnKilos, float masa2EnKilos, float distanciaEnMetros)
        {
            var n = Constants.ConstanteGravitacionUniversal * masa1EnKilos * masa2EnKilos;
            var d = (float)Math.Pow(distanciaEnMetros, 2);
            return n / d;
        }
    }
}