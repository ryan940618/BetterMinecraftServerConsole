Public Class Form3

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click
        Label37.Text = "false: Players in air for at least 5 seconds get kicked." & vbNewLine & "true: Players in air for at least 5 seconds won't get kicked." & vbNewLine & vbNewLine & "Has no effect in Creative mode."
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        Label37.Text = "The time in milliseconds for server watchdog to detect and stops the server." & vbNewLine & "The default is 60000 milliseconds(60 seconds)" & vbNewLine & ", means if the server lagged(delayed) for over 60 second, watchdog will close the server." & vbNewLine & vbNewLine & "The maximum is 0.05" & vbNewLine & "-1: disable watchdog , this option was added in 14w32a(1.8 snapshot)"
    End Sub
End Class