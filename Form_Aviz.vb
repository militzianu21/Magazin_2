Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization
Public Class Form_Aviz
    Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Private Sub Form_Aviz_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
        Dim sql_read As String = "SELECT nr_nir,data,tip_document FROM intrari WHERE tip_document='NIR' ORDER BY data DESC,nr_nir DESC LIMIT 1"
        Try
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_read, dbconn)

            dbread = dbcomm.ExecuteReader()
            dbread.Read()

            nr_nir_Textbox.Text = (dbread("nr_nir").ToString) + 1

            data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)


        Catch ex As Exception
            MsgBox("Problem loading lables (form load): " & ex.Message.ToString)

        End Try
        dbread.Close()
        dbconn.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim tip_document As String = "NIR"
        Dim nr_nir As String = nr_nir_Textbox.Text
        Dim explicatii As String = "Transfer Marfa"
        Dim suma As String = suma_Textbox.Text
        If suma = "" Then
            suma = 0
        End If
        If suma = 0 Then
            Dim yesno As Integer = MsgBox("Suma introdusa este 0(zero). Vrei sa introduci??", MsgBoxStyle.YesNo)
            If yesno = DialogResult.No Then
                Exit Sub
            ElseIf yesno = DialogResult.Yes Then
            End If
        End If
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
        Dim sql As String = "INSERT INTO intrari(data,tip_document,nr_nir,explicatii,suma,magazin) " _
                            & "VALUES(@data,@tip_document,@nr_nir,@explicatii,@suma,@magazin)"
        Try
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql, dbconn)
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value.Date)
            dbcomm.Parameters.AddWithValue("@tip_document", tip_document)
            dbcomm.Parameters.AddWithValue("@nr_nir", nr_nir)
            dbcomm.Parameters.AddWithValue("@explicatii", explicatii)
            dbcomm.Parameters.AddWithValue("@suma", CDec(suma))
            dbcomm.Parameters.AddWithValue("@magazin", Label1.Text)
            dbcomm.ExecuteNonQuery()
            transaction.Commit()
        Catch ex As Exception
            MsgBox("Failed to insert data: " & ex.Message.ToString())
        End Try
        dbconn.Close()
        Me.Close()
    End Sub

End Class