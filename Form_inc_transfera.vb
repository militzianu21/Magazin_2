Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization
Imports System.IO
Public Class Form_inc_transfera
    Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Public AppDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
    Private Sub save_Bu_Click(sender As Object, e As EventArgs) Handles save_Bu.Click
        Application.CurrentCulture = New CultureInfo("ro-RO")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("ro-RO")
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
        Dim sql As String = "UPDATE incasari SET suma_cash=@suma_cash,suma_card=@suma_card WHERE id=@id"
        Try
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql, dbconn)
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()

            dbcomm.Parameters.AddWithValue("@suma_cash", CDec(cash_TB.Text))
            dbcomm.Parameters.AddWithValue("@suma_card", CDec(pos_TB.Text))
            dbcomm.Parameters.AddWithValue("@id", id_Lbl.Text)
            dbcomm.ExecuteNonQuery()
            transaction.Commit()
            dbconn.Close()
        Catch ex As Exception
            MsgBox("Failed to insert data: " & ex.Message.ToString())
        End Try

        With Form_principal
            .Load_Incasari()
            .Incasari_DGV.ClearSelection()

            For i = 0 To .Incasari_DGV.Rows.Count - 1
                If .Incasari_DGV.Rows(i).Cells("id").Value.ToString = id_Lbl.Text Then
                    .Incasari_DGV.Rows(i).Selected = True
                    If i > 0 Then
                        .Incasari_DGV.FirstDisplayedScrollingRowIndex = i - 1
                    Else : .Incasari_DGV.FirstDisplayedScrollingRowIndex = i
                    End If
                    Exit For
                End If
            Next
        End With
        If Form_Incasari.Visible = True Then
            With Form_Incasari
                .Load_DGV()
                .DataGridView1.ClearSelection()

                For i = 0 To .DataGridView1.Rows.Count - 1
                    If .DataGridView1.Rows(i).Cells("id").Value.ToString = id_Lbl.Text Then
                        .DataGridView1.Rows(i).Selected = True
                        If i > 0 Then
                            .DataGridView1.FirstDisplayedScrollingRowIndex = i - 1
                        Else : .DataGridView1.FirstDisplayedScrollingRowIndex = i
                        End If
                        Exit For
                    End If
                Next
            End With
        End If
        Me.Dispose()
    End Sub
    Private Sub TextBox_Suma_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cash_TB.KeyPress, pos_TB.KeyPress
        If Not Char.IsDigit(e.KeyChar) Then e.Handled = True
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = "." Or e.KeyChar = ",") And (sender.Text.IndexOf(".") = -1 And sender.Text.IndexOf(",") = -1)))
        If Asc(e.KeyChar) = 46 Then
            e.KeyChar = ChrW(44)
        End If
    End Sub

    Private Sub cash_TB_TextChanged(sender As Object, e As EventArgs) Handles cash_TB.TextChanged
        If cash_TB.Text = "" Then
            cash_TB.Text = 0
            cash_TB.Select()

        End If
        pos_TB.Text = suma_Lbl.Text - cash_TB.Text

    End Sub

    Private Sub pos_TB_TextChanged(sender As Object, e As EventArgs) Handles pos_TB.TextChanged
        If pos_TB.Text = "" Then
            pos_TB.Text = 0
            pos_TB.Select()

        End If
        cash_TB.Text = suma_Lbl.Text - pos_TB.Text
    End Sub

    Private Sub enter_Keypress(ByVal sender As System.Object, ByVal e As KeyPressEventArgs) Handles cash_TB.KeyPress, pos_TB.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If

    End Sub

    Private Sub Form_inc_transfera_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cash_TB.SelectAll()
        cash_TB.Focus()
    End Sub
End Class