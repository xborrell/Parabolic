using System;

namespace Parabolic.Console
{
    public static class Ecuacion
    {
        public static double AtraccioEntreDosMasas(double masa1EnKilos, double masa2EnKilos, double distanciaEnMetros)
        {
            var n = Constants.ConstanteGravitacionUniversal * masa1EnKilos * masa2EnKilos;
            var d = Math.Pow(distanciaEnMetros, 2);
            return n / d;
        }
    }
}