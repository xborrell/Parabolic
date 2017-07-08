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
            var segment = SegmentDangles.CreaEnGraus(inicial, final);
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

        [Fact]
        public void ThrowExceptionWhenTheZoomPointIsLowerThanSegment()
        {
            // assign
            var segment = SegmentDangles.CreaEnGraus(0, 90);

            //action
            Action zoom = () =>
            {
                segment.Zoom(-1 * Constants.DegreeToRadianCoeficient);
            };

            //assert
            zoom.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void ThrowExceptionWhenTheZoomPointIsGreaterThanSegment()
        {
            // assign
            var segment = SegmentDangles.CreaEnGraus(0, 90);

            //action
            Action zoom = () =>
            {
                segment.Zoom(91 * Constants.DegreeToRadianCoeficient);
            };

            //assert
            zoom.ShouldThrow<ArgumentException>();
        }

        [Theory]
        [InlineData(0F, 90F, 50F, 40F, 60F)]
        [InlineData(45F, 54F, 50F, 49F, 51F)]
        [InlineData(49.5F, 50.4F, 50F, 49.9F, 50.1F)]
        [InlineData(49.95F, 50.04F, 50F, 49.99F, 50.01F)]
        [InlineData(49.995F, 50.004F, 50F, 49.999F, 50.001F)]
        [InlineData(0F, 90F, 0F, 0F, 20F)]
        [InlineData(0F, 90F, 90F, 70F, 90F)]
        public void ReturnANewSegmentWithTheZoomRequestedInRadians(float inicial, float final, float puntDeZoom, float angleMinimPrevist, float angleMaximPrevist)
        {
            // assign
            var segment = SegmentDangles.CreaEnGraus(inicial, final);
            var expectedSegment = SegmentDangles.CreaEnGraus(angleMinimPrevist, angleMaximPrevist);

            //action
            var obtainedSegment = segment.Zoom(puntDeZoom * Constants.DegreeToRadianCoeficient);

            //assert
            obtainedSegment.Should().Be(expectedSegment);
        }

        [Theory]
        [InlineData(49.9995F, 50.0004F, 50F)]
        public void ThrowExceptionWhenTheZoomIsTooLittle(float inicial, float final, float puntDeZoom)
        {
            // assign
            var segment = SegmentDangles.CreaEnGraus(inicial, final);

            //action
            Action zoom = () =>
            {
                segment.Zoom(puntDeZoom * Constants.DegreeToRadianCoeficient);
            };

            //assert
            zoom.ShouldThrow<ArgumentException>();
        }
    }
}
