Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization
Public Class Form_firme_introd
    Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Private Sub Form_firme_introd_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.KeyPreview = True

        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
        dbcomm = New MySqlCommand("SELECT forma_juridica, count(forma_juridica) FROM firme GROUP BY forma_juridica ORDER BY count(forma_juridica) DESC", dbconn)
        If dbconn.State = ConnectionState.Closed Then
            dbconn.Open()
        End If
        Dim lst As New List(Of String)
        dbread = dbcomm.ExecuteReader()
        While dbread.Read()
            lst.Add(dbread("forma_juridica").ToString())
        End While
        Dim mysource As New AutoCompleteStringCollection
        mysource.AddRange(lst.ToArray)
        forma_juridica_text.AutoCompleteSource = AutoCompleteSource.CustomSource
        forma_juridica_text.AutoCompleteCustomSource = mysource
        forma_juridica_text.AutoCompleteMode = AutoCompleteMode.SuggestAppend

        forma_juridica_text.Text = "S.R.L."
    End Sub

    Private Sub cui_textbox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cui_text.KeyPress ', TextBox2.KeyPress

        If Not Char.IsDigit(e.KeyChar) Then e.Handled = True
        If e.KeyChar = Chr(8) OrElse e.KeyChar = Chr(45) Then e.Handled = False 'allow Backspace and minus sign

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cui_text.TextChanged

        If Verifica_CIF(cui_text.Text) = False And Verifica_CNP(cui_text.Text) = False Then
            cui_text.ForeColor = Color.Red
        Else : cui_text.ForeColor = Color.Black
        End If
    End Sub
   
 
    Private Sub firma_text_Enter(sender As Object, e As EventArgs) Handles firma_text.Enter
        CAPS_ON()
    End Sub

    Private Sub firma_text_LEAVE(sender As Object, e As EventArgs) Handles firma_text.Leave
        CAPS_OFF()
    End Sub

    Private Sub main_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If (e.KeyCode = Keys.Return AndAlso e.Modifiers = Keys.Control) Then
            Button1.PerformClick()
        End If
    End Sub
    Private Sub forma_juridica_text_Keydown(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles forma_juridica_text.KeyDown
        If e.KeyValue = 13 Then
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If

    End Sub
    Private Sub enter_Keypress(ByVal sender As System.Object, ByVal e As KeyPressEventArgs) Handles firma_text.KeyPress, cui_text.KeyPress, j_text.KeyPress, adresa_text.KeyPress, status_text.KeyPress, tip_text.KeyPress, banca_text.KeyPress, cont_text.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If

    End Sub

    
End Class