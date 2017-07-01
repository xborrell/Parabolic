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
        public float AngleInicialEnGraus { get { return AngleInicialEnRadians * Constants.RadianToDegreeCoeficient; } }
        public float AngleFinalEnGraus { get { return AngleFinalEnRadians * Constants.RadianToDegreeCoeficient; } }
        public float AngleInicialEnRadians { get; protected set; }
        public float AngleFinalEnRadians { get; protected set; }
        public int Passos { get; protected set; }

        public static SegmentDangles CreaEnRadians(float angleInicial, float angleFinal, int passos)
        {
            return new SegmentDangles(angleInicial, angleFinal, passos);
        }

        public static SegmentDangles CreaEnGraus(float angleInicial, float angleFinal, int passos)
        {
            return CreaEnRadians(
                Constants.DegreeToRadianCoeficient * angleInicial,
                Constants.DegreeToRadianCoeficient * angleFinal,
                passos
            );
        }

        public SegmentDangles(float angleInicial, float angleFinal, int passos)
        {
            AngleInicialEnRadians = Math.Min(angleInicial, angleFinal);
            AngleFinalEnRadians = Math.Max(angleInicial, angleFinal);
            Passos = passos;
        }

        public IEnumerable<float> EnRadians()
        {
            return AmbPasos(AngleInicialEnRadians, AngleFinalEnRadians);
        }

        public IEnumerable<float> EnGrausAmbPasos(int pasos)
        {
            return AmbPasos(AngleInicialEnGraus, AngleFinalEnGraus);
        }

        public SegmentDangles ZoomEnRadians(float puntDeZoom)
        {
            if (puntDeZoom < AngleInicialEnRadians) throw new ArgumentException("El punt de zoom no pot ser inferior al angle inicial.");
            if (puntDeZoom > AngleFinalEnRadians) throw new ArgumentException("El punt de zoom no pot ser superior al angle final.");

            var passosNous = Passos / 10;
            float increment = (AngleInicialEnRadians - AngleFinalEnRadians) / (Passos - 1);
            float nouIncrement = increment / 10;

            float iniciPrevist = puntDeZoom - (nouIncrement * 4);
            float fiPrevist = iniciPrevist + increment - nouIncrement;

            return SegmentDangles.CreaEnRadians(iniciPrevist, fiPrevist, passosNous);
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

            return AngleInicialEnRadians.AreEqualApproximately(other.AngleInicialEnRadians, 0.0001F)
                && AngleFinalEnRadians.AreEqualApproximately(other.AngleFinalEnRadians, 0.0001F)
                ;
        }

        private IEnumerable<float> AmbPasos(float inici, float final)
        {
            float angle = inici;
            float increment = (final - inici) / (Passos - 1);

            while (angle <= final)
            {
                yield return angle;

                angle += increment;
            }

            if( angle - (increment / 100) <= final)
            {
                yield return angle;
            }
        }
    }
}
