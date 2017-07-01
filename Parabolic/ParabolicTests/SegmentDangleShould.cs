using FluentAssertions;
using Parabolic;
using System;
using System.Collections.Generic;
using Xunit;

namespace ParabolicTests
{
    public class SegmentDangleShould
    {
        [Theory]
        [InlineData(0F, 90F, 10F)]
        [InlineData(45F, 54F, 1F)]
        [InlineData(49.5F, 50.4F, 0.1F)]
        [InlineData(49.95F, 50.04F, 0.01F)]
        [InlineData(49.995F, 50.004F, 0.001F)]
        [InlineData(49.9995F, 50.0004F, 0.0001F)]
        public void ServeTheRightAnglesInRadiansWithTheServedSegmentAndIntervalWhen10StepsAreRequested(float inicial, float final, float increment)
        {
            // assign
            var segment = SegmentDangles.CreaEnGraus(inicial, final, 10);
            var expectedList = new List<float>();

            for (var angle = inicial; angle <= final; angle += increment)
            {
                expectedList.Add(angle * Constants.DegreeToRadianCoeficient);
            }

            //action
            var obtainedList = new List<float>();
            foreach (var angle in segment.EnRadians())
            {
                obtainedList.Add(angle);
            }

            //assert
            obtainedList.Should().Equal(expectedList, (left, right) => left.AreEqualApproximately(right, 0.01F));
        }

        [Theory]
        [InlineData(0F, 90F, 10F)]
        [InlineData(45F, 54F, 1F)]
        [InlineData(49.5F, 50.4F, 0.1F)]
        [InlineData(49.95F, 50.04F, 0.01F)]
        [InlineData(49.995F, 50.004F, 0.001F)]
        [InlineData(49.9995F, 50.0004F, 0.0001F)]
        public void ServeTheRightAnglesInDegreesWithTheServedSegmentAndIntervalWhen10StepsAreRequested(float inicial, float final, float increment)
        {
            // assign
            var segment = SegmentDangles.CreaEnGraus(inicial, final, 10);
            var expectedList = new List<float>();

            for (var angle = inicial; angle <= final; angle += increment)
            {
                expectedList.Add(angle);
            }

            //action
            var obtainedList = new List<float>();
            foreach (var angle in segment.EnGrausAmbPasos(10))
            {
                obtainedList.Add(angle);
            }

            //assert
            obtainedList.Should().Equal(expectedList, (left, right) => left.AreEqualApproximately(right, 0.01F));
        }

        [Fact]
        public void ThrowExceptionWhenTheZoomPointIsLowerThanSegment()
        {
            // assign
            var segment = SegmentDangles.CreaEnGraus(0, 90, 10);

            //action
            Action zoom = () =>
            {
                segment.ZoomEnRadians(-1 * Constants.DegreeToRadianCoeficient);
            };

            //assert
            zoom.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void ThrowExceptionWhenTheZoomPointIsGreaterThanSegment()
        {
            // assign
            var segment = SegmentDangles.CreaEnGraus(0, 90, 10);

            //action
            Action zoom = () =>
            {
                segment.ZoomEnRadians(91 * Constants.DegreeToRadianCoeficient);
            };

            //assert
            zoom.ShouldThrow<ArgumentException>();
        }


        //[InlineData(0F, 90F, 10F)]
        //[InlineData(45F, 54F, 1F)]
        //[InlineData(49.5F, 50.4F, 0.1F)]
        //[InlineData(49.95F, 50.04F, 0.01F)]
        //[InlineData(49.995F, 50.004F, 0.001F)]
        //[InlineData(49.9995F, 50.0004F, 0.0001F)]


        [Theory]
        [InlineData(0F, 90F, 50F, 45F, 54F)]
        [InlineData(45F, 54F, 50F, 49.5F, 50.4F)]
        [InlineData(49.5F, 50.4F, 50F, 49.95F, 50.04F)]
        [InlineData(49.95F, 50.04F, 50F, 49.995F, 50.004F)]
        [InlineData(49.995F, 50.004F, 50F, 49.9995F, 50.0004F)]
        [InlineData(49.9995F, 50.0004F, 50F, 49.99995F, 50.00004F)]
        public void ReturnANewSegmentWithTheZoomAndIntervalRequested(float inicial, float final, float puntDeZoom, float angleMinimPrevist, float angleMaximPrevist)
        {
            // assign
            const int passos = 10;
            var segment = SegmentDangles.CreaEnGraus(inicial, final, passos);
            var expectedSegment = SegmentDangles.CreaEnGraus(angleMinimPrevist, angleMaximPrevist, passos / 10);

            //action
            var obtainedSegment = segment.ZoomEnRadians(puntDeZoom * Constants.DegreeToRadianCoeficient);

            //assert
            obtainedSegment.Should().Be(expectedSegment);
        }

        //[Theory]
        //[InlineData(45F, 35F, 55F)]
        //public void ReturnANewSegmentWithTheZoomRequestedAndAAuthomaticInterval(float puntDeZoom, float angleMinimPrevist, float angleMaximPrevist)
        //{
        //    // assign
        //    var segment = SegmentDangles.CreaEnGraus(0, 90, 10);
        //    var expectedSegment = SegmentDangles.CreaEnGraus(angleMinimPrevist, angleMaximPrevist, 10);

        //    //action
        //    var obtainedSegment = segment.ZoomEnRadians(puntDeZoom * Constants.DegreeToRadianCoeficient);

        //    //assert
        //    obtainedSegment.Should().Be(expectedSegment);
        //}
    }
}
