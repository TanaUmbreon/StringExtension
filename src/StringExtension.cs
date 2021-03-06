﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StringExtension
{
    /// <summary>
    /// <see cref="String"/> クラスの拡張メソッドを提供します。
    /// </summary>
    public static class StringExtension
    {
        #region フィールド

        /// <summary>Shift-JIS の文字エンコーディング</summary>
        private static readonly Encoding ShiftJis = Encoding.GetEncoding("sjis");

        #endregion

        #region LenB

        /// <summary>
        /// 文字列を Shift-JIS として扱った場合のバイト数を取得します。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <returns>文字列のバイト数。</returns>
        public static int LenB(this string value)
        {
            return ShiftJis.GetByteCount(value);
        }

        #endregion

        #region MidB

        /// <summary>
        /// 文字列を Shift-JIS として扱い、バイト単位で指定した位置と長さに該当する部分文字列を取得します。 
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <param name="startIndex">0 から始まるバイト単位の開始位置。</param>
        /// <param name="length">バイト単位の長さ。</param>
        /// <returns>
        /// <paramref name="startIndex"/> から長さ <paramref name="length"/> を抽出することによって得られる部分文字列。
        /// <paramref name="startIndex"/> が文字列の長さを超えている、または、<paramref name="length"/> がゼロの場合は <see cref="String.Empty"/>。
        /// <paramref name="startIndex"/> と <paramref name="length"/> で表される部分が文字列を超えている場合は、超えている部分を無視した部分文字列。
        /// 部分文字列の末尾の文字が途中で分断された全角文字になる場合は、
        /// その分断された文字を切り詰め、長さ <paramref name="length"/> になるまで半角スペースで埋めます。
        /// </returns>
        public static string MidB(this string value, int startIndex, int length)
        {
            if (value == null) { throw new ArgumentNullException("value"); }
            if (startIndex < 0) { throw new ArgumentOutOfRangeException("startIndex", "開始位置を 0 未満にすることはできません。"); }
            if (length < 0) { throw new ArgumentOutOfRangeException("length", "長さを 0 未満にすることはできません。"); }

            // 空文字が確定している場合は無駄な処理をさせないようすぐ返す
            if (length == 0) { return ""; }
            var bytes = ShiftJis.GetBytes(value);
            if (startIndex >= bytes.Length) { return ""; }

            // 長さがオーバーしているとGetStringで例外になるので長さ調整してから部分文字列を取得
            var adjustedLength = (bytes.Length < startIndex + length) ? bytes.Length - startIndex : length;
            var result = ShiftJis.GetString(bytes, startIndex, adjustedLength);

            // 末尾にある全角文字の途中を抽出すると長さがずれることがある。
            // その場合は末尾を切り詰めて半角スペースで埋める(Shift-JIS前提で決め打ち)
            if (ShiftJis.GetByteCount(result) == adjustedLength) { return result; }
            return ShiftJis.GetString(bytes, startIndex, adjustedLength - 1) + ' ';
        }

        /// <summary>
        /// 文字列を Shift-JIS として扱い、バイト単位で指定した位置から始まる部分文字列を返します。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <param name="startIndex">0 から始まるバイト単位の開始位置。</param>
        /// <returns>
        /// <paramref name="startIndex"/> から始まる部分文字列。
        /// <paramref name="startIndex"/> が文字列の長さを超えている場合は <see cref="String.Empty"/>。
        /// </returns>
        public static string MidB(this string value, int startIndex)
        {
            // MidB(string, startIndex, length)のオーバーロードは呼び出さず独立したメソッドにしている。
            // 末尾の全角文字の途中を抽出するリスクがなく実装がシンプルになる為

            if (value == null) { throw new ArgumentNullException("value"); }
            if (startIndex < 0) { throw new ArgumentOutOfRangeException("startIndex", "開始位置を 0 未満にすることはできません。"); }

            // 開始位置が文字列の長さを超えた場合は空文字確定なのですぐに返す
            var bytes = ShiftJis.GetBytes(value);
            if (bytes.Length <= startIndex) { return ""; }

            return ShiftJis.GetString(bytes, startIndex, bytes.Length - startIndex);
        }

        #endregion

        #region LeftB

        /// <summary>
        /// 文字列を Shift-JIS として扱い、先頭からバイト単位で指定した長さまでの部分文字列を返します。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <param name="length">文字列の先頭から始まるバイト単位の長さ。</param>
        /// <returns>
        /// 先頭から長さ <paramref name="length"/> を抽出することによって得られる部分文字列。
        /// <paramref name="length"/> が文字列を超えている場合は元と同等の文字列。
        /// </returns>
        public static string LeftB(this string value, int length)
        {
            if (value == null) { throw new ArgumentNullException("value"); }
            if (length < 0) { throw new ArgumentOutOfRangeException("length", "長さを 0 未満にすることはできません。"); }

            // 指定した長さが元の文字列の長さ以上になる場合は元の文字列そのものなのですぐに返す
            var bytes = ShiftJis.GetBytes(value);
            if (length >= bytes.Length) { return value; }

            // 末尾にある全角文字の途中を抽出すると長さがずれることがある。
            // その場合は末尾を切り詰めて半角スペースで埋める(Shift-JIS前提で決め打ち)
            var result = ShiftJis.GetString(bytes, 0, length);
            if (ShiftJis.GetByteCount(result) == length) { return result; }
            return ShiftJis.GetString(bytes, 0, length - 1) + ' ';
        }

        #endregion

        #region RightB

        /// <summary>
        /// 文字列を Shift-JIS として扱い、末尾からバイト単位で指定した長さまでの部分文字列を返します。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <param name="length">文字列の末尾から始まるバイト単位の長さ。</param>
        /// <returns>
        /// 末尾から長さ <paramref name="length"/> を抽出することによって得られる部分文字列。
        /// <paramref name="length"/> が文字列を超えている場合は元と同等の文字列。
        /// </returns>
        public static string RightB(this string value, int length)
        {
            if (value == null) { throw new ArgumentNullException("value"); }
            if (length < 0) { throw new ArgumentOutOfRangeException("length", "長さを 0 未満にすることはできません。"); }

            // 空文字が確定している場合は無駄な処理をさせないようすぐ返す
            if (length == 0) { return ""; }

            // 指定した長さが元の文字列の長さ以上になる場合は元の文字列そのものなのですぐに返す
            var bytes = ShiftJis.GetBytes(value);
            if (length >= bytes.Length) { return value; }

            return ShiftJis.GetString(bytes, bytes.Length - length, length);
        }

        #endregion

        #region PadLeftB

        /// <summary>
        /// 文字列を Shift-JIS として扱い、バイト単位で指定した文字列の長さになるまで、指定した文字を左側に埋め込みます。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        /// <param name="paddingChar">埋め込み文字。</param>
        /// <returns>
        /// <paramref name="totalWidth"/> の長さになるまで左側に
        /// <paramref name="paddingChar"/> の文字が埋め込まれ、右寄せされた文字列。
        /// <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列と等しい文字列。
        /// </returns>
        public static string PadLeftB(this string value, int totalWidth, char paddingChar)
        {
            if (value == null) { throw new ArgumentNullException("value"); }
            if (totalWidth < 0) { throw new ArgumentOutOfRangeException("totalWidth", "長さを 0 未満にすることはできません。"); }

            // 指定した長さが元の文字列の長さ以下になる場合は元の文字列そのものなのですぐに返す
            var bytes = ShiftJis.GetBytes(value);
            if (totalWidth <= bytes.Length) { return value; }

            // totalWidthの長さを超えないように指定された文字を埋め込む
            int paddingCharLength = ShiftJis.GetByteCount(paddingChar.ToString());
            int toPaddingLength = totalWidth - bytes.Length; // 埋め込もうとする文字列の長さ
            int modLength = toPaddingLength % paddingCharLength; // 完全に埋め込むことができない長さ
            string result = new string(paddingChar, toPaddingLength / paddingCharLength) + value;

            // 完全に埋め込むとこができない場合は代わりに半角スペースで残りを埋め込む
            // (paddingCharに全角文字を指定すると起こりえる)
            return modLength == 0 ? result : new string(' ', modLength) + result;
        }

        /// <summary>
        /// 文字列を Shift-JIS として扱い、バイト単位で指定した文字列の長さになるまで、左側に空白を埋め込みます。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        /// <returns>
        /// <paramref name="totalWidth"/> の長さになるまで左側に空白が埋め込まれ、右寄せされた文字列。
        /// <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列と等しい文字列。
        /// </returns>
        public static string PadLeftB(this string value, int totalWidth)
        {
            return PadLeftB(value, totalWidth, ' ');
        }

        #endregion

        #region PadRightB

        /// <summary>
        /// 文字列を Shift-JIS として扱い、バイト単位で指定した文字列の長さになるまで、指定した文字を右側に埋め込みます。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        /// <param name="paddingChar">埋め込み文字。</param>
        /// <returns>
        /// <paramref name="totalWidth"/> の長さになるまで右側に
        /// <paramref name="paddingChar"/> の文字が埋め込まれ、左寄せされた文字列。
        /// <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列と等しい文字列。
        /// </returns>
        public static string PadRightB(this string value, int totalWidth, char paddingChar)
        {
            if (value == null) { throw new ArgumentNullException("value"); }
            if (totalWidth < 0) { throw new ArgumentOutOfRangeException("totalWidth", "長さを 0 未満にすることはできません。"); }

            var bytes = ShiftJis.GetBytes(value);
            if (totalWidth <= bytes.Length) { return value; }

            int paddingCharLength = ShiftJis.GetByteCount(paddingChar.ToString());
            int toPaddingLength = totalWidth - bytes.Length;
            int modLength = toPaddingLength % paddingCharLength;
            string result = value + new string(paddingChar, toPaddingLength / paddingCharLength);

            return modLength == 0 ? result : result + new string(' ', modLength);
        }

        /// <summary>
        /// 文字列を Shift-JIS として扱い、バイト単位で指定した文字列の長さになるまで、右側に空白を埋め込みます。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        /// <returns>
        /// <paramref name="totalWidth"/> の長さになるまで右側に空白が埋め込まれ、左寄せされた文字列。
        /// <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列と等しい文字列。
        /// </returns>
        public static string PadRightB(this string value, int totalWidth)
        {
            return PadRightB(value, totalWidth, ' ');
        }

        #endregion

        #region FixLeftB

        /// <summary>
        /// 文字列を Shift-JIS として扱い、右端からバイト単位で指定した文字列の長さまでの固定された文字列を返します。
        /// 指定した長さより短い場合は、指定した文字を左側に埋め込みます。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        /// <param name="paddingChar">埋め込み文字。</param>
        /// <returns>
        /// <paramref name="totalWidth"/> の長さになるまで左側に
        /// <paramref name="paddingChar"/> の文字が埋め込まれ、右寄せされた文字列。
        /// <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列の右端から
        /// <paramref name="totalWidth"/> までの部分文字列。
        /// </returns>
        public static string FixLeftB(this string value, int totalWidth, char paddingChar)
        {
            return value.PadLeftB(totalWidth, paddingChar).RightB(totalWidth);
        }

        /// <summary>
        /// 文字列を Shift-JIS として扱い、右端からバイト単位で指定した文字列の長さまでの固定された文字列を返します。
        /// 指定した長さより短い場合は、左側に空白を埋め込みます。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        /// <returns>
        /// <paramref name="totalWidth"/> の長さになるまで左側に空白が埋め込まれ、右寄せされた文字列。
        /// <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列の右端から
        /// <paramref name="totalWidth"/> までの部分文字列。
        /// </returns>
        public static string FixLeftB(this string value, int totalWidth)
        {
            return FixLeftB(value, totalWidth, ' ');
        }

        #endregion

        #region FixRightB

        /// <summary>
        /// 文字列を Shift-JIS として扱い、左端からバイト単位で指定した文字列の長さまでの固定された文字列を返します。
        /// 指定した長さより短い場合は、指定した文字を右側に埋め込みます。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        /// <param name="paddingChar">埋め込み文字。</param>
        /// <returns>
        /// <paramref name="totalWidth"/> の長さになるまで右側に
        /// <paramref name="paddingChar"/> の文字が埋め込まれ、左寄せされた文字列。
        /// <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列の左端から
        /// <paramref name="totalWidth"/> までの部分文字列。
        /// </returns>
        public static string FixRightB(this string value, int totalWidth, char paddingChar)
        {
            return value.PadRightB(totalWidth, paddingChar).LeftB(totalWidth);
        }

        /// <summary>
        /// 文字列を Shift-JIS として扱い、左端からバイト単位で指定した文字列の長さまでの固定された文字列を返します。
        /// 指定した長さより短い場合は、右側に空白を埋め込みます。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        /// <returns>
        /// <paramref name="totalWidth"/> の長さになるまで右側に空白が埋め込まれ、左寄せされた文字列。
        /// <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列の左端から
        /// <paramref name="totalWidth"/> までの部分文字列。
        /// </returns>
        public static string FixRightB(this string value, int totalWidth)
        {
            return FixRightB(value, totalWidth, ' ');
        }

        #endregion
    }
}
