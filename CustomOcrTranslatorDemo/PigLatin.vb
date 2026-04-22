'
'* 	This demo shows how to write a custom translator.  This winform application uses
'* the custom translator from PigLatinTranslator.cs.  All the basics are shown here,
'* but for more information please see the OCR Developer's Guide.
'*
'


Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.IO
Imports Atalasoft.Imaging
Imports Atalasoft.Imaging.WinControls
Imports Atalasoft.Ocr
Imports Atalasoft.Ocr.Tesseract
Imports Atalasoft.Imaging.Codec
Imports WinDemoHelperMethods.WinDemoHelperMethods

Namespace CustomOcrTranslatorDemo
    ''' <summary>
    ''' Summary description for Form1.
    ''' </summary>
    Public Class Form1
        Inherits System.Windows.Forms.Form
        Private WithEvents SelectImage As System.Windows.Forms.Button
        Private groupBox1 As System.Windows.Forms.GroupBox
        Private WithEvents plainRadio As System.Windows.Forms.RadioButton
        Private WithEvents pigRadio As System.Windows.Forms.RadioButton
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.Container = Nothing
        Private engine As OcrEngine
        Private theText As System.Windows.Forms.TextBox
        Private anslatorTray As PigLatinTranslator
        Private WithEvents AboutButton As System.Windows.Forms.Button
        Private Shared tempFile As String = Path.GetTempPath() + "tempOCR.txt"
        Private _validLicense As Boolean

#Region "Windows Form Designer generated code"
        Public Sub New()
            '
            ' Required for Windows Form Designer support
            '
            InitializeComponent()

            'Initialize the ocr engin only once, at start up.
            ' setting the OcrResources path to null forces the OcrEngine to find them either from
            ' the registry entry(hklm\software\atalasoft\dotimage\3.0) or in the same directory as the atalasoft dll's.
            Try
                engine = New Tesseract5Engine
                _validLicense = True
            Catch e1 As AtalasoftLicenseException
                _validLicense = False
                LicenseCheckFailure("This Demo cannot run without an Atalasoft OCR License, with Tesseract Engine.  Please request an evaluation license, or purchase one from www.Atalasoft.com")
                Return
            End Try
            engine.Initialize()
            ' add the custom pigLatinTranslator to the collection of availible translators.
            ' During translation, the OcrEngine will use the first translator, in the collection,
            ' that supports the given mime type.
            anslatorTray = New PigLatinTranslator
            engine.Translators.Add(anslatorTray)
        End Sub

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not components Is Nothing Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub


        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.SelectImage = New System.Windows.Forms.Button()
            Me.plainRadio = New System.Windows.Forms.RadioButton()
            Me.pigRadio = New System.Windows.Forms.RadioButton()
            Me.groupBox1 = New System.Windows.Forms.GroupBox()
            Me.theText = New System.Windows.Forms.TextBox()
            Me.AboutButton = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            ' 
            ' SelectImage
            ' 
            Me.SelectImage.Location = New System.Drawing.Point(24, 16)
            Me.SelectImage.Name = "SelectImage"
            Me.SelectImage.Size = New System.Drawing.Size(128, 72)
            Me.SelectImage.TabIndex = 0
            Me.SelectImage.Text = "Translate Image..."
            '			Me.SelectImage.Click += New System.EventHandler(Me.SelectImage_Click);
            ' 
            ' plainRadio
            ' 
            Me.plainRadio.Checked = True
            Me.plainRadio.Location = New System.Drawing.Point(216, 32)
            Me.plainRadio.Name = "plainRadio"
            Me.plainRadio.Size = New System.Drawing.Size(80, 24)
            Me.plainRadio.TabIndex = 1
            Me.plainRadio.TabStop = True
            Me.plainRadio.Text = "Plain Text"
            '			Me.plainRadio.CheckedChanged += New System.EventHandler(Me.plainRadio_CheckedChanged);
            ' 
            ' pigRadio
            ' 
            Me.pigRadio.Location = New System.Drawing.Point(216, 56)
            Me.pigRadio.Name = "pigRadio"
            Me.pigRadio.Size = New System.Drawing.Size(80, 24)
            Me.pigRadio.TabIndex = 2
            Me.pigRadio.Text = "Pig Latin"
            '			Me.pigRadio.CheckedChanged += New System.EventHandler(Me.pigRadio_CheckedChanged);
            ' 
            ' groupBox1
            ' 
            Me.groupBox1.Location = New System.Drawing.Point(200, 8)
            Me.groupBox1.Name = "groupBox1"
            Me.groupBox1.Size = New System.Drawing.Size(112, 80)
            Me.groupBox1.TabIndex = 3
            Me.groupBox1.TabStop = False
            Me.groupBox1.Text = "Translate To"
            ' 
            ' theText
            ' 
            Me.theText.Anchor = (CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
            Me.theText.Location = New System.Drawing.Point(0, 96)
            Me.theText.Multiline = True
            Me.theText.Name = "theText"
            Me.theText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.theText.Size = New System.Drawing.Size(440, 440)
            Me.theText.TabIndex = 4
            Me.theText.Text = ""
            ' 
            ' AboutButton
            ' 
            Me.AboutButton.Location = New System.Drawing.Point(352, 8)
            Me.AboutButton.Name = "AboutButton"
            Me.AboutButton.TabIndex = 5
            Me.AboutButton.Text = "About ..."
            '			Me.AboutButton.Click += New System.EventHandler(Me.AboutButton_Click);
            ' 
            ' Form1
            ' 
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(440, 534)
            Me.Controls.Add(Me.AboutButton)
            Me.Controls.Add(Me.theText)
            Me.Controls.Add(Me.pigRadio)
            Me.Controls.Add(Me.plainRadio)
            Me.Controls.Add(Me.SelectImage)
            Me.Controls.Add(Me.groupBox1)
            Me.Name = "Form1"
            Me.Text = "Pig Latin Translator"
            Me.ResumeLayout(False)

        End Sub


        ''' <summary>
        ''' The main entry point for the application.
        ''' </summary>
        <STAThread()>
        Shared Sub Main()
            Application.Run(New Form1())
        End Sub
#End Region


#Region "License check code"

        Private Sub LicenseCheckFailure(ByVal message As String)
            AddHandler Load, AddressOf Form1_Load
            If MessageBox.Show(Me, message & Constants.vbCrLf & Constants.vbCrLf & "Would you like to request an evaluation license?", "License Required", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
                ' Locate the activation utility.
                Dim path As String = ""
                Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Atalasoft\dotImage\5.0")
                If Not key Is Nothing Then
                    path = Convert.ToString(key.GetValue("AssemblyBasePath"))
                    If Not path Is Nothing AndAlso path.Length > 5 Then
                        path = path.Substring(0, path.Length - 3) & "AtalasoftToolkitActivation.exe"
                    Else
                        path = System.IO.Path.GetFullPath("..\..\..\..\..\AtalasoftToolkitActivation.exe")
                    End If

                    key.Close()
                End If

                If System.IO.File.Exists(path) Then
                    System.Diagnostics.Process.Start(path)
                Else
                    MessageBox.Show(Me, "We were unable to location the DotImage activation utility." & Constants.vbCrLf & "Please run it from the Start menu shortcut.", "File Not Found")
                End If
            End If
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs)
            ' close the demo if there is no valid license
            If (Not Me._validLicense) Then
                Application.Exit()
            End If
        End Sub

#End Region

        Shared Sub New()
            HelperMethods.PopulateDecoders(RegisteredDecoders.Decoders)
        End Sub

        ' this method loads the image and translates it to the selected mime type.
        Private Sub SelectImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SelectImage.Click
            Dim oif As OpenImageFileDialog = New OpenImageFileDialog
            oif.Filter = HelperMethods.CreateDialogFilter(True)

            ' try to locate images folder
            Dim imagesFolder As String = Application.ExecutablePath
            ' we assume we are running under the DotImage install folder
            Dim pos As Integer = imagesFolder.IndexOf("DotImage ")
            If pos <> -1 Then
                imagesFolder = imagesFolder.Substring(0, imagesFolder.IndexOf("\", pos)) & "\Images\OCR"
            End If

            'use this folder as starting point			
            oif.InitialDirectory = imagesFolder

            If oif.ShowDialog(Me) <> System.Windows.Forms.DialogResult.OK Then
                Return
            End If

            Dim paths As String() = New String(0) {}
            paths(0) = oif.FileName
            oif.Dispose()
            ' this is how the translate method takes images as input.
            Dim source As FileSystemImageSource = New FileSystemImageSource(paths, True)

            Dim reader As StreamReader = Nothing

            Dim mime As String = GetMimeType()
            ' Translate the document.
            If engine.CanStream(mime) Then
                Dim stream As MemoryStream = New MemoryStream
                Try
                    engine.Translate(source, mime, stream)
                Catch err As System.Exception
                    MessageBox.Show("Caught an error: " & err.Message)
                End Try
                stream.Seek(0, SeekOrigin.Begin)
                reader = New StreamReader(stream)
            ElseIf engine.CanTranslate(mime) Then
                ' this mime type can only be output to a file directly
                Try
                    engine.Translate(source, mime, tempFile)
                Catch err As System.Exception
                    MessageBox.Show("Caught an error" & err.Message)
                    If File.Exists(tempFile) Then
                        File.Delete(tempFile)
                    End If
                End Try
                reader = New StreamReader(tempFile)
            Else
                MessageBox.Show(Me, "Can't find a translator for type """ & mime & """")
            End If
            If Not reader Is Nothing Then ' else the translate operation failed for some reason.
                Try
                    ' read text back to text box.
                    theText.Text = reader.ReadToEnd()
                Catch err As System.Exception
                    MessageBox.Show("Caught an error reading in data: " & err.Message)
                Finally
                    reader.Close()
                    If File.Exists(tempFile) Then
                        File.Delete(tempFile)
                    End If
                End Try
            End If
        End Sub


#Region "Handlers and Helpers"

        ' returns the currently selected mime type.
        Private Function GetMimeType() As String
            If plainRadio.Checked Then
                Return "text/plain"
            Else
                Return "text/pig-latin"
            End If
        End Function

        ' event handler for radio buttons
        Private Sub plainRadio_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles plainRadio.CheckedChanged
            If plainRadio.Checked Then
                pigRadio.Checked = False
            End If
        End Sub
        ' event handler for radio buttons.
        Private Sub pigRadio_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles pigRadio.CheckedChanged
            If pigRadio.Checked Then
                plainRadio.Checked = False
            End If
        End Sub
#End Region

        Private Sub AboutButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AboutButton.Click
            Dim aboutBox As AtalaDemos.AboutBox.About = New AtalaDemos.AboutBox.About("About Atalasoft DotImage Ocr Pig Latin Translator Demo", "DotImage Ocr Pig Latin Translator Demo")
            aboutBox.Description = "Demonstrates how to create and use a custom OCR translator.  The translator implemented here will convert regular text to pig latin.  A custom translator will be useful if to convert documents to an unsupported (or custom) format.  Writing your own translator is relatively simple, and this demo serves as a good reference. "
            aboutBox.ShowDialog()
        End Sub
    End Class
End Namespace
