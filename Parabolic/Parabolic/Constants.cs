using System;

namespace Parabolic
{
    public static class Constants
    {
        public const float EarthRadius = 6378280F;    // Radio de la tierra en metros
        public const float EarthMass = 5.974E24F;     // masa de la tierra en kilos
        public const float FixedDeltaTime = 0.1F;     // segundos
        public const float Mu = 398600F;              // Masa de la tierra * constante de gravitacion universal en Km3 / segundo2
        public const float ConstanteGravitacionUniversal = 6.674E-11F; //

        public static float RadianToDegreeCoeficient { get { return 180 / (float)Math.PI; } }
        public static float DegreeToRadianCoeficient { get { return (float)Math.PI / 180; } }
    }
}
