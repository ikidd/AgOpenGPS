using NUnit.Framework;

namespace AgOpenGPS.Tests.Models
{
    public class YouTurnEffectiveRadiusTests
    {
        [Test]
        public void EffectiveTurnRadiusUsesBaseRadiusWhenToolMinimumIsZero()
        {
            Assert.That(CYouTurn.GetEffectiveTurnRadius(8.1, 0.0), Is.EqualTo(8.1));
        }

        [Test]
        public void EffectiveTurnRadiusUsesBaseRadiusWhenToolMinimumIsSmaller()
        {
            Assert.That(CYouTurn.GetEffectiveTurnRadius(8.1, 6.0), Is.EqualTo(8.1));
        }

        [Test]
        public void EffectiveTurnRadiusUsesToolMinimumWhenItIsLarger()
        {
            Assert.That(CYouTurn.GetEffectiveTurnRadius(8.1, 12.5), Is.EqualTo(12.5));
        }

        [Test]
        public void LineAcquisitionSteerLimitKeepsVehicleLimitWhenDisabled()
        {
            Assert.That(
                CYouTurn.GetLineAcquisitionSteerLimit(45.0, 3.0, 12.0, false),
                Is.EqualTo(45.0));
        }

        [Test]
        public void LineAcquisitionSteerLimitUsesEffectiveRadiusWhenEnabled()
        {
            double limit = CYouTurn.GetLineAcquisitionSteerLimit(45.0, 3.0, 12.0, true);

            Assert.That(limit, Is.EqualTo(14.036243467926479).Within(0.000001));
        }

        [Test]
        public void LineAcquisitionSteerLimitNeverExceedsVehicleLimit()
        {
            Assert.That(
                CYouTurn.GetLineAcquisitionSteerLimit(25.0, 3.0, 3.0, true),
                Is.EqualTo(25.0));
        }

        [Test]
        public void LineAcquisitionClampPreservesSignWhenLimited()
        {
            Assert.That(
                CYouTurn.ClampLineAcquisitionSteerAngle(-40.0, 45.0, 3.0, 12.0, true),
                Is.EqualTo(-14.036243467926479).Within(0.000001));
        }
    }
}
