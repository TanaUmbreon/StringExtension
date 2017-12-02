using System;
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
    }
}
