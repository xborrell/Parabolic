using System;

namespace Parabolic.Console
{
    public static class Constants
    {
        public const double EarthRadius = 6378280;    // Radio de la tierra en metros
        public const double EarthMass = 5.974E24;     // masa de la tierra en kilos
        public const double FixedDeltaTime = 0.1;     // segundos
        public const double Mu = 398600;              // Masa de la tierra * constante de gravitacion universal en Km3 / segundo2
        public const double ConstanteGravitacionUniversal = 6.674E-11;
        public const double incrementMinimDeVelocitat = 1E-6;

        public static double RadianToDegreeCoeficient { get { return 180 / Math.PI; } }
        public static double DegreeToRadianCoeficient { get { return Math.PI / 180; } }
    }
}
