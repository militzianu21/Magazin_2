Imports MySql.Data.MySqlClient
Imports System.IO
Public Class Form_input

    Private Sub Form_input_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Button1.DialogResult = Windows.Forms.DialogResult.OK
        Button2.DialogResult = Windows.Forms.DialogResult.Cancel
        Dim valu As String = Me.Text

        TextBox1.SelectAll()
        TextBox1.Focus()
        Dim setare As String = Label1.Text
        Dim valoare As String = TextBox1.Text
    End Sub

    Private Sub _Keypress(ByVal sender As System.Object, ByVal e As KeyPressEventArgs) Handles TextBox1.KeyPress, MyBase.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If
    End Sub


    Private Sub browse_Button_Click(sender As Object, e As EventArgs) Handles browse_Button.Click
        SaveFileDialog1.Title = "Selectati Folderul"
        SaveFileDialog1.FileName = "Selectati Folderul"
        SaveFileDialog1.Filter = "pdf Files (*.pdf*)|*.pdf"

        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim folder As String = System.IO.Path.GetDirectoryName(SaveFileDialog1.FileName) & "\"
 
            TextBox1.Text = folder
        End If
    End Sub


    Private Sub Form_input_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class