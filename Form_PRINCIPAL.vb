Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Data.OleDb
Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing.Layout
Imports Microsoft.VisualBasic.FileIO


Public Class Form_principal
    Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Public dbread2 As MySqlDataReader
    'Public dbread3 As MySqlDataReader
    Public AppDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
    Public folder_registru As String
    Public folder_raport As String
    Public folder_di As String
    Public folder_dp As String
    Public folder_nir As String
    Public folder_facturi As String
    

    'insert into nir (data,nir,nume_firma,select if(explicatii='Transfer Marfa',explicatii,substring(explicatii,1,length(explicatii)-7)) from intrari where tip_document='NIR';

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'If System.IO.Directory.Exists(AppDataFolder & "\Magazin") = False Then
        '    System.IO.Directory.CreateDirectory(AppDataFolder & "\Magazin")
        'End If
        Dim dbconn As MySqlConnection = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        Try
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
        Catch ex As Exception
            MsgBox("Problem conecting to mysql: " & ex.Message.ToString)
        End Try
        Panel1.Visible = False
        gestiune_verif_button.Enabled = False
        casa_verif_button.Enabled = False


        Load_Setari()
        If System.IO.Directory.Exists(folder_registru) = False Then
            With Form_input
                .Label2.Text = "path_registru"
                .Text = "MODIFICA"
                .TextBox1.Text = folder_registru
                Dim valu As String = folder_registru
            End With
            Form_input.ShowDialog()
            If Form_input.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim setting As String = Form_input.Label2.Text
                Dim value As String = Form_input.TextBox1.Text
                Dim sql_upd As String = "UPDATE setari SET valoare = @valoare WHERE setare=@setare"
                Try
                    'Dim dbconn As New MySqlConnection

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
            Else : MsgBox("Nu s-a modificat nimic")
            End If

            'End If
            Form_input.Dispose()
            Load_Setari()
        End If
        Load_Documente()

        Me.Text = "MAGAZIN"

        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        Nonquery("CREATE DATABASE IF NOT EXISTS magazin")

        Nonquery("CREATE TABLE IF NOT EXISTS incasari (" _
                 & "`id` int(11) NOT NULL AUTO_INCREMENT," _
                 & "`data` date NOT NULL," _
                 & "`nr_rzf` varchar(255) NOT NULL," _
                 & "`tip_incasare` varchar(10) NOT NULL," _
                 & "`explicatii` varchar(500) NOT NULL DEFAULT '-'," _
                 & "`suma_cash` double DEFAULT NULL," _
                 & "`suma_card` double DEFAULT NULL," _
                 & "`magazin` varchar(15) DEFAULT NULL," _
                 & "PRIMARY KEY (`id`)," _
                 & "UNIQUE KEY `uk_data_magazin` (`data`,`magazin`)" _
                 & ")")
        Nonquery("CREATE TABLE IF NOT EXISTS cheltuieli (" _
                 & "`id` int(11) NOT NULL AUTO_INCREMENT," _
                 & "`data` date NOT NULL," _
                 & "`nr_chitanta` varchar(255) NOT NULL," _
                 & "`tip_cheltuiala` varchar(10) NOT NULL," _
                 & "`explicatii` varchar(500) NOT NULL," _
                 & "`suma` double DEFAULT NULL," _
                 & "`magazin` varchar(15) DEFAULT NULL," _
                 & "`cash` tinyint(1) NOT NULL DEFAULT '1'," _
                 & " PRIMARY KEY (`id`)," _
                 & " UNIQUE KEY `uk_data_nr_chitanta_explicatii` (`data`,`nr_chitanta`,`explicatii`)" _
                 & ")")
        Nonquery("CREATE TABLE IF NOT EXISTS setari (" _
                 & "setare VARCHAR(255)  NOT NULL," _
                 & "valoare VARCHAR(500)  NOT NULL, " _
                 & "UNIQUE (setare)" _
                 & ")")
        Nonquery("CREATE TABLE IF NOT EXISTS intrari (" _
                 & "id INT(11) NOT NULL AUTO_INCREMENT," _
                 & "data DATE NOT NULL," _
                 & "nr_nir VARCHAR(15)  NOT NULL," _
                 & "explicatii VARCHAR(500)  NOT NULL DEFAULT '-'," _
                 & "suma DOUBLE NOT NULL DEFAULT '0'," _
                 & "tip_document VARCHAR(5) DEFAULT 'NIR'," _
                 & "magazin VARCHAR(15) DEFAULT NULL," _
                 & "PRIMARY KEY (id)," _
                 & "UNIQUE KEY (nr_nir)" _
                 & ")")
        Nonquery("CREATE TABLE IF NOT EXISTS iesiri (" _
                 & "`id` int(11) NOT NULL AUTO_INCREMENT," _
                 & "`data` date NOT NULL," _
                 & "`nr_rzf` varchar(255) NOT NULL," _
                 & "`tip_incasare` varchar(10) NOT NULL," _
                 & "`explicatii` varchar(500) NOT NULL DEFAULT '-'," _
                 & "`suma` double NOT NULL DEFAULT '0'," _
                 & "`magazin` varchar(15) DEFAULT NULL," _
                 & " PRIMARY KEY (`id`)," _
                 & " UNIQUE KEY `uk_data_magazin` (`data`,`magazin`)" _
                 & ")")
        Nonquery("CREATE TABLE IF NOT EXISTS solduri_casa (" _
                & "data DATE NOT NULL," _
                & "casa_sold_initial DOUBLE NOT NULL DEFAULT '0'," _
                & "incasari DOUBLE NOT NULL DEFAULT '0'," _
                & "cheltuieli DOUBLE NOT NULL DEFAULT '0'," _
                & "casa_sold_final DOUBLE NOT NULL DEFAULT '0'," _
                & "magazin VARCHAR(15)," _
                & "permanent BOOLEAN NOT NULL DEFAULT FALSE," _
                & "UNIQUE KEY `data_magazin` (`data`,`magazin`)" _
                & ")")
        Nonquery("CREATE TABLE IF NOT EXISTS solduri_gestiune (" _
                & "data DATE NOT NULL," _
                & "gestiune_sold_initial DOUBLE NOT NULL DEFAULT '0'," _
                & "intrari DOUBLE NOT NULL DEFAULT '0'," _
                & "iesiri DOUBLE NOT NULL DEFAULT '0'," _
                & "gestiune_sold_final DOUBLE NOT NULL DEFAULT '0'," _
                & "magazin VARCHAR(15)," _
                & "permanent BOOLEAN NOT NULL DEFAULT FALSE," _
                & "UNIQUE KEY `data_magazin` (`data`,`magazin`)" _
                & ")")
        Nonquery("CREATE TABLE IF NOT EXISTS firme (" _
                & "id int(10) unsigned NOT NULL AUTO_INCREMENT," _
                & "firma VARCHAR(255) NOT NULL," _
                & "cui INT(10) UNSIGNED DEFAULT NULL," _
                & "j VARCHAR(255) DEFAULT NULL," _
                & "forma_juridica VARCHAR(10) DEFAULT NULL," _
                & "adresa VARCHAR(500) DEFAULT NULL," _
                & "tip VARCHAR(255) DEFAULT NULL," _
                & "status VARCHAR(255) DEFAULT NULL," _
                & "cont VARCHAR(24) DEFAULT NULL," _
                & "banca VARCHAR(255) DEFAULT NULL," _
                & "PRIMARY KEY (`id`)," _
                & "UNIQUE KEY `firma` (`firma`)" _
                & ")")
        'insert into niruri (nir,data,valoare,nume_firma,magazin) select nr_nir,data,suma,if(explicatii='Transfer Marfa',explicatii,substring(explicatii,1,length(explicatii)-7)),magazin from intrari where tip_document='NIR'
        'update niruri join firme on niruri.nume_firma=firme.firma set niruri.cif_firma=firme.cui
        Nonquery("CREATE TABLE IF NOT EXISTS `niruri` (" _
                 & "`id` int(11) NOT NULL AUTO_INCREMENT," _
                 & "`nir` varchar(15) DEFAULT NULL," _
                 & "`data` date DEFAULT NULL," _
                 & "`valoare` double DEFAULT NULL," _
                 & "`tva` int(11) DEFAULT NULL," _
                 & "`nr_factura` varchar(45) DEFAULT NULL," _
                 & "`nume_firma` varchar(255) DEFAULT NULL," _
                 & "`cif_firma` varchar(45) DEFAULT NULL," _
                 & "`magazin` varchar(5) DEFAULT NULL," _
                 & "PRIMARY KEY (`id`)," _
                 & "UNIQUE KEY `nir` (`nir`)" _
                 & ")")
        Nonquery("CREATE TABLE IF NOT EXISTS `marfa` (" _
                 & "`id` int(11) NOT NULL AUTO_INCREMENT," _
                 & "`produs` varchar(45) NOT NULL DEFAULT '-'," _
                 & "`pret` double NOT NULL DEFAULT '0'," _
                 & "`bucati` int(11) NOT NULL DEFAULT '0'," _
                 & "`pret_intrare` double DEFAULT NULL," _
                 & "`nir` varchar(15) DEFAULT NULL," _
                 & "`data` date DEFAULT NULL," _
                 & "`magazin` varchar(5) DEFAULT NULL," _
                 & "PRIMARY KEY (`id`)" _
                 & ")")
        Nonquery("CREATE TABLE IF NOT EXISTS `niruri_obiecte` (" _
                 & "`id` int(11) NOT NULL AUTO_INCREMENT," _
                 & "`nir` varchar(15) DEFAULT NULL," _
                 & "`data` date DEFAULT NULL," _
                 & "`valoare` double DEFAULT NULL," _
                 & "`tva` int(11) DEFAULT NULL," _
                 & "`nr_factura` varchar(45) DEFAULT NULL," _
                 & "`nume_firma` varchar(255) DEFAULT NULL," _
                 & "`cif_firma` varchar(45) DEFAULT NULL," _
                 & "`magazin` varchar(5) DEFAULT NULL," _
                 & "PRIMARY KEY (`id`)," _
                 & "UNIQUE KEY `nir` (`nir`)" _
                 & ")")
        Nonquery("CREATE TABLE IF NOT EXISTS `obiecte_inventar` (" _
                 & "`id` int(11) NOT NULL AUTO_INCREMENT," _
                 & "`produs` varchar(45) NOT NULL DEFAULT '-'," _
                 & "`tip` varchar(10) NOT NULL DEFAULT '-'," _
                 & "`pret` double NOT NULL DEFAULT '0'," _
                 & "`bucati` double NOT NULL DEFAULT '0'," _
                 & "`pret_intrare` double DEFAULT NULL," _
                 & "`nir` varchar(15) DEFAULT NULL," _
                 & "`data` date DEFAULT NULL," _
                 & "`magazin` varchar(5) DEFAULT NULL," _
                 & "PRIMARY KEY (`id`)" _
                 & ")")

        Nonquery("CREATE TABLE IF NOT EXISTS `banca_tranzactii` (" _
                 & "`id` int(11) NOT NULL AUTO_INCREMENT," _
                 & "`data` date DEFAULT NULL," _
                 & "`tip_tranzactie` varchar(45) DEFAULT NULL," _
                 & "`suma` double NOT NULL DEFAULT '0'," _
                 & "`id_tranzactie` varchar(45) DEFAULT NULL," _
                 & "`descriere` varchar(255) DEFAULT NULL," _
                 & "`data_procesare` date DEFAULT NULL," _
                 & " PRIMARY KEY (`id`)," _
                 & "UNIQUE KEY `id_tranzactie` (`id_tranzactie`)" _
                 & ")")
        Nonquery("CREATE TABLE IF NOT EXISTS `banca_comisioane` (" _
                 & "`id` int(11) NOT NULL AUTO_INCREMENT," _
                 & "`data` date DEFAULT NULL," _
                 & "`tip_tranzactie` varchar(45) DEFAULT NULL," _
                 & "`suma` DOUBLE NOT NULL DEFAULT '0'," _
                 & "`id_tranzactie` varchar(45) DEFAULT NULL," _
                 & "`descriere` varchar(255) DEFAULT NULL," _
                 & "`data_procesare` date DEFAULT NULL," _
                 & " PRIMARY KEY (`id`)," _
                 & "UNIQUE KEY `id_tranzactie` (`id_tranzactie`)" _
                 & ")")

        'Nonquery("CREATE INDEX IF NOT EXISTS IDX_RASPUNS_BANCI ON raspuns_banci(cif_banca DESC,cnp DESC,nume_debitor DESC)")
        'Nonquery("CREATE INDEX IF NOT EXISTS IDX_RASPUNS_VENIT ON raspuns_venit(cnp DESC,cif_angajator DESC,nume_angajator DESC)")
        'Nonquery("CREATE INDEX IF NOT EXISTS IDX_TOTI ON toti(cnp DESC,dosar DESC,debitor DESC)")




        For m = 1 To 12
            ComboBox1.Items.Add(m)
        Next
        For y = 2013 To Today.Year
            ComboBox2.Items.Add(y)
        Next
        Dim data_disp As Date = Today
        If Today.Day > 5 Then
            data_disp = Today
            'ComboBox1.SelectedItem = data_dis.Month
        Else
            data_disp = DateAdd(DateInterval.Month, -1, Today)
            'ComboBox1.SelectedItem = DateAdd(month
        End If
        ComboBox1.SelectedItem = data_disp.Month
        ComboBox2.SelectedItem = data_disp.Year

        'ComboBox3.Items.AddRange({"1. Petru Maior", "2. Mihai Viteazu"})
        'ComboBox3.SelectedIndex = 0
        ComboBox3.DisplayMember = "Text"
        ComboBox3.ValueMember = "Value"
        Dim tb As New DataTable
        tb.Columns.Add("Text", GetType(String))
        tb.Columns.Add("Value", GetType(String))
        tb.Rows.Add("1. Petru Maior", "PM")
        tb.Rows.Add("2. Mihai Viteazu", "MV")
        ComboBox3.DataSource = tb

        Dim luna As String = ""
        Dim an As String = ""
        Dim magazin As String = ""
        Dim cash_true As String = ""
        If CheckBox1.CheckState = CheckState.Checked Then
            luna = ""
            an = ""
            magazin = ""
            cash_true = "WHERE cash=TRUE"
            CheckBox1.Text = "Tot"
            ComboBox1.Enabled = False
            ComboBox2.Enabled = False
            ComboBox3.Enabled = False
            incas_tot_luna_label.Text = ""
        ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
            CheckBox1.Text = "Tot"
            ComboBox1.Enabled = True
            ComboBox2.Enabled = True
            ComboBox3.Enabled = True
            luna = "WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "'"
            an = "WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "'"
            magazin = "AND magazin= '" & ComboBox3.SelectedValue & "'"
            cash_true = "AND cash=TRUE"
        End If
        For Each t As TabPage In TabControl1.TabPages
            TabControl1.SelectedTab = t
            TabControl1.SelectedIndex = 0
        Next

        'Dim mag As String = ComboBox3.SelectedValue
        Dim mag_id As String = "PM"
        'If mag = "PM" Then
        '    mag_id = "PM"
        'ElseIf mag = "MV" Then
        '    mag_id = "MV"
        'End If
        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
        End If

        CheckBox2.CheckState = CheckState.Unchecked

        Load_Situatie()
        Load_Incasari()
        Load_Cheltuieli()
        Load_Intrari()
        Load_Iesiri()
        Load_Solduri()
        Load_Firme()
        Load_Marfa()
        Load_Banca()
        Load_Facturi()

        dbconn.Close()
        AddHandler CheckBox2.CheckedChanged, AddressOf CheckBox2_CheckedChanged
    End Sub
   
    Public Sub Load_Setari()
        '-------------------- LOAD setari

        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")

        Try
            Dim set_sql As String = "SELECT * FROM setari WHERE setare='path_registru'"
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(set_sql, dbconn)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = True Then
                folder_registru = dbread("valoare")
            End If
            
        Catch ex As Exception
            MsgBox("Problem loading setari: path registru: " & ex.Message.ToString)

        End Try

        dbread.Close()

        Try
            Dim set_sql As String = "SELECT * FROM setari WHERE setare='path_raport_gestiune'"

            dbcomm = New MySqlCommand(set_sql, dbconn)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = True Then
                folder_raport = dbread("valoare")
            End If

        Catch ex As Exception
            MsgBox("Problem loading setari: path raport: " & ex.Message.ToString)
        End Try
        dbread.Close()

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

        Try
            Dim set_sql As String = "SELECT * FROM setari WHERE setare='path_nir'"

            dbcomm = New MySqlCommand(set_sql, dbconn)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = True Then
                folder_nir = dbread("valoare")
            End If

        Catch ex As Exception
            MsgBox("Problem loading setari: path nir: " & ex.Message.ToString)
        End Try
        dbread.Close()

        Try
            Dim set_sql As String = "SELECT * FROM setari WHERE setare='path_facturi'"

            dbcomm = New MySqlCommand(set_sql, dbconn)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = True Then
                folder_facturi = dbread("valoare")
            End If

        Catch ex As Exception
            MsgBox("Problem loading setari: path facturi: " & ex.Message.ToString)
        End Try
        dbread.Close()
        '--------------------------------
    End Sub
    Public Sub Load_Documente()

        'Load_Registru_Listview()
        'Load_Raport_Listview()
        'Load_DP_Listview()
        'Load_DI_Listview()
        'Load_Nir_Listview()
        'Load_Facturi_Listview()


    End Sub
    Public Sub Load_Incasari()
        Dim DGV As DataGridView = Incasari_DGV
        DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DGV.AllowUserToAddRows = False
        DGV.AllowUserToResizeRows = False
        DGV.RowHeadersVisible = False
        DGV.CellBorderStyle = DataGridViewCellBorderStyle.None


        Dim anul As Integer = CInt(ComboBox2.SelectedItem)
        Dim luna_actuala As Integer = CInt(ComboBox1.SelectedItem)
        Dim an_actual As Integer = CInt(ComboBox2.SelectedItem)
        Dim luna_prec As Integer = luna_actuala - 1
        If luna_actuala = 1 Then
            luna_prec = 12
            anul = anul - 1
        End If

        Dim luna As String = ""
        Dim an As String = ""
        Dim magazin As String = ""
        Dim cash_true As String = ""
        If CheckBox1.CheckState = CheckState.Checked Then
            luna = ""
            an = ""
            magazin = ""
        ElseIf CheckBox1.CheckState = CheckState.Unchecked Then

            luna = "WHERE MONTH(incasari.data)='" & ComboBox1.SelectedItem & "' and year(incasari.data)= '" & ComboBox2.SelectedItem & "'"
            an = "WHERE YEAR(incasari.data)= '" & ComboBox2.SelectedItem & "'"
            magazin = "AND incasari.magazin= '" & ComboBox3.SelectedValue & "'"
        End If
        ' -------------------- LOAD incasari INTO GRID
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            'Dim tot_sql As String = "SELECT data,nr_rzf,tip_incasare,explicatii,suma FROM incasari " & luna & " ORDER BY data ASC"
            Dim tot_sql As String = "SELECT id,incasari.data,incasari.nr_rzf,incasari.tip_incasare,incasari.explicatii,incasari.suma_cash as Cash,suma_card as POS,magazin FROM incasari " & luna & " " & magazin & " ORDER BY incasari.data DESC,incasari.id DESC"

            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                sda.SelectCommand = dbcomm
                sda.Fill(dbdataset)
                bsource.DataSource = dbdataset
                DGV.DataSource = bsource
                sda.Update(dbdataset)
            Catch ex As Exception
                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
        End Using

        For i = 1 To DGV.RowCount - 1

            If DGV.Rows(i).Cells("data").Value = DGV.Rows(i - 1).Cells("data").Value Then
                DGV.Rows(i).DefaultCellStyle.BackColor = DGV.Rows(i - 1).DefaultCellStyle.BackColor
            ElseIf DGV.Rows(i).Cells("data").Value <> DGV.Rows(i - 1).Cells("data").Value And DGV.Rows(i - 1).DefaultCellStyle.BackColor <> Color.LightGray Then
                DGV.Rows(i).DefaultCellStyle.BackColor = Color.LightGray
            End If
        Next

        '-------------- Statusuri
        If CheckBox1.CheckState = CheckState.Unchecked Then
            Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
                Dim sda As New MySqlDataAdapter
                Dim dbdataset As New DataTable
                Dim bsource As New BindingSource
                Dim tipincas As String = "AND tip_incasare='RZF'"
                If CheckBox1.CheckState = CheckState.Checked Then
                    tipincas = "WHERE tip_incasare='RZF'"
                End If
                Dim tot_sql As String = "SELECT data,nr_rzf,tip_incasare FROM incasari " & luna & " " & tipincas & " " & magazin & " ORDER BY data ASC"

                dbcomm = New MySqlCommand(tot_sql, dbconn)

                Try
                    sda.SelectCommand = dbcomm
                    sda.Fill(dbdataset)
                    bsource.DataSource = dbdataset
                    sda.Update(dbdataset)
                Catch ex As Exception
                    MsgBox("Problem loading toti: " & ex.Message.ToString)
                End Try
                For i = 1 To dbdataset.Rows.Count - 1
                    Dim nr_rzf As Integer = CInt(dbdataset.Rows(i).Item("nr_rzf").ToString.Substring(2))
                    Dim nr_rzf_prec As Integer = CInt(dbdataset.Rows(i - 1).Item("nr_rzf").ToString.Substring(2))
                    If nr_rzf <> nr_rzf_prec + 1 Then
                        z_status_label.Text = "* Status Z-uri: Lipsa Z: " & nr_rzf - 1
                        z_status_label.ForeColor = Color.Red
                        Exit For
                    Else : z_status_label.Text = "* Status Z-uri: OK"
                        z_status_label.ForeColor = Color.Green
                    End If
                Next
            End Using

        End If


        ' -------------------- LOAD incasari INTO label
        '----------------------luna

        Dim incas_cash_luna As Decimal = 0
        Dim incas_card_luna As Decimal = 0

        Dim incas_total_cash As Decimal = 0 'an
        Dim incas_total_card As Decimal = 0 'an
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbtable As New DataTable
            Dim bsource As New BindingSource

            'Dim tot_sql As String = "SELECT sum(suma_cash), sum(suma_card) FROM incasari " & luna & " " & magazin & ""
            Dim tot_sql As String = "SELECT * FROM " _
                                    & "(SELECT sum(suma_cash) as lu_cash, sum(suma_card) as lu_card FROM incasari " & luna & " " & magazin & ") luna, " _
                                    & "(SELECT sum(suma_cash) as an_cash, sum(suma_card) as an_card FROM incasari " & an & " " & magazin & ") an"

            'dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                dbconn.Open()
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                sda.SelectCommand = dbcomm
                sda.Fill(dbtable)
                bsource.DataSource = dbtable
                sda.Update(dbtable)
                If CheckBox1.CheckState = CheckState.Checked Then
                    incas_tot_luna_label.Text = ""
                    incas_card_luna_lLbl.Text = ""
                    If IsDBNull(dbtable.Rows(0).Item("an_cash")) = False Then
                        Incas_tot_an_label.Text = "Total ani: " & FormatNumber(CDec(dbtable.Rows(0).Item("an_cash").ToString), 2, TriState.True, TriState.False, TriState.True) & " Lei"
                    End If
                ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
                    If IsDBNull(dbtable.Rows(0).Item("lu_cash")) = False Then
                        incas_cash_luna = CDec(dbtable.Rows(0).Item("lu_cash").ToString)
                        incas_tot_luna_label.Text = "Total Cash luna: " & FormatNumber(incas_cash_luna, 2, TriState.True, TriState.False, TriState.True) & " Lei"
                    Else
                        incas_tot_luna_label.Text = "Total Cash luna: 0,00 lei"
                    End If
                    If IsDBNull(dbtable.Rows(0).Item("lu_card")) = False Then
                        incas_card_luna = CDec(dbtable.Rows(0).Item("lu_card").ToString)
                        incas_card_luna_lLbl.Text = "Total card luna: " & FormatNumber(incas_card_luna, 2, TriState.True, TriState.False, TriState.True) & " Lei"
                    Else
                        incas_card_luna_lLbl.Text = "Total card luna: 0,00 lei"
                    End If
                    total_i_luna_Lbl.Text = "Total Incasari Luna: " & FormatNumber(incas_cash_luna + incas_card_luna, 2, TriState.True, TriState.False, TriState.True) & " Lei"

                    If IsDBNull(dbtable.Rows(0).Item("an_cash")) = False Then
                        incas_total_cash = CDec(dbtable.Rows(0).Item("an_cash").ToString)
                        Incas_tot_an_label.Text = "Total an: " & FormatNumber(CDec(incas_total_cash), 2, TriState.True, TriState.False, TriState.True) & " Lei"
                    Else
                        Incas_tot_an_label.Text = "Total an: 0,00 lei"
                    End If
                    If IsDBNull(dbtable.Rows(0).Item("an_card")) = False Then
                        incas_total_card = CDec(dbtable.Rows(0).Item("an_card").ToString)
                        incas_card_an_Lbl.Text = "Total an card: " & FormatNumber(CDec(incas_total_card), 2, TriState.True, TriState.False, TriState.True) & " Lei"
                    Else
                        incas_card_an_Lbl.Text = "Total an card: 0,00 lei"
                    End If
                    total_i_an_Lbl.Text = "Total Incasari An: " & FormatNumber(incas_total_cash + incas_total_card, 2, TriState.True, TriState.False, TriState.True) & " Lei"
                End If


            Catch ex As Exception
                MsgBox("Problem loading label incasari: " & ex.Message.ToString)
            End Try
        End Using


        ' -------------------- LOAD solduri INTO Textbox
        '----------------------luna
        Dim sold_ini As Decimal = 0
        If CheckBox1.CheckState = CheckState.Checked Then
            luna = ""
            an = ""
            magazin = ""
            cash_true = ""
        ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
            luna = "WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' and year(data)= '" & ComboBox2.SelectedItem & "'"
            an = "WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "'"
            magazin = "AND magazin='" & ComboBox3.SelectedValue & "'"
            cash_true = "AND cash=TRUE"
        End If
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource



            Dim tot_sql As String = "SELECT * FROM solduri_casa WHERE MONTH(data)=" & luna_prec & " AND YEAR(data)=" & anul & " " & magazin & ""
            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                dbread = dbcomm.ExecuteReader()

                If CheckBox1.CheckState = CheckState.Checked Then
                    sold_ini_Txt.Text = "-"
                ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
                    dbread.Read()
                    If dbread.HasRows = False Then
                        sold_ini_Txt.Text = "Nu avem"
                    Else
                        sold_ini = CDec(dbread("casa_sold_final"))
                        sold_ini_Txt.Text = FormatNumber(sold_ini, 2, TriState.True, TriState.False, TriState.True) & " Lei"
                    End If
                End If
            Catch ex As Exception
                MsgBox("Problem loading sold initial: " & ex.Message.ToString)
            End Try
        End Using

        Dim chelt_tot_luna As Decimal = 0
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String = "SELECT sum(suma) FROM cheltuieli " & luna & " " & magazin & " " & cash_true & ""

            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                dbconn.Open()
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If CheckBox1.CheckState = CheckState.Checked Then
                    sold_fin_Txt.Text = "-"
                ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
                    If IsDBNull(dbread("sum(suma)")) Then
                        chelt_tot_luna = 0
                    Else : chelt_tot_luna = CDec(dbread("sum(suma)"))
                    End If
                End If
            Catch ex As Exception
                MsgBox("Problem loading sold final (cheltuieli): " & ex.Message.ToString)
            End Try

        End Using
        sold_fin_Txt.Text = FormatNumber(CDec(sold_ini + incas_cash_luna - chelt_tot_luna), 2, TriState.True, TriState.False, TriState.True) & " Lei"

        '-------------------------

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")


            Dim sold_prec As Decimal = sold_ini

            For d = 1 To Date.DaysInMonth(an_actual, luna_actuala)

                Dim data As String = Format(Date.Parse(d & "." & luna_actuala & "." & an_actual), "yyyy-MM-dd")

                Dim datatext As Date = data.ToString
                Dim suma_inc As Decimal = 0
                Dim suma_che As Decimal = 0

                Try
                    Dim sql_inc As String = "SELECT sum(suma_cash) FROM incasari WHERE data='" & data & "' " & magazin & ""
                    If dbconn.State = ConnectionState.Closed Then
                        dbconn.Open()
                    End If
                    dbcomm = New MySqlCommand(sql_inc, dbconn)
                    dbread = dbcomm.ExecuteReader()
                    dbread.Read()
                    If IsDBNull(dbread("sum(suma_cash)")) = False Then
                        suma_inc = CDec(dbread("sum(suma_cash)"))
                    End If
                    dbread.Close()
                Catch ex As Exception
                    MsgBox("bla 1" & ex.Message)
                End Try
                Try
                    dbcomm = New MySqlCommand("SELECT sum(suma) FROM cheltuieli WHERE data='" & data & "' " & magazin & " AND cash=TRUE", dbconn)
                    dbread = dbcomm.ExecuteReader()
                    dbread.Read()
                    If IsDBNull(dbread("sum(suma)")) = False Then
                        suma_che = CDec(dbread("sum(suma)"))
                    End If
                    dbread.Close()
                Catch ex As Exception
                    MsgBox("bla 2" & ex.Message)
                End Try
                sold_prec = sold_prec + suma_inc - suma_che
                If sold_prec < 0 Then
                    minus_label_status.Text = "* Status Casa: " & FormatNumber(CDec(sold_prec), 2, TriState.True, TriState.False, TriState.True) & " Lei" & " Data: " & datatext
                    minus_label_status.ForeColor = Color.Red
                    Exit For
                Else
                    minus_label_status.Text = "* Status Casa: OK (+)"
                    minus_label_status.ForeColor = Color.Green
                End If
            Next

        End Using

        If CheckBox1.CheckState = CheckState.Checked Then
            z_status_label.Text = ""
            minus_label_status.Text = ""
        End If
        DGV.Columns("id").Visible = False
        DGV.Columns("magazin").Visible = False

        DGV.Columns("data").FillWeight = DGV.Width * 20 / 100
        DGV.Columns("data").HeaderText = "Data"
        DGV.Columns("data").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("nr_rzf").FillWeight = DGV.Width * 15 / 100
        DGV.Columns("nr_rzf").HeaderText = "Nr. RZF"
        DGV.Columns("nr_rzf").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("tip_incasare").FillWeight = DGV.Width * 10 / 100
        DGV.Columns("tip_incasare").HeaderText = "Tip Incas"
        DGV.Columns("tip_incasare").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("explicatii").FillWeight = DGV.Width * 25 / 100
        DGV.Columns("explicatii").HeaderText = "Explicatii"
        DGV.Columns("explicatii").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("Cash").FillWeight = DGV.Width * 15 / 100
        DGV.Columns("Cash").HeaderText = "Cash"
        DGV.Columns("Cash").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        'For Each control In DGV.Controls
        '    If control.GetType() Is GetType(VScrollBar) Then
        '        Dim vbar As VScrollBar = DirectCast(control, VScrollBar)
        '        If vbar.Visible = True Then
        '            DGV.Columns("POS").FillWeight = DGV.Width * 10.5 / 100
        '        Else
        DGV.Columns("POS").FillWeight = DGV.Width * 15 / 100
        '        End If
        '    End If
        'Next
        DGV.Columns("POS").HeaderText = "POS"
        DGV.Columns("POS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight



        DGV.ClearSelection()
        DGV.CurrentCell = Nothing
    End Sub
    Public Sub Load_Cheltuieli()
        Dim DGV As DataGridView = Cheltuieli_DGV
        DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DGV.AllowUserToAddRows = False
        DGV.AllowUserToResizeRows = False
        DGV.RowHeadersVisible = False



        ' -------------------- LOAD cheltuieli INTO GRID
        Dim luna As String = ""
        Dim an As String = ""
        Dim magazin As String = ""
        Dim cash_true As String = ""
        If CheckBox1.CheckState = CheckState.Checked Then
            luna = ""
            an = ""
            magazin = ""
            cash_true = ""
        ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
            luna = "WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "'"
            an = "WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "'"
            magazin = "AND magazin= '" & ComboBox3.SelectedValue & "'"
            cash_true = "AND cash=TRUE"
        End If
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource
            Dim tot_sql As String = "SELECT id,data,nr_chitanta,tip_cheltuiala,explicatii,suma,magazin,cash FROM cheltuieli " & luna & " " & magazin & " " & cash_true & " ORDER BY data DESC"

            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                sda.SelectCommand = dbcomm
                sda.Fill(dbdataset)
                bsource.DataSource = dbdataset
                DGV.DataSource = bsource
                sda.Update(dbdataset)
            Catch ex As Exception
                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
        End Using
        DGV.CellBorderStyle = DataGridViewCellBorderStyle.None
        For i = 1 To DGV.RowCount - 1
            If DGV.Rows(i).Cells("data").Value = DGV.Rows(i - 1).Cells("data").Value Then
                DGV.Rows(i).DefaultCellStyle.BackColor = DGV.Rows(i - 1).DefaultCellStyle.BackColor
            ElseIf DGV.Rows(i).Cells("data").Value <> DGV.Rows(i - 1).Cells("data").Value And DGV.Rows(i - 1).DefaultCellStyle.BackColor <> Color.LightGray Then
                DGV.Rows(i).DefaultCellStyle.BackColor = Color.LightGray
            End If
        Next
        If CheckBox1.CheckState = CheckState.Unchecked Then

            ' ------- VERIFICARE SALARII -------------

            Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
                Dim sda As New MySqlDataAdapter
                Dim dbdataset As New DataTable
                Dim bsource As New BindingSource
                Dim tot_sql As String = "SELECT COUNT(suma),SUM(suma) FROM cheltuieli " & luna & " AND explicatii='SALARII'"
                Dim nr_sal As Integer = 1
                Dim suma As Decimal = 0
                dbcomm = New MySqlCommand(tot_sql, dbconn)

                Try
                    dbconn.Open()
                    dbcomm = New MySqlCommand(tot_sql, dbconn)
                    dbread = dbcomm.ExecuteReader()
                    dbread.Read()
                    nr_sal = CInt(dbread("COUNT(suma)"))
                    If IsDBNull(dbread("SUM(suma)")) Then
                        suma = 0
                    Else : suma = CDec(dbread("SUM(suma)"))
                    End If
                Catch ex As Exception
                    MsgBox("Problem loading toti: " & ex.Message.ToString)
                End Try

                If nr_sal < 1 Then
                    salarii_status_label.Text = "*Status Salarii: Nu ai pus Salariile luna asta"
                    salarii_status_label.ForeColor = Color.Red
                ElseIf nr_sal > 1 Then
                    salarii_status_label.Text = "*Status Salarii: S-au pus mai multe Salarii luna asta"
                    salarii_status_label.ForeColor = Color.Red
                ElseIf nr_sal = 1 Then
                    If suma > 1500 AndAlso suma < 3000 Then
                        salarii_status_label.Text = "*Status Salarii: OK"
                        salarii_status_label.ForeColor = Color.Green
                    Else
                        salarii_status_label.Text = "*Status Salarii: Atentie la suma. Prea putin sau prea mult"
                        salarii_status_label.ForeColor = Color.Goldenrod
                    End If
                End If

            End Using

            '-----------------------------------^
            If CheckBox1.CheckState = CheckState.Unchecked Then
                Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
                    Dim sda As New MySqlDataAdapter
                    Dim dbdataset As New DataTable
                    Dim bsource As New BindingSource

                    CheckBox1.Text = "Tot"
                    ComboBox1.Enabled = True
                    ComboBox2.Enabled = True
                    Dim luna2 As String = "WHERE MONTH(cheltuieli.data)='" & ComboBox1.SelectedItem & "' and year(cheltuieli.data)= '" & ComboBox2.SelectedItem & "' AND cheltuieli.magazin= '" & ComboBox3.SelectedValue & "'"
                    Dim tot_sql As String = "SELECT cheltuieli.data,incasari.data FROM cheltuieli left join incasari on cheltuieli.data=incasari.data " & luna2 & " AND incasari.data IS NULL " & cash_true & ""

                    If dbconn.State = ConnectionState.Closed Then
                        dbconn.Open()
                    End If
                    dbcomm = New MySqlCommand(tot_sql, dbconn)

                    dbread = dbcomm.ExecuteReader()
                    dbread.Read()
                    If dbread.HasRows Then
                        Label1.Text = "*Status Data: In data de: " & dbread("data") & " nu s-au facut incasari!!!"
                        Label1.ForeColor = Color.YellowGreen
                    Else : Label1.Text = "*Status Data: OK"
                        Label1.ForeColor = Color.Green
                    End If

                End Using
            End If
        ElseIf CheckBox1.CheckState = CheckState.Checked Then
            salarii_status_label.Text = ""
            Label1.Text = ""
        End If


        ' -------------------- LOAD cheltuieli INTO label

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String = "SELECT sum(suma) FROM cheltuieli " & luna & " " & magazin & " " & cash_true & ""


            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                dbconn.Open()
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If CheckBox1.CheckState = CheckState.Checked Then
                    chelt_tot_luna_label.Text = ""
                ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
                    If dbread("sum(suma)").ToString <> "" Then
                        chelt_tot_luna_label.Text = "Total luna: " & FormatNumber(CDec(dbread("sum(suma)").ToString), 2, TriState.True, TriState.False, TriState.True) & " Lei"
                    Else
                        chelt_tot_luna_label.Text = "Total luna: 0,00 lei"
                    End If
                End If
            Catch ex As Exception
                MsgBox("Problem loading label cheltuieli (combobox): " & ex.Message.ToString)
            End Try
        End Using
        '-----------------------------------cheltuieli  AN

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource
            If CheckBox1.CheckState = CheckState.Checked Then
                an = ""
            End If
            Dim tot_sql As String = "SELECT sum(suma) FROM cheltuieli " & an & " " & magazin & " " & cash_true & ""


            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                dbconn.Open()
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If CheckBox1.CheckState = CheckState.Checked Then
                    chelt_tot_luna_label.Text = ""
                    chelt_tot_an_label.Text = "Total ani: " & FormatNumber(CDec(dbread("sum(suma)").ToString), 2, TriState.True, TriState.False, TriState.True) & " Lei"
                ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
                    If dbread("sum(suma)").ToString <> "" Then
                        chelt_tot_an_label.Text = "Total an: " & FormatNumber(CDec(dbread("sum(suma)").ToString), 2, TriState.True, TriState.False, TriState.True) & " Lei"
                    Else
                        chelt_tot_an_label.Text = "Total an: 0,00 lei"
                    End If
                End If

            Catch ex As Exception
                MsgBox("Problem loading label cheltuieli (combobox): " & ex.Message.ToString)
            End Try
        End Using
        DGV.Columns("id").Visible = False
        DGV.Columns("magazin").Visible = False
        DGV.Columns("cash").Visible = False

        DGV.Columns("data").FillWeight = DGV.Width * 20 / 100
        DGV.Columns("data").HeaderText = "Data"
        DGV.Columns("data").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("nr_chitanta").FillWeight = DGV.Width * 15 / 100
        DGV.Columns("nr_chitanta").HeaderText = "Nr. Act"
        DGV.Columns("nr_chitanta").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("tip_cheltuiala").FillWeight = DGV.Width * 10 / 100
        DGV.Columns("tip_cheltuiala").HeaderText = "Tip Chelt."
        DGV.Columns("tip_cheltuiala").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("explicatii").FillWeight = DGV.Width * 38 / 100
        DGV.Columns("explicatii").HeaderText = "Explicatii"
        DGV.Columns("explicatii").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'For Each control In DGV.Controls
        '    If control.GetType() Is GetType(VScrollBar) Then
        '        Dim vbar As VScrollBar = DirectCast(control, VScrollBar)
        '        If vbar.Visible = True Then
        '            DGV.Columns("suma").FillWeight = DGV.Width * 15 / 100
        '        Else
        DGV.Columns("suma").FillWeight = DGV.Width * 17 / 100
        '        End If
        '    End If
        'Next
        DGV.Columns("suma").HeaderText = "Suma"
        DGV.Columns("suma").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DGV.Columns("suma").DefaultCellStyle.Format = "#,#0.00"



        DGV.ClearSelection()
        DGV.CurrentCell = Nothing
    End Sub
    Public Sub Load_Intrari()
        Dim DGV As DataGridView = Intrari_DGV
        DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DGV.AllowUserToAddRows = False
        DGV.AllowUserToResizeRows = False
        DGV.RowHeadersVisible = False

        ' -------------------- LOAD intrari INTO GRID
        Dim luna As String = ""
        Dim an As String = ""
        Dim magazin As String = ""
        If CheckBox1.CheckState = CheckState.Checked Then
            luna = ""
            an = ""
            magazin = ""
        ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
            luna = "WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "'"
            an = "WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "'"
            magazin = "AND magazin= '" & ComboBox3.SelectedValue & "'"
        End If
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String = "SELECT id,data,nr_nir,tip_document,explicatii,suma,magazin FROM intrari " & luna & " " & magazin & " ORDER BY data DESC,nr_nir DESC"


            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                sda.SelectCommand = dbcomm
                sda.Fill(dbdataset)
                bsource.DataSource = dbdataset
                DGV.DataSource = bsource
                sda.Update(dbdataset)
            Catch ex As Exception
                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
        End Using
        DGV.CellBorderStyle = DataGridViewCellBorderStyle.None
        For i = 1 To DGV.RowCount - 1
            If DGV.Rows(i).Cells("data").Value = DGV.Rows(i - 1).Cells("data").Value Then
                DGV.Rows(i).DefaultCellStyle.BackColor = DGV.Rows(i - 1).DefaultCellStyle.BackColor
            ElseIf DGV.Rows(i).Cells("data").Value <> DGV.Rows(i - 1).Cells("data").Value And DGV.Rows(i - 1).DefaultCellStyle.BackColor <> Color.LightGray Then
                DGV.Rows(i).DefaultCellStyle.BackColor = Color.LightGray
            End If
        Next

        DGV.Columns("id").Visible = False
        DGV.Columns("magazin").Visible = False

        DGV.Columns("data").FillWeight = DGV.Width * 20 / 100
        DGV.Columns("data").HeaderText = "Data"
        DGV.Columns("data").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("nr_nir").FillWeight = DGV.Width * 15 / 100
        DGV.Columns("nr_nir").HeaderText = "Nr. NIR"
        DGV.Columns("nr_nir").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("tip_document").FillWeight = DGV.Width * 10 / 100
        DGV.Columns("tip_document").HeaderText = "Tip Doc"
        DGV.Columns("tip_document").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("explicatii").FillWeight = DGV.Width * 38 / 100
        DGV.Columns("explicatii").HeaderText = "Explicatii"
        DGV.Columns("explicatii").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'For Each control In DGV.Controls
        '    If control.GetType() Is GetType(VScrollBar) Then
        '        Dim vbar As VScrollBar = DirectCast(control, VScrollBar)
        '        If vbar.Visible = True Then
        '            DGV.Columns("suma").FillWeight = DGV.Width * 15 / 100
        '        Else
        DGV.Columns("suma").FillWeight = DGV.Width * 17 / 100
        '        End If
        '    End If
        'Next
        DGV.Columns("suma").HeaderText = "Suma"
        DGV.Columns("suma").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DGV.Columns("suma").DefaultCellStyle.Format = "#,#0.00"


        ' -------------------- LOAD intrari INTO label
        Dim intrari_total_luna As Decimal = 0
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String = "SELECT sum(suma) FROM intrari " & luna & " " & magazin & ""


            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                dbconn.Open()
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If CheckBox1.CheckState = CheckState.Checked Then
                    int_luna_total.Text = ""
                ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
                    If dbread("sum(suma)").ToString <> "" Then
                        int_luna_total.Text = "Total luna: " & FormatNumber(CDec(dbread("sum(suma)").ToString), 2, TriState.True, TriState.False, TriState.True) & " Lei"
                        intrari_total_luna = CDec(dbread("sum(suma)").ToString)
                    Else
                        int_luna_total.Text = "Total luna: 0,00 lei"
                    End If
                End If
            Catch ex As Exception
                MsgBox("Problem loading label intrari: " & ex.Message.ToString)
            End Try
        End Using
        '-----------------------------------intrari  AN

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource
            If CheckBox1.CheckState = CheckState.Checked Then
                an = ""
            End If
            Dim tot_sql As String = "SELECT sum(suma) FROM intrari " & an & " " & magazin & ""


            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                dbconn.Open()
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If CheckBox1.CheckState = CheckState.Checked Then
                    int_luna_total.Text = ""
                    int_an_total.Text = "Total ani: " & FormatNumber(CDec(dbread("sum(suma)").ToString), 2, TriState.True, TriState.False, TriState.True) & " Lei"
                ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
                    If dbread("sum(suma)").ToString <> "" Then
                        int_an_total.Text = "Total an: " & FormatNumber(CDec(dbread("sum(suma)").ToString), 2, TriState.True, TriState.False, TriState.True) & " Lei"
                    Else
                        int_an_total.Text = "Total an: 0,00 lei"
                    End If
                End If

            Catch ex As Exception
                MsgBox("Problem loading label intrari: " & ex.Message.ToString)
            End Try
        End Using

        ' -------------------- LOAD solduri INTO Textbox
        '----------------------luna
        Dim sold_ini As Decimal = 0
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim anul As Integer = CInt(ComboBox2.SelectedItem)
            Dim luna_actuala As Integer = CInt(ComboBox1.SelectedItem)
            Dim luna_prec As Integer = luna_actuala - 1
            If luna_actuala = 1 Then
                luna_prec = 12
                anul = anul - 1
            End If


            Dim tot_sql As String = "SELECT * FROM solduri_gestiune WHERE MONTH(data)=" & luna_prec & " AND YEAR(data)=" & anul & " " & magazin & ""
            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                dbread = dbcomm.ExecuteReader()

                If CheckBox1.CheckState = CheckState.Checked Then
                    sold_ges_ini_txt.Text = "-"
                ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
                    dbread.Read()
                    If dbread.HasRows = False Then
                        sold_ges_ini_txt.Text = "Nu avem"
                    Else
                        sold_ini = CDec(dbread("gestiune_sold_final"))
                        sold_ges_ini_txt.Text = FormatNumber(sold_ini, 2, TriState.True, TriState.False, TriState.True) & " Lei"
                    End If
                End If
            Catch ex As Exception
                MsgBox("Problem loading sold initial: " & ex.Message.ToString)
            End Try
        End Using

        Dim ies_tot_luna As Decimal = 0
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String = "SELECT sum(suma) FROM iesiri " & luna & " " & magazin & ""

            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                dbconn.Open()
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If CheckBox1.CheckState = CheckState.Checked Then
                    sold_ges_fin_txt.Text = "-"
                ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
                    If IsDBNull(dbread("sum(suma)")) Then
                        ies_tot_luna = 0
                    Else : ies_tot_luna = CDec(dbread("sum(suma)"))
                    End If
                End If
            Catch ex As Exception
                MsgBox("Problem loading sold final (iesiri): " & ex.Message.ToString)
            End Try
        End Using
        sold_ges_fin_txt.Text = FormatNumber(CDec(sold_ini + intrari_total_luna - ies_tot_luna), 2, TriState.True, TriState.False, TriState.True) & " Lei"
        DGV.ClearSelection()
        DGV.CurrentCell = Nothing
    End Sub
    Public Sub Load_Iesiri()
        Dim DGV As DataGridView = Iesiri_DGV
        DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DGV.AllowUserToAddRows = False
        DGV.AllowUserToResizeRows = False
        DGV.RowHeadersVisible = False
        ' -------------------- LOAD iesiri INTO GRID
        Dim luna As String = ""
        Dim an As String = ""
        Dim magazin As String = ""
        If CheckBox1.CheckState = CheckState.Checked Then
            luna = ""
            an = ""
            magazin = ""
        ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
            luna = "WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "'"
            an = "WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "'"
            magazin = "AND magazin= '" & ComboBox3.SelectedValue & "'"
        End If
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String = "SELECT id,data,nr_rzf,tip_incasare,explicatii,suma,magazin FROM iesiri " & luna & " " & magazin & " ORDER BY data DESC,nr_rzf DESC"


            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                sda.SelectCommand = dbcomm
                sda.Fill(dbdataset)
                bsource.DataSource = dbdataset
                Iesiri_DGV.DataSource = bsource
                sda.Update(dbdataset)
            Catch ex As Exception
                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
        End Using
        Iesiri_DGV.CellBorderStyle = DataGridViewCellBorderStyle.None
        For i = 1 To Iesiri_DGV.RowCount - 2
            If Iesiri_DGV.Rows(i).Cells("data").Value = Iesiri_DGV.Rows(i - 1).Cells("data").Value Then
                Iesiri_DGV.Rows(i).DefaultCellStyle.BackColor = Iesiri_DGV.Rows(i - 1).DefaultCellStyle.BackColor
            ElseIf Iesiri_DGV.Rows(i).Cells("data").Value <> Iesiri_DGV.Rows(i - 1).Cells("data").Value And Iesiri_DGV.Rows(i - 1).DefaultCellStyle.BackColor <> Color.LightGray Then
                Iesiri_DGV.Rows(i).DefaultCellStyle.BackColor = Color.LightGray
            End If
        Next
        DGV.Columns("data").FillWeight = DGV.Width * 20 / 100
        DGV.Columns("data").HeaderText = "Data"
        DGV.Columns("data").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("nr_rzf").FillWeight = DGV.Width * 15 / 100
        DGV.Columns("nr_rzf").HeaderText = "Nr. RZF"
        DGV.Columns("nr_rzf").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("tip_incasare").FillWeight = DGV.Width * 10 / 100
        DGV.Columns("tip_incasare").HeaderText = "Tip Inc"
        DGV.Columns("tip_incasare").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("explicatii").FillWeight = DGV.Width * 38 / 100
        DGV.Columns("explicatii").HeaderText = "Explicatii"
        DGV.Columns("explicatii").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'For Each control In DGV.Controls
        '    If control.GetType() Is GetType(VScrollBar) Then
        '        Dim vbar As VScrollBar = DirectCast(control, VScrollBar)
        '        If vbar.Visible = True Then
        '            DGV.Columns("suma").FillWeight = DGV.Width * 15 / 100
        '        Else
        DGV.Columns("suma").FillWeight = DGV.Width * 17 / 100
        '        End If
        '    End If
        'Next
        DGV.Columns("suma").HeaderText = "Suma"
        DGV.Columns("suma").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DGV.Columns("suma").DefaultCellStyle.Format = "#,#0.00"


        ' -------------------- LOAD iesiri INTO label

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String = "SELECT sum(suma) FROM iesiri " & luna & " " & magazin & ""


            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                dbconn.Open()
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If CheckBox1.CheckState = CheckState.Checked Then
                    ies_luna_total.Text = ""
                ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
                    If dbread("sum(suma)").ToString <> "" Then
                        ies_luna_total.Text = "Total luna: " & FormatNumber(CDec(dbread("sum(suma)").ToString), 2, TriState.True, TriState.False, TriState.True) & " Lei"
                    Else
                        ies_luna_total.Text = "Total luna: 0,00 lei"
                    End If
                End If
            Catch ex As Exception
                MsgBox("Problem loading label intrari: " & ex.Message.ToString)
            End Try
        End Using
        '-----------------------------------intrari  AN

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource
            If CheckBox1.CheckState = CheckState.Checked Then
                an = ""
            End If
            Dim tot_sql As String = "SELECT sum(suma) FROM iesiri " & an & " " & magazin & ""


            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                dbconn.Open()
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If CheckBox1.CheckState = CheckState.Checked Then
                    ies_luna_total.Text = ""
                    ies_an_total.Text = "Total ani: " & FormatNumber(CDec(dbread("sum(suma)").ToString), 2, TriState.True, TriState.False, TriState.True) & " Lei"
                ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
                    If dbread("sum(suma)").ToString <> "" Then
                        ies_an_total.Text = "Total an: " & FormatNumber(CDec(dbread("sum(suma)").ToString), 2, TriState.True, TriState.False, TriState.True) & " Lei"
                    Else
                        ies_an_total.Text = "Total an: 0,00 lei"
                    End If
                End If

            Catch ex As Exception
                MsgBox("Problem loading label intrari: " & ex.Message.ToString)
            End Try
        End Using
        DGV.ClearSelection()
        DGV.CurrentCell = Nothing
    End Sub
    Public Sub Load_Solduri()
        Sold_Casa_DGV.AllowUserToAddRows = False
        Sold_Gestiune_DGV.AllowUserToAddRows = False
        Sold_Casa_DGV.AllowUserToResizeRows = False
        Sold_Casa_DGV.DataSource = Nothing
        Sold_Casa_DGV.RowHeadersVisible = False
        Sold_Casa_DGV.Rows.Clear()
        Sold_Casa_DGV.Columns.Clear()
        Sold_Casa_DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Sold_Gestiune_DGV.DataSource = Nothing
        Sold_Gestiune_DGV.RowHeadersVisible = False
        Sold_Gestiune_DGV.AllowUserToResizeRows = False
        Sold_Gestiune_DGV.Rows.Clear()
        Sold_Gestiune_DGV.Columns.Clear()
        Sold_Gestiune_DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' -------------------- LOAD solduri_casa INTO GRID
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            'Dim tot_sql As String = "SELECT * FROM solduri_" & ComboBox3.SelectedValue & " ORDER BY data ASC"
            Dim casa_sql As String = "SELECT solduri_casa.data," _
                                    & "casa_sold_initial," _
                                    & "incasari,cheltuieli," _
                                    & "casa_sold_final," _
                                    & "solduri_casa.magazin, " _
                                    & "solduri_casa.permanent " _
                                    & "FROM solduri_casa " _
                                    & "WHERE solduri_casa.magazin='" & ComboBox3.SelectedValue & "' ORDER BY data DESC"

            dbcomm = New MySqlCommand(casa_sql, dbconn)

            Try
                sda.SelectCommand = dbcomm
                sda.Fill(dbdataset)
                bsource.DataSource = dbdataset
                Sold_Casa_DGV.DataSource = bsource
                sda.Update(dbdataset)
            Catch ex As Exception
                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
        End Using
        Sold_Casa_DGV.Columns("data").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Sold_Casa_DGV.Columns("data").HeaderText = "Data"
        Sold_Casa_DGV.Columns("data").DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue
        Sold_Casa_DGV.Columns("data").DefaultCellStyle.SelectionForeColor = Color.White
        Sold_Casa_DGV.Columns("casa_sold_initial").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Sold_Casa_DGV.Columns("casa_sold_initial").HeaderText = "Sold Initial"
        Sold_Casa_DGV.Columns("casa_sold_initial").DefaultCellStyle.Format = "#,#0.00"
        Sold_Casa_DGV.Columns("incasari").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Sold_Casa_DGV.Columns("incasari").HeaderText = "Incasari"
        Sold_Casa_DGV.Columns("incasari").DefaultCellStyle.Format = "#,#0.00"

        Sold_Casa_DGV.Columns("cheltuieli").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Sold_Casa_DGV.Columns("cheltuieli").HeaderText = "Cheltuieli"
        Sold_Casa_DGV.Columns("cheltuieli").DefaultCellStyle.Format = "#,#0.00"
        Sold_Casa_DGV.Columns("casa_sold_final").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Sold_Casa_DGV.Columns("casa_sold_final").HeaderText = "Sold Final"
        Sold_Casa_DGV.Columns("casa_sold_final").DefaultCellStyle.Format = "#,#0.00"
        Sold_Casa_DGV.Columns("magazin").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Sold_Casa_DGV.Columns("magazin").HeaderText = "Mag"
        Sold_Casa_DGV.Columns("magazin").DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue
        Sold_Casa_DGV.Columns("magazin").DefaultCellStyle.SelectionForeColor = Color.White
        Sold_Casa_DGV.Columns("permanent").DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue
        Sold_Casa_DGV.Columns("permanent").DefaultCellStyle.SelectionForeColor = Color.White
        For r = 0 To Sold_Casa_DGV.RowCount - 1

            Sold_Casa_DGV.Rows(r).Cells("casa_sold_initial").Style.BackColor = Color.LightBlue
            Sold_Casa_DGV.Rows(r).Cells("casa_sold_initial").Style.SelectionBackColor = Color.LightSeaGreen
            Sold_Casa_DGV.Rows(r).Cells("incasari").Style.BackColor = Color.LightBlue
            Sold_Casa_DGV.Rows(r).Cells("incasari").Style.SelectionBackColor = Color.LightSeaGreen
            Sold_Casa_DGV.Rows(r).Cells("cheltuieli").Style.BackColor = Color.LightBlue
            Sold_Casa_DGV.Rows(r).Cells("cheltuieli").Style.SelectionBackColor = Color.LightSeaGreen
            Sold_Casa_DGV.Rows(r).Cells("casa_sold_final").Style.BackColor = Color.LightBlue
            Sold_Casa_DGV.Rows(r).Cells("casa_sold_final").Style.SelectionBackColor = Color.LightSeaGreen

        Next
        For i = 1 To Sold_Casa_DGV.RowCount - 1
            For j = 1 To Sold_Casa_DGV.ColumnCount - 3

                If Sold_Casa_DGV.Rows(i).Cells(j).Value = 0 Then
                    Sold_Casa_DGV.Rows(i).Cells(j).Style.BackColor = Color.Yellow
                    Sold_Casa_DGV.Rows(i).Cells(j).Style.SelectionBackColor = Color.Goldenrod
                End If
            Next

            If CDec(Sold_Casa_DGV.Rows(i).Cells("casa_sold_initial").Value) + CDec(Sold_Casa_DGV.Rows(i).Cells("incasari").Value) - CDec(Sold_Casa_DGV.Rows(i).Cells("cheltuieli").Value) <> CDec(Sold_Casa_DGV.Rows(i).Cells("casa_sold_final").Value) Then
                Sold_Casa_DGV.Rows(i).Cells("data").Style.BackColor = Color.Pink
                Sold_Casa_DGV.Rows(i).Cells("data").Style.SelectionBackColor = Color.DeepPink
            End If

            If Sold_Casa_DGV.Rows(i - 1).Cells("casa_sold_initial").Value <> Sold_Casa_DGV.Rows(i).Cells("casa_sold_final").Value Then
                Sold_Casa_DGV.Rows(i - 1).Cells("casa_sold_initial").Style.BackColor = Color.Red
                Sold_Casa_DGV.Rows(i - 1).Cells("casa_sold_initial").Style.SelectionBackColor = Color.DarkRed
                Sold_Casa_DGV.Rows(i).Cells("casa_sold_final").Style.BackColor = Color.Red
                Sold_Casa_DGV.Rows(i).Cells("casa_sold_final").Style.SelectionBackColor = Color.DarkRed
            End If

        Next

        ' -------------------- LOAD solduri_gestiune INTO GRID

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            'Dim tot_sql As String = "SELECT * FROM solduri_" & ComboBox3.SelectedValue & " ORDER BY data ASC"
            Dim tot_sql As String = "SELECT solduri_gestiune.data," _
                                    & "gestiune_sold_initial," _
                                    & "intrari,iesiri," _
                                    & "gestiune_sold_final," _
                                    & "magazin, " _
                                    & "permanent " _
                                    & "FROM solduri_gestiune " _
                                    & "WHERE magazin='" & ComboBox3.SelectedValue & "' ORDER BY data DESC"

            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                sda.SelectCommand = dbcomm
                sda.Fill(dbdataset)
                bsource.DataSource = dbdataset
                Sold_Gestiune_DGV.DataSource = bsource
                sda.Update(dbdataset)
            Catch ex As Exception
                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
        End Using
        Sold_Gestiune_DGV.Columns("data").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Sold_Gestiune_DGV.Columns("data").HeaderText = "Data"
        Sold_Gestiune_DGV.Columns("data").DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue
        Sold_Gestiune_DGV.Columns("data").DefaultCellStyle.SelectionForeColor = Color.White
        Sold_Gestiune_DGV.Columns("gestiune_sold_initial").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Sold_Gestiune_DGV.Columns("gestiune_sold_initial").HeaderText = "Sold Initial"
        Sold_Gestiune_DGV.Columns("gestiune_sold_initial").DefaultCellStyle.Format = "#,#0.00"
        Sold_Gestiune_DGV.Columns("iesiri").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Sold_Gestiune_DGV.Columns("iesiri").HeaderText = "Iesiri"
        Sold_Gestiune_DGV.Columns("iesiri").DefaultCellStyle.Format = "#,#0.00"
        Sold_Gestiune_DGV.Columns("intrari").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Sold_Gestiune_DGV.Columns("intrari").HeaderText = "Intrari"
        Sold_Gestiune_DGV.Columns("intrari").DefaultCellStyle.Format = "#,#0.00"
        Sold_Gestiune_DGV.Columns("gestiune_sold_final").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Sold_Gestiune_DGV.Columns("gestiune_sold_final").HeaderText = "Sold Final"
        Sold_Gestiune_DGV.Columns("gestiune_sold_final").DefaultCellStyle.Format = "#,#0.00"
        Sold_Gestiune_DGV.Columns("magazin").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Sold_Gestiune_DGV.Columns("magazin").HeaderText = "Mag"
        Sold_Gestiune_DGV.Columns("magazin").DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue
        Sold_Gestiune_DGV.Columns("magazin").DefaultCellStyle.SelectionForeColor = Color.White
        Sold_Gestiune_DGV.Columns("permanent").DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue
        Sold_Gestiune_DGV.Columns("permanent").DefaultCellStyle.SelectionForeColor = Color.White
        For r = 0 To Sold_Gestiune_DGV.RowCount - 1


            Sold_Gestiune_DGV.Rows(r).Cells("gestiune_sold_initial").Style.BackColor = Color.LightGreen
            Sold_Gestiune_DGV.Rows(r).Cells("gestiune_sold_initial").Style.SelectionBackColor = Color.LightSeaGreen
            Sold_Gestiune_DGV.Rows(r).Cells("intrari").Style.BackColor = Color.LightGreen
            Sold_Gestiune_DGV.Rows(r).Cells("intrari").Style.SelectionBackColor = Color.LightSeaGreen
            Sold_Gestiune_DGV.Rows(r).Cells("iesiri").Style.BackColor = Color.LightGreen
            Sold_Gestiune_DGV.Rows(r).Cells("iesiri").Style.SelectionBackColor = Color.LightSeaGreen
            Sold_Gestiune_DGV.Rows(r).Cells("gestiune_sold_final").Style.BackColor = Color.LightGreen
            Sold_Gestiune_DGV.Rows(r).Cells("gestiune_sold_final").Style.SelectionBackColor = Color.LightSeaGreen
        Next
        For i = 1 To Sold_Gestiune_DGV.RowCount - 1


            For j = 1 To Sold_Gestiune_DGV.ColumnCount - 3

                If Sold_Gestiune_DGV.Rows(i).Cells(j).Value = 0 Then
                    Sold_Gestiune_DGV.Rows(i).Cells(j).Style.BackColor = Color.Yellow
                    Sold_Gestiune_DGV.Rows(i).Cells(j).Style.SelectionBackColor = Color.Goldenrod
                End If
            Next

            'End If
            If Sold_Gestiune_DGV.Rows(i - 1).Cells("gestiune_sold_initial").Value <> Sold_Gestiune_DGV.Rows(i).Cells("gestiune_sold_final").Value Then
                Sold_Gestiune_DGV.Rows(i - 1).Cells("gestiune_sold_initial").Style.BackColor = Color.Red
                Sold_Gestiune_DGV.Rows(i - 1).Cells("gestiune_sold_initial").Style.SelectionBackColor = Color.DarkRed
                Sold_Gestiune_DGV.Rows(i).Cells("gestiune_sold_final").Style.BackColor = Color.Red
                Sold_Gestiune_DGV.Rows(i).Cells("gestiune_sold_final").Style.SelectionBackColor = Color.DarkRed
            End If
        Next
        Sold_Casa_DGV.ClearSelection()
        Sold_Casa_DGV.CurrentCell = Nothing
        Sold_Gestiune_DGV.ClearSelection()
        Sold_Gestiune_DGV.CurrentCell = Nothing
        'Sold_Casa_DGV.FirstDisplayedScrollingRowIndex = Sold_Casa_DGV.Rows.Count - 1
        'Sold_Gestiune_DGV.FirstDisplayedScrollingRowIndex = Sold_Gestiune_DGV.Rows.Count - 1
    End Sub
    Public Sub Load_Firme()
        Dim DGV As DataGridView = Firme_DGV
        DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.AllowUserToAddRows = False
        DGV.AllowUserToResizeRows = False
        DGV.RowHeadersVisible = False

        Cauta_firme_TB.BackColor = Color.MintCream
        Dim caut As String = Cauta_firme_TB.Text
        ' -------------------- LOAD firme INTO GRID
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            'Dim tot_sql As String = "SELECT id,firma,forma_juridica,cui,j,adresa,tip,status,cont,banca FROM firme ORDER BY firma ASC"
            Dim tot_sql As String = "SELECT id,firma,forma_juridica,cui,tva,j,adresa,localitate,judet,tip,status,cont,banca FROM firme WHERE firma LIKE @caut OR cui LIKE @caut ORDER BY firma ASC"


            dbcomm = New MySqlCommand(tot_sql, dbconn)
            dbcomm.Parameters.AddWithValue("@caut", "%" + caut + "%")
            Try
                sda.SelectCommand = dbcomm
                sda.Fill(dbdataset)
                bsource.DataSource = dbdataset
                DGV.DataSource = bsource
                sda.Update(dbdataset)
            Catch ex As Exception
                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
            'dbread.Close()
        End Using
        For Each DataGridViewColumn In DGV.Columns
            DGV.Columns("id").Visible = False
            DGV.Columns("tva").Visible = False
            DGV.Columns("j").Visible = False
            DGV.Columns("adresa").Visible = False
            DGV.Columns("localitate").Visible = False
            DGV.Columns("judet").Visible = False
            DGV.Columns("cont").Visible = False
            DGV.Columns("banca").Visible = False
        Next

        For i = 0 To DGV.RowCount - 1
            DGV.Rows(i).Cells("firma").ToolTipText = "Firma: " & DGV.Rows(i).Cells("firma").Value.ToString & " " & DGV.Rows(i).Cells("forma_juridica").Value.ToString & vbNewLine _
                & "CUI: " & DGV.Rows(i).Cells("cui").Value.ToString & ", J: " & DGV.Rows(i).Cells("j").Value.ToString & vbNewLine _
                & "Adresa: " & DGV.Rows(i).Cells("adresa").Value.ToString & vbNewLine _
                & "Tip: " & DGV.Rows(i).Cells("tip").Value.ToString & vbNewLine _
                & "Status" & DGV.Rows(i).Cells("status").Value.ToString & vbNewLine _
                & "IBAN: " & DGV.Rows(i).Cells("cont").Value.ToString & vbNewLine _
                & "Banca: " & DGV.Rows(i).Cells("banca").Value.ToString

        Next

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")

            'Dim tot_sql As String = "SELECT id,firma,forma_juridica,cui,j,adresa,tip,status,cont,banca FROM firme ORDER BY firma ASC"
            'Dim tot_sql As String = "SELECT SUM(suma) FROM cheltuieli WHERE explicatii LIKE '%" & caut & "%' OR cui LIKE '%" & caut & "%'"
            Dim tot_sql As String = "SELECT SUM(suma) FROM cheltuieli WHERE explicatii LIKE @caut " 'OR cui LIKE '%@caut%'"
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(tot_sql, dbconn)
            dbcomm.Parameters.AddWithValue("@caut", "%" + caut + "%")
            Try
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If IsDBNull(dbread("SUM(suma)")) = False Then
                    Label29.Text = dbread("sum(suma)")
                Else : Label29.Text = 0
                End If
            Catch ex As Exception
                MsgBox("Problem loading label: " & ex.Message.ToString)
            End Try
            'dbread.Close()
        End Using

        DGV.Columns("firma").FillWeight = DGV.Width * 45 / 100
        DGV.Columns("firma").HeaderText = "Firma"
        DGV.Columns("firma").DefaultCellStyle.Font = New Font(DGV.Font, FontStyle.Bold)
        DGV.Columns("firma").DefaultCellStyle.BackColor = Color.Azure
        DGV.Columns("firma").DefaultCellStyle.SelectionBackColor = Color.LightBlue
        DGV.Columns("firma").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("firma").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("forma_juridica").FillWeight = DGV.Width * 10 / 100
        DGV.Columns("forma_juridica").HeaderText = "Forma Juridica"
        DGV.Columns("forma_juridica").DefaultCellStyle.Font = New Font(DGV.Font, FontStyle.Bold)
        DGV.Columns("forma_juridica").DefaultCellStyle.BackColor = Color.Azure
        DGV.Columns("forma_juridica").DefaultCellStyle.SelectionBackColor = Color.LightBlue
        DGV.Columns("forma_juridica").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("forma_juridica").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("cui").FillWeight = DGV.Width * 10 / 100
        DGV.Columns("cui").HeaderText = "CUI"
        DGV.Columns("cui").DefaultCellStyle.BackColor = Color.LightPink
        DGV.Columns("cui").DefaultCellStyle.SelectionBackColor = Color.HotPink
        DGV.Columns("cui").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("cui").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("tip").FillWeight = DGV.Width * 15 / 100
        DGV.Columns("tip").HeaderText = "Tip"
        DGV.Columns("tip").DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
        DGV.Columns("tip").DefaultCellStyle.SelectionBackColor = Color.SandyBrown
        DGV.Columns("tip").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("tip").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        'For Each control In Incasari_DGV.Controls
        '    If control.GetType() Is GetType(VScrollBar) Then
        '        Dim vbar As VScrollBar = DirectCast(control, VScrollBar)
        '        If vbar.Visible = True Then
        '            DGV.Columns("status").FillWeight = DGV.Width * 14.5 / 100
        '        Else
        DGV.Columns("status").FillWeight = DGV.Width * 20 / 100
        '        End If
        '    End If
        'Next
        DGV.Columns("status").HeaderText = "Status"
        DGV.Columns("status").DefaultCellStyle.BackColor = Color.PaleGreen
        DGV.Columns("status").DefaultCellStyle.SelectionBackColor = Color.LightSeaGreen
        DGV.Columns("status").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        For i = 0 To DGV.Rows.Count - 1
            If DGV.Rows(i).Cells("tip").Value = "Taxe" Then
                DGV.Rows(i).Cells("firma").Style.ForeColor = Color.Red
                DGV.Rows(i).Cells("firma").Style.SelectionForeColor = Color.Red
                DGV.Rows(i).Cells("forma_juridica").Style.ForeColor = Color.Red
                DGV.Rows(i).Cells("forma_juridica").Style.SelectionForeColor = Color.Red
            End If
            If DGV.Rows(i).Cells("tip").Value = "Servicii" Then
                DGV.Rows(i).Cells("firma").Style.ForeColor = Color.Blue
                DGV.Rows(i).Cells("firma").Style.SelectionForeColor = Color.Blue
                DGV.Rows(i).Cells("forma_juridica").Style.ForeColor = Color.Blue
                DGV.Rows(i).Cells("forma_juridica").Style.SelectionForeColor = Color.Blue
            End If
        Next
        DGV.ClearSelection()
        DGV.CurrentCell = Nothing
    End Sub
    Public Sub Load_Marfa()

        Dim DGV As DataGridView = Marfa_DGV
        DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.AllowUserToAddRows = False
        DGV.AllowUserToResizeRows = False
        'DGV.CellBorderStyle = DataGridViewCellBorderStyle.None
        DGV.RowHeadersVisible = False

        cauta_prod_TB.BackColor = Color.MintCream
        Dim grup As String = ""
        Dim bucati As String = ""
        Dim dat As String = ""
        If CheckBox2.CheckState = CheckState.Unchecked Then
            grup = "  ORDER BY produs ASC,pret ASC,an DESC,luna DESC "
            bucati = "bucati"
            dat = "MONTHNAME(data) as luna, YEAR(data) AS an"
        ElseIf CheckBox2.CheckState = CheckState.Checked Then
            grup = "GROUP BY produs,pret ORDER BY produs ASC,pret ASC,an DESC,MONTH(MAX(data)) DESC "
            bucati = "SUM(bucati) AS bucati"
            dat = "MONTHNAME(MAX(data)) AS luna, YEAR(MAX(data)) AS an"
        End If
        Dim prod As String = cauta_prod_TB.Text
        ' -------------------- LOAD firme INTO GRID
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String = "SELECT id,produs,pret," & bucati & ",nir,data," & dat & ",magazin FROM marfa WHERE produs LIKE '%" & prod & "%' " & grup & ""

            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                sda.SelectCommand = dbcomm
                sda.Fill(dbdataset)
                bsource.DataSource = dbdataset
                DGV.DataSource = bsource
                sda.Update(dbdataset)
            Catch ex As Exception
                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
        End Using
        For Each DataGridViewColumn In DGV.Columns
            DGV.Columns("id").Visible = False
            DGV.Columns("magazin").Visible = False
        Next
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String = "SELECT SUM(valoare) as total FROM (SELECT pret*bucati as valoare FROM marfa WHERE produs LIKE '%" & prod & "%') AS totaluri"

            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                Dim suma As Decimal = 0
                dbconn.Open()
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If IsDBNull(dbread("total")) Then
                    suma = 0
                Else
                    suma = CDec(dbread("total"))
                End If
                Label6.Text = "Total: " & FormatNumber(suma, 2, TriState.True, TriState.False, TriState.True) & " lei"
            Catch ex As Exception
                MsgBox("Problem loading suma incasari: " & ex.Message.ToString)
            End Try
        End Using
        DGV.Columns("data").Visible = False
        DGV.Columns("produs").FillWeight = DGV.Width * 35 / 100
        DGV.Columns("produs").HeaderText = "Produs"
        DGV.Columns("produs").DefaultCellStyle.Font = New Font(DGV.Font, FontStyle.Bold)
        DGV.Columns("produs").DefaultCellStyle.BackColor = Color.Azure
        DGV.Columns("produs").DefaultCellStyle.SelectionBackColor = Color.LightBlue
        DGV.Columns("produs").DefaultCellStyle.SelectionForeColor = Color.Black
        'DGV.Columns("produs").SortMode = DataGridViewColumnSortMode.NotSortable
        DGV.Columns("produs").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("pret").FillWeight = DGV.Width * 15 / 100
        DGV.Columns("pret").HeaderText = "Pret"
        DGV.Columns("pret").DefaultCellStyle.Font = New Font(DGV.Font, FontStyle.Bold)
        DGV.Columns("pret").DefaultCellStyle.BackColor = Color.Azure
        DGV.Columns("pret").DefaultCellStyle.SelectionBackColor = Color.LightBlue
        DGV.Columns("pret").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("pret").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("bucati").FillWeight = DGV.Width * 10 / 100
        DGV.Columns("bucati").HeaderText = "Bucati"
        DGV.Columns("bucati").DefaultCellStyle.BackColor = Color.Pink
        DGV.Columns("bucati").DefaultCellStyle.SelectionBackColor = Color.HotPink
        DGV.Columns("bucati").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("bucati").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("nir").FillWeight = DGV.Width * 10 / 100
        DGV.Columns("nir").HeaderText = "NIR"
        DGV.Columns("nir").DefaultCellStyle.BackColor = Color.LightYellow
        DGV.Columns("nir").DefaultCellStyle.SelectionBackColor = Color.SandyBrown
        DGV.Columns("nir").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("nir").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("luna").FillWeight = DGV.Width * 15 / 100
        'DGV.Columns("luna").ValueType = GetType(System.String)
        DGV.Columns("luna").HeaderText = "Luna"
        DGV.Columns("luna").DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
        DGV.Columns("luna").DefaultCellStyle.SelectionBackColor = Color.SandyBrown
        DGV.Columns("luna").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("luna").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("an").FillWeight = DGV.Width * 15 / 100
        DGV.Columns("an").HeaderText = "An"
        DGV.Columns("an").DefaultCellStyle.BackColor = Color.PaleGreen
        DGV.Columns("an").DefaultCellStyle.SelectionBackColor = Color.LightSeaGreen
        DGV.Columns("an").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("an").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        'MsgBox(DGV.Columns("luna").ValueType.ToString)

        For i = 1 To DGV.RowCount - 1

            If DGV.Rows(i).Cells("produs").Value = DGV.Rows(i - 1).Cells("produs").Value Then
                'DGV.Rows(i).DefaultCellStyle.BackColor = DGV.Rows(i - 1).DefaultCellStyle.BackColor
                DGV.Rows(i).Cells("produs").Style.BackColor = DGV.Rows(i - 1).Cells("produs").Style.BackColor
                DGV.Rows(i).Cells("pret").Style.BackColor = DGV.Rows(i - 1).Cells("pret").Style.BackColor
                DGV.Rows(i).Cells("bucati").Style.BackColor = DGV.Rows(i - 1).Cells("bucati").Style.BackColor
                DGV.Rows(i).Cells("nir").Style.BackColor = DGV.Rows(i - 1).Cells("nir").Style.BackColor
                DGV.Rows(i).Cells("luna").Style.BackColor = DGV.Rows(i - 1).Cells("luna").Style.BackColor
                DGV.Rows(i).Cells("an").Style.BackColor = DGV.Rows(i - 1).Cells("an").Style.BackColor

            ElseIf DGV.Rows(i).Cells("produs").Value <> DGV.Rows(i - 1).Cells("produs").Value And DGV.Rows(i - 1).Cells("produs").Style.BackColor <> Color.Lavender Then

                DGV.Rows(i).Cells("produs").Style.BackColor = Color.Lavender
                DGV.Rows(i).Cells("pret").Style.BackColor = Color.Lavender
                DGV.Rows(i).Cells("bucati").Style.BackColor = Color.LightPink
                DGV.Rows(i).Cells("nir").Style.BackColor = Color.PaleGoldenrod
                DGV.Rows(i).Cells("luna").Style.BackColor = Color.PaleGoldenrod
                DGV.Rows(i).Cells("an").Style.BackColor = Color.LightGreen
            End If
        Next
        DGV.ClearSelection()
        DGV.CurrentCell = Nothing
    End Sub
    Public Sub Load_Banca()

        Dim DGV As DataGridView = Banca_DGV
        DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.AllowUserToAddRows = False
        DGV.AllowUserToResizeRows = False
        'DGV.CellBorderStyle = DataGridViewCellBorderStyle.None
        DGV.RowHeadersVisible = False

        'cauta_prod_TB.BackColor = Color.MintCream
        'Dim grup As String = ""
        'Dim bucati As String = ""
        'Dim dat As String = ""
        'If CheckBox2.CheckState = CheckState.Unchecked Then
        '    grup = "  ORDER BY produs ASC,pret ASC,an DESC,luna DESC "
        '    bucati = "bucati"
        '    dat = "MONTHNAME(data) as luna, YEAR(data) AS an"
        'ElseIf CheckBox2.CheckState = CheckState.Checked Then
        '    grup = "GROUP BY produs,pret ORDER BY produs ASC,pret ASC,an DESC,MONTH(MAX(data)) DESC "
        '    bucati = "SUM(bucati) AS bucati"
        '    dat = "MONTHNAME(MAX(data)) AS luna, YEAR(MAX(data)) AS an"
        'End If
        'Dim prod As String = cauta_prod_TB.Text
        ' -------------------- LOAD firme INTO GRID
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String = "SELECT banca_tranzactii.data,banca_tranzactii.tip_tranzactie,banca_tranzactii.suma,banca_comisioane.suma as comision,banca_tranzactii.descriere " _
                                    & " FROM banca_tranzactii LEFT JOIN banca_comisioane on banca_tranzactii.id_tranzactie=banca_comisioane.id_tranzactie ORDER BY banca_tranzactii.data DESC"

            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                sda.SelectCommand = dbcomm
                sda.Fill(dbdataset)
                bsource.DataSource = dbdataset
                DGV.DataSource = bsource
                sda.Update(dbdataset)
            Catch ex As Exception
                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
        End Using
        'For Each DataGridViewColumn In DGV.Columns
        '    DGV.Columns("id").Visible = False
        'Next
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String = "SELECT * FROM " _
                                    & "(SELECT sum(suma) as incas FROM banca_tranzactii WHERE tip_tranzactie='Incasare') incasari, " _
                                    & "(SELECT sum(suma) as plati FROM banca_tranzactii WHERE tip_tranzactie='Plata') plati, " _
                                    & "(SELECT sum(suma) as comis FROM banca_comisioane) comisioane "

            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                Dim incas As Decimal = 0
                Dim plati As Decimal = 0
                Dim comis As Decimal = 0
                dbconn.Open()
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If IsDBNull(dbread("incas")) Then
                    incas = 0
                Else
                    incas = CDec(dbread("incas"))
                End If

                If IsDBNull(dbread("plati")) Then
                    plati = 0
                Else
                    plati = CDec(dbread("plati"))
                End If

                If IsDBNull(dbread("comis")) Then
                    comis = 0
                Else
                    comis = CDec(dbread("comis"))
                End If
                ba_Incasari_TB.Text = Format(incas, "#,#0.00")
                ba_Plati_TB.Text = Format(plati, "#,#0.00")
                ba_comis_TB.Text = Format(comis, "#,#0.00")

                Dim sold_bnc As Decimal = incas - plati - comis
                ba_Sold_TB.Text = Format(sold_bnc, "#,#0.00")
            Catch ex As Exception
                MsgBox("Problem loading suma incasari: " & ex.Message.ToString)
            End Try
        End Using

        DGV.Columns("data").FillWeight = DGV.Width * 10 / 100
        DGV.Columns("data").HeaderText = "Data"
        'DGV.Columns("data").DefaultCellStyle.Font = New Font(DGV.Font, FontStyle.Bold)
        DGV.Columns("data").DefaultCellStyle.BackColor = Color.Azure
        DGV.Columns("data").DefaultCellStyle.SelectionBackColor = Color.LightBlue
        DGV.Columns("data").DefaultCellStyle.SelectionForeColor = Color.Black
        'DGV.Columns("data").SortMode = DataGridViewColumnSortMode.NotSortable
        DGV.Columns("data").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("tip_tranzactie").FillWeight = DGV.Width * 15 / 100
        DGV.Columns("tip_tranzactie").HeaderText = "Tip Tranzactie"
        DGV.Columns("tip_tranzactie").DefaultCellStyle.Font = New Font(DGV.Font, FontStyle.Bold)
        DGV.Columns("tip_tranzactie").DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
        DGV.Columns("tip_tranzactie").DefaultCellStyle.SelectionBackColor = Color.SandyBrown
        DGV.Columns("tip_tranzactie").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("tip_tranzactie").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.Columns("suma").FillWeight = DGV.Width * 15 / 100
        DGV.Columns("suma").HeaderText = "Suma"
        DGV.Columns("suma").DefaultCellStyle.Font = New Font(DGV.Font, FontStyle.Bold)
        DGV.Columns("suma").DefaultCellStyle.Format = "#,#0.00"
        DGV.Columns("suma").DefaultCellStyle.BackColor = Color.Azure
        DGV.Columns("suma").DefaultCellStyle.SelectionBackColor = Color.LightBlue
        DGV.Columns("suma").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("suma").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DGV.Columns("comision").FillWeight = DGV.Width * 15 / 100
        DGV.Columns("comision").HeaderText = "Comision"
        DGV.Columns("comision").DefaultCellStyle.Format = "#,#0.00"
        DGV.Columns("comision").DefaultCellStyle.BackColor = Color.Pink
        DGV.Columns("comision").DefaultCellStyle.SelectionBackColor = Color.HotPink
        DGV.Columns("comision").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("comision").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DGV.Columns("descriere").FillWeight = DGV.Width * 45 / 100
        DGV.Columns("descriere").HeaderText = "Descriere"
        DGV.Columns("descriere").DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
        DGV.Columns("descriere").DefaultCellStyle.SelectionBackColor = Color.SandyBrown
        DGV.Columns("descriere").DefaultCellStyle.SelectionForeColor = Color.Black
        DGV.Columns("descriere").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ''MsgBox(DGV.Columns("luna").ValueType.ToString)

        For i = 1 To DGV.RowCount - 1

            If DGV.Rows(i).Cells("tip_tranzactie").Value = "Plata" Then
                'DGV.Rows(i).DefaultCellStyle.BackColor = DGV.Rows(i - 1).DefaultCellStyle.BackColor
                DGV.Rows(i).DefaultCellStyle.ForeColor = Color.OrangeRed
                DGV.Rows(i).DefaultCellStyle.ForeColor = Color.OrangeRed
                DGV.Rows(i).DefaultCellStyle.SelectionForeColor = Color.Red
            End If

            If DGV.Rows(i).Cells("tip_tranzactie").Value = "Incasare" Then
                If DGV.Rows(i).Cells("descriere").Value.Contains("36005685") Then
                    DGV.Rows(i).Cells("descriere").Style.BackColor = Color.OrangeRed
                End If
                If DGV.Rows(i).Cells("descriere").Value.Contains("36006273") Then
                    DGV.Rows(i).Cells("descriere").Style.BackColor = Color.Turquoise
                End If
            End If
        Next

        ' MsgBox(DGV.Columns("suma").Width)

        Dim x_inc As Integer = DGV.Location.X + DGV.Columns("data").Width + DGV.Columns("tip_tranzactie").Width + 2
        Dim y_inc As Integer = DGV.Location.Y + DGV.Height + 6

        ba_Incasari_TB.Location = New System.Drawing.Point(x_inc, y_inc)
        ba_Incasari_TB.Width = DGV.Columns("tip_tranzactie").Width - 2

        ba_Plati_TB.Location = New System.Drawing.Point(x_inc, y_inc + 24)
        ba_Plati_TB.Width = DGV.Columns("tip_tranzactie").Width - 2

        Dim x_com As Integer = DGV.Location.X + DGV.Columns("data").Width + DGV.Columns("tip_tranzactie").Width + DGV.Columns("suma").Width + 2

        ba_comis_TB.Location = New System.Drawing.Point(x_com, y_inc)
        ba_comis_TB.Width = DGV.Columns("comision").Width - 2

        Label22.Location = New System.Drawing.Point(x_inc - Label22.Width - 5, y_inc)
        Label23.Location = New System.Drawing.Point(x_inc - Label23.Width - 5, y_inc + 24)

        DGV.ClearSelection()
        DGV.CurrentCell = Nothing
        AddHandler Banca_DGV.Resize, AddressOf Banca_DGV_Resize
    End Sub


    Private Sub Banca_DGV_Resize(sender As Object, e As EventArgs)
        Dim DGV As DataGridView = Banca_DGV
        Dim x_inc As Integer = DGV.Location.X + DGV.Columns("data").Width + DGV.Columns("tip_tranzactie").Width + 2
        Dim y_inc As Integer = DGV.Location.Y + DGV.Height + 6
        ba_Incasari_TB.Location = New System.Drawing.Point(x_inc, y_inc)
        ba_Incasari_TB.Width = DGV.Columns("tip_tranzactie").Width - 2

        ba_Plati_TB.Location = New System.Drawing.Point(x_inc, y_inc + 24)
        ba_Plati_TB.Width = DGV.Columns("tip_tranzactie").Width - 2

        'Dim x_com As Integer = DGV.Location.X + DGV.Columns("data").Width + DGV.Columns("tip_tranzactie").Width + DGV.Columns("suma").Width + 2
        Dim x_com As Integer = (x_inc + ba_Incasari_TB.Width + 4)

        ba_comis_TB.Location = New System.Drawing.Point(x_com, y_inc)
        ba_comis_TB.Width = DGV.Columns("comision").Width - 2

        Label22.Location = New System.Drawing.Point(x_inc - Label22.Width - 5, y_inc)
        Label23.Location = New System.Drawing.Point(x_inc - Label23.Width - 5, y_inc + 24)
    End Sub

    Public Sub Load_Facturi()

        Dim DGV As DataGridView = Facturi_DGV
        DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.AllowUserToAddRows = False
        DGV.AllowUserToResizeRows = False
        'DGV.CellBorderStyle = DataGridViewCellBorderStyle.None
        DGV.RowHeadersVisible = False

        'cauta_prod_TB.BackColor = Color.MintCream
        'Dim grup As String = ""
        'Dim bucati As String = ""
        'Dim dat As String = ""
        'If CheckBox2.CheckState = CheckState.Unchecked Then
        '    grup = "  ORDER BY produs ASC,pret ASC,an DESC,luna DESC "
        '    bucati = "bucati"
        '    dat = "MONTHNAME(data) as luna, YEAR(data) AS an"
        'ElseIf CheckBox2.CheckState = CheckState.Checked Then
        '    grup = "GROUP BY produs,pret ORDER BY produs ASC,pret ASC,an DESC,MONTH(MAX(data)) DESC "
        '    bucati = "SUM(bucati) AS bucati"
        '    dat = "MONTHNAME(MAX(data)) AS luna, YEAR(MAX(data)) AS an"
        'End If
        'Dim prod As String = cauta_prod_TB.Text


        ' -------------------- LOAD firme INTO GRID
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String = "SELECT facturi.id,numar,data,valoare,firma FROM facturi left join firme on cui_firma=firme.cui order by numar desc"

            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                sda.SelectCommand = dbcomm
                sda.Fill(dbdataset)
                bsource.DataSource = dbdataset
                DGV.DataSource = bsource
                sda.Update(dbdataset)
            Catch ex As Exception
                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
        End Using
        For Each DataGridViewColumn In DGV.Columns
            DGV.Columns("id").Visible = False

        Next
        'Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        '    Dim sda As New MySqlDataAdapter
        '    Dim dbdataset As New DataTable
        '    Dim bsource As New BindingSource

        '    Dim tot_sql As String = "SELECT SUM(valoare) as total FROM (SELECT pret*bucati as valoare FROM marfa WHERE produs LIKE '%" & prod & "%') AS totaluri"

        '    dbcomm = New MySqlCommand(tot_sql, dbconn)

        '    Try
        '        Dim suma As Decimal = 0
        '        dbconn.Open()
        '        dbcomm = New MySqlCommand(tot_sql, dbconn)
        '        dbread = dbcomm.ExecuteReader()
        '        dbread.Read()
        '        If IsDBNull(dbread("total")) Then
        '            suma = 0
        '        Else
        '            suma = CDec(dbread("total"))
        '        End If
        '        Label6.Text = "Total: " & FormatNumber(suma, 2, TriState.True, TriState.False, TriState.True) & " lei"
        '    Catch ex As Exception
        '        MsgBox("Problem loading suma incasari: " & ex.Message.ToString)
        '    End Try
        'End Using
        'DGV.Columns("data").Visible = False
        'DGV.Columns("produs").FillWeight = DGV.Width * 35 / 100
        'DGV.Columns("produs").HeaderText = "Produs"
        'DGV.Columns("produs").DefaultCellStyle.Font = New Font(DGV.Font, FontStyle.Bold)
        'DGV.Columns("produs").DefaultCellStyle.BackColor = Color.Azure
        'DGV.Columns("produs").DefaultCellStyle.SelectionBackColor = Color.LightBlue
        'DGV.Columns("produs").DefaultCellStyle.SelectionForeColor = Color.Black
        ''DGV.Columns("produs").SortMode = DataGridViewColumnSortMode.NotSortable
        'DGV.Columns("produs").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'DGV.Columns("pret").FillWeight = DGV.Width * 15 / 100
        'DGV.Columns("pret").HeaderText = "Pret"
        'DGV.Columns("pret").DefaultCellStyle.Font = New Font(DGV.Font, FontStyle.Bold)
        'DGV.Columns("pret").DefaultCellStyle.BackColor = Color.Azure
        'DGV.Columns("pret").DefaultCellStyle.SelectionBackColor = Color.LightBlue
        'DGV.Columns("pret").DefaultCellStyle.SelectionForeColor = Color.Black
        'DGV.Columns("pret").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'DGV.Columns("bucati").FillWeight = DGV.Width * 10 / 100
        'DGV.Columns("bucati").HeaderText = "Bucati"
        'DGV.Columns("bucati").DefaultCellStyle.BackColor = Color.Pink
        'DGV.Columns("bucati").DefaultCellStyle.SelectionBackColor = Color.HotPink
        'DGV.Columns("bucati").DefaultCellStyle.SelectionForeColor = Color.Black
        'DGV.Columns("bucati").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'DGV.Columns("nir").FillWeight = DGV.Width * 10 / 100
        'DGV.Columns("nir").HeaderText = "NIR"
        'DGV.Columns("nir").DefaultCellStyle.BackColor = Color.LightYellow
        'DGV.Columns("nir").DefaultCellStyle.SelectionBackColor = Color.SandyBrown
        'DGV.Columns("nir").DefaultCellStyle.SelectionForeColor = Color.Black
        'DGV.Columns("nir").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'DGV.Columns("luna").FillWeight = DGV.Width * 15 / 100
        ''DGV.Columns("luna").ValueType = GetType(System.String)
        'DGV.Columns("luna").HeaderText = "Luna"
        'DGV.Columns("luna").DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
        'DGV.Columns("luna").DefaultCellStyle.SelectionBackColor = Color.SandyBrown
        'DGV.Columns("luna").DefaultCellStyle.SelectionForeColor = Color.Black
        'DGV.Columns("luna").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'DGV.Columns("an").FillWeight = DGV.Width * 15 / 100
        'DGV.Columns("an").HeaderText = "An"
        'DGV.Columns("an").DefaultCellStyle.BackColor = Color.PaleGreen
        'DGV.Columns("an").DefaultCellStyle.SelectionBackColor = Color.LightSeaGreen
        'DGV.Columns("an").DefaultCellStyle.SelectionForeColor = Color.Black
        'DGV.Columns("an").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ''MsgBox(DGV.Columns("luna").ValueType.ToString)

        'For i = 1 To DGV.RowCount - 1

        '    If DGV.Rows(i).Cells("produs").Value = DGV.Rows(i - 1).Cells("produs").Value Then
        '        'DGV.Rows(i).DefaultCellStyle.BackColor = DGV.Rows(i - 1).DefaultCellStyle.BackColor
        '        DGV.Rows(i).Cells("produs").Style.BackColor = DGV.Rows(i - 1).Cells("produs").Style.BackColor
        '        DGV.Rows(i).Cells("pret").Style.BackColor = DGV.Rows(i - 1).Cells("pret").Style.BackColor
        '        DGV.Rows(i).Cells("bucati").Style.BackColor = DGV.Rows(i - 1).Cells("bucati").Style.BackColor
        '        DGV.Rows(i).Cells("nir").Style.BackColor = DGV.Rows(i - 1).Cells("nir").Style.BackColor
        '        DGV.Rows(i).Cells("luna").Style.BackColor = DGV.Rows(i - 1).Cells("luna").Style.BackColor
        '        DGV.Rows(i).Cells("an").Style.BackColor = DGV.Rows(i - 1).Cells("an").Style.BackColor

        '    ElseIf DGV.Rows(i).Cells("produs").Value <> DGV.Rows(i - 1).Cells("produs").Value And DGV.Rows(i - 1).Cells("produs").Style.BackColor <> Color.Lavender Then

        '        DGV.Rows(i).Cells("produs").Style.BackColor = Color.Lavender
        '        DGV.Rows(i).Cells("pret").Style.BackColor = Color.Lavender
        '        DGV.Rows(i).Cells("bucati").Style.BackColor = Color.LightPink
        '        DGV.Rows(i).Cells("nir").Style.BackColor = Color.PaleGoldenrod
        '        DGV.Rows(i).Cells("luna").Style.BackColor = Color.PaleGoldenrod
        '        DGV.Rows(i).Cells("an").Style.BackColor = Color.LightGreen
        '    End If
        'Next
        'DGV.ClearSelection()
        DGV.CurrentCell = Nothing
    End Sub
    Private Sub DGV_prod_fact_click_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles Facturi_DGV.CellMouseClick
        'If e.Button = Windows.Forms.MouseButtons.Right And e.ColumnIndex > -1 And e.RowIndex > -1 Then
        '    Dim cell As DataGridViewCell = sender.Rows(e.RowIndex).Cells(e.ColumnIndex)
        '    sender.CurrentCell = cell
        '    sender.focus()
        'End If

        Dim DGV As DataGridView = prod_fact_DGV
        DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.AllowUserToAddRows = False
        DGV.AllowUserToResizeRows = False
        'DGV.CellBorderStyle = DataGridViewCellBorderStyle.None
        DGV.RowHeadersVisible = False
        'If IsDBNull(DataGridView1.SelectedRows(0).Cells("magazin").Value) = False Then
        '    mag_act = DataGridView1.SelectedRows(0).Cells("magazin").Value
        '    'Dim mag_nou As String = DataGridView1.SelectedRows(0).Cells("magazin").Value
        '    'If mag_act = "PM" Then
        '    '    mag_nou = "MV"
        '    'ElseIf mag_act = "MV" Then
        '    '    mag_nou = "PM"
        '    'End If
        'End If
        Dim row As DataGridViewRow = Facturi_DGV.CurrentRow
        Dim numar As String = row.Cells("numar").Value.ToString


        'Dim dbconn As New MySqlConnection
        Dim dbcomm As New MySqlCommand
        'dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String = "SELECT facturi.id,produs, prod_facturi.buc,pret, pret * prod_facturi.buc as valoare " _
                                    & "FROM prod_facturi left join facturi on nr_fact=numar where facturi.numar='" & numar & "'"

            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try
                sda.SelectCommand = dbcomm
                sda.Fill(dbdataset)
                bsource.DataSource = dbdataset
                DGV.DataSource = bsource
                sda.Update(dbdataset)
            Catch ex As Exception
                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
        End Using
        For Each DataGridViewColumn In DGV.Columns
            DGV.Columns("id").Visible = False

        Next


        ''Dim id As Integer = row.Cells("id").Value
        ''Dim data As Date = CDate(row.Cells("data").Value)

        'Dim tip_incasare As String = row.Cells("tip_incasare").Value.ToString
        'If tip_incasare = "RZF" Then
        '    nr_rzf = row.Cells("nr_rzf").Value.ToString.Substring(2)
        'End If

        'Dim explicatii As String = row.Cells("explicatii").Value
        'Dim suma_cash As Decimal = row.Cells("Cash").Value
        'Dim suma_card As Decimal = row.Cells("POS").Value
        'Dim cash_cont As Boolean = row.Cells("cash").Value



        'edit_Lbl.Text = "Edit"
        'id_Lbl.Text = id
        'mag_v_Lbl.Text = mag_act
        'rzf_v_Lbl.Text = row.Cells("nr_rzf").Value.ToString
        'GroupBox1.Visible = True

        'DateTimePicker1.Value = data
        'ComboBox1.SelectedIndex = ComboBox1.Items.IndexOf(tip_incasare)
        'nr_rzf_Textbox.Text = nr_rzf
        'explicatii_Textbox.Text = explicatii
        'suma_CASA_Textbox.Text = suma_cash
        'suma_CARD_Textbox.Text = suma_card
    End Sub
    Public Sub Load_Situatie()
        'ListBox1.Items.Clear()

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")

            Dim sda As New MySqlDataAdapter
            Dim dbtable As New DataTable
            Dim bsource As New BindingSource

            'Dim tot_sql As String = "SELECT * FROM " _
            '                        & "(SELECT sum(suma_cash) as inc_cash_l, sum(suma_card) as inc_card_l,(sum(suma_card)+sum(suma_cash)) as inc_tot_l FROM incasari WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_incasare = 'RZF') t_inc_luna," _
            '                        & "(SELECT sum(suma_cash) as inc_cash_a, sum(suma_card) as inc_card_a,(sum(suma_card)+sum(suma_cash)) as inc_tot_a FROM incasari WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_incasare = 'RZF') t_inc_an," _
            '                        & "(SELECT sum(suma_cash) as inc_cash_t, sum(suma_card) as inc_card_t,(sum(suma_card)+sum(suma_cash)) as inc_tot_t FROM incasari WHERE tip_incasare = 'RZF') t_inc_t," _
            '                        & "(SELECT avg(med_inc_lun) as med_inc_lun FROM (SELECT (sum(suma_card)+sum(suma_cash)) as med_inc_lun FROM incasari WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_incasare = 'RZF' GROUP BY MONTH(DATA)) med_inc_lun) med_inc_lun," _
            '                        & "(SELECT sum(suma_cash) as di_l FROM incasari WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_incasare = 'DI') t_di_luna," _
            '                        & "(SELECT sum(suma_cash) as di_a FROM incasari WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_incasare = 'DI') t_di_an," _
            '                        & "(SELECT sum(suma_cash) as di_t FROM incasari WHERE tip_incasare = 'DI') t_di_t," _
            '                        & "(SELECT sum(suma) as che_cash_l FROM cheltuieli WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala <> 'DP' AND cash=TRUE) t_che_cash_luna," _
            '                        & "(SELECT sum(suma) as che_cont_l FROM cheltuieli WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala <> 'DP' AND cash=FALSE AND nr_chitanta NOT LIKE('CP01.%')) t_che_cont_luna," _
            '                        & "(SELECT sum(suma) as che_cash_a FROM cheltuieli WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala <> 'DP' AND cash=TRUE) t_che_cash_an," _
            '                        & "(SELECT sum(suma) as che_cont_a FROM cheltuieli WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala <> 'DP' AND cash=FALSE AND nr_chitanta NOT LIKE('CP01.%')) t_che_cont_an," _
            '                        & "(SELECT sum(suma) as che_cash_t FROM cheltuieli WHERE tip_cheltuiala <> 'DP' AND cash=TRUE) t_che_cash_t," _
            '                        & "(SELECT sum(suma) as che_cont_t FROM cheltuieli WHERE tip_cheltuiala <> 'DP' AND cash=FALSE AND nr_chitanta NOT LIKE('CP01.%')) t_che_cont_t," _
            '                        & "(SELECT sum(suma) as dp_l FROM cheltuieli WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala = 'DP') t_dp_luna," _
            '                        & "(SELECT sum(suma) as dp_a FROM cheltuieli WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala = 'DP') t_dp_an," _
            '                        & "(SELECT sum(suma) as dp_t FROM cheltuieli WHERE tip_cheltuiala = 'DP') t_dp_t"
            '& "(SELECT sum(suma) as che_cont_l FROM cheltuieli WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala <> 'DP' AND cash=FALSE AND nr_chitanta NOT LIKE('CRRR.416010%')) t_che_cont_luna," _
            '& "(SELECT sum(suma) as che_cont_a FROM cheltuieli WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala <> 'DP' AND cash=FALSE AND nr_chitanta NOT LIKE('CRRR.416010%')) t_che_cont_an," _

            Dim tot_sql As String = "SELECT * FROM " _
                                    & "(SELECT sum(suma_cash) as v_cash_l, sum(suma_card) as v_card_l,(sum(suma_card)+sum(suma_cash)) as v_tot_l FROM incasari WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_incasare = 'RZF') t_v_luna," _
                                    & "(SELECT sum(suma_cash) as int_cash_l FROM incasari WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "') t_int_cash_luna," _
                                    & "(SELECT sum(suma) as int_card_l FROM banca_tranzactii WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_tranzactie = 'Incasare') t_int_card_luna," _
                                    & "(SELECT sum(suma_cash) as v_cash_a, sum(suma_card) as v_card_a,(sum(suma_card)+sum(suma_cash)) as v_tot_a FROM incasari WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_incasare = 'RZF') t_v_an," _
                                    & "(SELECT sum(suma_cash) as int_cash_a FROM incasari WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "') t_int_cash_an," _
                                    & "(SELECT sum(suma) as int_card_a FROM banca_tranzactii WHERE YEAR(data)='" & ComboBox2.SelectedItem & "' AND tip_tranzactie = 'Incasare') t_int_card_an," _
                                    & "(SELECT sum(suma_cash) as v_cash_t, sum(suma_card) as v_card_t,(sum(suma_card)+sum(suma_cash)) as v_tot_t FROM incasari WHERE tip_incasare = 'RZF') t_v_t," _
                                    & "(SELECT sum(suma_cash) as int_cash_t FROM incasari) t_int_cash_t," _
                                    & "(SELECT sum(suma) as int_card_t FROM banca_tranzactii WHERE tip_tranzactie = 'Incasare') t_int_card_t," _
                                    & "(SELECT avg(med_inc_lun) as med_inc_lun FROM (SELECT (sum(suma_card)+sum(suma_cash)) as med_inc_lun FROM incasari WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_incasare = 'RZF' GROUP BY MONTH(DATA)) med_inc_lun) med_inc_lun," _
                                    & "(SELECT sum(suma_cash) as di_l FROM incasari WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_incasare = 'DI') t_di_luna," _
                                    & "(SELECT sum(suma_cash) as di_a FROM incasari WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_incasare = 'DI') t_di_an," _
                                    & "(SELECT sum(suma_cash) as di_t FROM incasari WHERE tip_incasare = 'DI') t_di_t," _
                                    & "(SELECT sum(suma) as che_cash_l FROM cheltuieli WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala <> 'DP' AND cash=TRUE) t_che_cash_luna," _
                                    & "(SELECT sum(suma) as che_cont_l FROM cheltuieli WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala <> 'DP' AND cash=FALSE) t_che_cont_luna," _
                                    & "(SELECT sum(suma) as che_cash_a FROM cheltuieli WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala <> 'DP' AND cash=TRUE) t_che_cash_an," _
                                    & "(SELECT sum(suma) as che_cont_a FROM cheltuieli WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala <> 'DP' AND cash=FALSE) t_che_cont_an," _
                                    & "(SELECT sum(suma) as che_cash_t FROM cheltuieli WHERE tip_cheltuiala <> 'DP' AND cash=TRUE) t_che_cash_t," _
                                    & "(SELECT sum(suma) as che_cont_t FROM cheltuieli WHERE tip_cheltuiala <> 'DP' AND cash=FALSE) t_che_cont_t," _
                                    & "(SELECT sum(suma) as dp_l FROM cheltuieli WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala = 'DP') t_dp_luna," _
                                    & "(SELECT sum(suma) as dp_a FROM cheltuieli WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala = 'DP') t_dp_an," _
                                    & "(SELECT sum(suma) as dp_t FROM cheltuieli WHERE tip_cheltuiala = 'DP') t_dp_t"

            dbcomm = New MySqlCommand(tot_sql, dbconn)
            Try
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                sda.SelectCommand = dbcomm
                sda.Fill(dbtable)
                bsource.DataSource = dbtable
                sda.Update(dbtable)
                '-------------------------------Vanzari
                '----------------- V - CASH 
                Dim VcashL As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("v_cash_l")) Then
                    VcashL = 0
                Else : VcashL = CDec(dbtable.Rows(0).Item("v_cash_l"))
                End If
                vLcsh_TB.Text = FormatNumber(CDec(VcashL), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim VcashA As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("v_cash_a")) Then
                    VcashA = 0
                Else : VcashA = CDec(dbtable.Rows(0).Item("v_cash_a"))
                End If
                vAcsh_TB.Text = FormatNumber(CDec(VcashA), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim VcashT As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("v_cash_t")) Then
                    VcashT = 0
                Else : VcashT = CDec(dbtable.Rows(0).Item("v_cash_t"))
                End If
                vTcsh_TB.Text = FormatNumber(CDec(VcashT), 2, TriState.True, TriState.False, TriState.True) & " lei"


                '----------------- V -  CARD

                Dim VcardL As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("v_card_l")) Then
                    VcardL = 0
                Else : VcardL = CDec(dbtable.Rows(0).Item("v_card_l"))
                End If
                vLcrd_TB.Text = FormatNumber(CDec(VcardL), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim VcardA As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("v_card_a")) Then
                    VcardA = 0
                Else : VcardA = CDec(dbtable.Rows(0).Item("v_card_a"))
                End If
                vAcrd_TB.Text = FormatNumber(CDec(VcardA), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim VcardT As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("v_card_t")) Then
                    VcardT = 0
                Else : VcardT = CDec(dbtable.Rows(0).Item("v_card_t"))
                End If
                vTcrd_TB.Text = FormatNumber(CDec(VcardT), 2, TriState.True, TriState.False, TriState.True) & " lei"


                '----------------- V - TOTAL

                Dim tot_vL As Decimal = 0
                'If IsDBNull(dbtable.Rows(0).Item("inc_tot_l")) Then
                '    tot_vanzari = 0
                'Else : tot_vanzari = CDec(dbtable.Rows(0).Item("inc_tot_l"))
                'End If
                tot_vL = VcashL + VcardL
                vLttl_TB.Text = FormatNumber(CDec(tot_vL), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim tot_vA As Decimal = 0
                'If IsDBNull(dbtable.Rows(0).Item("inc_tot_a")) Then
                '    total_tot_vanzari = 0
                'Else : total_tot_vanzari = CDec(dbtable.Rows(0).Item("inc_tot_a"))
                'End If
                tot_vA = VcashA + VcardA
                vAttl_TB.Text = FormatNumber(CDec(tot_vA), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim tot_vT As Decimal = 0
                'If IsDBNull(dbtable.Rows(0).Item("inc_tot_t")) Then
                '    total_tot_ani_vanzari = 0
                'Else : total_tot_ani_vanzari = CDec(dbtable.Rows(0).Item("inc_tot_t"))
                'End If
                tot_vT = VcashT + VcardT
                vTttl_TB.Text = FormatNumber(CDec(tot_vT), 2, TriState.True, TriState.False, TriState.True) & " lei"




                Dim media_luna As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("med_inc_lun")) Then
                    media_luna = 0
                Else : media_luna = CDec(dbtable.Rows(0).Item("med_inc_lun"))
                End If
                ' media_luna = total_tot_vanzari / ComboBox1.SelectedItem
                TextBox33.Text = FormatNumber(CDec(media_luna), 2, TriState.True, TriState.False, TriState.True) & " lei"
                If media_luna < 17000 Then
                    TextBox33.ForeColor = Color.Green
                ElseIf 17000 < media_luna < 18000 Then
                    TextBox33.ForeColor = Color.Orange
                ElseIf media_luna > 18000 Then
                    TextBox33.ForeColor = Color.Red

                End If
                '----------------------- INTRARI
                '---------------- I - CASH 
                Dim IcashL As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("int_cash_l")) Then
                    IcashL = 0
                Else : IcashL = CDec(dbtable.Rows(0).Item("int_cash_l"))
                End If
                iLcsh_TB.Text = FormatNumber(CDec(IcashL), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim IcashA As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("int_cash_a")) Then
                    IcashA = 0
                Else : IcashA = CDec(dbtable.Rows(0).Item("int_cash_a"))
                End If
                iAcsh_TB.Text = FormatNumber(CDec(IcashA), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim IcashT As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("int_cash_t")) Then
                    IcashT = 0
                Else : IcashT = CDec(dbtable.Rows(0).Item("int_cash_t"))
                End If
                iTcsh_TB.Text = FormatNumber(CDec(IcashT), 2, TriState.True, TriState.False, TriState.True) & " lei"


                '---------------- I -  CARD

                Dim IcardL As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("int_card_l")) Then
                    IcardL = 0
                Else : IcardL = CDec(dbtable.Rows(0).Item("int_card_l"))
                End If
                iLcrd_TB.Text = FormatNumber(CDec(IcardL), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim IcardA As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("int_card_a")) Then
                    IcardA = 0
                Else : IcardA = CDec(dbtable.Rows(0).Item("int_card_a"))
                End If
                iAcrd_TB.Text = FormatNumber(CDec(IcardA), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim IcardT As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("int_card_t")) Then
                    IcardT = 0
                Else : IcardT = CDec(dbtable.Rows(0).Item("int_card_t"))
                End If
                iTcrd_TB.Text = FormatNumber(CDec(IcardT), 2, TriState.True, TriState.False, TriState.True) & " lei"


                '-----------------  TOTAL INTRARI

                Dim tot_iL As Decimal = 0
                'If IsDBNull(dbtable.Rows(0).Item("inc_tot_l")) Then
                '    tot_vanzari = 0
                'Else : tot_vanzari = CDec(dbtable.Rows(0).Item("inc_tot_l"))
                'End If
                tot_iL = IcashL + IcardL
                iLttl_TB.Text = FormatNumber(CDec(tot_iL), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim tot_iA As Decimal = 0
                'If IsDBNull(dbtable.Rows(0).Item("inc_tot_a")) Then
                '    total_tot_vanzari = 0
                'Else : total_tot_vanzari = CDec(dbtable.Rows(0).Item("inc_tot_a"))
                'End If
                tot_iA = IcashA + IcardA
                iAttl_TB.Text = FormatNumber(CDec(tot_iA), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim tot_iT As Decimal = 0
                'If IsDBNull(dbtable.Rows(0).Item("inc_tot_t")) Then
                '    total_tot_ani_vanzari = 0
                'Else : total_tot_ani_vanzari = CDec(dbtable.Rows(0).Item("inc_tot_t"))
                'End If
                tot_iT = IcashT + IcardT
                iTttl_TB.Text = FormatNumber(CDec(tot_iT), 2, TriState.True, TriState.False, TriState.True) & " lei"





                '----------------- CHELTUIELI

                '----------------- CHELTUIELI CASH $
                Dim cheltuieli_cash As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("che_cash_l")) Then
                    cheltuieli_cash = 0
                Else : cheltuieli_cash = CDec(dbtable.Rows(0).Item("che_cash_l"))
                End If
                cLcsh_TB.Text = FormatNumber(CDec(cheltuieli_cash), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim total_cheltuieli_cash As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("che_cash_a")) Then
                    total_cheltuieli_cash = 0
                Else : total_cheltuieli_cash = CDec(dbtable.Rows(0).Item("che_cash_a"))
                End If
                cAcsh_TB.Text = FormatNumber(CDec(total_cheltuieli_cash), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim total_ani_cheltuieli_cash As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("che_cash_t")) Then
                    total_ani_cheltuieli_cash = 0
                Else : total_ani_cheltuieli_cash = CDec(dbtable.Rows(0).Item("che_cash_t"))
                End If
                cTcsh_TB.Text = FormatNumber(CDec(total_ani_cheltuieli_cash), 2, TriState.True, TriState.False, TriState.True) & " lei"

                '----------------- CHELTUIELI CONT
                Dim cheltuieli_cont As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("che_cont_l")) Then
                    cheltuieli_cont = 0
                Else : cheltuieli_cont = CDec(dbtable.Rows(0).Item("che_cont_l"))
                End If
                cLcrd_TB.Text = FormatNumber(CDec(cheltuieli_cont), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim total_cheltuieli_cont As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("che_cont_a")) Then
                    total_cheltuieli_cont = 0
                Else : total_cheltuieli_cont = CDec(dbtable.Rows(0).Item("che_cont_a"))
                End If
                cAcrd_TB.Text = FormatNumber(CDec(total_cheltuieli_cont), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim total_ani_cheltuieli_cont As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("che_cont_t")) Then
                    total_ani_cheltuieli_cont = 0
                Else : total_ani_cheltuieli_cont = CDec(dbtable.Rows(0).Item("che_cont_t"))
                End If
                cTcrd_TB.Text = FormatNumber(CDec(total_ani_cheltuieli_cont), 2, TriState.True, TriState.False, TriState.True) & " lei"

                '----------------- TOTAL CHELTUIELI
                Dim cheltuieli As Decimal = 0
                cheltuieli = cheltuieli_cash + cheltuieli_cont
                cLttl_TB.Text = FormatNumber(CDec(cheltuieli), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim total_cheltuieli As Decimal = 0
                total_cheltuieli = total_cheltuieli_cash + total_cheltuieli_cont
                cAttl_TB.Text = FormatNumber(CDec(total_cheltuieli), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim total_ani_cheltuieli As Decimal = 0
                total_ani_cheltuieli = total_ani_cheltuieli_cash + total_ani_cheltuieli_cont
                cTttl_TB.Text = FormatNumber(CDec(total_ani_cheltuieli), 2, TriState.True, TriState.False, TriState.True) & " lei"


                '----------------- PROFIT -  DIFERENTA INTRARI - CHELTUIELI-------LUNA

                Dim dif_luna As Decimal = IcardL + IcashL - cheltuieli
                prfL_TB.Text = FormatNumber(CDec(dif_luna), 2, TriState.True, TriState.False, TriState.True) & " lei"
                If dif_luna < 0 Then
                    prfL_TB.ForeColor = Color.Red
                ElseIf dif_luna >= 0 Then
                    prfL_TB.ForeColor = Color.Green
                End If

                '----------------- PROFIT -  DIFERENTA INTRARI - CHELTUIELI-------AN

                Dim dif_an As Decimal = IcardA + IcashA - total_cheltuieli
                prfA_TB.Text = FormatNumber(CDec(dif_an), 2, TriState.True, TriState.False, TriState.True) & " lei"
                If dif_an < 0 Then
                    prfA_TB.ForeColor = Color.Red
                ElseIf dif_an >= 0 Then
                    prfA_TB.ForeColor = Color.Green
                End If

                '----------------- PROFIT -  DIFERENTA INTRARI - CHELTUIELI-------TOTAL

                Dim dif_ani_an As Decimal = IcardT + IcashT - total_ani_cheltuieli
                prfT_TB.Text = FormatNumber(CDec(dif_ani_an), 2, TriState.True, TriState.False, TriState.True) & " lei"
                If dif_ani_an < 0 Then
                    prfT_TB.ForeColor = Color.Red
                ElseIf dif_ani_an >= 0 Then
                    prfT_TB.ForeColor = Color.Green
                End If

                '----------------- DISPOZITII INCASARE

                Dim DI As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("di_l")) Then
                    DI = 0
                Else : DI = CDec(dbtable.Rows(0).Item("di_l"))
                End If
                TextBox9.Text = FormatNumber(CDec(DI), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim total_DI As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("di_a")) Then
                    total_DI = 0
                Else : total_DI = CDec(dbtable.Rows(0).Item("di_a"))
                End If
                TextBox10.Text = FormatNumber(CDec(total_DI), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim DI_T As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("di_t")) Then
                    DI_T = 0
                Else : DI_T = CDec(dbtable.Rows(0).Item("di_t"))
                End If
                TextBox19.Text = FormatNumber(CDec(DI_T), 2, TriState.True, TriState.False, TriState.True) & " lei"

                '----------------- DISPOZITII PLATA

                Dim DP As Decimal = 0

                If IsDBNull(dbtable.Rows(0).Item("dp_l")) Then
                    DP = 0
                Else : DP = CDec(dbtable.Rows(0).Item("dp_l"))
                End If
                TextBox11.Text = FormatNumber(CDec(DP), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim total_DP As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("dp_a")) Then
                    total_DP = 0
                Else : total_DP = CDec(dbtable.Rows(0).Item("dp_a"))
                End If
                TextBox12.Text = FormatNumber(CDec(total_DP), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Dim DP_T As Decimal = 0
                If IsDBNull(dbtable.Rows(0).Item("dp_t")) Then
                    DP_T = 0
                Else : DP_T = CDec(dbtable.Rows(0).Item("dp_t"))
                End If
                TextBox17.Text = FormatNumber(CDec(DP_T), 2, TriState.True, TriState.False, TriState.True) & " lei"

                '----------------- DIFERENTA DISPOZITII-------LUNA

                TextBox13.Text = FormatNumber(CDec(DP - DI), 2, TriState.True, TriState.False, TriState.True) & " lei"
                If DP - DI < 0 Then
                    TextBox13.ForeColor = Color.Red
                Else : TextBox13.ForeColor = Color.Green
                End If

                '----------------- DIFERENTA DISPOZITII-------AN

                TextBox14.Text = FormatNumber(CDec(total_DP - total_DI), 2, TriState.True, TriState.False, TriState.True) & " lei"
                If total_DP - total_DI < 0 Then
                    TextBox14.ForeColor = Color.Red
                Else : TextBox14.ForeColor = Color.Green
                End If

                '----------------- DIFERENTA DISPOZITII-------TOTAL

                TextBox15.Text = FormatNumber(CDec(DP_T - DI_T), 2, TriState.True, TriState.False, TriState.True) & " lei"
                If DP_T - DI_T < 0 Then
                    TextBox15.ForeColor = Color.Red
                Else : TextBox15.ForeColor = Color.Green
                End If
            Catch ex As Exception
                MsgBox("Problem loading sume incasari !!!: " & ex.Message.ToString)
            End Try
        End Using

        '----------------- SOLD GESTIUNE


        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String = "SELECT (intr-ies) AS sold FROM " _
                                    & "(SELECT SUM(suma) AS intr FROM intrari) t_intrari," _
                                   & "(SELECT SUM(suma) AS ies FROM iesiri) t_iesiri"
            dbcomm = New MySqlCommand(tot_sql, dbconn)
            Dim sold As Decimal = 0
            Try
                dbconn.Open()
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If IsDBNull(dbread("sold")) Then
                    sold = 0
                Else : sold = CDec(dbread("sold"))
                End If
                TextBox18.Text = FormatNumber(CDec(sold), 2, TriState.True, TriState.False, TriState.True) & " lei"

            Catch ex As Exception
                MsgBox("Problem loading sold gestiune: " & ex.Message.ToString)
            End Try
        End Using

        '----------------- SOLD BANCA


        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String = "SELECT (incasari-plati-comisioane) AS sold FROM " _
                                    & "(SELECT SUM(suma) AS incasari FROM banca_tranzactii WHERE tip_tranzactie='Incasare') t_incasari," _
                                    & "(SELECT SUM(suma) AS plati FROM banca_tranzactii WHERE tip_tranzactie='Plata') t_plati," _
                                    & "(SELECT SUM(suma) AS comisioane FROM banca_comisioane) t_comisioane"

            dbcomm = New MySqlCommand(tot_sql, dbconn)
            Dim sold As Decimal = 0
            Try
                dbconn.Open()
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If IsDBNull(dbread("sold")) Then
                    sold = 0
                Else : sold = CDec(dbread("sold"))
                End If
                TextBox16.Text = FormatNumber(CDec(sold), 2, TriState.True, TriState.False, TriState.True) & " lei"

            Catch ex As Exception
                MsgBox("Problem loading sold gestiune: " & ex.Message.ToString)
            End Try
        End Using


    End Sub



    Public Sub Load_Registru_Listview()

        Dim mag As String = ComboBox3.SelectedValue
        Dim mag_id As String = "PM"
        If mag = "PM" Then
            mag_id = "PM"
        ElseIf mag = "MV" Then
            mag_id = "MV"
        End If


        Registru_Listview.Clear()

        Registru_Listview.Columns.Add("Document", CInt((Registru_Listview.Width - SystemInformation.VerticalScrollBarWidth) * 45 / 100))
        Registru_Listview.Columns.Add("Data", CInt((Registru_Listview.Width - SystemInformation.VerticalScrollBarWidth) * 28 / 100))
        Registru_Listview.Columns.Add("Magazin", CInt((Registru_Listview.Width - SystemInformation.VerticalScrollBarWidth) * 25 / 100))
        Registru_Listview.Columns.Add("Path", 0)

        Registru_Listview.View = View.Details
        Dim files_tab As New DataTable
        files_tab.Columns.Add("file")
        files_tab.Columns.Add("data")
        files_tab.Columns.Add("magazin")
        files_tab.Columns.Add("an")
        files_tab.Columns.Add("luna")

        Dim row As Integer = 0
        For Each file As String In System.IO.Directory.GetFiles(folder_registru)

            If file.Length = folder_registru.Length + CStr("Registru Casa_yyyy_MM_" & mag_id & ".pdf").Length Then

                If file.Substring(file.Length - CStr("Registru Casa_yyyy_MM_" & mag_id & ".pdf").Length, CStr("Registru Casa_").Length) = "Registru Casa_" Then
                    files_tab.Rows.Add()
                    files_tab.Rows(row).Item("file") = file
                    Dim edateValue As Date
                    Dim dat As String = Date.TryParseExact(file.Substring((file.Length - 14), 7), "yyyy_MM", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, edateValue)
                    files_tab.Rows(row).Item("data") = Format(edateValue, "dd.MM.yyyy")
                    files_tab.Rows(row).Item("magazin") = file.Substring(file.Length - 6, 2)
                    files_tab.Rows(row).Item("an") = Format(edateValue, "yyyy")
                    files_tab.Rows(row).Item("luna") = Format(edateValue, "MM")
                    row = row + 1
                End If
            End If
        Next
        files_tab.DefaultView.Sort = "an DESC, luna DESC, magazin DESC"
        files_tab = files_tab.DefaultView.ToTable

        Dim n As Integer = -1
        For i = 0 To files_tab.Rows.Count - 1

            ImageList1.Images.Add(Icon.ExtractAssociatedIcon(files_tab.Rows(i).Item("file")))
            n = n + 1
            Registru_Listview.Items.Add("Registru Casa", ImageList1.Images.Count - 1)

            Registru_Listview.Items(n).SubItems.Add(Format(CDate(files_tab.Rows(i).Item("data")), "MM-yyyy"))
            Registru_Listview.Items(n).SubItems.Add(files_tab.Rows(i).Item("magazin"))
            Registru_Listview.Items(n).SubItems.Add(files_tab.Rows(i).Item("file"))
        Next
        For j = 0 To Registru_Listview.Items.Count - 1
            If Registru_Listview.Items(j).SubItems(2).Text = "MV" Then
                Registru_Listview.Items(j).BackColor = Color.LightGray
            End If
        Next

    End Sub
    Public Sub Load_Raport_Listview()

        Dim mag As String = ComboBox3.SelectedValue
        Dim mag_id As String = "PM"
        If mag = "PM" Then
            mag_id = "PM"
        ElseIf mag = "MV" Then
            mag_id = "MV"
        End If

        Raport_Listview.Clear()

        Raport_Listview.Columns.Add("Document", CInt((Raport_Listview.Width - SystemInformation.VerticalScrollBarWidth) * 45 / 100))
        Raport_Listview.Columns.Add("Data", CInt((Raport_Listview.Width - SystemInformation.VerticalScrollBarWidth) * 28 / 100))
        Raport_Listview.Columns.Add("Magazin", CInt((Raport_Listview.Width - SystemInformation.VerticalScrollBarWidth) * 25 / 100))
        Raport_Listview.Columns.Add("Path", 0)

        Dim files_tab As New DataTable
        files_tab.Columns.Add("file")
        files_tab.Columns.Add("data")
        files_tab.Columns.Add("magazin")
        files_tab.Columns.Add("an")
        files_tab.Columns.Add("luna")

        Dim row As Integer = 0
        For Each file As String In System.IO.Directory.GetFiles(folder_raport)

            If file.Length = folder_raport.Length + CStr("Raport Gestiune_yyyy_MM_" & mag_id & ".pdf").Length Then
                If file.Substring(file.Length - CStr("Raport Gestiune_yyyy_MM_" & mag_id & ".pdf").Length, CStr("Raport Gestiune_").Length) = "Raport Gestiune_" Then
                    files_tab.Rows.Add()
                    files_tab.Rows(row).Item("file") = file
                    Dim edateValue As Date
                    Dim dat As String = Date.TryParseExact(file.Substring((file.Length - 14), 7), "yyyy_MM", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, edateValue)
                    files_tab.Rows(row).Item("data") = Format(edateValue, "dd.MM.yyyy")
                    files_tab.Rows(row).Item("magazin") = file.Substring(file.Length - 6, 2)
                    files_tab.Rows(row).Item("an") = Format(edateValue, "yyyy")
                    files_tab.Rows(row).Item("luna") = Format(edateValue, "MM")
                    row = row + 1
                End If

            End If
        Next
        files_tab.DefaultView.Sort = "an DESC, luna DESC, magazin DESC"
        files_tab = files_tab.DefaultView.ToTable

        Dim n As Integer = -1
        For i = 0 To files_tab.Rows.Count - 1

            ImageList1.Images.Add(Icon.ExtractAssociatedIcon(files_tab.Rows(i).Item("file")))
            n = n + 1
            Raport_Listview.Items.Add("Raport Gestiune", ImageList1.Images.Count - 1)

            Raport_Listview.Items(n).SubItems.Add(Format(CDate(files_tab.Rows(i).Item("data")), "MM-yyyy"))
            Raport_Listview.Items(n).SubItems.Add(files_tab.Rows(i).Item("magazin"))
            Raport_Listview.Items(n).SubItems.Add(files_tab.Rows(i).Item("file"))
        Next
        For j = 0 To Raport_Listview.Items.Count - 1
            If Raport_Listview.Items(j).SubItems(2).Text = "MV" Then
                Raport_Listview.Items(j).BackColor = Color.LightGray
            End If
        Next

    End Sub
    Public Sub Load_DI_Listview()

        Dim mag As String = ComboBox3.SelectedValue
        Dim mag_id As String = "PM"
        If mag = "PM" Then
            mag_id = "PM"
        ElseIf mag = "MV" Then
            mag_id = "MV"
        End If


        DI_Listview.Clear()

        DI_Listview.Columns.Add("Document", CInt(DI_Listview.Width * 50 / 100))
        DI_Listview.Columns.Add("Data", CInt(DI_Listview.Width * 40 / 100))
        DI_Listview.Columns.Add("Path", 0)

        DI_Listview.View = View.Details

        Dim files_tab As New DataTable
        files_tab.Columns.Add("file")
        files_tab.Columns.Add("di")
        files_tab.Columns.Add("data")
        files_tab.Columns.Add("ext")

        Dim dp As Integer = -1
        Dim row As Integer = 0
        For Each file As String In System.IO.Directory.GetFiles(folder_dp)
            If file.Length = folder_dp.Length + CStr("Dispozitie Incasare_000_yyyyMMdd").Length + System.IO.Path.GetExtension(file).Length Then
                If file.Substring(file.Length - CStr("Dispozitie Incasare_000_yyyyMMdd" & System.IO.Path.GetExtension(file)).Length, CStr("Dispozitie Incasare_").Length) = "Dispozitie Incasare_" Then

                    files_tab.Rows.Add()

                    files_tab.Rows(row).Item("file") = file
                    files_tab.Rows(row).Item("di") = file.Substring(folder_di.Length + 20, 3)
                    files_tab.Rows(row).Item("ext") = System.IO.Path.GetExtension(file)
                    Dim edateValue As Date
                    Dim dat As String = Date.TryParseExact(file.Substring((file.Length - (System.IO.Path.GetExtension(file).Length + 8)), 8), "yyyyMMdd", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, edateValue)
                    files_tab.Rows(row).Item("data") = Format(edateValue, "dd.MM.yyyy")
                    row = row + 1
                End If
            End If
        Next
        files_tab.DefaultView.Sort = "di DESC"
        files_tab = files_tab.DefaultView.ToTable

        Dim n As Integer = -1
        For i = 0 To files_tab.Rows.Count - 1

            ImageList1.Images.Add(Icon.ExtractAssociatedIcon(files_tab.Rows(i).Item("file")))

            n = n + 1
            DI_Listview.Items.Add("DI - " & files_tab.Rows(i).Item("di"), ImageList1.Images.Count - 1)

            DI_Listview.Items(n).SubItems.Add(files_tab.Rows(i).Item("data"))
            DI_Listview.Items(n).SubItems.Add(files_tab.Rows(i).Item("file"))
        Next
    End Sub
    Public Sub Load_DP_Listview()


        Dim mag As String = ComboBox3.SelectedValue
        Dim mag_id As String = "PM"
        If mag = "PM" Then
            mag_id = "PM"
        ElseIf mag = "MV" Then
            mag_id = "MV"
        End If


        DP_Listview.Clear()

        DP_Listview.Columns.Add("Document", CInt(DP_Listview.Width * 50 / 100))
        DP_Listview.Columns.Add("Data", CInt(DP_Listview.Width * 40 / 100))
        DP_Listview.Columns.Add("Path", 0)

        DP_Listview.View = View.Details

        Dim files_tab As New DataTable
        files_tab.Columns.Add("file")
        files_tab.Columns.Add("dp")
        files_tab.Columns.Add("data")
        files_tab.Columns.Add("ext")

        Dim dp As Integer = -1
        Dim row As Integer = 0
        For Each file As String In System.IO.Directory.GetFiles(folder_dp)
            If file.Length = folder_dp.Length + CStr("Dispozitie Plata_000_yyyyMMdd").Length + System.IO.Path.GetExtension(file).Length Then
                If file.Substring(file.Length - CStr("Dispozitie Plata_000_yyyyMMdd" & System.IO.Path.GetExtension(file)).Length, CStr("Dispozitie Plata_").Length) = "Dispozitie Plata_" Then

                    files_tab.Rows.Add()

                    files_tab.Rows(row).Item("file") = file
                    files_tab.Rows(row).Item("dp") = file.Substring(folder_dp.Length + 17, 3)
                    files_tab.Rows(row).Item("ext") = System.IO.Path.GetExtension(file)
                    Dim edateValue As Date
                    Dim dat As String = Date.TryParseExact(file.Substring((file.Length - (System.IO.Path.GetExtension(file).Length + 8)), 8), "yyyyMMdd", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, edateValue)
                    files_tab.Rows(row).Item("data") = Format(edateValue, "dd.MM.yyyy")
                    row = row + 1
                End If
            End If
        Next
        files_tab.DefaultView.Sort = "dp DESC"
        files_tab = files_tab.DefaultView.ToTable

        Dim n As Integer = -1
        For i = 0 To files_tab.Rows.Count - 1

            ImageList1.Images.Add(Icon.ExtractAssociatedIcon(files_tab.Rows(i).Item("file")))

            n = n + 1
            DP_Listview.Items.Add("DP - " & files_tab.Rows(i).Item("dp"), ImageList1.Images.Count - 1)

            DP_Listview.Items(n).SubItems.Add(files_tab.Rows(i).Item("data"))
            DP_Listview.Items(n).SubItems.Add(files_tab.Rows(i).Item("file"))
        Next
    End Sub
    Public Sub Load_Nir_Listview()

        Dim mag As String = ComboBox3.SelectedValue
        Dim mag_id As String = "PM"
        If mag = "PM" Then
            mag_id = "PM"
        ElseIf mag = "MV" Then
            mag_id = "MV"
        End If

        Nir_Listview.Items.Clear()
        Nir_Listview.Clear()

        Nir_Listview.Columns.Add("Document", CInt(Nir_Listview.Width * 35 / 100))
        Nir_Listview.Columns.Add("Data", CInt(Nir_Listview.Width * 30 / 100))
        Nir_Listview.Columns.Add("Magazin", CInt(Nir_Listview.Width * 28 / 100))
        Nir_Listview.Columns.Add("Path", 0)
        Nir_Listview.View = View.Details

        Dim files_tab As New DataTable
        files_tab.Columns.Add("file")
        files_tab.Columns.Add("nir")
        files_tab.Columns.Add("data")
        files_tab.Columns.Add("magazin")

        Dim row As Integer = 0
        For Each fisier As String In System.IO.Directory.GetFiles(folder_nir)
            If fisier.Length = folder_nir.Length + CStr("NIR_000_yyyyMMdd_" & mag_id & ".pdf").Length Then
                If fisier.Substring(fisier.Length - CStr("NIR_000_yyyyMMdd_" & mag_id & ".pdf").Length, CStr("NIR_").Length) = "NIR_" Then
                    files_tab.Rows.Add()
                    files_tab.Rows(row).Item("file") = fisier
                    files_tab.Rows(row).Item("nir") = fisier.Substring(folder_nir.Length + 4, 3)
                    Dim edateValue As Date
                    Dim dat As String = Date.TryParseExact(fisier.Substring((fisier.Length - 15), 8), "yyyyMMdd", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, edateValue)
                    files_tab.Rows(row).Item("data") = Format(edateValue, "dd.MM.yyyy")
                    files_tab.Rows(row).Item("magazin") = fisier.Substring(folder_nir.Length + 17, 2)
                    row = row + 1
                End If
            End If
        Next
        files_tab.DefaultView.Sort = "nir DESC"
        files_tab = files_tab.DefaultView.ToTable

        Dim n As Integer = -1
        For i = 0 To files_tab.Rows.Count - 1

            ImageList1.Images.Add(Icon.ExtractAssociatedIcon(files_tab.Rows(i).Item("file")))

            n = n + 1
            Nir_Listview.Items.Add("NIR " & files_tab.Rows(i).Item("nir"), ImageList1.Images.Count - 1)

            Nir_Listview.Items(n).SubItems.Add(files_tab.Rows(i).Item("data"))
            Nir_Listview.Items(n).SubItems.Add(files_tab.Rows(i).Item("magazin"))
            Nir_Listview.Items(n).SubItems.Add(files_tab.Rows(i).Item("file"))
        Next

        For k = 0 To Nir_Listview.Items.Count - 1
            If Nir_Listview.Items(k).SubItems(2).Text = "MV" Then
                Nir_Listview.Items(k).BackColor = Color.LightGray
            End If
        Next

    End Sub
    Public Sub Load_Facturi_Listview()

        Dim mag As String = ComboBox3.SelectedValue
        Dim mag_id As String = "PM"
        If mag = "PM" Then
            mag_id = "PM"
        ElseIf mag = "MV" Then
            mag_id = "MV"
        End If


        Facturi_ListView.Clear()

        Facturi_ListView.Columns.Add("Document", CInt(Facturi_ListView.Width * 50 / 100))
        Facturi_ListView.Columns.Add("Data", CInt(Facturi_ListView.Width * 40 / 100))
        Facturi_ListView.Columns.Add("Path", 0)

        Facturi_ListView.View = View.Details

        Dim files_tab As New DataTable
        files_tab.Columns.Add("file")
        files_tab.Columns.Add("numar")
        files_tab.Columns.Add("data")
        files_tab.Columns.Add("ext")

        Dim dp As Integer = -1
        Dim row As Integer = 0
        'MsgBox(folder_facturi)
        For Each file As String In System.IO.Directory.GetFiles(folder_facturi)

            If file.Length = folder_facturi.Length + CStr("Factura_000_yyyyMMdd").Length + System.IO.Path.GetExtension(file).Length Then
                If file.Substring(file.Length - CStr("Factura_000_yyyyMMdd" & System.IO.Path.GetExtension(file)).Length, CStr("Factura_").Length) = "Factura_" Then

                    files_tab.Rows.Add()

                    files_tab.Rows(row).Item("file") = file
                    files_tab.Rows(row).Item("numar") = file.Substring(folder_facturi.Length + 8, 3)
                    files_tab.Rows(row).Item("ext") = System.IO.Path.GetExtension(file)
                    Dim edateValue As Date
                    Dim dat As String = Date.TryParseExact(file.Substring((file.Length - (System.IO.Path.GetExtension(file).Length + 8)), 8), "yyyyMMdd", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, edateValue)
                    'MsgBox(file.Substring((file.Length - (System.IO.Path.GetExtension(file).Length + 8)), 8))
                    files_tab.Rows(row).Item("data") = Format(edateValue, "dd.MM.yyyy")
                    row = row + 1
                End If
            End If
        Next
        files_tab.DefaultView.Sort = "numar DESC"
        files_tab = files_tab.DefaultView.ToTable

        Dim n As Integer = -1
        For i = 0 To files_tab.Rows.Count - 1

            ImageList1.Images.Add(Icon.ExtractAssociatedIcon(files_tab.Rows(i).Item("file")))

            n = n + 1
            Facturi_ListView.Items.Add("Factura - " & files_tab.Rows(i).Item("numar"), ImageList1.Images.Count - 1)

            Facturi_ListView.Items(n).SubItems.Add(files_tab.Rows(i).Item("data"))
            Facturi_ListView.Items(n).SubItems.Add(files_tab.Rows(i).Item("file"))
        Next
    End Sub
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged

        If CheckBox1.CheckState = CheckState.Checked Then

            CheckBox1.Text = "Tot"
            ComboBox1.Enabled = False
            ComboBox2.Enabled = False
            ComboBox3.Enabled = False
            incas_tot_luna_label.Text = ""
        ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
            CheckBox1.Text = "Tot"
            ComboBox1.Enabled = True
            ComboBox2.Enabled = True
            ComboBox3.Enabled = True

        End If
        Load_Incasari()
        Load_Cheltuieli()
        Load_Intrari()
        Load_Iesiri()

    End Sub
    Private Sub ComboBox_DropDownClosed(sender As Object, e As EventArgs) Handles ComboBox1.DropDownClosed, ComboBox2.DropDownClosed, ComboBox3.DropDownClosed

        Dim mag As String = ComboBox3.SelectedValue
        Dim mag_id As String = ""
        If mag = "PM" Then
            mag_id = "PM"
        ElseIf mag = "MV" Then
            mag_id = "MV"
        End If
        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
        End If
        Load_Incasari()
        Load_Cheltuieli()
        Load_Intrari()
        Load_Iesiri()
        Load_Situatie()
        Load_Solduri()
        'Load_Listview_Documente()

    End Sub
    Private Sub DGV_rightclick_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles Incasari_DGV.CellMouseDown, Cheltuieli_DGV.CellMouseDown, Intrari_DGV.CellMouseDown, Iesiri_DGV.CellMouseDown, Sold_Casa_DGV.CellMouseDown, Sold_Gestiune_DGV.CellMouseDown, Firme_DGV.CellMouseDown, Marfa_DGV.CellMouseDown
        If e.Button = Windows.Forms.MouseButtons.Right And e.ColumnIndex > -1 And e.RowIndex > -1 Then
            Dim cell As DataGridViewCell = sender.Rows(e.RowIndex).Cells(e.ColumnIndex)
            sender.CurrentCell = cell
            sender.focus()
        End If
    End Sub

    Private Sub SetariToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SetariToolStripMenuItem.Click
        Form_setari.Show()
    End Sub

    Private Sub Incasari_BU_Click(sender As Object, e As EventArgs) Handles Incasari_BU.Click
        Form_Incasari.Show()
        Me.Hide()
    End Sub
    Private Sub Strip_Incasari_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Strip_Incasari.Opening
        Dim DGV As DataGridView = Incasari_DGV
        Dim row As DataGridViewRow = DGV.CurrentRow
        If DGV.Rows.Count > 0 Then
            If DGV.CurrentRow.Index > -1 AndAlso DGV.SelectedRows.Count <> 0 Then
                Dim tip_incasare As String = row.Cells("tip_incasare").Value.ToString()

                If tip_incasare = "DI" Then
                    PrinteazaDIToolStripMenuItem.Enabled = True
                Else : PrinteazaDIToolStripMenuItem.Enabled = False
                End If
            End If
        ElseIf DGV.Rows.Count < 1 Then
            e.Cancel = True
        End If
    End Sub
    Private Sub StergeIncasareaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StergeIncasareaToolstripMenuItem.Click
        Dim DGV As DataGridView = Incasari_DGV
        Dim yesno As Integer = MsgBox("Stergi Inregistrarea?", MsgBoxStyle.YesNo)

        Dim row As DataGridViewRow = DGV.CurrentRow
        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then
            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            For i = 0 To DGV.SelectedRows.Count - 1
                Dim id As Integer = DGV.SelectedRows(i).Cells("id").Value
                Dim data As Date = CDate(DGV.SelectedRows(i).Cells("data").Value)
                Dim nr_rzf As String = DGV.SelectedRows(i).Cells("nr_rzf").Value.ToString
                Dim tip_incasare As String = DGV.SelectedRows(i).Cells("tip_incasare").Value.ToString
                Dim explicatii As String = DGV.SelectedRows(i).Cells("explicatii").Value

                Try
                    Dim sql_del_inc As String = "DELETE FROM incasari WHERE id=@id AND data=@data"
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
                    MsgBox("Nu s-a sters din iesiri: " & ex.Message.ToString)
                End Try
            Next
            transaction.Commit()

            Load_Incasari()
            Load_Cheltuieli()
        End If

    End Sub
    Private Sub PrinteazaDIToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrinteazaDIToolStripMenuItem.Click
        Dim DGV As DataGridView = Incasari_DGV
        Dim row As DataGridViewRow = DGV.CurrentRow
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

        graph.DrawString("Unitatea: MILICOM CAZ SRL" & maga, text_font, XBrushes.Black,
                         New XRect(45, 20, 100, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawLine(pen, 40, 35, 555, 35)

        graph.DrawString("DISPOZITIE DE INCASARE CATRE CASIERIE", text_font, XBrushes.Black,
                         New XRect(45, 37, 300, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 40, 52, 555, 52)

        graph.DrawLine(pen, 40, 35, 40, 232)
        graph.DrawLine(pen, 555, 35, 555, 232)


        graph.DrawString("Nr. " & nr_rzf & " din " & data, text_font, XBrushes.Black,
                         New XRect(45, 67, 300, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Numele si prenumele:", text_font, XBrushes.Black,
                         New XRect(45, 97, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("CAZAN MIHAI", text_font, XBrushes.Black,
                         New XRect(195, 97, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Functia (calitatea):", text_font, XBrushes.Black,
                         New XRect(45, 112, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("ADMINISTRATOR", text_font, XBrushes.Black,
                         New XRect(195, 112, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Suma:", text_font, XBrushes.Black,
                         New XRect(45, 127, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(suma & " lei (" & NrCuv(suma) & " lei)", text_font, XBrushes.Black,
                         New XRect(195, 127, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Scopul incasarii:", text_font, XBrushes.Black,
                         New XRect(45, 142, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("CREDITARE SOCIETATE", text_font, XBrushes.Black,
                         New XRect(195, 142, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawRectangle(pen, New XRect(40, 172, 75, 60))
        graph.DrawString("Semnatura", mic_font, XBrushes.Black,
                         New XRect(40, 172, 75, 80), XStringFormats.TopCenter)

        graph.DrawRectangle(pen, New XRect(115, 172, 146, 20))
        graph.DrawString("Conducatorul unitatii", mic_font, XBrushes.Black,
                         New XRect(117, 172, 146, 20), XStringFormats.Center)
        graph.DrawRectangle(pen, New XRect(115, 192, 146, 40))

        graph.DrawRectangle(pen, New XRect(261, 172, 146, 20))
        graph.DrawString("Viza de control financiar-preventiv", mic_font, XBrushes.Black,
                         New XRect(261, 172, 146, 20), XStringFormats.Center)
        graph.DrawRectangle(pen, New XRect(261, 192, 146, 40))

        graph.DrawRectangle(pen, New XRect(407, 172, 148, 20))
        graph.DrawString("Compartiment financiar-contabil", mic_font, XBrushes.Black,
                         New XRect(409, 172, 146, 20), XStringFormats.Center)
        graph.DrawRectangle(pen, New XRect(407, 192, 148, 40))


        graph.DrawLine(pen, 40, 247, 555, 247)


        graph.DrawString("CASIER,", text_font, XBrushes.Black,
                         New XRect(45, 249, 300, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("Incasat suma de", text_font, XBrushes.Black,
                        New XRect(45, 264, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(suma & " lei", text_font, XBrushes.Black,
                         New XRect(195, 264, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Data:", text_font, XBrushes.Black,
                         New XRect(45, 279, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(Format(CDate(row.Cells("data").Value), "dd MMMM yyyy"), text_font, XBrushes.Black,
                         New XRect(195, 279, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("Semnatura:", text_font, XBrushes.Black,
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
    End Sub
    Private Sub ModificaIncasareaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModificaIncasareaToolStripMenuItem.Click
        Dim DGV As DataGridView = Incasari_DGV
        Dim mag_act As String = ""

        Dim row As DataGridViewRow = DGV.CurrentRow


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

        Form_Incasari.Show()
        With Form_Incasari


            .edit_Lbl.Text = "Edit"
            .id_Lbl.Text = id
            .mag_v_Lbl.Text = mag_act
            .rzf_v_Lbl.Text = row.Cells("nr_rzf").Value.ToString
            .data_ies_lbl.Text = row.Cells("data").Value.ToString
            .GroupBox1.Visible = True

            .DateTimePicker1.Value = data
            .ComboBox1.SelectedIndex = ComboBox1.Items.IndexOf(tip_incasare)
            .nr_rzf_Textbox.Text = nr_rzf
            .explicatii_Textbox.Text = explicatii
            .suma_CASA_Textbox.Text = suma_cash
            .suma_CARD_Textbox.Text = suma_card

            If data.Month <> Today.Month Or data.Year <> Today.Year Then
                .CheckBox2.CheckState = CheckState.Unchecked
            End If

            .DataGridView1.ClearSelection()
            For i = 0 To .DataGridView1.Rows.Count - 1
                If .DataGridView1.Rows(i).Cells("id").Value.ToString = id Then
                    .DataGridView1.Rows(i).Selected = True
                    If i > 0 Then
                        .DataGridView1.FirstDisplayedScrollingRowIndex = i - 1
                    Else
                        .DataGridView1.FirstDisplayedScrollingRowIndex = i
                    End If
                    Exit For
                End If
            Next
        End With
    End Sub

    Private Sub UpdateGestiuneToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateGestiuneToolStripMenuItem.Click
        Dim DGV As DataGridView = Incasari_DGV
        Dim mag_act As String = ""

        Dim row As DataGridViewRow = DGV.CurrentRow
        Dim id As Integer = row.Cells("id").Value
        Dim data As Date = CDate(row.Cells("data").Value)
        Dim nr_rzf As String = row.Cells("nr_rzf").Value.ToString
        Dim tip_incasare As String = row.Cells("tip_incasare").Value.ToString
        Dim explicatii As String = row.Cells("explicatii").Value
        Dim suma_cash As Decimal = row.Cells("Cash").Value
        Dim suma_card As Decimal = row.Cells("POS").Value
        Dim suma_total = suma_cash + suma_card

        Dim cash_cont As Boolean = row.Cells("cash").Value

        If dbconn.State = ConnectionState.Closed Then
            dbconn.Open()
        End If
        Dim sql_iesiri As String = "REPLACE INTO iesiri(data,tip_incasare,nr_rzf,explicatii,suma,magazin) " _
                                & "VALUES(@data,@tip_incasare,@nr_rzf,@explicatii,@suma,@magazin)"
        Try

            dbcomm = New MySqlCommand(sql_iesiri, dbconn)
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            dbcomm.Parameters.AddWithValue("@data", data)
            dbcomm.Parameters.AddWithValue("@tip_incasare", tip_incasare)
            dbcomm.Parameters.AddWithValue("@nr_rzf", nr_rzf)
            dbcomm.Parameters.AddWithValue("@explicatii", explicatii)
            dbcomm.Parameters.AddWithValue("@suma", CDec(suma_total))
            dbcomm.Parameters.AddWithValue("@magazin", ComboBox3.SelectedValue)
            dbcomm.ExecuteNonQuery()
            transaction.Commit()
        Catch ex As Exception
            MsgBox("Failed to insert data iesiri: " & ex.Message.ToString())
        End Try

    End Sub
    Private Sub TransferaCashPOSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TransferaCashPOSToolStripMenuItem.Click
        Dim DGV As DataGridView = Incasari_DGV
        Dim row As DataGridViewRow = DGV.CurrentRow

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
        Else : suma_card = row.Cells("POS").Value
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
    Private Sub SchimbaMagazinToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SchimbaMagazinToolStripMenuItem.Click
        Dim DGV As DataGridView = Incasari_DGV

        Dim mag As String = ComboBox3.SelectedValue.ToString
        Dim maga As String = ""
        If mag = "PM" Then
            maga = "MV"
        ElseIf mag = "MV" Then
            maga = "PM"
        End If
        Dim yesno As Integer = MsgBox("Schimb magazinul in " & maga & "?", MsgBoxStyle.YesNo)

        Dim row As DataGridViewRow = DGV.CurrentRow
        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then
            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            For i = 0 To DGV.SelectedRows.Count - 1
                Dim id As Integer = DGV.SelectedRows(i).Cells("id").Value
                Dim data As Date = CDate(DGV.SelectedRows(i).Cells("data").Value)
                Dim nr_rzf As String = DGV.SelectedRows(i).Cells("nr_rzf").Value.ToString
                Dim tip_incasare As String = DGV.SelectedRows(i).Cells("tip_incasare").Value.ToString
                Dim explicatii As String = DGV.SelectedRows(i).Cells("explicatii").Value

                Try
                    Dim sql_del_inc As String = "Update incasari SET magazin=@magazin WHERE id=@id AND data=@data"
                    dbcomm = New MySqlCommand(sql_del_inc, dbconn)
                    dbcomm.Parameters.AddWithValue("@id", id)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@magazin", maga)
                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Nu s-a schimbat nimic: " & ex.Message.ToString)
                End Try

                Try
                    Dim sql_del_ies As String = "Update iesiri SET magazin=@magazin WHERE data=@data AND nr_rzf=@nr_rzf AND tip_incasare=@tip_incasare AND magazin='" & mag & "'"

                    dbcomm = New MySqlCommand(sql_del_ies, dbconn)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@nr_rzf", nr_rzf)
                    dbcomm.Parameters.AddWithValue("@tip_incasare", tip_incasare)
                    dbcomm.Parameters.AddWithValue("@magazin", maga)
                    dbcomm.ExecuteNonQuery()
                    dbcomm.ExecuteNonQuery()

                Catch ex As Exception
                    MsgBox("Nu s-a modificat iesiri: " & ex.Message.ToString)
                End Try
            Next
            transaction.Commit()

            Load_Incasari()
            Load_Cheltuieli()
            Load_Iesiri()
        End If

    End Sub
    Private Sub Cheltuieli_BU_Click(sender As Object, e As EventArgs) Handles Cheltuieli_BU.Click
        Form_cheltuieli.Show()
        Me.Hide()
    End Sub
    Private Sub Strip_Cheltuieli_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Strip_Cheltuieli.Opening
        Dim DGV As DataGridView = Cheltuieli_DGV
        If DGV.Rows.Count > 0 Then
            If DGV.CurrentRow.Index > -1 AndAlso DGV.SelectedRows.Count <> 0 Then
                Dim row As DataGridViewRow = DGV.CurrentRow
                Dim tip_cheltuiala As String = row.Cells("tip_cheltuiala").Value.ToString

                If tip_cheltuiala = "DP" Then
                    PrinteazaDPToolStripMenuItem.Enabled = True
                ElseIf tip_cheltuiala <> "DP" Then
                    PrinteazaDPToolStripMenuItem.Enabled = False
                End If
            End If
        ElseIf DGV.Rows.Count < 1 Then
            e.Cancel = True
        End If
    End Sub
    Private Sub PrinteazaDPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrinteazaDPToolStripMenuItem.Click
        Dim DGV As DataGridView = Cheltuieli_DGV
        Dim row As DataGridViewRow = DGV.CurrentRow
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
        graph.DrawString("Unitatea", text_font, XBrushes.Black,
                         New XRect(102, 22, 40, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawLine(pen, 140, 20, 140, 37) '|
        graph.DrawString("MILICOM CAZ SRL" & maga, text_bold_font, XBrushes.Black,
                         New XRect(140, 22, 355, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 100, 37, 495, 37) '----


        graph.DrawLine(pen, 100, 43, 495, 43) '----

        graph.DrawString("DISPOZITIE DE *)", text_font, XBrushes.Black,
                         New XRect(102, 45, 80, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawLine(pen, 180, 43, 180, 60) '|
        graph.DrawString("PLATA", text_font, XBrushes.Black,
                         New XRect(180, 45, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 43, 260, 60) '|
        graph.DrawString("", text_font, XBrushes.Black,
                         New XRect(260, 45, 80, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawLine(pen, 340, 43, 340, 60) '|
        graph.DrawString("CATRE CASIERIE", text_font, XBrushes.Black,
                         New XRect(340, 45, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 420, 43, 420, 60) '|
        graph.DrawString("UNITATE", text_font, XBrushes.Black,
                         New XRect(420, 45, 75, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 100, 60, 495, 60) '----
        graph.DrawString("nr.", text_font, XBrushes.Black,
                         New XRect(102, 62, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 140, 60, 140, 77) '|
        graph.DrawString(nr_chitanta, text_bold_font, XBrushes.Black,
                         New XRect(140, 62, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 220, 60, 220, 77) '|
        graph.DrawString("din", text_font, XBrushes.Black,
                         New XRect(220, 62, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 60, 260, 77) '|
        graph.DrawString(data, text_bold_font, XBrushes.Black,
                         New XRect(260, 62, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 340, 60, 340, 77) '|

        graph.DrawLine(pen, 100, 77, 340, 77) '----
        graph.DrawString("Numele si Prenumele", mic_font, XBrushes.Black,
                         New XRect(102, 81, 80, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawLine(pen, 180, 77, 180, 94) '|
        graph.DrawString("CAZAN MIHAI", text_bold_font, XBrushes.Black,
                         New XRect(180, 79, 160, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 340, 77, 340, 94) '|

        graph.DrawLine(pen, 100, 94, 340, 94)  '----
        graph.DrawString("Functia (calitatea)", mic_font, XBrushes.Black,
                        New XRect(102, 98, 80, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawLine(pen, 180, 94, 180, 111) '|
        graph.DrawString("administrator", text_font, XBrushes.Black,
                         New XRect(180, 96, 160, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 340, 94, 340, 111) '|

        graph.DrawLine(pen, 100, 111, 495, 111) '----
        graph.DrawString("Suma", mic_font, XBrushes.Black,
                         New XRect(102, 115, 40, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawLine(pen, 140, 111, 140, 128) '|
        graph.DrawString(suma_form, text_bold_font, XBrushes.Black,
                         New XRect(140, 113, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 220, 111, 220, 128) '|
        graph.DrawString("lei", text_font, XBrushes.Black,
                         New XRect(220, 113, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 111, 260, 128) '|
        graph.DrawString(NrCuv(suma) & " lei", text_bold_font, XBrushes.Black,
                         New XRect(260, 113, 240, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 100, 128, 495, 128) '----
        graph.DrawLine(pen, 140, 128, 140, 145) '|
        graph.DrawString("(in cifre)", text_font, XBrushes.Black,
                         New XRect(140, 130, 120, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 128, 260, 145) '|
        graph.DrawString("(in litere)", text_font, XBrushes.Black,
                         New XRect(260, 130, 240, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 100, 145, 495, 145) '----
        graph.DrawString("Scopul platii", mic_font, XBrushes.Black,
                        New XRect(102, 149, 80, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawLine(pen, 180, 145, 180, 162) '|
        graph.DrawString("restituire imprumut", text_bold_font, XBrushes.Black,
                         New XRect(180, 147, 315, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 100, 162, 495, 162) '----

        graph.DrawLine(pen, 100, 179, 495, 179) '----
        graph.DrawString("Semnatura", mic_font, XBrushes.Black,
                         New XRect(100, 190, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 140, 179, 140, 213) '|
        graph.DrawString("Conducatorul unitatii", mic_font, XBrushes.Black,
                         New XRect(140, 183, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 220, 179, 220, 213) '|
        graph.DrawString("Viza de control financiar preventiv", mic_font, XBrushes.Black,
                         New XRect(220, 183, 120, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 340, 179, 340, 213) '|
        graph.DrawString("Compartiment financiar-contabil", mic_font, XBrushes.Black,
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
        graph.DrawString("Date suplimentare privind beneficiarul sumei", text_font, XBrushes.Black,
                         New XRect(180, 22, 315, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 180, 37, 495, 37) '----


        graph.DrawLine(pen, 180, 43, 495, 43) '----
        graph.DrawString("Actul de identitate", text_font, XBrushes.Black,
                         New XRect(180, 45, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 43, 260, 60) '|
        graph.DrawString("CI", text_bold_font, XBrushes.Black,
                         New XRect(260, 45, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 300, 43, 300, 60) '|
        graph.DrawString("Seria", text_font, XBrushes.Black,
                         New XRect(300, 45, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 340, 43, 340, 60) '|
        graph.DrawString("MS", text_bold_font, XBrushes.Black,
                         New XRect(340, 45, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 380, 43, 380, 60) '|
        graph.DrawString("nr.", text_font, XBrushes.Black,
                         New XRect(380, 45, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 420, 43, 420, 60) '|
        graph.DrawString("722599", text_bold_font, XBrushes.Black,
                         New XRect(420, 45, 75, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 180, 60, 495, 60) '----
        graph.DrawString("Am primit suma de", text_font, XBrushes.Black,
                         New XRect(180, 62, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 60, 260, 77) '|
        graph.DrawString(suma_form, text_bold_font, XBrushes.Black,
                         New XRect(260, 62, 160, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 420, 60, 420, 77) '|
        graph.DrawString("lei", text_font, XBrushes.Black,
                         New XRect(420, 62, 75, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawString("Se completeaza numai", mic_font, XBrushes.Black,
                         New XRect(100, 79, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("pentru plati", mic_font, XBrushes.Black,
                         New XRect(100, 87, 80, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 180, 77, 495, 77) '----
        graph.DrawString("(in cifre)", text_font, XBrushes.Black,
                         New XRect(180, 79, 315, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 180, 94, 495, 94)  '----
        graph.DrawLine(pen, 220, 94, 220, 111) '|
        graph.DrawString("Data", text_font, XBrushes.Black,
                         New XRect(220, 96, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 94, 260, 111) '|
        graph.DrawString(data, text_bold_font, XBrushes.Black,
                         New XRect(260, 96, 160, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 420, 94, 420, 111) '|

        graph.DrawLine(pen, 180, 111, 495, 111) '----
        graph.DrawLine(pen, 220, 111, 220, 145) '|
        graph.DrawString("Semnatura", mic_font, XBrushes.Black,
                         New XRect(220, 124, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 260, 111, 260, 145) '|
        graph.DrawLine(pen, 420, 111, 420, 145) '|

        graph.DrawLine(pen, 180, 20, 180, 145) '|

        graph.DrawLine(pen, 100, 145, 495, 145) '----
        graph.DrawString("CASIER", text_font, XBrushes.Black,
                         New XRect(100, 147, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 140, 145, 140, 162) '|

        graph.DrawLine(pen, 100, 162, 495, 162) '----
        graph.DrawString("Platit/incasat suma de", mic_font, XBrushes.Black,
                        New XRect(100, 166, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 180, 162, 180, 179) '|
        graph.DrawString(suma_form, text_bold_font, XBrushes.Black,
                       New XRect(180, 164, 240, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 420, 162, 420, 179) '|
        graph.DrawString("lei", text_font, XBrushes.Black,
                       New XRect(420, 164, 75, pdfPage.Height.Point), XStringFormats.TopCenter)


        graph.DrawLine(pen, 100, 179, 495, 179) '----
        graph.DrawString("Data", text_font, XBrushes.Black,
                         New XRect(100, 188, 80, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 180, 179, 180, 213) '|
        graph.DrawString(data, text_bold_font, XBrushes.Black,
                         New XRect(180, 188, 120, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, 300, 179, 300, 213) '|
        graph.DrawString("Semnatura", text_font, XBrushes.Black,
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
    Private Sub StergeInregistrareaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StergeInregistrareaToolStripMenuItem.Click
        Dim DGV As DataGridView = Cheltuieli_DGV
        Dim yesno As Integer = MsgBox("Stergi Inregistrarea?", MsgBoxStyle.YesNo)
        Dim row As DataGridViewRow = DGV.CurrentRow
        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then

            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            For i = 0 To DGV.SelectedRows.Count - 1
                Dim id As Integer = CInt(DGV.SelectedRows(i).Cells("id").Value)
                Dim data As Date = CDate(DGV.SelectedRows(i).Cells("data").Value)
                Dim nr_chitanta As String = DGV.SelectedRows(i).Cells("nr_chitanta").Value.ToString
                Dim tip_cheltuiala As String = DGV.SelectedRows(i).Cells("tip_cheltuiala").Value.ToString
                Dim explicatii As String = DGV.SelectedRows(i).Cells("explicatii").Value

                Try
                    Dim sql_del_inc As String = "DELETE FROM cheltuieli WHERE id=@id AND data=@data AND nr_chitanta=@nr_chitanta AND tip_cheltuiala=@tip_cheltuiala AND magazin='" & ComboBox3.SelectedValue & "'"
                    dbcomm = New MySqlCommand(sql_del_inc, dbconn)
                    dbcomm.Parameters.AddWithValue("@id", id)
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


            Load_Cheltuieli()
            dbconn.Close()
        End If
    End Sub
    Private Sub ModificaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModificaToolStripMenuItem.Click
        Dim DGV As DataGridView = Cheltuieli_DGV
        Dim row As DataGridViewRow = DGV.CurrentRow

        Dim dbconn As New MySqlConnection
        Dim dbcomm As New MySqlCommand
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        If dbconn.State = 0 Then
            dbconn.Open()
        End If


        Dim data As Date = CDate(row.Cells("data").Value)
        Dim nr_chitanta As String = row.Cells("nr_chitanta").Value.ToString
        Dim tip_cheltuiala As String = row.Cells("tip_cheltuiala").Value.ToString
        Dim explicatii As String = row.Cells("explicatii").Value
        Dim suma As Decimal = row.Cells("suma").Value
        Dim id As Integer = row.Cells("id").Value
        Dim mag As String = ComboBox3.SelectedValue
        Dim cash_cont As Boolean = row.Cells("cash").Value
        Form_cheltuieli.Show()
        With Form_cheltuieli

            .edit_Lbl.Text = "Edit"
            'edit_Lbl.Visible = True
            .id_Lbl.Text = id
            'id_Lbl.Visible = False
            .GroupBox1.Visible = True

            .DateTimePicker1.Value = data
            .ComboBox1.SelectedIndex = .ComboBox1.Items.IndexOf(tip_cheltuiala)
            .nr_rzf_Textbox.Text = nr_chitanta
            .explicatii_Textbox.Text = explicatii
            .suma_Textbox.Text = suma
            If cash_cont = True Then
                .cash_RB.Checked = True
            ElseIf cash_cont = False Then
                .cash_RB.Checked = False
            End If
            If data.Month <> Today.Month Or data.Year <> Today.Year Then
                .CheckBox2.CheckState = CheckState.Unchecked
            End If
            .DataGridView1.ClearSelection()
            For i = 0 To .DataGridView1.Rows.Count - 1
                If .DataGridView1.Rows(i).Cells("id").Value.ToString = id Then
                    .DataGridView1.Rows(i).Selected = True
                    If i > 0 Then
                        .DataGridView1.FirstDisplayedScrollingRowIndex = i - 1
                    Else
                        .DataGridView1.FirstDisplayedScrollingRowIndex = i
                    End If
                    Exit For
                End If
            Next
        End With

    End Sub
    Private Sub TransferaCheltuialaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TransferaCheltuialaToolStripMenuItem.Click
        Dim DGV As DataGridView = Cheltuieli_DGV
        Dim mag_act As String = DGV.SelectedRows(0).Cells("magazin").Value
        Dim mag_nou As String = DGV.SelectedRows(0).Cells("magazin").Value
        If mag_act = "PM" Then
            mag_nou = "MV"
        ElseIf mag_act = "MV" Then
            mag_nou = "PM"
        End If

        Dim yesno As Integer = MsgBox("Transferi Cheltuiala in Casa " & mag_nou & " ?", MsgBoxStyle.YesNo)
        Dim row As DataGridViewRow = DGV.CurrentRow
        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then

            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            For i = 0 To DGV.SelectedRows.Count - 1
                Dim data As Date = CDate(DGV.SelectedRows(i).Cells("data").Value)
                Dim nr_chitanta As String = DGV.SelectedRows(i).Cells("nr_chitanta").Value.ToString
                Dim tip_cheltuiala As String = DGV.SelectedRows(i).Cells("tip_cheltuiala").Value.ToString
                Dim explicatii As String = DGV.SelectedRows(i).Cells("explicatii").Value

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

            Load_Cheltuieli()

            dbconn.Close()
        End If
    End Sub

    Private Sub Gestiune_BU_Click(sender As Object, e As EventArgs) Handles Gestiune_BU.Click
        Form_gestiune.Show()
    End Sub
    Private Sub ad_nir_BU_Click(sender As Object, e As EventArgs) Handles ad_nir_BU.Click, Button1.Click
        Form_adauga_nir.Show()
        Form_adauga_nir.ComboBox3.SelectedValue = ComboBox3.SelectedValue
    End Sub
    Private Sub ArataNIRToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ArataNIRToolStripMenuItem.Click
        Dim row As DataGridViewRow = Marfa_DGV.CurrentRow

        Dim data As Date = CDate(row.Cells("data").Value)
        Dim nir As String = row.Cells("nir").Value.ToString
        Dim mag As String = row.Cells("magazin").Value.ToString

        With Form_afiseaza_nir
            .nir_TB.Text = nir
            .DateTimePicker1.Value = data

        End With
        Form_afiseaza_nir.Show()

        Form_afiseaza_nir.ComboBox3.SelectedValue = mag
    End Sub
    Private Sub StergeIntrareaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StergeIntrareaToolStripMenuItem.Click

        Dim yesno As Integer = MsgBox("Stergi Inregistrarea?", MsgBoxStyle.YesNo)

        Dim row As DataGridViewRow = Intrari_DGV.CurrentRow
        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then

            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")

            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()

            For i = 0 To Intrari_DGV.SelectedRows.Count - 1
                Dim id As Integer = CInt(Intrari_DGV.SelectedRows(i).Cells("id").Value)
                Dim data As Date = CDate(Intrari_DGV.SelectedRows(i).Cells("data").Value)
                Dim nr_nir As String = Intrari_DGV.SelectedRows(i).Cells("nr_nir").Value.ToString
                Dim tip_document As String = Intrari_DGV.SelectedRows(i).Cells("tip_document").Value.ToString
                Dim explicatii As String = Intrari_DGV.SelectedRows(i).Cells("explicatii").Value
                Dim magazin As String = Intrari_DGV.SelectedRows(i).Cells("magazin").Value

                Try
                    Dim sql_del_inc As String = "DELETE FROM intrari WHERE id=@id AND data=@data"
                    dbcomm = New MySqlCommand(sql_del_inc, dbconn)
                    dbcomm.Parameters.AddWithValue("@id", id)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@nr_nir", nr_nir)
                    dbcomm.Parameters.AddWithValue("@tip_document", tip_document)
                    dbcomm.Parameters.AddWithValue("@explicatii", explicatii)
                    dbcomm.Parameters.AddWithValue("@magazin", magazin)
                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Nu s-a sters din incasari: " & ex.Message.ToString)
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

            Load_Intrari()
            dbread.Close()
            dbconn.Close()
        End If
    End Sub
    Private Sub ModificaIntrareToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModificaIntrareToolStripMenuItem.Click

        Dim row As DataGridViewRow = Intrari_DGV.CurrentRow
        If IsDBNull(row.Cells("id").Value) = False Then
            Dim mag_act As String = Intrari_DGV.SelectedRows(0).Cells("magazin").Value
            Dim mag_nou As String = Intrari_DGV.SelectedRows(0).Cells("magazin").Value
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


            Form_gestiune.Show()
            With Form_gestiune

                .edit_Lbl.Text = "Edit"
                .id_Lbl.Text = id
                .mag_v_Lbl.Text = mag_act
                .rzf_v_Lbl.Text = row.Cells("nr_nir").Value.ToString
                .GroupBox1.Visible = True

                .DateTimePicker1.Value = data
                .ComboBox1.SelectedIndex = .ComboBox1.Items.IndexOf(tip_document)
                .nr_nir_Textbox.Text = nr_nir
                .explicatii_Textbox.Text = explicatii
                .suma_Textbox.Text = suma

                If data.Month <> Today.Month Or data.Year <> Today.Year Then
                    .CheckBox2.CheckState = CheckState.Unchecked
                End If

                .DataGridView1.ClearSelection()
                For i = 0 To .DataGridView1.Rows.Count - 1
                    If .DataGridView1.Rows(i).Cells("id").Value.ToString = id Then
                        .DataGridView1.Rows(i).Selected = True
                        If i > 0 Then
                            .DataGridView1.FirstDisplayedScrollingRowIndex = i - 1
                        Else
                            .DataGridView1.FirstDisplayedScrollingRowIndex = i
                        End If
                        Exit For
                    End If
                Next
            End With
        End If

    End Sub

    Private Sub ToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem4.Click
        Dim yesno As Integer = MsgBox("Stergi Inregistrarea?", MsgBoxStyle.YesNo)

        Dim row As DataGridViewRow = Iesiri_DGV.CurrentRow
        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then

            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")

            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()

            For i = 0 To Iesiri_DGV.SelectedRows.Count - 1
                Dim id As Integer = CInt(Iesiri_DGV.SelectedRows(i).Cells("id").Value)
                Dim data As Date = CDate(Iesiri_DGV.SelectedRows(i).Cells("data").Value)
                Dim magazin As String = Iesiri_DGV.SelectedRows(i).Cells("magazin").Value

                Try
                    Dim sql_del_inc As String = "DELETE FROM iesiri WHERE id=@id AND data=@data"
                    dbcomm = New MySqlCommand(sql_del_inc, dbconn)
                    dbcomm.Parameters.AddWithValue("@id", id)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@magazin", magazin)
                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Nu s-a sters din incasari: " & ex.Message.ToString)
                End Try
            Next
            transaction.Commit()
            Load_Iesiri()
            dbread.Close()
            dbconn.Close()
        End If
    End Sub
    Private Sub Registru_Button_Click(sender As Object, e As EventArgs) Handles Registru_Button.Click
        '' -------------------- LOAD setari

        'dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        'Dim folder_pdf As String = ""
        'Try
        '    Dim set_sql As String = "SELECT * FROM setari WHERE setare='path_registru'"
        '    dbconn.Open()
        '    dbcomm = New MySqlCommand(set_sql, dbconn)
        '    dbread = dbcomm.ExecuteReader()
        '    dbread.Read()
        '    If dbread.HasRows = False Then
        '        SaveFileDialog1.Title = "Selectati Folderul pt Registru"
        '        SaveFileDialog1.FileName = "Selectati Folderul"
        '        SaveFileDialog1.Filter = "pdf Files (*.pdf*)|*.pdf"

        '        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
        '            Dim folder As String = System.IO.Path.GetDirectoryName(SaveFileDialog1.FileName) & "\"
        '            Dim dbconn As MySqlConnection = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        '            Try
        '                If dbconn.State = 0 Then
        '                    dbconn.Open()
        '                End If

        '                Dim set_path_sql As String = "REPLACE INTO setari(setare,valoare) VALUES('path_registru',@folder)"
        '                Using dbcomm As MySqlCommand = New MySqlCommand(set_path_sql, dbconn)
        '                    dbcomm.Parameters.AddWithValue("@folder", folder)
        '                    dbcomm.ExecuteNonQuery()
        '                End Using
        '            Catch ex As Exception
        '                MsgBox("Problem Nonquery: " & ex.Message.ToString)
        '            End Try
        '        End If

        '    ElseIf dbread.HasRows = True Then
        '        folder_pdf = dbread("valoare")
        '    End If

        'Catch ex As Exception
        '    MsgBox("Problem loading setari savefile: " & ex.Message.ToString)
        'End Try
        'dbread.Close()
        ''--------------------------------

        'Form_luna_Registru.ShowDialog()
        'Dim luna_reg As Date = Date.Parse(Date.DaysInMonth(Form_luna_Registru.ComboBox2.SelectedItem, Form_luna_Registru.ComboBox1.SelectedItem) & "." & Form_luna_Registru.ComboBox1.SelectedItem & "." & Form_luna_Registru.ComboBox2.SelectedItem)
        'Dim pdf As PdfSharp.Pdf.PdfDocument = New PdfSharp.Pdf.PdfDocument

        'If Form_luna_Registru.DialogResult = Windows.Forms.DialogResult.Cancel Then
        '    Exit Sub
        'End If
        'Dim punct_lucru As String = ""
        'If Form_luna_Registru.ComboBox3.SelectedValue = "PM" Then
        '    punct_lucru = "Magazin: Petru Maior nr. 9"
        'ElseIf Form_luna_Registru.ComboBox3.SelectedValue = "MV" Then
        '    punct_lucru = "Magazin: Mihai Viteazu nr. 28"
        'End If

        'Dim mag As String = Form_luna_Registru.ComboBox3.SelectedValue
        'pdf.Info.Title = "Registru Casa"
        'Dim pdfPage As PdfSharp.Pdf.PdfPage = pdf.AddPage


        'Dim graph As XGraphics = XGraphics.FromPdfPage(pdfPage)


        'Dim titlu_font As XFont = New XFont("Verdana", 16, XFontStyle.Bold)
        'Dim top_font As XFont = New XFont("Verdana", 12, XFontStyle.Bold)
        'Dim report_font As XFont = New XFont("Verdana", 10, XFontStyle.Bold)
        'Dim report_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        'Dim text_font As XFont = New XFont("Verdana", 10, XFontStyle.Regular)
        'Dim text_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        'Dim bottom_font As XFont = New XFont("Verdana", 12, XFontStyle.Bold)

        'Dim incas_tot As Double = 0
        'Dim chelt_tot As Double = 0
        ''A4 = 8.27x11.69" x72points/inch = 595x842 points
        ''-----------------HEADER ----------------------------
        'graph.DrawString("MILICOM CAZ SRL", report_font, XBrushes.Black, _
        '                 New XRect(20, 20, 100, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawString("CIF: 31240216", text_font, XBrushes.Black, _
        '                 New XRect(20, 35, 100, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawString(punct_lucru, report_font, XBrushes.Black, _
        '                 New XRect(415, 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

        'graph.DrawString("REGISTRU DE CASA", titlu_font, XBrushes.Black, _
        '                 New XRect(20, 50, 575, pdfPage.Height.Point), XStringFormats.TopCenter)

        'graph.DrawLine(XPens.Black, 20, 85, 575, 85)
        'graph.DrawLine(XPens.Black, 20, 129, 575, 129)


        'graph.DrawString("Nr", top_font, XBrushes.Black, _
        '                 New XRect(20, 92, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
        'graph.DrawString("crt.", top_font, XBrushes.Black, _
        '                 New XRect(20, 105, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
        'graph.DrawString("Nr act", top_font, XBrushes.Black, _
        '                 New XRect(70, 92, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
        'graph.DrawString("casa", top_font, XBrushes.Black, _
        '                 New XRect(70, 105, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
        'graph.DrawString("Nr anexa", top_font, XBrushes.Black, _
        '                 New XRect(120, 100, 50, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawString("Explicatii".ToString, top_font, XBrushes.Black, _
        '                 New XRect(190, 100, 300, pdfPage.Height.Point), XStringFormats.TopCenter)
        'graph.DrawString("Incasari".ToString, top_font, XBrushes.Black, _
        '                 New XRect(450, 100, 60, pdfPage.Height.Point), XStringFormats.TopCenter)
        'graph.DrawString("Plati".ToString, top_font, XBrushes.Black, _
        '                 New XRect(520, 100, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
        ''-----------------HEADER ---------------------------



        'Dim sold_initial As Double = CDbl(Form_luna_Registru.TextBox1.Text)
        'Dim sold_prec As Double = sold_initial
        'Dim sold_zi As Double = 0

        'Dim nxtRow As Double = 120
        'Dim suma_inc As Double = 0
        'Dim suma_che As Double = 0

        'For d = 1 To Date.DaysInMonth(luna_reg.Year, luna_reg.Month)

        '    Dim data As String = Format(Date.Parse(d & "." & luna_reg.Month & "." & luna_reg.Year), "yyyy-MM-dd")

        '    Dim datatext As Date = data.ToString

        '    suma_inc = 0
        '    suma_che = 0
        '    Try
        '        Dim sql_chl As String = "SELECT * FROM cheltuieli WHERE data='" & data & "' AND magazin='" & mag & "' AND cash=TRUE"
        '        Dim chelt As Boolean = False
        '        If dbconn.State = ConnectionState.Closed Then
        '            dbconn.Open()
        '        End If
        '        dbcomm = New MySqlCommand(sql_chl, dbconn)
        '        dbread2 = dbcomm.ExecuteReader()
        '        If dbread2.HasRows = True Then
        '            chelt = True
        '        End If
        '        dbread2.Close()
        '        Dim sql_inc As String = "SELECT * FROM incasari WHERE data='" & data & "' AND magazin='" & mag & "'"
        '        If dbconn.State = ConnectionState.Closed Then
        '            dbconn.Open()
        '        End If
        '        dbcomm = New MySqlCommand(sql_inc, dbconn)
        '        dbread = dbcomm.ExecuteReader()
        '        If dbread.HasRows = True Or chelt = True Then

        '            nxtRow = nxtRow + 20

        '            If nxtRow > 650 Then
        '                '-----------------FOOTER ----------------------------
        '                ' MsgBox("footer")
        '                graph.DrawLine(XPens.Black, 20, 770, 575, 770)
        '                graph.DrawString("Report pagina/total".ToString, top_font, XBrushes.Black, _
        '                                 New XRect(190, 775, 300, pdfPage.Height.Point), XStringFormats.TopCenter)


        '                Dim tf2 As XTextFormatter = New XTextFormatter(graph)
        '                tf2.Alignment = XParagraphAlignment.Right

        '                tf2.DrawString(FormatNumber(CDec(incas_tot), 2, TriState.True, TriState.False, TriState.True), report_italic_font, XBrushes.Black, _
        '                               New XRect(450, 775, 60, pdfPage.Height.Point), XStringFormats.TopLeft)

        '                tf2.DrawString(FormatNumber(CDec(chelt_tot), 2, TriState.True, TriState.False, TriState.True), report_italic_font, XBrushes.Black, _
        '                               New XRect(520, 775, 55, pdfPage.Height.Point), XStringFormats.TopLeft)

        '                graph.DrawLine(XPens.Black, 20, 800, 575, 800)
        '                graph.DrawString("Casier".ToString, text_font, XBrushes.Black, _
        '                                 New XRect(20, 805, 300, pdfPage.Height.Point), XStringFormats.TopLeft)
        '                graph.DrawString("Compartiment Financiar - Contabil".ToString, text_font, XBrushes.Black, _
        '                                 New XRect(370, 805, pdfPage.Height.Point, pdfPage.Height.Point), XStringFormats.TopLeft)
        '                '-----------------FOOTER ----------------------------

        '                pdfPage = pdf.AddPage()
        '                graph = XGraphics.FromPdfPage(pdfPage)

        '                '-----------------HEADER ----------------------------
        '                'MsgBox("header")

        '                graph.DrawString("MILICOM CAZ SRL", report_font, XBrushes.Black, _
        '                                 New XRect(20, 20, 100, pdfPage.Height.Point), XStringFormats.TopLeft)
        '                graph.DrawString("CIF: 31240216", text_font, XBrushes.Black, _
        '                                 New XRect(20, 35, 100, pdfPage.Height.Point), XStringFormats.TopLeft)
        '                graph.DrawString(punct_lucru, report_font, XBrushes.Black, _
        '                                 New XRect(415, 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

        '                graph.DrawString("REGISTRU DE CASA", titlu_font, XBrushes.Black, _
        '                                 New XRect(20, 50, 575, pdfPage.Height.Point), XStringFormats.TopCenter)

        '                graph.DrawLine(XPens.Black, 20, 85, 575, 85)
        '                graph.DrawLine(XPens.Black, 20, 129, 575, 129)
        '                graph.DrawString("Nr", top_font, XBrushes.Black, _
        '                                 New XRect(20, 92, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
        '                graph.DrawString("crt.", top_font, XBrushes.Black, _
        '                                 New XRect(20, 105, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
        '                graph.DrawString("Nr act", top_font, XBrushes.Black, _
        '                                 New XRect(70, 92, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
        '                graph.DrawString("casa", top_font, XBrushes.Black, _
        '                                 New XRect(70, 105, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
        '                graph.DrawString("Nr anexa", top_font, XBrushes.Black, _
        '                                 New XRect(120, 100, 50, pdfPage.Height.Point), XStringFormats.TopLeft)
        '                graph.DrawString("Explicatii".ToString, top_font, XBrushes.Black, _
        '                                 New XRect(190, 100, 300, pdfPage.Height.Point), XStringFormats.TopCenter)
        '                graph.DrawString("Incasari".ToString, top_font, XBrushes.Black, _
        '                                 New XRect(450, 100, 60, pdfPage.Height.Point), XStringFormats.TopCenter)
        '                graph.DrawString("Plati".ToString, top_font, XBrushes.Black, _
        '                                 New XRect(520, 100, 55, pdfPage.Height.Point), XStringFormats.TopCenter)
        '                '-----------------HEADER ---------------------------

        '                nxtRow = 140
        '            End If

        '            graph.DrawString("Report/Sold ziua Precedenta", report_font, XBrushes.Black, _
        '                             New XRect(20, nxtRow, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)
        '            graph.DrawString("Sold initial", report_font, XBrushes.Black, _
        '                             New XRect(270, nxtRow, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)
        '            graph.DrawString(datatext, report_font, XBrushes.Black, _
        '                             New XRect(350, nxtRow, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)
        '            'graph.DrawString(sold_prec.ToString, report_font, XBrushes.Black, _
        '            '                 New XRect(460, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)

        '            Dim tf As XTextFormatter = New XTextFormatter(graph)
        '            tf.Alignment = XParagraphAlignment.Right
        '            tf.DrawString(FormatNumber(CDec(sold_prec), 2, TriState.True, TriState.False, TriState.True), report_font, XBrushes.Black, _
        '                          New XRect(450, nxtRow, 60, pdfPage.Height.Point), XStringFormats.TopLeft)
        '            Dim crt As Integer = 0

        '            While dbread.Read
        '                crt = crt + 1
        '                suma_inc = suma_inc + CDbl(dbread("suma_cash"))
        '                nxtRow = nxtRow + 15

        '                incas_tot = incas_tot + CDbl(dbread("suma_cash"))
        '                incas_tot = incas_tot
        '                graph.DrawString((crt) & ".", text_font, XBrushes.Black, _
        '                                 New XRect(20, nxtRow, 30, pdfPage.Height.Point), XStringFormats.TopCenter)

        '                graph.DrawString(dbread("tip_incasare"), text_font, XBrushes.Black, _
        '                                 New XRect(70, nxtRow, 30, pdfPage.Height.Point), XStringFormats.TopCenter)

        '                graph.DrawString(dbread("nr_rzf"), text_font, XBrushes.Black, _
        '                                 New XRect(125, nxtRow, 45, pdfPage.Height.Point), XStringFormats.TopLeft)

        '                graph.DrawString(dbread("explicatii"), text_font, XBrushes.Black, _
        '                                 New XRect(200, nxtRow, 240, pdfPage.Height.Point), XStringFormats.TopLeft)

        '                Dim suma_incasari As String = FormatNumber(CDec(dbread("suma_cash")), 2, TriState.True, TriState.False, TriState.True)
        '                tf.DrawString(suma_incasari, text_font, XBrushes.Black, New XRect(450, nxtRow, 60, pdfPage.Height.Point), XStringFormats.TopLeft)
        '                tf.DrawString("-", text_font, XBrushes.Black, _
        '                              New XRect(520, nxtRow, 55, pdfPage.Height.Point), XStringFormats.TopLeft)

        '            End While

        '            dbread.Close()

        '            Dim sql_che As String = "SELECT * FROM cheltuieli WHERE data='" & data & "' AND magazin='" & mag & "' AND cash=TRUE"
        '            dbcomm = New MySqlCommand(sql_che, dbconn)
        '            dbread = dbcomm.ExecuteReader()

        '            While dbread.Read
        '                crt = crt + 1
        '                suma_che = suma_che + CDbl(dbread("suma"))

        '                nxtRow = nxtRow + 15

        '                chelt_tot = chelt_tot + CDbl(dbread("suma"))
        '                chelt_tot = chelt_tot

        '                graph.DrawString((crt) & ".", text_font, XBrushes.Black, _
        '                                 New XRect(20, nxtRow, 30, pdfPage.Height.Point), XStringFormats.TopCenter)

        '                graph.DrawString(dbread("tip_cheltuiala"), text_font, XBrushes.Black, _
        '                                 New XRect(70, nxtRow, 30, pdfPage.Height.Point), XStringFormats.TopCenter)

        '                graph.DrawString(dbread("nr_chitanta"), text_font, XBrushes.Black, _
        '                                 New XRect(125, nxtRow, 45, pdfPage.Height.Point), XStringFormats.TopLeft)

        '                graph.DrawString(dbread("explicatii"), text_font, XBrushes.Black, _
        '                                 New XRect(200, nxtRow, 240, pdfPage.Height.Point), XStringFormats.TopLeft)

        '                '    graph.DrawString("-", text_font, XBrushes.Black, _
        '                'New XRect(460, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
        '                tf.DrawString("-", text_font, XBrushes.Black, _
        '                              New XRect(450, nxtRow, 60, pdfPage.Height.Point), XStringFormats.TopLeft)

        '                '    graph.DrawString(dbread("suma"), text_font, XBrushes.Black, _
        '                'New XRect(530, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
        '                Dim suma_cheltuiala As String = FormatNumber(CDec(dbread("suma")), 2, TriState.True, TriState.False, TriState.True)
        '                tf.DrawString(suma_cheltuiala, text_font, XBrushes.Black, _
        '                              New XRect(520, nxtRow, 55, pdfPage.Height.Point), XStringFormats.TopLeft)

        '            End While

        '            dbread.Close()

        '            sold_zi = suma_inc - suma_che
        '            sold_prec = CDec(sold_zi) + CDec(sold_prec)
        '            nxtRow = nxtRow + 20

        '            graph.DrawString("RULAJ ZI", report_font, XBrushes.Black, _
        '                             New XRect(300, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopLeft)
        '            'graph.DrawString(suma_inc, report_italic_font, XBrushes.Black, _
        '            '                 New XRect(460, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
        '            tf.DrawString(FormatNumber(CDec(suma_inc), 2, TriState.True, TriState.False, TriState.True), report_italic_font, XBrushes.Black, _
        '                            New XRect(450, nxtRow, 60, pdfPage.Height.Point), XStringFormats.TopLeft)

        '            'graph.DrawString(suma_che, report_italic_font, XBrushes.Black, _
        '            '                 New XRect(530, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
        '            tf.DrawString(FormatNumber(CDec(suma_che), 2, TriState.True, TriState.False, TriState.True), report_italic_font, XBrushes.Black, _
        '                             New XRect(520, nxtRow, 55, pdfPage.Height.Point), XStringFormats.TopLeft)

        '            nxtRow = nxtRow + 15
        '            graph.DrawString("SOLD FINAL", report_font, XBrushes.Black, _
        '                             New XRect(300, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopLeft)
        '            'graph.DrawString(sold_prec, report_font, XBrushes.Black, _
        '            '                 New XRect(460, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
        '            If sold_prec < 0 Then

        '                With Form_DI

        '                    .Label1.Text = "Adauga DI in data de " & datatext & " cu suma de " & sold_prec * (-1) & ""

        '                    .DateTimePicker1.Value = datatext
        '                    .suma_Textbox.Text = Math.Ceiling(sold_prec * (-1)).ToString
        '                    .ShowDialog()
        '                End With

        '                Exit Sub
        '            End If
        '            tf.DrawString(FormatNumber(CDec(sold_prec), 2, TriState.True, TriState.False, TriState.True), report_font, XBrushes.Black, _
        '                            New XRect(450, nxtRow, 60, pdfPage.Height.Point), XStringFormats.TopLeft)
        '        End If
        '        dbread.Close()
        '        dbconn.Close()
        '    Catch ex As Exception
        '        MsgBox("Problem loading data: " & ex.Message.ToString)
        '    End Try

        'Next
        ''-----------------FOOTER ----------------------------

        'graph.DrawLine(XPens.Black, 20, 770, 575, 770)
        'graph.DrawString("Report pagina/total".ToString, top_font, XBrushes.Black, _
        '                 New XRect(190, 775, 300, pdfPage.Height.Point), XStringFormats.TopCenter)




        'Dim tf1 As XTextFormatter = New XTextFormatter(graph)
        'tf1.Alignment = XParagraphAlignment.Right
        'tf1.DrawString(FormatNumber(CDec(incas_tot), 2, TriState.True, TriState.False, TriState.True), report_italic_font, XBrushes.Black, _
        '               New XRect(450, 775, 60, pdfPage.Height.Point), XStringFormats.TopLeft)

        'tf1.DrawString(FormatNumber(CDec(chelt_tot), 2, TriState.True, TriState.False, TriState.True), report_italic_font, XBrushes.Black, _
        '               New XRect(520, 775, 55, pdfPage.Height.Point), XStringFormats.TopLeft)

        'graph.DrawLine(XPens.Black, 20, 800, 575, 800)
        'graph.DrawString("Casier".ToString, text_font, XBrushes.Black, _
        '                 New XRect(20, 805, 300, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawString("Compartiment Financiar - Contabil".ToString, text_font, XBrushes.Black, _
        '                 New XRect(370, 805, pdfPage.Height.Point, pdfPage.Height.Point), XStringFormats.TopLeft)
        ''-----------------FOOTER ----------------------------
        'dbread.Dispose()
        'dbread2.Dispose()

        ''------------solduri luna RAPORT
        'If dbconn.State = ConnectionState.Closed Then
        '    dbconn.Open()
        'End If

        'dbcomm = New MySqlCommand("SELECT * FROM solduri_casa WHERE data=@data AND magazin='" & Form_luna_Registru.ComboBox3.SelectedValue & "'", dbconn)
        'dbcomm.Parameters.AddWithValue("@data", luna_reg)


        'Dim dbread3 As MySqlDataReader
        'dbread3 = dbcomm.ExecuteReader()
        'dbread3.Read()
        'If dbread3.HasRows = True AndAlso dbread3("permanent") = True Then
        '    MsgBox("Atentie!!! Soldurile nu se pot modifica")
        '    dbread3.Close()
        'ElseIf dbread3.HasRows = False Or (dbread3.HasRows = True AndAlso dbread3("permanent") = False) Then
        '    dbread3.Close()
        '    Dim sql As String = "INSERT INTO solduri_casa(data,casa_sold_initial,incasari,cheltuieli,casa_sold_final,magazin) " _
        '                       & "VALUES(@data,@casa_sold_initial,@incasari,@cheltuieli,@casa_sold_final,@magazin) ON DUPLICATE KEY UPDATE casa_sold_initial=@casa_sold_initial,incasari=@incasari,cheltuieli=@cheltuieli,casa_sold_final=@casa_sold_final,magazin=@magazin"
        '    Dim data_ultima As Date = Format(Date.Parse(Date.DaysInMonth(luna_reg.Year, luna_reg.Month) & "." & luna_reg.Month & "." & luna_reg.Year), "yyyy-MM-dd")


        '    Try
        '        If dbconn.State = 0 Then
        '            dbconn.Open()
        '        End If
        '        chelt_tot = FormatNumber(CDec(chelt_tot), 2, TriState.True, TriState.False, TriState.True)
        '        dbcomm = New MySqlCommand(sql, dbconn)
        '        Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
        '        dbcomm.Parameters.AddWithValue("@data", data_ultima)
        '        dbcomm.Parameters.AddWithValue("@casa_sold_initial", sold_initial)
        '        dbcomm.Parameters.AddWithValue("@incasari", incas_tot)
        '        dbcomm.Parameters.AddWithValue("@cheltuieli", chelt_tot)
        '        dbcomm.Parameters.AddWithValue("@casa_sold_final", sold_prec)
        '        dbcomm.Parameters.AddWithValue("@magazin", mag)
        '        'MsgBox(chelt_tot & " " & FormatNumber(CDec(chelt_tot), 2, TriState.True, TriState.False, TriState.True))
        '        dbcomm.ExecuteNonQuery()
        '        transaction.Commit()
        '        dbconn.Close()
        '    Catch ex As Exception
        '        MsgBox("Failed to insert solduri precedente: " & ex.Message.ToString())
        '    End Try
        '    dbread.Close()
        '    dbread3.Close()
        'End If


        'Load_Solduri()
        'Dim mag_id As String = ""
        'If mag = "PM" Then
        '    mag_id = "PM"
        'ElseIf mag = "MV" Then
        '    mag_id = "MV"
        'End If
        ''Dim pdfFilename As String = folder_raport & "Raport Gestiune_" & mag_id & "_" & luna_rap.Year & "_" & Format(luna_rap, "MM") & ".pdf"
        'Dim pdfFilename As String = folder_pdf & "Registru Casa_" & mag_id & "_" & luna_reg.Year & "_" & Format(luna_reg, "MM") & ".pdf"
        'pdf.Save(pdfFilename)
        'Load_Listview_Documente()
        'Process.Start(pdfFilename)

        Form_Registru.Show()
    End Sub
    Private Sub Raport_Button_Click_1(sender As Object, e As EventArgs) Handles Raport_Button.Click

        Form_Raport.Show()
    End Sub


    Private Sub Strip_Solduri_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Strip_Solduri.Opening
        Dim DG As DataGridView = Sold_Casa_DGV
        Dim sold As String = ""
        If Strip_Solduri.SourceControl.Name = Sold_Casa_DGV.Name Then
            DG = Sold_Casa_DGV
            sold = "casa"

        End If
        If Strip_Solduri.SourceControl.Name = Sold_Gestiune_DGV.Name = True Then
            DG = Sold_Gestiune_DGV
            sold = "gestiune"

        End If

        For i = 0 To DG.SelectedRows.Count - 1
            If DG.SelectedRows(i).Cells("permanent").Value = True Then
                ScoatePermanentToolStripMenuItem.Enabled = True
                PunePermanentToolStripMenuItem.Enabled = False
            ElseIf DG.SelectedRows(i).Cells("permanent").Value = False Then
                ScoatePermanentToolStripMenuItem.Enabled = False
                PunePermanentToolStripMenuItem.Enabled = True

            End If
        Next

    End Sub
    Private Sub StregeInregistrareaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StregeInregistrareaToolStripMenuItem.Click
        Dim DG As DataGridView = Sold_Casa_DGV
        Dim sold As String = ""
        If Strip_Solduri.SourceControl.Name = Sold_Casa_DGV.Name Then
            DG = Sold_Casa_DGV
            sold = "casa"

        End If
        If Strip_Solduri.SourceControl.Name = Sold_Gestiune_DGV.Name = True Then
            DG = Sold_Gestiune_DGV
            sold = "gestiune"
        End If

        Dim dbconn As New MySqlConnection
        Dim dbcomm As New MySqlCommand
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        If dbconn.State = 0 Then
            dbconn.Open()
        End If

        Try
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            For i = 0 To DG.SelectedRows.Count - 1
                Dim data As Date = CDate(DG.SelectedRows(i).Cells("data").Value)

                If DG.SelectedRows(i).Cells("permanent").Value = True Then
                    MsgBox("Nu se poate sterge soldul din data: " & DG.SelectedRows(i).Cells("data").Value)
                ElseIf DG.SelectedRows(i).Cells("permanent").Value = False Then
                    Dim sql_del As String = "DELETE FROM solduri_" & sold & " WHERE data=@data AND magazin='" & ComboBox3.SelectedValue & "'"
                    dbcomm = New MySqlCommand(sql_del, dbconn)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.ExecuteNonQuery()
                End If
            Next

            Dim yesno As Integer = MsgBox("Stergi Inregistrarea?", MsgBoxStyle.YesNo)
            If yesno = DialogResult.No Then
            ElseIf yesno = DialogResult.Yes Then
                transaction.Commit()
                Load_Solduri()
            End If
        Catch ex As Exception
            MsgBox("Problem deleting: " & ex.Message.ToString)
        End Try
    End Sub
    Private Sub PunePermanentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PunePermanentToolStripMenuItem.Click
        Dim DG As DataGridView = Sold_Casa_DGV
        Dim sold As String = ""
        If Strip_Solduri.SourceControl.Name = Sold_Casa_DGV.Name Then
            DG = Sold_Casa_DGV
            sold = "casa"

        End If
        If Strip_Solduri.SourceControl.Name = Sold_Gestiune_DGV.Name = True Then
            DG = Sold_Gestiune_DGV
            sold = "gestiune"

        End If

        'Dim yesno As Integer = MsgBox("Stergi Inregistrarea?", MsgBoxStyle.YesNo)


        'If yesno = DialogResult.No Then
        'ElseIf yesno = DialogResult.Yes Then
        Dim dbconn As New MySqlConnection
        Dim dbcomm As New MySqlCommand
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        If dbconn.State = 0 Then
            dbconn.Open()
        End If
        Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
        For i = 0 To DG.SelectedRows.Count - 1
            Try
                Dim data As Date = CDate(DG.SelectedRows(i).Cells("data").Value)

                Dim sql_del As String = "UPDATE solduri_" & sold & " SET permanent=True WHERE data=@data AND magazin='" & ComboBox3.SelectedValue & "'"
                dbcomm = New MySqlCommand(sql_del, dbconn)
                dbcomm.Parameters.AddWithValue("@data", data)
                dbcomm.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox("Nu s-a sters: " & ex.Message.ToString)
            End Try
        Next
        transaction.Commit()
        Load_Solduri()
        'End If
    End Sub
    Private Sub ScoatePermanentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScoatePermanentToolStripMenuItem.Click
        Dim DG As DataGridView = Sold_Casa_DGV
        Dim sold As String = ""
        If Strip_Solduri.SourceControl.Name = Sold_Casa_DGV.Name Then
            DG = Sold_Casa_DGV
            sold = "casa"

        End If
        If Strip_Solduri.SourceControl.Name = Sold_Gestiune_DGV.Name = True Then
            DG = Sold_Gestiune_DGV
            sold = "gestiune"

        End If

        'Dim yesno As Integer = MsgBox("Stergi Inregistrarea?", MsgBoxStyle.YesNo)


        'If yesno = DialogResult.No Then
        'ElseIf yesno = DialogResult.Yes Then
        Dim dbconn As New MySqlConnection
        Dim dbcomm As New MySqlCommand
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        If dbconn.State = 0 Then
            dbconn.Open()
        End If
        Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
        For i = 0 To DG.SelectedRows.Count - 1
            Try
                Dim data As Date = CDate(DG.SelectedRows(i).Cells("data").Value)

                Dim sql_del As String = "UPDATE solduri_" & sold & " SET permanent=False WHERE data=@data AND magazin='" & ComboBox3.SelectedValue & "'"
                dbcomm = New MySqlCommand(sql_del, dbconn)
                dbcomm.Parameters.AddWithValue("@data", data)
                dbcomm.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox("Nu s-a sters: " & ex.Message.ToString)
            End Try
        Next
        transaction.Commit()
        Load_Solduri()
        'End If
    End Sub
    Private Sub casa_verif_button_Click(sender As Object, e As EventArgs) Handles casa_verif_button.Click
        For i = Sold_Casa_DGV.RowCount - 1 To 1 Step -1

            If CDec(Sold_Casa_DGV.Rows(i - 1).Cells("casa_sold_final").Value) <> CDec(Sold_Casa_DGV.Rows(i).Cells("casa_sold_initial").Value) Then
                Dim sold_final_bun As Decimal = CDec(Sold_Casa_DGV.Rows(i).Cells("casa_sold_initial").Value)
                Sold_Casa_DGV.Rows(i - 1).Cells("casa_sold_final").Value = sold_final_bun

                Dim sql As String = "UPDATE solduri_luna SET casa_sold_final=@casa_sold_final WHERE data=@data"
                Try
                    If dbconn.State = 0 Then
                        dbconn.Open()
                    End If
                    dbcomm = New MySqlCommand(sql, dbconn)
                    Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
                    dbcomm.Parameters.AddWithValue("@data", Sold_Casa_DGV.Rows(i - 1).Cells("data").Value)
                    dbcomm.Parameters.AddWithValue("@casa_sold_final", sold_final_bun)
                    dbcomm.ExecuteNonQuery()
                    transaction.Commit()

                Catch ex As Exception
                    MsgBox("Failed to insert data in casa sold final : " & ex.Message.ToString())
                End Try

            End If

            If CDec(Sold_Casa_DGV.Rows(i - 1).Cells("casa_sold_initial").Value + Sold_Casa_DGV.Rows(i - 1).Cells("incasari").Value - Sold_Casa_DGV.Rows(i - 1).Cells("cheltuieli").Value) <> CDec(Sold_Casa_DGV.Rows(i - 1).Cells("casa_sold_final").Value) Then
                Dim sold_initial_bun As Decimal = CDec(Sold_Casa_DGV.Rows(i).Cells("casa_sold_initial").Value) + CDec(Sold_Casa_DGV.Rows(i - 1).Cells("cheltuieli").Value) - CDec(Sold_Casa_DGV.Rows(i - 1).Cells("incasari").Value)
                Sold_Casa_DGV.Rows(i - 1).Cells("casa_sold_initial").Value = sold_initial_bun

                Dim sql2 As String = "UPDATE solduri_luna SET casa_sold_initial=@casa_sold_initial WHERE data=@data"
                Try
                    If dbconn.State = 0 Then
                        dbconn.Open()
                    End If
                    dbcomm = New MySqlCommand(sql2, dbconn)
                    Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
                    dbcomm.Parameters.AddWithValue("@data", Sold_Casa_DGV.Rows(i - 1).Cells("data").Value)
                    dbcomm.Parameters.AddWithValue("@casa_sold_initial", sold_initial_bun)
                    dbcomm.ExecuteNonQuery()
                    transaction.Commit()

                Catch ex As Exception
                    MsgBox("Failed to insert data in gestiune sold initial : " & ex.Message.ToString())
                End Try
            End If
        Next

        Load_Solduri()
    End Sub
    Private Sub check_BU_Click(sender As Object, e As EventArgs) Handles check_casa_BU.Click
        Form_verif_gestiune.ShowDialog()
        If Form_verif_gestiune.DialogResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        Dim mag As String = Form_verif_gestiune.ComboBox3.SelectedValue
        Dim sold_initial As Decimal = CDec(Form_verif_gestiune.TextBox1.Text)
        Dim sold_final As Decimal = 0
        Dim data_sold_pornire As Date = Date.Parse(Date.DaysInMonth(Form_verif_gestiune.ComboBox2.SelectedItem, Form_verif_gestiune.ComboBox1.SelectedItem) & "." & Form_verif_gestiune.ComboBox1.SelectedItem & "." & Form_verif_gestiune.ComboBox2.SelectedItem)
        Dim permanent As Boolean = False

        Dim intai As Date = Date.Parse("01" & "." & Form_verif_gestiune.ComboBox1.SelectedItem & "." & Form_verif_gestiune.ComboBox2.SelectedItem)
        Dim data_sold As Date = Today

        If dbconn.State = ConnectionState.Closed Then
            dbconn.Open()
        End If

        Dim row As Integer = 0
        Dim incasari As Decimal = 0
        Dim cheltuieli As Decimal = 0

        Sold_Casa_DGV.DataSource = Nothing
        Sold_Casa_DGV.Rows.Clear()
        Sold_Casa_DGV.Columns.Clear()


        Dim dataCol As New DataGridViewTextBoxColumn
        dataCol.HeaderText = "data"
        dataCol.Name = "data"
        dataCol.ReadOnly = True

        Dim sold_iniCol As New DataGridViewTextBoxColumn
        sold_iniCol.HeaderText = "sold_initial"
        sold_iniCol.Name = "sold_initial"
        sold_iniCol.ReadOnly = True

        Dim incasCol As New DataGridViewTextBoxColumn
        incasCol.HeaderText = "incasari"
        incasCol.Name = "incasari"
        incasCol.ReadOnly = True

        Dim cheltCol As New DataGridViewTextBoxColumn
        cheltCol.HeaderText = "cheltuieli"
        cheltCol.Name = "cheltuieli"
        cheltCol.ReadOnly = True

        Dim sold_finCol As New DataGridViewTextBoxColumn
        sold_finCol.HeaderText = "sold_final"
        sold_finCol.Name = "sold_final"
        sold_finCol.ReadOnly = True

        Dim magCol As New DataGridViewTextBoxColumn
        magCol.HeaderText = "magazin"
        magCol.Name = "magazin"
        magCol.ReadOnly = True

        Dim modCol As New DataGridViewCheckBoxColumn
        modCol.HeaderText = "edit"
        modCol.Name = "edit"
        modCol.ReadOnly = False

        Dim permCol As New DataGridViewCheckBoxColumn
        permCol.HeaderText = "perm"
        permCol.Name = "perm"
        permCol.ReadOnly = True

        Sold_Casa_DGV.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {dataCol, sold_iniCol, incasCol, cheltCol, sold_finCol, magCol, modCol, permCol})

        While data_sold_pornire <= Today

            Dim sql As String = "SELECT SUM(suma_cash) AS incasari FROM incasari WHERE MONTH(data)=@luna AND YEAR(data)=@an AND magazin=@magazin"
            dbcomm = New MySqlCommand(sql, dbconn)
            dbcomm.Parameters.AddWithValue("@luna", data_sold_pornire.Month)
            dbcomm.Parameters.AddWithValue("@an", data_sold_pornire.Year)
            dbcomm.Parameters.AddWithValue("@magazin", mag)
            data_sold = Date.Parse(Date.DaysInMonth(data_sold_pornire.Year, data_sold_pornire.Month) & "." & data_sold_pornire.Month & "." & data_sold_pornire.Year)

            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = True AndAlso IsDBNull(dbread("incasari")) = False Then
                incasari = dbread("incasari")
                'MsgBox(data_sold_pornire & " " & dbread("incasari").ToString)
            End If
            dbread.Close()

            sql = "SELECT SUM(suma) AS cheltuieli FROM cheltuieli WHERE MONTH(data)=@luna AND YEAR(data)=@an AND magazin=@magazin AND cash=TRUE"
            dbcomm = New MySqlCommand(sql, dbconn)
            dbcomm.Parameters.AddWithValue("@luna", data_sold_pornire.Month)
            dbcomm.Parameters.AddWithValue("@an", data_sold_pornire.Year)
            dbcomm.Parameters.AddWithValue("@magazin", mag)

            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = True AndAlso IsDBNull(dbread("cheltuieli")) = False Then
                cheltuieli = dbread("cheltuieli")
            End If
            dbread.Close()


            sql = "SELECT * FROM solduri_casa WHERE MONTH(data)=@luna AND YEAR(data)=@an AND magazin=@magazin"
            dbcomm = New MySqlCommand(sql, dbconn)
            dbcomm.Parameters.AddWithValue("@luna", data_sold_pornire.Month)
            dbcomm.Parameters.AddWithValue("@an", data_sold_pornire.Year)
            dbcomm.Parameters.AddWithValue("@magazin", mag)
            Dim perma As Boolean = False
            Dim casa_sold_initial As Decimal = 0
            Dim casa_sold_final As Decimal = 0
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = True Then
                perma = dbread("permanent")
                casa_sold_initial = dbread("casa_sold_initial")
                casa_sold_final = dbread("casa_sold_final")
            End If
            dbread.Close()

            If perma = True Then
                sold_initial = casa_sold_initial
                sold_final = casa_sold_final
            ElseIf perma = False Then
                sold_final = sold_initial + incasari - cheltuieli
            End If

            Sold_Casa_DGV.Rows.Add()
            Sold_Casa_DGV.Rows(row).Cells("data").Value = FormatDateTime(data_sold, DateFormat.GeneralDate)
            Sold_Casa_DGV.Rows(row).Cells("sold_initial").Value = sold_initial
            Sold_Casa_DGV.Rows(row).Cells("incasari").Value = incasari
            Sold_Casa_DGV.Rows(row).Cells("cheltuieli").Value = cheltuieli
            Sold_Casa_DGV.Rows(row).Cells("sold_final").Value = sold_final
            Sold_Casa_DGV.Rows(row).Cells("magazin").Value = mag

            sold_initial = sold_final
            row = row + 1
            data_sold_pornire = data_sold_pornire.AddMonths(1)
        End While

        Dim data_s As Date
        Dim s_ini As Decimal = 0
        Dim s_fin As Decimal = 0
        Try
            Dim sql As String = "SELECT data,permanent,casa_sold_initial,casa_sold_final FROM solduri_casa WHERE permanent=TRUE AND magazin=@magazin ORDER BY data DESC LIMIT 1"
            dbcomm = New MySqlCommand(sql, dbconn)
            dbcomm.Parameters.AddWithValue("@magazin", mag)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = True Then
                data_s = dbread("data").ToString
                s_ini = dbread("casa_sold_initial").ToString()
                s_fin = dbread("casa_sold_final").ToString()
            End If

        Catch ex As Exception
            MsgBox("Problem loading toti: " & ex.Message.ToString)
        End Try
        Dim rp As Integer = 0
        For i = 0 To Sold_Casa_DGV.Rows.Count - 1
            If Sold_Casa_DGV.Rows(i).Cells("data").Value = data_s Then
                rp = i
                Exit For
            End If
        Next
        Sold_Casa_DGV.Rows(rp).Cells("sold_initial").Value = s_ini
        For i = Sold_Casa_DGV.Rows(rp - 1).Index To 0 Step -1
            Dim inc As Decimal = Sold_Casa_DGV.Rows(i).Cells("incasari").Value
            Dim che As Decimal = Sold_Casa_DGV.Rows(i).Cells("cheltuieli").Value
            s_fin = Sold_Casa_DGV.Rows(i + 1).Cells("sold_initial").Value
            Sold_Casa_DGV.Rows(i).Cells("sold_final").Value = s_fin
            s_ini = s_fin + che - inc
            Sold_Casa_DGV.Rows(i).Cells("sold_initial").Value = s_ini
        Next
        dbread.Close()
        Try
            For i = 0 To Sold_Casa_DGV.Rows.Count - 1
                Sold_Casa_DGV.Rows(i).Cells("edit").Value = 1

                Dim sql As String = "SELECT * FROM solduri_casa WHERE data=@data AND magazin=@magazin"
                dbcomm = New MySqlCommand(sql, dbconn)
                dbcomm.Parameters.AddWithValue("@data", Format(CDate(Sold_Casa_DGV.Rows(i).Cells("data").Value), "yyyy-MM-dd"))
                dbcomm.Parameters.AddWithValue("@magazin", mag)
                data_sold = Date.Parse(Date.DaysInMonth(data_sold_pornire.Year, data_sold_pornire.Month) & "." & data_sold_pornire.Month & "." & data_sold_pornire.Year)

                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If dbread.HasRows = True Then

                    incasari = dbread("incasari").ToString
                    cheltuieli = dbread("cheltuieli").ToString
                    sold_initial = dbread("casa_sold_initial").ToString
                    sold_final = dbread("casa_sold_final").ToString
                    permanent = CBool(dbread("permanent").ToString)

                    Dim ed As Integer = 0

                    If Sold_Casa_DGV.Rows(i).Cells("sold_initial").Value = sold_initial AndAlso permanent = True Then
                        Sold_Casa_DGV.Rows(i).Cells("sold_initial").Style.BackColor = Color.Green
                    ElseIf Sold_Casa_DGV.Rows(i).Cells("sold_initial").Value = sold_initial Then
                        Sold_Casa_DGV.Rows(i).Cells("sold_initial").Style.BackColor = Color.PaleGreen
                        'DataGridView4.Rows(i).Cells("edit").Value = 0
                        ed = 0
                    ElseIf Sold_Casa_DGV.Rows(i).Cells("sold_initial").Value <> sold_initial AndAlso sold_initial = 0 Then
                        Sold_Casa_DGV.Rows(i).Cells("sold_initial").Style.BackColor = Color.Yellow
                        'DataGridView4.Rows(i).Cells("edit").Value = 1
                        ed = ed + 1
                    ElseIf Sold_Casa_DGV.Rows(i).Cells("sold_initial").Value <> sold_initial Then
                        Sold_Casa_DGV.Rows(i).Cells("sold_initial").Style.BackColor = Color.Red
                        'DataGridView4.Rows(i).Cells("edit").Value = 1
                        ed = ed + 1
                    End If
                    'ed += ed

                    If Sold_Casa_DGV.Rows(i).Cells("incasari").Value = incasari AndAlso permanent = True Then
                        Sold_Casa_DGV.Rows(i).Cells("incasari").Style.BackColor = Color.Green
                    ElseIf Sold_Casa_DGV.Rows(i).Cells("incasari").Value = incasari Then
                        Sold_Casa_DGV.Rows(i).Cells("incasari").Style.BackColor = Color.PaleGreen
                        'DataGridView4.Rows(i).Cells("edit").Value = 0
                        ed = ed + 0
                    ElseIf Sold_Casa_DGV.Rows(i).Cells("incasari").Value <> incasari AndAlso incasari = 0 Then
                        Sold_Casa_DGV.Rows(i).Cells("incasari").Style.BackColor = Color.Yellow
                        'DataGridView4.Rows(i).Cells("edit").Value = 1
                        ed = ed + 1
                    ElseIf Sold_Casa_DGV.Rows(i).Cells("incasari").Value <> incasari Then
                        Sold_Casa_DGV.Rows(i).Cells("incasari").Style.BackColor = Color.Red
                        'DataGridView4.Rows(i).Cells("edit").Value = 1
                        ed = ed + 1
                    End If

                    If Sold_Casa_DGV.Rows(i).Cells("cheltuieli").Value = cheltuieli AndAlso permanent = True Then
                        Sold_Casa_DGV.Rows(i).Cells("cheltuieli").Style.BackColor = Color.Green
                    ElseIf Sold_Casa_DGV.Rows(i).Cells("cheltuieli").Value = cheltuieli Then
                        Sold_Casa_DGV.Rows(i).Cells("cheltuieli").Style.BackColor = Color.PaleGreen
                        'DataGridView4.Rows(i).Cells("edit").Value = 0
                        ed = ed + 0
                    ElseIf Sold_Casa_DGV.Rows(i).Cells("cheltuieli").Value <> cheltuieli AndAlso cheltuieli = 0 Then
                        Sold_Casa_DGV.Rows(i).Cells("cheltuieli").Style.BackColor = Color.Yellow
                        'DataGridView4.Rows(i).Cells("edit").Value = 1
                        ed = ed + 1
                    ElseIf Sold_Casa_DGV.Rows(i).Cells("cheltuieli").Value <> cheltuieli Then
                        Sold_Casa_DGV.Rows(i).Cells("cheltuieli").Style.BackColor = Color.Red
                        'DataGridView4.Rows(i).Cells("edit").Value = 1
                        ed = ed + 1
                    End If

                    If Sold_Casa_DGV.Rows(i).Cells("sold_final").Value = sold_final AndAlso permanent = True Then
                        Sold_Casa_DGV.Rows(i).Cells("sold_final").Style.BackColor = Color.Green
                    ElseIf Sold_Casa_DGV.Rows(i).Cells("sold_final").Value = sold_final Then
                        Sold_Casa_DGV.Rows(i).Cells("sold_final").Style.BackColor = Color.PaleGreen
                        'DataGridView4.Rows(i).Cells("edit").Value = 0
                        ed = ed + 0
                    ElseIf Sold_Casa_DGV.Rows(i).Cells("sold_final").Value <> sold_final Then
                        Sold_Casa_DGV.Rows(i).Cells("sold_final").Style.BackColor = Color.Red
                        'DataGridView4.Rows(i).Cells("edit").Value = 1
                        ed = ed + 1
                    End If
                    If ed = 0 Then
                        Sold_Casa_DGV.Rows(i).Cells("edit").Value = 0
                    ElseIf ed <> 0 Then
                        Sold_Casa_DGV.Rows(i).Cells("edit").Value = 1
                    End If
                    If permanent = True Then
                        Sold_Casa_DGV.Rows(i).Cells("perm").Value = 1
                        Sold_Casa_DGV.Rows(i).Cells("edit").Value = 0
                    End If
                End If

                dbread.Close()
            Next
        Catch ex As Exception
            MsgBox("Problem loading toti: " & ex.Message.ToString)
        End Try
    End Sub
    Private Sub save_BU_Click(sender As Object, e As EventArgs) Handles save_casa_BU.Click
        Dim yesno As Integer = MsgBox("Salvezi soldurile?", MsgBoxStyle.YesNo)
        Dim row As DataGridViewRow = Firme_DGV.CurrentRow
        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then
            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()

            For i = 0 To Sold_Casa_DGV.Rows.Count - 1

                Dim DG As DataGridView = Sold_Casa_DGV

                Dim data As Date = CDate(DG.Rows(i).Cells("data").Value)
                Dim casa_sold_initial As Decimal = CDec(DG.Rows(i).Cells("sold_initial").Value)
                Dim incasari As Decimal = CDec(DG.Rows(i).Cells("incasari").Value)
                Dim cheltuieli As Decimal = CDec(DG.Rows(i).Cells("cheltuieli").Value)
                Dim casa_sold_final As Decimal = CDec(DG.Rows(i).Cells("sold_final").Value)
                Dim permanent As Boolean = DG.Rows(i).Cells("perm").Value
                Dim mag As String = DG.Rows(i).Cells("magazin").Value
                If permanent = False Then

                    Dim sql_ins As String = "INSERT INTO solduri_casa (data,casa_sold_initial,incasari,cheltuieli,casa_sold_final,magazin) " _
                                            & " VALUES(@data,@casa_sold_initial,@incasari,@cheltuieli,@casa_sold_final,@magazin) " _
                                            & "ON DUPLICATE KEY UPDATE casa_sold_initial=@casa_sold_initial,incasari=@incasari,cheltuieli=@cheltuieli,casa_sold_final=@casa_sold_final,magazin=@magazin"

                    dbcomm = New MySqlCommand(sql_ins, dbconn)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@casa_sold_initial", casa_sold_initial)
                    dbcomm.Parameters.AddWithValue("@incasari", incasari)
                    dbcomm.Parameters.AddWithValue("@cheltuieli", cheltuieli)
                    dbcomm.Parameters.AddWithValue("@casa_sold_final", casa_sold_final)
                    dbcomm.Parameters.AddWithValue("@magazin", mag)

                    dbcomm.ExecuteNonQuery()

                End If

            Next
            transaction.Commit()
            dbread.Close()
            dbconn.Close()
        End If
        Load_Solduri()
    End Sub
    Private Sub check_gest_BU_Click(sender As Object, e As EventArgs) Handles check_gest_BU.Click
        Form_verif_gestiune.ShowDialog()
        If Form_verif_gestiune.DialogResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        Dim mag As String = Form_verif_gestiune.ComboBox3.SelectedValue
        Dim sold_initial As Decimal = CDec(Form_verif_gestiune.TextBox1.Text)
        Dim sold_final As Decimal = 0
        Dim data_sold_pornire As Date = Date.Parse(Date.DaysInMonth(Form_verif_gestiune.ComboBox2.SelectedItem, Form_verif_gestiune.ComboBox1.SelectedItem) & "." & Form_verif_gestiune.ComboBox1.SelectedItem & "." & Form_verif_gestiune.ComboBox2.SelectedItem)
        Dim permanent As Boolean = False

        Dim intai As Date = Date.Parse("01" & "." & Form_verif_gestiune.ComboBox1.SelectedItem & "." & Form_verif_gestiune.ComboBox2.SelectedItem)
        Dim data_sold As Date = Today

        If dbconn.State = ConnectionState.Closed Then
            dbconn.Open()
        End If

        Dim row As Integer = 0


        Sold_Gestiune_DGV.DataSource = Nothing
        Sold_Gestiune_DGV.Rows.Clear()
        Sold_Gestiune_DGV.Columns.Clear()


        Dim dataCol As New DataGridViewTextBoxColumn
        dataCol.HeaderText = "data"
        dataCol.Name = "data"
        dataCol.ReadOnly = True

        Dim sold_iniCol As New DataGridViewTextBoxColumn
        sold_iniCol.HeaderText = "sold_initial"
        sold_iniCol.Name = "sold_initial"
        sold_iniCol.ReadOnly = True

        Dim iesCol As New DataGridViewTextBoxColumn
        iesCol.HeaderText = "iesiri"
        iesCol.Name = "iesiri"
        iesCol.ReadOnly = True

        Dim intrCol As New DataGridViewTextBoxColumn
        intrCol.HeaderText = "intrari"
        intrCol.Name = "intrari"
        intrCol.ReadOnly = True

        Dim sold_finCol As New DataGridViewTextBoxColumn
        sold_finCol.HeaderText = "sold_final"
        sold_finCol.Name = "sold_final"
        sold_finCol.ReadOnly = True

        Dim magCol As New DataGridViewTextBoxColumn
        magCol.HeaderText = "magazin"
        magCol.Name = "magazin"
        magCol.ReadOnly = True

        Dim modCol As New DataGridViewCheckBoxColumn
        modCol.HeaderText = "edit"
        modCol.Name = "edit"
        modCol.ReadOnly = False

        Dim permCol As New DataGridViewCheckBoxColumn
        permCol.HeaderText = "perm"
        permCol.Name = "perm"
        permCol.ReadOnly = True

        Sold_Gestiune_DGV.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {dataCol, sold_iniCol, intrCol, iesCol, sold_finCol, magCol, modCol, permCol})

        While data_sold_pornire <= Today
            Dim iesiri As Decimal = 0
            Dim intrari As Decimal = 0
            Dim sql As String = "SELECT SUM(suma) AS iesiri FROM iesiri WHERE MONTH(data)=@luna AND YEAR(data)=@an AND magazin=@magazin"
            dbcomm = New MySqlCommand(sql, dbconn)
            dbcomm.Parameters.AddWithValue("@luna", data_sold_pornire.Month)
            dbcomm.Parameters.AddWithValue("@an", data_sold_pornire.Year)
            dbcomm.Parameters.AddWithValue("@magazin", mag)
            data_sold = Date.Parse(Date.DaysInMonth(data_sold_pornire.Year, data_sold_pornire.Month) & "." & data_sold_pornire.Month & "." & data_sold_pornire.Year)

            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = True AndAlso IsDBNull(dbread("iesiri")) = False Then
                iesiri = dbread("iesiri")
                'MsgBox(data_sold_pornire & " " & dbread("incasari").ToString)
            End If
            dbread.Close()

            sql = "SELECT SUM(suma) AS intrari FROM intrari WHERE MONTH(data)=@luna AND YEAR(data)=@an AND magazin=@magazin"
            dbcomm = New MySqlCommand(sql, dbconn)
            dbcomm.Parameters.AddWithValue("@luna", data_sold_pornire.Month)
            dbcomm.Parameters.AddWithValue("@an", data_sold_pornire.Year)
            dbcomm.Parameters.AddWithValue("@magazin", mag)

            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = True AndAlso IsDBNull(dbread("intrari")) = False Then
                intrari = dbread("intrari")
            End If
            dbread.Close()


            sql = "SELECT * FROM solduri_gestiune WHERE MONTH(data)=@luna AND YEAR(data)=@an AND magazin=@magazin"
            dbcomm = New MySqlCommand(sql, dbconn)
            dbcomm.Parameters.AddWithValue("@luna", data_sold_pornire.Month)
            dbcomm.Parameters.AddWithValue("@an", data_sold_pornire.Year)
            dbcomm.Parameters.AddWithValue("@magazin", mag)
            Dim perma As Boolean = False
            Dim gestiune_sold_initial As Decimal = 0
            Dim gestiune_sold_final As Decimal = 0
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = True Then
                perma = dbread("permanent")
                gestiune_sold_initial = dbread("gestiune_sold_initial")
                gestiune_sold_final = dbread("gestiune_sold_final")
            End If
            dbread.Close()

            If perma = True Then
                sold_initial = gestiune_sold_initial
                sold_final = gestiune_sold_final
            ElseIf perma = False Then
                sold_final = sold_initial + intrari - iesiri
            End If

            Sold_Gestiune_DGV.Rows.Add()
            Sold_Gestiune_DGV.Rows(row).Cells("data").Value = FormatDateTime(data_sold, DateFormat.GeneralDate)
            Sold_Gestiune_DGV.Rows(row).Cells("sold_initial").Value = sold_initial
            Sold_Gestiune_DGV.Rows(row).Cells("iesiri").Value = iesiri
            Sold_Gestiune_DGV.Rows(row).Cells("intrari").Value = intrari
            Sold_Gestiune_DGV.Rows(row).Cells("sold_final").Value = sold_final
            Sold_Gestiune_DGV.Rows(row).Cells("magazin").Value = mag

            sold_initial = sold_final
            row = row + 1
            data_sold_pornire = data_sold_pornire.AddMonths(1)
        End While

        Dim data_s As Date
        Dim s_ini As Decimal = 0
        Dim s_fin As Decimal = 0
        Try
            Dim sql As String = "SELECT data,permanent,gestiune_sold_initial,gestiune_sold_final FROM solduri_gestiune WHERE permanent=TRUE AND magazin=@magazin ORDER BY data DESC LIMIT 1"
            dbcomm = New MySqlCommand(sql, dbconn)
            dbcomm.Parameters.AddWithValue("@magazin", mag)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = True Then
                data_s = dbread("data").ToString
                s_ini = dbread("gestiune_sold_initial").ToString()
                s_fin = dbread("gestiune_sold_final").ToString()
            End If

        Catch ex As Exception
            MsgBox("Problem loading toti: " & ex.Message.ToString)
        End Try
        Dim rp As Integer = 0
        For i = 0 To Sold_Gestiune_DGV.Rows.Count - 1
            If Sold_Gestiune_DGV.Rows(i).Cells("data").Value = data_s Then
                rp = i
                Exit For
            End If
        Next
        Sold_Gestiune_DGV.Rows(rp).Cells("sold_initial").Value = s_ini
        For i = Sold_Gestiune_DGV.Rows(rp - 1).Index To 0 Step -1
            Dim ies As Decimal = Sold_Gestiune_DGV.Rows(i).Cells("iesiri").Value
            Dim int As Decimal = Sold_Gestiune_DGV.Rows(i).Cells("intrari").Value
            s_fin = Sold_Gestiune_DGV.Rows(i + 1).Cells("sold_initial").Value
            Sold_Gestiune_DGV.Rows(i).Cells("sold_final").Value = s_fin
            s_ini = s_fin + ies - int
            Sold_Gestiune_DGV.Rows(i).Cells("sold_initial").Value = s_ini
        Next
        dbread.Close()
        Try
            Dim iesiri As Decimal = 0
            Dim intrari As Decimal = 0
            For i = 0 To Sold_Gestiune_DGV.Rows.Count - 1
                Sold_Gestiune_DGV.Rows(i).Cells("edit").Value = 1

                Dim sql As String = "SELECT * FROM solduri_gestiune WHERE data=@data AND magazin=@magazin"
                dbcomm = New MySqlCommand(sql, dbconn)
                dbcomm.Parameters.AddWithValue("@data", Format(CDate(Sold_Gestiune_DGV.Rows(i).Cells("data").Value), "yyyy-MM-dd"))
                dbcomm.Parameters.AddWithValue("@magazin", mag)
                data_sold = Date.Parse(Date.DaysInMonth(data_sold_pornire.Year, data_sold_pornire.Month) & "." & data_sold_pornire.Month & "." & data_sold_pornire.Year)

                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If dbread.HasRows = True Then

                    iesiri = dbread("iesiri").ToString
                    intrari = dbread("intrari").ToString
                    sold_initial = dbread("gestiune_sold_initial").ToString
                    sold_final = dbread("gestiune_sold_final").ToString
                    permanent = CBool(dbread("permanent").ToString)

                    Dim ed As Integer = 0

                    If Sold_Gestiune_DGV.Rows(i).Cells("sold_initial").Value = sold_initial AndAlso permanent = True Then
                        Sold_Gestiune_DGV.Rows(i).Cells("sold_initial").Style.BackColor = Color.Green
                    ElseIf Sold_Gestiune_DGV.Rows(i).Cells("sold_initial").Value = sold_initial Then
                        Sold_Gestiune_DGV.Rows(i).Cells("sold_initial").Style.BackColor = Color.PaleGreen
                        'datagridview7.Rows(i).Cells("edit").Value = 0
                        ed = 0
                    ElseIf Sold_Gestiune_DGV.Rows(i).Cells("sold_initial").Value <> sold_initial AndAlso sold_initial = 0 Then
                        Sold_Gestiune_DGV.Rows(i).Cells("sold_initial").Style.BackColor = Color.Yellow
                        'datagridview7.Rows(i).Cells("edit").Value = 1
                        ed = ed + 1
                    ElseIf Sold_Gestiune_DGV.Rows(i).Cells("sold_initial").Value <> sold_initial Then
                        Sold_Gestiune_DGV.Rows(i).Cells("sold_initial").Style.BackColor = Color.Red
                        'datagridview7.Rows(i).Cells("edit").Value = 1
                        ed = ed + 1
                    End If
                    'ed += ed

                    If Sold_Gestiune_DGV.Rows(i).Cells("iesiri").Value = iesiri AndAlso permanent = True Then
                        Sold_Gestiune_DGV.Rows(i).Cells("iesiri").Style.BackColor = Color.Green
                    ElseIf Sold_Gestiune_DGV.Rows(i).Cells("iesiri").Value = iesiri Then
                        Sold_Gestiune_DGV.Rows(i).Cells("iesiri").Style.BackColor = Color.PaleGreen
                        'datagridview7.Rows(i).Cells("edit").Value = 0
                        ed = ed + 0
                    ElseIf Sold_Gestiune_DGV.Rows(i).Cells("iesiri").Value <> iesiri AndAlso iesiri = 0 Then
                        Sold_Gestiune_DGV.Rows(i).Cells("iesiri").Style.BackColor = Color.Yellow
                        'datagridview7.Rows(i).Cells("edit").Value = 1
                        ed = ed + 1
                    ElseIf Sold_Gestiune_DGV.Rows(i).Cells("iesiri").Value <> iesiri Then
                        Sold_Gestiune_DGV.Rows(i).Cells("iesiri").Style.BackColor = Color.Red
                        'datagridview7.Rows(i).Cells("edit").Value = 1
                        ed = ed + 1
                    End If

                    If Sold_Gestiune_DGV.Rows(i).Cells("intrari").Value = intrari AndAlso permanent = True Then
                        Sold_Gestiune_DGV.Rows(i).Cells("intrari").Style.BackColor = Color.Green
                    ElseIf Sold_Gestiune_DGV.Rows(i).Cells("intrari").Value = intrari Then
                        Sold_Gestiune_DGV.Rows(i).Cells("intrari").Style.BackColor = Color.PaleGreen
                        'datagridview7.Rows(i).Cells("edit").Value = 0
                        ed = ed + 0
                    ElseIf Sold_Gestiune_DGV.Rows(i).Cells("intrari").Value <> intrari AndAlso intrari = 0 Then
                        Sold_Gestiune_DGV.Rows(i).Cells("intrari").Style.BackColor = Color.Yellow
                        'datagridview7.Rows(i).Cells("edit").Value = 1
                        ed = ed + 1
                    ElseIf Sold_Gestiune_DGV.Rows(i).Cells("intrari").Value <> intrari Then
                        Sold_Gestiune_DGV.Rows(i).Cells("intrari").Style.BackColor = Color.Red
                        'datagridview7.Rows(i).Cells("edit").Value = 1
                        ed = ed + 1
                    End If

                    If Sold_Gestiune_DGV.Rows(i).Cells("sold_final").Value = sold_final AndAlso permanent = True Then
                        Sold_Gestiune_DGV.Rows(i).Cells("sold_final").Style.BackColor = Color.Green
                    ElseIf Sold_Gestiune_DGV.Rows(i).Cells("sold_final").Value = sold_final Then
                        Sold_Gestiune_DGV.Rows(i).Cells("sold_final").Style.BackColor = Color.PaleGreen
                        'datagridview7.Rows(i).Cells("edit").Value = 0
                        ed = ed + 0
                    ElseIf Sold_Gestiune_DGV.Rows(i).Cells("sold_final").Value <> sold_final Then
                        Sold_Gestiune_DGV.Rows(i).Cells("sold_final").Style.BackColor = Color.Red
                        'datagridview7.Rows(i).Cells("edit").Value = 1
                        ed = ed + 1
                    End If
                    If ed = 0 Then
                        Sold_Gestiune_DGV.Rows(i).Cells("edit").Value = 0
                    ElseIf ed <> 0 Then
                        Sold_Gestiune_DGV.Rows(i).Cells("edit").Value = 1
                    End If
                    If permanent = True Then
                        Sold_Gestiune_DGV.Rows(i).Cells("perm").Value = 1
                        Sold_Gestiune_DGV.Rows(i).Cells("edit").Value = 0
                    End If
                End If

                dbread.Close()
            Next
        Catch ex As Exception
            MsgBox("Problem loading toti: " & ex.Message.ToString)
        End Try
    End Sub
    Private Sub gestiune_verif_button_Click(sender As Object, e As EventArgs) Handles gestiune_verif_button.Click
        For i = Sold_Casa_DGV.RowCount - 1 To 1 Step -1

            If CDec(Sold_Casa_DGV.Rows(i - 1).Cells("gestiune_sold_final").Value) <> CDec(Sold_Casa_DGV.Rows(i).Cells("gestiune_sold_initial").Value) Then
                Dim sold_final_bun As Decimal = CDec(Sold_Casa_DGV.Rows(i).Cells("gestiune_sold_initial").Value)
                Sold_Casa_DGV.Rows(i - 1).Cells("gestiune_sold_final").Value = sold_final_bun

                Dim sql As String = "UPDATE solduri_luna SET gestiune_sold_final=@gestiune_sold_final WHERE data=@data"
                Try
                    If dbconn.State = 0 Then
                        dbconn.Open()
                    End If
                    dbcomm = New MySqlCommand(sql, dbconn)
                    Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
                    dbcomm.Parameters.AddWithValue("@data", Sold_Casa_DGV.Rows(i - 1).Cells("data").Value)
                    dbcomm.Parameters.AddWithValue("@gestiune_sold_final", sold_final_bun)
                    dbcomm.ExecuteNonQuery()
                    transaction.Commit()

                Catch ex As Exception
                    MsgBox("Failed to insert data in gestiune sold final : " & ex.Message.ToString())
                End Try
            End If


            If CDec(Sold_Casa_DGV.Rows(i - 1).Cells("gestiune_sold_initial").Value + Sold_Casa_DGV.Rows(i - 1).Cells("intrari").Value - Sold_Casa_DGV.Rows(i - 1).Cells("iesiri").Value) <> CDec(Sold_Casa_DGV.Rows(i - 1).Cells("gestiune_sold_final").Value) Then
                Dim sold_initial_bun As Decimal = CDec(Sold_Casa_DGV.Rows(i).Cells("gestiune_sold_initial").Value) + CDec(Sold_Casa_DGV.Rows(i - 1).Cells("iesiri").Value) - CDec(Sold_Casa_DGV.Rows(i - 1).Cells("intrari").Value)
                Sold_Casa_DGV.Rows(i - 1).Cells("gestiune_sold_initial").Value = sold_initial_bun

                Dim sql2 As String = "UPDATE solduri_luna SET gestiune_sold_initial=@gestiune_sold_initial WHERE data=@data"
                Try
                    If dbconn.State = 0 Then
                        dbconn.Open()
                    End If
                    dbcomm = New MySqlCommand(sql2, dbconn)
                    Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
                    dbcomm.Parameters.AddWithValue("@data", Sold_Casa_DGV.Rows(i - 1).Cells("data").Value)
                    dbcomm.Parameters.AddWithValue("@gestiune_sold_initial", sold_initial_bun)
                    dbcomm.ExecuteNonQuery()
                    transaction.Commit()

                Catch ex As Exception
                    MsgBox("Failed to insert data in gestiune sold initial : " & ex.Message.ToString())
                End Try

            End If
        Next

        Load_Solduri()
    End Sub
    Private Sub save_gest_BU_Click(sender As Object, e As EventArgs) Handles save_gest_BU.Click
        Dim yesno As Integer = MsgBox("Salvezi soldurile?", MsgBoxStyle.YesNo)
        Dim row As DataGridViewRow = Firme_DGV.CurrentRow
        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then
            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()

            For i = 0 To Sold_Gestiune_DGV.Rows.Count - 1

                Dim DG As DataGridView = Sold_Gestiune_DGV

                Dim data As Date = CDate(DG.Rows(i).Cells("data").Value)
                Dim gestiune_sold_initial As Decimal = CDec(DG.Rows(i).Cells("sold_initial").Value)
                Dim iesiri As Decimal = CDec(DG.Rows(i).Cells("iesiri").Value)
                Dim intrari As Decimal = CDec(DG.Rows(i).Cells("intrari").Value)
                Dim gestiune_sold_final As Decimal = CDec(DG.Rows(i).Cells("sold_final").Value)
                Dim permanent As Boolean = DG.Rows(i).Cells("perm").Value
                Dim mag As String = DG.Rows(i).Cells("magazin").Value
                If permanent = False Then

                    Dim sql_ins As String = "INSERT INTO solduri_gestiune (data,gestiune_sold_initial,iesiri,intrari,gestiune_sold_final,magazin) " _
                                            & " VALUES(@data,@gestiune_sold_initial,@iesiri,@intrari,@gestiune_sold_final,@magazin) " _
                                            & "ON DUPLICATE KEY UPDATE gestiune_sold_initial=@gestiune_sold_initial,iesiri=@iesiri,intrari=@intrari,gestiune_sold_final=@gestiune_sold_final,magazin=@magazin"

                    dbcomm = New MySqlCommand(sql_ins, dbconn)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@gestiune_sold_initial", gestiune_sold_initial)
                    dbcomm.Parameters.AddWithValue("@iesiri", iesiri)
                    dbcomm.Parameters.AddWithValue("@intrari", intrari)
                    dbcomm.Parameters.AddWithValue("@gestiune_sold_final", gestiune_sold_final)
                    dbcomm.Parameters.AddWithValue("@magazin", mag)

                    dbcomm.ExecuteNonQuery()

                End If

            Next
            transaction.Commit()
            dbread.Close()
            dbconn.Close()
        End If
        Load_Solduri()
    End Sub


    Private Sub _StergeDocumentToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StergeDocumentulToolStripMenuItem.Click
        Dim LV As ListView = Registru_Listview
        If Strip_Documente.SourceControl.Name = Registru_Listview.Name Then
            LV = Registru_Listview
        End If
        If Strip_Documente.SourceControl.Name = Raport_Listview.Name Then
            LV = Raport_Listview
        End If
        If Strip_Documente.SourceControl.Name = Nir_Listview.Name Then
            LV = Nir_Listview
        End If
        If Strip_Documente.SourceControl.Name = DP_Listview.Name Then
            LV = DP_Listview
        End If
        If Strip_Documente.SourceControl.Name = DI_Listview.Name Then
            LV = DI_Listview
        End If
        If Strip_Documente.SourceControl.Name = Facturi_ListView.Name Then
            LV = Facturi_ListView
        End If


        Dim yesno As Integer = MsgBox("Stergi Documentul?", MsgBoxStyle.YesNo)

        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then
            Dim p As Integer = LV.Items(0).SubItems.Count - 1
            For i = 0 To LV.SelectedItems.Count - 1
                Dim file As String = LV.SelectedItems.Item(i).SubItems.Item(p).Text
                Try
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(file)

                Catch ex As Exception
                    MsgBox("Nu s-a sters: " & ex.Message.ToString)
                End Try
            Next
            If LV.Name = Registru_Listview.Name Then
                Load_Registru_Listview()
            ElseIf LV.Name = Raport_Listview.Name Then
                Load_Raport_Listview()
            ElseIf LV.Name = DI_Listview.Name Then
                Load_DI_Listview()
            ElseIf LV.Name = DP_Listview.Name Then
                Load_DP_Listview()
            ElseIf LV.Name = Nir_Listview.Name Then
                Load_Nir_Listview()
            ElseIf LV.Name = Facturi_ListView.Name Then
                Load_Facturi_Listview()
            End If

        End If
    End Sub
    Private Sub _ArataInFolderToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ArataInFolderToolStripMenuItem1.Click


        Dim LV As ListView = Registru_Listview
        If Strip_Documente.SourceControl.Name = Registru_Listview.Name Then
            LV = Registru_Listview
        End If
        If Strip_Documente.SourceControl.Name = Raport_Listview.Name Then
            LV = Raport_Listview
        End If
        If Strip_Documente.SourceControl.Name = Nir_Listview.Name Then
            LV = Nir_Listview
        End If
        If Strip_Documente.SourceControl.Name = DP_Listview.Name Then
            LV = DP_Listview
        End If
        If Strip_Documente.SourceControl.Name = DI_Listview.Name Then
            LV = DI_Listview
        End If
        If Strip_Documente.SourceControl.Name = Facturi_ListView.Name Then
            LV = Facturi_ListView
        End If

        Dim p As Integer = LV.Items(0).SubItems.Count - 1
        Dim file As String = LV.SelectedItems.Item(0).SubItems.Item(p).Text
        Try
            Process.Start("explorer.exe", "/select," & file)
        Catch ex As Exception
            MsgBox("Eroare: " & ex.Message.ToString)
        End Try
    End Sub
    Private Sub _Listview_MouseDoubleClick(sender As Object, e As EventArgs) Handles Registru_Listview.MouseDoubleClick, Raport_Listview.MouseDoubleClick, DI_Listview.MouseDoubleClick, DP_Listview.MouseDoubleClick, Nir_Listview.MouseDoubleClick, Facturi_ListView.MouseDoubleClick

        Dim LV As ListView = sender
        Dim p As Integer = LV.Items(0).SubItems.Count - 1
        Dim file As String = LV.SelectedItems.Item(0).SubItems.Item(p).Text

        Process.Start(file)


    End Sub


    Private Sub ad_prod_But_Click(sender As Object, e As EventArgs) Handles ad_prod_But.Click, Button6.Click
        Form_NIRuri.Show()

    End Sub
    Private Sub cauta_prod_TB_TextChanged(sender As Object, e As EventArgs) Handles cauta_prod_TB.TextChanged
        Load_Marfa()
    End Sub
    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) 'Handles CheckBox2.CheckedChanged
        Dim selected As DataGridViewSelectedRowCollection = Marfa_DGV.SelectedRows
        Dim prod As String = ""
        Dim pret As String = ""
        Dim dif As Integer = 0
        If selected.Count > 0 Then
            prod = selected(0).Cells("produs").Value
            pret = selected(0).Cells("pret").Value
            dif = selected(0).Index - Marfa_DGV.FirstDisplayedScrollingRowIndex
        End If
        Load_Marfa()

        For i = 0 To Marfa_DGV.Rows.Count - 1
            If Marfa_DGV.Rows(i).Cells("produs").Value.ToString = prod AndAlso Marfa_DGV.Rows(i).Cells("pret").Value.ToString = pret Then
                Marfa_DGV.Rows(i).Selected = True
                If i > 0 AndAlso i - dif > 0 Then
                    Marfa_DGV.FirstDisplayedScrollingRowIndex = i - dif
                Else
                    Marfa_DGV.FirstDisplayedScrollingRowIndex = i
                End If
                Exit For
            End If
        Next

    End Sub
    Private Sub Marfa_DGV_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Marfa_DGV.CellMouseDoubleClick
        If e.Button = Windows.Forms.MouseButtons.Right And e.ColumnIndex > -1 And e.RowIndex > -1 Then
            Dim cell As DataGridViewCell = Marfa_DGV.Rows(e.RowIndex).Cells("produs")
            Dim prod As String = cell.Value

            cauta_prod_TB.Text = prod
            Load_Marfa()
            cauta_prod_TB.Focus()
        End If
    End Sub
    Private Sub produse_list_BU_Click(sender As Object, e As EventArgs) Handles produse_list_BU.Click

        Dim pdf As PdfSharp.Pdf.PdfDocument = New PdfSharp.Pdf.PdfDocument

        pdf.Info.Title = "Marfa"
        Dim pdfPage As PdfSharp.Pdf.PdfPage = pdf.AddPage

        Dim graph As XGraphics = XGraphics.FromPdfPage(pdfPage)
        Dim pen As XPen = New XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5)

        Dim titlu_font As XFont = New XFont("Verdana", 16, XFontStyle.Bold)
        Dim top_font As XFont = New XFont("Verdana", 12, XFontStyle.Regular)
        Dim report_font As XFont = New XFont("Verdana", 10, XFontStyle.Bold)
        Dim report_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim text_font As XFont = New XFont("Verdana", 10, XFontStyle.Regular)
        Dim text_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim bottom_font As XFont = New XFont("Verdana", 12, XFontStyle.Bold)

        'A4 = 8.27x11.69" x72points/inch = 595x842 points

        '-----------------HEADER ----------------------------


        graph.DrawLine(pen, 20, 18, 401, 18)
        graph.DrawLine(pen, 20, 37, 401, 37)


        graph.DrawString("Produs", top_font, XBrushes.Black,
                         New XRect(20, 20, 140, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawString("Pret".ToString, top_font, XBrushes.Black,
                         New XRect(160, 20, 50, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawString("Bucati".ToString, top_font, XBrushes.Black,
                         New XRect(210, 20, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("Luna".ToString, top_font, XBrushes.Black,
                         New XRect(260, 20, 100, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("An".ToString, top_font, XBrushes.Black,
                         New XRect(360, 20, 40, pdfPage.Height.Point), XStringFormats.TopCenter)


        graph.DrawLine(pen, 20, 18, 20, 800)
        graph.DrawLine(pen, 401, 18, 401, 800)
        '^^^-----------------HEADER ---------------------------^^^
        graph.DrawLine(pen, 20, 800, 401, 800)
        Dim nxtRow As Double = 23

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Try
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If

                Dim sda As New MySqlDataAdapter
                Dim dbtable As New DataTable
                Dim bsource As New BindingSource
                Dim grup As String = ""
                Dim buc As String = ""
                Dim dat As String = ""

                Dim tot_sql As String = "SELECT produs, COUNT(produs) AS nr_prod FROM marfa GROUP BY produs ORDER BY produs ASC"
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                sda.SelectCommand = dbcomm
                sda.Fill(dbtable)
                bsource.DataSource = dbtable
                sda.Update(dbtable)

                dbread.Close()

                If CheckBox2.CheckState = CheckState.Unchecked Then
                    grup = ""
                    buc = "bucati"
                    dat = "MONTH(data) as luna, YEAR(data) AS an"
                ElseIf CheckBox2.CheckState = CheckState.Checked Then
                    grup = "GROUP BY produs,pret"
                    buc = "SUM(bucati) AS bucati"
                    dat = "MONTH(MAX(data)) AS luna, YEAR(MAX(data)) AS an"
                End If

                nxtRow = nxtRow + 20
                Dim nr2 As Integer = 0

                For i = 0 To dbtable.Rows.Count - 1
                    Dim produs As String = dbtable.Rows(i).Item("produs").ToString()

                    Dim sql As String = "SELECT produs, pret, " & buc & ", " & dat & " FROM marfa WHERE produs=@produs " & grup & " ORDER BY pret ASC, YEAR(data) DESC, MONTH(data) DESC"
                    dbcomm = New MySqlCommand(sql, dbconn)
                    dbcomm.Parameters.AddWithValue("@produs", produs)
                    dbread = dbcomm.ExecuteReader()

                    If dbread.HasRows = True Then
                        Dim nr As Integer = 0
                        While dbread.Read
                            Dim pret As String = dbread("pret")
                            Dim bucati As String = dbread("bucati")
                            Dim luna As String = Format(CDate("01." & dbread("luna") & ".2015"), "MMMM")
                            Dim an As String = dbread("an")

                            Dim tf As XTextFormatter = New XTextFormatter(graph)
                            tf.Alignment = XParagraphAlignment.Right

                            If nr2 Mod 2 = 0 Then
                                graph.DrawRectangle(XBrushes.LightGray, 161, nxtRow - 5, 239, 19)

                            End If
                            If nr < 1 Then
                                graph.DrawString(produs, text_font, XBrushes.Black,
                                                 New XRect(20, nxtRow, 140, pdfPage.Height.Point), XStringFormats.TopCenter)
                            End If
                            graph.DrawString(pret.ToString, text_font, XBrushes.Black,
                                             New XRect(160, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)

                            graph.DrawString(bucati.ToString, text_font, XBrushes.Black,
                                             New XRect(210, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString(luna.ToString, text_font, XBrushes.Black,
                                             New XRect(260, nxtRow, 100, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString(an.ToString, text_font, XBrushes.Black,
                                             New XRect(360, nxtRow, 40, pdfPage.Height.Point), XStringFormats.TopCenter)

                            nxtRow = nxtRow + 20
                            nr = nr + 1
                            nr2 = nr2 + 1
                            If nxtRow > 800 Then
                                pdfPage = pdf.AddPage()
                                graph = XGraphics.FromPdfPage(pdfPage)
                                tf = New XTextFormatter(graph)
                                tf.Alignment = XParagraphAlignment.Right
                                '-----------------HEADER ----------------------------

                                graph.DrawLine(pen, 20, 18, 401, 18)
                                graph.DrawLine(pen, 20, 37, 401, 37)


                                graph.DrawString("Produs", top_font, XBrushes.Black,
                                                 New XRect(20, 20, 140, pdfPage.Height.Point), XStringFormats.TopCenter)

                                graph.DrawString("Pret".ToString, top_font, XBrushes.Black,
                                                 New XRect(160, 20, 50, pdfPage.Height.Point), XStringFormats.TopCenter)

                                graph.DrawString("Bucati".ToString, top_font, XBrushes.Black,
                                                 New XRect(210, 20, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
                                graph.DrawString("Luna".ToString, top_font, XBrushes.Black,
                                                 New XRect(260, 20, 100, pdfPage.Height.Point), XStringFormats.TopCenter)
                                graph.DrawString("An".ToString, top_font, XBrushes.Black,
                                                 New XRect(360, 20, 40, pdfPage.Height.Point), XStringFormats.TopCenter)


                                graph.DrawLine(pen, 20, 18, 20, 800)
                                graph.DrawLine(pen, 401, 18, 401, 800)
                                '^^^-----------------HEADER ---------------------------^^^
                                graph.DrawLine(pen, 20, 800, 401, 800)
                                nxtRow = 43
                                nr = 0
                            End If
                        End While

                    End If
                    dbread.Close()
                    graph.DrawLine(pen, 20, nxtRow - 6, 401, nxtRow - 6)
                Next
            Catch ex As Exception
                MsgBox("Problem loading data: " & ex.Message.ToString)
            End Try

        End Using
        Dim pdfFilename As String = FileIO.SpecialDirectories.Desktop & "\Marfa.pdf"
        pdf.Save(pdfFilename)
        Process.Start(pdfFilename)
    End Sub
    Private Sub inventar_BU_Click(sender As Object, e As EventArgs) Handles inventar_BU.Click
        Dim pdf As PdfSharp.Pdf.PdfDocument = New PdfSharp.Pdf.PdfDocument

        pdf.Info.Title = "Inventar"
        Dim pdfPage As PdfSharp.Pdf.PdfPage = pdf.AddPage

        Dim graph As XGraphics = XGraphics.FromPdfPage(pdfPage)
        Dim pen As XPen = New XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5)

        Dim titlu_font As XFont = New XFont("Verdana", 16, XFontStyle.Bold)
        Dim top_font As XFont = New XFont("Verdana", 12, XFontStyle.Regular)
        Dim report_font As XFont = New XFont("Verdana", 10, XFontStyle.Bold)
        Dim report_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim text_font As XFont = New XFont("Verdana", 10, XFontStyle.Regular)
        Dim text_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim bottom_font As XFont = New XFont("Verdana", 12, XFontStyle.Bold)

        'A4 = 8.27x11.69" x72points/inch = 595x842 points

        '-----------------HEADER ----------------------------


        graph.DrawLine(pen, 20, 18, 401, 18)
        graph.DrawLine(pen, 20, 37, 401, 37)


        graph.DrawString("Produs", top_font, XBrushes.Black,
                         New XRect(20, 20, 140, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawString("Pret".ToString, top_font, XBrushes.Black,
                         New XRect(160, 20, 50, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawString("Bucati".ToString, top_font, XBrushes.Black,
                         New XRect(210, 20, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("Luna".ToString, top_font, XBrushes.Black,
                         New XRect(260, 20, 100, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("An".ToString, top_font, XBrushes.Black,
                         New XRect(360, 20, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("Mag. Petru Maior 9".ToString, bottom_font, XBrushes.Black,
                         New XRect(400, 20, 150, pdfPage.Height.Point), XStringFormats.TopCenter)


        graph.DrawLine(pen, 20, 18, 20, 800)
        graph.DrawLine(pen, 401, 18, 401, 800)
        '^^^-----------------HEADER ---------------------------^^^
        graph.DrawLine(pen, 20, 800, 401, 800)
        Dim nxtRow As Double = 23

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Try
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If

                Dim sda As New MySqlDataAdapter
                Dim dbtable As New DataTable
                Dim bsource As New BindingSource
                Dim grup As String = ""
                Dim buc As String = ""
                Dim dat As String = ""

                Dim tot_sql As String = "SELECT produs, COUNT(produs) AS nr_prod FROM marfa GROUP BY produs ORDER BY produs ASC"
                dbcomm = New MySqlCommand(tot_sql, dbconn)
                sda.SelectCommand = dbcomm
                sda.Fill(dbtable)
                bsource.DataSource = dbtable
                sda.Update(dbtable)

                dbread.Close()

                If CheckBox2.CheckState = CheckState.Unchecked Then
                    grup = ""
                    buc = "bucati"
                    dat = "MONTH(data) as luna, YEAR(data) AS an"
                ElseIf CheckBox2.CheckState = CheckState.Checked Then
                    grup = "GROUP BY produs,pret"
                    buc = "SUM(bucati) AS bucati"
                    dat = "MONTH(MAX(data)) AS luna, YEAR(MAX(data)) AS an"
                End If

                nxtRow = nxtRow + 20
                Dim nr2 As Integer = 0
                Dim sum_tot As Integer = 0
                For i = 0 To dbtable.Rows.Count - 1
                    Dim produs As String = dbtable.Rows(i).Item("produs").ToString()

                    Dim sql As String = "SELECT produs, pret, " & buc & ", " & dat & " FROM marfa WHERE produs=@produs AND magazin='PM' AND YEAR(data)=2021 " & grup & " ORDER BY pret ASC, YEAR(data) DESC, MONTH(data) DESC"
                    dbcomm = New MySqlCommand(sql, dbconn)
                    dbcomm.Parameters.AddWithValue("@produs", produs)
                    dbread = dbcomm.ExecuteReader()
                    Dim sum_prod As Integer = 0
                    If dbread.HasRows = True Then
                        Dim nr As Integer = 0

                        While dbread.Read
                            Dim pret As String = dbread("pret")
                            Dim bucati As String = dbread("bucati")
                            Dim luna As String = Format(CDate("01." & dbread("luna") & ".2015"), "MMMM")
                            Dim an As String = dbread("an")

                            sum_prod = sum_prod + (CInt(pret) * CInt(bucati))
                            Dim tf As XTextFormatter = New XTextFormatter(graph)
                            tf.Alignment = XParagraphAlignment.Right

                            If nr2 Mod 2 = 0 Then
                                graph.DrawRectangle(XBrushes.LightGray, 161, nxtRow - 5, 239, 19)

                            End If
                            If nr < 1 Then
                                graph.DrawString(produs, text_font, XBrushes.Black,
                                                 New XRect(20, nxtRow, 140, pdfPage.Height.Point), XStringFormats.TopCenter)
                            End If
                            graph.DrawString(pret.ToString, text_font, XBrushes.Black,
                                             New XRect(160, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)

                            graph.DrawString(bucati.ToString, text_font, XBrushes.Black,
                                             New XRect(210, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString(luna.ToString, text_font, XBrushes.Black,
                                             New XRect(260, nxtRow, 100, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString(an.ToString, text_font, XBrushes.Black,
                                             New XRect(360, nxtRow, 40, pdfPage.Height.Point), XStringFormats.TopCenter)

                            nxtRow = nxtRow + 20
                            nr = nr + 1
                            nr2 = nr2 + 1
                            If nxtRow > 800 Then
                                pdfPage = pdf.AddPage()
                                graph = XGraphics.FromPdfPage(pdfPage)
                                tf = New XTextFormatter(graph)
                                tf.Alignment = XParagraphAlignment.Right
                                '-----------------HEADER ----------------------------

                                graph.DrawLine(pen, 20, 18, 401, 18)
                                graph.DrawLine(pen, 20, 37, 401, 37)


                                graph.DrawString("Produs", top_font, XBrushes.Black,
                                                 New XRect(20, 20, 140, pdfPage.Height.Point), XStringFormats.TopCenter)

                                graph.DrawString("Pret".ToString, top_font, XBrushes.Black,
                                                 New XRect(160, 20, 50, pdfPage.Height.Point), XStringFormats.TopCenter)

                                graph.DrawString("Bucati".ToString, top_font, XBrushes.Black,
                                                 New XRect(210, 20, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
                                graph.DrawString("Luna".ToString, top_font, XBrushes.Black,
                                                 New XRect(260, 20, 100, pdfPage.Height.Point), XStringFormats.TopCenter)
                                graph.DrawString("An".ToString, top_font, XBrushes.Black,
                                                 New XRect(360, 20, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
                                graph.DrawString("Mag. Petru Maior 9".ToString, bottom_font, XBrushes.Black,
                                                 New XRect(400, 20, 150, pdfPage.Height.Point), XStringFormats.TopCenter)

                                graph.DrawLine(pen, 20, 18, 20, 800)
                                graph.DrawLine(pen, 401, 18, 401, 800)
                                '^^^-----------------HEADER ---------------------------^^^
                                graph.DrawLine(pen, 20, 800, 401, 800)
                                nxtRow = 43
                                nr = 0
                            End If
                        End While

                    End If
                    sum_tot = sum_tot + sum_prod

                    dbread.Close()
                    graph.DrawLine(pen, 20, nxtRow - 6, 401, nxtRow - 6)
                Next

                MsgBox(sum_tot)
                pdfPage = pdf.AddPage()
                graph = XGraphics.FromPdfPage(pdfPage)

                '-----------------HEADER ----------------------------

                graph.DrawLine(pen, 20, 18, 401, 18)
                graph.DrawLine(pen, 20, 37, 401, 37)


                graph.DrawString("Produs", top_font, XBrushes.Black,
                                 New XRect(20, 20, 140, pdfPage.Height.Point), XStringFormats.TopCenter)

                graph.DrawString("Pret".ToString, top_font, XBrushes.Black,
                                 New XRect(160, 20, 50, pdfPage.Height.Point), XStringFormats.TopCenter)

                graph.DrawString("Bucati".ToString, top_font, XBrushes.Black,
                                 New XRect(210, 20, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
                graph.DrawString("Luna".ToString, top_font, XBrushes.Black,
                                 New XRect(260, 20, 100, pdfPage.Height.Point), XStringFormats.TopCenter)
                graph.DrawString("An".ToString, top_font, XBrushes.Black,
                                 New XRect(360, 20, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
                graph.DrawString("Mag. Mihai Viteazu 28".ToString, bottom_font, XBrushes.Black,
                                 New XRect(400, 20, 150, pdfPage.Height.Point), XStringFormats.TopCenter)

                graph.DrawLine(pen, 20, 18, 20, 800)
                graph.DrawLine(pen, 401, 18, 401, 800)
                '^^^-----------------HEADER ---------------------------^^^
                graph.DrawLine(pen, 20, 800, 401, 800)
                nxtRow = 23
                nxtRow = nxtRow + 20
                sum_tot = 0
                For i = 0 To dbtable.Rows.Count - 1
                    Dim produs As String = dbtable.Rows(i).Item("produs").ToString()

                    Dim sql As String = "SELECT produs, pret, " & buc & ", " & dat & " FROM marfa WHERE produs=@produs AND magazin='MV' AND YEAR(data)=2021  " & grup & " ORDER BY pret ASC, YEAR(data) DESC, MONTH(data) DESC"
                    dbcomm = New MySqlCommand(sql, dbconn)
                    dbcomm.Parameters.AddWithValue("@produs", produs)
                    dbread = dbcomm.ExecuteReader()
                    Dim sum_prod As Integer = 0
                    If dbread.HasRows = True Then
                        Dim nr As Integer = 0
                        While dbread.Read
                            Dim pret As String = dbread("pret")
                            Dim bucati As String = dbread("bucati")
                            Dim luna As String = Format(CDate("01." & dbread("luna") & ".2015"), "MMMM")
                            Dim an As String = dbread("an")

                            sum_prod = sum_prod + (CInt(pret) * CInt(bucati))

                            Dim tf As XTextFormatter = New XTextFormatter(graph)
                            tf.Alignment = XParagraphAlignment.Right

                            If nr2 Mod 2 = 0 Then
                                graph.DrawRectangle(XBrushes.LightGray, 161, nxtRow - 5, 239, 19)

                            End If
                            If nr < 1 Then
                                graph.DrawString(produs, text_font, XBrushes.Black,
                                                 New XRect(20, nxtRow, 140, pdfPage.Height.Point), XStringFormats.TopCenter)
                            End If
                            graph.DrawString(pret.ToString, text_font, XBrushes.Black,
                                             New XRect(160, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)

                            graph.DrawString(bucati.ToString, text_font, XBrushes.Black,
                                             New XRect(210, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString(luna.ToString, text_font, XBrushes.Black,
                                             New XRect(260, nxtRow, 100, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString(an.ToString, text_font, XBrushes.Black,
                                             New XRect(360, nxtRow, 40, pdfPage.Height.Point), XStringFormats.TopCenter)

                            nxtRow = nxtRow + 20
                            nr = nr + 1
                            nr2 = nr2 + 1
                            If nxtRow > 800 Then
                                pdfPage = pdf.AddPage()
                                graph = XGraphics.FromPdfPage(pdfPage)
                                tf = New XTextFormatter(graph)
                                tf.Alignment = XParagraphAlignment.Right
                                '-----------------HEADER ----------------------------

                                graph.DrawLine(pen, 20, 18, 401, 18)
                                graph.DrawLine(pen, 20, 37, 401, 37)


                                graph.DrawString("Produs", top_font, XBrushes.Black,
                                                 New XRect(20, 20, 140, pdfPage.Height.Point), XStringFormats.TopCenter)

                                graph.DrawString("Pret".ToString, top_font, XBrushes.Black,
                                                 New XRect(160, 20, 50, pdfPage.Height.Point), XStringFormats.TopCenter)

                                graph.DrawString("Bucati".ToString, top_font, XBrushes.Black,
                                                 New XRect(210, 20, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
                                graph.DrawString("Luna".ToString, top_font, XBrushes.Black,
                                                 New XRect(260, 20, 100, pdfPage.Height.Point), XStringFormats.TopCenter)
                                graph.DrawString("An".ToString, top_font, XBrushes.Black,
                                                 New XRect(360, 20, 40, pdfPage.Height.Point), XStringFormats.TopCenter)
                                graph.DrawString("Mag. Mihai Viteazu 28".ToString, bottom_font, XBrushes.Black,
                                                 New XRect(400, 20, 150, pdfPage.Height.Point), XStringFormats.TopCenter)

                                graph.DrawLine(pen, 20, 18, 20, 800)
                                graph.DrawLine(pen, 401, 18, 401, 800)
                                '^^^-----------------HEADER ---------------------------^^^
                                graph.DrawLine(pen, 20, 800, 401, 800)
                                nxtRow = 43
                                nr = 0
                            End If
                        End While

                    End If

                    sum_tot = sum_tot + sum_prod
                    dbread.Close()
                    graph.DrawLine(pen, 20, nxtRow - 6, 401, nxtRow - 6)


                Next
                MsgBox(sum_tot)
            Catch ex As Exception
                MsgBox("Problem loading data: " & ex.Message.ToString)
            End Try

        End Using
        Dim pdfFilename As String = FileIO.SpecialDirectories.Desktop & "\Inventar.pdf"
        pdf.Save(pdfFilename)
        Process.Start(pdfFilename)
    End Sub

    Private Sub ModificaInformatiaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModificaInformatiaToolStripMenuItem.Click
        Dim cell As DataGridViewCell = Firme_DGV.CurrentCell
        Dim i As Integer = cell.ColumnIndex
        If i > -1 Then
            Dim col_name = Firme_DGV.Columns(i).HeaderText
            Dim row As DataGridViewRow = Firme_DGV.CurrentRow

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


            id = row.Cells("id").Value.ToString
            firma = row.Cells("firma").Value.ToString
            forma_juridica = row.Cells("forma_juridica").Value.ToString
            cui = row.Cells("cui").Value.ToString
            If IsDBNull(row.Cells("tva").Value) Then
                tva = 0
            Else
                tva = CBool(row.Cells("tva").Value)
            End If
            j = row.Cells("j").Value.ToString
            adresa = row.Cells("adresa").Value.ToString
            localitate = row.Cells("localitate").Value.ToString
            judet = row.Cells("judet").Value.ToString
            tip = row.Cells("tip").Value.ToString
            status = row.Cells("status").Value.ToString
            cont = row.Cells("cont").Value.ToString
            banca = row.Cells("banca").Value.ToString

            With Form_firme_introd
                .Text = "MODIFICA FIRMA"

                .firma_text.Text = firma
                .forma_juridica_text.Text = forma_juridica
                .cui_text.Text = cui
                If tva = True Then
                    .CheckBox1.CheckState = CheckState.Checked
                Else
                    .CheckBox1.CheckState = CheckState.Unchecked
                End If
                .j_text.Text = j
                .adresa_text.Text = adresa
                .localitate_text.Text = localitate
                .judet_text.Text = judet
                .tip_text.Text = tip
                .status_text.Text = status
                .cont_text.Text = cont
                .banca_text.Text = banca

            End With
            Form_firme_introd.ShowDialog()
            If Form_firme_introd.DialogResult = Windows.Forms.DialogResult.OK Then
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
                Dim sql_upd As String = "UPDATE firme SET " _
                                        & "firma = @firma, " _
                                        & "forma_juridica = @forma_juridica, " _
                                        & "cui = " & cui & ", " _
                                        & "tva = @tva, " _
                                        & "j = @j, " _
                                        & "adresa = @adresa, " _
                                        & "localitate = @localitate, " _
                                        & "judet = @judet, " _
                                        & "tip = @tip, " _
                                        & "status = @status, " _
                                        & "cont = @cont, " _
                                        & "banca = @banca " _
                                        & "WHERE id=@id"
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
                Load_Firme()

                For i = 0 To Firme_DGV.Rows.Count - 1
                    If Firme_DGV.Rows(i).Cells("id").Value = id Then
                        Firme_DGV.ClearSelection()
                        Firme_DGV.Rows(i).Selected = True
                        If i > 0 Then
                            Firme_DGV.FirstDisplayedScrollingRowIndex = i - 1
                        Else
                            Firme_DGV.FirstDisplayedScrollingRowIndex = i
                        End If

                        Exit For
                    End If
                Next
            End If
        Else : MsgBox("Nu s-a modificat nimic")

        End If
        Form_firme_introd.Close()
    End Sub
    Private Sub StergeFirmaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StergeFirmaToolStripMenuItem.Click
        Dim yesno As Integer = MsgBox("Stergi Firmele selectate?", MsgBoxStyle.YesNo)
        Dim row As DataGridViewRow = Firme_DGV.CurrentRow
        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then

            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            For i = 0 To Firme_DGV.SelectedRows.Count - 1
                Dim id As Integer = Firme_DGV.SelectedRows(i).Cells("id").Value.ToString

                Dim firma As String = Firme_DGV.SelectedRows(i).Cells("firma").Value.ToString
                Dim cui As String = Firme_DGV.SelectedRows(i).Cells("cui").Value.ToString
                'MsgBox(id & " " & firma & " " & cui)

                Try
                    Dim sql_del_inc As String = "DELETE FROM firme WHERE id=@id AND firma=@firma " 'AND cui=@cui"
                    dbcomm = New MySqlCommand(sql_del_inc, dbconn)
                    dbcomm.Parameters.AddWithValue("@id", id)
                    dbcomm.Parameters.AddWithValue("@firma", firma)
                    'dbcomm.Parameters.AddWithValue("@cui", cui)

                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Nu s-au sters firmele: " & ex.Message.ToString)
                End Try
            Next
            transaction.Commit()
            Load_Firme()

        End If
    End Sub
    Private Sub Cauta_firme_TB_TextChanged(sender As Object, e As EventArgs) Handles Cauta_firme_TB.TextChanged
        Load_Firme()
    End Sub
    Private Sub Ad_firma_But_Click(sender As Object, e As EventArgs) Handles Ad_firma_But.Click
        Form_firme_introd.ShowDialog()
        If Form_firme_introd.DialogResult = Windows.Forms.DialogResult.OK Then
            Dim id As String = ""
            Dim firma As String = ""
            Dim forma_juridica As String = ""
            Dim cui As String = ""
            Dim j As String = ""
            Dim adresa As String = ""
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
            j = Form_firme_introd.j_text.Text
            adresa = Form_firme_introd.adresa_text.Text
            tip = Form_firme_introd.tip_text.Text
            status = Form_firme_introd.status_text.Text
            cont = Form_firme_introd.cont_text.Text
            banca = Form_firme_introd.banca_text.Text
            Dim sql_upd As String = "INSERT INTO firme " _
                                    & "(firma,forma_juridica,cui,j,adresa,tip,status,cont,banca) " _
                                    & " VALUES" _
                                    & "(@firma, @forma_juridica," & cui & ",@j,@adresa,@tip,@status,@cont,@banca)"
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
                    dbcomm.Parameters.AddWithValue("@j", j)
                    dbcomm.Parameters.AddWithValue("@adresa", adresa)
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
            Load_Firme()
            Firme_DGV.ClearSelection()
            For i = 0 To Firme_DGV.Rows.Count - 1
                If Firme_DGV.Rows(i).Cells("firma").Value.ToString = firma Then
                    Firme_DGV.Rows(i).Selected = True
                    Firme_DGV.FirstDisplayedScrollingRowIndex = i
                    Exit For
                End If
            Next


        End If
    End Sub
    Private Sub Firme_DGV_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Firme_DGV.CellMouseDoubleClick
        'If e.Button = Windows.Forms.MouseButtons.Right And e.ColumnIndex > -1 And e.RowIndex > -1 Then
        Dim cell As DataGridViewCell = Firme_DGV.Rows(e.RowIndex).Cells("firma")
        Dim firm As String = cell.Value

        Cauta_firme_TB.Text = firm
        Load_Firme()
        Cauta_firme_TB.Focus()
        'End If
    End Sub


    Private Sub imp_extr_BU_Click_1(sender As Object, e As EventArgs) Handles imp_extr_BU.Click
        Dim OFD As OpenFileDialog = OpenFileDialog1
        OFD.Filter = "XML Files|*.xml"
        OFD.FileName = "Extras Cont"
        OFD.Title = "Adauga Extras Cont"

        If OFD.ShowDialog() = Windows.Forms.DialogResult.OK Then

            Dim extras As String = OFD.FileName
            Import_Extras(extras)
        End If
        Load_Banca()
    End Sub
    Public Sub Imp_Ex_CSV_BT()
        OpenFileDialog2.Filter = "Text Files|*.csv"
        OpenFileDialog2.Title = "Deschide .csv BT"
        OpenFileDialog2.FileName = ""
        If OpenFileDialog2.ShowDialog() = Windows.Forms.DialogResult.OK Then

            Dim path As String = OpenFileDialog2.FileName
            Dim Table As New DataTable With {.TableName = "CSVData"}
            Try
                'Dim csvReader As New System.IO.StreamReader(path)
                'Dim FormattedData As String = ""


                'Dim row As Integer = 0
                'Dim col As Integer = 0
                'Dim tblrow As Integer = 0
                'Dim begin As Boolean = False

                'While (csvReader.Peek() > -1)
                '    row += 1

                '    Dim rand As String = csvReader.ReadLine()
                '    Dim row_data As Array
                '    row_data = rand.Split(""",""")

                '    'MsgBox(row_data(0))
                '    If row_data(0) = "Data procesarii" Then
                '        begin = True

                '    End If
                '    If begin Then
                '        If Table.Columns.Count <= 0 Then
                '            For i = 0 To row_data.GetUpperBound(0)
                '                Table.Columns.Add()
                '            Next
                '        End If
                '        Dim row_new As DataRow
                '        row_new = Table.NewRow()

                '        For i = 0 To row_data.GetUpperBound(0)
                '            'row_new(i) = row_data.GetValue(i).ToString().Replace("""", "").Trim()
                '            MsgBox(row_data.GetValue(i).ToString().Trim())
                '            row_new(i) = row_data.GetValue(i).ToString().Trim()
                '        Next
                '        Table.Rows.Add(row_new)
                '        row_new = Nothing

                '    End If

                'End While

                'csvReader.Close()

                Dim csvParser As New TextFieldParser(path)
                csvParser.HasFieldsEnclosedInQuotes = True
                csvParser.SetDelimiters(",")

                Dim FormattedData As String = ""


                Dim row As Integer = 0
                Dim col As Integer = 0
                Dim tblrow As Integer = 0
                Dim begin As Boolean = False

                While (csvParser.EndOfData = False)
                    row += 1

                    Dim rand = csvParser.ReadFields()
                    'Dim row_data As Array
                    'row_data = rand.Split(""",""")

                    'MsgBox(row_data(0))
                    If rand(0) = "Data procesarii" Then
                        begin = True

                    End If
                    If begin Then
                        If Table.Columns.Count <= 0 Then
                            For i = 0 To rand.GetUpperBound(0)
                                Table.Columns.Add()
                            Next
                        End If
                        Dim row_new As DataRow
                        row_new = Table.NewRow()

                        For i = 0 To rand.GetUpperBound(0)
                            'row_new(i) = row_data.GetValue(i).ToString().Replace("""", "").Trim()
                            'MsgBox(rand.GetValue(i).ToString().Trim())
                            row_new(i) = rand.GetValue(i).ToString().Trim()
                        Next
                        Table.Rows.Add(row_new)
                        row_new = Nothing

                    End If

                End While

                csvParser.Close()
            Catch exp As Exception
                MessageBox.Show("Ceva Nu e bun la Lipire." & exp.Message)
            End Try

            Dim AppDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
            Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
                If dbconn.State = 0 Then
                    dbconn.Open()
                End If
                Dim dbcomm As New MySqlCommand
                Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
                For i = 1 To Table.Rows.Count - 1
                    'Dim d_arr As Array = Table.Rows(i).Item(0).ToString().Split("-")
                    'Dim zi As Integer = d_arr(0)
                    'Dim luna As Integer = d_arr(1)
                    'Dim an As Integer = d_arr(2)

                    Dim data_procesare As Date = Date.ParseExact(Table.Rows(i).Item(0).ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    Dim data As Date = Date.ParseExact(Table.Rows(i).Item(1).ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    Dim descriere As String = Table.Rows(i).Item(2).ToString()
                    Dim id_tranzactie As String = Table.Rows(i).Item(3).ToString()
                    Dim debit As String = Table.Rows(i).Item(4).ToString().Replace(",", "")
                    If debit = "" Then
                        debit = 0
                    End If
                    Dim credit As String = Table.Rows(i).Item(5).ToString().Replace(",", "")
                    If credit = "" Then
                        credit = 0
                    End If
                    Dim suma As String = 0

                    Dim tip_tranzactie As String = "-"
                    If credit > 0 Then
                        tip_tranzactie = "Incasare"
                        suma = credit
                    ElseIf credit <= 0 Then
                        tip_tranzactie = "Plata"
                        suma = debit
                    End If


                    Dim sql_ins As String = ""

                    If descriere.Substring(0, 8) = "Comision" Then 'Or id_tranzactie.Substring(0, 3) = "CTP"
                        sql_ins = "INSERT IGNORE INTO banca_comisioane (data,tip_tranzactie,suma,id_tranzactie,descriere,data_procesare)" _
                                            & " VALUES (@data,@tip_tranzactie,@suma,@id_tranzactie,@descriere,@data_procesare)"
                    Else
                        sql_ins = "INSERT IGNORE INTO banca_tranzactii (data,tip_tranzactie,suma,id_tranzactie,descriere,data_procesare)" _
                                            & " VALUES (@data,@tip_tranzactie,@suma,@id_tranzactie,@descriere,@data_procesare)"
                    End If
                    '-------------- insert

                    dbcomm = New MySqlCommand(sql_ins, dbconn)
                    dbcomm.Parameters.AddWithValue("@data", data_procesare)
                    dbcomm.Parameters.AddWithValue("@tip_tranzactie", tip_tranzactie)
                    dbcomm.Parameters.AddWithValue("@suma", suma)
                    dbcomm.Parameters.AddWithValue("@id_tranzactie", id_tranzactie)
                    dbcomm.Parameters.AddWithValue("@descriere", descriere)
                    dbcomm.Parameters.AddWithValue("@data_procesare", data_procesare)
                    dbcomm.ExecuteNonQuery()
                Next
                transaction.Commit()
            End Using
            If dbconn.State = ConnectionState.Open Then
                dbconn.Close()
            End If

            MsgBox("S-a Importat")
        End If
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbtable As New DataTable
            Dim bsource As New BindingSource

            Dim com_sql As String = "SELECT id_tranzactie,data,tip_tranzactie,suma,descriere FROM banca_comisioane"

            dbcomm = New MySqlCommand(com_sql, dbconn)

            Try
                If dbconn.State = ConnectionState.Broken Then
                    dbconn.Open()
                End If
                sda.SelectCommand = dbcomm
                sda.Fill(dbtable)
                bsource.DataSource = dbtable
                sda.Update(dbtable)
                dbread.Close()

                For i = 0 To dbtable.Rows.Count - 1
                    Dim data As Date = CDate(dbtable.Rows(i).Item("data"))
                    If data.DayOfWeek = DayOfWeek.Monday Then
                        data = DateAdd(DateInterval.Day, -3, data)
                    ElseIf data.DayOfWeek <> DayOfWeek.Friday Then
                        data = DateAdd(DateInterval.Day, -1, data)
                    End If
                    Dim tip_cheltuiala As String = "COM"
                    Dim nr_chitanta As String = dbtable.Rows(i).Item("id_tranzactie")
                    'MsgBox(nr_chitanta.Substring(0, 3))
                    Dim explicatii As String = "Comision Tranzactie"


                    Dim suma As Decimal = CDec(dbtable.Rows(i).Item("suma"))


                    Dim sql As String = "INSERT IGNORE INTO cheltuieli(data,tip_cheltuiala,nr_chitanta,explicatii,suma,magazin,cash) " _
                            & "VALUES(@data,@tip_cheltuialA,@nr_chitanta,@explicatii,@suma,@magazin,@cash)"

                    Try
                        If dbconn.State = 0 Then
                            dbconn.Open()
                        End If
                        dbcomm = New MySqlCommand(sql, dbconn)
                        Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
                        dbcomm.Parameters.AddWithValue("@data", data)
                        dbcomm.Parameters.AddWithValue("@tip_cheltuiala", tip_cheltuiala)
                        dbcomm.Parameters.AddWithValue("@nr_chitanta", nr_chitanta)
                        dbcomm.Parameters.AddWithValue("@explicatii", explicatii)
                        dbcomm.Parameters.AddWithValue("@suma", CDec(suma))
                        dbcomm.Parameters.AddWithValue("@magazin", "PM")
                        dbcomm.Parameters.AddWithValue("@cash", False)
                        dbcomm.ExecuteNonQuery()
                        transaction.Commit()
                        'dbconn.Close()
                    Catch ex As Exception
                        MsgBox("Failed to insert comisioane: " & ex.Message.ToString())
                    End Try
                Next

            Catch ex As Exception

                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
            dbtable.Clear()


            Dim plat_sql As String = "SELECT id_tranzactie,data,tip_tranzactie,suma,descriere FROM banca_tranzactii WHERE tip_tranzactie='Plata' AND descriere NOT LIKE '%Transfer card%'"
            'Dim plat_sql As String = "SELECT id_tranzactie,data,tip_tranzactie,suma,descriere FROM banca_tranzactii WHERE tip_tranzactie='Plata'"
            dbcomm = New MySqlCommand(plat_sql, dbconn)

            Try
                If dbconn.State = ConnectionState.Broken Then
                    dbconn.Open()
                End If
                sda.SelectCommand = dbcomm
                sda.Fill(dbtable)
                bsource.DataSource = dbtable
                sda.Update(dbtable)
                dbread.Close()

                For i = 0 To dbtable.Rows.Count - 1
                    Dim data As Date = CDate(dbtable.Rows(i).Item("data"))
                    If data.DayOfWeek = DayOfWeek.Monday Then
                        data = DateAdd(DateInterval.Day, -3, data)
                    ElseIf data.DayOfWeek <> DayOfWeek.Friday Then
                        data = DateAdd(DateInterval.Day, -1, data)
                    End If
                    Dim tip_cheltuiala As String = "OP"
                    Dim nr_chitanta As String = dbtable.Rows(i).Item("id_tranzactie")
                    Dim explicatii As String = ""

                    If nr_chitanta.Substring(0, 4) = "OLIT" Then
                        Dim detalii As String = dbtable.Rows(i).Item("descriere")
                        detalii = detalii.Split(":")(2)
                        detalii = detalii.Split("-")(0)
                        explicatii = detalii
                    ElseIf nr_chitanta.Substring(0, 3) = "CTP" Then
                        explicatii = "Comision Pachet POS"
                    Else : explicatii = "Plati Cont"
                    End If

                    Dim suma As Decimal = CDec(dbtable.Rows(i).Item("suma"))


                    Dim sql As String = "INSERT IGNORE INTO cheltuieli(data,tip_cheltuiala,nr_chitanta,explicatii,suma,magazin,cash) " _
                            & "VALUES(@data,@tip_cheltuiala,@nr_chitanta,@explicatii,@suma,@magazin,@cash)"

                    Try
                        If dbconn.State = 0 Then
                            dbconn.Open()
                        End If
                        dbcomm = New MySqlCommand(sql, dbconn)
                        Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
                        dbcomm.Parameters.AddWithValue("@data", data)
                        dbcomm.Parameters.AddWithValue("@tip_cheltuiala", tip_cheltuiala)
                        dbcomm.Parameters.AddWithValue("@nr_chitanta", nr_chitanta)
                        dbcomm.Parameters.AddWithValue("@explicatii", explicatii)
                        dbcomm.Parameters.AddWithValue("@suma", CDec(suma))
                        dbcomm.Parameters.AddWithValue("@magazin", "PM")
                        dbcomm.Parameters.AddWithValue("@cash", False)
                        dbcomm.ExecuteNonQuery()
                        transaction.Commit()
                        'dbconn.Close()
                    Catch ex As Exception
                        MsgBox("Failed to insert comisioane: " & ex.Message.ToString())
                    End Try
                Next

            Catch ex As Exception

                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
            dbtable.Clear()
        End Using
        Load_Situatie()
    End Sub

    Private Sub Curata_prod_BU_Click(sender As Object, e As EventArgs) Handles Curata_prod_BU.Click
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            Try
                Dim transaction As MySqlTransaction = dbconn.BeginTransaction()

                Dim sql_BLUZA As String = "UPDATE marfa SET produs='Bluze' WHERE produs='Bluza'"
                dbcomm = New MySqlCommand(sql_BLUZA, dbconn)
                dbcomm.ExecuteNonQuery()

                Dim sql_GEACA As String = "UPDATE marfa SET produs='Geci' WHERE produs='Geaca'"
                dbcomm = New MySqlCommand(sql_GEACA, dbconn)
                dbcomm.ExecuteNonQuery()

                Dim sql_FUSTE As String = "UPDATE marfa SET produs='Fuste' WHERE produs='Fusta'"
                dbcomm = New MySqlCommand(sql_FUSTE, dbconn)
                dbcomm.ExecuteNonQuery()

                Dim sql_CAMASI As String = "UPDATE marfa SET produs='Camasi' WHERE produs='Camasa'"
                dbcomm = New MySqlCommand(sql_CAMASI, dbconn)
                dbcomm.ExecuteNonQuery()

                Dim sql_GENTI As String = "UPDATE marfa SET produs='Genti' WHERE produs='Geanta'"
                dbcomm = New MySqlCommand(sql_GENTI, dbconn)
                dbcomm.ExecuteNonQuery()

                Dim sql_TRICOU As String = "UPDATE marfa SET produs='Tricouri' WHERE produs='Tricou'"
                dbcomm = New MySqlCommand(sql_TRICOU, dbconn)
                dbcomm.ExecuteNonQuery()

                Dim sql_VESTA As String = "UPDATE marfa SET produs='Veste' WHERE produs='Vesta'"
                dbcomm = New MySqlCommand(sql_VESTA, dbconn)
                dbcomm.ExecuteNonQuery()

                transaction.Commit()
            Catch ex As Exception
                MsgBox(ex)
            End Try
        End Using
        Load_Marfa()
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click, Button3.Click, Button4.Click, Button9.Click, Button8.Click, Button7.Click, Button12.Click, Button11.Click, Button10.Click
        Dim buton As Button = sender
        Dim x_but As Integer = buton.Location.X + Button2.Width + 10
        Dim y_but As Integer = buton.Location.Y
        Panel1.Location = New System.Drawing.Point(x_but, y_but)
        If Panel1.Visible = False Then
            Panel1.Show()
        ElseIf Panel1.Visible = True Then
            Panel1.Hide()
        End If
        TextBox25.Text = Nothing
        TextBox32.Text = Nothing
        If buton.Location = Button2.Location Then
            Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")

                Dim sda As New MySqlDataAdapter
                Dim dbtable As New DataTable
                Dim bsource As New BindingSource

                Dim tot_sql As String = "SELECT * FROM " _
                                        & "(SELECT sum(suma_cash) as inc_cash_PM FROM incasari WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_incasare = 'RZF' AND magazin = 'PM') inc_PM," _
                                        & "(SELECT sum(suma_cash) as inc_cash_MV FROM incasari WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_incasare = 'RZF' AND magazin = 'MV') inc_MV"
                '& "(SELECT sum(suma_cash) as inc_cash_a, sum(suma_card) as inc_card_a,(sum(suma_card)+sum(suma_cash)) as inc_tot_a FROM incasari WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_incasare = 'RZF') t_inc_an," _
                '& "(SELECT sum(suma_cash) as inc_cash_t, sum(suma_card) as inc_card_t,(sum(suma_card)+sum(suma_cash)) as inc_tot_t FROM incasari WHERE tip_incasare = 'RZF') t_inc_t," _
                '& "(SELECT sum(suma_cash) as di_l FROM incasari WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_incasare = 'DI') t_di_luna," _
                '& "(SELECT sum(suma_cash) as di_a FROM incasari WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_incasare = 'DI') t_di_an," _
                '& "(SELECT sum(suma_cash) as di_t FROM incasari WHERE tip_incasare = 'DI') t_di_t," _
                '& "(SELECT sum(suma) as che_cash_l FROM cheltuieli WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala <> 'DP' AND cash=TRUE) t_che_cash_luna," _
                '& "(SELECT sum(suma) as che_cont_l FROM cheltuieli WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala <> 'DP' AND cash=FALSE AND nr_chitanta NOT LIKE('CRRR.416010%')) t_che_cont_luna," _
                '& "(SELECT sum(suma) as che_cash_a FROM cheltuieli WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala <> 'DP' AND cash=TRUE) t_che_cash_an," _
                '& "(SELECT sum(suma) as che_cont_a FROM cheltuieli WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala <> 'DP' AND cash=FALSE AND nr_chitanta NOT LIKE('CRRR.416010%')) t_che_cont_an," _
                '& "(SELECT sum(suma) as che_cash_t FROM cheltuieli WHERE tip_cheltuiala <> 'DP' AND cash=TRUE) t_che_cash_t," _
                '& "(SELECT sum(suma) as che_cont_t FROM cheltuieli WHERE tip_cheltuiala <> 'DP' AND cash=FALSE AND nr_chitanta NOT LIKE('CRRR.416010%')) t_che_cont_t," _
                '& "(SELECT sum(suma) as dp_l FROM cheltuieli WHERE MONTH(data)='" & ComboBox1.SelectedItem & "' AND YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala = 'DP') t_dp_luna," _
                '& "(SELECT sum(suma) as dp_a FROM cheltuieli WHERE YEAR(data)= '" & ComboBox2.SelectedItem & "' AND tip_cheltuiala = 'DP') t_dp_an," _
                '& "(SELECT sum(suma) as dp_t FROM cheltuieli WHERE tip_cheltuiala = 'DP') t_dp_t"

                dbcomm = New MySqlCommand(tot_sql, dbconn)
                Try
                    If dbconn.State = ConnectionState.Closed Then
                        dbconn.Open()
                    End If
                    dbcomm = New MySqlCommand(tot_sql, dbconn)
                    sda.SelectCommand = dbcomm
                    sda.Fill(dbtable)
                    bsource.DataSource = dbtable
                    sda.Update(dbtable)
                    '-------------------------------INCASARI
                    '----------------- CASH 
                    Dim cashPM As Decimal = 0
                    If IsDBNull(dbtable.Rows(0).Item("inc_cash_PM")) Then
                        cashPM = 0
                    Else : cashPM = CDec(dbtable.Rows(0).Item("inc_cash_PM"))
                    End If
                    TextBox25.Text = FormatNumber(CDec(cashPM), 2, TriState.True, TriState.False, TriState.True) & " lei"

                    Dim cashMV As Decimal = 0
                    If IsDBNull(dbtable.Rows(0).Item("inc_cash_MV")) Then
                        cashMV = 0
                    Else : cashMV = CDec(dbtable.Rows(0).Item("inc_cash_MV"))
                    End If

                    TextBox32.Text = FormatNumber(CDec(cashMV), 2, TriState.True, TriState.False, TriState.True) & " lei"

                Catch ex As Exception
                    MsgBox("Problem loading sume incasari !!!: " & ex.Message.ToString)
                End Try
            End Using

        End If
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Load_Documente()
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        Form_adauga_factura.Show()
    End Sub


    Private Sub _Listview_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Registru_Listview.MouseDoubleClick, Raport_Listview.MouseDoubleClick, Nir_Listview.MouseDoubleClick, DP_Listview.MouseDoubleClick, DI_Listview.MouseDoubleClick, Facturi_ListView.MouseDoubleClick

    End Sub

    Private Sub BackupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BackupToolStripMenuItem.Click
        Dim dt_acum As String = Now.ToString("yyyyMMddHHmmss")
        SaveFileDialog1.InitialDirectory = "D:\Backup MySQL DB"
        SaveFileDialog1.Title = "Selectati Folderul pentru Backup"
        SaveFileDialog1.FileName = "Selectati Folderul"
        SaveFileDialog1.Filter = "sql Files (*.sql*)|*.sql"

        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim folder As String = System.IO.Path.GetDirectoryName(SaveFileDialog1.FileName) & "\"
            Dim dbconn As MySqlConnection = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim con As String = "server=localhost;user=root;pwd=*******;database=magazin;"
            Dim file As String = folder & "backup_" & dt_acum & ".sql"
            dbcomm = New MySqlCommand(con, dbconn)
            Dim mb As New MySqlBackup(dbcomm)
            dbconn.Open()
            'MsgBox(file)
            mb.ExportToFile(file)
            MsgBox("Backup created !")
            dbconn.Close()
        End If
    End Sub




    Private Sub StergeFacturileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StergeFacturileToolStripMenuItem.Click
        Dim yesno As Integer = MsgBox("Stergi Factura?", MsgBoxStyle.YesNo)

        Dim row As DataGridViewRow = Facturi_DGV.CurrentRow
        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then

            Dim dbconn As New MySqlConnection
            Dim dbcomm As New MySqlCommand
            dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")

            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()

            For i = 0 To Facturi_DGV.SelectedRows.Count - 1
                Dim id As Integer = CInt(Facturi_DGV.SelectedRows(i).Cells("id").Value)
                Dim data As Date = CDate(Facturi_DGV.SelectedRows(i).Cells("data").Value)
                Dim nr_fact As String = Facturi_DGV.SelectedRows(i).Cells("numar").Value.ToString
                'Dim tip_document As String = Intrari_DGV.SelectedRows(i).Cells("tip_document").Value.ToString
                'Dim explicatii As String = Intrari_DGV.SelectedRows(i).Cells("explicatii").Value
                'Dim magazin As String = Intrari_DGV.SelectedRows(i).Cells("magazin").Value

                Try
                    Dim sql_del_inc As String = "DELETE FROM facturi WHERE id=@id AND data=@data"
                    dbcomm = New MySqlCommand(sql_del_inc, dbconn)
                    dbcomm.Parameters.AddWithValue("@id", id)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    'dbcomm.Parameters.AddWithValue("@numar", nr_fact)
                    'dbcomm.Parameters.AddWithValue("@tip_document", tip_document)
                    'dbcomm.Parameters.AddWithValue("@explicatii", explicatii)
                    'dbcomm.Parameters.AddWithValue("@magazin", magazin)
                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Nu s-a sters din facturi: " & ex.Message.ToString)
                End Try
                Try
                    Dim sql_del_nir As String = "DELETE FROM prod_facturi WHERE nr_fact=@nr_fact"
                    dbcomm = New MySqlCommand(sql_del_nir, dbconn)
                    dbcomm.Parameters.AddWithValue("@data", data)
                    dbcomm.Parameters.AddWithValue("@nr_fact", nr_fact)
                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Nu s-a sters din prod_facturi: " & ex.Message.ToString)
                End Try

                '    Try
                '        Dim sql_del_mrf As String = "DELETE FROM marfa WHERE data=@data AND nir=@nir"
                '        dbcomm = New MySqlCommand(sql_del_mrf, dbconn)
                '        dbcomm.Parameters.AddWithValue("@data", data)
                '        dbcomm.Parameters.AddWithValue("@nir", nr_fact)
                '        dbcomm.ExecuteNonQuery()
                '    Catch ex As Exception
                '        MsgBox("Nu s-a sters din marfa: " & ex.Message.ToString)
                '    End Try
            Next
            transaction.Commit()

            Load_Facturi()
            prod_fact_DGV.Rows.Clear()
            dbread.Close()
            dbconn.Close()
        End If
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Form_adauga_obinv.Show()

    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        Imp_Ex_CSV_BT()
    End Sub

    Private Sub Button27_Click(sender As Object, e As EventArgs) Handles Button27.Click
        Form_Inventar.Show()

    End Sub
End Class
