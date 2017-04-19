using System;
using NUnit.Framework;
using VrPlayer.Helpers;

namespace VrPlayer.Tests
{
    [TestFixture]
    public class QuaternionHelperTests
    {
        [Test]
        public void VerifyQuaternionToEulerIntegrity()
        {
            var q1 = QuaternionHelper.EulerAnglesInDegToQuaternion(30, 10, 25);
            var e = QuaternionHelper.QuaternionToEulerAnglesInDeg(q1);
            var q2 = QuaternionHelper.EulerAnglesInDegToQuaternion(e.Y, e.X, e.Z);

            Assert.That(q1, Is.EqualTo(q2));
        }

        [Test]
        public void AngleIntegrity()
        {
            var q = QuaternionHelper.EulerAnglesInDegToQuaternion(30, 0, 0);
            var angles = QuaternionHelper.QuaternionToEulerAnglesInDeg(q);
            Assert.That(angles.X, Is.EqualTo(30));
        }
        
        [Test]
        public void NormalizeAngleLessThanZero()
        {
            Assert.That(QuaternionHelper.NormaliseAngle(-10), Is.EqualTo(350));
        }
    }
}