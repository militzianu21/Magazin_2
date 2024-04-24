Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization
Public Class Form_gestiune
    Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Public AppDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
    Private Sub Form_Gestiune_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Application.CurrentCulture = New CultureInfo("ro-RO")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("ro-RO")
        Me.KeyPreview = True

        Dim DGV As DataGridView = DataGridView1
        DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGV.ScrollBars = ScrollBars.Vertical
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.AllowUserToAddRows = False

        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")

        ComboBox3.DisplayMember = "Text"
        ComboBox3.ValueMember = "Value"
        Dim tb As New DataTable
        tb.Columns.Add("Text", GetType(String))
        tb.Columns.Add("Value", GetType(String))
        tb.Rows.Add("1. Petru Maior", "PM")
        tb.Rows.Add("2. Mihai Viteazu", "MV")
        ComboBox3.DataSource = tb
        ComboBox3.SelectedValue = Form_principal.ComboBox3.SelectedValue

        ComboBox1.Items.AddRange({"NIR", "Aviz", "Alte"})
        ComboBox1.SelectedItem = "NIR"

        explicatii_Textbox.Text = "Introduceti Firma"
        CheckBox1.CheckState = CheckState.Checked
        CheckBox2.Checked = True
        Load_DGV()

        id_Lbl.Text = ""
        edit_Lbl.Text = ""
        mag_v_Lbl.Text = ""
        rzf_v_Lbl.Text = ""
        GroupBox1.Visible = False
        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource
        Try
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If

            dbcomm = New MySqlCommand("SELECT CONCAT(firma,' ',forma_juridica) as firma FROM firme", dbconn)
            Dim lst As New List(Of String)
            dbread = dbcomm.ExecuteReader()
            While dbread.Read()
                lst.Add(dbread("firma").ToString())
            End While
            Dim mysource As New AutoCompleteStringCollection
            mysource.AddRange(lst.ToArray)
            explicatii_Textbox.AutoCompleteSource = AutoCompleteSource.CustomSource
            explicatii_Textbox.AutoCompleteCustomSource = mysource
            explicatii_Textbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        Catch ex As Exception
            MsgBox("Problem loading grid (form load) sau firme in autocomplete: " & ex.Message.ToString)
        End Try
        dbread.Close()

        Dim sql_read As String = "SELECT nr_nir,data,tip_document FROM intrari WHERE tip_document='NIR' AND data<@data ORDER BY data DESC,nr_nir DESC LIMIT 1"
        Try
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_read, dbconn)
            dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()

            nr_nir_Textbox.Text = (dbread("nr_nir").ToString) + 1

            data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)


        Catch ex As Exception
            MsgBox("Problem loading lables (form load): " & ex.Message.ToString)

        End Try

        dbread.Close()
        dbconn.Close()


        suma_Textbox.Text = "0,00"
        With DateTimePicker1
            .Focus()
            .Select()
        End With

        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
            'Else : PictureBox1.BackColor = Color.LightYellow
        End If
    End Sub
    Private Sub Load_DGV()
        Dim DGV As DataGridView = DataGridView1

        Dim limit As String = ""
        If CheckBox2.CheckState = CheckState.Checked Then
            'limit = "WHERE YEAR(data) = YEAR(CURRENT_DATE()) AND MONTH(data) = MONTH(CURRENT_DATE())"
            limit = "WHERE YEAR(data) = YEAR(CURRENT_DATE()) AND IF(DAYOFMONTH(CURRENT_DATE)<6, MONTH(data) = (MONTH(CURRENT_DATE)-1), MONTH(data) = MONTH(CURRENT_DATE))"
            If CheckBox2.CheckState = CheckState.Unchecked Then
                limit = ""
            End If
        End If
        Dim sql_tot As String = "SELECT * FROM intrari " & limit & " ORDER BY data DESC,nr_nir DESC"
        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource
        Try
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_tot, dbconn)

            sda.SelectCommand = dbcomm
            sda.Fill(dbdataset)
            bsource.DataSource = dbdataset
            DataGridView1.DataSource = bsource
            sda.Update(dbdataset)

        Catch ex As Exception
            MsgBox("Problem loading dgv: " & ex.Message.ToString)

        End Try
        'dbread.Close()
        dbconn.Close()
        DataGridView1.Columns("id").Visible = False

        DGV.Columns("data").Width = DGV.Width * 18 / 100
        DGV.Columns("data").HeaderText = "Data"
        DGV.Columns("data").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("nr_nir").Width = DGV.Width * 10 / 100
        DGV.Columns("nr_nir").HeaderText = "Nr. NIR"
        DGV.Columns("nr_nir").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("explicatii").Width = DGV.Width * 36.5 / 100
        DGV.Columns("explicatii").HeaderText = "Explicatii"
        DGV.Columns("explicatii").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("suma").Width = DGV.Width * 14 / 100
        DGV.Columns("suma").HeaderText = "Suma"
        DGV.Columns("suma").DefaultCellStyle.Format = "#,#0.##"
        DGV.Columns("suma").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DGV.Columns("tip_document").Width = DGV.Width * 10 / 100
        DGV.Columns("tip_document").HeaderText = "Tip Doc"
        DGV.Columns("tip_document").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("magazin").Width = DGV.Width * 11 / 100
        DGV.Columns("magazin").HeaderText = "Mag"
        DGV.Columns("magazin").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter


    End Sub
    Private Sub Form_Gestiune_Leave(sender As Object, e As EventArgs) Handles Me.FormClosed
        CAPS_OFF()
        Form_principal.Show()
        Form_principal.Load_Intrari()
        Form_principal.Load_Iesiri()
        Form_principal.Load_Situatie()

    End Sub
    Private Sub save_BU_Click(sender As Object, e As EventArgs) Handles save_BU.Click

        data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)

        Dim tip_document As String = ComboBox1.SelectedItem
        Dim nr_nir As String = nr_nir_Textbox.Text
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

        If tip_document = "NIR" Then
            nr_nir = nr_nir_Textbox.Text
        End If
        Dim sql As String = "INSERT INTO intrari(data,tip_document,nr_nir,explicatii,suma,magazin) " _
                            & "VALUES(@data,@tip_document,@nr_nir,@explicatii,@suma,@magazin)"
        If edit_Lbl.Text = "Edit" Then
            sql = "UPDATE intrari SET data=@data,tip_document=@tip_document,explicatii=@explicatii,suma=@suma,magazin=@magazin WHERE id=@id"
        End If
        Try
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql, dbconn)
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value.Date)
            dbcomm.Parameters.AddWithValue("@tip_document", tip_document)
            dbcomm.Parameters.AddWithValue("@nr_nir", nr_nir)
            dbcomm.Parameters.AddWithValue("@explicatii", explicatii_Textbox.Text)

            dbcomm.Parameters.AddWithValue("@id", id_Lbl.Text)


            If tip_document = "Aviz" Then
                If suma < 0 Then
                    suma = suma
                ElseIf suma > 0 Then
                    suma = Decimal.Negate(suma)
                End If
            End If
            dbcomm.Parameters.AddWithValue("@suma", CDec(suma))
            dbcomm.Parameters.AddWithValue("@magazin", ComboBox3.SelectedValue)
            dbcomm.ExecuteNonQuery()
            transaction.Commit()
        Catch ex As Exception
            MsgBox("Failed to insert data: " & ex.Message.ToString())
        End Try
        dbconn.Close()
        If tip_document = "Aviz" Then
            With Form_Aviz
                If ComboBox3.SelectedValue = "PM" Then
                    .Label1.Text = "MV"
                    .Label2.Text = "Mihai Viteazu"
                ElseIf ComboBox3.SelectedValue = "MV" Then
                    .Label1.Text = "PM"
                    .Label2.Text = "Petru Maior"
                End If
                .DateTimePicker1.Value = DateTimePicker1.Value
                .suma_Textbox.Text = Math.Abs(CDec(suma))
            End With
            Form_Aviz.ShowDialog()
        End If
        Dim sql_read As String = "SELECT * FROM intrari ORDER BY data DESC,nr_nir DESC"
        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource
        Try
            If dbconn.State = ConnectionState.Closed Then
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
        dbread.Close()
        dbconn.Close()
        Label1.Text = ""
        Label1.Visible = False
        id_Lbl.Text = ""
        id_Lbl.Visible = False

        If IsNumeric(nr_nir_Textbox.Text) Then
            nr_nir_Textbox.Text = nr_nir_Textbox.Text + 1
        End If
        suma_Textbox.Text = 0
        explicatii_Textbox.Text = "Introduceti Firma"
        explicatii_Textbox.Select()
        explicatii_Textbox.Focus()

        Dim DGV As DataGridView = DataGridView1
        For i = 0 To DGV.Rows.Count - 1
            If DGV.Rows(i).Cells("nr_nir").Value = nr_nir Then
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
    Private Sub TextBox_Suma_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles suma_Textbox.KeyPress
        If Not Char.IsDigit(e.KeyChar) Then e.Handled = True
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 45 Or ((e.KeyChar = "." Or e.KeyChar = ",") And (sender.Text.IndexOf(".") = -1 And sender.Text.IndexOf(",") = -1)))
        If Asc(e.KeyChar) = 46 Then
            e.KeyChar = ChrW(44)
        End If
    End Sub
    Private Sub TextBox_NrNIR_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles nr_nir_Textbox.KeyPress
        If ComboBox1.SelectedItem = "NIR" Then
            If Not Char.IsDigit(e.KeyChar) Then e.Handled = True
            e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8)
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.DropDownClosed
        explicatii_Textbox.Text = ""
        Dim tip_document As String = ComboBox1.SelectedItem

        Dim nr_nir As String = nr_nir_Textbox.Text
        Dim explicatii As String = explicatii_Textbox.Text
        Dim suma As String = suma_Textbox.Text
        If tip_document = "NIR" Then
            If CheckBox1.CheckState = CheckState.Checked Then
                data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
                explicatii_Textbox.Text = "Introduceti Firma"
                Dim sql_read As String = "SELECT nr_nir,data FROM intrari where tip_document='NIR' ORDER BY data DESC LIMIT 1"
                Try
                    If dbconn.State = ConnectionState.Closed Then
                        dbconn.Open()
                    End If
                    dbcomm = New MySqlCommand(sql_read, dbconn)
                    dbread = dbcomm.ExecuteReader()
                    dbread.Read()
                    If dbread.HasRows Then
                        nr_nir_Textbox.Text = (dbread("nr_nir").ToString) + 1
                        explicatii_Textbox.Select()
                        explicatii_Textbox.Focus()
                    End If
                Catch ex As Exception
                    MsgBox("Problem loading data: " & ex.Message.ToString)
                End Try
                dbread.Close()
                dbconn.Close()
            End If
        End If
        If tip_document = "Aviz" Then
            If CheckBox1.CheckState = CheckState.Checked Then
                data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
                explicatii_Textbox.Text = "Transfer Marfa"
                Dim sql_read As String = "SELECT nr_nir,data FROM intrari where tip_document='Aviz' ORDER BY data DESC LIMIT 1"
                Try
                    If dbconn.State = ConnectionState.Closed Then
                        dbconn.Open()
                    End If
                    dbcomm = New MySqlCommand(sql_read, dbconn)
                    dbread = dbcomm.ExecuteReader()
                    dbread.Read()
                    If dbread.HasRows Then
                        nr_nir_Textbox.Text = (dbread("nr_nir").ToString) + 1
                    ElseIf dbread.HasRows = False Then
                        nr_nir_Textbox.Text = 1
                    End If
                Catch ex As Exception
                    MsgBox("Problem loading data: " & ex.Message.ToString)
                End Try

                suma_Textbox.Select()
                suma_Textbox.Focus()

                dbread.Close()
                dbconn.Close()
            End If
        End If


    End Sub

    Private Sub StergeInregistrareaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StergeIntrareaToolStripMenuItem.Click

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
                Dim id As Integer = CInt(DataGridView1.SelectedRows(i).Cells("id").Value)
                Dim data As Date = CDate(DataGridView1.SelectedRows(i).Cells("data").Value)
                Dim nr_nir As String = DataGridView1.SelectedRows(i).Cells("nr_nir").Value.ToString
                'Dim tip_document As String = DataGridView1.SelectedRows(i).Cells("tip_document").Value.ToString
                'Dim explicatii As String = DataGridView1.SelectedRows(i).Cells("explicatii").Value
                'Dim magazin As String = DataGridView1.SelectedRows(i).Cells("magazin").Value

                Try
                    Dim sql_del_int As String = "DELETE FROM intrari WHERE id=@id AND data=@data AND nr_nir=@nr_nir"
                    dbcomm = New MySqlCommand(sql_del_int, dbconn)

                    dbcomm.Parameters.AddWithValue("@id", ID)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@nr_nir", nr_nir)
                    'dbcomm.Parameters.AddWithValue("@tip_document", tip_document)
                    'dbcomm.Parameters.AddWithValue("@explicatii", explicatii)
                    'dbcomm.Parameters.AddWithValue("@magazin", magazin)
                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Nu s-a sters din intrari: " & ex.Message.ToString)
                End Try

                Try
                    Dim sql_del_nir As String = "DELETE FROM niruri WHERE data=@data AND nir=@nir"
                    dbcomm = New MySqlCommand(sql_del_nir, dbconn)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@nir", nr_nir)
                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Nu s-a sters din niruri: " & ex.Message.ToString)
                End Try

                Try
                    Dim sql_del_mrf As String = "DELETE FROM marfa WHERE data=@data AND nir=@nir"
                    dbcomm = New MySqlCommand(sql_del_mrf, dbconn)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@nir", nr_nir)
                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Nu s-a sters din marfa: " & ex.Message.ToString)
                End Try
            Next
            transaction.Commit()

            Load_DGV()
            dbread.Close()
            dbconn.Close()
        End If
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
    End Sub

    Private Sub suma_Textbox_TextChanged(sender As Object, e As EventArgs) Handles suma_Textbox.TextChanged
        If suma_Textbox.Text = "" Then
            suma_Textbox.Text = 0
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
    Private Sub nr_rzf_Textbox_focused(sender As Object, e As EventArgs) Handles nr_nir_Textbox.GotFocus, nr_nir_Textbox.MouseClick
        nr_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        nr_Label.ForeColor = Color.Blue
        nr_nir_Textbox.SelectAll()
    End Sub
    Private Sub nr_rzf_Textbox_lostFocus(sender As Object, e As EventArgs) Handles nr_nir_Textbox.LostFocus
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
    Private Sub suma_Textbox_focused(sender As Object, e As EventArgs) Handles suma_Textbox.GotFocus, suma_Textbox.MouseClick
        suma_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        suma_Label.ForeColor = Color.Blue
        suma_Textbox.SelectAll()
    End Sub
    Private Sub suma_Textbox_lostFocus(sender As Object, e As EventArgs) Handles suma_Textbox.LostFocus
        suma_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        suma_Label.ForeColor = Color.Black
    End Sub
    Private Sub enter_Keypress(ByVal sender As System.Object, ByVal e As KeyPressEventArgs) Handles suma_Textbox.KeyPress, DateTimePicker1.KeyPress, ComboBox1.KeyPress, nr_nir_Textbox.KeyPress
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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
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

                dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
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
        End If
    End Sub

    Private Sub explicatii_Textbox_Enter(sender As Object, e As EventArgs) Handles explicatii_Textbox.Enter
        CAPS_ON()
    End Sub
    'Private Sub explicatii_Textbox_leave(sender As Object, e As EventArgs) Handles explicatii_Textbox.Leave
    '    CAPS_OFF()
    'End Sub

    Private Sub ComboBox3_DropDownClosed(sender As Object, e As EventArgs) Handles ComboBox3.DropDownClosed
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


    Private Sub ModificaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModificaIntrareToolStripMenuItem.Click

        Dim row As DataGridViewRow = DataGridView1.CurrentRow
        If IsDBNull(row.Cells("id").Value) = False Then
            Dim mag_act As String = DataGridView1.SelectedRows(0).Cells("magazin").Value
            Dim mag_nou As String = DataGridView1.SelectedRows(0).Cells("magazin").Value
            If mag_act = "PM" Then
                mag_nou = "MV"
            ElseIf mag_act = "MV" Then
                mag_nou = "PM"
            End If


            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            If dbconn.State = 0 Then
                dbconn.Open()
            End If


            Dim data As Date = CDate(row.Cells("data").Value)
            Dim nr_nir As String = row.Cells("nr_nir").Value.ToString
            Dim tip_document As String = row.Cells("tip_document").Value.ToString
            Dim explicatii As String = row.Cells("explicatii").Value
            Dim suma As Decimal = row.Cells("suma").Value
            Dim id As Integer = row.Cells("id").Value
            Dim mag As String = row.Cells("magazin").Value.ToString

            'Label1.Text = "Edit"
            'Label1.Visible = True
            'id_Lbl.Text = id
            'id_Lbl.Visible = False

            ComboBox3.SelectedValue = mag
            'DateTimePicker1.Value = data
            'ComboBox1.SelectedIndex = ComboBox1.Items.IndexOf(tip_document)
            'nr_nir_Textbox.Text = nr_nir
            'explicatii_Textbox.Text = explicatii
            'suma_Textbox.Text = suma
            If ComboBox3.SelectedValue = "PM" Then
                PictureBox1.BackColor = Color.OrangeRed
            ElseIf ComboBox3.SelectedValue = "MV" Then
                PictureBox1.BackColor = Color.Turquoise
            Else : PictureBox1.BackColor = Color.LightYellow
            End If

            edit_Lbl.Text = "Edit"
            id_Lbl.Text = id
            mag_v_Lbl.Text = mag_act
            rzf_v_Lbl.Text = row.Cells("nr_nir").Value.ToString
            GroupBox1.Visible = True

            DateTimePicker1.Value = data
            ComboBox1.SelectedIndex = ComboBox1.Items.IndexOf(tip_document)
            nr_nir_Textbox.Text = nr_nir
            explicatii_Textbox.Text = explicatii
            suma_Textbox.Text = suma
        End If

    End Sub
    Private Sub renunt_edit_But_Click(sender As Object, e As EventArgs) Handles renunt_edit_But.Click
        suma_Textbox.Text = 0
        data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
        explicatii_Textbox.Clear()
        nr_nir_Textbox.Clear()
        DateTimePicker1.Value = Today
        DateTimePicker1.Focus()
        id_Lbl.Text = ""
        edit_Lbl.Text = ""
        GroupBox1.Visible = False
    End Sub
    Private Sub DataGridView1_Mousedoubleclick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDoubleClick
        'If e.Button = Windows.Forms.MouseButtons.Right And e.ColumnIndex > -1 And e.RowIndex > -1 Then
        Dim row As DataGridViewRow = DataGridView1.CurrentRow
        If IsDBNull(row.Cells("id").Value) = False Then
            Dim mag_act As String = DataGridView1.SelectedRows(0).Cells("magazin").Value
            Dim mag_nou As String = DataGridView1.SelectedRows(0).Cells("magazin").Value
            If mag_act = "PM" Then
                mag_nou = "MV"
            ElseIf mag_act = "MV" Then
                mag_nou = "PM"
            End If


            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            If dbconn.State = 0 Then
                dbconn.Open()
            End If


            Dim data As Date = CDate(row.Cells("data").Value)
            Dim nr_nir As String = row.Cells("nr_nir").Value.ToString
            Dim tip_document As String = row.Cells("tip_document").Value.ToString
            Dim explicatii As String = row.Cells("explicatii").Value
            Dim suma As Decimal = row.Cells("suma").Value
            Dim id As Integer = row.Cells("id").Value
            Dim mag As String = row.Cells("magazin").Value.ToString

            'Label1.Text = "Edit"
            'Label1.Visible = True
            'id_Lbl.Text = id
            'id_Lbl.Visible = False

            ComboBox3.SelectedValue = mag
            'DateTimePicker1.Value = data
            'ComboBox1.SelectedIndex = ComboBox1.Items.IndexOf(tip_document)
            'nr_nir_Textbox.Text = nr_nir
            'explicatii_Textbox.Text = explicatii
            'suma_Textbox.Text = suma
            If ComboBox3.SelectedValue = "PM" Then
                PictureBox1.BackColor = Color.OrangeRed
            ElseIf ComboBox3.SelectedValue = "MV" Then
                PictureBox1.BackColor = Color.Turquoise
            Else : PictureBox1.BackColor = Color.LightYellow
            End If

            edit_Lbl.Text = "Edit"
            id_Lbl.Text = id
            mag_v_Lbl.Text = mag_act
            rzf_v_Lbl.Text = row.Cells("nr_nir").Value.ToString
            GroupBox1.Visible = True

            DateTimePicker1.Value = data
            ComboBox1.SelectedIndex = ComboBox1.Items.IndexOf(tip_document)
            nr_nir_Textbox.Text = nr_nir
            explicatii_Textbox.Text = explicatii
            suma_Textbox.Text = suma
        End If
        'End If
    End Sub
    Private Sub DataGridView1_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDown
        If e.Button = Windows.Forms.MouseButtons.Right And e.ColumnIndex > -1 And e.RowIndex > -1 Then
            Dim cell As DataGridViewCell = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex)
            DataGridView1.CurrentCell = cell
        End If
        If edit_Lbl.Visible = True Then
            If e.Button = Windows.Forms.MouseButtons.Left And e.ColumnIndex > -1 AndAlso e.RowIndex <> DataGridView1.CurrentRow.Index AndAlso e.RowIndex > -1 Then
                suma_Textbox.Text = 0
                data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
                explicatii_Textbox.Clear()
                nr_nir_Textbox.Clear()
                DateTimePicker1.Value = Today
                'DateTimePicker1.Focus()
                id_Lbl.Text = ""
                edit_Lbl.Text = ""
                GroupBox1.Visible = False
            End If
        End If
    End Sub
    Private Sub main_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Modifiers = Keys.Control Then
            Button2.PerformClick()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form_NIRuri.Show()
    End Sub

    Private Sub ArataModificaNIRToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ArataModificaNIRToolStripMenuItem.Click
        Dim row As DataGridViewRow = DataGridView1.CurrentRow

        Dim data As Date = CDate(row.Cells("data").Value)
        Dim nir As String = row.Cells("nr_nir").Value.ToString
        Dim mag As String = row.Cells("magazin").Value.ToString

        With Form_afiseaza_nir
            .nir_TB.Text = nir
            .DateTimePicker1.Value = data

        End With
        Form_afiseaza_nir.Show()
        Form_afiseaza_nir.ComboBox3.SelectedValue = mag
    End Sub
End Class
