Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization
Imports System.IO

Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing.Layout
Public Class Form_adauga_obinv
    Dim posY As Integer = 10
    Dim nr As Integer = 1
    Dim tab_ind As Integer = 13
    Dim tva As Integer = 0

    Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Public folder_nir As String
    Private Sub Form_adauga_obinv(sender As Object, e As EventArgs) Handles MyBase.Load
        Application.CurrentCulture = New CultureInfo("ro-RO")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("ro-RO")


        'DataGridView1.Rows.Add()
        'DataGridView1.Rows(0).Cells("crt").Value = "1"
        ''DataGridView1.Rows(0).Cells("produs").Value = "1."
        'DataGridView1.Rows(0).Cells("buc").Value = "Buc"
        'DataGridView1.Rows(0).Cells("cant").Value = 1
        'Dim bucati As Integer = DataGridView1.Rows(0).Cells("cant").Value
        'DataGridView1.Rows(0).Cells("pr_ach").Value = 0
        'Dim pr_ach As Decimal = DataGridView1.Rows(0).Cells("pr_ach").Value
        'Dim pr_ach_tva As Decimal = pr_ach * tva / 100
        'DataGridView1.Rows(0).Cells("pr_ach_tva").Value = pr_ach_tva
        'DataGridView1.Rows(0).Cells("pret").Value = 0
        'Dim pret As Decimal = DataGridView1.Rows(0).Cells("pret").Value
        'Dim valoare As Decimal = pret * bucati
        'DataGridView1.Rows(0).Cells("valoare").Value = valoare
        'DataGridView1.Rows(0).Cells("chk").Value = False
        'DataGridView1.Rows(0).Cells("but").Value = "edit"

        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        ' -------------------- LOAD setari

        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        'Dim folder_nir As String = ""
        Try
            Dim set_sql As String = "SELECT * FROM setari WHERE setare='path_nir'"
            dbconn.Open()
            dbcomm = New MySqlCommand(set_sql, dbconn)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = False Then
                SaveFileDialog1.Title = "Selectati Folderul pt NIR-uri"
                SaveFileDialog1.FileName = "Selectati Folderul"
                SaveFileDialog1.Filter = "pdf Files (*.pdf*)|*.pdf"

                If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim folder As String = System.IO.Path.GetDirectoryName(SaveFileDialog1.FileName) & "\"
                    Dim dbconn As MySqlConnection = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
                    Try
                        If dbconn.State = 0 Then
                            dbconn.Open()
                        End If

                        Dim set_path_sql As String = "REPLACE INTO setari(setare,valoare) VALUES('path_nir',@folder)"
                        Using dbcomm As MySqlCommand = New MySqlCommand(set_path_sql, dbconn)
                            dbcomm.Parameters.AddWithValue("@folder", folder)
                            dbcomm.ExecuteNonQuery()
                        End Using
                    Catch ex As Exception
                        MsgBox("Problem Nonquery: " & ex.Message.ToString)
                    End Try
                End If

            ElseIf dbread.HasRows = True Then
                folder_nir = dbread("valoare")
            End If

        Catch ex As Exception
            MsgBox("Problem loading setari savefile: " & ex.Message.ToString)
        End Try
        dbread.Close()

        '--------------------------------
        Try
            Dim set_sql As String = "SELECT * FROM setari WHERE setare='tva'"
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(set_sql, dbconn)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = True Then
                tva = CInt(dbread("valoare"))
            End If

        Catch ex As Exception
            MsgBox("Problem loading setari: tva: " & ex.Message.ToString)
        End Try
        dbread.Close()
        tip_nir_CB.DisplayMember = "Text"
        tip_nir_CB.ValueMember = "Value"
        Dim tbn As New DataTable
        tbn.Columns.Add("Text", GetType(String))
        tbn.Columns.Add("Value", GetType(String))
        tbn.Rows.Add("1. Obiecte Inventar", "OI")
        tbn.Rows.Add("2. Consumabile", "CO")
        tbn.Rows.Add("3. Mijloace Fixe", "MF")
        tbn.Rows.Add("4. Auxiliare", "AX")
        tbn.Rows.Add("5. Transfer", "Tr")
        tip_nir_CB.DataSource = tbn

        ComboBox3.DisplayMember = "Text"
        ComboBox3.ValueMember = "Value"
        Dim tb As New DataTable
        tb.Columns.Add("Text", GetType(String))
        tb.Columns.Add("Value", GetType(String))
        tb.Rows.Add("1. Petru Maior", "PM")
        tb.Rows.Add("2. Mihai Viteazu", "MV")
        ComboBox3.DataSource = tb
        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
        Else : PictureBox1.BackColor = Color.LightYellow
        End If

        inclTVA_CB.Checked = False
        upd_int_CHK.Checked = True

        'Dim sql_tot As String = "SELECT * FROM niruri ORDER BY data DESC, nir DESC"
        'Dim sda As New MySqlDataAdapter
        'Dim dbdataset As New DataTable
        'Dim bsource As New BindingSource
        Try
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            '    dbcomm = New MySqlCommand(sql_tot, dbconn)

            '    sda.SelectCommand = dbcomm
            '    sda.Fill(dbdataset)
            '    bsource.DataSource = dbdataset
            '    'DataGridView1.DataSource = bsource
            '    sda.Update(dbdataset)

            dbcomm = New MySqlCommand("SELECT firma FROM firme", dbconn)
            Dim lst As New List(Of String)
            dbread = dbcomm.ExecuteReader()
            While dbread.Read()
                lst.Add(dbread("firma").ToString())
            End While
            Dim mysource As New AutoCompleteStringCollection
            mysource.AddRange(lst.ToArray)
            firma_TB.AutoCompleteSource = AutoCompleteSource.CustomSource
            firma_TB.AutoCompleteCustomSource = mysource
            firma_TB.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            dbread.Close()

            dbcomm = New MySqlCommand("SELECT produs FROM obiecte_inventar group by produs", dbconn)
            Dim lst2 As New List(Of String)
            dbread = dbcomm.ExecuteReader()
            While dbread.Read()
                lst2.Add(dbread("produs").ToString())
            End While
            Dim mysource2 As New AutoCompleteStringCollection
            mysource2.AddRange(lst2.ToArray)
            produs_TB_1.AutoCompleteSource = AutoCompleteSource.CustomSource
            produs_TB_1.AutoCompleteCustomSource = mysource2
            produs_TB_1.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            dbread.Close()
        Catch ex As Exception
            MsgBox("Problem loading grid (form load) sau firme in autocomplete: " & ex.Message.ToString)
        End Try
        dbread.Close()

        Dim sql_read As String = "SELECT nir,data,magazin FROM niruri_obiecte ORDER BY data DESC, nir DESC LIMIT 1"
        Try
            Dim data_nir As Date = Today
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_read, dbconn)

            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows Then
                nir_TB.Text = (dbread("nir").ToString) + 1
                data_nir = dbread("data")
            Else
                nir_TB.Text = 1
            End If

            DateTimePicker1.Value = data_nir
            'data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)


        Catch ex As Exception
            MsgBox("Problem loading numar nir1: " & ex.Message.ToString)

        End Try
        dbread.Close()
        crt_Lbl.Text = row + 1
        tva_TB.Text = tva
        buc_TB_1.Text = "Buc"
        cant_TB_1.Text = 1
        pret_ach_TB_1.Text = 0
        pret_ach_tva_TB_1.Text = CDec(pret_ach_TB_1.Text) * tva / 100
        pret_vanzare_TB_1.Text = 0
        valoare_TB_1.Text = 0
        tot_pret_TB.Text = 0
        tot_valoare_TB.Text = 0

        'AddHandler ComboBox3.SelectedValueChanged, AddressOf ComboBox3_SelectedValueChanged
    End Sub
    Private Sub Form_adauga_nir_Leave(sender As Object, e As EventArgs) Handles MyBase.Leave
        Form_NIRuri.Load_niruri()
    End Sub

    Dim row As Integer = 0
    Dim edit_mode As Boolean = False

    Private Sub Ad_prod_But_Click(sender As Object, e As EventArgs) Handles ad_prod_But.Click
        Dim bun As Boolean = True
        tva = CInt(tva_TB.Text)
        If Trim(produs_TB_1.Text) = Nothing Then
            produs_TB_1.BackColor = Color.Red
            bun = False
        Else : produs_TB_1.BackColor = Color.White
        End If

        If cant_TB_1.Text = 0 Then
            cant_TB_1.BackColor = Color.Red
            bun = False
        ElseIf cant_TB_1.Text = 1 Then
            cant_TB_1.BackColor = Color.LightPink
        Else : cant_TB_1.BackColor = Color.White
        End If

        If pret_ach_TB_1.Text = 0 Then
            pret_ach_TB_1.BackColor = Color.Red
            bun = False
        Else : pret_ach_TB_1.BackColor = Color.White
        End If

        If pret_ach_tva_TB_1.Text = 0 Then
            pret_ach_tva_TB_1.BackColor = Color.Red
            bun = False
        Else : pret_ach_tva_TB_1.BackColor = Color.White
        End If

        If pret_vanzare_TB_1.Text = 0 Then
            pret_vanzare_TB_1.BackColor = Color.Red
            bun = False
        ElseIf pret_vanzare_TB_1.Text < pret_ach_tva_TB_1.Text Then
            pret_vanzare_TB_1.BackColor = Color.LightPink
        Else : pret_vanzare_TB_1.BackColor = Color.White
        End If

        If valoare_TB_1.Text = 0 Then
            valoare_TB_1.BackColor = Color.Red
            bun = False
        Else : valoare_TB_1.BackColor = Color.White
        End If

        If bun = True Then
            If edit_mode = False Then

                DataGridView1.Rows.Add()
                DataGridView1.Rows(row).Cells("crt").Value = crt_Lbl.Text
                DataGridView1.Rows(row).Cells("produs").Value = StrConv(produs_TB_1.Text, VbStrConv.ProperCase)
                DataGridView1.Rows(row).Cells("buc").Value = buc_TB_1.Text
                DataGridView1.Rows(row).Cells("cant").Value = CDec(cant_TB_1.Text)
                DataGridView1.Rows(row).Cells("pr_ach").Value = CDec(pret_ach_TB_1.Text)
                DataGridView1.Rows(row).Cells("pr_ach_tva").Value = CDec(pret_ach_tva_TB_1.Text)
                DataGridView1.Rows(row).Cells("pret").Value = CDec(pret_vanzare_TB_1.Text)
                DataGridView1.Rows(row).Cells("valoare").Value = CDec(valoare_TB_1.Text)
                DataGridView1.Rows(row).Cells("chk").Value = True
                DataGridView1.Rows(row).Cells("edit").Value = "edit"
                DataGridView1.Rows(row).Cells("del").Value = "del"

                crt_Lbl.Text = row + 2

                row = row + 1

            ElseIf edit_mode = True Then
                Dim row As Integer = 0
                For i = 0 To DataGridView1.Rows.Count - 1
                    If DataGridView1.Rows(i).Cells("crt").Value = crt_Lbl.Text Then
                        row = i
                        Exit For
                    End If
                Next
                DataGridView1.Rows(row).Cells("crt").Value = crt_Lbl.Text
                DataGridView1.Rows(row).Cells("produs").Value = produs_TB_1.Text
                DataGridView1.Rows(row).Cells("buc").Value = buc_TB_1.Text
                DataGridView1.Rows(row).Cells("cant").Value = CDec(cant_TB_1.Text)
                DataGridView1.Rows(row).Cells("pr_ach").Value = CDec(pret_ach_TB_1.Text)
                DataGridView1.Rows(row).Cells("pr_ach_tva").Value = CDec(pret_ach_tva_TB_1.Text)
                DataGridView1.Rows(row).Cells("pret").Value = CDec(pret_vanzare_TB_1.Text)
                DataGridView1.Rows(row).Cells("valoare").Value = CDec(valoare_TB_1.Text)
                DataGridView1.Rows(row).Cells("chk").Value = True
                DataGridView1.Rows(row).Cells("edit").Value = "edit"
                DataGridView1.Rows(row).Cells("del").Value = "del"
                crt_Lbl.Text = DataGridView1.Rows.Count + 1
                For j = 0 To DataGridView1.Rows.Count - 1
                    For k = 0 To DataGridView1.Columns.Count - 1
                        DataGridView1.Rows(j).Cells(k).Style.BackColor = Color.White
                    Next
                Next
            End If

            produs_TB_1.SelectAll()
            buc_TB_1.Text = "Buc"
            cant_TB_1.Text = 1
            pret_ach_TB_1.Text = 0
            If tva = 0 Or inclTVA_CB.CheckState = CheckState.Checked Then
                pret_ach_tva_TB_1.Text = CDec(pret_ach_TB_1.Text)
            Else : pret_ach_tva_TB_1.Text = CDec(pret_ach_TB_1.Text) * tva / 100
            End If
            pret_vanzare_TB_1.Text = 0
            valoare_TB_1.Text = 0
            '-----------------------------------------

            Dim produs As String = ""
            Dim bucati As Decimal = 0
            Dim pret As Decimal = 0
            Dim valoare As Decimal = 0
            Dim nir As String = nir_TB.Text

            Dim cant As Decimal = 0
            Dim pr_ach As Decimal = 0
            Dim pr_ach_tva As Decimal = 0
            Dim pr_intrare As Decimal = 0
            Dim dat As String = DateTimePicker1.Value
            tva = CDec(tva_TB.Text)
            Dim total_ach_tva As Decimal = 0
            Dim total_bucati As Decimal = 0
            Dim total_valoare As Decimal = 0


            For i As Integer = 0 To DataGridView1.RowCount - 1
                bucati = bucati + DataGridView1.Rows(i).Cells("cant").Value

                cant = DataGridView1.Rows(i).Cells("cant").Value
                pr_ach_tva = DataGridView1.Rows(i).Cells("pr_ach_tva").Value

                total_ach_tva = total_ach_tva + (pr_ach_tva * cant)

                valoare = valoare + DataGridView1.Rows(i).Cells("valoare").Value
            Next

            total_ach_tva = total_ach_tva
            total_valoare = valoare
            total_bucati = bucati


            tot_pret_TB.Text = total_ach_tva
            tot_valoare_TB.Text = total_valoare
            tot_cant_TB.Text = total_bucati
            adaos_Lbl.Text = "Adaos: " & FormatNumber((total_valoare / total_ach_tva * 100 - 100), 2, TriState.True, TriState.False, TriState.True) & " %"
            produs_TB_1.Focus()

        ElseIf bun = False Then

        End If
        edit_mode = False
        ad_prod_But.Image = Global.Magazin.My.Resources.Resources.ic_input_add
        'AddHandler ComboBox3.SelectedValueChanged, AddressOf ComboBox3_SelectedValueChanged
    End Sub

    Private Sub Save_Bu_Click(sender As Object, e As EventArgs) Handles save_Bu.Click
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")


        Dim nir As String = nir_TB.Text
        Dim dat As String = DateTimePicker1.Value
        Dim tva As String = tva_TB.Text
        Dim firma As String = firma_TB.Text
        Dim nr_fact As String = fact_TB.Text
        Dim cif_firma As String = cif_firma_TB.Text
        Dim mag As String = ComboBox3.SelectedValue
        Dim valoare As Decimal = CDec(tot_valoare_TB.Text)

        If dbconn.State = ConnectionState.Closed Then
            dbconn.Open()
        End If
        'If tip_nir_CB.SelectedValue = "OI" Then
        Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                Dim produs As String = StrConv(DataGridView1.Rows(i).Cells("produs").Value.ToString, VbStrConv.ProperCase)
                Dim bucati As Decimal = DataGridView1.Rows(i).Cells("cant").Value
                Dim tip As String = tip_nir_CB.SelectedValue.ToString
                Dim pret As Decimal = DataGridView1.Rows(i).Cells("pret").Value
                Dim pr_intrare As Decimal = DataGridView1.Rows(i).Cells("pr_ach_tva").Value



            Dim sql_marf As String = "INSERT INTO obiecte_inventar (produs,pret,tip,bucati,nir,data,pret_intrare,magazin) VALUES(@produs,@pret,@tip,@bucati,@nir,@data,@pret_intrare,@magazin)"

            Try

                    dbcomm = New MySqlCommand(sql_marf, dbconn)

                    dbcomm.Parameters.AddWithValue("@produs", produs)
                    dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value.Date)
                    dbcomm.Parameters.AddWithValue("@pret", CDec(pret))
                    dbcomm.Parameters.AddWithValue("@bucati", bucati)
                    dbcomm.Parameters.AddWithValue("@tip", tip)
                    dbcomm.Parameters.AddWithValue("@nir", nir)
                    dbcomm.Parameters.AddWithValue("@pret_intrare", CDec(pr_intrare))
                    dbcomm.Parameters.AddWithValue("@magazin", mag)

                    dbcomm.ExecuteNonQuery()


                Catch ex As Exception
                    MsgBox("Failed to insert into marfa: " & ex.Message.ToString())
                End Try
            Next
        Dim sql_nir As String = "INSERT INTO niruri_obiecte (nir,data,valoare,tva,nr_factura,nume_firma,cif_firma,magazin) VALUES(@nir,@data,@valoare,@tva,@nr_factura,@nume_firma,@cif_firma,@magazin)"

        Try

                dbcomm = New MySqlCommand(sql_nir, dbconn)
                dbcomm.Parameters.AddWithValue("@nir", nir)
                dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value.Date)
                dbcomm.Parameters.AddWithValue("@valoare", valoare)
                dbcomm.Parameters.AddWithValue("@tva", tva)
                dbcomm.Parameters.AddWithValue("@nr_factura", nr_fact)
                dbcomm.Parameters.AddWithValue("@nume_firma", firma)
                dbcomm.Parameters.AddWithValue("@cif_firma", cif_firma)
                dbcomm.Parameters.AddWithValue("@magazin", mag)
                dbcomm.ExecuteNonQuery()


            Catch ex As Exception
            MsgBox("Failed to insert into niruri_obiecte: " & ex.Message.ToString())
        End Try
        transaction.Commit()

        Dim sql_fir_tva As String = "UPDATE firme SET tva=@tva WHERE firma=@nume_firma"
            Dim bool As Boolean = True
            If tva > 0 Then
                bool = True
            Else : bool = False
            End If
            Try

                dbcomm = New MySqlCommand(sql_fir_tva, dbconn)
                'dbcomm.Parameters.AddWithValue("@nir", nir)
                'dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value.Date)
                'dbcomm.Parameters.AddWithValue("@valoare", valoare)
                dbcomm.Parameters.AddWithValue("@tva", bool)
                'dbcomm.Parameters.AddWithValue("@nr_factura", nr_fact)
                dbcomm.Parameters.AddWithValue("@nume_firma", firma)
                'dbcomm.Parameters.AddWithValue("@cif_firma", cif_firma)
                'dbcomm.Parameters.AddWithValue("@magazin", mag)
                dbcomm.ExecuteNonQuery()


            Catch ex As Exception
                MsgBox("Failed to insert into firme: " & ex.Message.ToString())
            End Try

        'If upd_int_CHK.CheckState = CheckState.Checked Then

        '    Dim sql_ins_nir As String = "INSERT INTO intrari (data,nr_nir,explicatii,suma,tip_document,magazin) VALUES(@data,@nr_nir,@explicatii,@suma,@tip_document,@magazin)"

        '    Try
        '        dbcomm = New MySqlCommand(sql_ins_nir, dbconn)

        '        dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value.Date)
        '        dbcomm.Parameters.AddWithValue("@nr_nir", nir)
        '        dbcomm.Parameters.AddWithValue("@explicatii", firma & " " & Trim(f_jur_TB.Text))
        '        dbcomm.Parameters.AddWithValue("@suma", valoare)
        '        dbcomm.Parameters.AddWithValue("@tip_document", "NIR")
        '        dbcomm.Parameters.AddWithValue("@magazin", mag)

        '        dbcomm.ExecuteNonQuery()
        '    Catch ex As Exception
        '        MsgBox("Failed to insert into intrari: " & ex.Message.ToString())
        '    End Try
        '    transaction.Commit()

        'End If
        'dbread.Close()
        Dim sql_che As String = "SELECT data,suma,explicatii FROM cheltuieli WHERE data=@data AND suma=@suma AND explicatii=@explicatii AND magazin=@magazin"
            Try
                dbcomm = New MySqlCommand(sql_che, dbconn)

                dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value.Date)
                dbcomm.Parameters.AddWithValue("@explicatii", firma & " " & Trim(f_jur_TB.Text))
                dbcomm.Parameters.AddWithValue("@suma", CDec(tot_pret_TB.Text))
                dbcomm.Parameters.AddWithValue("@magazin", mag)

                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                ' MsgBox(dbread("suma"))
                If dbread.HasRows = False Then
                    MsgBox("Atentie ! Nu exista chitanta pt. factura curenta !")
                End If
                dbread.Close()
            Catch ex As Exception
                MsgBox("Failed to get cheltuieli:  " & ex.Message.ToString())
            End Try


        Dim yes_no As DialogResult = MsgBox("Printezi NIR si Bon Consum?", MsgBoxStyle.YesNo)
        If yes_no = Windows.Forms.DialogResult.Yes Then
                PrintNir_BU.PerformClick()
            End If
        'End If

        'If tip_nir_CB.SelectedValue = "Tr" Then
        '    Dim transaction As MySqlTransaction = dbconn.BeginTransaction()

        '    Dim sql_nir As String = "INSERT INTO niruri (nir,data,valoare,tva,nr_factura,nume_firma,cif_firma,magazin) VALUES(@nir,@data,@valoare,@tva,@nr_factura,@nume_firma,@cif_firma,@magazin)"

        '    Try

        '        dbcomm = New MySqlCommand(sql_nir, dbconn)
        '        dbcomm.Parameters.AddWithValue("@nir", nir)
        '        dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value.Date)
        '        dbcomm.Parameters.AddWithValue("@valoare", valoare)
        '        dbcomm.Parameters.AddWithValue("@tva", tva)
        '        dbcomm.Parameters.AddWithValue("@nr_factura", nr_fact)
        '        dbcomm.Parameters.AddWithValue("@nume_firma", "Transfer Marfa")
        '        dbcomm.Parameters.AddWithValue("@cif_firma", cif_firma)
        '        dbcomm.Parameters.AddWithValue("@magazin", mag)
        '        dbcomm.ExecuteNonQuery()


        '    Catch ex As Exception
        '        MsgBox("Failed To insert into marfa: " & ex.Message.ToString())
        '    End Try


        '    'transaction.Commit()

        '    Dim sql_fir_tva As String = "UPDATE firme SET tva=@tva WHERE firma=@nume_firma"
        '    Dim bool As Boolean = True
        '    If tva > 0 Then
        '        bool = True
        '    Else : bool = False
        '    End If
        '    Try

        '        dbcomm = New MySqlCommand(sql_fir_tva, dbconn)
        '        dbcomm.Parameters.AddWithValue("@tva", bool)
        '        dbcomm.Parameters.AddWithValue("@nume_firma", firma)
        '        dbcomm.ExecuteNonQuery()


        '    Catch ex As Exception
        '        MsgBox("Failed to insert into firme: " & ex.Message.ToString())
        '    End Try

        '    If upd_int_CHK.CheckState = CheckState.Checked Then

        '        Dim sql_ins_nir As String = "INSERT INTO intrari (data,nr_nir,explicatii,suma,tip_document,magazin) VALUES(@data,@nr_nir,@explicatii,@suma,@tip_document,@magazin)"

        '        Try
        '            dbcomm = New MySqlCommand(sql_ins_nir, dbconn)

        '            dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value.Date)
        '            dbcomm.Parameters.AddWithValue("@nr_nir", nir)
        '            dbcomm.Parameters.AddWithValue("@explicatii", "Transfer Marfa")
        '            dbcomm.Parameters.AddWithValue("@suma", valoare)
        '            dbcomm.Parameters.AddWithValue("@tip_document", "NIR")
        '            dbcomm.Parameters.AddWithValue("@magazin", mag)

        '            dbcomm.ExecuteNonQuery()
        '        Catch ex As Exception
        '            MsgBox("Failed to insert into intrari: " & ex.Message.ToString())
        '        End Try

        '        Dim sql_ins_avz As String = "INSERT INTO intrari (data,nr_nir,explicatii,suma,tip_document,magazin) VALUES(@data,@nr_nir,@explicatii,@suma,@tip_document,@magazin)"

        '        Try
        '            dbcomm = New MySqlCommand(sql_ins_nir, dbconn)

        '            dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value.Date)
        '            dbcomm.Parameters.AddWithValue("@nr_nir", nr_fact)
        '            dbcomm.Parameters.AddWithValue("@explicatii", "Transfer Marfa")
        '            dbcomm.Parameters.AddWithValue("@suma", -valoare)
        '            dbcomm.Parameters.AddWithValue("@tip_document", "Aviz")
        '            If ComboBox3.SelectedValue = "PM" Then
        '                mag = "MV"
        '            Else
        '                mag = "PM"
        '            End If
        '            dbcomm.Parameters.AddWithValue("@magazin", mag)

        '            dbcomm.ExecuteNonQuery()
        '        Catch ex As Exception
        '            MsgBox("Failed to insert into intrari: " & ex.Message.ToString())
        '        End Try
        '        transaction.Commit()

        '    End If
        'Dim yes_no As DialogResult = MsgBox("Printezi NIR", MsgBoxStyle.YesNo)
        'If yes_no = Windows.Forms.DialogResult.Yes Then
        '    PrintNir_BU.PerformClick()
        'End If
        'End If


        dbconn.Close()

        'nir_TB.Text = nir + 1
        'fact_TB.Clear()
        'firma_TB.Clear()
        'f_jur_TB.Clear()
        'cif_firma_TB.Clear()
        'crt_Lbl.Text = 1
        'DataGridView1.Rows.Clear()
        'row = 0
        'tot_cant_TB.Clear()
        'tot_pret_TB.Clear()
        'tot_valoare_TB.Clear()
        'adaos_Lbl.Text = "0 %"

        'Form_NIRuri.Load_niruri()
        Me.Focus()

    End Sub

    Private Sub Enter_Keypress(ByVal sender As System.Object, ByVal e As KeyPressEventArgs) Handles buc_TB_1.KeyPress, pret_ach_TB_1.KeyPress, pret_ach_tva_TB_1.KeyPress, fact_TB.KeyPress, DateTimePicker1.KeyPress, DateTimePicker2.KeyPress, pret_vanzare_TB_1.KeyPress, tva_TB.KeyPress, produs_TB_1.KeyPress, valoare_TB_1.KeyPress, nir_TB.KeyPress, cant_TB_1.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If
    End Sub
    Private Sub _Keydown_TAB(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles firma_TB.KeyDown, produs_TB_1.KeyDown
        If e.KeyValue = 13 Then
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If

    End Sub

    Private Sub ComboBox3_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedValueChanged
        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
        Else : PictureBox1.BackColor = Color.LightYellow
        End If
    End Sub

    Private Sub Tip_nir_CB_SelectedValueChanged(sender As Object, e As EventArgs) Handles tip_nir_CB.SelectedValueChanged
        If tip_nir_CB.SelectedValue = "Tr" Then

            firma_TB.Text = "MILICOM CAZ"
            f_jur_TB.Text = "S.R.L."
            tva_TB.Text = 0
            Dim nr_avz As Integer = 0

            Dim sql_read As String = "SELECT nr_nir,data,tip_document,magazin FROM intrari WHERE tip_document='Aviz' ORDER BY data DESC, nr_nir DESC LIMIT 1"
            Try
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(sql_read, dbconn)

                dbread = dbcomm.ExecuteReader()
                dbread.Read()

                nr_avz = (dbread("nr_nir").ToString) + 1

            Catch ex As Exception
                MsgBox("Problem loading numar nir: " & ex.Message.ToString)

            End Try
            dbread.Close()

            fact_TB.Text = nr_avz
        End If

    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged

        DateTimePicker2.Value = DateTimePicker1.Value
    End Sub


    Private Sub Firma_TB_Leave(sender As Object, e As EventArgs) Handles firma_TB.Leave
        tva = tva
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        If dbconn.State = ConnectionState.Closed Then
            dbconn.Open()
        End If
        dbcomm = New MySqlCommand("SELECT firma,cui,forma_juridica,tva FROM firme WHERE firma LIKE '" & firma_TB.Text & "%'", dbconn)

        dbread = dbcomm.ExecuteReader()
        dbread.Read()
        'If Trim(f_jur_TB.Text) = Nothing Or Trim(firma_TB.Text) = Nothing Then
        If dbread.HasRows = False Or Trim(firma_TB.Text) = Nothing Then
            dbread.Close() '---------------
            Form_firme_introd.tip_text.Text = "Marfa"
            Form_firme_introd.ShowDialog()
            If Form_firme_introd.DialogResult = Windows.Forms.DialogResult.OK Then
                dbread.Close() '---------------

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
                                        & "(firma,forma_juridica,cui,tva,j,adresa,tip,status,cont,banca) " _
                                        & " VALUES" _
                                        & "(@firma, @forma_juridica," & cui & ",@tva,@j,@adresa,@tip,@status,@cont,@banca)"
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
                        'dbcomm.Parameters.AddWithValue("@ro", ro)
                        dbcomm.Parameters.AddWithValue("@j", j)
                        dbcomm.Parameters.AddWithValue("@adresa", adresa)
                        dbcomm.Parameters.AddWithValue("@localitate", localitate)
                        dbcomm.Parameters.AddWithValue("@judet", judet)
                        dbcomm.Parameters.AddWithValue("@tip", tip)
                        dbcomm.Parameters.AddWithValue("@status", status)
                        dbcomm.Parameters.AddWithValue("@cont", cont)
                        dbcomm.Parameters.AddWithValue("@banca", banca)
                        dbcomm.Parameters.AddWithValue("@tva", tva)
                        dbcomm.Parameters.AddWithValue("@id", id)
                        dbcomm.ExecuteNonQuery()

                        dbread.Close() '---------------
                    End Using

                    dbread.Close() '---------------
                Catch ex As Exception
                    MsgBox("Nu s-a modificat: " & ex.Message.ToString)
                End Try

                'Form_principal.Load_Firme()
                dbcomm = New MySqlCommand("SELECT firma,cui,forma_juridica FROM firme WHERE firma LIKE '%" & firma & "%'", dbconn)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                f_jur_TB.Text = dbread("forma_juridica").ToString
                cif_firma_TB.Text = dbread("cui").ToString()
                firma_TB.Text = firma
                firma_TB.SelectAll()
                firma_TB.Focus()
                f_jur_TB.Text = dbread("forma_juridica").ToString
                cif_firma_TB.Text = dbread("cui").ToString()

                dbread.Close() '---------------
            End If

            'dbread.Close() '---------------

        ElseIf dbread.HasRows = True Then
            'dbread = dbcomm.ExecuteReader()
            dbread.Read()
            f_jur_TB.Text = dbread("forma_juridica").ToString
            cif_firma_TB.Text = dbread("cui").ToString()
            Dim tva_bool As Boolean = True
            If IsDBNull(dbread("tva")) Then
                'MsgBox("iserror(cbool)")
                tva_TB.Text = tva
                tva_TB.BackColor = Color.Pink
            ElseIf CBool(dbread("tva")) = True Then
                tva_TB.Text = tva
                tva_TB.BackColor = Color.White
            ElseIf dbread("tva").ToString = False Then
                tva_TB.Text = 0
                tva_TB.BackColor = Color.White
            End If
            dbread.Close() '---------------
        End If

        dbread.Close() '---------------
        'End If
    End Sub

    'Dim r As Integer = 0
    'Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    '    Dim cell1 As DataGridViewCell = DataGridView1.CurrentCell
    '    r = r
    '    If cell1.ColumnIndex = DataGridView1.Rows(r).Cells("but").ColumnIndex Then

    '        DataGridView1.Rows.Add()
    '        DataGridView1.Rows(r + 1).Cells("crt").Value = r + 1
    '        DataGridView1.Rows(r + 1).Cells("but").Value = "+"

    '        DataGridView1.Rows(r + 1).Cells("buc").Value = "Buc"
    '        DataGridView1.Rows(r + 1).Cells("cant").Value = 1
    '        Dim bucati As Integer = DataGridView1.Rows(r + 1).Cells("cant").Value
    '        DataGridView1.Rows(r + 1).Cells("pr_ach").Value = 0
    '        Dim pr_ach As Decimal = DataGridView1.Rows(r + 1).Cells("pr_ach").Value
    '        Dim pr_ach_tva As Decimal = pr_ach * tva / 100
    '        DataGridView1.Rows(r + 1).Cells("pr_ach_tva").Value = pr_ach_tva
    '        DataGridView1.Rows(r + 1).Cells("pret").Value = 0
    '        Dim pret As Decimal = DataGridView1.Rows(r + 1).Cells("pret").Value
    '        Dim valoare As Decimal = pret * bucati
    '        DataGridView1.Rows(r + 1).Cells("valoare").Value = valoare
    '        DataGridView1.Rows(r + 1).Cells("chk").Value = False
    '        DataGridView1.Rows(r + 1).Cells("but").Value = "+"

    '        DataGridView1.Rows(r + 1).Cells("crt").Value = r + 2
    '        DataGridView1.Rows(r + 1).Cells("but").Value = "+"


    '        DataGridView1.FirstDisplayedScrollingRowIndex = r + 1
    '        DataGridView1.Rows(r + 1).Cells("produs").Selected = True
    '        If DataGridView1.Rows(r + 1).Cells("produs").IsInEditMode = False Then
    '            'Dim ed = DataGridView1.Rows(r + 1).Cells("produs").EditType
    '            'DataGridView1.Rows(r + 1).Cells("produs").InitializeEditingControl(r + 1, DataGridView1.DefaultCellStyle.NullValue.ToString)
    '        End If
    '        r = r + 1
    '        SendKeys.Send("{TAB}")
    '    End If

    'End Sub
    'Private Sub DataGridView1_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellLeave
    '    Dim cell As DataGridViewCell = DataGridView1.CurrentCell
    '    'tva = tva_TB.Text

    '    Dim row As Integer = DataGridView1.CurrentRow.Index

    '    'Dim bucati As Integer = DataGridView1.Rows(row).Cells("cant").Value

    '    If cell.ColumnIndex = DataGridView1.Rows(row).Cells("pr_ach").ColumnIndex Or
    '        cell.ColumnIndex = DataGridView1.Rows(row).Cells("produs").ColumnIndex Or
    '        cell.ColumnIndex = DataGridView1.Rows(row).Cells("pret").ColumnIndex Or
    '        cell.ColumnIndex = DataGridView1.Rows(row).Cells("valoare").ColumnIndex Then
    '        SendKeys.Send("{TAB}")
    '    End If

    '    'If cell.ColumnIndex = DataGridView1.Rows(row).Cells("pr_ach").ColumnIndex Then
    '    '    Dim pr_ach As Decimal = DataGridView1.Rows(row).Cells("pr_ach").Value
    '    '    Dim pr_ach_tva As Decimal = pr_ach + (pr_ach * tva / 100)
    '    '    DataGridView1.Rows(row).Cells("pr_ach_tva").Value = pr_ach_tva
    '    'End If

    '    'Dim pret As Decimal = DataGridView1.Rows(row).Cells("pret").Value
    '    'Dim valoare As Decimal = pret * bucati
    '    'DataGridView1.Rows(row).Cells("valoare").Value = valoare

    '    For i = 0 To DataGridView1.RowCount - 1
    '        For j = 0 To DataGridView1.ColumnCount - 1
    '            Dim bucati As Integer = DataGridView1.Rows(i).Cells("cant").Value

    '            Dim pr_ach As Decimal = DataGridView1.Rows(i).Cells("pr_ach").Value
    '            Dim pr_ach_tva As Decimal = pr_ach + (pr_ach * tva / 100)
    '            DataGridView1.Rows(i).Cells("pr_ach_tva").Value = pr_ach_tva
    '            Dim pret As Decimal = DataGridView1.Rows(i).Cells("pret").Value
    '            Dim valoare As Decimal = pret * bucati
    '            DataGridView1.Rows(i).Cells("valoare").Value = valoare

    '            If Trim(DataGridView1.Rows(i).Cells("produs").Value) = Nothing Then
    '                DataGridView1.Rows(i).Cells("produs").Style.BackColor = Color.Red
    '                DataGridView1.Rows(i).Cells("chk").Value = False
    '            ElseIf IsNothing(Trim(DataGridView1.Rows(i).Cells("produs").Value)) = False Then
    '                DataGridView1.Rows(i).Cells("produs").Style.BackColor = Color.White
    '                DataGridView1.Rows(i).Cells("chk").Value = True
    '            End If
    '        Next
    '    Next

    'End Sub

    Private Sub Numere_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cant_TB_1.KeyPress, pret_ach_TB_1.KeyPress, pret_ach_tva_TB_1.KeyPress, pret_vanzare_TB_1.KeyPress, valoare_TB_1.KeyPress

        If Not Char.IsDigit(e.KeyChar) Then e.Handled = True
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 45 Or ((e.KeyChar = "." Or e.KeyChar = ",") And (sender.Text.IndexOf(".") = -1 And sender.Text.IndexOf(",") = -1)))
        If Asc(e.KeyChar) = 46 Then
            e.KeyChar = ChrW(44)
        End If
    End Sub

    Private Sub Textbox_1_Leave(sender As Object, e As EventArgs) Handles produs_TB_1.Leave, buc_TB_1.Leave, cant_TB_1.Leave, pret_vanzare_TB_1.Leave '', valoare_TB_1.Leave

        valoare_TB_1.Text = CDec(cant_TB_1.Text) * CDec(pret_vanzare_TB_1.Text)
        ad_prod_But.Enabled = True

    End Sub
    Private Sub Textbox_2_Leave(sender As Object, e As EventArgs) Handles cant_TB_1.Leave, pret_ach_TB_1.Leave
        tva = CInt(tva_TB.Text)
        If tva = 0 Or inclTVA_CB.CheckState = CheckState.Checked Then
            pret_ach_tva_TB_1.Text = FormatNumber(CDec(pret_ach_TB_1.Text), 2)
        Else
            pret_ach_tva_TB_1.Text = FormatNumber(CDec(pret_ach_TB_1.Text) + (CDec(pret_ach_TB_1.Text) * tva / 100), 2)

        End If
        pret_vanzare_TB_1.Text = FormatNumber(CDec(pret_ach_tva_TB_1.Text), 2)
        ad_prod_But.Enabled = True


    End Sub
    Private Sub Produs_TB_1_TextChanged(sender As Object, e As EventArgs) Handles produs_TB_1.TextChanged
        If Trim(produs_TB_1.Text) = Nothing Then
            produs_TB_1.BackColor = Color.Red
        Else : produs_TB_1.BackColor = Color.White
        End If

    End Sub

    Private Sub Cant_TB_1_TextChanged(sender As Object, e As EventArgs) Handles cant_TB_1.TextChanged
        If cant_TB_1.Text = "" Then
            cant_TB_1.Text = 0
            cant_TB_1.SelectAll()
            cant_TB_1.Focus()
        End If
        If cant_TB_1.Text = 0 Then
            cant_TB_1.BackColor = Color.Red
        ElseIf cant_TB_1.Text = 1 Then
            cant_TB_1.BackColor = Color.LightPink
        Else : cant_TB_1.BackColor = Color.White
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If e.RowIndex < 0 Then
            Exit Sub
        End If

        Dim grid = DirectCast(sender, DataGridView)

        If TypeOf grid.Columns(e.ColumnIndex) Is DataGridViewButtonColumn Then

            If grid.Columns(e.ColumnIndex).Name = "edit" Then

                If edit_mode = True Then
                    edit_mode = False
                    ad_prod_But.Image = Global.Magazin.My.Resources.Resources.ic_input_add
                    crt_Lbl.Text = DataGridView1.Rows.Count + 1
                    For j = 0 To DataGridView1.Rows.Count - 1
                        For k = 0 To DataGridView1.Columns.Count - 1
                            DataGridView1.Rows(j).Cells(k).Style.BackColor = Color.White
                        Next
                    Next
                    buc_TB_1.Text = "Buc"
                    cant_TB_1.Text = 1
                    pret_ach_TB_1.Text = 0
                    If tva = 0 Or inclTVA_CB.CheckState = CheckState.Checked Then
                        pret_ach_tva_TB_1.Text = CDec(pret_ach_TB_1.Text)
                    Else : pret_ach_tva_TB_1.Text = CDec(pret_ach_TB_1.Text) * tva / 100
                    End If
                    pret_vanzare_TB_1.Text = 0
                    valoare_TB_1.Text = 0
                    produs_TB_1.SelectAll()
                    produs_TB_1.Focus()

                ElseIf edit_mode = False Then
                    edit_mode = True
                    ad_prod_But.Image = Global.Magazin.My.Resources.Resources.server_ok
                    Dim rand As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

                    For j = 0 To DataGridView1.Rows.Count - 1
                        For k = 0 To DataGridView1.Columns.Count - 1
                            DataGridView1.Rows(j).Cells(k).Style.BackColor = Color.White
                        Next
                    Next
                    For l = 0 To DataGridView1.Columns.Count - 1
                        rand.Cells(l).Style.BackColor = Color.LightPink
                    Next

                    Dim crt As String = rand.Cells("crt").Value.ToString
                    Dim produs As String = rand.Cells("produs").Value.ToString
                    Dim buc As String = rand.Cells("buc").Value.ToString
                    Dim cant As String = rand.Cells("cant").Value.ToString
                    Dim pr_ach As String = rand.Cells("pr_ach").Value.ToString
                    Dim pr_ach_tva As String = rand.Cells("pr_ach_tva").Value.ToString
                    Dim pret As String = rand.Cells("pret").Value.ToString
                    Dim valoare As String = rand.Cells("valoare").Value.ToString

                    crt_Lbl.Text = crt
                    produs_TB_1.Text = produs
                    buc_TB_1.Text = buc
                    cant_TB_1.Text = cant
                    pret_ach_TB_1.Text = pr_ach
                    pret_ach_tva_TB_1.Text = pr_ach_tva
                    pret_vanzare_TB_1.Text = pret
                    valoare_TB_1.Text = valoare
                    produs_TB_1.SelectAll()
                    produs_TB_1.Focus()
                End If
            End If
            If grid.Columns(e.ColumnIndex).Name = "del" Then
                Dim rand As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
                DataGridView1.Rows.Remove(rand)
                row = DataGridView1.Rows.Count
                If DataGridView1.Rows.Count > 0 Then

                    crt_Lbl.Text = DataGridView1.Rows.Count + 1
                    For i = 0 To DataGridView1.Rows.Count - 1
                        DataGridView1.Rows(i).Cells("crt").Value = i + 1
                    Next
                Else : crt_Lbl.Text = 1

                End If

                produs_TB_1.SelectAll()
                produs_TB_1.Focus()
            End If

            Dim bucati_ As Decimal = 0
            Dim pret_ As Decimal = 0
            Dim valoare_ As Decimal = 0
            Dim cant_ As Decimal = 0
            Dim pr_ach_ As Decimal = 0
            Dim pr_ach_tva_ As Decimal = 0
            Dim pr_intrare_ As Decimal = 0
            Dim dat As String = DateTimePicker1.Value
            tva = CDec(tva_TB.Text)
            Dim total_ach_tva_ As Decimal = 0
            Dim total_bucati_ As Decimal = 0
            Dim total_valoare_ As Decimal = 0


            For i As Integer = 0 To DataGridView1.RowCount - 1
                bucati_ = bucati_ + DataGridView1.Rows(i).Cells("cant").Value
                cant_ = DataGridView1.Rows(i).Cells("cant").Value
                pr_ach_tva_ = DataGridView1.Rows(i).Cells("pr_ach_tva").Value
                total_ach_tva_ = total_ach_tva_ + (pr_ach_tva_ * cant_)
                valoare_ = valoare_ + DataGridView1.Rows(i).Cells("valoare").Value
            Next

            total_ach_tva_ = total_ach_tva_
            total_valoare_ = valoare_
            total_bucati_ = bucati_


            tot_pret_TB.Text = total_ach_tva_
            tot_valoare_TB.Text = total_valoare_
            tot_cant_TB.Text = total_bucati_
            Dim adaos As Decimal = 0
            If total_ach_tva_ = 0 Then
                total_ach_tva_ = 1
            End If


            adaos = ((total_valoare_ / total_ach_tva_) * 100) - 100
            adaos_Lbl.Text = "Adaos: " & FormatNumber(adaos, 2, TriState.True, TriState.False, TriState.True) & " %"
        End If

    End Sub

    Private Sub PrintNir_BU_Click(sender As Object, e As EventArgs) Handles PrintNir_BU.Click

        ''Dim yesno As Integer = MsgBox("Printezi Dispozitia de incasare?", MsgBoxStyle.YesNo)
        ''If yesno = DialogResult.No Then
        ''ElseIf yesno = DialogResult.Yes Then
        'Dim row As DataGridViewRow = DataGridView1.CurrentRow
        Dim mag As String = ComboBox3.SelectedValue
        Dim magazin As String = ""
        If ComboBox3.SelectedValue = "PM" Then
            magazin = "Magazin PETRU MAIOR 9"
        ElseIf ComboBox3.SelectedValue = "MV" Then
            magazin = "Magazin MIHAI VITEAZU 28"
        End If
        Dim data_nir As String = Format(DateTimePicker1.Value, "dd.MM.yyyy")
        Dim nir As String = nir_TB.Text
        Dim tip As String = tip_nir_CB.SelectedValue.ToString
        Dim firma As String = firma_TB.Text
        Dim f_jur As String = f_jur_TB.Text
        Dim nr_factura As String = fact_TB.Text
        Dim data_factura As String = Format(DateTimePicker2.Value, "dd.MM.yyyy")
        'Dim nr_rzf As String = row.Cells("nr_rzf").Value.ToString
        'Dim tip_incasare As String = row.Cells("tip_incasare").Value.ToString
        'Dim explicatii As String = row.Cells("explicatii").Value
        'Dim suma As String = row.Cells("suma_cash").Value

        Dim pdf As PdfSharp.Pdf.PdfDocument = New PdfSharp.Pdf.PdfDocument
        pdf.Info.Title = "NIR"
        Dim pdfPage As PdfSharp.Pdf.PdfPage = pdf.AddPage
        pdfPage.Orientation = PageOrientation.Landscape

        Dim graph As XGraphics = XGraphics.FromPdfPage(pdfPage)
        Dim tf As XTextFormatter = New XTextFormatter(graph)
        tf.Alignment = XParagraphAlignment.Right
        Dim pen As XPen = New XPen(Color.Black, 0.5)


        Dim titlu_font As XFont = New XFont("Verdana", 16, XFontStyle.Bold)
        Dim top_font As XFont = New XFont("Calibri", 12, XFontStyle.Bold)
        Dim mic_font As XFont = New XFont("Calibri", 11, XFontStyle.Regular)
        Dim report_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim text_font As XFont = New XFont("Calibri", 12, XFontStyle.Regular)
        Dim text_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim bottom_font As XFont = New XFont("Verdana", 12, XFontStyle.Bold)

        'Dim incas_tot As Double = 0
        'Dim chelt_tot As Double = 0
        ''A4 LANDSCAPE = 8.27x11.69" x72points/inch = 842X595 points

        graph.DrawString("Unitatea: ", text_font, XBrushes.Black,
                         New XRect(45, 20, 50, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("MILICOM CAZ S.R.L.", top_font, XBrushes.Black,
                         New XRect(95, 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)


        graph.DrawString("Gestiune: ", text_font, XBrushes.Black,
                        New XRect(45, 37, 50, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(magazin, top_font, XBrushes.Black,
                        New XRect(95, 37, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)



        graph.DrawString("NOTA DE RECEPTIE", titlu_font, XBrushes.Black,
                         New XRect(145, 20, 552, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawString("Nr. Document: ", text_font, XBrushes.Black,
                         New XRect(667, 24, 60, pdfPage.Height.Point), XStringFormats.TopLeft)
        tf.DrawString(nir & "." & tip, titlu_font, XBrushes.Black,
                         New XRect(747, 20, 50, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawLine(pen, 697, 35, 140, 35)

        graph.DrawString("Data: ", text_font, XBrushes.Black,
                        New XRect(712, 37, 80, pdfPage.Height.Point), XStringFormats.TopLeft)
        tf.DrawString(data_nir, top_font, XBrushes.Black,
                        New XRect(697, 37, 100, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawLine(pen, 697, 52, 140, 52)
        Dim subsemnatii As String = "Subsemnatii, membrii comisiei de receptie, am procedat la receptionarea valorilor materiale furnizate de "
        Dim fact_avz As String = ""
        If tip_nir_CB.SelectedValue = "Tr" Then
            fact_avz = "aviz nr. "
        Else
            fact_avz = "factura fiscala nr. "
        End If
        Dim auto As String = "cu auto nr.___________, avand ca document de insotire " & fact_avz & nr_factura & " / " & data_factura & ", constatand urmatoarele:"


        graph.DrawString(subsemnatii, text_font, XBrushes.Black,
                         New XRect(45, 67, 560, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString(firma & " " & f_jur, top_font, XBrushes.Black,
                         New XRect(560, 67, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString(auto, text_font, XBrushes.Black,
                         New XRect(45, 84, 752, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawLine(pen, 45, 100, 797, 100) 'H

        graph.DrawLine(pen, 45, 100, 45, 132) ' V

        graph.DrawString("NR.", mic_font, XBrushes.Black,
                         New XRect(45, 104, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("CRT.", mic_font, XBrushes.Black,
                         New XRect(45, 114, 30, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 75, 100, 75, 132) ' V

        graph.DrawString("Denumirea marfii", mic_font, XBrushes.Black,
                         New XRect(75, 109, 190, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 265, 100, 265, 132) ' V

        graph.DrawString("U.M.", mic_font, XBrushes.Black,
                         New XRect(265, 109, 30, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 295, 100, 295, 132) ' V

        graph.DrawString("Cantitate", mic_font, XBrushes.Black,
                         New XRect(295, 109, 100, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 395, 100, 395, 132) ' V

        graph.DrawString("Pret Intrare", mic_font, XBrushes.Black,
                         New XRect(395, 109, 100, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 495, 100, 495, 132) ' V

        graph.DrawString("Valoare Intrare", mic_font, XBrushes.Black,
                         New XRect(495, 109, 100, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 595, 100, 595, 132) ' V

        graph.DrawString("Pret Vanzare", mic_font, XBrushes.Black,
                         New XRect(595, 109, 100, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 695, 100, 695, 132) ' V

        graph.DrawString("Valoare Vanzare", mic_font, XBrushes.Black,
                         New XRect(695, 109, 101, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 797, 100, 797, 132) ' V

        'graph.DrawLine(pen, strtH, strtV, length, stopV)
        graph.DrawLine(pen, 45, 132, 797, 132) ' H

        Dim nxtRow As Integer = 135
        Dim DGV As DataGridView = DataGridView1
        For i = 0 To DGV.Rows.Count - 1
            Dim crt As String = DGV.Rows(i).Cells("crt").Value.ToString
            Dim produs As String = DGV.Rows(i).Cells("produs").Value.ToString
            Dim buc As String = DGV.Rows(i).Cells("buc").Value.ToString
            Dim cant As Decimal = CDec(DGV.Rows(i).Cells("cant").Value.ToString)
            Dim pret_intrare As Decimal = CDec(DGV.Rows(i).Cells("pr_ach_tva").Value.ToString)
            Dim valoare_intrare As Decimal = cant * pret_intrare
            Dim pret_vanzare As Decimal = DGV.Rows(i).Cells("pret").Value.ToString
            Dim valoare_vanzare As Decimal = DGV.Rows(i).Cells("valoare").Value.ToString

            'graph.DrawLine(pen, 45, 100, 797, 100) 'H

            'graph.DrawLine(pen, 45, 100, 45, 132) ' V

            graph.DrawString(crt, text_font, XBrushes.Black,
                             New XRect(45, nxtRow, 30, pdfPage.Height.Point), XStringFormats.TopCenter)

            'graph.DrawLine(pen, 75, 100, 75, 132) ' V

            graph.DrawString(produs, text_font, XBrushes.Black,
                             New XRect(75, nxtRow, 190, pdfPage.Height.Point), XStringFormats.TopCenter)

            'graph.DrawLine(pen, 265, 100, 265, 132) ' V

            graph.DrawString(buc, text_font, XBrushes.Black,
                             New XRect(265, nxtRow, 30, pdfPage.Height.Point), XStringFormats.TopCenter)

            'graph.DrawLine(pen, 295, 100, 295, 132) ' V

            graph.DrawString(cant, text_font, XBrushes.Black,
                             New XRect(295, nxtRow, 100, pdfPage.Height.Point), XStringFormats.TopCenter)

            'graph.DrawLine(pen, 395, 100, 395, 132) ' V

            tf.DrawString(Format(pret_intrare, "#,#0.00"), text_font, XBrushes.Black,
                             New XRect(395, nxtRow, 95, pdfPage.Height.Point), XStringFormats.TopLeft)

            'graph.DrawLine(pen, 495, 100, 495, 132) ' V

            tf.DrawString(Format(valoare_intrare, "#,#0.00"), text_font, XBrushes.Black,
                             New XRect(495, nxtRow, 95, pdfPage.Height.Point), XStringFormats.TopLeft)

            'graph.DrawLine(pen, 595, 100, 595, 132) ' V

            tf.DrawString(Format(pret_vanzare, "#,#0.00"), text_font, XBrushes.Black,
                             New XRect(595, nxtRow, 95, pdfPage.Height.Point), XStringFormats.TopLeft)

            'graph.DrawLine(pen, 695, 100, 695, 132) ' V

            tf.DrawString(Format(valoare_vanzare, "#,#0.00"), text_font, XBrushes.Black,
                             New XRect(695, nxtRow, 95, pdfPage.Height.Point), XStringFormats.TopLeft)

            'graph.DrawLine(pen, 796, 100, 796, 132) ' V


            'graph.DrawLine(pen, 45, 132, 797, 132) ' H
            nxtRow = nxtRow + 23
        Next

        graph.DrawLine(pen, 45, 480, 797, 480) ' H

        graph.DrawLine(pen, 45, 480, 45, 555) ' V

        graph.DrawString("Membrii comisiei (Nume, Prenume, Semnatura)", mic_font, XBrushes.Black,
                        New XRect(45, 485, 250, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 295, 480, 295, 555) ' V


        Dim total_vanzare As Decimal = CDec(tot_valoare_TB.Text)
        Dim total_intrare As Decimal = CDec(tot_pret_TB.Text)

        graph.DrawString("TOTAL", top_font, XBrushes.Black,
                       New XRect(295, 485, 200, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 495, 480, 495, 555) ' V

        tf.DrawString(Format(total_intrare, "#,#0.00"), top_font, XBrushes.Black,
                       New XRect(495, 485, 95, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawLine(pen, 595, 480, 595, 555) ' V

        tf.DrawString(Format(total_vanzare, "#,#0.00"), top_font, XBrushes.Black,
                       New XRect(695, 485, 95, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawLine(pen, 797, 480, 797, 555) ' V


        graph.DrawLine(pen, 45, 555, 797, 555) ' H


        Dim pdfFilename As String = folder_nir & "NIR_" & nir & "_" & tip & "_" & Format(CDate(data_nir), "yyyyMMdd") & "_" & mag & ".pdf"

        If System.IO.File.Exists(pdfFilename) = True Then
            Dim OkCancel As Integer = MsgBox("Fisierul exista. Inlocuiti?", MsgBoxStyle.OkCancel)
            If OkCancel = DialogResult.Cancel Then
                Exit Sub
            ElseIf OkCancel = DialogResult.OK Then
                pdf.Save(pdfFilename)
                'Form_principal.Load_Nir_Listview()
                Process.Start(pdfFilename)
            End If
        Else : pdf.Save(pdfFilename)
            'Form_principal.Load_Nir_Listview()
            Process.Start(pdfFilename)
        End If




    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim mag As String = ComboBox3.SelectedValue
        Dim magazin As String = ""
        If ComboBox3.SelectedValue = "PM" Then
            magazin = "Magazin PETRU MAIOR 9"
        ElseIf ComboBox3.SelectedValue = "MV" Then
            magazin = "Magazin MIHAI VITEAZU 28"
        End If
        Dim data_nir As String = Format(DateTimePicker1.Value, "dd.MM.yyyy")
        Dim nir As String = nir_TB.Text
        Dim tip As String = tip_nir_CB.SelectedValue.ToString
        'Dim firma As String = firma_TB.Text
        'Dim f_jur As String = f_jur_TB.Text
        ' Dim nr_factura As String = fact_TB.Text
        'Dim data_factura As String = Format(DateTimePicker2.Value, "dd.MM.yyyy")


        Dim pdf_bon As PdfSharp.Pdf.PdfDocument = New PdfSharp.Pdf.PdfDocument
        pdf_bon.Info.Title = "BON"
        Dim pdfPage_bon As PdfSharp.Pdf.PdfPage = pdf_bon.AddPage
        pdfPage_bon.Orientation = PageOrientation.Landscape

        Dim graph_bon As XGraphics = XGraphics.FromPdfPage(pdfPage_bon)
        Dim tf_bon As XTextFormatter = New XTextFormatter(graph_bon)
        tf_bon.Alignment = XParagraphAlignment.Right
        Dim pen As XPen = New XPen(Color.Black, 0.5)


        Dim titlu_font As XFont = New XFont("Verdana", 16, XFontStyle.Bold)
        Dim top_font As XFont = New XFont("Calibri", 12, XFontStyle.Bold)
        Dim mic_font As XFont = New XFont("Calibri", 11, XFontStyle.Regular)
        Dim report_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim text_font As XFont = New XFont("Calibri", 12, XFontStyle.Regular)
        Dim text_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim bottom_font As XFont = New XFont("Verdana", 12, XFontStyle.Bold)

        'Dim incas_tot As Double = 0
        'Dim chelt_tot As Double = 0
        ''A4 LANDSCAPE = 8.27x11.69" x72points/inch = 842X595 points

        graph_bon.DrawLine(pen, 45, 20, 797, 20) 'H
        graph_bon.DrawLine(pen, 45, 20, 45, 100) ' V

        graph_bon.DrawString("UNITATEA: ", text_font, XBrushes.Black,
                         New XRect(65, 60, 60, pdfPage_bon.Height.Point), XStringFormats.TopLeft)
        graph_bon.DrawString("MILICOM CAZ S.R.L.", top_font, XBrushes.Black,
                         New XRect(125, 60, 150, pdfPage_bon.Height.Point), XStringFormats.TopLeft)

        graph_bon.DrawLine(pen, 275, 20, 275, 100) ' V

        graph_bon.DrawString("Numar document ", text_font, XBrushes.Black,
                        New XRect(275, 40, 100, pdfPage_bon.Height.Point), XStringFormats.TopCenter)
        graph_bon.DrawString(nir, top_font, XBrushes.Black,
                        New XRect(275, 75, 100, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        graph_bon.DrawLine(pen, 375, 20, 375, 100) ' V

        graph_bon.DrawString("Data", text_font, XBrushes.Black,
                        New XRect(375, 25, 150, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        graph_bon.DrawString("Zi", text_font, XBrushes.Black,
                        New XRect(375, 55, 37.5, pdfPage_bon.Height.Point), XStringFormats.TopCenter)
        graph_bon.DrawString(DateTimePicker2.Value.Day, top_font, XBrushes.Black,
                        New XRect(375, 75, 37.5, pdfPage_bon.Height.Point), XStringFormats.TopCenter)
        graph_bon.DrawLine(pen, 412.5, 55, 412.5, 100) ' V

        graph_bon.DrawString("Luna", text_font, XBrushes.Black,
                        New XRect(412.5, 55, 37.5, pdfPage_bon.Height.Point), XStringFormats.TopCenter)
        graph_bon.DrawString(DateTimePicker2.Value.Month, top_font, XBrushes.Black,
                        New XRect(412.5, 75, 37.5, pdfPage_bon.Height.Point), XStringFormats.TopCenter)
        graph_bon.DrawLine(pen, 450, 55, 450, 100) ' V

        graph_bon.DrawString("An", text_font, XBrushes.Black,
                        New XRect(450, 55, 75, pdfPage_bon.Height.Point), XStringFormats.TopCenter)
        graph_bon.DrawString(DateTimePicker2.Value.Year, top_font, XBrushes.Black,
                        New XRect(450, 75, 75, pdfPage_bon.Height.Point), XStringFormats.TopCenter)
        graph_bon.DrawLine(pen, 525, 55, 525, 100) ' V

        graph_bon.DrawLine(pen, 375, 55, 525, 55) 'H
        graph_bon.DrawLine(pen, 275, 70, 525, 70) 'H

        graph_bon.DrawLine(pen, 525, 20, 525, 100) ' V
        graph_bon.DrawLine(pen, 45, 100, 797, 100) 'H


        graph_bon.DrawString("BON DE CONSUM", titlu_font, XBrushes.Black,
                         New XRect(525, 50, 250, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        graph_bon.DrawLine(pen, 797, 20, 797, 132) ' V

        graph_bon.DrawLine(pen, 45, 100, 45, 132) ' V

        graph_bon.DrawString("Nr. crt", mic_font, XBrushes.Black,
                         New XRect(45, 110, 35, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        graph_bon.DrawLine(pen, 80, 100, 80, 132) ' V

        graph_bon.DrawString("Denumirea materialelor", mic_font, XBrushes.Black,
                         New XRect(80, 110, 295, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        graph_bon.DrawLine(pen, 375, 100, 375, 132) ' V

        graph_bon.DrawString("Cant. necesara", mic_font, XBrushes.Black,
                         New XRect(375, 110, 75, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        graph_bon.DrawLine(pen, 450, 100, 450, 132) ' V

        graph_bon.DrawString("UM", mic_font, XBrushes.Black,
                         New XRect(450, 110, 75, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        graph_bon.DrawLine(pen, 525, 100, 525, 132) ' V

        graph_bon.DrawString("Cant. eliberata", mic_font, XBrushes.Black,
                         New XRect(525, 110, 75, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        graph_bon.DrawLine(pen, 600, 100, 600, 132) ' V

        graph_bon.DrawString("Pret unitar", mic_font, XBrushes.Black,
                         New XRect(600, 110, 98.5, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        graph_bon.DrawLine(pen, 698.5, 100, 698.5, 132) ' V

        graph_bon.DrawString("Valoare", mic_font, XBrushes.Black,
                         New XRect(698.5, 110, 98.5, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        graph_bon.DrawLine(pen, 45, 132, 797, 132) 'H

        'graph_bon.DrawString("Nr. Document: ", text_font, XBrushes.Black,
        '                 New XRect(667, 24, 60, pdfPage_bon.Height.Point), XStringFormats.TopLeft)
        'tf_bon.DrawString(nir & "." & tip, titlu_font, XBrushes.Black,
        '                 New XRect(747, 20, 50, pdfPage_bon.Height.Point), XStringFormats.TopLeft)
        ''graph_bon.DrawLine(pen, 697, 35, 140, 35)

        'graph_bon.DrawString("Data: ", text_font, XBrushes.Black,
        '                New XRect(712, 37, 80, pdfPage_bon.Height.Point), XStringFormats.TopLeft)
        'tf_bon.DrawString(data_nir, top_font, XBrushes.Black,
        '                New XRect(697, 37, 100, pdfPage_bon.Height.Point), XStringFormats.TopLeft)
        ''graph_bon.DrawLine(pen, 697, 52, 140, 52)
        'Dim subsemnatii As String = "Subsemnatii, membrii comisiei de receptie, am procedat la receptionarea valorilor materiale furnizate de "
        'Dim fact_avz As String = ""
        'If tip_nir_CB.SelectedValue = "OI" Then
        '    fact_avz = "factura fiscala nr. "
        'ElseIf tip_nir_CB.SelectedValue = "CO" Then
        '    fact_avz = "factura fiscala nr. "
        'ElseIf tip_nir_CB.SelectedValue = "Tr" Then
        '    fact_avz = "aviz nr. "
        'End If
        'Dim auto As String = "cu auto nr.___________, avand ca document de insotire " & fact_avz & nr_factura & " / " & data_factura & ", constatand urmatoarele:"


        'graph_bon.DrawString(subsemnatii, text_font, XBrushes.Black,
        '                 New XRect(45, 67, 560, pdfPage_bon.Height.Point), XStringFormats.TopLeft)

        'graph_bon.DrawString(firma & " " & f_jur, top_font, XBrushes.Black,
        '                 New XRect(560, 67, 150, pdfPage_bon.Height.Point), XStringFormats.TopLeft)

        'graph_bon.DrawString(auto, text_font, XBrushes.Black,
        '                 New XRect(45, 84, 752, pdfPage_bon.Height.Point), XStringFormats.TopLeft)

        'graph_bon.DrawLine(pen, 45, 100, 797, 100) 'H

        'graph_bon.DrawLine(pen, 45, 100, 45, 132) ' V

        'graph_bon.DrawString("NR.", mic_font, XBrushes.Black,
        '                 New XRect(45, 104, 30, pdfPage_bon.Height.Point), XStringFormats.TopCenter)
        'graph_bon.DrawString("CRT.", mic_font, XBrushes.Black,
        '                 New XRect(45, 114, 30, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        'graph_bon.DrawLine(pen, 75, 100, 75, 132) ' V

        'graph_bon.DrawString("Denumirea marfii", mic_font, XBrushes.Black,
        '                 New XRect(75, 109, 190, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        'graph_bon.DrawLine(pen, 265, 100, 265, 132) ' V

        'graph_bon.DrawString("U.M.", mic_font, XBrushes.Black,
        '                 New XRect(265, 109, 30, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        'graph_bon.DrawLine(pen, 295, 100, 295, 132) ' V

        'graph_bon.DrawString("Cantitate", mic_font, XBrushes.Black,
        '                 New XRect(295, 109, 100, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        'graph_bon.DrawLine(pen, 395, 100, 395, 132) ' V

        'graph_bon.DrawString("Pret Intrare", mic_font, XBrushes.Black,
        '                 New XRect(395, 109, 100, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        'graph_bon.DrawLine(pen, 495, 100, 495, 132) ' V

        'graph_bon.DrawString("Valoare Intrare", mic_font, XBrushes.Black,
        '                 New XRect(495, 109, 100, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        'graph_bon.DrawLine(pen, 595, 100, 595, 132) ' V

        'graph_bon.DrawString("Pret Vanzare", mic_font, XBrushes.Black,
        '                 New XRect(595, 109, 100, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        'graph_bon.DrawLine(pen, 695, 100, 695, 132) ' V

        'graph_bon.DrawString("Valoare Vanzare", mic_font, XBrushes.Black,
        '                 New XRect(695, 109, 101, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        'graph_bon.DrawLine(pen, 797, 100, 797, 132) ' V

        ''graph_bon.DrawLine(pen, strtH, strtV, length, stopV)
        'graph_bon.DrawLine(pen, 45, 132, 797, 132) ' H

        Dim nxtRow As Integer = 140
        Dim DGV As DataGridView = DataGridView1
        For i = 0 To DGV.Rows.Count - 1
            Dim crt As String = DGV.Rows(i).Cells("crt").Value.ToString
            Dim produs As String = DGV.Rows(i).Cells("produs").Value.ToString
            Dim buc As String = DGV.Rows(i).Cells("buc").Value.ToString
            Dim cant As Decimal = CDec(DGV.Rows(i).Cells("cant").Value.ToString)
            Dim pret_intrare As Decimal = CDec(DGV.Rows(i).Cells("pr_ach_tva").Value.ToString)
            Dim valoare_intrare As Decimal = cant * pret_intrare
            'Dim pret_vanzare As Decimal = DGV.Rows(i).Cells("pret").Value.ToString
            'Dim valoare_vanzare As Decimal = DGV.Rows(i).Cells("valoare").Value.ToString



            'graph_bon.DrawLine(pen, 45, 100, 797, 100) 'H

            'graph_bon.DrawLine(pen, 45, 100, 45, 132) ' V

            graph_bon.DrawString(crt, mic_font, XBrushes.Black,
                             New XRect(45, nxtRow, 35, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

            'graph_bon.DrawLine(pen, 75, 100, 75, 132) ' V

            graph_bon.DrawString(produs, mic_font, XBrushes.Black,
                             New XRect(85, nxtRow, 295, pdfPage_bon.Height.Point), XStringFormats.TopLeft)

            'graph_bon.DrawLine(pen, 265, 100, 265, 132) ' V

            graph_bon.DrawString(cant, mic_font, XBrushes.Black,
                             New XRect(375, nxtRow, 75, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

            'graph_bon.DrawLine(pen, 295, 100, 295, 132) ' V

            graph_bon.DrawString(buc, mic_font, XBrushes.Black,
                             New XRect(450, nxtRow, 75, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

            graph_bon.DrawString(cant, mic_font, XBrushes.Black,
                             New XRect(525, nxtRow, 75, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

            'graph_bon.DrawLine(pen, 395, 100, 395, 132) ' V


            tf_bon.DrawString(Format(valoare_intrare, "#,#0.00"), mic_font, XBrushes.Black,
                             New XRect(600, nxtRow, 96.5, pdfPage_bon.Height.Point), XStringFormats.TopLeft)

            'graph_bon.DrawLine(pen, 595, 100, 595, 132) ' V

            tf_bon.DrawString(Format(valoare_intrare, "#,#0.00"), mic_font, XBrushes.Black,
                             New XRect(698.5, nxtRow, 96.5, pdfPage_bon.Height.Point), XStringFormats.TopLeft)

            'graph_bon.DrawLine(pen, 796, 100, 796, 132) ' V


            'graph_bon.DrawLine(pen, 45, 132, 797, 132) ' H
            nxtRow += 13
        Next

        If nxtRow < 290 Then
            nxtRow = 290
        End If
        graph_bon.DrawLine(pen, 45, 132, 45, nxtRow + 5) ' V
        graph_bon.DrawLine(pen, 797, 132, 797, nxtRow + 5) ' V
        graph_bon.DrawLine(pen, 45, nxtRow + 5, 797, nxtRow + 5) ' H
        'graph_bon.DrawLine(pen, 45, 480, 797, 480) ' H

        'graph_bon.DrawLine(pen, 45, 480, 45, 555) ' V

        graph_bon.DrawString("Membrii comisiei (Nume, Prenume, Semnatura)", mic_font, XBrushes.Black,
                        New XRect(45, nxtRow + 20, 250, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        'graph_bon.DrawLine(pen, 295, 480, 295, 555) ' V


        'Dim total_vanzare As Decimal = CDec(tot_valoare_TB.Text)
        'Dim total_intrare As Decimal = CDec(tot_pret_TB.Text)

        'graph_bon.DrawString("TOTAL", top_font, XBrushes.Black,
        '               New XRect(295, 485, 200, pdfPage_bon.Height.Point), XStringFormats.TopCenter)

        'graph_bon.DrawLine(pen, 495, 480, 495, 555) ' V

        'tf_bon.DrawString(Format(total_intrare, "#,#0.00"), top_font, XBrushes.Black,
        '               New XRect(495, 485, 95, pdfPage_bon.Height.Point), XStringFormats.TopLeft)

        'graph_bon.DrawLine(pen, 595, 480, 595, 555) ' V

        'tf_bon.DrawString(Format(total_vanzare, "#,#0.00"), top_font, XBrushes.Black,
        '               New XRect(695, 485, 95, pdfPage_bon.Height.Point), XStringFormats.TopLeft)

        'graph_bon.DrawLine(pen, 797, 480, 797, 555) ' V


        'graph_bon.DrawLine(pen, 45, 555, 797, 555) ' H


        Dim pdf_bon_Filename As String = folder_nir & "BON_" & nir & "_" & tip & "_" & Format(CDate(data_nir), "yyyyMMdd") & "_" & mag & ".pdf"
        'MsgBox(pdf_bon_Filename)
        If System.IO.File.Exists(pdf_bon_Filename) = True Then
            Dim OkCancel As Integer = MsgBox("Fisierul exista. Inlocuiti?", MsgBoxStyle.OkCancel)
            If OkCancel = DialogResult.Cancel Then
                Exit Sub
            ElseIf OkCancel = DialogResult.OK Then
                pdf_bon.Save(pdf_bon_Filename)
                'Form_principal.Load_Nir_Listview()
                Process.Start(pdf_bon_Filename)
            End If
        Else : pdf_bon.Save(pdf_bon_Filename)
            'Form_principal.Load_Nir_Listview()
            Process.Start(pdf_bon_Filename)
        End If
    End Sub
End Class
