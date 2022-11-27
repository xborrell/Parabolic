
namespace Parabolic.Console
{
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections;
    using Parabolic.Tools;

    public class SegmentDangles
    {
        public const int Passos = 10;
        public readonly double intervalMinim = 1E-3 * Constants.DegreeToRadianCoeficient;

        public double Interval { get { return (AngleFinal - AngleInicial) / (Passos - 1); } }

        public double AngleInicial { get; protected set; }
        public double AngleFinal { get; protected set; }

        public static SegmentDangles CreaEnRadians(double angleInicial, double angleFinal)
        {
            return new SegmentDangles(angleInicial, angleFinal);
        }

        public static SegmentDangles CreaEnGraus(double angleInicial, double angleFinal)
        {
            return CreaEnRadians(
                Constants.DegreeToRadianCoeficient * angleInicial,
                Constants.DegreeToRadianCoeficient * angleFinal
            );
        }

        public SegmentDangles(double angleInicial, double angleFinal)
        {
            AngleInicial = Math.Min(angleInicial, angleFinal);
            AngleFinal = Math.Max(angleInicial, angleFinal);
        }

        public IEnumerable<double> EnRadians()
        {
            double angle = AngleInicial;

            while (angle <= AngleFinal)
            {
                yield return angle;

                angle += Interval;
            }

            if (angle - (Interval / 100) <= AngleFinal)
            {
                yield return angle;
            }
        }

        public bool CanZoom()
        {
            return (Interval >= intervalMinim);
        }

        public SegmentDangles Zoom(double puntDeZoomEnRadians)
        {
            if (puntDeZoomEnRadians < AngleInicial) throw new ArgumentException("El punt de zoom no pot ser inferior al angle inicial.");
            if (puntDeZoomEnRadians > AngleFinal) throw new ArgumentException("El punt de zoom no pot ser superior al angle final.");

            if (!CanZoom()) throw new ArgumentException("No es soporta zoom per sota de milesimas de grau.");
            var epsilon = Interval / 100;

            var valorsActuals = new List<double>();
            var indexDelZoom = -1;
            var diferenciaDeZoom = double.MaxValue;
            foreach (var valor in EnRadians())
            {
                var diferencia = Math.Abs(puntDeZoomEnRadians - valor);
                if ( diferenciaDeZoom > diferencia)
                {
                    indexDelZoom = valorsActuals.Count;
                    diferenciaDeZoom = diferencia;
                }

                valorsActuals.Add(valor);
            }

            if (indexDelZoom <= 1) indexDelZoom = 2;
            if (indexDelZoom >= valorsActuals.Count - 2) indexDelZoom = valorsActuals.Count - 3;

            double iniciPrevist = valorsActuals[indexDelZoom - 2];
            double fiPrevist = valorsActuals[indexDelZoom + 2];

            return SegmentDangles.CreaEnRadians(iniciPrevist, fiPrevist);
        }

        public override bool Equals(object obj)
        {
            var other = obj as SegmentDangles;

            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            var epsilon = Interval / 100;

            return AngleInicial.AreEqualApproximately(other.AngleInicial, epsilon)
                && AngleFinal.AreEqualApproximately(other.AngleFinal, epsilon)
                ;
        }

        public override string ToString()
        {
            return $"From {AngleInicial * Constants.RadianToDegreeCoeficient} to {AngleFinal * Constants.RadianToDegreeCoeficient} ({Interval * Constants.RadianToDegreeCoeficient})";
        }
    }
}
