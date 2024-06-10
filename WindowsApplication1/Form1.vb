Imports System.Text
Imports System.IO
Imports Microsoft.Win32

Public Class Form1
    Dim NewProcess As New System.Diagnostics.Process()
    Private Shared processOutput As StringBuilder = Nothing
    Private Shared Sub OutputHandler(ByVal sendingProcess As Object, ByVal outLine As DataReceivedEventArgs)
        ' Collect the sort command output.
        If Not String.IsNullOrEmpty(outLine.Data) Then
            ' Add the text to the collected output.
            processOutput.AppendLine(outLine.Data)
        End If
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        processOutput = New StringBuilder()
        NewProcess = New Process
        With NewProcess.StartInfo
            .FileName = My.MySettings.Default.Java_Path & "\bin\javaw.exe"
            .Arguments = "-jar """ & My.MySettings.Default.Server_FullPath & """ -Xms1024m -Xmx1024m"
            .WorkingDirectory = My.MySettings.Default.Server_Folder
            .RedirectStandardOutput = True
            .RedirectStandardError = True
            .RedirectStandardInput = True
            .UseShellExecute = False
            .WindowStyle = ProcessWindowStyle.Hidden
            .CreateNoWindow = True
        End With

        ' Set our event handler to asynchronously read the sort output.
        AddHandler NewProcess.OutputDataReceived, AddressOf OutputHandler
        NewProcess.Start()
        NewProcess.BeginOutputReadLine()
        Dim myStreamWriter As StreamWriter = NewProcess.StandardInput
        NewProcess.WaitForExit()
        NewProcess.CancelOutputRead()
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TextBox1.Text = ""
        RichTextBox1.Text = ""
        TextBox2.Enabled = True
        Button3.Enabled = True
        BackgroundWorker1.RunWorkerAsync()
        Timer1.Start()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim myStreamWriter As StreamWriter = NewProcess.StandardInput
        myStreamWriter.WriteLine(TextBox2.Text)
        temp()
        TextBox2.Text = ""
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        temp()
        If NewProcess.HasExited = True Then
            RichTextBox1.Text = RichTextBox1.Text & vbNewLine & "The process has been terminated." & vbNewLine
            Dim len As Integer = RichTextBox1.TextLength
            Dim lastindex = RichTextBox1.Text.LastIndexOf("The process has been terminated.")
            Dim index As Integer = 0
            While index < lastindex
                RichTextBox1.Find("The process has been terminated.", index, len, RichTextBoxFinds.None)
                RichTextBox1.SelectionColor = Color.Red
                index = RichTextBox1.Text.IndexOf("The process has been terminated.", index) + 1
            End While

            lastindex = RichTextBox1.Text.LastIndexOf("[INFO]")
            index = 0

            While index < lastindex
                RichTextBox1.Find("[INFO]", index, len, RichTextBoxFinds.None)
                RichTextBox1.SelectionColor = Color.Blue
                index = RichTextBox1.Text.IndexOf("[INFO]", index) + 1
            End While

            lastindex = RichTextBox1.Text.LastIndexOf("[WARNING]")
            index = 0

            While index < lastindex
                RichTextBox1.Find("[WARNING]", index, len, RichTextBoxFinds.None)
                RichTextBox1.SelectionColor = Color.Goldenrod
                index = RichTextBox1.Text.IndexOf("[WARNING]", index) + 1
            End While

            lastindex = RichTextBox1.Text.LastIndexOf("[SHUTDOWN]")
            index = 0

            While index < lastindex
                RichTextBox1.Find("[SHUTDOWN]", index, len, RichTextBoxFinds.None)
                RichTextBox1.SelectionColor = Color.Maroon
                index = RichTextBox1.Text.IndexOf("[SHUTDOWN]", index) + 1
            End While
            RichTextBox1.SelectionLength = 0
            BackgroundWorker1.CancelAsync()
            Timer1.Stop()
            NewProcess.CancelOutputRead()
            processOutput.Clear()
            TextBox2.Enabled = False
            Button3.Enabled = False
            RichTextBox1.ScrollToCaret()
        End If

    End Sub
    Sub GetText()
        If Not TextBox1.Text = processOutput.ToString And Not NewProcess.HasExited = True Then
            TextBox1.Text = processOutput.ToString
            RichTextBox1.Text = processOutput.ToString.Replace("[Server thread/INFO]", "[INFO]")
            RichTextBox1.Text = RichTextBox1.Text.Replace("[Server thread/WARN]", "[WARNING]")
            RichTextBox1.Text = RichTextBox1.Text.Replace("[Server Shutdown Thread/INFO]", "[SHUTDOWN]")
            Dim len As Integer = RichTextBox1.TextLength
            Dim lastindex = RichTextBox1.Text.LastIndexOf("[INFO]")
            Dim index As Integer = 0

            While index < lastindex
                RichTextBox1.Find("[INFO]", index, len, RichTextBoxFinds.None)
                RichTextBox1.SelectionColor = Color.Blue
                index = RichTextBox1.Text.IndexOf("[INFO]", index) + 1
            End While

            lastindex = RichTextBox1.Text.LastIndexOf("[WARNING]")
            index = 0
            While index < lastindex
                RichTextBox1.Find("[WARNING]", index, len, RichTextBoxFinds.None)
                RichTextBox1.SelectionColor = Color.Goldenrod
                index = RichTextBox1.Text.IndexOf("[WARNING]", index) + 1
            End While

            lastindex = RichTextBox1.Text.LastIndexOf("[SHUTDOWN]")
            index = 0
            While index < lastindex
                RichTextBox1.Find("[SHUTDOWN]", index, len, RichTextBoxFinds.None)
                RichTextBox1.SelectionColor = Color.Maroon
                index = RichTextBox1.Text.IndexOf("[SHUTDOWN]", index) + 1
            End While
            RichTextBox1.SelectionLength = 0
            RichTextBox1.ScrollToCaret()
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Shell("taskkill /f /im javaw.exe", vbHide)
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        ListBox1.Items.Add("ryan940618")
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.SelectedItem Is Nothing Then
            Label1.Visible = False
            ListBox2.Visible = False
        Else
            Label1.Text = ListBox1.SelectedItem
            Label1.Visible = True
            ListBox2.Visible = True
        End If
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox2.SelectedIndexChanged
        If Not ListBox2.SelectedIndex = 1 And Not ListBox2.SelectedIndex = 2 And Not ListBox2.SelectedIndex = 3 Then
            Label2.Visible = False
            TextBox3.Visible = False
            TextBox3.Text = ""
        End If

        If ListBox2.SelectedIndex = 1 Then
            Label2.Text = "Kick Reason: "
            Label2.Visible = True
            TextBox3.Visible = True
        End If

        If ListBox2.SelectedIndex = 2 Then
            Label2.Text = "Ban Reason: "
            Label2.Visible = True
            TextBox3.Visible = True
        End If

        If ListBox2.SelectedIndex = 3 Then
            Label2.Text = "Ban Reason: "
            Label2.Visible = True
            TextBox3.Visible = True
        End If
    End Sub

    Private Sub ListBox2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox2.DoubleClick
        Dim myStreamWriter As StreamWriter = NewProcess.StandardInput
        If ListBox2.SelectedIndex = 0 Then
            myStreamWriter.WriteLine("kill " & ListBox1.SelectedItem)
        End If
        If ListBox2.SelectedIndex = 1 Then
            myStreamWriter.WriteLine("kick " & ListBox1.SelectedItem & " " & TextBox3.Text)
        End If
        If ListBox2.SelectedIndex = 2 Then
            myStreamWriter.WriteLine("ban " & ListBox1.SelectedItem & " " & TextBox3.Text)
        End If
        If ListBox2.SelectedIndex = 3 Then
            myStreamWriter.WriteLine("ban-ip " & ListBox1.SelectedItem & " " & TextBox3.Text)
        End If
        If ListBox2.SelectedIndex = 4 Then
            myStreamWriter.WriteLine("pardon " & ListBox1.SelectedItem)
        End If
        If ListBox2.SelectedIndex = 5 Then
            myStreamWriter.WriteLine("pardon-ip " & ListBox1.SelectedItem)
        End If
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim myStreamWriter As StreamWriter = NewProcess.StandardInput
        myStreamWriter.WriteLine("stop")
    End Sub

    Sub temp()
        If Not processOutput.ToString = "" And Not NewProcess.HasExited = True Then
            TextBox1.Text = processOutput.ToString
            TextBox1.Text = TextBox1.Text.Replace("[Server thread/INFO]", "[INFO]")
            TextBox1.Text = TextBox1.Text.Replace("[Server thread/WARN]", "[WARNING]")
            TextBox1.Text = TextBox1.Text.Replace("[Server Shutdown Thread/INFO]", "[SHUTDOWN]")
            RichTextBox1.Text = RichTextBox1.Text & TextBox1.Text
            Dim len As Integer = RichTextBox1.TextLength
            Dim lastindex = RichTextBox1.Text.LastIndexOf("[INFO]")
            Dim index As Integer = 0
            While index < lastindex
                RichTextBox1.Find("[INFO]", index, len, RichTextBoxFinds.None)
                RichTextBox1.SelectionColor = Color.Blue
                index = RichTextBox1.Text.IndexOf("[INFO]", index) + 1
            End While

            lastindex = RichTextBox1.Text.LastIndexOf("[WARNING]")
            index = 0
            While index < lastindex
                RichTextBox1.Find("[WARNING]", index, len, RichTextBoxFinds.None)
                RichTextBox1.SelectionColor = Color.Goldenrod
                index = RichTextBox1.Text.IndexOf("[WARNING]", index) + 1
            End While

            lastindex = RichTextBox1.Text.LastIndexOf("[SHUTDOWN]")
            index = 0
            While index < lastindex
                RichTextBox1.Find("[SHUTDOWN]", index, len, RichTextBoxFinds.None)
                RichTextBox1.SelectionColor = Color.Maroon
                index = RichTextBox1.Text.IndexOf("[SHUTDOWN]", index) + 1
            End While

            Dim detection As Integer = 1
            Dim regex As RegularExpressions.Regex
            Dim match As RegularExpressions.Match

            Do While detection > 0
                regex = New RegularExpressions.Regex("INFO]: (.*?) joined the game")
                match = regex.Match(TextBox1.Text)
                If match.Success Then
                    If (match.Value.Replace("INFO]: ", "").Replace(" joined the game", "")).Contains("[") OrElse (match.Value.Replace("INFO]: ", "").Replace(" joined the game", "")).Contains("]") OrElse (match.Value.Replace("INFO]: ", "").Replace(" joined the game", "")).Contains("*") Then
                        TextBox1.Text = TextBox1.Text.Replace(match.Value, "")
                        detection = 2
                    Else
                        ListBox1.Items.Add(match.Value.Replace("INFO]: ", "").Replace(" joined the game", ""))
                        TextBox1.Text = TextBox1.Text.Replace(match.Value, "")
                        detection = 2
                    End If
                End If

                regex = New RegularExpressions.Regex("INFO]: (.*?) left the game")
                match = regex.Match(TextBox1.Text)
                If match.Success Then
                    If (match.Value.Replace("INFO]: ", "").Replace(" joined the game", "")).Contains("[") OrElse (match.Value.Replace("INFO]: ", "").Replace(" joined the game", "")).Contains("]") OrElse (match.Value.Replace("INFO]: ", "").Replace(" joined the game", "")).Contains("*") Then
                        TextBox1.Text = TextBox1.Text.Replace(match.Value, "")
                        detection = 2
                    Else
                        ListBox1.Items.Remove(match.Value.Replace("INFO]: ", "").Replace(" left the game", ""))
                        TextBox1.Text = TextBox1.Text.Replace(match.Value, "")
                        detection = 2
                    End If
                End If

                If detection = 2 Then
                    detection = 1
                Else
                    detection = 0
                End If
            Loop

            RichTextBox1.SelectionLength = 0
            RichTextBox1.ScrollToCaret()
            processOutput.Clear()
        End If
    End Sub

    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox2.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            e.SuppressKeyPress = True
            Dim myStreamWriter As StreamWriter = NewProcess.StandardInput
            myStreamWriter.WriteLine(TextBox2.Text)
            temp()
            TextBox2.Text = ""
        End If
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        ListBox1.Items.Remove("ryan940618")
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        ListBox1.Items.Add("able")
        ListBox1.Items.Add("new")
        ListBox1.Items.Add("island")
        ListBox1.Items.Add("maid")
        ListBox1.Items.Add("contempt")
        ListBox1.Items.Add("grounds")
        ListBox1.Items.Add("rain")
        ListBox1.Items.Add("bond")
        ListBox1.Items.Add("storm")
        ListBox1.Items.Add("install")
        ListBox1.Items.Add("eaux")
        ListBox1.Items.Add("college")
        ListBox1.Items.Add("nerve")
        ListBox1.Items.Add("trait")
        ListBox1.Items.Add("integrated")
        ListBox1.Items.Add("tube")
        ListBox1.Items.Add("copy")
        ListBox1.Items.Add("sum")
        ListBox1.Items.Add("store")
        ListBox1.Items.Add("edge")
        ListBox1.Items.Add("water")
        ListBox1.Items.Add("zero")
        ListBox1.Items.Add("student")
        ListBox1.Items.Add("nightmare")
        ListBox1.Items.Add("forget")
        ListBox1.Items.Add("virtue")
        ListBox1.Items.Add("cheek")
        ListBox1.Items.Add("exposure")
        ListBox1.Items.Add("church")
        ListBox1.Items.Add("coal")
        ListBox1.Items.Add("slogan")
        ListBox1.Items.Add("passion")
        ListBox1.Items.Add("classroom")
        ListBox1.Items.Add("surprise")
        ListBox1.Items.Add("intensify")
        ListBox1.Items.Add("important")
        ListBox1.Items.Add("gossip")
        ListBox1.Items.Add("responsibility")
        ListBox1.Items.Add("prescription")
        ListBox1.Items.Add("sheep")
        ListBox1.Items.Add("treat")
        ListBox1.Items.Add("donor")
        ListBox1.Items.Add("brown")
        ListBox1.Items.Add("outside")
        ListBox1.Items.Add("be")
        ListBox1.Items.Add("intervention")
        ListBox1.Items.Add("punch")
        ListBox1.Items.Add("dollar")
        ListBox1.Items.Add("rear")
        ListBox1.Items.Add("hobby")
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        ListBox1.Items.Remove(ListBox1.SelectedItem)
    End Sub

    Private Sub SettingsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DawdToolStripMenuItem.Click
        Form2.Label6.Text = My.MySettings.Default.Server_FullPath
        Dim LocalMachine As RegistryKey
        Dim JavaRegRoot As RegistryKey
        Dim JavaRegLocation As RegistryKey
        Dim Java_Ver As String
        Dim Java_Location As String
        LocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
        JavaRegRoot = LocalMachine.OpenSubKey("SOFTWARE\JavaSoft\Java Runtime Environment")
        Java_Ver = JavaRegRoot.GetValue("CurrentVersion").ToString
        JavaRegLocation = LocalMachine.OpenSubKey("SOFTWARE\JavaSoft\Java Runtime Environment\" & Java_Ver)
        Java_Location = JavaRegLocation.GetValue("JavaHome").ToString
        Form2.Label2.Text = Java_Ver
        Form2.Label4.Text = Java_Location
        Form2.Label7.Visible = False
        My.MySettings.Default.Java_Path = Java_Location
        My.MySettings.Default.Save()
        Form2.Show()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim LocalMachine As RegistryKey
        Dim JavaRegRoot As RegistryKey
        Dim JavaRegLocation As RegistryKey
        Dim Java_Ver As String
        Dim Java_Location As String
        LocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
        JavaRegRoot = LocalMachine.OpenSubKey("SOFTWARE\JavaSoft\Java Runtime Environment")
        Java_Ver = JavaRegRoot.GetValue("CurrentVersion").ToString
        JavaRegLocation = LocalMachine.OpenSubKey("SOFTWARE\JavaSoft\Java Runtime Environment\" & Java_Ver)
        Java_Location = JavaRegLocation.GetValue("JavaHome").ToString
        Form2.Label4.Text = Java_Location
        Form2.Label2.Text = Java_Ver
        My.MySettings.Default.Java_Path = Java_Location
        My.MySettings.Default.Save()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Form3.TextBox1.Text = My.Computer.FileSystem.ReadAllText(My.MySettings.Default.Server_Folder & "\server.properties")
        Form3.Show()
    End Sub
End Class
