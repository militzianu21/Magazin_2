Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization
Imports System.IO

Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing.Layout
Public Class Form_afiseaza_obinv
    Dim posY As Integer = 10
    Dim nr As Integer = 1
    Dim tab_ind As Integer = 13
    Dim tva As String = 0
    Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Public folder_nir As String

    Private Sub Form_afiseaza_obinv_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Application.CurrentCulture = New CultureInfo("ro-RO")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("ro-RO")
        ' -------------------- LOAD setari

        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")

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
                    Dim dbconn As MySqlConnection = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
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

        upd_int_CHK.Checked = True

        Dim nir As String ' = ""
        If nir_TB.Text = Nothing Then
            nir = ""
            nir_TB.Enabled = True
        Else : nir = "AND obiecte_inventar.nir=" & nir_TB.Text
            nir_TB.Enabled = False
        End If

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")

            Dim sql_tot As String = "SELECT obiecte_inventar.id,obiecte_inventar.produs,obiecte_inventar.bucati,obiecte_inventar.pret,obiecte_inventar.pret_intrare,obiecte_inventar.data,niruri_obiecte.tva,niruri_obiecte.nr_factura,niruri_obiecte.cif_firma,niruri_obiecte.nume_firma " _
                                    & "FROM obiecte_inventar LEFT JOIN niruri_obiecte ON obiecte_inventar.nir=niruri_obiecte.nir " _
                                    & "WHERE obiecte_inventar.data=@data " & nir & " ORDER BY id asc"
            'Clipboard.SetText(sql_tot)
            Dim sda As New MySqlDataAdapter
            Dim dbtable As New DataTable
            Dim bsource As New BindingSource

            Try
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If

                dbcomm = New MySqlCommand(sql_tot, dbconn)

                dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value)
                sda.SelectCommand = dbcomm
                sda.Fill(dbtable)
                bsource.DataSource = dbtable
                sda.Update(dbtable)
            Catch ex As Exception
                MsgBox("Problem loading dbtable: " & ex.Message.ToString)
            End Try
            '--------load TVA
            If IsDBNull(dbtable.Rows(0).Item("tva")) Or IsNothing(dbtable.Rows(0).Item("tva").ToString) Then
                If DateTimePicker1.Value < CDate("01.01.2016") Then
                    tva = 24
                ElseIf DateTimePicker1.Value >= CDate("01.01.2016") AndAlso DateTimePicker1.Value < CDate("01.01.2017") Then
                    tva = 20
                ElseIf DateTimePicker1.Value > CDate("01.01.2017") Then
                    tva = 19
                End If
                tva_TB.ForeColor = Color.Red
            Else : tva = dbtable.Rows(0).Item("tva")
                tva_TB.ForeColor = Color.Black
            End If
            tva_TB.Text = tva

            '-----------load nr. Factura
            If IsDBNull(dbtable.Rows(0).Item("nr_factura")) = True Or IsNothing(dbtable.Rows(0).Item("nr_factura").ToString) Then
                fact_TB.Text = Nothing
            Else : fact_TB.Text = dbtable.Rows(0).Item("nr_factura").ToString
            End If

            '--------------------load firma-------cif
            Dim nume_firma As String = Nothing
            Dim forma_jur As String = Nothing
            Dim cif As String = Nothing

            If IsDBNull(dbtable.Rows(0).Item("cif_firma")) = True Or IsNothing(dbtable.Rows(0).Item("cif_firma").ToString) Then
                cif = Nothing
            Else : cif = dbtable.Rows(0).Item("cif_firma").ToString
            End If


            If IsDBNull(dbtable.Rows(0).Item("nume_firma")) = True Or IsNothing(dbtable.Rows(0).Item("nume_firma").ToString) Then
                nume_firma = Nothing
            Else
                nume_firma = (dbtable.Rows(0).Item("nume_firma").ToString)
                If nume_firma.Contains("S.R.L.") Then
                    nume_firma = Trim(nume_firma.Substring(0, nume_firma.Length - 6))
                End If
                If nume_firma.Contains("S.A.") Then
                    nume_firma = Trim(nume_firma.Substring(0, nume_firma.Length - 4))
                End If
            End If

            If IsNothing(cif) = False Or IsNothing(nume_firma) = False Then
                Try
                    If dbconn.State = ConnectionState.Closed Then
                        dbconn.Open()
                    End If
                    dbcomm = New MySqlCommand("SELECT firma,forma_juridica FROM firme WHERE cui=@cui OR firma=@firma", dbconn)
                    dbcomm.Parameters.AddWithValue("@cui", cif)
                    dbcomm.Parameters.AddWithValue("@firma", nume_firma)
                    dbread = dbcomm.ExecuteReader()
                    dbread.Read()
                    nume_firma = dbread("firma").ToString
                    forma_jur = dbread("forma_juridica").ToString
                Catch ex As Exception
                    MsgBox("Problem loading cui/firma: " & ex.Message.ToString)
                End Try
            End If

            firma_TB.Text = nume_firma
            f_jur_TB.Text = forma_jur
            cif_firma_TB.Text = cif

            Try
                '-----------load Grid
                For i = 0 To dbtable.Rows.Count - 1
                    DataGridView1.Rows.Add()

                    DataGridView1.Rows(i).Cells("crt").Value = i + 1
                    DataGridView1.Rows(i).Cells("id").Value = dbtable.Rows(i).Item("id").ToString
                    DataGridView1.Rows(i).Cells("produs").Value = dbtable.Rows(i).Item("produs").ToString
                    DataGridView1.Rows(i).Cells("buc").Value = "Buc"
                    DataGridView1.Rows(i).Cells("cant").Value = dbtable.Rows(i).Item("bucati").ToString

                    If IsDBNull(dbtable.Rows(i).Item("pret_intrare")) = True Or IsNothing(dbtable.Rows(i).Item("pret_intrare").ToString) Then
                        DataGridView1.Rows(i).Cells("pret_ach").Value = 0
                        DataGridView1.Rows(i).Cells("pret_ach_tva").Value = 0
                    Else : DataGridView1.Rows(i).Cells("pret_ach").Value = FormatNumber(CDec(dbtable.Rows(i).Item("pret_intrare")).ToString / CDec("1," & tva), 2, TriState.True, TriState.False, TriState.True)
                        DataGridView1.Rows(i).Cells("pret_ach_tva").Value = FormatNumber(CDec(dbtable.Rows(i).Item("pret_intrare")).ToString, 2, TriState.True, TriState.False, TriState.True)
                    End If

                    DataGridView1.Rows(i).Cells("pret").Value = dbtable.Rows(i).Item("pret").ToString
                    DataGridView1.Rows(i).Cells("valoare").Value = dbtable.Rows(i).Item("pret").ToString * dbtable.Rows(i).Item("bucati").ToString
                    DataGridView1.Rows(i).Cells("chk").Value = True
                    DataGridView1.Rows(i).Cells("edit").Value = "edit"
                    DataGridView1.Rows(i).Cells("del").Value = "del"

                    DataGridView1.Columns("cant").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    DataGridView1.Columns("pret_ach").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    DataGridView1.Columns("pret_ach_tva").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    DataGridView1.Columns("pret").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    DataGridView1.Columns("valoare").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                Next
            Catch ex As Exception
                MsgBox("Problem loading grid: " & ex.Message.ToString)
            End Try

            'DateTimePicker1.Value = CDate(dbtable.Rows(0).Item("data").ToString)
            'If IsDBNull(dbtable.Rows(0).Item("tva")) = True Or IsNothing(dbtable.Rows(0).Item("tva").ToString) Then
            '    If DateTimePicker1.Value < CDate("01.01.2017") Then
            '        tva = 20
            '    ElseIf DateTimePicker1.Value > CDate("01.01.2017") Then
            '        tva = 19
            '    End If
            '    tva_TB.ForeColor = Color.Red
            'Else : tva = dbtable.Rows(0).Item("tva").ToString
            '    tva_TB.ForeColor = Color.Black
            'End If




        End Using
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")

            Try
                dbconn.Open()
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
            Catch ex As Exception
                MsgBox("Problem loading firme in autocomplete: " & ex.Message.ToString)
            End Try
            Try
                dbcomm = New MySqlCommand("SELECT produs FROM marfa group by produs", dbconn)
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
                MsgBox("Problem loading marfa in autocomplete: " & ex.Message.ToString)
            End Try
            dbread.Close()
        End Using

        Dim produs As String = ""
        Dim bucati As Decimal = 0
        Dim pret As Decimal = 0
        Dim valoare As Decimal = 0
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
            pr_ach_tva = DataGridView1.Rows(i).Cells("pret_ach_tva").Value
            total_ach_tva = total_ach_tva + (pr_ach_tva * cant)
            valoare = valoare + DataGridView1.Rows(i).Cells("valoare").Value
        Next

        total_ach_tva = total_ach_tva
        total_valoare = valoare
        total_bucati = bucati


        tot_pret_TB.Text = total_ach_tva
        tot_valoare_TB.Text = total_valoare
        tot_cant_TB.Text = total_bucati
        Dim adaos As Decimal = 0
        If total_ach_tva = 0 Then
            total_ach_tva = 1
        End If
        adaos = ((total_valoare / total_ach_tva) * 100) - 100
        adaos_Lbl.Text = "Adaos: " & FormatNumber(adaos, 2, TriState.True, TriState.False, TriState.True) & " %"


        For l = 0 To DataGridView1.Columns.Count - 1
            DataGridView1.Rows(0).Cells(l).Style.BackColor = Color.LightPink
        Next

        Dim crt_ As String = DataGridView1.Rows(0).Cells("crt").Value.ToString
        Dim produs_ As String = DataGridView1.Rows(0).Cells("produs").Value.ToString
        Dim buc_ As String = DataGridView1.Rows(0).Cells("buc").Value.ToString
        Dim cant_ As String = DataGridView1.Rows(0).Cells("cant").Value.ToString
        Dim pr_ach_ As String = DataGridView1.Rows(0).Cells("pret_ach").Value.ToString
        Dim pr_ach_tva_ As String = DataGridView1.Rows(0).Cells("pret_ach_tva").Value.ToString
        Dim pret_ As String = DataGridView1.Rows(0).Cells("pret").Value.ToString
        Dim valoare_ As String = DataGridView1.Rows(0).Cells("valoare").Value.ToString

        crt_Lbl.Text = crt_
        produs_TB_1.Text = produs_
        buc_TB_1.Text = buc_
        cant_TB_1.Text = cant_
        pret_ach_TB_1.Text = pr_ach_
        pret_ach_tva_TB_1.Text = pr_ach_tva_
        pret_vanzare_TB_1.Text = pret_
        valoare_TB_1.Text = valoare_


        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.ClearSelection()
        nir_TB.SelectAll()
        nir_TB.Focus()

        AddHandler ComboBox3.SelectedValueChanged, AddressOf ComboBox3_SelectedValueChanged
        AddHandler DateTimePicker1.ValueChanged, AddressOf DateTimePicker1_ValueChanged
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
                DataGridView1.Rows(row).Cells("pret_ach").Value = CDec(pret_ach_TB_1.Text)
                DataGridView1.Rows(row).Cells("pret_ach_tva").Value = FormatNumber(CDec(pret_ach_tva_TB_1.Text), 2)
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
                DataGridView1.Rows(row).Cells("pret_ach").Value = CDec(pret_ach_TB_1.Text)
                DataGridView1.Rows(row).Cells("pret_ach_tva").Value = FormatNumber(CDec(pret_ach_tva_TB_1.Text), 2)
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
            If tva = 0 Then
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
                pr_ach_tva = DataGridView1.Rows(i).Cells("pret_ach_tva").Value

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

        'dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")

        Dim nir As String = nir_TB.Text
        Dim dat As String = DateTimePicker1.Value
        Dim tva As String = tva_TB.Text
        Dim firma As String = firma_TB.Text
        Dim nr_fact As String = fact_TB.Text
        Dim cif_firma As String = cif_firma_TB.Text
        Dim mag As String = ComboBox3.SelectedValue
        Dim valoare As Decimal = CDec(tot_valoare_TB.Text)

        If Trim(nir) = Nothing Or
            Trim(tva) = Nothing Or
            Trim(firma) = Nothing Or
            Trim(valoare) = Nothing Or
            ComboBox3.SelectedValue = Nothing Then
            MsgBox("Ceva Lipseste")
        Else

            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                Dim id As Integer = DataGridView1.Rows(i).Cells("id").Value
                Dim produs As String = StrConv(DataGridView1.Rows(i).Cells("produs").Value.ToString, VbStrConv.ProperCase)
                Dim bucati As Decimal = DataGridView1.Rows(i).Cells("cant").Value
                Dim pret As Decimal = DataGridView1.Rows(i).Cells("pret").Value
                Dim pr_intrare As Decimal = DataGridView1.Rows(i).Cells("pret_ach_tva").Value


                Dim sql_prod As String = "UPDATE marfa_obiecte SET produs=@produs,pret=@pret,bucati=@bucati,nir=@nir,data=@data,pret_intrare=@pret_intrare,magazin=@magazin WHERE id=@id"
                Try
                    dbcomm = New MySqlCommand(sql_prod, dbconn)

                    dbcomm.Parameters.AddWithValue("@produs", produs)
                    dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value.Date)
                    dbcomm.Parameters.AddWithValue("@pret", CDec(pret))
                    dbcomm.Parameters.AddWithValue("@bucati", bucati)
                    dbcomm.Parameters.AddWithValue("@nir", nir)
                    dbcomm.Parameters.AddWithValue("@pret_intrare", CDec(pr_intrare))
                    dbcomm.Parameters.AddWithValue("@magazin", mag)
                    dbcomm.Parameters.AddWithValue("@id", id)

                    dbcomm.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Failed to insert produse: " & ex.Message.ToString())
                End Try
            Next

            Dim sql_ins As String = "INSERT INTO niruri_obiecte (nir,data,valoare,tva,nr_factura,nume_firma,cif_firma,magazin) " _
                                    & "VALUES(@nir,@data,@valoare,@tva,@nr_factura,@nume_firma,@cif_firma,@magazin) " _
                                    & "ON DUPLICATE KEY UPDATE data=@data,valoare=@valoare,tva=@tva,nr_factura=@nr_factura,nume_firma=@nume_firma,cif_firma=@cif_firma,magazin=@magazin"
            Try


                dbcomm = New MySqlCommand(sql_ins, dbconn)
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
                MsgBox("Failed to insert nir: " & ex.Message.ToString())
            End Try
            transaction.Commit()
            'If upd_int_CHK.CheckState = CheckState.Checked Then

            '    transaction = dbconn.BeginTransaction()

            '    Dim sql_ins_nir As String = "INSERT INTO intrari (data,nr_nir,explicatii,suma,tip_document,magazin) VALUES(@data,@nr_nir,@explicatii,@suma,@tip_document,@magazin) " _
            '                                & " ON DUPLICATE KEY UPDATE data=@data,nr_nir=@nr_nir,explicatii=@explicatii,suma=@suma,tip_document=@tip_document,magazin=@magazin"

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
            '        MsgBox("Failed to insert nir in intrari: " & ex.Message.ToString())
            '    End Try
            '    transaction.Commit()

            'End If

            Form_NIRuri.Load_niruri()

            'Dim DGV As DataGridView = Form_NIRuri.DataGridView1
            'For i = 0 To DGV.Rows.Count - 1
            '    If DGV.Rows(i).Cells("nir").Value = nir Then
            '        DGV.ClearSelection()
            '        DGV.Rows(i).Selected = True
            '        If i > 0 Then
            '            DGV.FirstDisplayedScrollingRowIndex = i - 1
            '        Else
            '            DGV.FirstDisplayedScrollingRowIndex = i
            '        End If

            '        Exit For
            '    End If
            'Next
            'Me.Dispose()
        End If
    End Sub

    Private Sub enter_Keypress(ByVal sender As System.Object, ByVal e As KeyPressEventArgs) Handles buc_TB_1.KeyPress, pret_ach_TB_1.KeyPress, pret_ach_tva_TB_1.KeyPress, fact_TB.KeyPress, DateTimePicker1.KeyPress, DateTimePicker2.KeyPress, pret_vanzare_TB_1.KeyPress, tva_TB.KeyPress, produs_TB_1.KeyPress, valoare_TB_1.KeyPress, nir_TB.KeyPress, cant_TB_1.KeyPress
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

    Private Sub _Focused(sender As Object, e As EventArgs) Handles nir_TB.GotFocus, nir_TB.MouseClick, fact_TB.GotFocus, fact_TB.MouseClick, tva_TB.GotFocus, tva_TB.MouseClick, pret_ach_TB_1.GotFocus, pret_ach_TB_1.MouseClick, pret_ach_tva_TB_1.GotFocus, pret_ach_tva_TB_1.MouseClick, pret_vanzare_TB_1.GotFocus, pret_vanzare_TB_1.MouseClick, valoare_TB_1.GotFocus, valoare_TB_1.MouseClick
        Dim tb = DirectCast(sender, TextBox)
        tb.SelectAll()

    End Sub

    Private Sub ComboBox3_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedValueChanged

        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
        Else : PictureBox1.BackColor = Color.LightYellow
        End If
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        DateTimePicker2.Value = DateTimePicker1.Value
        If DateTimePicker1.Value < CDate("01.01.2016") Then
            tva = 24
        ElseIf DateTimePicker1.Value >= CDate("01.01.2016") AndAlso DateTimePicker1.Value < CDate("01.01.2017") Then
            tva = 20
        ElseIf DateTimePicker1.Value > CDate("01.01.2017") Then
            tva = 19
        End If
        tva_TB.Text = tva
    End Sub
    Private Sub firma_TB_Leave(sender As Object, e As EventArgs) Handles firma_TB.Leave
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
        If dbconn.State = ConnectionState.Closed Then
            dbconn.Open()
        End If
        dbcomm = New MySqlCommand("SELECT firma,cui,forma_juridica FROM firme WHERE firma LIKE '" & firma_TB.Text & "%'", dbconn)

        dbread = dbcomm.ExecuteReader()
        dbread.Read()
        If f_jur_TB.Text = Nothing Or Trim(firma_TB.Text) = Nothing Then
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

                        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
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

                dbread.Close() '---------------

            ElseIf dbread.HasRows = True Then
                'dbread = dbcomm.ExecuteReader()
                dbread.Read()
                f_jur_TB.Text = dbread("forma_juridica").ToString
                cif_firma_TB.Text = dbread("cui").ToString()

                dbread.Close() '---------------
            End If

            dbread.Close() '---------------
        End If
    End Sub

    Private Sub numere_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cant_TB_1.KeyPress, pret_ach_TB_1.KeyPress, pret_ach_tva_TB_1.KeyPress, pret_vanzare_TB_1.KeyPress, valoare_TB_1.KeyPress

        If Not Char.IsDigit(e.KeyChar) Then e.Handled = True
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 45 Or ((e.KeyChar = "." Or e.KeyChar = ",") And (sender.Text.IndexOf(".") = -1 And sender.Text.IndexOf(",") = -1)))
        If Asc(e.KeyChar) = 46 Then
            e.KeyChar = ChrW(44)
        End If
    End Sub

    Private Sub textbox_1_Leave(sender As Object, e As EventArgs) Handles cant_TB_1.Leave, pret_ach_TB_1.Leave, pret_ach_tva_TB_1.Leave, pret_vanzare_TB_1.Leave, valoare_TB_1.Leave ',produs_TB_1.Leave,buc_TB_1.Leave,

        pret_ach_tva_TB_1.Text = FormatNumber(CDec(pret_ach_TB_1.Text) + (CDec(pret_ach_TB_1.Text) * tva / 100), 2)
        valoare_TB_1.Text = CDec(cant_TB_1.Text) * CDec(pret_vanzare_TB_1.Text)
        ad_prod_But.Enabled = True




    End Sub

    Private Sub produs_TB_1_TextChanged(sender As Object, e As EventArgs) Handles produs_TB_1.TextChanged
        If Trim(produs_TB_1.Text) = Nothing Then
            produs_TB_1.BackColor = Color.Red
        Else : produs_TB_1.BackColor = Color.White
        End If

    End Sub

    Private Sub cant_TB_1_TextChanged(sender As Object, e As EventArgs) Handles cant_TB_1.TextChanged
        If cant_TB_1.Text = "" Then
            cant_TB_1.Text = 0
            cant_TB_1.SelectAll()
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
                Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

                For j = 0 To DataGridView1.Rows.Count - 1
                    For k = 0 To DataGridView1.Columns.Count - 1
                        DataGridView1.Rows(j).Cells(k).Style.BackColor = Color.White
                    Next
                Next
                For l = 0 To DataGridView1.Columns.Count - 1
                    row.Cells(l).Style.BackColor = Color.LightPink
                Next

                Dim crt As String = row.Cells("crt").Value.ToString
                Dim produs As String = row.Cells("produs").Value.ToString
                Dim buc As String = row.Cells("buc").Value.ToString
                Dim cant As String = row.Cells("cant").Value.ToString
                Dim pr_ach As String = row.Cells("pret_ach").Value.ToString
                Dim pr_ach_tva As String = row.Cells("pret_ach_tva").Value.ToString
                Dim pret As String = row.Cells("pret").Value.ToString
                Dim valoare As String = row.Cells("valoare").Value.ToString

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

            If grid.Columns(e.ColumnIndex).Name = "del" Then
                Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
                DataGridView1.Rows.Remove(row)
                If DataGridView1.Rows.Count > 0 Then
                    For j = 0 To DataGridView1.Rows.Count - 1
                        For k = 0 To DataGridView1.Columns.Count - 1
                            DataGridView1.Rows(j).Cells(k).Style.BackColor = Color.White
                        Next
                    Next
                    For l = 0 To DataGridView1.Columns.Count - 1
                        DataGridView1.Rows(0).Cells(l).Style.BackColor = Color.LightPink
                    Next

                    crt_Lbl.Text = DataGridView1.Rows(0).Cells("crt").Value.ToString
                    produs_TB_1.Text = DataGridView1.Rows(0).Cells("produs").Value.ToString
                    buc_TB_1.Text = DataGridView1.Rows(0).Cells("buc").Value.ToString
                    cant_TB_1.Text = DataGridView1.Rows(0).Cells("cant").Value.ToString
                    pret_ach_TB_1.Text = DataGridView1.Rows(0).Cells("pret_ach").Value.ToString
                    pret_ach_tva_TB_1.Text = DataGridView1.Rows(0).Cells("pret_ach_tva").Value.ToString
                    pret_vanzare_TB_1.Text = DataGridView1.Rows(0).Cells("pret").Value.ToString
                    valoare_TB_1.Text = DataGridView1.Rows(0).Cells("valoare").Value.ToString

                ElseIf DataGridView1.Rows.Count = 0 Then
                    crt_Lbl.Text = 1
                    produs_TB_1.Text = ""
                    buc_TB_1.Text = "Buc"
                    cant_TB_1.Text = 1
                    pret_ach_TB_1.Text = 0
                    pret_ach_tva_TB_1.Text = 0
                    pret_vanzare_TB_1.Text = 0
                    valoare_TB_1.Text = 0
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
                pr_ach_tva_ = DataGridView1.Rows(i).Cells("pret_ach_tva").Value
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

    Private Sub tva_TB_TextChanged(sender As Object, e As EventArgs) Handles tva_TB.TextChanged
        If tva_TB.Text = "" Then
            tva_TB.Text = 0
            tva_TB.SelectAll()
        End If
        If pret_ach_TB_1.Text = "" Then
            pret_ach_TB_1.Text = 0
        End If
        tva = CInt(tva_TB.Text)
        pret_ach_tva_TB_1.Text = CDec(pret_ach_TB_1.Text) + (CDec(pret_ach_TB_1.Text) * tva / 100)
    End Sub

    Private Sub pret_ach_tva_TB_1_TextChanged(sender As Object, e As EventArgs) Handles pret_ach_tva_TB_1.TextChanged
        If tva_TB.Text = "" Then
            tva_TB.Text = 0
            tva_TB.SelectAll()
        End If
        If pret_ach_tva_TB_1.Text = "" Then
            pret_ach_tva_TB_1.Text = 0
            pret_ach_tva_TB_1.SelectAll()
        End If
        tva = CInt(tva_TB.Text)
        pret_ach_TB_1.Text = FormatNumber(CDec(pret_ach_tva_TB_1.Text) / ((100 + tva) / 100), 2)
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
                         New XRect(687, 24, 80, pdfPage.Height.Point), XStringFormats.TopLeft)
        tf.DrawString(nir, titlu_font, XBrushes.Black,
                         New XRect(747, 20, 50, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawLine(pen, 697, 35, 140, 35)

        graph.DrawString("Data: ", text_font, XBrushes.Black,
                        New XRect(712, 37, 80, pdfPage.Height.Point), XStringFormats.TopLeft)
        tf.DrawString(data_nir, top_font, XBrushes.Black,
                        New XRect(697, 37, 100, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawLine(pen, 697, 52, 140, 52)
        Dim subsemnatii As String = "Subsemnatii, membrii comisiei de receptie, am procedat la receptionarea valorilor materiale furnizate de "

        Dim auto As String = "cu auto nr.___________, avand ca document de insotire factura fiscala nr. " & nr_factura & " / " & data_factura & ", constatand urmatoarele:"


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
            Dim pret_intrare As Decimal = CDec(DGV.Rows(i).Cells("pret_ach_tva").Value.ToString)
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


        Dim pdfFilename As String = folder_nir & "NIR_" & nir & "_" & Format(CDate(data_nir), "yyyyMMdd") & "_" & mag & ".pdf"

        If System.IO.File.Exists(pdfFilename) = True Then
            Dim OkCancel As Integer = MsgBox("Fisierul exista. Inlocuiti?", MsgBoxStyle.OkCancel)
            If OkCancel = DialogResult.Cancel Then
                Exit Sub
            ElseIf OkCancel = DialogResult.OK Then
                pdf.Save(pdfFilename)
                Form_principal.Load_Nir_Listview()
                Process.Start(pdfFilename)
            End If
        Else : pdf.Save(pdfFilename)
            Form_principal.Load_Nir_Listview()
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
        pdf_bon.Info.Title = "BON CONSUM"
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
            Dim pret_intrare As Decimal = CDec(DGV.Rows(i).Cells("pret_ach_tva").Value.ToString)
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


            tf_bon.DrawString(Format(pret_intrare, "#,#0.00"), mic_font, XBrushes.Black,
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
