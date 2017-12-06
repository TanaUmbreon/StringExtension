﻿using System;
using NUnit.Framework;

namespace StringExtension.Tests
{
    [TestFixture]
    public class StringExtensionTest
    {
        [Test]
        public void LenB()
        {
            // 例外が発生するパターン
            Assert.Throws<ArgumentNullException>(() =>
            {
                string nullText = null;
                nullText.LenB();
            });

            Assert.AreEqual(0, "".LenB()); // 空文字
            Assert.AreEqual(4, "\t\r\n ".LenB()); // エスケープ文字
            Assert.AreEqual(24, "科学の　力って　すげー！".LenB()); // 全角のみ
            Assert.AreEqual(19, "Science is amazing!".LenB()); // 半角のみ
            Assert.AreEqual(21, "Science　is　amazing!".LenB()); // 全角半角混在
        }

        /// <summary>
        /// MidB(string, int, int)
        /// </summary>
        [Test]
        public void MidB1()
        {
            // 例外が発生するパターン
            Assert.Throws<ArgumentNullException>(() =>
            {
                string nullText = null;
                nullText.MidB(0, 0);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => { "".MidB(-1, 0); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { "".MidB(0, -1); });

            // 長さがゼロ
            Assert.AreEqual("", "".MidB(0, 0));
            Assert.AreEqual("", "Pikachu".MidB(3, 0));
            Assert.AreEqual("", "ピカチュウ".MidB(4, 0));

            // 開始位置がオーバー
            Assert.AreEqual("", "".MidB(1, 2));
            Assert.AreEqual("", "Pikachu".MidB(7, 1));
            Assert.AreEqual("", "ピカチュウ".MidB(10, 2));

            // 長さがオーバー
            Assert.AreEqual("", "".MidB(0, 2));
            Assert.AreEqual("hu", "Pikachu".MidB(5, 3));
            Assert.AreEqual("ウ", "ピカチュウ".MidB(8, 3));

            Assert.AreEqual("Pika", "Pikachu".MidB(0, 4));
            Assert.AreEqual("チュウ", "ピカチュウ".MidB(4, 6));
            Assert.AreEqual("ピ ", "ピカチュウ".MidB(0, 3)); // 全角文字の途中(末尾)
            Assert.AreEqual("sカ", "ピカチュウ".MidB(1, 3)); // 全角文字の途中(先頭)
        }

        /// <summary>
        /// MidB(string, int)
        /// </summary>
        [Test]
        public void MidB2()
        {
            // 例外が発生するパターン
            Assert.Throws<ArgumentNullException>(() =>
            {
                string nullText = null;
                nullText.MidB(0);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => { "".MidB(-1); });

            // 開始位置がオーバー
            Assert.AreEqual("", "".MidB(1));
            Assert.AreEqual("", "Pikachu".MidB(7));
            Assert.AreEqual("", "ピカチュウ".MidB(10));

            Assert.AreEqual("Pikachu", "Pikachu".MidB(0));
            Assert.AreEqual("チュウ", "ピカチュウ".MidB(4));
            Assert.AreEqual("sカチュウ", "ピカチュウ".MidB(1)); // 全角文字の途中(先頭)
        }

        [Test]
        public void LeftB()
        {
            // 例外が発生するパターン
            Assert.Throws<ArgumentNullException>(() =>
            {
                string nullText = null;
                nullText.LeftB(0);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => { "".LeftB(-1); });

            // 長さがオーバー
            Assert.AreEqual("", "".LeftB(1));
            Assert.AreEqual("Poké Ball", "Poké Ball".LeftB(10)); // 長さオーバーは元の文字列を返すので"é"のまま
            Assert.AreEqual("モンスターボール", "モンスターボール".LeftB(17));

            Assert.AreEqual("Poke B", "Poké Ball".LeftB(6)); // "é"は"e"に化けてしまう？
            Assert.AreEqual("モンス", "モンスターボール".LeftB(6));
            Assert.AreEqual("モンス ", "モンスターボール".LeftB(7)); // 全角文字の途中
        }
    }
}
