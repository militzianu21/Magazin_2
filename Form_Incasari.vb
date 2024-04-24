Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization
Imports System.IO

Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing.Layout
Public Class Form_Incasari
    Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Public AppDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
    Public folder_di As String

    Private Sub Form_Incasari_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Text = "Incasari"
        Application.CurrentCulture = New CultureInfo("ro-RO")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("ro-RO")
        Dim DGV As DataGridView = DataGridView1
        DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGV.ScrollBars = ScrollBars.Vertical
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.AllowUserToAddRows = False
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        '------------------
        If dbconn.State = ConnectionState.Closed Then
            dbconn.Open()
        End If
        Try
            Dim set_sql As String = "SELECT * FROM setari WHERE setare='path_di'"

            dbcomm = New MySqlCommand(set_sql, dbconn)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = True Then
                folder_di = dbread("valoare")
            End If

        Catch ex As Exception
            MsgBox("Problem loading setari: path raport: " & ex.Message.ToString)
        End Try
        dbread.Close()

        id_Lbl.Text = ""
        edit_Lbl.Text = ""
        mag_v_Lbl.Text = ""
        rzf_v_Lbl.Text = ""
        GroupBox1.Visible = False

        '-------------------
        ComboBox3.DisplayMember = "Text"
        ComboBox3.ValueMember = "Value"
        Dim tb As New DataTable
        tb.Columns.Add("Text", GetType(String))
        tb.Columns.Add("Value", GetType(String))
        tb.Rows.Add("1. Petru Maior", "PM")
        tb.Rows.Add("2. Mihai Viteazu", "MV")
        ComboBox3.DataSource = tb
        ComboBox3.SelectedValue = Form_principal.ComboBox3.SelectedValue

        ComboBox1.Items.AddRange({"RZF", "CH", "DI", "CRN"})
        ComboBox1.SelectedItem = "RZF"
        explicatii_Textbox.Text = "Incasari Vanzari"
        CheckBox1.CheckState = CheckState.Checked

        CheckBox2.CheckState = CheckState.Checked

        Load_DGV()



        Dim sql_read As String = "SELECT nr_rzf,data FROM incasari where tip_incasare='RZF' AND incasari.magazin='" & ComboBox3.SelectedValue & "' ORDER BY DATA DESC LIMIT 1"
        Try
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_read, dbconn)

            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            'If dbread.HasRows Then
            nr_rzf_Textbox.Text = (dbread("nr_rzf").ToString.Substring(2)) + 1
            Dim ultimazi As Date = CDate(dbread("data"))
            If ultimazi.DayOfWeek = DayOfWeek.Saturday Then
                DateTimePicker1.Value = ultimazi.AddDays(2)
            Else
                DateTimePicker1.Value = ultimazi.AddDays(1)
            End If
            data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
            'End If

        Catch ex As Exception
            MsgBox("Problem loading lables (form load): " & ex.Message.ToString)

        End Try
        dbread.Close()
        dbconn.Close()


        suma_CASA_Textbox.Text = "0"
        With suma_CASA_Textbox
            .Focus()
            .Select()
        End With
        suma_CARD_Textbox.Text = "0"
        total_label.Text = "Total: " & (CDec(suma_CASA_Textbox.Text) + CDec(suma_CARD_Textbox.Text))
        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
        Else : PictureBox1.BackColor = Color.LightYellow
        End If
        DataGridView1.Columns("magazin").Visible = False
    End Sub
    Private Sub Form_Incasari_Leave(sender As Object, e As EventArgs) Handles Me.FormClosed
        Form_principal.Show()
        Form_principal.Load_Incasari()
        Form_principal.Load_Cheltuieli()
        Form_principal.Load_Situatie()
    End Sub

    Public Sub Load_DGV()
        Dim DGV As DataGridView = DataGridView1
        DGV.ContextMenuStrip = ContextMenuStrip1

        Dim limit As String = ""
        Dim data_ult As Date = Today
        
        If CheckBox2.CheckState = CheckState.Checked Then
            If Today.Day < 6 Then
                data_ult = DateAdd(DateInterval.Month, -1, Today)
            End If
            limit = "AND YEAR(data) = @anul_ult AND MONTH(data) = @luna_ult"
        End If
        If CheckBox2.CheckState = CheckState.Unchecked Then
            limit = ""
        End If
        Dim anul_ult As Integer = data_ult.Year
        Dim luna_ult As Integer = data_ult.Month
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sql_tot As String = "SELECT id,data,nr_rzf,tip_incasare,explicatii,suma_cash as Cash,suma_card as POS,magazin FROM incasari WHERE magazin='" & ComboBox3.SelectedValue & "' " & limit & " ORDER BY incasari.data DESC,incasari.id DESC"
            'Dim sql_tot As String = "SELECT * FROM incasari ORDER BY incasari.data DESC"
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource
           
            Try
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(sql_tot, dbconn)
                dbcomm.Parameters.AddWithValue("@anul_ult", anul_ult)
                dbcomm.Parameters.AddWithValue("@luna_ult", luna_ult)
                sda.SelectCommand = dbcomm
                sda.Fill(dbdataset)
                bsource.DataSource = dbdataset
                DataGridView1.DataSource = bsource
                sda.Update(dbdataset)
            Catch ex As Exception
                MsgBox("Problem loading grid (form load): " & ex.Message.ToString)
            End Try

            DataGridView1.Columns("id").Visible = False
            DataGridView1.Columns("magazin").Visible = False

            DGV.Columns("data").FillWeight = DGV.Width * 20 / 100
            DGV.Columns("data").HeaderText = "Data"
            DGV.Columns("data").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DGV.Columns("nr_rzf").FillWeight = DGV.Width * 15 / 100
            DGV.Columns("nr_rzf").HeaderText = "Nr. RZF"
            DGV.Columns("nr_rzf").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DGV.Columns("tip_incasare").FillWeight = DGV.Width * 10 / 100
            DGV.Columns("tip_incasare").HeaderText = "Tip Incas"
            DGV.Columns("tip_incasare").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DGV.Columns("explicatii").FillWeight = DGV.Width * 25.5 / 100
            DGV.Columns("explicatii").HeaderText = "Explicatii"
            DGV.Columns("explicatii").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DGV.Columns("Cash").FillWeight = DGV.Width * 13 / 100
            DGV.Columns("Cash").HeaderText = "Cash"
            DGV.Columns("Cash").DefaultCellStyle.Format = "#,#0.##"
            DGV.Columns("Cash").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DGV.Columns("POS").FillWeight = DGV.Width * 14 / 100
            DGV.Columns("POS").HeaderText = "POS"
            DGV.Columns("POS").DefaultCellStyle.Format = "#,#0.##"
            DGV.Columns("POS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter


            Try
                Dim sql_read As String = "SELECT sum(suma_cash),data FROM incasari WHERE magazin='" & ComboBox3.SelectedValue & "' " & limit & " "
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(sql_read, dbconn)
                dbcomm.Parameters.AddWithValue("@anul_ult", anul_ult)
                dbcomm.Parameters.AddWithValue("@luna_ult", luna_ult)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If IsDBNull(dbread("sum(suma_cash)")) = False Then
                    Label2.Text = CDec(dbread("sum(suma_cash)")) ' + CDec(suma_CASA_Textbox.Text) + CDec(suma_CARD_Textbox.Text)
                Else
                    Label2.Text = "0"
                End If

            Catch ex As Exception
                MsgBox("Problem loading lables total: " & ex.Message.ToString)

            End Try
            dbread.Close()

            Try
                Dim sql_read As String = "SELECT sum(suma_cash+suma_card) as suma,data FROM incasari WHERE incasari.magazin='" & ComboBox3.SelectedValue & "' AND tip_incasare='RZF'"
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(sql_read, dbconn)

                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If IsDBNull(dbread("suma")) = False Then
                    Label3.Text = CDec(dbread("suma")) '+ CDec(suma_CASA_Textbox.Text) + CDec(suma_CARD_Textbox.Text)
                Else
                    Label3.Text = "0"
                End If

            Catch ex As Exception
                MsgBox("Problem loading lables total an: " & ex.Message.ToString)

            End Try
            dbread.Close()
        End Using
    End Sub
    Private Sub save_BU_Click(sender As Object, e As EventArgs) Handles save_BU.Click

        data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
        Dim id As Integer = 0
        Dim tip_incasare As String = ComboBox1.SelectedItem
        Dim nr_rzf As String = nr_rzf_Textbox.Text
        Dim explicatii As String = explicatii_Textbox.Text
        Dim nr_rzf_vechi As String = rzf_v_Lbl.Text
        Dim magazin_vechi As String = mag_v_Lbl.Text
        Dim new_date
        Date.TryParse(data_ies_lbl.Text, new_date)
        Dim data_veche As Date = new_date

        Dim suma_casa As String = suma_CASA_Textbox.Text
        Dim suma_card As String = suma_CARD_Textbox.Text
        Dim suma_total = (CDec(suma_CASA_Textbox.Text) + CDec(suma_CARD_Textbox.Text))
        If suma_card = "" Then
            suma_card = 0
        End If
        If suma_casa = "" Then
            suma_casa = 0
        End If
        If suma_casa = 0 Then
            Dim yesno As Integer = MsgBox("Suma introdusa este 0(zero). Vrei sa introduci??", MsgBoxStyle.YesNo)
            If yesno = DialogResult.No Then
                Exit Sub
            ElseIf yesno = DialogResult.Yes Then
            End If
        End If

        If tip_incasare = "RZF" Then
            nr_rzf = "Z " & nr_rzf_Textbox.Text
            explicatii_Textbox.Text = "Incasari vanzari"
        End If
        If tip_incasare = "DI" Then
            explicatii_Textbox.Text = "Creditare Societate"
        End If
        If tip_incasare = "CRN" Then
            explicatii_Textbox.Text = "Retragere din cont curent"
        End If

        Dim sql_incasari As String = "INSERT INTO incasari(data,tip_incasare,nr_rzf,explicatii,suma_cash,suma_card,magazin) " _
                            & "VALUES(@data,@tip_incasare,@nr_rzf,@explicatii,@suma_cash,@suma_card,@magazin)"

        If edit_Lbl.Text = "Edit" Then
            sql_incasari = "UPDATE incasari SET data=@data,tip_incasare=@tip_incasare,nr_rzf=@nr_rzf,explicatii=@explicatii,suma_cash=@suma_cash,suma_card=@suma_card,magazin=@magazin WHERE id=@id"
        End If

        If dbconn.State = ConnectionState.Closed Then
            dbconn.Open()
        End If

        Try

            dbcomm = New MySqlCommand(sql_incasari, dbconn)
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value.Date)
            dbcomm.Parameters.AddWithValue("@tip_incasare", tip_incasare)
            dbcomm.Parameters.AddWithValue("@nr_rzf", nr_rzf)
            dbcomm.Parameters.AddWithValue("@explicatii", explicatii_Textbox.Text)
            dbcomm.Parameters.AddWithValue("@suma_cash", CDec(suma_casa))
            dbcomm.Parameters.AddWithValue("@suma_card", CDec(suma_card))
            dbcomm.Parameters.AddWithValue("@magazin", ComboBox3.SelectedValue)
            dbcomm.Parameters.AddWithValue("@id", id_Lbl.Text)

            dbcomm.ExecuteNonQuery()
            transaction.Commit()
        Catch ex As Exception
            MsgBox("Failed to insert data incasari: " & ex.Message.ToString())
        End Try

        If tip_incasare <> "DI" AndAlso tip_incasare <> "CRN" Then
            Dim sql_iesiri As String = "INSERT INTO iesiri(data,tip_incasare,nr_rzf,explicatii,suma,magazin) " _
                                & "VALUES(@data,@tip_incasare,@nr_rzf,@explicatii,@suma,@magazin)"

            If edit_Lbl.Text = "Edit" Then

                sql_iesiri = "UPDATE iesiri SET data=@data,tip_incasare=@tip_incasare,nr_rzf=@nr_rzf,explicatii=@explicatii,suma=@suma,magazin=@magazin WHERE nr_rzf='" & nr_rzf_vechi & "' AND magazin='" & magazin_vechi & "' AND data=@data_veche"

            End If
            Try
                ' dbconn.Open()
                dbcomm = New MySqlCommand(sql_iesiri, dbconn)
                Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
                dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value.Date)
                dbcomm.Parameters.AddWithValue("@data_veche", data_veche)
                dbcomm.Parameters.AddWithValue("@tip_incasare", tip_incasare)
                dbcomm.Parameters.AddWithValue("@nr_rzf", nr_rzf)
                dbcomm.Parameters.AddWithValue("@explicatii", explicatii_Textbox.Text)
                dbcomm.Parameters.AddWithValue("@suma", CDec(suma_total))
                dbcomm.Parameters.AddWithValue("@magazin", ComboBox3.SelectedValue)
                dbcomm.ExecuteNonQuery()
                transaction.Commit()
            Catch ex As Exception
                MsgBox("Failed to insert data iesiri: " & ex.Message.ToString())
            End Try
        End If
        
        'dbconn.Close()

        Load_DGV()
        'Dim nr_rzf As String = "Z " & nr_rzf_Textbox.Text
        Dim rzf As String = DataGridView1.Rows(0).Cells("nr_rzf").Value.ToString.Substring(2)
        Dim exista As Boolean = False
        For i = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Cells("nr_rzf").Value = nr_rzf Or nr_rzf_Textbox.Text > rzf Then
                exista = True
                Exit For
            End If
        Next

        If exista = False Then
            If CheckBox2.Checked = True Then
                CheckBox2.Checked = False
            End If
        End If
        For i = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Cells("nr_rzf").Value = nr_rzf Then
                DataGridView1.ClearSelection()
                DataGridView1.Rows(i).Selected = True
                If i > 0 Then
                    DataGridView1.FirstDisplayedScrollingRowIndex = i - 1
                Else
                    DataGridView1.FirstDisplayedScrollingRowIndex = i
                End If

                Exit For
            End If
        Next

        If ComboBox3.SelectedValue = "PM" AndAlso DateTimePicker1.Value.DayOfWeek = DayOfWeek.Saturday Then
            DateTimePicker1.Value = DateTimePicker1.Value.AddDays(2)
            'ElseIf ComboBox3.SelectedValue = "MV" AndAlso DateTimePicker1.Value.DayOfWeek = DayOfWeek.Friday Then      '' Se sare peste ziua de
            'DateTimePicker1.Value = DateTimePicker1.Value.AddDays(3)                                                   '' Sambata
        ElseIf ComboBox3.SelectedValue = "MV" AndAlso DateTimePicker1.Value.DayOfWeek = DayOfWeek.Saturday Then
            DateTimePicker1.Value = DateTimePicker1.Value.AddDays(2)
        Else
            DateTimePicker1.Value = DateTimePicker1.Value.AddDays(1)
        End If

        id_Lbl.Text = ""
        edit_Lbl.Text = ""
        mag_v_Lbl.Text = ""
        rzf_v_Lbl.Text = ""
        GroupBox1.Visible = False

        nr_rzf_Textbox.Text = nr_rzf_Textbox.Text + 1
        suma_CARD_Textbox.Text = 0
        suma_CASA_Textbox.Text = 0
        suma_CASA_Textbox.Select()
        suma_CASA_Textbox.Focus()
        total_label.Text = "Total: " & (CDec(suma_CASA_Textbox.Text) + CDec(suma_CARD_Textbox.Text))

        Dim DGV As DataGridView = DataGridView1
        For i = 0 To DGV.Rows.Count - 1
            If DGV.Rows(i).Cells("nr_rzf").Value = nr_rzf Then
                DGV.ClearSelection()
                DGV.Rows(i).Selected = True
                If i > 0 Then
                    DGV.FirstDisplayedScrollingRowIndex = i - 1
                Else
                    DGV.FirstDisplayedScrollingRowIndex = i
                End If
                Exit For
            End If
        Next
    End Sub
    Private Sub TextBox_Suma_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles suma_CASA_Textbox.KeyPress, suma_CARD_Textbox.KeyPress
        If Not Char.IsDigit(e.KeyChar) Then e.Handled = True
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = "." Or e.KeyChar = ",") And (sender.Text.IndexOf(".") = -1 And sender.Text.IndexOf(",") = -1)))
        If Asc(e.KeyChar) = 46 Then
            e.KeyChar = ChrW(44)
        End If
    End Sub
    Private Sub TextBox_NrRZF_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles nr_rzf_Textbox.KeyPress
        If ComboBox1.SelectedItem = "RZF" Then
            If Not Char.IsDigit(e.KeyChar) Then e.Handled = True
            e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8)
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedValueChanged ', ComboBox1.Leave ',ComboBox1.DropDownClosed, 
        Dim tip_incasare As String = ComboBox1.SelectedItem

        Dim nr_rzf As String = nr_rzf_Textbox.Text
        Dim explicatii As String = explicatii_Textbox.Text
        Dim suma As String = suma_CASA_Textbox.Text
        If tip_incasare = "RZF" Then
            suma_CARD_Textbox.Enabled = True
            If CheckBox1.CheckState = CheckState.Checked Then
                data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
                explicatii_Textbox.Text = "Incasari vanzari"
                Dim sql_read As String = "SELECT nr_rzf,data FROM incasari where tip_incasare='RZF' AND data<@data AND magazin='" & ComboBox3.SelectedValue & "' ORDER BY data DESC LIMIT 1"
                Try
                    If dbconn.State = ConnectionState.Closed Then
                        dbconn.Open()
                    End If
                    dbcomm = New MySqlCommand(sql_read, dbconn)
                    dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value)
                    dbread = dbcomm.ExecuteReader()
                    dbread.Read()
                    If dbread.HasRows Then
                        nr_rzf_Textbox.Text = (dbread("nr_rzf").ToString.Substring(2)) + 1
                        DateTimePicker1.Value = CDate(dbread("data")).AddDays(1)
                        suma_CASA_Textbox.Select()
                        suma_CASA_Textbox.Focus()
                    End If
                Catch ex As Exception
                    MsgBox("Problem loading data: " & ex.Message.ToString)
                End Try

            End If
            If CheckBox1.CheckState = CheckState.Unchecked Then
                data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
                explicatii_Textbox.Text = "Incasari vanzari"
            End If
            dbread.Close()
        End If

        If tip_incasare = "DI" Then
            suma_CARD_Textbox.Enabled = False
            Dim sql_read As String = "SELECT nr_rzf,data FROM incasari where tip_incasare='DI' AND data<@data ORDER BY data DESC LIMIT 1"
            Try
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(sql_read, dbconn)
                dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If dbread.HasRows Then
                    nr_rzf_Textbox.Text = dbread("nr_rzf").ToString + 1
                    DateTimePicker1.Select()
                    DateTimePicker1.Focus()
                Else : nr_rzf_Textbox.Text = 1
                    nr_rzf_Textbox.Select()
                    nr_rzf_Textbox.Focus()
                End If
            Catch ex As Exception
                MsgBox("Problem loading data: " & ex.Message.ToString)
            End Try
            data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
            explicatii_Textbox.Text = "Creditare Societate"
            dbread.Close()
        End If

        If tip_incasare = "CH" Then
            suma_CARD_Textbox.Enabled = True
            explicatii_Textbox.Clear()
            nr_rzf_Textbox.Clear()
        End If
        If tip_incasare = "CRN" Then
            suma_CARD_Textbox.Enabled = True
            explicatii_Textbox.Text = "Retragere din cont curent"
            nr_rzf_Textbox.Clear()
        End If
        dbconn.Close()
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        Dim row As DataGridViewRow = DataGridView1.CurrentRow
        If DataGridView1.Rows.Count > 0 Then
            If DataGridView1.CurrentRow.Index > -1 AndAlso DataGridView1.SelectedRows.Count <> 0 Then
                Dim tip_incasare As String = row.Cells("tip_incasare").Value.ToString()

                If tip_incasare = "DI" Then
                    PrinteazaDIToolStripMenuItem.Enabled = True
                Else : PrinteazaDIToolStripMenuItem.Enabled = False
                End If
            End If
        ElseIf DataGridView1.Rows.Count < 1 Then
            e.Cancel = True
        End If
    End Sub
    Private Sub StergeInregistrareaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StergeInregistrareaToolStripMenuItem.Click

        Dim yesno As Integer = MsgBox("Stergi Inregistrarea?", MsgBoxStyle.YesNo)

        Dim row As DataGridViewRow = DataGridView1.CurrentRow
        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then
            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            For i = 0 To DataGridView1.SelectedRows.Count - 1
                Dim id As Integer = DataGridView1.SelectedRows(i).Cells("id").Value
                Dim data As Date = CDate(DataGridView1.SelectedRows(i).Cells("data").Value)
                Dim nr_rzf As String = DataGridView1.SelectedRows(i).Cells("nr_rzf").Value.ToString
                Dim tip_incasare As String = DataGridView1.SelectedRows(i).Cells("tip_incasare").Value.ToString
                Dim explicatii As String = DataGridView1.SelectedRows(i).Cells("explicatii").Value

                Try
                    Dim sql_del_inc As String = "DELETE FROM incasari WHERE id=@id AND data=@data AND nr_rzf=@nr_rzf AND tip_incasare=@tip_incasare AND magazin='" & ComboBox3.SelectedValue & "'"
                    dbcomm = New MySqlCommand(sql_del_inc, dbconn)
                    dbcomm.Parameters.AddWithValue("@id", id)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@nr_rzf", nr_rzf)
                    dbcomm.Parameters.AddWithValue("@tip_incasare", tip_incasare)
                    dbcomm.Parameters.AddWithValue("@explicatii", explicatii)
                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Nu s-a sters din incasari: " & ex.Message.ToString)
                End Try

                Try
                    Dim sql_del_ies As String = "DELETE FROM iesiri WHERE data=@data AND nr_rzf=@nr_rzf AND tip_incasare=@tip_incasare AND magazin='" & ComboBox3.SelectedValue & "'"

                    dbcomm = New MySqlCommand(sql_del_ies, dbconn)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@nr_rzf", nr_rzf)
                    dbcomm.Parameters.AddWithValue("@tip_incasare", tip_incasare)
                    dbcomm.Parameters.AddWithValue("@explicatii", explicatii)
                    dbcomm.ExecuteNonQuery()
                    dbcomm.ExecuteNonQuery()

                Catch ex As Exception
                    MsgBox("Nu s-a sters din incasari: " & ex.Message.ToString)
                End Try
            Next
            transaction.Commit()

            Load_DGV()

        End If
       
    End Sub
    Private Sub PrinteazaDIToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrinteazaDIToolStripMenuItem.Click
        'Dim yesno As Integer = MsgBox("Printezi Dispozitia de incasare?", MsgBoxStyle.YesNo)
        'If yesno = DialogResult.No Then
        'ElseIf yesno = DialogResult.Yes Then
        Dim row As DataGridViewRow = DataGridView1.CurrentRow
        Dim data As String = row.Cells("data").Value
        Dim nr_rzf As String = row.Cells("nr_rzf").Value.ToString
        Dim tip_incasare As String = row.Cells("tip_incasare").Value.ToString
        Dim explicatii As String = row.Cells("explicatii").Value
        Dim suma As String = row.Cells("Cash").Value
        Dim maga As String = ""
        If row.Cells("magazin").Value = "PM" Then
            maga = " - Magazin: PETRU MAIOR 9"
        ElseIf row.Cells("magazin").Value = "MV" Then
            maga = " - Magazin: MIHAI VITEAZU 28"
        End If

        Dim pdf As PdfSharp.Pdf.PdfDocument = New PdfSharp.Pdf.PdfDocument
        pdf.Info.Title = "Dispozitie Incasare"
        Dim pdfPage As PdfSharp.Pdf.PdfPage = pdf.AddPage


        Dim graph As XGraphics = XGraphics.FromPdfPage(pdfPage)
        Dim pen As XPen = New XPen(Color.Black, 0.5)

        Dim titlu_font As XFont = New XFont("Verdana", 16, XFontStyle.Bold)
        Dim top_font As XFont = New XFont("Verdana", 12, XFontStyle.Bold)
        Dim mic_font As XFont = New XFont("Calibri", 10, XFontStyle.Regular)
        Dim report_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim text_font As XFont = New XFont("Calibri", 11, XFontStyle.Regular)
        Dim text_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim bottom_font As XFont = New XFont("Verdana", 12, XFontStyle.Bold)

        Dim incas_tot As Double = 0
        Dim chelt_tot As Double = 0
        'A4 = 8.27x11.69" x72points/inch = 595x842 points

        graph.DrawString("Unitatea: MILICOM CAZ SRL" & maga, text_font, XBrushes.Black, _
                         New XRect(45, 20, 100, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawLine(pen, 40, 35, 555, 35)

        graph.DrawString("DISPOZITIE DE INCASARE CATRE CASIERIE", text_font, XBrushes.Black, _
                         New XRect(45, 37, 300, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 40, 52, 555, 52)

        graph.DrawLine(pen, 40, 35, 40, 232)
        graph.DrawLine(pen, 555, 35, 555, 232)


        graph.DrawString("Nr. " & nr_rzf & " din " & data, text_font, XBrushes.Black, _
                         New XRect(45, 67, 300, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Numele si prenumele:", text_font, XBrushes.Black, _
                         New XRect(45, 97, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("CAZAN MIHAI", text_font, XBrushes.Black, _
                         New XRect(195, 97, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Functia (calitatea):", text_font, XBrushes.Black, _
                         New XRect(45, 112, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("ADMINISTRATOR", text_font, XBrushes.Black, _
                         New XRect(195, 112, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Suma:", text_font, XBrushes.Black, _
                         New XRect(45, 127, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(suma & " lei (" & NrCuv(suma) & " lei)", text_font, XBrushes.Black, _
                         New XRect(195, 127, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Scopul incasarii:", text_font, XBrushes.Black, _
                         New XRect(45, 142, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("CREDITARE SOCIETATE", text_font, XBrushes.Black, _
                         New XRect(195, 142, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawRectangle(pen, New XRect(40, 172, 75, 60))
        graph.DrawString("Semnatura", mic_font, XBrushes.Black, _
                         New XRect(40, 172, 75, 80), XStringFormats.TopCenter)

        graph.DrawRectangle(pen, New XRect(115, 172, 146, 20))
        graph.DrawString("Conducatorul unitatii", mic_font, XBrushes.Black, _
                         New XRect(117, 172, 146, 20), XStringFormats.Center)
        graph.DrawRectangle(pen, New XRect(115, 192, 146, 40))

        graph.DrawRectangle(pen, New XRect(261, 172, 146, 20))
        graph.DrawString("Viza de control financiar-preventiv", mic_font, XBrushes.Black, _
                         New XRect(261, 172, 146, 20), XStringFormats.Center)
        graph.DrawRectangle(pen, New XRect(261, 192, 146, 40))

        graph.DrawRectangle(pen, New XRect(407, 172, 148, 20))
        graph.DrawString("Compartiment financiar-contabil", mic_font, XBrushes.Black, _
                         New XRect(409, 172, 146, 20), XStringFormats.Center)
        graph.DrawRectangle(pen, New XRect(407, 192, 148, 40))


        graph.DrawLine(pen, 40, 247, 555, 247)


        graph.DrawString("CASIER,", text_font, XBrushes.Black, _
                         New XRect(45, 249, 300, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("Incasat suma de", text_font, XBrushes.Black, _
                        New XRect(45, 264, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(suma & " lei", text_font, XBrushes.Black, _
                         New XRect(195, 264, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Data:", text_font, XBrushes.Black, _
                         New XRect(45, 279, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(Format(CDate(row.Cells("data").Value), "dd MMMM yyyy"), text_font, XBrushes.Black, _
                         New XRect(195, 279, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("Semnatura:", text_font, XBrushes.Black, _
                        New XRect(315, 279, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawLine(pen, 40, 294, 555, 294)
        graph.DrawLine(pen, 40, 247, 40, 294)
        graph.DrawLine(pen, 555, 247, 555, 294)


        Dim pdfFilename As String = folder_di & "Dispozitie Incasare_" & nr_rzf.ToString.PadLeft(3, "0") & "_" & Format(CDate(data), "yyyyMMdd") & ".pdf"
        If System.IO.File.Exists(pdfFilename) = True Then
            Dim OkCancel As Integer = MsgBox("Fisierul exista. Inlocuiti?", MsgBoxStyle.OkCancel)
            If OkCancel = DialogResult.Cancel Then
                Exit Sub
            ElseIf OkCancel = DialogResult.OK Then
                pdf.Save(pdfFilename)
                Process.Start(pdfFilename)
            End If
        Else : pdf.Save(pdfFilename)
            Process.Start(pdfFilename)
        End If
        'End If
    End Sub
    Private Sub ModificaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModificaToolStripMenuItem.Click
        Dim mag_act As String = ""
        If IsDBNull(DataGridView1.SelectedRows(0).Cells("magazin").Value) = False Then
            mag_act = DataGridView1.SelectedRows(0).Cells("magazin").Value
            'Dim mag_nou As String = DataGridView1.SelectedRows(0).Cells("magazin").Value
            'If mag_act = "PM" Then
            '    mag_nou = "MV"
            'ElseIf mag_act = "MV" Then
            '    mag_nou = "PM"
            'End If
        End If
        Dim row As DataGridViewRow = DataGridView1.CurrentRow

        Dim dbconn As New MySqlConnection
        Dim dbcomm As New MySqlCommand
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        If dbconn.State = 0 Then
            dbconn.Open()
        End If

        Dim id As Integer = row.Cells("id").Value
        Dim data As Date = CDate(row.Cells("data").Value)
        Dim nr_rzf As String = row.Cells("nr_rzf").Value.ToString
        Dim tip_incasare As String = row.Cells("tip_incasare").Value.ToString
        If tip_incasare = "RZF" Then
            nr_rzf = row.Cells("nr_rzf").Value.ToString.Substring(2)
        End If

        Dim explicatii As String = row.Cells("explicatii").Value
        Dim suma_cash As Decimal = row.Cells("Cash").Value
        Dim suma_card As Decimal = 0
        If IsNumeric(row.Cells("POS").Value) Then
            suma_card = row.Cells("POS").Value

        End If
        Dim cash_cont As Boolean = row.Cells("cash").Value


        edit_Lbl.Text = "Edit"
        id_Lbl.Text = id
        mag_v_Lbl.Text = mag_act
        rzf_v_Lbl.Text = row.Cells("nr_rzf").Value.ToString
        GroupBox1.Visible = True

        DateTimePicker1.Value = data
        ComboBox1.SelectedIndex = ComboBox1.Items.IndexOf(tip_incasare)
        nr_rzf_Textbox.Text = nr_rzf
        explicatii_Textbox.Text = explicatii
        suma_CASA_Textbox.Text = suma_cash
        suma_CARD_Textbox.Text = suma_card
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
    End Sub

    Private Sub suma_CASA_Textbox_KeyUp(sender As Object, e As EventArgs) Handles suma_CASA_Textbox.KeyUp
        If suma_CASA_Textbox.Text = "" Then
            suma_CASA_Textbox.Text = 0
            suma_CASA_Textbox.Select()
            'suma_CASA_Textbox.Focus()
        End If
        If IsNumeric(suma_CASA_Textbox.Text) Then
            total_label.Text = "Total: " & (CDec(suma_CASA_Textbox.Text) + CDec(suma_CARD_Textbox.Text))
        End If
    End Sub
    Private Sub suma_CARD_Textbox_KeyUp(sender As Object, e As EventArgs) Handles suma_CARD_Textbox.KeyUp
        If suma_CARD_Textbox.Text = "" Then
            suma_CARD_Textbox.Text = 0
            suma_CARD_Textbox.SelectAll()
            suma_CARD_Textbox.Focus()
            'ElseIf suma_CARD_Textbox.Text = "." Then
            '    suma_CARD_Textbox.Text = "0."
        End If
        If IsNumeric(suma_CARD_Textbox.Text) Then
            total_label.Text = "Total: " & (CDec(suma_CASA_Textbox.Text) + CDec(suma_CARD_Textbox.Text))
        End If
    End Sub

    Private Sub datetimepicker1_focused(sender As Object, e As EventArgs) Handles DateTimePicker1.GotFocus
        data_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        data_Label.ForeColor = Color.Blue
    End Sub
    Private Sub datetimepicker1_lostFocus(sender As Object, e As EventArgs) Handles DateTimePicker1.LostFocus
        data_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        data_Label.ForeColor = Color.Black
    End Sub
    Private Sub ComboBox1_focused(sender As Object, e As EventArgs) Handles ComboBox1.GotFocus
        tip_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        tip_Label.ForeColor = Color.Blue
    End Sub
    Private Sub ComboBox1_lostFocus(sender As Object, e As EventArgs) Handles ComboBox1.LostFocus
        tip_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        tip_Label.ForeColor = Color.Black
    End Sub
    Private Sub nr_rzf_Textbox_focused(sender As Object, e As EventArgs) Handles nr_rzf_Textbox.GotFocus, nr_rzf_Textbox.MouseClick
        nr_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        nr_Label.ForeColor = Color.Blue
        nr_rzf_Textbox.SelectAll()
    End Sub
    Private Sub nr_rzf_Textbox_lostFocus(sender As Object, e As EventArgs) Handles nr_rzf_Textbox.LostFocus
        nr_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        nr_Label.ForeColor = Color.Black
    End Sub
    Private Sub explicatii_Textbox_focused(sender As Object, e As EventArgs) Handles explicatii_Textbox.GotFocus, explicatii_Textbox.MouseClick
        explicatii_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        explicatii_Label.ForeColor = Color.Blue
        explicatii_Textbox.SelectAll()
    End Sub
    Private Sub explicatii_Textbox_lostFocus(sender As Object, e As EventArgs) Handles explicatii_Textbox.LostFocus
        explicatii_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        explicatii_Label.ForeColor = Color.Black
    End Sub
    Private Sub suma_CASA_Textbox_focused(sender As Object, e As EventArgs) Handles suma_CASA_Textbox.GotFocus, suma_CASA_Textbox.MouseClick
        suma_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        suma_Label.ForeColor = Color.Blue
        suma_CASA_Textbox.SelectAll()
    End Sub
    Private Sub suma_CASA_Textbox_lostFocus(sender As Object, e As EventArgs) Handles suma_CASA_Textbox.LostFocus
        suma_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        suma_Label.ForeColor = Color.Black
    End Sub
    Private Sub suma_CARD_Textbox_focused(sender As Object, e As EventArgs) Handles suma_CARD_Textbox.GotFocus, suma_CARD_Textbox.MouseClick
        suma_CARD_label.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        suma_CARD_label.ForeColor = Color.Blue
        suma_CARD_Textbox.SelectAll()
    End Sub
    Private Sub suma_CARD_Textbox_lostFocus(sender As Object, e As EventArgs) Handles suma_CARD_Textbox.LostFocus
        suma_CARD_label.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        suma_CARD_label.ForeColor = Color.Black
    End Sub

    Private Sub suma_CARD_Textbox_leave(sender As Object, e As EventArgs) Handles suma_CARD_Textbox.Leave
        Dim luna As Integer = Today.Month
        Dim an As Integer = Today.Year
        If Today.Day < 6 Then
            luna = Today.Month - 1
            If Today.Month = 1 Then
                an = Today.Year - 1
            End If
        End If
       


        Try
            Dim sql_read As String = "SELECT sum(suma_cash),data FROM incasari where MONTH(data)=" & luna & " AND YEAR(data)=" & an & " AND incasari.magazin='" & ComboBox3.SelectedValue & "'"
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_read, dbconn)

            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If IsDBNull(dbread("sum(suma_cash)")) = False Then
                Label2.Text = CDec(dbread("sum(suma_cash)")) + CDec(suma_CASA_Textbox.Text) + CDec(suma_CARD_Textbox.Text)
            Else
                Label2.Text = "0"
            End If

        Catch ex As Exception
            MsgBox("Problem loading lables total luna: " & ex.Message.ToString)

        End Try
        dbread.Close()

        Try
            Dim sql_read As String = "SELECT sum(suma_cash),data FROM incasari WHERE incasari.magazin='" & ComboBox3.SelectedValue & "'"
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_read, dbconn)

            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If IsDBNull(dbread("sum(suma_cash)")) = False Then
                Label3.Text = CDec(dbread("sum(suma_cash)")) + CDec(suma_CASA_Textbox.Text) + CDec(suma_CARD_Textbox.Text)
            Else
                Label3.Text = "0"
            End If

        Catch ex As Exception
            MsgBox("Problem loading lables total an: " & ex.Message.ToString)

        End Try
        dbread.Close()
    End Sub
    Private Sub enter_Keypress(ByVal sender As System.Object, ByVal e As KeyPressEventArgs) Handles suma_CASA_Textbox.KeyPress, suma_CARD_Textbox.KeyPress, DateTimePicker1.KeyPress, ComboBox1.KeyPress, explicatii_Textbox.KeyPress, nr_rzf_Textbox.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If

    End Sub

    'Private Sub explicatii_Textbox_Enter(sender As Object, e As EventArgs) Handles explicatii_Textbox.Enter
    '    CAPS_ON()
    'End Sub
    'Private Sub explicatii_Textbox_leave(sender As Object, e As EventArgs) Handles explicatii_Textbox.Leave
    '    CAPS_OFF()
    'End Sub

    Private Sub ComboBox3_DropDownClosed(sender As Object, e As EventArgs) Handles ComboBox3.DropDownClosed

        ComboBox1.SelectedItem = "RZF"
        explicatii_Textbox.Text = "Incasari Vanzari"
        ' CheckBox1.CheckState = CheckState.Checked
        Load_DGV()

        Dim sql_read As String = "SELECT nr_rzf,data FROM incasari where tip_incasare='RZF' AND incasari.magazin='" & ComboBox3.SelectedValue & "' ORDER BY DATA DESC LIMIT 1"
        Try
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_read, dbconn)

            dbread = dbcomm.ExecuteReader()
            dbread.Read()

                nr_rzf_Textbox.Text = (dbread("nr_rzf").ToString.Substring(2)) + 1
                Dim ultimazi As Date = CDate(dbread("data"))
            If ultimazi.DayOfWeek = DayOfWeek.Saturday AndAlso ComboBox3.SelectedValue = "PM" Then
                DateTimePicker1.Value = ultimazi.AddDays(2)
            ElseIf ultimazi.DayOfWeek = DayOfWeek.Saturday AndAlso ComboBox3.SelectedValue = "MV" Then
                DateTimePicker1.Value = ultimazi.AddDays(2)
                'ElseIf ultimazi.DayOfWeek = DayOfWeek.Friday AndAlso ComboBox3.SelectedValue = "MV" Then    '' Sari peste ziua de
                '    DateTimePicker1.Value = ultimazi.AddDays(3)                                             '' Sambata
            Else
                DateTimePicker1.Value = ultimazi.AddDays(1)
            End If
            data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)


        Catch ex As Exception
            MsgBox("Problem loading lables (combobox changed): " & ex.Message.ToString)

        End Try
        dbread.Close()
        dbconn.Close()


        suma_CASA_Textbox.Text = "0"
        'With suma_CASA_Textbox
        '    .Focus()
        '    .Select()
        'End With
        suma_CARD_Textbox.Text = "0"
        total_label.Text = "Total: " & (CDec(suma_CASA_Textbox.Text) + CDec(suma_CARD_Textbox.Text))
        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
        Else : PictureBox1.BackColor = Color.LightYellow
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        Load_DGV()
    End Sub

    Private Sub nr_rzf_Textbox_KeyUp(sender As Object, e As EventArgs) Handles nr_rzf_Textbox.KeyUp
        Timer1.Interval = 1000
        Timer1.Stop()
        Timer1.Start()
        'Dim nr_rzf As String = "Z " & nr_rzf_Textbox.Text

        'DataGridView1.ClearSelection()
        'For i = 0 To DataGridView1.Rows.Count - 1
        '    If DataGridView1.Rows(i).Cells("nr_rzf").Value <> nr_rzf Then
        '        If CheckBox2.Checked = True Then
        '            CheckBox2.Checked = False
        '            Exit For
        '        End If
        '    End If
        'Next
        'For i = 0 To DataGridView1.Rows.Count - 1
        '    If DataGridView1.Rows(i).Cells("nr_rzf").Value = nr_rzf Then
        '        DataGridView1.Rows(i).Selected = True
        '        DataGridView1.FirstDisplayedScrollingRowIndex = i - 1
        '        Exit For
        '    End If
        'Next
        'Timer1.Stop()
    End Sub

    Private Sub TransferaCashPOSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TransferaCashPOSToolStripMenuItem.Click

        Dim row As DataGridViewRow = DataGridView1.CurrentRow

        Dim dbconn As New MySqlConnection
        Dim dbcomm As New MySqlCommand
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        If dbconn.State = 0 Then
            dbconn.Open()
        End If

        Dim suma_cash As Decimal = row.Cells("Cash").Value
        Dim suma_card As Decimal = 0
        If row.Cells("POS").Value.ToString = "" Then
            suma_card = 0
        Else : suma_card = row.Cells("POS").Value.ToString
        End If
        Dim id As Integer = row.Cells("id").Value

        Form_inc_transfera.Show()
        With Form_inc_transfera
            .id_Lbl.Text = id
            .suma_Lbl.Text = suma_cash + suma_card
            .cash_TB.Text = suma_cash
            .pos_TB.Text = suma_card
            .cash_TB.SelectAll()
            .cash_TB.Focus()
        End With

       
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Dim nr_rzf As String = "Z " & nr_rzf_Textbox.Text
        Dim rzf As String = ""
        If DataGridView1.Rows.Count > 0 Then
            DataGridView1.Rows(0).Cells("nr_rzf").Value.ToString.Substring(2)
        End If
        Dim exista As Boolean = False

        For i = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Cells("nr_rzf").Value = nr_rzf Or nr_rzf_Textbox.Text > rzf Then
                exista = True
                Exit For
            End If
        Next

        If exista = False Then
            If CheckBox2.Checked = True Then
                CheckBox2.Checked = False
            End If
        End If
        For i = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Cells("nr_rzf").Value = nr_rzf Then
                DataGridView1.ClearSelection()
                DataGridView1.Rows(i).Selected = True
                If i > 0 Then
                    DataGridView1.FirstDisplayedScrollingRowIndex = i - 1
                Else
                    DataGridView1.FirstDisplayedScrollingRowIndex = i
                End If
                DateTimePicker1.Value = DataGridView1.Rows(i).Cells("data").Value
                Exit For
            End If
        Next
    End Sub

   
    Private Sub DataGridView1_Mousedoubleclick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDoubleClick
        Dim mag_act As String = ""
        If IsDBNull(DataGridView1.SelectedRows(0).Cells("magazin").Value) = False Then
            mag_act = DataGridView1.SelectedRows(0).Cells("magazin").Value
            'Dim mag_nou As String = DataGridView1.SelectedRows(0).Cells("magazin").Value
            'If mag_act = "PM" Then
            '    mag_nou = "MV"
            'ElseIf mag_act = "MV" Then
            '    mag_nou = "PM"
            'End If
        End If
        Dim row As DataGridViewRow = DataGridView1.CurrentRow

        Dim dbconn As New MySqlConnection
        Dim dbcomm As New MySqlCommand
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        If dbconn.State = 0 Then
            dbconn.Open()
        End If

        Dim id As Integer = row.Cells("id").Value
        Dim data As Date = CDate(row.Cells("data").Value)
        Dim nr_rzf As String = row.Cells("nr_rzf").Value.ToString
        Dim tip_incasare As String = row.Cells("tip_incasare").Value.ToString
        If tip_incasare = "RZF" Then
            nr_rzf = row.Cells("nr_rzf").Value.ToString.Substring(2)
        End If

        Dim explicatii As String = row.Cells("explicatii").Value
        Dim suma_cash As Decimal = row.Cells("Cash").Value
        Dim suma_card As Decimal = row.Cells("POS").Value
        Dim cash_cont As Boolean = row.Cells("cash").Value



        edit_Lbl.Text = "Edit"
        id_Lbl.Text = id
        mag_v_Lbl.Text = mag_act
        rzf_v_Lbl.Text = row.Cells("nr_rzf").Value.ToString
        GroupBox1.Visible = True

        DateTimePicker1.Value = data
        ComboBox1.SelectedIndex = ComboBox1.Items.IndexOf(tip_incasare)
        nr_rzf_Textbox.Text = nr_rzf
        explicatii_Textbox.Text = explicatii
        suma_CASA_Textbox.Text = suma_cash
        suma_CARD_Textbox.Text = suma_card
    End Sub

    Private Sub renunt_edit_But_Click(sender As Object, e As EventArgs) Handles renunt_edit_But.Click
        suma_CASA_Textbox.Text = 0
        suma_CARD_Textbox.Text = 0
        data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
        explicatii_Textbox.Clear()
        nr_rzf_Textbox.Clear()
        DateTimePicker1.Value = Today
        DateTimePicker1.Focus()
        id_Lbl.Text = ""
        edit_Lbl.Text = ""
        GroupBox1.Visible = False
    End Sub

    Private Sub DataGridView1_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDown

        'Dim CurrentCell As DataGridViewCell = DataGridView1.Item(hit.ColumnIndex, hit.RowIndex)
        If e.Button = Windows.Forms.MouseButtons.Right And e.ColumnIndex > -1 And e.RowIndex > -1 Then
            Dim cell As DataGridViewCell = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex)
            DataGridView1.CurrentCell = cell
        End If
        If edit_Lbl.Visible = True Then
            If e.Button = Windows.Forms.MouseButtons.Left And e.ColumnIndex > -1 And e.RowIndex <> DataGridView1.CurrentRow.Index Then
                suma_CASA_Textbox.Text = 0
                suma_CARD_Textbox.Text = 0
                data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
                explicatii_Textbox.Clear()
                nr_rzf_Textbox.Clear()
                DateTimePicker1.Value = Today
                'DateTimePicker1.Focus()
                id_Lbl.Text = ""
                edit_Lbl.Text = ""
                GroupBox1.Visible = False
            End If
        End If
    End Sub

    Private Sub suma_CARD_Textbox_TextChanged(sender As Object, e As EventArgs) Handles suma_CARD_Textbox.TextChanged

    End Sub
End Class
