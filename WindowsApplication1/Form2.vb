Imports Microsoft.Win32
Imports System.IO

Public Class Form2
    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label6.Text = My.MySettings.Default.Server_FullPath
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
        Label2.Text = Java_Ver
        Label4.Text = Java_Location
        Label7.Visible = False
        My.MySettings.Default.Java_Path = Java_Location
        My.MySettings.Default.Save()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim Choose_Server = New OpenFileDialog
        With Choose_Server
            .Title = "Select Server File"
            .Filter = "Minecraft Server exe/jar|*.exe;*.jar"
            .InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}"
        End With

        If Choose_Server.ShowDialog = Windows.Forms.DialogResult.OK AndAlso Choose_Server.FileName <> "" Then
            Label6.Text = Choose_Server.FileName
            My.MySettings.Default.Server_FullPath = Choose_Server.FileName
            My.MySettings.Default.Server_Folder = Path.GetDirectoryName(Choose_Server.FileName)
            My.MySettings.Default.Server_File = Choose_Server.FileName.Replace(Path.GetDirectoryName(Choose_Server.FileName) & "\", "")
            My.MySettings.Default.Save()
            Label7.Visible = True
        End If
    End Sub

    Private Sub Form2_FormClosing(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.FormClosing
        My.MySettings.Default.Save()
        Label7.Visible = False
    End Sub
End Class