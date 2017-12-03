using System;
using NUnit.Framework;

namespace StringExtension.Tests
{
    [TestFixture]
    public class StringExtensionTest
    {
        [Test]
        public void LenB()
        {
            Assert.AreEqual(0, "".LenB()); // 空文字
            Assert.AreEqual(4, "\t\r\n ".LenB()); // エスケープ文字
            Assert.AreEqual(24, "科学の　力って　すげー！".LenB()); // 全角のみ
            Assert.AreEqual(19, "Science is amazing!".LenB()); // 半角のみ
            Assert.AreEqual(21, "Science　is　amazing!".LenB()); // 全角半角混在

            // null値
            Assert.Throws<ArgumentNullException>(() =>
            {
                string nullText = null;
                nullText.LenB();
            });
        }
    }
}
