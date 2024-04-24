Imports MySql.Data.MySqlClient
Imports System.IO

Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing.Layout
Public Class Form_setari
    Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Public AppDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
    Private Sub Form_setari_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' -------------------- LOAD setari INTO GRID
        Using dbconn As New mysqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New mysqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim setari_sql As String = "SELECT setare,valoare FROM setari"

            dbcomm = New MySqlCommand(setari_sql, dbconn)

            Try
                sda.SelectCommand = dbcomm
                sda.Fill(dbdataset)
                bsource.DataSource = dbdataset
                DataGridView1.DataSource = bsource
                sda.Update(dbdataset)
            Catch ex As Exception
                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
        End Using
    End Sub

    Private Sub ModificaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModificaToolStripMenuItem.Click
        Dim cell As DataGridViewCell = DataGridView1.CurrentCell
        Dim i As Integer = cell.ColumnIndex
        If i > -1 Then
            Dim col_name = DataGridView1.Columns(i).HeaderText
            Dim row As DataGridViewRow = DataGridView1.CurrentRow

            Dim setare As String = ""
            Dim valoare As String = ""
            
            setare = row.Cells("setare").Value.ToString
            valoare = row.Cells("valoare").Value.ToString

            With Form_input
                .Label2.Text = row.Cells(0).Value.ToString
                .Text = "MODIFICA"
                .TextBox1.Text = row.Cells(1).Value.ToString
                Dim valu As String = row.Cells(1).Value.ToString
            End With
            Form_input.ShowDialog()
            If Form_input.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim setting As String = Form_input.Label2.Text
                Dim value As String = Form_input.TextBox1.Text
                Dim sql_upd As String = "UPDATE setari SET valoare = @valoare WHERE setare=@setare"
                    Try
                        Dim dbconn As New MySqlConnection

                        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
                        If dbconn.State = 0 Then
                            dbconn.Open()
                    End If
                    Using dbcomm As MySqlCommand = New MySqlCommand(sql_upd, dbconn)
                        dbcomm.Parameters.AddWithValue("@valoare", value)
                        dbcomm.Parameters.AddWithValue("@setare", setting)
                        dbcomm.ExecuteNonQuery()
                    End Using
                Catch ex As Exception
                    MsgBox("Nu s-a sters: " & ex.Message.ToString)
                End Try

                End If
            Else : MsgBox("Nu s-a modificat nimic")
            End If
        Form_input.Dispose()
        Reload()


    End Sub

    Private Sub Reload()
        ' -------------------- LOAD setari INTO GRID
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim setari_sql As String = "SELECT setare,valoare FROM setari"

            dbcomm = New MySqlCommand(setari_sql, dbconn)

            Try
                sda.SelectCommand = dbcomm
                sda.Fill(dbdataset)
                bsource.DataSource = dbdataset
                DataGridView1.DataSource = bsource
                sda.Update(dbdataset)
            Catch ex As Exception
                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
        End Using
    End Sub

    Private Sub DeschideFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeschideFolderToolStripMenuItem.Click
        Dim setare As String = ""
        Dim valoare As String = ""
        Dim cell As DataGridViewCell = DataGridView1.CurrentCell
        Dim i As Integer = cell.ColumnIndex
        If i > -1 Then
            Dim col_name = DataGridView1.Columns(i).HeaderText
            Dim row As DataGridViewRow = DataGridView1.CurrentRow

            setare = row.Cells("setare").Value.ToString
            valoare = row.Cells("valoare").Value.ToString
        End If
        Process.Start(valoare)
    End Sub

    Private Sub AdaugaSetareToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdaugaSetareToolStripMenuItem.Click

        Dim setare As String = InputBox("Adauga Setare", "Setare")

        With Form_input
            .Text = "Adauga Path"
            .Label2.Text = setare
        End With
        Form_input.ShowDialog()
        If Form_input.DialogResult = Windows.Forms.DialogResult.OK Then
            ' Dim setting As String = Form_input.Label2.Text
            Dim value As String = Form_input.TextBox1.Text
            Dim sql_upd As String = "INSERT INTO setari (valoare,setare) VALUES(@valoare,@setare)"
            Try
                Dim dbconn As New MySqlConnection

                dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
                If dbconn.State = 0 Then
                    dbconn.Open()
                End If
                Using dbcomm As MySqlCommand = New MySqlCommand(sql_upd, dbconn)
                    dbcomm.Parameters.AddWithValue("@valoare", value)
                    dbcomm.Parameters.AddWithValue("@setare", setare)
                    dbcomm.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                MsgBox("Nu s-a sters: " & ex.Message.ToString)
            End Try

            'End If
        Else : MsgBox("Nu s-a modificat nimic")
        End If
        Form_input.Dispose()
        Reload()
    End Sub

    'Public Sub Form_setari_Leave(sender As Object, e As EventArgs) Handles MyBase.Leave

    '    Form_principal.Load_Setari()


    'End Sub
End Class
