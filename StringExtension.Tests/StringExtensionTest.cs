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

            Assert.AreEqual("Poke B", "Poké Ball".LeftB(6)); // "é"は"e"に化けてしまう(Shift-JISにéがないため?)
            Assert.AreEqual("モンス", "モンスターボール".LeftB(6));
            Assert.AreEqual("モンス ", "モンスターボール".LeftB(7)); // 全角文字の途中
        }

        [Test]
        public void RightB()
        {
            // 例外が発生するパターン
            Assert.Throws<ArgumentNullException>(() =>
            {
                string nullText = null;
                nullText.RightB(0);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => { "".RightB(-1); });

            // 長さがゼロ
            Assert.AreEqual("", "".RightB(0));
            Assert.AreEqual("", "Poké Ball".RightB(0));
            Assert.AreEqual("", "モンスターボール".RightB(0));

            // 長さがオーバー
            Assert.AreEqual("", "".RightB(1));
            Assert.AreEqual("Poké Ball", "Poké Ball".RightB(10));
            Assert.AreEqual("モンスターボール", "モンスターボール".RightB(17));

            // 文字の範囲内で取得
            Assert.AreEqual("e Ball", "Poké Ball".RightB(6));
            Assert.AreEqual("ボール", "モンスターボール".RightB(6));
            Assert.AreEqual("[ボール", "モンスターボール".RightB(7)); // 全角文字の途中
        }

        /// <summary>
        /// PadLeftB(string, int, char)
        /// </summary>
        [Test]
        public void PadLeftB1()
        {
            // 例外パターン
            Assert.Throws<ArgumentNullException>(() =>
            {
                string nullText = null;
                nullText.PadLeftB(0, ' ');
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => { "".PadLeftB(-1, '0'); });

            // 元の文字列以下の長さ
            Assert.AreEqual("", "".PadLeftB(0, '0'));
            Assert.AreEqual("55", "55".PadLeftB(2, '0'));
            Assert.AreEqual("５５", "５５".PadLeftB(4, '0'));

            // 元の文字列より大きい長さ
            Assert.AreEqual("00000", "0".PadLeftB(5, '0'));
            Assert.AreEqual("00050", "50".PadLeftB(5, '0'));
            Assert.AreEqual("0４５", "４５".PadLeftB(5, '0'));
            Assert.AreEqual("００６５", "６５".PadLeftB(8, '０')); // 全角でパディング
            Assert.AreEqual(" ０５５", "５５".PadLeftB(7, '０')); // 全角の途中
        }

        /// <summary>
        /// PadLeftB(string, int)
        /// </summary>
        [Test]
        public void PadLeftB2()
        {
            // 例外パターン
            Assert.Throws<ArgumentNullException>(() =>
            {
                string nullText = null;
                nullText.PadLeftB(0);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => { "".PadLeftB(-1); });

            // 元の文字列以下の長さ
            Assert.AreEqual("", "".PadLeftB(0));
            Assert.AreEqual("55", "55".PadLeftB(2));
            Assert.AreEqual("５５", "５５".PadLeftB(4));

            // 元の文字列より大きい長さ
            Assert.AreEqual("    0", "0".PadLeftB(5));
            Assert.AreEqual("   50", "50".PadLeftB(5));
            Assert.AreEqual(" ４５", "４５".PadLeftB(5));
            Assert.AreEqual("    ６５", "６５".PadLeftB(8));
            Assert.AreEqual("   ５５", "５５".PadLeftB(7));
        }

        /// <summary>
        /// PadRightB(string, int, char)
        /// </summary>
        [Test]
        public void PadRightB1()
        {
            // 例外パターン
            Assert.Throws<ArgumentNullException>(() =>
            {
                string nullText = null;
                nullText.PadRightB(0, ' ');
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => { "".PadRightB(-1, '0'); });

            // 元の文字列以下の長さ
            Assert.AreEqual("", "".PadRightB(0, '0'));
            Assert.AreEqual("55", "55".PadRightB(2, '0'));
            Assert.AreEqual("５５", "５５".PadRightB(4, '0'));

            // 元の文字列より大きい長さ
            Assert.AreEqual("00000", "0".PadRightB(5, '0'));
            Assert.AreEqual("50000", "50".PadRightB(5, '0'));
            Assert.AreEqual("４５0", "４５".PadRightB(5, '0'));
            Assert.AreEqual("６５００", "６５".PadRightB(8, '０')); // 全角でパディング
            Assert.AreEqual("５５０ ", "５５".PadRightB(7, '０')); // 全角の途中
        }

        /// <summary>
        /// PadRightB(string, int)
        /// </summary>
        [Test]
        public void PadRightB2()
        {
            // 例外パターン
            Assert.Throws<ArgumentNullException>(() =>
            {
                string nullText = null;
                nullText.PadRightB(0);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => { "".PadRightB(-1); });

            // 元の文字列以下の長さ
            Assert.AreEqual("", "".PadRightB(0));
            Assert.AreEqual("55", "55".PadRightB(2));
            Assert.AreEqual("５５", "５５".PadRightB(4));

            // 元の文字列より大きい長さ
            Assert.AreEqual("0    ", "0".PadRightB(5));
            Assert.AreEqual("50   ", "50".PadRightB(5));
            Assert.AreEqual("４５ ", "４５".PadRightB(5));
            Assert.AreEqual("６５    ", "６５".PadRightB(8));
            Assert.AreEqual("５５   ", "５５".PadRightB(7));
        }

        /// <summary>
        /// FixLeftB(string, int, char)
        /// </summary>
        [Test]
        public void FixLeftB1()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                string nullText = null;
                nullText.FixLeftB(0, ' ');
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => { "".FixLeftB(-1, '0'); });

            // 元の文字列と同じ長さだとそのまま
            Assert.AreEqual("", "".FixLeftB(0, ' '));
            Assert.AreEqual("Mudkip is so CUUUUTE!", "Mudkip is so CUUUUTE!".FixLeftB(21, ' '));
            Assert.AreEqual("ミズゴロウかわえぇぇ！", "ミズゴロウかわえぇぇ！".FixLeftB(22, ' '));

            // 元の文字列未満の長さ
            Assert.AreEqual("ark?", "Bwark?".FixLeftB(4, ' '));
            Assert.AreEqual("しゃま？", "あしゃま？".FixLeftB(8, ' '));
            Assert.AreEqual("ｵゃま？", "あしゃま？".FixLeftB(7, ' ')); // 全角の途中

            // 元の文字列より大きい長さ
            Assert.AreEqual("YaaaHoo!", "Y" + "Hoo!".FixLeftB(7, 'a'));
            Assert.AreEqual("もももふう！", "もふう！".FixLeftB(12, 'も'));
            Assert.AreEqual(" ももふう！", "もふう！".FixLeftB(11, 'も')); // 全角の途中
        }

        /// <summary>
        /// FixLeftB(string, int)
        /// </summary>
        [Test]
        public void FixLeftB2()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                string nullText = null;
                nullText.FixLeftB(0);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => { "".FixLeftB(-1); });

            // 元の文字列と同じ長さだとそのまま
            Assert.AreEqual("", "".FixLeftB(0));
            Assert.AreEqual("Mudkip is so CUUUUTE!", "Mudkip is so CUUUUTE!".FixLeftB(21));
            Assert.AreEqual("ミズゴロウかわえぇぇ！", "ミズゴロウかわえぇぇ！".FixLeftB(22));

            // 元の文字列未満の長さ
            Assert.AreEqual("ark?", "Bwark?".FixLeftB(4));
            Assert.AreEqual("しゃま？", "あしゃま？".FixLeftB(8));
            Assert.AreEqual("ｵゃま？", "あしゃま？".FixLeftB(7)); // 全角の途中

            // 元の文字列より大きい長さ
            Assert.AreEqual("   Hoo!", "Hoo!".FixLeftB(7));
            Assert.AreEqual("    もふう！", "もふう！".FixLeftB(12));
        }

        /// <summary>
        /// FixRightB(string, int, char)
        /// </summary>
        [Test]
        public void FixRightB1()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                string nullText = null;
                nullText.FixRightB(0, ' ');
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => { "".FixRightB(-1, '0'); });

            // 元の文字列と同じ長さだとそのまま
            Assert.AreEqual("", "".FixRightB(0, ' '));
            Assert.AreEqual("Mudkip is so CUUUUTE!", "Mudkip is so CUUUUTE!".FixRightB(21, ' '));
            Assert.AreEqual("ミズゴロウかわえぇぇ！", "ミズゴロウかわえぇぇ！".FixRightB(22, ' '));

            // 元の文字列未満の長さ
            Assert.AreEqual("Bwar", "Bwark?".FixRightB(4, ' '));
            Assert.AreEqual("あしゃま", "あしゃま？".FixRightB(8, ' '));
            Assert.AreEqual("あしゃ ", "あしゃま？".FixRightB(7, ' ')); // 全角の途中

            // 元の文字列より大きい長さ
            Assert.AreEqual("Hoo!!!!", "Hoo!".FixRightB(7, '!'));
            Assert.AreEqual("もふう！！！", "もふう！".FixRightB(12, '！'));
            Assert.AreEqual("もふう！！ ", "もふう！".FixRightB(11, '！')); // 全角の途中
        }

        /// <summary>
        /// FixRightB(string, int)
        /// </summary>
        [Test]
        public void FixRightB2()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                string nullText = null;
                nullText.FixRightB(0);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => { "".FixRightB(-1); });

            // 元の文字列と同じ長さだとそのまま
            Assert.AreEqual("", "".FixRightB(0));
            Assert.AreEqual("Mudkip is so CUUUUTE!", "Mudkip is so CUUUUTE!".FixRightB(21));
            Assert.AreEqual("ミズゴロウかわえぇぇ！", "ミズゴロウかわえぇぇ！".FixRightB(22));

            // 元の文字列未満の長さ
            Assert.AreEqual("Bwar", "Bwark?".FixRightB(4));
            Assert.AreEqual("あしゃま", "あしゃま？".FixRightB(8));
            Assert.AreEqual("あしゃ ", "あしゃま？".FixRightB(7)); // 全角の途中

            // 元の文字列より大きい長さ
            Assert.AreEqual("Hoo!   ", "Hoo!".FixRightB(7));
            Assert.AreEqual("もふう！    ", "もふう！".FixRightB(12));
        }
    }
}
