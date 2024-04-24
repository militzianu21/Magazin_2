Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization

Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing.Layout
Public Class Form_cheltuieli
    'Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Public AppDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
    Public folder_dp As String
    Public dbconn As MySqlConnection = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Cheltuieli"
        Application.CurrentCulture = New CultureInfo("ro-RO")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("ro-RO")
        Me.KeyPreview = True


        Dim DGV As DataGridView = DataGridView1
        DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGV.ScrollBars = ScrollBars.Vertical
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.AllowUserToAddRows = False

       
        '----------------------
        ComboBox3.DisplayMember = "Text"
        ComboBox3.ValueMember = "Value"
        Dim tb As New DataTable
        tb.Columns.Add("Text", GetType(String))
        tb.Columns.Add("Value", GetType(String))
        tb.Rows.Add("1. Petru Maior", "PM")
        tb.Rows.Add("2. Mihai Viteazu", "MV")
        ComboBox3.DataSource = tb
        ComboBox3.SelectedValue = Form_principal.ComboBox3.SelectedValue

        ComboBox1.Items.AddRange({"CH", "DP", "OP", "-"})
        ComboBox1.SelectedItem = "CH"
        DateTimePicker1.Value = Today
        With DateTimePicker1
            .Focus()
            .Select()
        End With
        data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
        CheckBox2.Checked = True
        cash_RB.Checked = True
        suma_Textbox.Text = 0
        id_Lbl.Text = ""
        edit_Lbl.Text = ""
        'id_Lbl.Visible = False
        'edit_Lbl.Visible = False
        GroupBox1.Visible = False
        Try

            Load_DGV()

            dbcomm = New MySqlCommand("SELECT CONCAT(firma,' ',forma_juridica) as firma FROM firme", dbconn)
            Dim lst As New List(Of String)
            dbread = dbcomm.ExecuteReader()
            While dbread.Read()
                lst.Add(Trim(dbread("firma").ToString()))
            End While
            Dim mysource As New AutoCompleteStringCollection
            mysource.AddRange(lst.ToArray)
            explicatii_Textbox.AutoCompleteSource = AutoCompleteSource.CustomSource
            explicatii_Textbox.AutoCompleteCustomSource = mysource
            explicatii_Textbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        Catch ex As Exception
            MsgBox("Problem loading cheltuieli into grid sau firme in autocomplete: " & ex.Message.ToString)
        End Try
        dbread.Close()
        dbconn.Close()
        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
        Else : PictureBox1.BackColor = Color.LightYellow
        End If
        Load_Sold()
    End Sub
    Private Sub Form_Cheltuieli_FormClosed(sender As Object, e As EventArgs) Handles Me.FormClosed
        CAPS_OFF()
        Form_principal.Show()
        Form_principal.Load_Cheltuieli()
        Form_principal.Load_Incasari()
        Form_principal.Load_Situatie()
    End Sub
    Private Sub save_But_Click(sender As Object, e As EventArgs) Handles save_But.Click
        data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)

        Dim tip_incasare As String = ComboBox1.SelectedItem
        Dim nr_rzf As String = nr_rzf_Textbox.Text
        Dim explicatii As String = explicatii_Textbox.Text
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

        If tip_incasare = "DP" Then
            explicatii_Textbox.Text = "Restituire Imprumut"
        End If

        Dim cash As Boolean = True
        If cash_RB.Checked = True Then
            cash = True
        ElseIf cash_RB.Checked = False Then
            cash = False
        End If
        Dim sql As String = "INSERT INTO cheltuieli(data,tip_cheltuiala,nr_chitanta,explicatii,suma,magazin,cash) " _
                            & "VALUES(@data,@tip_cheltuialA,@nr_chitanta,@explicatii,@suma,@magazin,@cash)"
        If edit_Lbl.Text = "Edit" Then
            sql = "UPDATE cheltuieli SET data=@data,tip_cheltuiala=@tip_cheltuiala,nr_chitanta=@nr_chitanta,explicatii=@explicatii,suma=@suma,magazin=@magazin,cash=@cash WHERE id=@id"
        End If

        Try
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql, dbconn)
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value.Date)
            dbcomm.Parameters.AddWithValue("@tip_cheltuialA", tip_incasare)
            dbcomm.Parameters.AddWithValue("@nr_chitanta", nr_rzf)
            dbcomm.Parameters.AddWithValue("@explicatii", explicatii_Textbox.Text)
            dbcomm.Parameters.AddWithValue("@suma", CDec(suma))
            dbcomm.Parameters.AddWithValue("@magazin", ComboBox3.SelectedValue)
            dbcomm.Parameters.AddWithValue("@cash", cash)
            dbcomm.Parameters.AddWithValue("@id", id_Lbl.Text)
            dbcomm.ExecuteNonQuery()
            transaction.Commit()
            dbconn.Close()
        Catch ex As Exception
            MsgBox("Failed to insert data: " & ex.Message.ToString())
        End Try

        Load_DGV()
        DataGridView1.ClearSelection()
        'MsgBox(DateTimePicker1.Value.Month & " " & Today.Month & " " & DateTimePicker1.Value.Year & " " & Today.Year)
        If DateTimePicker1.Value.Month <> Today.Month Or DateTimePicker1.Value.Year <> Today.Year Then
            If CheckBox2.Checked = True Then
                CheckBox2.Checked = False
            End If
        End If
        For i = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Cells("nr_chitanta").Value.ToString = nr_rzf AndAlso DataGridView1.Rows(i).Cells("suma").Value.ToString = suma Then
                DataGridView1.Rows(i).Selected = True
                If i > 0 Then
                    DataGridView1.FirstDisplayedScrollingRowIndex = i - 1
                Else
                    DataGridView1.FirstDisplayedScrollingRowIndex = i
                End If
                Exit For
            End If
        Next
        'DataGridView1.ClearSelection()

        'For i = 0 To DataGridView1.Rows.Count - 1
        '    If DataGridView1.Rows(i).Cells("nr_rzf").Value = nr_rzf Then
        '        DataGridView1.Rows(i).Selected = True
        '        DataGridView1.FirstDisplayedScrollingRowIndex = i - 1
        '        Exit For
        '    End If
        'Next
        dbconn.Close()

        suma_Textbox.Text = 0
        data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
        explicatii_Textbox.Clear()
        nr_rzf_Textbox.Clear()
        ComboBox1.SelectedItem = "CH"
        DateTimePicker1.Focus()
        id_Lbl.Text = ""
        edit_Lbl.Text = ""
        GroupBox1.Visible = False

        DataGridView1.Columns("id").Visible = False
        Load_Sold()

    End Sub

    Private Sub Load_DGV()
        Dim DGV As DataGridView = DataGridView1
        DGV.MultiSelect = True

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
        Dim sql_read As String = "SELECT * FROM cheltuieli WHERE cheltuieli.magazin='" & ComboBox3.SelectedValue & "' " & limit & " ORDER BY data DESC, id DESC"
        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource
        Try
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_read, dbconn)
            sda.SelectCommand = dbcomm
            dbcomm.Parameters.AddWithValue("@anul_ult", anul_ult)
            dbcomm.Parameters.AddWithValue("@luna_ult", luna_ult)
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            DataGridView1.DataSource = bsource
            sda.Update(dbdataset)
        Catch ex As Exception
            MsgBox("Problem loading data: " & ex.Message.ToString)
        End Try
        DataGridView1.Columns("id").Visible = False
        DGV.Columns("data").FillWeight = DGV.Width * 16 / 100
        DGV.Columns("data").HeaderText = "Data"
        DGV.Columns("data").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("nr_chitanta").FillWeight = DGV.Width * 10 / 100
        DGV.Columns("nr_chitanta").HeaderText = "Nr. Ch"
        DGV.Columns("nr_chitanta").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("tip_cheltuiala").FillWeight = DGV.Width * 7 / 100
        DGV.Columns("tip_cheltuiala").HeaderText = "Tip Ch"
        DGV.Columns("tip_cheltuiala").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("explicatii").FillWeight = DGV.Width * 34.5 / 100
        DGV.Columns("explicatii").HeaderText = "Explicatii"
        DGV.Columns("explicatii").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("suma").FillWeight = DGV.Width * 14 / 100
        DGV.Columns("suma").HeaderText = "Suma"
        DGV.Columns("suma").DefaultCellStyle.Format = "#,#0.00"
        DGV.Columns("suma").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DGV.Columns("magazin").FillWeight = DGV.Width * 7 / 100
        DGV.Columns("magazin").HeaderText = "Mag"
        DGV.Columns("magazin").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'For Each control In DataGridView1.Controls
        'If control.GetType() Is GetType(VScrollBar) Then
        '    Dim vbar As VScrollBar = DirectCast(control, VScrollBar)
        '    If vbar.Visible = True Then
        '        DGV.Columns("cash").FillWeight = DGV.Width * 5.5 / 100
        '    Else
        DGV.Columns("cash").FillWeight = DGV.Width * 11 / 100
        '    End If
        'End If
        'Next
        DGV.Columns("cash").HeaderText = "Cash"
        For i = 0 To DGV.Rows.Count - 1
            DGV.Rows(i).DefaultCellStyle.BackColor = Color.Azure
            If DGV.Rows(i).Cells("cash").Value = False Then
                DGV.Rows(i).DefaultCellStyle.ForeColor = Color.Gray
                DGV.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
            If DGV.Rows(i).Cells("explicatii").Value.ToString.Contains("ELECTRICA") Or
                DGV.Rows(i).Cells("explicatii").Value.ToString.Contains("RAGCL") Or
                DGV.Rows(i).Cells("explicatii").Value.ToString.Contains("MAVIPROD") Or
                DGV.Rows(i).Cells("explicatii").Value.ToString.Contains("BIROTECH") Or
                DGV.Rows(i).Cells("explicatii").Value.ToString.Contains("NEFER") Or
                DGV.Rows(i).Cells("explicatii").Value.ToString.Contains("MINISTERUL") Or
                DGV.Rows(i).Cells("explicatii").Value.ToString.Contains("MUNICIPIUL") Or
                DGV.Rows(i).Cells("explicatii").Value.ToString.Contains("BALAZS") Then
                DGV.Rows(i).DefaultCellStyle.BackColor = Color.LightPink
            End If
            If DGV.Rows(i).Cells("explicatii").Value.ToString.Contains("SALARII") Then
                DGV.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
            End If
            If DGV.Rows(i).Cells("explicatii").Value.ToString.Contains("Restituire") Then
                DGV.Rows(i).DefaultCellStyle.BackColor = Color.PaleGreen
            End If

        Next
        'If dbread.IsClosed = False Then
        '    dbread.Close()
        'End If
    End Sub
    Private Sub Load_Sold()
        Dim data As Date = DateTimePicker1.Value
        Dim luna As Integer = DateTimePicker1.Value.Month
        Dim an As Integer = DateTimePicker1.Value.Year
        Dim data_ant As Date = DateAdd(DateInterval.Month, -1, DateTimePicker1.Value)
        Dim luna_ant As Integer = data_ant.Month
        Dim an_ant As Integer = data_ant.Year

        Dim sql_read As String = "SELECT sold, incasari, cheltuieli FROM " _
                                 & "(SELECT casa_sold_final as sold FROM solduri_casa WHERE MONTH(data)=@luna_ant AND YEAR(data)=@an_ant AND magazin='" & ComboBox3.SelectedValue & "') sold," _
                                 & "(SELECT SUM(suma_cash) as incasari FROM incasari WHERE MONTH(data)=@luna AND YEAR(data)=@an AND data<=@data AND magazin='" & ComboBox3.SelectedValue & "') incasari," _
                                 & "(SELECT SUM(suma) as cheltuieli FROM cheltuieli WHERE MONTH(data)=@luna AND YEAR(data)=@an AND data<=@data AND magazin='" & ComboBox3.SelectedValue & "' and cash=true) cheltuieli"

        Dim sda As New MySqlDataAdapter
        Dim dbtable As New DataTable
        Dim bsource As New BindingSource
        Try
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If

            dbcomm = New MySqlCommand(sql_read, dbconn)
            dbcomm.Parameters.AddWithValue("@data", data)
            dbcomm.Parameters.AddWithValue("@luna", luna)
            dbcomm.Parameters.AddWithValue("@an", an)
            dbcomm.Parameters.AddWithValue("@luna_ant", luna_ant)
            dbcomm.Parameters.AddWithValue("@an_ant", an_ant)

            sda.SelectCommand = dbcomm
            sda.Fill(dbtable)
            bsource.DataSource = dbtable
            sda.Update(dbtable)

            Dim sold As Decimal = 0
            Dim incasari As Decimal = 0
            Dim cheltuieli As Decimal = 0
            If dbtable.Rows.Count > 0 Then
                If IsDBNull(dbtable.Rows(0).Item("sold")) Then
                    sold = 0
                Else : sold = dbtable.Rows(0).Item("sold")
                End If
                If IsDBNull(dbtable.Rows(0).Item("incasari")) Then
                    incasari = 0
                Else : incasari = dbtable.Rows(0).Item("incasari")
                End If
                If IsDBNull(dbtable.Rows(0).Item("cheltuieli")) Then
                    cheltuieli = 0
                Else : cheltuieli = dbtable.Rows(0).Item("cheltuieli")
                End If

                'Label2.Text = CDec(dbread("sold")) + CDec(dbread("incasari")) - CDec(dbread("cheltuieli"))
                Label2.Text = sold + incasari - cheltuieli
                'Else
                'Label2.Text = "0"
                'End If
            Else : Label2.Text = "--"
            End If

        Catch ex As Exception
            MsgBox("Problem loading lables sold final: " & ex.Message.ToString)

        End Try

    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedValueChanged, ComboBox1.Leave
        Dim tip_incasare As String = ComboBox1.SelectedItem

        Dim nr_rzf As String = nr_rzf_Textbox.Text
        Dim explicatii As String = explicatii_Textbox.Text
        Dim suma As String = suma_Textbox.Text

        If tip_incasare = "CH" Then
            data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
            explicatii_Textbox.Clear()
            nr_rzf_Textbox.Clear()
            cash_RB.Checked = True
        End If

        If tip_incasare = "DP" Then
            Dim sql_read As String = "SELECT nr_chitanta,data FROM cheltuieli WHERE tip_cheltuiala='DP' AND data<@data ORDER BY data DESC LIMIT 1"
            Try
                If dbconn.State = 0 Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(sql_read, dbconn)
                dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If dbread.HasRows Then
                    nr_rzf_Textbox.Text = dbread("nr_chitanta").ToString + 1
                    'DateTimePicker1.Select()
                    'DateTimePicker1.Focus()
                Else : nr_rzf_Textbox.Text = 1
                    nr_rzf_Textbox.Select()
                    nr_rzf_Textbox.Focus()
                End If
            Catch ex As Exception
                MsgBox("Problem loading data: " & ex.Message.ToString)
            End Try
            dbread.Close()
            dbconn.Close()
            data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
            explicatii_Textbox.Text = "Restituire imprumut"
            cash_RB.Checked = True
        End If

        If tip_incasare = "OP" Then
            'Dim sql_read As String = "SELECT nr_chitanta,data FROM cheltuieli where tip_cheltuiala='OP' ORDER BY data DESC LIMIT 1"
            'Try
            '    If dbconn.State = 0 Then
            '        dbconn.Open()
            '    End If
            '    dbcomm = New MySqlCommand(sql_read, dbconn)
            '    dbread = dbcomm.ExecuteReader()
            '    dbread.Read()
            '    If dbread.HasRows Then
            '        nr_rzf_Textbox.Text = dbread("nr_chitanta").ToString + 1
            '        'DateTimePicker1.Select()
            '        'DateTimePicker1.Focus()
            '    Else : nr_rzf_Textbox.Text = 1
            '        'nr_rzf_Textbox.Select()
            '        'nr_rzf_Textbox.Focus()
            '    End If
            'Catch ex As Exception
            '    MsgBox("Problem loading data: " & ex.Message.ToString)
            'End Try
            'dbread.Close()
            'dbconn.Close()
            data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
            explicatii_Textbox.Text = ""
            cont_RB.Checked = True
        End If
        If tip_incasare = "-" Then
            data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
            explicatii_Textbox.Text = "SALARII"
            nr_rzf_Textbox.Text = "-"
            cash_RB.Checked = True
        End If
        nr_Label.Text = "Nr. " & tip_incasare
        tip_Label.Text = tip_incasare
        dbconn.Close()

    End Sub
    Private Sub StergeInregistrareaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StergeInregistrareaToolStripMenuItem.Click

        Dim yesno As Integer = MsgBox("Stergi Inregistrarea?", MsgBoxStyle.YesNo)
        Dim row As DataGridViewRow = DataGridView1.CurrentRow
        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then

            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            For i = 0 To DataGridView1.SelectedRows.Count - 1
                Dim data As Date = CDate(DataGridView1.SelectedRows(i).Cells("data").Value)
                Dim nr_chitanta As String = DataGridView1.SelectedRows(i).Cells("nr_chitanta").Value.ToString
                Dim tip_cheltuiala As String = DataGridView1.SelectedRows(i).Cells("tip_cheltuiala").Value.ToString
                Dim explicatii As String = DataGridView1.SelectedRows(i).Cells("explicatii").Value

                Try
                    Dim sql_del_inc As String = "DELETE FROM cheltuieli WHERE data=@data AND nr_chitanta=@nr_chitanta AND tip_cheltuiala=@tip_cheltuiala AND magazin='" & ComboBox3.SelectedValue & "'"
                    dbcomm = New MySqlCommand(sql_del_inc, dbconn)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@nr_chitanta", nr_chitanta)
                    dbcomm.Parameters.AddWithValue("@tip_cheltuiala", tip_cheltuiala)
                    dbcomm.Parameters.AddWithValue("@explicatii", explicatii)
                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Nu s-a sters din cheltuieli: " & ex.Message.ToString)
                End Try
            Next
            transaction.Commit()

            Dim limit As String = ""
            If CheckBox2.CheckState = CheckState.Checked Then
                limit = "AND YEAR(data) = YEAR(CURRENT_DATE()) AND MONTH(data) = MONTH(CURRENT_DATE())"
                If CheckBox2.CheckState = CheckState.Unchecked Then
                    limit = ""
                End If
            End If
            'Dim sql_read As String = "SELECT * FROM cheltuieli WHERE cheltuieli.magazin='" & ComboBox3.SelectedValue & "' " & limit & " ORDER BY data DESC, id DESC"
            'Dim sda As New MySqlDataAdapter
            'Dim dbdataset As New DataTable
            'Dim bsource As New BindingSource
            'Try
            '    If dbconn.State = 0 Then
            '        dbconn.Open()
            '    End If
            '    dbcomm = New MySqlCommand(sql_read, dbconn)
            '    sda.SelectCommand = dbcomm
            '    sda.Fill(dbdataset)
            '    bsource.DataSource = dbdataset
            '    DataGridView1.DataSource = bsource
            '    sda.Update(dbdataset)
            'Catch ex As Exception
            '    MsgBox("Problem loading data: " & ex.Message.ToString)
            'End Try
            ''dbread.Close()
            Load_DGV()
            Load_Sold()
            dbconn.Close()
        End If
    End Sub
    Private Sub suma_textbox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles suma_Textbox.KeyPress
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
    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
        Load_Sold()
    End Sub

    Private Sub suma_Textbox_TextChanged(sender As Object, e As EventArgs) Handles suma_Textbox.TextChanged
        If suma_Textbox.Text = "" Then
            suma_Textbox.Text = 0
        End If
    End Sub

    Private Sub datetimepicker1_focused(sender As Object, e As EventArgs) Handles DateTimePicker1.MouseDown
        data_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        data_Label.ForeColor = Color.Blue
        DateTimePicker1.Select()
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
        '    explicatii_Textbox.SelectAll()
    End Sub
    Private Sub explicatii_Textbox_lostFocus(sender As Object, e As EventArgs) Handles explicatii_Textbox.LostFocus
        explicatii_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        explicatii_Label.ForeColor = Color.Black
        If ComboBox1.SelectedItem = "CH" Then
            explicatii_Textbox.Text = StrConv(explicatii_Textbox.Text, VbStrConv.Uppercase)
        End If

    End Sub
    Private Sub suma_Textbox_focused(sender As Object, e As EventArgs) Handles suma_Textbox.GotFocus, suma_Textbox.MouseClick
        suma_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        suma_Label.ForeColor = Color.Blue
        suma_Textbox.SelectAll()
    End Sub
    Private Sub suma_Textbox_lostFocus(sender As Object, e As EventArgs) Handles suma_Textbox.LostFocus
        suma_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        suma_Label.ForeColor = Color.Black
    End Sub
    Private Sub enter_Keypress(ByVal sender As System.Object, ByVal e As KeyPressEventArgs) Handles suma_Textbox.KeyPress, DateTimePicker1.KeyPress, ComboBox1.KeyPress, nr_rzf_Textbox.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If

    End Sub
    Private Sub explicatii_text_Keydown(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles explicatii_Textbox.KeyDown
        If e.KeyValue = 13 Then
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If

    End Sub

   
    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

        If DataGridView1.Rows.Count > 0 Then
            If DataGridView1.CurrentRow.Index > -1 AndAlso DataGridView1.SelectedRows.Count <> 0 Then
                Dim row As DataGridViewRow = DataGridView1.CurrentRow
                Dim tip_cheltuiala As String = row.Cells("tip_cheltuiala").Value.ToString

                If tip_cheltuiala = "DP" Then
                    PrinteazaDPToolStripMenuItem.Enabled = True
                ElseIf tip_cheltuiala <> "DP" Then
                    PrinteazaDPToolStripMenuItem.Enabled = False
                End If
            End If
        ElseIf DataGridView1.Rows.Count < 1 Then
            e.Cancel = True
        End If

    End Sub
    Private Sub PrinteazaDPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrinteazaDPToolStripMenuItem.Click
        '----------setari
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")

        If dbconn.State = ConnectionState.Closed Then
            dbconn.Open()
        End If
        Try
            Dim set_sql As String = "SELECT * FROM setari WHERE setare='path_dp'"

            dbcomm = New MySqlCommand(set_sql, dbconn)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = True Then
                folder_dp = dbread("valoare")
            End If

        Catch ex As Exception
            MsgBox("Problem loading setari: path raport: " & ex.Message.ToString)
        End Try
        dbread.Close()
        '------------------
        Dim row As DataGridViewRow = DataGridView1.CurrentRow
        Dim data As String = row.Cells("data").Value
        Dim nr_chitanta As String = row.Cells("nr_chitanta").Value.ToString
        Dim tip_cheltuiala As String = row.Cells("tip_cheltuiala").Value.ToString
        Dim explicatii As String = row.Cells("explicatii").Value
        Dim suma As String = row.Cells("suma").Value
        Dim suma_form As String = FormatNumber(CDec(suma.ToString), 2, TriState.True, TriState.False, TriState.True)
        Dim maga As String = ""
        If row.Cells("magazin").Value = "PM" Then
            maga = " - Magazin: PETRU MAIOR 9"
        ElseIf row.Cells("magazin").Value = "MV" Then
            maga = " - Magazin: MIHAI VITEAZU 28"
        End If

        Dim pdf As PdfSharp.Pdf.PdfDocument = New PdfSharp.Pdf.PdfDocument
        pdf.Info.Title = "Dispozitie Plata"
        Dim pdfPage As PdfSharp.Pdf.PdfPage = pdf.AddPage


        Dim graph As XGraphics = XGraphics.FromPdfPage(pdfPage)
        Dim pen As XPen = New XPen(Color.Black, 0.5)

        Dim titlu_font As XFont = New XFont("Verdana", 16, XFontStyle.Bold)
        Dim top_font As XFont = New XFont("Verdana", 12, XFontStyle.Bold)
        Dim mic_font As XFont = New XFont("Calibri", 8, XFontStyle.Regular)
        Dim report_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim text_font As XFont = New XFont("Calibri", 10, XFontStyle.Regular)
        Dim text_bold_font As XFont = New XFont("Calibri", 10, XFontStyle.Bold)
        Dim bottom_font As XFont = New XFont("Verdana", 12, XFontStyle.Bold)

        Dim incas_tot As Double = 0
        Dim chelt_tot As Double = 0
        'A4 = 8.27x11.69" x72points/inch = 595x842 points
        'v--------------------------------------v
        graph.DrawLine(pen, 100, 20, 100, 213) '|
        graph.DrawLine(pen, 495, 20, 495, 213) '|

        graph.DrawLine(pen, 100, 20, 495, 20) '----
        graph.DrawString("Unitatea", text_font, XBrushes.Black, _
                         New XRect(102, 22, 40, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawLine(pen, 140, 20, 140, 37) '|
        graph.DrawString("MILICOM CAZ SRL" & maga, text_bold_font, XBrushes.Black, _
                         New XRect(140, 22, 355, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 100, 37, 495, 37) '----


        graph.DrawLine(pen, 100, 43, 495, 43) '----

        graph.DrawString("DISPOZITIE DE *)", text_font, XBrushes.Black, _
                         New XRect(102, 45, 80, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawLine(pen, 180, 43, 180, 60) '|
        graph.DrawString("PLATA", text_font, XBrushes.Black, _
                         New XRect(180, 45, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 43, 260, 60) '|
        graph.DrawString("", text_font, XBrushes.Black, _
                         New XRect(260, 45, 80, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawLine(pen, 340, 43, 340, 60) '|
        graph.DrawString("CATRE CASIERIE", text_font, XBrushes.Black, _
                         New XRect(340, 45, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 420, 43, 420, 60) '|
        graph.DrawString("UNITATE", text_font, XBrushes.Black, _
                         New XRect(420, 45, 75, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 100, 60, 495, 60) '----
        graph.DrawString("nr.", text_font, XBrushes.Black, _
                         New XRect(102, 62, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 140, 60, 140, 77) '|
        graph.DrawString(nr_chitanta, text_bold_font, XBrushes.Black, _
                         New XRect(140, 62, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 220, 60, 220, 77) '|
        graph.DrawString("din", text_font, XBrushes.Black, _
                         New XRect(220, 62, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 60, 260, 77) '|
        graph.DrawString(data, text_bold_font, XBrushes.Black, _
                         New XRect(260, 62, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 340, 60, 340, 77) '|

        graph.DrawLine(pen, 100, 77, 340, 77) '----
        graph.DrawString("Numele si Prenumele", mic_font, XBrushes.Black, _
                         New XRect(102, 81, 80, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawLine(pen, 180, 77, 180, 94) '|
        graph.DrawString("CAZAN MIHAI", text_bold_font, XBrushes.Black, _
                         New XRect(180, 79, 160, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 340, 77, 340, 94) '|

        graph.DrawLine(pen, 100, 94, 340, 94)  '----
        graph.DrawString("Functia (calitatea)", mic_font, XBrushes.Black, _
                        New XRect(102, 98, 80, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawLine(pen, 180, 94, 180, 111) '|
        graph.DrawString("administrator", text_font, XBrushes.Black, _
                         New XRect(180, 96, 160, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 340, 94, 340, 111) '|

        graph.DrawLine(pen, 100, 111, 495, 111) '----
        graph.DrawString("Suma", mic_font, XBrushes.Black, _
                         New XRect(102, 115, 40, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawLine(pen, 140, 111, 140, 128) '|
        graph.DrawString(suma_form, text_bold_font, XBrushes.Black, _
                         New XRect(140, 113, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 220, 111, 220, 128) '|
        graph.DrawString("lei", text_font, XBrushes.Black, _
                         New XRect(220, 113, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 111, 260, 128) '|
        graph.DrawString(NrCuv(suma) & " lei", text_bold_font, XBrushes.Black, _
                         New XRect(260, 113, 240, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 100, 128, 495, 128) '----
        graph.DrawLine(pen, 140, 128, 140, 145) '|
        graph.DrawString("(in cifre)", text_font, XBrushes.Black, _
                         New XRect(140, 130, 120, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 128, 260, 145) '|
        graph.DrawString("(in litere)", text_font, XBrushes.Black, _
                         New XRect(260, 130, 240, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 100, 145, 495, 145) '----
        graph.DrawString("Scopul platii", mic_font, XBrushes.Black, _
                        New XRect(102, 149, 80, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawLine(pen, 180, 145, 180, 162) '|
        graph.DrawString("restituire imprumut", text_bold_font, XBrushes.Black, _
                         New XRect(180, 147, 315, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 100, 162, 495, 162) '----

        graph.DrawLine(pen, 100, 179, 495, 179) '----
        graph.DrawString("Semnatura", mic_font, XBrushes.Black, _
                         New XRect(100, 190, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 140, 179, 140, 213) '|
        graph.DrawString("Conducatorul unitatii", mic_font, XBrushes.Black, _
                         New XRect(140, 183, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 220, 179, 220, 213) '|
        graph.DrawString("Viza de control financiar preventiv", mic_font, XBrushes.Black, _
                         New XRect(220, 183, 120, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 340, 179, 340, 213) '|
        graph.DrawString("Compartiment financiar-contabil", mic_font, XBrushes.Black, _
                         New XRect(340, 183, 155, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 140, 196, 495, 196) '----

        graph.DrawLine(pen, 100, 213, 495, 213) '----


        pdfPage = pdf.AddPage()
        graph = XGraphics.FromPdfPage(pdfPage)


        ' PAGINA 2
        'v--------------------------------------v
        graph.DrawLine(pen, 100, 20, 100, 213) '|
        graph.DrawLine(pen, 495, 20, 495, 213) '|

        graph.DrawLine(pen, 100, 20, 495, 20) '----
        graph.DrawString("Date suplimentare privind beneficiarul sumei", text_font, XBrushes.Black, _
                         New XRect(180, 22, 315, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 180, 37, 495, 37) '----


        graph.DrawLine(pen, 180, 43, 495, 43) '----
        graph.DrawString("Actul de identitate", text_font, XBrushes.Black, _
                         New XRect(180, 45, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 43, 260, 60) '|
        graph.DrawString("CI", text_bold_font, XBrushes.Black, _
                         New XRect(260, 45, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 300, 43, 300, 60) '|
        graph.DrawString("Seria", text_font, XBrushes.Black, _
                         New XRect(300, 45, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 340, 43, 340, 60) '|
        graph.DrawString("MS", text_bold_font, XBrushes.Black, _
                         New XRect(340, 45, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 380, 43, 380, 60) '|
        graph.DrawString("nr.", text_font, XBrushes.Black, _
                         New XRect(380, 45, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 420, 43, 420, 60) '|
        graph.DrawString("722599", text_bold_font, XBrushes.Black, _
                         New XRect(420, 45, 75, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 180, 60, 495, 60) '----
        graph.DrawString("Am primit suma de", text_font, XBrushes.Black, _
                         New XRect(180, 62, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 60, 260, 77) '|
        graph.DrawString(suma_form, text_bold_font, XBrushes.Black, _
                         New XRect(260, 62, 160, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 420, 60, 420, 77) '|
        graph.DrawString("lei", text_font, XBrushes.Black, _
                         New XRect(420, 62, 75, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawString("Se completeaza numai", mic_font, XBrushes.Black, _
                         New XRect(100, 79, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("pentru plati", mic_font, XBrushes.Black, _
                         New XRect(100, 87, 80, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 180, 77, 495, 77) '----
        graph.DrawString("(in cifre)", text_font, XBrushes.Black, _
                         New XRect(180, 79, 315, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 180, 94, 495, 94)  '----
        graph.DrawLine(pen, 220, 94, 220, 111) '|
        graph.DrawString("Data", text_font, XBrushes.Black, _
                         New XRect(220, 96, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 94, 260, 111) '|
        graph.DrawString(data, text_bold_font, XBrushes.Black, _
                         New XRect(260, 96, 160, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 420, 94, 420, 111) '|

        graph.DrawLine(pen, 180, 111, 495, 111) '----
        graph.DrawLine(pen, 220, 111, 220, 145) '|
        graph.DrawString("Semnatura", mic_font, XBrushes.Black, _
                         New XRect(220, 124, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 111, 260, 145) '|
        graph.DrawLine(pen, 420, 111, 420, 145) '|

        graph.DrawLine(pen, 180, 20, 180, 145) '|

        graph.DrawLine(pen, 100, 145, 495, 145) '----
        graph.DrawString("CASIER", text_font, XBrushes.Black, _
                         New XRect(100, 147, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 140, 145, 140, 162) '|

        graph.DrawLine(pen, 100, 162, 495, 162) '----
        graph.DrawString("Platit/incasat suma de", mic_font, XBrushes.Black, _
                        New XRect(100, 166, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 180, 162, 180, 179) '|
        graph.DrawString(suma_form, text_bold_font, XBrushes.Black, _
                       New XRect(180, 164, 240, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 420, 162, 420, 179) '|
        graph.DrawString("lei", text_font, XBrushes.Black, _
                       New XRect(420, 164, 75, pdfPage.Height.Point), XStringFormats.TopCenter)


        graph.DrawLine(pen, 100, 179, 495, 179) '----
        graph.DrawString("Data", text_font, XBrushes.Black, _
                         New XRect(100, 188, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 180, 179, 180, 213) '|
        graph.DrawString(data, text_bold_font, XBrushes.Black, _
                         New XRect(180, 188, 120, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 300, 179, 300, 213) '|
        graph.DrawString("Semnatura", text_font, XBrushes.Black, _
                         New XRect(300, 188, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 380, 179, 380, 213) '|


        graph.DrawLine(pen, 100, 213, 495, 213) '----
        'String.Format("{0:00}", nr_chitanta)
        Dim pdfFilename As String = folder_dp & "Dispozitie Plata_" & nr_chitanta.ToString.PadLeft(3, "0") & "_" & Format(CDate(data), "yyyyMMdd") & ".pdf"
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
    End Sub

    Private Sub ad_firma_But_Click(sender As Object, e As EventArgs) Handles ad_firma_But.Click
        Form_firme_introd.firma_text.Clear()
        Form_firme_introd.forma_juridica_text.Clear()
        Form_firme_introd.cui_text.Clear()
        Form_firme_introd.j_text.Clear()
        Form_firme_introd.adresa_text.Clear()
        Form_firme_introd.tip_text.Clear()
        If ComboBox1.SelectedItem = "CH" Then
            Form_firme_introd.tip_text.Text = "Marfa"
        End If
        Form_firme_introd.status_text.Clear()
        Form_firme_introd.cont_text.Clear()
        Form_firme_introd.banca_text.Clear()

        Form_firme_introd.firma_text.Focus()
        Form_firme_introd.ShowDialog()


        If Form_firme_introd.DialogResult = Windows.Forms.DialogResult.OK Then
            Dim id As String = ""
            Dim firma As String = ""
            Dim forma_juridica As String = ""
            Dim cui As String = ""
            Dim tva As Boolean = False
            Dim j As String = ""
            Dim adresa As String = ""
            Dim localitate As String = ""
            Dim judet As String = ""
            Dim tip As String = ""
            Dim status As String = ""
            Dim cont As String = ""
            Dim banca As String = ""
            firma = Trim(Form_firme_introd.firma_text.Text)
            forma_juridica = Trim(Form_firme_introd.forma_juridica_text.Text)
            If Form_firme_introd.cui_text.Text = "" Then
                cui = "NULL"
            Else
                cui = Form_firme_introd.cui_text.Text
            End If
            If Form_firme_introd.CheckBox1.CheckState = CheckState.Checked Then
                tva = True
            Else
                tva = False
            End If

            j = Form_firme_introd.j_text.Text
            adresa = Form_firme_introd.adresa_text.Text
            localitate = Form_firme_introd.localitate_text.Text
            judet = Form_firme_introd.judet_text.Text
            tip = Form_firme_introd.tip_text.Text
            status = Form_firme_introd.status_text.Text
            cont = Form_firme_introd.cont_text.Text
            banca = Form_firme_introd.banca_text.Text
            Dim sql_upd As String = "INSERT INTO firme " _
                                    & "(firma,forma_juridica,cui,tva,j,adresa,localitate,judet,tip,status,cont,banca) " _
                                    & " VALUES" _
                                    & "(@firma, @forma_juridica," & cui & ",@tva,@j,@adresa,@localitate,@judet,@tip,@status,@cont,@banca)"
            Try
                Dim dbconn As New MySqlConnection

                dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
                If dbconn.State = 0 Then
                    dbconn.Open()
                End If
                Using dbcomm As MySqlCommand = New MySqlCommand(sql_upd, dbconn)
                    dbcomm.Parameters.AddWithValue("@firma", firma)
                    dbcomm.Parameters.AddWithValue("@forma_juridica", forma_juridica)
                    'dbcomm.Parameters.AddWithValue("@cui", cui)
                    dbcomm.Parameters.AddWithValue("@tva", tva)
                    dbcomm.Parameters.AddWithValue("@j", j)
                    dbcomm.Parameters.AddWithValue("@adresa", adresa)
                    dbcomm.Parameters.AddWithValue("@localitate", localitate)
                    dbcomm.Parameters.AddWithValue("@judet", judet)
                    dbcomm.Parameters.AddWithValue("@tip", tip)
                    dbcomm.Parameters.AddWithValue("@status", status)
                    dbcomm.Parameters.AddWithValue("@cont", cont)
                    dbcomm.Parameters.AddWithValue("@banca", banca)
                    dbcomm.Parameters.AddWithValue("@id", id)
                    dbcomm.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                MsgBox("Nu s-a modificat: " & ex.Message.ToString)
            End Try
            Form_principal.Load_Firme()
            explicatii_Textbox.Text = firma & " " & forma_juridica
            explicatii_Textbox.SelectAll()
            explicatii_Textbox.Focus()

            Form_firme_introd.Dispose()
        End If
    End Sub
    Private Sub explicatii_Textbox_Enter(sender As Object, e As EventArgs) Handles explicatii_Textbox.Enter
        CAPS_ON()
    End Sub
    'Private Sub explicatii_Textbox_leave(sender As Object, e As EventArgs) Handles explicatii_Textbox.Leave
    '    CAPS_OFF()
    'End Sub
    Private Sub ComboBox3_DropDownClosed(sender As Object, e As EventArgs) Handles ComboBox3.DropDownClosed


        Load_DGV()
        Load_Sold()
        dbread.Close()
        dbconn.Close()


        suma_Textbox.Text = "0"

        suma_Textbox.Text = "0"
        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
        Else : PictureBox1.BackColor = Color.LightYellow
        End If
    End Sub

    Private Sub TransferaCheltuialaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TransferaCheltuialaToolStripMenuItem.Click
        Dim mag_act As String = DataGridView1.SelectedRows(0).Cells("magazin").Value
        Dim mag_nou As String = DataGridView1.SelectedRows(0).Cells("magazin").Value
        If mag_act = "PM" Then
            mag_nou = "MV"
        ElseIf mag_act = "MV" Then
            mag_nou = "PM"
        End If

        Dim yesno As Integer = MsgBox("Transferi Cheltuiala in Casa " & mag_nou & " ?", MsgBoxStyle.YesNo)
        Dim row As DataGridViewRow = DataGridView1.CurrentRow
        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then

            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            For i = 0 To DataGridView1.SelectedRows.Count - 1
                Dim data As Date = CDate(DataGridView1.SelectedRows(i).Cells("data").Value)
                Dim nr_chitanta As String = DataGridView1.SelectedRows(i).Cells("nr_chitanta").Value.ToString
                Dim tip_cheltuiala As String = DataGridView1.SelectedRows(i).Cells("tip_cheltuiala").Value.ToString
                Dim explicatii As String = DataGridView1.SelectedRows(i).Cells("explicatii").Value

                Try
                    Dim sql_del_inc As String = "UPDATE cheltuieli SET magazin=@magazin WHERE data=@data AND nr_chitanta=@nr_chitanta AND tip_cheltuiala=@tip_cheltuiala AND magazin='" & mag_act & "'"
                    dbcomm = New MySqlCommand(sql_del_inc, dbconn)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@nr_chitanta", nr_chitanta)
                    dbcomm.Parameters.AddWithValue("@tip_cheltuiala", tip_cheltuiala)
                    dbcomm.Parameters.AddWithValue("@explicatii", explicatii)
                    dbcomm.Parameters.AddWithValue("@magazin", mag_nou)
                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Nu s-a sters din cheltuieli: " & ex.Message.ToString)
                End Try
            Next
            transaction.Commit()

            Load_DGV()
            Load_Sold()
            dbconn.Close()
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged

        Load_DGV()
    End Sub

    Private Sub ModificaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModificaToolStripMenuItem.Click
        Dim mag_act As String = DataGridView1.SelectedRows(0).Cells("magazin").Value
        Dim mag_nou As String = DataGridView1.SelectedRows(0).Cells("magazin").Value
        If mag_act = "PM" Then
            mag_nou = "MV"
        ElseIf mag_act = "MV" Then
            mag_nou = "PM"
        End If

        Dim row As DataGridViewRow = DataGridView1.CurrentRow

        Dim dbconn As New MySqlConnection
        Dim dbcomm As New MySqlCommand
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
        If dbconn.State = 0 Then
            dbconn.Open()
        End If


        Dim data As Date = CDate(row.Cells("data").Value)
        Dim nr_chitanta As String = row.Cells("nr_chitanta").Value.ToString
        Dim tip_cheltuiala As String = row.Cells("tip_cheltuiala").Value.ToString
        Dim explicatii As String = row.Cells("explicatii").Value
        Dim suma As Decimal = row.Cells("suma").Value
        Dim cash_cont As Boolean = row.Cells("cash").Value
        Dim id As Integer = row.Cells("id").Value


        edit_Lbl.Text = "Edit"
        'edit_Lbl.Visible = True
        id_Lbl.Text = id
        'id_Lbl.Visible = False
        GroupBox1.Visible = True

        DateTimePicker1.Value = data
        ComboBox1.SelectedIndex = ComboBox1.Items.IndexOf(tip_cheltuiala)
        nr_rzf_Textbox.Text = nr_chitanta
        explicatii_Textbox.Text = explicatii
        suma_Textbox.Text = suma
        If cash_cont = True Then
            cash_RB.Checked = True
        ElseIf cash_cont = False Then
            cash_RB.Checked = False
        End If

    End Sub
    Private Sub DataGridView1_Mousedoubleclick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDoubleClick
        Dim mag_act As String = ""
        If IsDBNull(DataGridView1.SelectedRows(0).Cells("magazin").Value) = False Then
            mag_act = DataGridView1.SelectedRows(0).Cells("magazin").Value
        End If
        'Dim mag_nou As String = DataGridView1.SelectedRows(0).Cells("magazin").Value
        'If mag_act = "PM" Then
        '    mag_nou = "MV"
        'ElseIf mag_act = "MV" Then
        '    mag_nou = "PM"
        'End If

        Dim row As DataGridViewRow = DataGridView1.CurrentRow

        Dim dbconn As New MySqlConnection
        Dim dbcomm As New MySqlCommand
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
        If dbconn.State = 0 Then
            dbconn.Open()
        End If


        Dim data As Date = CDate(row.Cells("data").Value)
        Dim nr_chitanta As String = row.Cells("nr_chitanta").Value.ToString
        Dim tip_cheltuiala As String = row.Cells("tip_cheltuiala").Value.ToString
        Dim explicatii As String = row.Cells("explicatii").Value
        Dim suma As Decimal = row.Cells("suma").Value
        Dim cash_cont As Boolean = row.Cells("cash").Value
        Dim id As Integer = row.Cells("id").Value


        edit_Lbl.Text = "Edit"
        'edit_Lbl.Visible = True
        id_Lbl.Text = id
        'id_Lbl.Visible = False
        GroupBox1.Visible = True

        DateTimePicker1.Value = data
        ComboBox1.SelectedIndex = ComboBox1.Items.IndexOf(tip_cheltuiala)
        nr_rzf_Textbox.Text = nr_chitanta
        explicatii_Textbox.Text = explicatii
        suma_Textbox.Text = suma
        If cash_cont = True Then
            cash_RB.Checked = True
        ElseIf cash_cont = False Then
            cash_RB.Checked = False
        End If

    End Sub
    
    Private Sub renunt_edit_But_Click(sender As Object, e As EventArgs) Handles renunt_edit_But.Click
        suma_Textbox.Text = 0
        data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
        explicatii_Textbox.Clear()
        nr_rzf_Textbox.Clear()
        DateTimePicker1.Value = Today
        DateTimePicker1.Focus()
        id_Lbl.Text = ""
        edit_Lbl.Text = ""
        GroupBox1.Visible = False
    End Sub

    Private Sub RB_CheckedChanged(sender As Object, e As EventArgs) Handles cash_RB.CheckedChanged ', cont_RB.CheckedChanged
        If cash_RB.Checked = True Then
            cash_RB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
            cash_RB.ForeColor = Color.Blue
            cont_RB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
            cont_RB.ForeColor = Color.Black
        Else
            cash_RB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
            cash_RB.ForeColor = Color.Black
            cont_RB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
            cont_RB.ForeColor = Color.Blue
        End If
    End Sub

    Private Sub suma_CARD_Textbox_leave(sender As Object, e As EventArgs) Handles suma_Textbox.Leave
        Dim luna As Integer = DateTimePicker1.Value.Month
        Dim an As Integer = DateTimePicker1.Value.Year

        Dim sql_read As String = "SELECT sum(suma),data FROM cheltuieli where MONTH(data)=" & luna & " AND YEAR(data)=" & an & " AND magazin='" & ComboBox3.SelectedValue & "'"
        Try
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_read, dbconn)

            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows AndAlso IsDBNull(dbread("sum(suma)")) = False Then
                Label1.Text = CDec(dbread("sum(suma)")) + CDec(suma_Textbox.Text)
            Else
                Label1.Text = "0"
            End If

        Catch ex As Exception
            MsgBox("Problem loading lables total: " & ex.Message.ToString)

        End Try
        dbread.Close()
    End Sub
    Private Sub main_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Modifiers = Keys.Control Then
            ad_firma_But.PerformClick()
        End If
    End Sub

    Private Sub incas_BUT_Click(sender As Object, e As EventArgs) Handles incas_BUT.Click


        Form_Incasari.Show()
        Form_Incasari.DateTimePicker1.Value = DateTimePicker1.Value
        Form_Incasari.ComboBox1.Focus()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim limit As String = ""
        If CheckBox2.CheckState = CheckState.Checked Then
            limit = "AND YEAR(data) = YEAR(CURRENT_DATE) AND IF(DAYOFMONTH(CURRENT_DATE)<6, MONTH(data) = (MONTH(CURRENT_DATE)-1), MONTH(data) = MONTH(CURRENT_DATE))"
            If CheckBox2.CheckState = CheckState.Unchecked Then
                limit = ""
            End If
        End If
        Dim tip As String = "AND tip_cheltuiala='" & ComboBox1.SelectedItem & "'"
        Dim sql_read As String = "SELECT * FROM cheltuieli WHERE cheltuieli.magazin='" & ComboBox3.SelectedValue & "' " & limit & " " & tip & " ORDER BY data DESC, id DESC"
        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource
        Try
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_read, dbconn)
            sda.SelectCommand = dbcomm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            DataGridView1.DataSource = bsource
            sda.Update(dbdataset)
        Catch ex As Exception
            MsgBox("Problem loading data: " & ex.Message.ToString)
        End Try
        DataGridView1.Columns("id").Visible = False
    End Sub

    Private Sub DataGridView1_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDown

        If e.Button = Windows.Forms.MouseButtons.Right And e.ColumnIndex > -1 And e.RowIndex > -1 Then
            Dim cell As DataGridViewCell = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex)
            DataGridView1.CurrentCell = cell

        End If
            If edit_Lbl.Visible = True Then
                If e.Button = Windows.Forms.MouseButtons.Left And e.ColumnIndex > -1 And e.RowIndex <> DataGridView1.CurrentRow.Index Then
                    suma_Textbox.Text = 0
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
        ' MsgBox(DataGridView1.SelectedRows.Count)
    End Sub


End Class