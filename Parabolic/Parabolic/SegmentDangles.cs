using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Parabolic
{
    public class SegmentDangles
    {
        public const int Passos = 10;
        public readonly float intervalMinim = 1E-3F * Constants.DegreeToRadianCoeficient;

        public float Interval { get { return (AngleFinal - AngleInicial) / (Passos - 1); } }

        public float AngleInicial { get; protected set; }
        public float AngleFinal { get; protected set; }

        public static SegmentDangles CreaEnRadians(float angleInicial, float angleFinal)
        {
            return new SegmentDangles(angleInicial, angleFinal);
        }

        public static SegmentDangles CreaEnGraus(float angleInicial, float angleFinal)
        {
            return CreaEnRadians(
                Constants.DegreeToRadianCoeficient * angleInicial,
                Constants.DegreeToRadianCoeficient * angleFinal
            );
        }

        public SegmentDangles(float angleInicial, float angleFinal)
        {
            AngleInicial = Math.Min(angleInicial, angleFinal);
            AngleFinal = Math.Max(angleInicial, angleFinal);
        }

        public IEnumerable<float> EnRadians()
        {
            float angle = AngleInicial;

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

        public SegmentDangles Zoom(float puntDeZoomEnRadians)
        {
            if (puntDeZoomEnRadians < AngleInicial) throw new ArgumentException("El punt de zoom no pot ser inferior al angle inicial.");
            if (puntDeZoomEnRadians > AngleFinal) throw new ArgumentException("El punt de zoom no pot ser superior al angle final.");

            if (!CanZoom()) throw new ArgumentException("No es soporta zoom per sota de milesimas de grau.");
            var epsilon = Interval / 100;

            var valorsActuals = new List<float>();
            var indexDelZoom = -1;
            var diferenciaDeZoom = float.MaxValue;
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

            if (indexDelZoom == 0) indexDelZoom++;
            if (indexDelZoom == valorsActuals.Count - 1) indexDelZoom--;

            float iniciPrevist = valorsActuals[indexDelZoom - 1];
            float fiPrevist = valorsActuals[indexDelZoom + 1];

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

            return AngleInicial.AreEqualApproximately(other.AngleInicial, 0.0001F)
                && AngleFinal.AreEqualApproximately(other.AngleFinal, 0.0001F)
                ;
        }
    }
}
