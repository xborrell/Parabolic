using System;
using System.Numerics;
using System.Numerics;

namespace Parabolic
{
    internal class Calculador
    {
        public Calculador() // Calcula la trajectoria d'una roca des d'alçada 0 a alçada 0
        {
        }

        internal void Throw(Roca roca)
        {
            float interval = 10F; // segons
            float temps = interval;

            do
            {
                CalcularNovaPosicio(roca);
                temps -= Constants.FixedDeltaTime;

                if( temps <= 0)
                {
                    temps += interval;
                }

            } while (roca.Posicio.Length() >= Constants.EarthRadius);
        }


        public void CalcularNovaPosicio(Roca roca)
        {
            var fuerzaGravitatoria = CalcularForçaGravitatoria(roca);
            var aceleracionGravitatoria = fuerzaGravitatoria / roca.Masa;

            roca.Velocitat += aceleracionGravitatoria * Constants.FixedDeltaTime;
            var desplaçament = roca.Velocitat * Constants.FixedDeltaTime;
            roca.Posicio += desplaçament;
        }

        Vector3 CalcularForçaGravitatoria(Roca roca)
        {
            var gravitationModulus = CalcularAtraccioTerrestre(roca);
            var gravitationForce = Vector3.Normalize(roca.Posicio);
            gravitationForce = gravitationForce * (gravitationModulus * -1);

            return gravitationForce;
        }

        public float CalcularAtraccioTerrestre(Roca roca)
        {
            var M = Constants.EarthMass;
            var m = roca.Masa;
            var r = roca.Posicio.Length();
            var G = Constants.ConstanteGravitacionUniversal;

            var F = G * M * m / Math.Pow(r, 2);

            return Ecuacion.AtraccioEntreDosMasas(M, m, r);
        }
    }
}