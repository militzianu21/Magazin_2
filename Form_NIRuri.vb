Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization
Public Class Form_NIRuri
    Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Public AppDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
    Private Sub Form_marfa_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Application.CurrentCulture = New CultureInfo("ro-RO")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("ro-RO")

        Dim DGV As DataGridView = DataGridView1
        DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        'DGV.ScrollBars = ScrollBars.Vertical
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.AllowUserToAddRows = False
        'dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")

        'ComboBox3.DisplayMember = "Text"
        'ComboBox3.ValueMember = "Value"
        Dim tb As New DataTable
        tb.Columns.Add("Text", GetType(String))
        tb.Columns.Add("Value", GetType(String))
        tb.Rows.Add("1. Petru Maior", "PM")
        tb.Rows.Add("2. Mihai Viteazu", "MV")

        tip_nir_CB.DisplayMember = "Text"
        tip_nir_CB.ValueMember = "Value"
        Dim tbn As New DataTable
        tbn.Columns.Add("Text", GetType(String))
        tbn.Columns.Add("Value", GetType(String))
        tbn.Rows.Add("1. Marfa", "M")
        'tbn.Rows.Add("2. Obiecte inventar", "OI")
        'tbn.Rows.Add("3. Consumabile", "CO")
        tip_nir_CB.DataSource = tbn
        'ComboBox3.DataSource = tb


        'bucati_Textbox.Text = ""


        'Dim sql_tot As String = "SELECT * FROM marfa ORDER BY data DESC,nir DESC"
        'Dim sda As New MySqlDataAdapter
        'Dim dbdataset As New DataTable
        'Dim bsource As New BindingSource
        'Try
        '    If dbconn.State = ConnectionState.Closed Then
        '        dbconn.Open()
        '    End If
        '    dbcomm = New MySqlCommand(sql_tot, dbconn)

        '    sda.SelectCommand = dbcomm
        '    sda.Fill(dbdataset)
        '    bsource.DataSource = dbdataset
        '    DataGridView1.DataSource = bsource
        '    sda.Update(dbdataset)

        '    dbcomm = New MySqlCommand("SELECT produs FROM marfa GROUP BY produs", dbconn)
        '    Dim lst As New List(Of String)
        '    dbread = dbcomm.ExecuteReader()
        '    While dbread.Read()
        '        lst.Add(dbread("produs").ToString())
        '    End While
        '    Dim mysource As New AutoCompleteStringCollection
        '    mysource.AddRange(lst.ToArray)
        '    produs_Textbox.AutoCompleteSource = AutoCompleteSource.CustomSource
        '    produs_Textbox.AutoCompleteCustomSource = mysource
        '    produs_Textbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        'Catch ex As Exception
        '    MsgBox("Problem loading grid (form load) sau produse in autocomplete: " & ex.Message.ToString)
        'End Try
        'dbread.Close()

        'Dim sql_read As String = "SELECT nir,data FROM marfa ORDER BY data DESC,nir DESC LIMIT 1"
        'Try
        '    If dbconn.State = ConnectionState.Closed Then
        '        dbconn.Open()
        '    End If
        '    dbcomm = New MySqlCommand(sql_read, dbconn)

        '    dbread = dbcomm.ExecuteReader()
        '    dbread.Read()

        '    nr_nir_Textbox.Text = (dbread("nir").ToString) + 1

        '    data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)


        'Catch ex As Exception
        '    MsgBox("Problem loading lables (form load): " & ex.Message.ToString)

        'End Try
        'dbread.Close()
        'dbconn.Close()

        'For Each DataGridViewColumn In DataGridView1.Columns
        '    DataGridView1.Columns("id").Visible = False
        'Next
        'bucati_Textbox.Text = "1"
        'pret_Textbox.Text = "0"
        'With DateTimePicker1
        '    .Focus()
        '    .Select()
        'End With
        'If ComboBox3.SelectedValue = "PM" Then
        '    PictureBox1.BackColor = Color.OrangeRed
        'ElseIf ComboBox3.SelectedValue = "MV" Then
        '    PictureBox1.BackColor = Color.Turquoise
        'Else : PictureBox1.BackColor = Color.LightYellow
        'End If
        Load_niruri()
        'AddHandler ComboBox3.SelectedValueChanged, AddressOf ComboBox3_SelectedValueChanged
    End Sub

    Public Sub Load_niruri()

        'Dim tip As String = tip_nir_CB.SelectedValue.ToString
        Dim tip_nir As String = "niruri"
        'If tip <> "M" Then
        '    tip_nir = "niruri_obiecte"
        'End If

        Dim DGV As DataGridView = DataGridView1
        DGV.Columns.Clear()

        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
        Dim sql_tot As String = "SELECT * FROM " & tip_nir & " ORDER BY data DESC,nir DESC"
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
            MsgBox("Problem loading lables (form load): " & ex.Message.ToString)

        End Try



        DGV.Columns("id").Visible = False

        DGV.Columns("nir").FillWeight = DGV.Width * 7 / 100
        DGV.Columns("nir").HeaderText = "NIR"
        DGV.Columns("nir").DefaultCellStyle.BackColor = Color.Pink
        DGV.Columns("nir").DefaultCellStyle.SelectionBackColor = Color.HotPink
        DGV.Columns("nir").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("nir").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("nir").DefaultCellStyle.Font = New Font(DGV.DefaultCellStyle.Font, FontStyle.Bold)
        DGV.Columns("data").FillWeight = DGV.Width * 15 / 100
        DGV.Columns("data").HeaderText = "Data"
        DGV.Columns("data").DefaultCellStyle.BackColor = Color.Azure
        DGV.Columns("data").DefaultCellStyle.SelectionBackColor = Color.LightBlue
        DGV.Columns("data").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("data").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("valoare").FillWeight = DGV.Width * 13 / 100
        DGV.Columns("valoare").HeaderText = "Valoare"
        DGV.Columns("valoare").DefaultCellStyle.BackColor = Color.LightGreen
        DGV.Columns("valoare").DefaultCellStyle.SelectionBackColor = Color.LightSeaGreen
        DGV.Columns("valoare").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("valoare").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DGV.Columns("valoare").DefaultCellStyle.Font = New Font(DGV.DefaultCellStyle.Font, FontStyle.Bold)
        DGV.Columns("valoare").DefaultCellStyle.Format = "#,#0.##"
        DGV.Columns("tva").FillWeight = DGV.Width * 5 / 100
        DGV.Columns("tva").HeaderText = "TVA"
        DGV.Columns("tva").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("nr_factura").FillWeight = DGV.Width * 10 / 100
        DGV.Columns("nr_factura").HeaderText = "Nr. Factura"
        DGV.Columns("nr_factura").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DGV.Columns("nume_firma").FillWeight = DGV.Width * 25 / 100
        DGV.Columns("nume_firma").HeaderText = "Firma"
        DGV.Columns("nume_firma").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("cif_firma").FillWeight = DGV.Width * 10 / 100
        DGV.Columns("cif_firma").HeaderText = "Cif Firma"
        DGV.Columns("cif_firma").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("magazin").FillWeight = DGV.Width * 5 / 100
        DGV.Columns("magazin").HeaderText = "Mag"
        DGV.Columns("magazin").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter


        'dbread.Close()
        dbconn.Close()
    End Sub
    Public Sub Load_niruri_obiecte()
        Dim DGV As DataGridView = DataGridView1
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
        Dim sql_tot As String = "SELECT * FROM niruri_obiecte ORDER BY data DESC,nir DESC"
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
            MsgBox("Problem loading lables (form load): " & ex.Message.ToString)

        End Try



        DGV.Columns("id").Visible = False

        DGV.Columns("nir").FillWeight = DGV.Width * 7 / 100
        DGV.Columns("nir").HeaderText = "NIR"
        DGV.Columns("nir").DefaultCellStyle.BackColor = Color.Pink
        DGV.Columns("nir").DefaultCellStyle.SelectionBackColor = Color.HotPink
        DGV.Columns("nir").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("nir").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("nir").DefaultCellStyle.Font = New Font(DGV.DefaultCellStyle.Font, FontStyle.Bold)
        DGV.Columns("data").FillWeight = DGV.Width * 15 / 100
        DGV.Columns("data").HeaderText = "Data"
        DGV.Columns("data").DefaultCellStyle.BackColor = Color.Azure
        DGV.Columns("data").DefaultCellStyle.SelectionBackColor = Color.LightBlue
        DGV.Columns("data").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("data").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("valoare").FillWeight = DGV.Width * 13 / 100
        DGV.Columns("valoare").HeaderText = "Valoare"
        DGV.Columns("valoare").DefaultCellStyle.BackColor = Color.LightGreen
        DGV.Columns("valoare").DefaultCellStyle.SelectionBackColor = Color.LightSeaGreen
        DGV.Columns("valoare").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("valoare").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DGV.Columns("valoare").DefaultCellStyle.Font = New Font(DGV.DefaultCellStyle.Font, FontStyle.Bold)
        DGV.Columns("valoare").DefaultCellStyle.Format = "#,#0.##"
        DGV.Columns("tva").FillWeight = DGV.Width * 5 / 100
        DGV.Columns("tva").HeaderText = "TVA"
        DGV.Columns("tva").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("nr_factura").FillWeight = DGV.Width * 10 / 100
        DGV.Columns("nr_factura").HeaderText = "Nr. Factura"
        DGV.Columns("nr_factura").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DGV.Columns("nume_firma").FillWeight = DGV.Width * 25 / 100
        DGV.Columns("nume_firma").HeaderText = "Firma"
        DGV.Columns("nume_firma").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("cif_firma").FillWeight = DGV.Width * 10 / 100
        DGV.Columns("cif_firma").HeaderText = "Cif Firma"
        DGV.Columns("cif_firma").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("magazin").FillWeight = DGV.Width * 5 / 100
        DGV.Columns("magazin").HeaderText = "Mag"
        DGV.Columns("magazin").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter


        'dbread.Close()
        dbconn.Close()
    End Sub
    Private Sub DataGridView1_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDown
        If e.Button = Windows.Forms.MouseButtons.Right And e.ColumnIndex > -1 And e.RowIndex > -1 Then
            Dim cell As DataGridViewCell = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex)
            DataGridView1.CurrentCell = cell
        End If
    End Sub

    Private Sub StergeInregistrareaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StergeInregistrareaToolStripMenuItem.Click

        Dim yesno As Integer = MsgBox("Stergi NIR-ul?", MsgBoxStyle.YesNo)

        Dim row As DataGridViewRow = DataGridView1.CurrentRow
        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then

            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")

            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()

            For i = 0 To DataGridView1.SelectedRows.Count - 1
                Dim data As Date = CDate(DataGridView1.SelectedRows(i).Cells("data").Value)
                Dim nir As String = DataGridView1.SelectedRows(i).Cells("nir").Value.ToString
                Dim id As String = DataGridView1.SelectedRows(i).Cells("id").Value
                Try
                    Dim sql_del_nir As String = "DELETE FROM niruri WHERE id=@id AND nir=@nir"
                    dbcomm = New MySqlCommand(sql_del_nir, dbconn)
                    dbcomm.Parameters.AddWithValue("@nir", nir)
                    dbcomm.Parameters.AddWithValue("@id", id)

                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Nu s-a sters din niruri: " & ex.Message.ToString)
                End Try

                Try
                    Dim sql_del_mrf As String = "DELETE FROM marfa WHERE data=@data AND nir=@nir"
                    dbcomm = New MySqlCommand(sql_del_mrf, dbconn)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@nir", nir)
                    dbcomm.Parameters.AddWithValue("@id", id)

                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Nu s-a sters din marfa: " & ex.Message.ToString)
                End Try

                Try
                    Dim sql_del_int As String = "DELETE FROM intrari WHERE data=@data AND nr_nir=@nr_nir"
                    dbcomm = New MySqlCommand(sql_del_int, dbconn)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@nr_nir", nir)


                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Nu s-a sters din intrari: " & ex.Message.ToString)
                End Try
            Next
            transaction.Commit()

            Load_niruri()
        End If
    End Sub

    Private Sub ArataNIRToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ArataNIRToolStripMenuItem.Click, DataGridView1.CellDoubleClick
        Dim row As DataGridViewRow = DataGridView1.CurrentRow

        Dim data As Date = CDate(row.Cells("data").Value)
        Dim nir As String = row.Cells("nir").Value.ToString
        Dim mag As String = row.Cells("magazin").Value.ToString
        Dim tip As String = tip_nir_CB.SelectedValue.ToString
        Dim tip_nir As String = "niruri"

        If tip = "M" Then
            With Form_afiseaza_nir

                .nir_TB.Text = nir
                .DateTimePicker1.Value = data

            End With
            Form_afiseaza_nir.Show()
            Form_afiseaza_nir.ComboBox3.SelectedValue = mag

        Else
            With Form_afiseaza_obinv

                .nir_TB.Text = nir
                .DateTimePicker1.Value = data

            End With
            Form_afiseaza_obinv.Show()
            Form_afiseaza_obinv.ComboBox3.SelectedValue = mag
        End If



    End Sub

    Private Sub Ad_nir_But_Click(sender As Object, e As EventArgs) Handles ad_nir_But.Click
        Form_adauga_nir.Show()
    End Sub

    Private Sub Tip_nir_CB_DropdownClosed(sender As Object, e As EventArgs) Handles tip_nir_CB.DropDownClosed
        Load_niruri()
    End Sub
End Class