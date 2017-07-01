using FluentAssertions;
using Parabolic;
using System;
using Xunit;

namespace ParabolicTests
{
    public class EcuacionesTest
    {
        [Fact]
        public void TestMethod1()
        {
            // assign
            var masa1 = 5.5F;
            var masa2 = 5.5F;
            var distancia = 3.2F;
            var expectedF = 1.97E-10F;

            //action
            var F = Ecuacion.AtraccioEntreDosMasas(masa1, masa2, distancia);

            //assert
            F.Should().BeApproximately(expectedF, 1E-12F);
        }
    }
}
