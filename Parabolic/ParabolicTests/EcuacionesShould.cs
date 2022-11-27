using FluentAssertions;
using Parabolic;
using System;
using Xunit;
using Parabolic.Console;

namespace ParabolicTests
{
    public class EcuacionesShould
    {
        [Theory]
        [InlineData(5.5F, 5.5F, 3.2F, 1.97E-10F)]
        public void CalculateTheRightForceBetweenTwoMasses(float masa1, float masa2, float distancia, float forçaEsperada)
        {
            //action
            var forçaObtinguda = Ecuacion.AtraccioEntreDosMasas(masa1, masa2, distancia);

            //assert
            forçaObtinguda.Should().BeApproximately(forçaEsperada, 1E-12F);
        }
    }
}
