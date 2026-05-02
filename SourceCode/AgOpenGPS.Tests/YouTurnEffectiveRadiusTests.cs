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
    }
}
