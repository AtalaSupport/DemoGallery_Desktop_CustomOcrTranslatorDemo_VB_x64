Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports Atalasoft.Ocr

Namespace CustomOcrTranslatorDemo
	''' <summary>
	''' The PigLatinTranslator class shows how to write a custom translator.  The point is to extend the 
	''' IForeignTranslator class and override the appropiate methods and variables.
	''' </summary>
	Public Class PigLatinTranslator
		Implements IForeignTranslator
		' this is the list of mime-types that will be handled by this translator.
		' this translator only translates to one type of text, pig latin.
		Private Shared _supported As String() = { "text/pig-latin" }
		Private Shared _vowels As String = "aeiouAEIOU"
		Public Sub New()
		End Sub
		#Region "xxIForeignTranslator Members"
        Public Overloads Function Prepare(ByVal engine As OcrEngine, ByVal doc As OcrDocument) As Object Implements Atalasoft.Ocr.IForeignTranslator.Prepare
            Return Nothing
        End Function
        Public Sub Finish(ByVal engine As OcrEngine, ByVal doc As OcrDocument, ByVal successful As Boolean, ByVal translationObject As Object) Implements Atalasoft.Ocr.IForeignTranslator.Finish
        End Sub

        ' this method is called from within the OcrEngine class.
        Public Sub Translate(ByVal engine As OcrEngine, ByVal doc As OcrDocument, ByVal mimeType As String, ByVal outStream As System.IO.Stream, ByVal translationObject As Object) Implements Atalasoft.Ocr.IForeignTranslator.Translate
            Dim writer As StreamWriter = New StreamWriter(outStream)
            If (Not Supports(mimeType)) Then
                Throw New NotImplementedException("Unknown output format: " & mimeType)
            End If
            ' traverse the OcrDocument so we can get to the text.
            For Each page As OcrPage In doc.Pages
                For Each region As OcrRegion In page.Regions
                    If TypeOf region Is OcrTextRegion Then
                        Dim textRegion As OcrTextRegion = CType(region, OcrTextRegion)
                        For Each line As OcrLine In textRegion.Lines
                            For Each word As OcrWord In line.Words
                                ' this is where we access each individual word, and we change it into its pig-latin form.
                                Dim igpay As String = Me.Igpay(word.Text)
                                ' write the translated word into the output stream.
                                writer.Write(igpay & " ")
                            Next word
                        Next line
                        ' lets seperate the lines with 2 line breaks.
                        writer.WriteLine("")
                        writer.WriteLine("")
                    End If
                Next region
            Next page
            ' we're done!
            writer.Flush()
        End Sub

        Private Function IsVowel(ByVal c As Char) As Boolean
            Return _vowels.IndexOf(c) >= 0
        End Function

        Private Function FindFirstVowel(ByVal s As String) As Integer
            For i As Integer = 0 To s.Length - 1
                If IsVowel(s.Chars(i)) Then
                    Return i
                End If
            Next i
            Return -1
        End Function

        Private Function Igpay(ByVal word As String) As String
            Dim output As Char() = New Char(word.Length + 2 - 1) {}
            Dim toCaps As Boolean = Char.IsUpper(word.Chars(0))

            ' starts with a vowell, add on "ay"
            If IsVowel(word.Chars(0)) Then
                Dim i As Integer
                For i = 0 To word.Length - 1
                    output(i) = word.Chars(i)
                Next i
                output(i) = "a"c
                output(i + 1) = "y"c
            Else
                ' starts with a consonant, find the first vowell
                Dim firstVowel, j, k As Integer
                firstVowel = FindFirstVowel(word)

                ' no vowell?  Don't translate
                If firstVowel = -1 Then
                    Return word
                End If

                ' copy everything from the first vowell on
                For j = firstVowel To word.Length - 1
                    output(j - firstVowel) = word.Chars(j)
                Next j

                ' copy everything up to the first vowell onto the end
                For k = 0 To firstVowel - 1
                    Dim c As Char = word.Chars(k)
                    If toCaps AndAlso k = 0 Then
                        c = Char.ToLower(c)
                    End If
                    output(k + word.Length - firstVowel) = c
                Next k
                ' add ay
                output(k + word.Length - firstVowel) = "a"c
                output(k + word.Length - firstVowel + 1) = "y"c
                ' correct for upper/lower case
                If toCaps Then
                    output(0) = Char.ToUpper(output(0))
                End If
            End If
            Return New String(output)
        End Function

        Public Sub Translate(ByVal engine As OcrEngine, ByVal doc As OcrDocument, ByVal mimeType As String, ByVal outFileName As String, ByVal translationObject As Object) Implements Atalasoft.Ocr.IForeignTranslator.Translate
            Dim stream As FileStream = New FileStream(outFileName, FileMode.Open, FileAccess.Read)
            Try
                Translate(engine, doc, mimeType, stream, translationObject)
            Catch e As System.Exception
                Throw e
            Finally
                stream.Close()
            End Try
        End Sub

#End Region

		#Region "ITranslator Members"

        Public Function Supports(ByVal mimeType As String) As Boolean Implements Atalasoft.Ocr.ITranslator.Supports
            For Each s As String In _supported
                If mimeType = s Then
                    Return True
                End If
            Next s
            Return False
        End Function

        Public Function IsNative() As Boolean
            Return False
        End Function

        Public Function Supported() As String() Implements Atalasoft.Ocr.ITranslator.Supported

            Return _supported
        End Function

        Public Function CanStream() As Boolean Implements Atalasoft.Ocr.ITranslator.CanStream
            Return True
        End Function

#End Region

    End Class
End Namespace
