using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parabolic.Console
{
    public class Resultat : IComparable<Resultat>
    {
        const int MasaDeLaRoca = 1;
        const int DistanciaACubrir = 1151000; // distancia a Breda en metres
        private static Calculador calculador = new Calculador();

        public double Angle { get; private set; }
        public double VelocitatInicial { get; private set; }
        public double Distancia { get; private set; }
        public int Sentit { get; private set; }

        public Resultat(double angle, double força)
        {
            Angle = angle;
            VelocitatInicial = força;
            CalcularDistancia();
        }

        public int CompareTo(Resultat other)
        {
            var d1 = (int)Math.Round(Distancia, 0);
            var d2 = (int)Math.Round(other.Distancia, 0);

            if( d1 == d2 )
            {
                return VelocitatInicial.CompareTo(other.VelocitatInicial);
            }

            return Distancia.CompareTo(other.Distancia);
        }

        public override string ToString()
        {
            var angleEnGraus = Constants.RadianToDegreeCoeficient * Angle;
            var signe = Sentit > 0 ? '-' : '+';
            return $"Amb angle {angleEnGraus}º i una velocitat inicial de {VelocitatInicial} m/s cau a {signe}{Distancia} metres.";
        }

        private void CalcularDistancia()
        {
            var posicioInicial = new Vector3(0, Constants.EarthRadius, 0);

            var roca = new Roca()
            {
                Posicio = posicioInicial,
                Velocitat = new Vector3(VelocitatInicial * Math.Cos(Angle), VelocitatInicial * Math.Sin(Angle), 0),
                Masa = MasaDeLaRoca
            };

            calculador.Throw(roca);

            var distanciaObtinguda = (int)Math.Round((roca.Posicio - posicioInicial).Length());
            Distancia = Math.Abs(DistanciaACubrir - distanciaObtinguda);
            Sentit = (int)((DistanciaACubrir - distanciaObtinguda) / Distancia);
        }
    }
}
