using System;

namespace Parabolic.Console
{
    internal class Calculador
    {
        public Calculador() // Calcula la trajectoria d'una roca des d'alçada 0 a alçada 0
        {
        }

        internal void Throw(Roca roca)
        {
            int iteracions = 10000;
            var posicioInicial = roca.Posicio;
            var velocitatInicial = roca.Velocitat;

            do
            {
                CalcularNovaPosicio(roca);

                if( iteracions-- < 0)
                {
                    roca.Posicio = new Vector3(double.MaxValue, double.MaxValue, double.MaxValue);
                    return;
                }


            } while (roca.Posicio.Length() >= Constants.EarthRadius);
        }


        public void CalcularNovaPosicio(Roca roca)
        {
            var fuerzaGravitatoria = CalcularForçaGravitatoria(roca);
            var aceleracionGravitatoria = fuerzaGravitatoria / roca.Masa;

            roca.Velocitat = roca.Velocitat + (aceleracionGravitatoria * Constants.FixedDeltaTime);
            var desplaçament = roca.Velocitat * Constants.FixedDeltaTime;
            roca.Posicio += desplaçament;
        }

        Vector3 CalcularForçaGravitatoria(Roca roca)
        {
            var gravitationModulus = CalcularAtraccioTerrestre(roca);
            var gravitationForce = roca.Posicio.Normalize();
            gravitationForce = gravitationForce * (gravitationModulus * -1);

            return gravitationForce;
        }

        public double CalcularAtraccioTerrestre(Roca roca)
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