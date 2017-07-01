using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Parabolic
{
    public class Buscador
    {
        const int DistanciaACubrir = 1151000; // distancia a Breda en metres

        public float angle { get; private set; } = Constantes.DegreeToRadianCoeficient * 45; // en radians
        public float força { get; private set; } = 0; // modulo del vector velocidad
        private Calculador calculador = new Calculador();

        public void Executar()
        {
            AproximaUnaForça();
        }

        private void AproximaUnaForça()
        {
            var incrementDeForça = 1000F;
            var ultimaForça = 0F;
            var ultimResultat = new Resultat(angle, ultimaForça);

            while (ultimResultat.Distancia > 1000)
            {
                ultimaForça += incrementDeForça;

                var resultatObtingut = new Resultat(angle, ultimaForça);

                Console.WriteLine(resultatObtingut.ToString());

                if (resultatObtingut.CompareTo( ultimResultat ) > 0)
                {
                    incrementDeForça *= -0.5F;
                }
                else
                {
                    força = ultimaForça;
                }
                ultimResultat = resultatObtingut;
            }
        }
    }
}
