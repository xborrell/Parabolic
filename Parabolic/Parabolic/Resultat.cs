using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Parabolic
{
    public class Resultat : IComparable<Resultat>
    {
        const int MasaDeLaRoca = 1;
        const int DistanciaACubrir = 1151000; // distancia a Breda en metres
        const float offsetAcceptable = 100F;
        private static Calculador calculador = new Calculador();

        public float Angle { get; private set; }
        public float Força { get; private set; }
        public float Distancia { get; private set; }
        public int Sentit { get; private set; }

        public Resultat(float angle, float força)
        {
            Angle = angle;
            Força = força;
            CalcularDistancia();
        }

        public int CompareTo(Resultat other)
        {
            var offset = Math.Abs(Distancia - other.Distancia);
            if (offset >= offsetAcceptable)
            {
                return Math.Abs(Distancia).CompareTo(Math.Abs(other.Distancia));
            }

            return Força.CompareTo(other.Força) * -1;
        }

        public override string ToString()
        {
            var angleEnGraus = Constantes.RadianToDegreeCoeficient * Angle;
            return $"Amb angle {angleEnGraus}º i una velocitat inicial de {Força} m/s cau a {Distancia} metres.";
        }

        private void CalcularDistancia()
        {
            var posicioInicial = new Vector3(0, Constantes.EarthRadius, 0);

            var roca = new Roca()
            {
                Posicio = posicioInicial,
                Velocitat = new Vector3(Força * (float)Math.Cos(Angle), Força * (float)Math.Sin(Angle), 0),
                Masa = MasaDeLaRoca
            };

            calculador.Throw(roca);

            var distanciaObtinguda = (int)Math.Round((roca.Posicio - posicioInicial).Length());
            Distancia = Math.Abs(DistanciaACubrir - distanciaObtinguda);
            Sentit = (int)(( DistanciaACubrir - distanciaObtinguda) / Distancia);
        }
    }
}
