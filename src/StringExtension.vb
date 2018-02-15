Imports System.Runtime.CompilerServices
Imports System.Text

' VB9 以前は名前空間の定義にGlobalを使用できない
#If VBC_VER > 9 Then
Namespace Global.StringExtension
#End If

    ''' <summary>
    ''' <see cref="String"/> クラスの拡張メソッドを提供します。
    ''' </summary>
    Public Module StringExtension

#Region "フィールド"

        ''' <summary>Shift-JIS の文字エンコーディング</summary>
        Private ReadOnly ShiftJis As Encoding = Encoding.GetEncoding("sjis")

#End Region

#Region "LenB"

        ''' <summary>
        ''' 文字列を Shift-JIS として扱った場合のバイト数を取得します。
        ''' </summary>
        ''' <param name="value">文字列。</param>
        ''' <returns>文字列のバイト数。</returns>
        <Extension()> Public Function LenB(ByVal value As String) As Integer
            Return ShiftJis.GetByteCount(value)
        End Function

#End Region

#Region "MidB"

        ''' <summary>
        ''' 文字列を Shift-JIS として扱い、バイト単位で指定した位置と長さに該当する部分文字列を取得します。 
        ''' </summary>
        ''' <param name="value">文字列。</param>
        ''' <param name="startIndex">0 から始まるバイト単位の開始位置。</param>
        ''' <param name="length">バイト単位の長さ。</param>
        ''' <returns>
        ''' <paramref name="startIndex"/> から長さ <paramref name="length"/> を抽出することによって得られる部分文字列。
        ''' <paramref name="startIndex"/> が文字列の長さを超えている、または、<paramref name="length"/> がゼロの場合は <see cref="String.Empty"/>。
        ''' <paramref name="startIndex"/> と <paramref name="length"/> で表される部分が文字列を超えている場合は、超えている部分を無視した部分文字列。
        ''' 部分文字列の末尾の文字が途中で分断された全角文字になる場合は、
        ''' その分断された文字を切り詰め、長さ <paramref name="length"/> になるまで半角スペースで埋めます。
        ''' </returns>
        <Extension()> Public Function MidB(ByVal value As String, ByVal startIndex As Integer, ByVal length As Integer) As String
            If (value Is Nothing) Then Throw New ArgumentNullException("value")
            If (startIndex < 0) Then Throw New ArgumentOutOfRangeException("startIndex", "開始位置を 0 未満にすることはできません。")
            If (length < 0) Then Throw New ArgumentOutOfRangeException("length", "長さを 0 未満にすることはできません。")

            ' 空文字が確定している場合は無駄な処理をさせないようすぐ返す
            If (length = 0) Then Return ""
            Dim bytes = ShiftJis.GetBytes(value)
            If (startIndex >= bytes.Length) Then Return ""

            ' 長さがオーバーしているとGetStringで例外になるので長さ調整してから部分文字列を取得
            Dim adjustedLength = If(bytes.Length < startIndex + length, bytes.Length - startIndex, length)
            Dim result = ShiftJis.GetString(bytes, startIndex, adjustedLength)

            ' 末尾にある全角文字の途中を抽出すると長さがずれることがある。
            ' その場合は末尾を切り詰めて半角スペースで埋める(Shift-JIS前提で決め打ち)
            If (ShiftJis.GetByteCount(result) = adjustedLength) Then Return result
            Return ShiftJis.GetString(bytes, startIndex, adjustedLength - 1) & " "c
        End Function

        ''' <summary>
        ''' 文字列を Shift-JIS として扱い、バイト単位で指定した位置から始まる部分文字列を返します。
        ''' </summary>
        ''' <param name="value">文字列。</param>
        ''' <param name="startIndex">0 から始まるバイト単位の開始位置。</param>
        ''' <returns>
        ''' <paramref name="startIndex"/> から始まる部分文字列。
        ''' <paramref name="startIndex"/> が文字列の長さを超えている場合は <see cref="String.Empty"/>。
        ''' </returns>
        <Extension()> Public Function MidB(ByVal value As String, ByVal startIndex As Integer) As String
            ' MidB(string, startIndex, length)のオーバーロードは呼び出さず独立したメソッドにしている。
            ' 末尾の全角文字の途中を抽出するリスクがなく実装がシンプルになる為

            If (value Is Nothing) Then Throw New ArgumentNullException("value")
            If (startIndex < 0) Then Throw New ArgumentOutOfRangeException("startIndex", "開始位置を 0 未満にすることはできません。")

            ' 開始位置が文字列の長さを超えた場合は空文字確定なのですぐに返す
            Dim bytes = ShiftJis.GetBytes(value)
            If (bytes.Length <= startIndex) Then Return ""

            Return ShiftJis.GetString(bytes, startIndex, bytes.Length - startIndex)
        End Function

#End Region

#Region "LeftB"

        ''' <summary>
        ''' 文字列を Shift-JIS として扱い、先頭からバイト単位で指定した長さまでの部分文字列を返します。
        ''' </summary>
        ''' <param name="value">文字列。</param>
        ''' <param name="length">文字列の先頭から始まるバイト単位の長さ。</param>
        ''' <returns>
        ''' 先頭から長さ <paramref name="length"/> を抽出することによって得られる部分文字列。
        ''' <paramref name="length"/> が文字列を超えている場合は元と同等の文字列。
        ''' </returns>
        <Extension()> Public Function LeftB(ByVal value As String, ByVal length As Integer) As String
            If (value Is Nothing) Then Throw New ArgumentNullException("value")
            If (length < 0) Then Throw New ArgumentOutOfRangeException("length", "長さを 0 未満にすることはできません。")

            ' 指定した長さが元の文字列の長さ以上になる場合は元の文字列そのものなのですぐに返す
            Dim bytes = ShiftJis.GetBytes(value)
            If (length >= bytes.Length) Then Return value

            ' 末尾にある全角文字の途中を抽出すると長さがずれることがある。
            ' その場合は末尾を切り詰めて半角スペースで埋める(Shift-JIS前提で決め打ち)
            Dim result = ShiftJis.GetString(bytes, 0, length)
            If (ShiftJis.GetByteCount(result) = length) Then Return result
            Return ShiftJis.GetString(bytes, 0, length - 1) & " "c
        End Function

#End Region

#Region "RightB"

        ''' <summary>
        ''' 文字列を Shift-JIS として扱い、末尾からバイト単位で指定した長さまでの部分文字列を返します。
        ''' </summary>
        ''' <param name="value">文字列。</param>
        ''' <param name="length">文字列の末尾から始まるバイト単位の長さ。</param>
        ''' <returns>
        ''' 末尾から長さ <paramref name="length"/> を抽出することによって得られる部分文字列。
        ''' <paramref name="length"/> が文字列を超えている場合は元と同等の文字列。
        ''' </returns>
        <Extension()> Public Function RightB(ByVal value As String, ByVal length As Integer) As String
            If (value Is Nothing) Then Throw New ArgumentNullException("value")
            If (length < 0) Then Throw New ArgumentOutOfRangeException("length", "長さを 0 未満にすることはできません。")

            ' 空文字が確定している場合は無駄な処理をさせないようすぐ返す
            If (length = 0) Then Return ""

            ' 指定した長さが元の文字列の長さ以上になる場合は元の文字列そのものなのですぐに返す
            Dim bytes = ShiftJis.GetBytes(value)
            If (length >= bytes.Length) Then Return value

            Return ShiftJis.GetString(bytes, bytes.Length - length, length)
        End Function

#End Region

#Region "PadLeftB"

        ''' <summary>
        ''' 文字列を Shift-JIS として扱い、バイト単位で指定した文字列の長さになるまで、指定した文字を左側に埋め込みます。
        ''' </summary>
        ''' <param name="value">文字列。</param>
        ''' <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        ''' <param name="paddingChar">埋め込み文字。</param>
        ''' <returns>
        ''' <paramref name="totalWidth"/> の長さになるまで左側に
        ''' <paramref name="paddingChar"/> の文字が埋め込まれ、右寄せされた文字列。
        ''' <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列と等しい文字列。
        ''' </returns>
        <Extension()> Public Function PadLeftB(ByVal value As String, ByVal totalWidth As Integer, ByVal paddingChar As Char) As String
            If (value Is Nothing) Then Throw New ArgumentNullException("value")
            If (totalWidth < 0) Then Throw New ArgumentOutOfRangeException("totalWidth", "長さを 0 未満にすることはできません。")

            ' 指定した長さが元の文字列の長さ以下になる場合は元の文字列そのものなのですぐに返す
            Dim bytes = ShiftJis.GetBytes(value)
            If (totalWidth <= bytes.Length) Then Return value

            ' totalWidthの長さを超えないように指定された文字を埋め込む
            Dim paddingCharLength As Integer = ShiftJis.GetByteCount(paddingChar.ToString())
            Dim toPaddingLength As Integer = totalWidth - bytes.Length ' 埋め込もうとする文字列の長さ
            Dim modLength As Integer = toPaddingLength Mod paddingCharLength ' 完全に埋め込むことができない長さ
            Dim result As String = New String(paddingChar, Convert.ToInt32(Math.Truncate(toPaddingLength / paddingCharLength))) & value

            ' 完全に埋め込むとこができない場合は代わりに半角スペースで残りを埋め込む
            ' (paddingCharに全角文字を指定すると起こりえる)
            Return If(modLength = 0, result, New String(" "c, modLength) & result)
        End Function

        ''' <summary>
        ''' 文字列を Shift-JIS として扱い、バイト単位で指定した文字列の長さになるまで、左側に空白を埋め込みます。
        ''' </summary>
        ''' <param name="value">文字列。</param>
        ''' <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        ''' <returns>
        ''' <paramref name="totalWidth"/> の長さになるまで左側に空白が埋め込まれ、右寄せされた文字列。
        ''' <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列と等しい文字列。
        ''' </returns>
        <Extension()> Public Function PadLeftB(ByVal value As String, ByVal totalWidth As Integer) As String
            Return PadLeftB(value, totalWidth, " "c)
        End Function

#End Region

#Region "PadRightB"

        ''' <summary>
        ''' 文字列を Shift-JIS として扱い、バイト単位で指定した文字列の長さになるまで、指定した文字を右側に埋め込みます。
        ''' </summary>
        ''' <param name="value">文字列。</param>
        ''' <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        ''' <param name="paddingChar">埋め込み文字。</param>
        ''' <returns>
        ''' <paramref name="totalWidth"/> の長さになるまで右側に
        ''' <paramref name="paddingChar"/> の文字が埋め込まれ、左寄せされた文字列。
        ''' <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列と等しい文字列。
        ''' </returns>
        <Extension()> Public Function PadRightB(ByVal value As String, ByVal totalWidth As Integer, ByVal paddingChar As Char) As String
            If (value Is Nothing) Then Throw New ArgumentNullException("value")
            If (totalWidth < 0) Then Throw New ArgumentOutOfRangeException("totalWidth", "長さを 0 未満にすることはできません。")

            Dim bytes = ShiftJis.GetBytes(value)
            If (totalWidth <= bytes.Length) Then Return value

            Dim paddingCharLength As Integer = ShiftJis.GetByteCount(paddingChar.ToString())
            Dim toPaddingLength As Integer = totalWidth - bytes.Length
            Dim modLength As Integer = toPaddingLength Mod paddingCharLength
            Dim result As String = value & New String(paddingChar, Convert.ToInt32(Math.Truncate(toPaddingLength / paddingCharLength)))

            Return If(modLength = 0, result, result & New String(" "c, modLength))
        End Function

        ''' <summary>
        ''' 文字列を Shift-JIS として扱い、バイト単位で指定した文字列の長さになるまで、右側に空白を埋め込みます。
        ''' </summary>
        ''' <param name="value">文字列。</param>
        ''' <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        ''' <returns>
        ''' <paramref name="totalWidth"/> の長さになるまで右側に空白が埋め込まれ、左寄せされた文字列。
        ''' <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列と等しい文字列。
        ''' </returns>
        <Extension()> Public Function PadRightB(ByVal value As String, ByVal totalWidth As Integer) As String
            Return PadRightB(value, totalWidth, " "c)
        End Function

#End Region

#Region "FixLeftB"

        ''' <summary>
        ''' 文字列を Shift-JIS として扱い、右端からバイト単位で指定した文字列の長さまでの固定された文字列を返します。
        ''' 指定した長さより短い場合は、指定した文字を左側に埋め込みます。
        ''' </summary>
        ''' <param name="value">文字列。</param>
        ''' <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        ''' <param name="paddingChar">埋め込み文字。</param>
        ''' <returns>
        ''' <paramref name="totalWidth"/> の長さになるまで左側に
        ''' <paramref name="paddingChar"/> の文字が埋め込まれ、右寄せされた文字列。
        ''' <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列の右端から
        ''' <paramref name="totalWidth"/> までの部分文字列。
        ''' </returns>
        <Extension()> Public Function FixLeftB(ByVal value As String, ByVal totalWidth As Integer, ByVal paddingChar As Char) As String
            Return value.PadLeftB(totalWidth, paddingChar).RightB(totalWidth)
        End Function

        ''' <summary>
        ''' 文字列を Shift-JIS として扱い、右端からバイト単位で指定した文字列の長さまでの固定された文字列を返します。
        ''' 指定した長さより短い場合は、左側に空白を埋め込みます。
        ''' </summary>
        ''' <param name="value">文字列。</param>
        ''' <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        ''' <returns>
        ''' <paramref name="totalWidth"/> の長さになるまで左側に空白が埋め込まれ、右寄せされた文字列。
        ''' <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列の右端から
        ''' <paramref name="totalWidth"/> までの部分文字列。
        ''' </returns>
        <Extension()> Public Function FixLeftB(ByVal value As String, ByVal totalWidth As Integer) As String
            Return FixLeftB(value, totalWidth, " "c)
        End Function

#End Region

#Region "FixRightB"

        ''' <summary>
        ''' 文字列を Shift-JIS として扱い、左端からバイト単位で指定した文字列の長さまでの固定された文字列を返します。
        ''' 指定した長さより短い場合は、指定した文字を右側に埋め込みます。
        ''' </summary>
        ''' <param name="value">文字列。</param>
        ''' <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        ''' <param name="paddingChar">埋め込み文字。</param>
        ''' <returns>
        ''' <paramref name="totalWidth"/> の長さになるまで右側に
        ''' <paramref name="paddingChar"/> の文字が埋め込まれ、左寄せされた文字列。
        ''' <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列の左端から
        ''' <paramref name="totalWidth"/> までの部分文字列。
        ''' </returns>
        <Extension()> Public Function FixRightB(ByVal value As String, ByVal totalWidth As Integer, ByVal paddingChar As Char) As String
            Return value.PadRightB(totalWidth, paddingChar).LeftB(totalWidth)
        End Function

        ''' <summary>
        ''' 文字列を Shift-JIS として扱い、左端からバイト単位で指定した文字列の長さまでの固定された文字列を返します。
        ''' 指定した長さより短い場合は、右側に空白を埋め込みます。
        ''' </summary>
        ''' <param name="value">文字列。</param>
        ''' <param name="totalWidth">結果として生成される、バイト単位の文字列の長さ。</param>
        ''' <returns>
        ''' <paramref name="totalWidth"/> の長さになるまで右側に空白が埋め込まれ、左寄せされた文字列。
        ''' <paramref name="totalWidth"/> が元の文字列の長さより短い場合は、元の文字列の左端から
        ''' <paramref name="totalWidth"/> までの部分文字列。
        ''' </returns>
        <Extension()> Public Function FixRightB(ByVal value As String, ByVal totalWidth As Integer) As String
            Return FixRightB(value, totalWidth, " "c)
        End Function

#End Region

    End Module

#If VBC_VER > 9 Then
End Namespace
#End If
