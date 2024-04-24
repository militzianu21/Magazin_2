Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization
Public Class Form_verif_casa
    Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Public dbread2 As MySqlDataReader
    Public AppDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)

    Private Sub Form_verif_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Alege Data Registru/Raport"
        Application.CurrentCulture = New CultureInfo("ro-RO")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("ro-RO")

        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()
        For m = 1 To 12
            ComboBox1.Items.Add(m)
        Next
        For y = 2013 To Today.Year
            ComboBox2.Items.Add(y)
        Next

        If Today.Day > 5 Then
            ComboBox1.SelectedItem = Today.Month
        Else
            ComboBox1.SelectedItem = Today.Month - 1
        End If
        ComboBox2.SelectedItem = Today.Year
        Label1.Text = MonthName(ComboBox1.SelectedItem) & " " & ComboBox2.SelectedItem

        ComboBox3.DisplayMember = "Text"
        ComboBox3.ValueMember = "Value"
        Dim tb As New DataTable
        tb.Columns.Add("Text", GetType(String))
        tb.Columns.Add("Value", GetType(String))
        tb.Rows.Add("1. Petru Maior", "PM")
        tb.Rows.Add("2. Mihai Viteazu", "MV")
        ComboBox3.DataSource = tb

        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        Dim luna_reg As Date = Date.Parse(Date.DaysInMonth(ComboBox2.SelectedItem, ComboBox1.SelectedItem) & "." & ComboBox1.SelectedItem & "." & ComboBox2.SelectedItem)
        Dim luna_prec As Date = DateAdd(DateInterval.Month, -1, luna_reg)
        Dim data_ultima As Date = Date.Parse(Date.DaysInMonth(luna_prec.Year, luna_prec.Month) & "." & luna_prec.Month & "." & luna_prec.Year)

        Dim sql_read As String = "SELECT * FROM solduri_casa WHERE data=@data AND magazin='" & ComboBox3.SelectedValue & "'"
        Try
            dbconn.Open()
            dbcomm = New MySqlCommand(sql_read, dbconn)
            dbcomm.Parameters.AddWithValue("@data", data_ultima)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows Then
                TextBox1.Text = (dbread("casa_sold_final").ToString)
            End If

        Catch ex As Exception
            MsgBox("Problem loading data: " & ex.Message.ToString)

        End Try
        dbread.Close()
        '------------solduri luna Registru
        Try

            dbcomm = New MySqlCommand("SELECT * FROM solduri_casa WHERE data=@data AND magazin='" & ComboBox3.SelectedValue & "'", dbconn)
            dbcomm.Parameters.AddWithValue("@data", luna_reg)
            dbread2 = dbcomm.ExecuteReader()
            dbread2.Read()
            If dbread2.HasRows Then
                If dbread2("permanent") = True Then
                    TextBox1.Enabled = False
                    If TextBox1.Text = "0" Then
                        TextBox1.Text = (dbread2("casa_sold_initial").ToString)
                    End If
                ElseIf dbread2("permanent") = False Then
                    TextBox1.Enabled = True
                End If
            Else
                TextBox1.Enabled = True
            End If
        Catch ex As Exception
            MsgBox("Problem loading data registru: " & ex.Message.ToString)
            'dbread2.Close()
        End Try
        dbread2.Close()
        dbconn.Close()
        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
        End If
    End Sub
    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If Not Char.IsDigit(e.KeyChar) Then e.Handled = True
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = "." Or e.KeyChar = ",") And (sender.Text.IndexOf(".") = -1 And sender.Text.IndexOf(",") = -1)))
        If Asc(e.KeyChar) = 46 Then
            e.KeyChar = ChrW(44)
        End If
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If
    End Sub
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text = "" Then
            TextBox1.Text = 0
        End If
    End Sub

    Private Sub ComboBox_1_2_3_DropDownClosed(sender As Object, e As EventArgs) Handles ComboBox1.DropDownClosed, ComboBox2.DropDownClosed, ComboBox3.DropDownClosed

        Label1.Text = MonthName(ComboBox1.SelectedItem) & " " & ComboBox2.SelectedItem
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")

        Dim luna_reg As Date = Date.Parse(Date.DaysInMonth(ComboBox2.SelectedItem, ComboBox1.SelectedItem) & "." & ComboBox1.SelectedItem & "." & ComboBox2.SelectedItem)
        Dim luna_prec As Date = DateAdd(DateInterval.Month, -1, luna_reg)
        Dim data_ultima As Date = Date.Parse(Date.DaysInMonth(luna_prec.Year, luna_prec.Month) & "." & luna_prec.Month & "." & luna_prec.Year)

        Dim sql_read As String = "SELECT * FROM solduri_casa WHERE data=@data and magazin='" & ComboBox3.SelectedValue & "'"
        Try

            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_read, dbconn)
            dbcomm.Parameters.AddWithValue("@data", data_ultima)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows Then
                TextBox1.Text = (dbread("casa_sold_final").ToString)
            Else
                TextBox1.Text = 0
            End If
        Catch ex As Exception
            MsgBox("Problem loading data prec: " & ex.Message.ToString)


        End Try
        dbread.Close()
        '------------solduri luna RAPORT
        Try
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If

            'MsgBox(luna_rap)
            dbcomm = New MySqlCommand(sql_read, dbconn)
            dbcomm.Parameters.AddWithValue("@data", luna_reg)
            dbread2 = dbcomm.ExecuteReader()
            dbread2.Read()
            If dbread2.HasRows Then
                If dbread2("permanent") = True Then
                    TextBox1.Enabled = False
                    If TextBox1.Text = 0 Then
                        TextBox1.Text = (dbread2("casa_sold_initial").ToString)
                        'MsgBox((dbread2("gestiune_sold_initial").ToString))
                    End If
                ElseIf dbread2("permanent") = False Then
                    TextBox1.Enabled = True
                End If
            Else
                TextBox1.Enabled = True
            End If

        Catch ex As Exception
            MsgBox("Problem loading data raport: " & ex.Message.ToString)
            dbread2.Close()
        End Try
        'dbread2.Close()
        'TextBox2.Enabled = True
        'TextBox2.Text = 0
        TextBox1.Select()
        TextBox1.Focus()
        dbconn.Close()
        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
        End If

    End Sub

    Private Sub verif_But_Click(sender As Object, e As EventArgs) Handles verif_But.Click


    End Sub
End Class
