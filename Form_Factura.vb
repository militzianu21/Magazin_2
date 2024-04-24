Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization
Imports System.IO

Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing.Layout
Public Class Form_adauga_factura
    Dim posY As Integer = 10
    Dim nr As Integer = 1
    Dim tab_ind As Integer = 13
    Dim tva As Integer = 0

    Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Public folder_nir As String
    Private Sub Form_adauga_factura_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        'Try
        '    Dim set_sql As String = "SELECT * FROM setari WHERE setare='tva'"
        '    If dbconn.State = ConnectionState.Closed Then
        '        dbconn.Open()
        '    End If
        '    dbcomm = New MySqlCommand(set_sql, dbconn)
        '    dbread = dbcomm.ExecuteReader()
        '    dbread.Read()
        '    If dbread.HasRows = True Then
        '        tva = CInt(dbread("valoare"))
        '    End If

        'Catch ex As Exception
        '    MsgBox("Problem loading setari: tva: " & ex.Message.ToString)
        'End Try
        'dbread.Close()

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
            MsgBox("Problem loading  firme in autocomplete: " & ex.Message.ToString)
        End Try
        dbread.Close()

        Dim sql_read As String = "SELECT numar,data FROM facturi ORDER BY data DESC, numar DESC LIMIT 1"
        Try
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_read, dbconn)

            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = False Then
                nr_fact_TB.Text = 1
            Else
                nr_fact_TB.Text = (dbread("numar").ToString) + 1
            End If
            'Dim data_fact As Date = Today
            'data_fact = dbread("data")
            data_DTP.Value = Today
            'data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)


        Catch ex As Exception
            MsgBox("Problem loading information (form load): " & ex.Message.ToString)

        End Try
        dbread.Close()

            Try
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If

            dbcomm = New MySqlCommand("SELECT nume FROM delegati GROUP BY nume", dbconn)
            Dim lst3 As New List(Of String)
                dbread = dbcomm.ExecuteReader()
                While dbread.Read()
                lst3.Add(dbread("nume").ToString())
                End While
                Dim mysource As New AutoCompleteStringCollection
            mysource.AddRange(lst3.ToArray)
                nume_TB.AutoCompleteSource = AutoCompleteSource.CustomSource
                nume_TB.AutoCompleteCustomSource = mysource
                nume_TB.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                dbread.Close()

            Catch ex As Exception
                MsgBox("Problem loading delegati in autocomplete: " & ex.Message.ToString)
            End Try
            crt_Lbl.Text = row + 1
            buc_TB_1.Text = "Buc"
            cant_TB_1.Text = 1
            pret_unitar_TB.Text = 0
            valoare_TB.Text = 0
            tot_valoare_TB.Text = 0

            'AddHandler ComboBox3.SelectedValueChanged, AddressOf ComboBox3_SelectedValueChanged
    End Sub

    Dim row As Integer = 0
    Dim edit_mode As Boolean = False

    Private Sub ad_prod_But_Click(sender As Object, e As EventArgs) Handles ad_prod_But.Click
        Dim bun As Boolean = True

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

        If pret_unitar_TB.Text = 0 Then
            pret_unitar_TB.BackColor = Color.Red
            bun = False
        Else : pret_unitar_TB.BackColor = Color.White
        End If

        If pret_unitar_TB.Text = 0 Then
            pret_unitar_TB.BackColor = Color.Red
            bun = False
        Else : pret_unitar_TB.BackColor = Color.White
        End If

      
        If valoare_TB.Text = 0 Then
            valoare_TB.BackColor = Color.Red
            bun = False
        Else : valoare_TB.BackColor = Color.White
        End If

        If bun = True Then
            If edit_mode = False Then

                DataGridView1.Rows.Add()
                DataGridView1.Rows(row).Cells("crt").Value = crt_Lbl.Text
                DataGridView1.Rows(row).Cells("produs").Value = StrConv(produs_TB_1.Text, VbStrConv.ProperCase)
                DataGridView1.Rows(row).Cells("buc").Value = buc_TB_1.Text
                DataGridView1.Rows(row).Cells("cant").Value = CDec(cant_TB_1.Text)
                DataGridView1.Rows(row).Cells("pret_unitar").Value = CDec(pret_unitar_TB.Text)
                DataGridView1.Rows(row).Cells("valoare").Value = CDec(valoare_TB.Text)
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
                DataGridView1.Rows(row).Cells("pret_unitar").Value = CDec(pret_unitar_TB.Text)
                DataGridView1.Rows(row).Cells("valoare").Value = CDec(valoare_TB.Text)
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
            pret_unitar_TB.Text = 0
            'If tva = 0 Then
            '    pret_ach_tva_TB_1.Text = CDec(pret_ach_TB_1.Text)
            'Else : pret_ach_tva_TB_1.Text = CDec(pret_ach_TB_1.Text) * tva / 100
            'End If
            'pret_vanzare_TB_1.Text = 0
            valoare_TB.Text = 0
            '-----------------------------------------

            Dim produs As String = ""
            Dim bucati As Decimal = 0
            Dim pret As Decimal = 0
            Dim valoare As Decimal = 0
            Dim numar As String = nr_fact_TB.Text()

            Dim cant As Decimal = 0
            Dim pr_unit As Decimal = 0
            Dim dat As String = data_DTP.Value
            Dim total_bucati As Decimal = 0
            Dim total_valoare As Decimal = 0


            For i As Integer = 0 To DataGridView1.RowCount - 1
                bucati = bucati + DataGridView1.Rows(i).Cells("cant").Value

                cant = DataGridView1.Rows(i).Cells("cant").Value

                valoare = valoare + DataGridView1.Rows(i).Cells("valoare").Value
            Next

            total_valoare = valoare
            total_bucati = bucati


            tot_valoare_TB.Text = total_valoare
            tot_cant_TB.Text = total_bucati
            'adaos_Lbl.Text = "Adaos: " & FormatNumber((total_valoare / total_ach_tva * 100 - 100), 2, TriState.True, TriState.False, TriState.True) & " %"
            produs_TB_1.Focus()

        ElseIf bun = False Then

        End If
        edit_mode = False
        ad_prod_But.Image = Global.Magazin.My.Resources.Resources.ic_input_add
        'AddHandler ComboBox3.SelectedValueChanged, AddressOf ComboBox3_SelectedValueChanged
    End Sub

    Private Sub Save_Bu_Click(sender As Object, e As EventArgs) Handles save_Bu.Click
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")


        Dim nr_fact As String = nr_fact_TB.Text
        Dim dat As String = data_DTP.Value
        Dim firma As String = firma_TB.Text
        Dim cif_firma As String = cif_firma_TB.Text
        Dim mag As String = ComboBox3.SelectedValue
        Dim valoare As Decimal = CDec(tot_valoare_TB.Text)
        Dim plata As String = ""


        Form_aleg_Plata.ShowDialog()
        If DialogResult.OK Then

            Form_Bon.ShowDialog()
            Form_Bon.data_bon_DTP.Value = data_DTP.Value

            If DialogResult.OK Then
                Dim maga As String = ""
                If Form_Bon.ComboBox3.SelectedValue = "PM" Then
                    maga = ", magazin Petru Maior 9"
                ElseIf Form_Bon.ComboBox3.SelectedValue = "MV" Then
                    maga = ", magazin Mihai Viteazu 28"
                End If
                plata = "Plata conform bon fiscal nr. " & Form_Bon.nr_bon.Text & " din data de " & Format(Form_Bon.data_bon_DTP.Value, "dd.MM.yyy") & maga
            End If
            obs_TB.Text = plata
        End If
        'MsgBox(plata)
        If dbconn.State = ConnectionState.Closed Then
            dbconn.Open()
        End If
        Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
  
        Dim sql_fact As String = "INSERT INTO facturi (numar,data,valoare,cui_firma,tva,plata) VALUES(@numar,@data,@valoare,@cui_firma,@tva,@plata)"

        Try

            dbcomm = New MySqlCommand(sql_fact, dbconn)
            dbcomm.Parameters.AddWithValue("@numar", nr_fact)
            dbcomm.Parameters.AddWithValue("@data", data_DTP.Value.Date)
            dbcomm.Parameters.AddWithValue("@valoare", valoare)
            dbcomm.Parameters.AddWithValue("@cui_firma", cif_firma)
            dbcomm.Parameters.AddWithValue("@tva", tva)
            dbcomm.Parameters.AddWithValue("@plata", plata)
            dbcomm.ExecuteNonQuery()


        Catch ex As Exception
            MsgBox("Failed to insert into facturi: " & ex.Message.ToString())
        End Try

        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            'Dim nr_fact As String = StrConv(DataGridView1.Rows(i).Cells("produs").Value.ToString, VbStrConv.ProperCase)
            Dim produs As String = StrConv(DataGridView1.Rows(i).Cells("produs").Value.ToString, VbStrConv.ProperCase)
            Dim bucati As Decimal = DataGridView1.Rows(i).Cells("cant").Value
            Dim pret As Decimal = DataGridView1.Rows(i).Cells("pret_unitar").Value


            Dim sql_prod As String = "INSERT INTO prod_facturi (nr_fact,produs,pret,buc) VALUES(@nr_fact,@produs,@pret,@buc)"

            Try

                dbcomm = New MySqlCommand(sql_prod, dbconn)
                dbcomm.Parameters.AddWithValue("@nr_fact", nr_fact)
                dbcomm.Parameters.AddWithValue("@produs", produs)
                dbcomm.Parameters.AddWithValue("@pret", pret)
                dbcomm.Parameters.AddWithValue("@buc", bucati)


                dbcomm.ExecuteNonQuery()


            Catch ex As Exception
                MsgBox("Failed to insert into prod_fact: " & ex.Message.ToString())
            End Try
        Next

        Dim sql_deleg As String = "INSERT INTO delegati (nr_fact,nume,serie,numar,cui_firma,data_elib) " _
                                  & "VALUES(@nr_fact,@nume,@serie,@numar,@cui_firma,@data_elib) " _
                                  & "ON DUPLICATE KEY UPDATE nume=@nume,serie=@serie,numar=@numar,cui_firma=@cui_firma,data_elib=@data_elib"
  
        Try

            dbcomm = New MySqlCommand(sql_deleg, dbconn)
            dbcomm.Parameters.AddWithValue("@nr_fact", nr_fact)
            dbcomm.Parameters.AddWithValue("@nume", StrConv(nume_TB.Text, VbStrConv.ProperCase))
            dbcomm.Parameters.AddWithValue("@serie", StrConv(seria_TB.Text, VbStrConv.Uppercase))
            dbcomm.Parameters.AddWithValue("@numar", nr_CI_TB.Text)
            dbcomm.Parameters.AddWithValue("@cui_firma", cif_firma)
            dbcomm.Parameters.AddWithValue("@data_elib", eliber_TB.Text)

            dbcomm.ExecuteNonQuery()


        Catch ex As Exception
            MsgBox("Failed to insert into delegati: " & ex.Message.ToString())
        End Try
        transaction.Commit()


        Dim yes_no As DialogResult = MsgBox("Printezi Factura", MsgBoxStyle.YesNo)
        If yes_no = Windows.Forms.DialogResult.Yes Then
            Print_BU.PerformClick()
        End If

        dbconn.Close()

        nr_fact_TB.Text = nr_fact + 1
        firma_TB.Clear()
        f_jur_TB.Clear()
        J_firma_TB.Clear()
        cif_firma_TB.Clear()
        ro_CB.CheckState = CheckState.Unchecked
        adresa_tb.Clear()
        localitate_TB.Clear()
        judet_TB.Clear()
        nume_TB.Clear()
        seria_TB.Clear()
        nr_CI_TB.Clear()
        eliber_TB.Clear()

        crt_Lbl.Text = 1
        DataGridView1.Rows.Clear()
        row = 0
        tot_cant_TB.Clear()
        tot_valoare_TB.Clear()

        'Form_NIRuri.Load_niruri()
        Me.Focus()

    End Sub

    Private Sub enter_Keypress(ByVal sender As System.Object, ByVal e As KeyPressEventArgs) Handles buc_TB_1.KeyPress, pret_unitar_TB.KeyPress, nr_fact_TB.KeyPress, data_DTP.KeyPress, produs_TB_1.KeyPress, valoare_TB.KeyPress, cant_TB_1.KeyPress, seria_TB.KeyPress, nr_CI_TB.KeyPress, eliber_TB.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If
    End Sub
    Private Sub _Keydown_TAB(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles firma_TB.KeyDown, produs_TB_1.KeyDown, nume_TB.KeyDown
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


    Private Sub firma_TB_Leave(sender As Object, e As EventArgs) Handles firma_TB.Leave
        tva = tva
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        If dbconn.State = ConnectionState.Closed Then
            dbconn.Open()
        End If
        dbcomm = New MySqlCommand("SELECT id,firma,forma_juridica,cui,j,adresa,localitate,judet,cont,banca,tva FROM firme WHERE firma LIKE '" & firma_TB.Text & "%'", dbconn)

        dbread = dbcomm.ExecuteReader()
        dbread.Read()
        'If Trim(f_jur_TB.Text) = Nothing Or Trim(firma_TB.Text) = Nothing Then
        If dbread.HasRows = False Or Trim(firma_TB.Text) = Nothing Then
            dbread.Close() '---------------
            Form_firme_introd.tip_text.Text = "Client"
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
                ElseIf Form_firme_introd.CheckBox1.CheckState = CheckState.Unchecked Then
                    tva = False
                    'End If
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

                        dbread.Close() '---------------
                    End Using

                    dbread.Close() '---------------
                Catch ex As Exception
                    MsgBox("Nu s-a modificat: " & ex.Message.ToString)
                End Try

                Form_principal.Load_Firme()
                dbcomm = New MySqlCommand("SELECT id,firma,forma_juridica,cui,tva,j,adresa,localitate,judet,cont,banca FROM firme WHERE firma LIKE '%" & firma & "%'", dbconn)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                id_Lbl.Text = dbread("id").ToString
                f_jur_TB.Text = dbread("forma_juridica").ToString
                cif_firma_TB.Text = dbread("cui").ToString()
                If CBool(dbread("tva")) = True Then
                    ro_CB.CheckState = CheckState.Checked
                Else 'If dbread("ro").ToString = False Then
                    ro_CB.CheckState = CheckState.Unchecked
                End If
                firma_TB.Text = firma
                firma_TB.SelectAll()
                firma_TB.Focus()
                J_firma_TB.Text = dbread("j").ToString()
                adresa_tb.Text = dbread("adresa").ToString()
                localitate_TB.Text = dbread("localitate").ToString()
                judet_TB.Text = dbread("judet").ToString()
                cont_TB.Text = dbread("cont").ToString()
                banca_TB.Text = dbread("banca").ToString()

                dbread.Close() '---------------
            End If

            'dbread.Close() '---------------

        ElseIf dbread.HasRows = True Then
            'dbread = dbcomm.ExecuteReader()
            dbread.Read()
            id_Lbl.Text = dbread("id").ToString
            f_jur_TB.Text = dbread("forma_juridica").ToString
            cif_firma_TB.Text = dbread("cui").ToString()
            If IsDBNull(dbread("tva")) = True Then
                ro_CB.CheckState = CheckState.Unchecked
            ElseIf IsDBNull(dbread("tva")) = False Then
                If CBool(dbread("tva")) = True Then
                    ro_CB.CheckState = CheckState.Checked
                Else 'If dbread("ro").ToString = False Then
                    ro_CB.CheckState = CheckState.Unchecked
                End If
            End If

            J_firma_TB.Text = dbread("j").ToString()
            adresa_tb.Text = dbread("adresa").ToString()
            localitate_TB.Text = dbread("localitate").ToString()
            judet_TB.Text = dbread("judet").ToString()
            cont_TB.Text = dbread("cont").ToString()
            banca_TB.Text = dbread("banca").ToString()
            Dim tva_bool As Boolean = True

            dbread.Close() '---------------
        End If

        dbread.Close() '---------------

        'If IsNumeric(cif_firma_TB.Text) = True Then

        '    Try
        '        If dbconn.State = ConnectionState.Closed Then
        '            dbconn.Open()
        '        End If

        '        dbcomm = New MySqlCommand("SELECT nume FROM delegati WHERE cui_firma=" & cif_firma_TB.Text & "", dbconn)
        '        Dim lst As New List(Of String)
        '        dbread = dbcomm.ExecuteReader()
        '        While dbread.Read()
        '            lst.Add(dbread("nume").ToString())
        '        End While
        '        Dim mysource As New AutoCompleteStringCollection
        '        mysource.AddRange(lst.ToArray)
        '        nume_TB.AutoCompleteSource = AutoCompleteSource.CustomSource
        '        nume_TB.AutoCompleteCustomSource = mysource
        '        nume_TB.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        '        dbread.Close()

        '    Catch ex As Exception
        '        MsgBox("Problem loading delegati in autocomplete: " & ex.Message.ToString)
        '    End Try

        'End If
        dbread.Close()
        Form_firme_introd.Close()
        'For Each contr As Control In Form_firme_introd.Controls
        '    contr.Text = 1
        'Next
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

    Private Sub numere_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cant_TB_1.KeyPress, pret_unitar_TB.KeyPress, valoare_TB.KeyPress

        If Not Char.IsDigit(e.KeyChar) Then e.Handled = True
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 45 Or ((e.KeyChar = "." Or e.KeyChar = ",") And (sender.Text.IndexOf(".") = -1 And sender.Text.IndexOf(",") = -1)))
        If Asc(e.KeyChar) = 46 Then
            e.KeyChar = ChrW(44)
        End If
    End Sub

    Private Sub textbox_1_Leave(sender As Object, e As EventArgs) Handles produs_TB_1.Leave, buc_TB_1.Leave, cant_TB_1.Leave, pret_unitar_TB.Leave, valoare_TB.Leave

        valoare_TB.Text = CDec(cant_TB_1.Text) * CDec(pret_unitar_TB.Text)
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
                    pret_unitar_TB.Text = 0
                    'If tva = 0 Then
                    '    pret_ach_tva_TB_1.Text = CDec(pret_ach_TB_1.Text)
                    'Else : pret_ach_tva_TB_1.Text = CDec(pret_ach_TB_1.Text) * tva / 100
                    'End If
                    'pret_vanzare_TB_1.Text = 0
                    valoare_TB.Text = 0
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
                    Dim pret_unitar As String = rand.Cells("pret_unitar").Value.ToString
                    Dim valoare As String = rand.Cells("valoare").Value.ToString

                    crt_Lbl.Text = crt
                    produs_TB_1.Text = produs
                    buc_TB_1.Text = buc
                    cant_TB_1.Text = cant
                    pret_unitar_TB.Text = pret_unitar
                    valoare_TB.Text = valoare
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
            Dim pret_unitar_ As Decimal = 0
            Dim dat As String = data_DTP.Value
            Dim total_bucati_ As Decimal = 0
            Dim total_valoare_ As Decimal = 0


            For i As Integer = 0 To DataGridView1.RowCount - 1
                bucati_ = bucati_ + DataGridView1.Rows(i).Cells("cant").Value
                cant_ = DataGridView1.Rows(i).Cells("cant").Value
                pret_unitar_ = DataGridView1.Rows(i).Cells("pret_unitar").Value
                valoare_ = valoare_ + DataGridView1.Rows(i).Cells("valoare").Value
            Next

            total_valoare_ = valoare_
            total_bucati_ = bucati_


            tot_valoare_TB.Text = total_valoare_
            tot_cant_TB.Text = total_bucati_

        End If

    End Sub

    Private Sub Print_BU_Click(sender As Object, e As EventArgs) Handles Print_BU.Click
        ' -------------------- LOAD setari

        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        Dim folder_facturi As String = ""
        Try
            Dim set_sql As String = "SELECT * FROM setari WHERE setare='path_facturi'"
            dbconn.Open()
            dbcomm = New MySqlCommand(set_sql, dbconn)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = False Then
                SaveFileDialog1.Title = "Selectati Folderul pt Facturi"
                SaveFileDialog1.FileName = "Selectati Folderul"
                SaveFileDialog1.Filter = "pdf Files (*.pdf*)|*.pdf"

                If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim folder As String = System.IO.Path.GetDirectoryName(SaveFileDialog1.FileName) & "\"
                    Dim dbconn As MySqlConnection = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
                    Try
                        If dbconn.State = 0 Then
                            dbconn.Open()
                        End If

                        Dim set_path_sql As String = "REPLACE INTO setari(setare,valoare) VALUES('path_facturi',@folder)"
                        Using dbcomm As MySqlCommand = New MySqlCommand(set_path_sql, dbconn)
                            dbcomm.Parameters.AddWithValue("@folder", folder)
                            dbcomm.ExecuteNonQuery()
                        End Using
                    Catch ex As Exception
                        MsgBox("Problem Nonquery: " & ex.Message.ToString)
                    End Try
                End If

            ElseIf dbread.HasRows = True Then
                folder_facturi = dbread("valoare")
            End If

        Catch ex As Exception
            MsgBox("Problem loading setari savefile: " & ex.Message.ToString)
        End Try
        dbread.Close()
        '--------------------------------
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
        Dim data_factura As String = Format(data_DTP.Value, "dd.MM.yyyy")
        Dim nr_factura As String = nr_fact_TB.Text
        Dim firma As String = firma_TB.Text
        Dim f_jur As String = f_jur_TB.Text

        Dim ro As String = ""
        If ro_CB.CheckState = CheckState.Checked Then
            ro = "RO"
        ElseIf ro_CB.CheckState = CheckState.Unchecked Then
            ro = ""
        End If
        Dim cui_firma As String = ro & cif_firma_TB.Text
        Dim j_firma As String = J_firma_TB.Text
        Dim adresa As String = adresa_tb.Text
        Dim localitate As String = localitate_TB.Text
        Dim judet As String = judet_TB.Text
        Dim observatii As String = obs_TB.Text


        'Dim nr_rzf As String = row.Cells("nr_rzf").Value.ToString
        'Dim tip_incasare As String = row.Cells("tip_incasare").Value.ToString
        'Dim explicatii As String = row.Cells("explicatii").Value
        'Dim suma As String = row.Cells("suma_cash").Value

        Dim pdf As PdfSharp.Pdf.PdfDocument = New PdfSharp.Pdf.PdfDocument
        pdf.Info.Title = "NIR"
        Dim pdfPage As PdfSharp.Pdf.PdfPage = pdf.AddPage
        pdfPage.Orientation = PageOrientation.Portrait

        '        // Step 1: Create an XForm and draw some graphics on it

        '// Create an empty XForm object with the specified width and height
        '// A form is bound to its target document when it is created. The reason is 
        '// that the form can share fonts and other objects with its target document.
        Dim form As XForm = New XForm(pdf, 595, 421)

        '// Create an XGraphics object for drawing the contents of the form.
        Dim graph As XGraphics = XGraphics.FromForm(form)
        Dim gfx As XGraphics = XGraphics.FromPdfPage(pdfPage)
        'Dim graph As XGraphics = XGraphics.FromPdfPage(pdfPage)

        Dim tf As XTextFormatter = New XTextFormatter(graph)
        tf.Alignment = XParagraphAlignment.Right
        Dim tfL As XTextFormatter = New XTextFormatter(graph)
        tfL.Alignment = XParagraphAlignment.Left
        Dim tfC As XTextFormatter = New XTextFormatter(graph)
        tfC.Alignment = XParagraphAlignment.Center
        Dim pen As XPen = New XPen(Color.Black, 0.5)


        Dim titlu_font As XFont = New XFont("Verdana", 16, XFontStyle.Bold)
        Dim bold_font As XFont = New XFont("Calibri", 11, XFontStyle.Bold)
        Dim mic_font As XFont = New XFont("Calibri", 10, XFontStyle.Regular)
        Dim micro_font As XFont = New XFont("Calibri", 9, XFontStyle.Regular)
        Dim pico_font As XFont = New XFont("Calibri", 7, XFontStyle.Regular)
        Dim pico_bold_font As XFont = New XFont("Calibri", 7, XFontStyle.Bold)
        Dim report_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim text_font As XFont = New XFont("Calibri", 11, XFontStyle.Regular)
        Dim text_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim bottom_font As XFont = New XFont("Verdana", 12, XFontStyle.Bold)

        'Dim incas_tot As Double = 0
        'Dim chelt_tot As Double = 0
        ''A4 LANDSCAPE = 8.27x11.69" x72points/inch = 842X595 points

        graph.DrawString("Furnizor: ", text_font, XBrushes.Black, _
                         New XRect(15, 40, 45, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("MILICOM CAZ S.R.L.", bold_font, XBrushes.Black, _
                         New XRect(60, 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)


        graph.DrawString("Nr. ORC/an: ", text_font, XBrushes.Black, _
                        New XRect(15, 55, 60, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("J26/170/2013", bold_font, XBrushes.Black, _
                        New XRect(75, 55, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)


        graph.DrawString("CIF/CUI: ", text_font, XBrushes.Black, _
                        New XRect(15, 70, 45, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("31240216", bold_font, XBrushes.Black, _
                        New XRect(60, 70, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Sediul: ", text_font, XBrushes.Black, _
                        New XRect(15, 85, 40, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("Reghin, P-ta. Petru Maior nr. 9", bold_font, XBrushes.Black, _
                        New XRect(55, 85, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)


        graph.DrawString("Judetul: ", text_font, XBrushes.Black, _
                        New XRect(15, 100, 45, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("Mures", bold_font, XBrushes.Black, _
                        New XRect(60, 100, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Contul: ", text_font, XBrushes.Black, _
                        New XRect(15, 115, 40, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("RO12OTPV320000721380RO02", bold_font, XBrushes.Black, _
                        New XRect(55, 115, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Banca: ", text_font, XBrushes.Black, _
                        New XRect(15, 130, 35, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("OTP BANK ROMANIA", bold_font, XBrushes.Black, _
                        New XRect(50, 130, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)


        graph.DrawLine(pen, 215, 62, 215, 110) ' V
        graph.DrawLine(pen, 380, 62, 380, 110) ' V

        graph.DrawLine(pen, 215, 62, 380, 62) ' H
        graph.DrawLine(pen, 215, 110, 380, 110) ' H

        graph.DrawString("Nr. Facturii: ", text_font, XBrushes.Black, _
                        New XRect(230, 65, 60, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(nr_fact_TB.Text, bold_font, XBrushes.Black, _
                        New XRect(290, 65, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Data: ", text_font, XBrushes.Black, _
                        New XRect(230, 80, 35, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(data_DTP.Value, bold_font, XBrushes.Black, _
                        New XRect(265, 80, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Nr. aviz insotire: ", text_font, XBrushes.Black, _
                        New XRect(230, 95, 60, pdfPage.Height.Point), XStringFormats.TopLeft)



        Dim rect As New XRect(390, 10, 195, 60)
        Dim y_rnd As Integer = 0
        y_rnd = 130

        graph.DrawString("Banca: ", text_font, XBrushes.Black, _
                        New XRect(390, y_rnd, 35, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(banca_TB.Text, bold_font, XBrushes.Black, _
                        New XRect(425, y_rnd, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

        y_rnd = y_rnd - 15

        graph.DrawString("Contul: ", text_font, XBrushes.Black, _
                        New XRect(390, y_rnd, 40, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(cont_TB.Text, bold_font, XBrushes.Black, _
                        New XRect(430, y_rnd, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

        Dim adresa_string As String = localitate_TB.Text & ", " & adresa_tb.Text
        If adresa_string.ToString.Length < 23 Then
            y_rnd = y_rnd - 15
        Else
            y_rnd = y_rnd - 30
        End If

        graph.DrawString("Sediul: ", text_font, XBrushes.Black, _
                        New XRect(390, y_rnd, 40, pdfPage.Height.Point), XStringFormats.TopLeft)
        tfL.DrawString("                " & localitate_TB.Text & ", " & adresa_tb.Text, bold_font, XBrushes.Black, _
                        New XRect(390, y_rnd, 190, 70), XStringFormats.TopLeft)

        y_rnd = y_rnd - 15

        graph.DrawString("Judetul: ", text_font, XBrushes.Black, _
                        New XRect(390, y_rnd, 45, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(judet_TB.Text, bold_font, XBrushes.Black, _
                        New XRect(435, y_rnd, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

        y_rnd = y_rnd - 15

        graph.DrawString("CIF/CUI: ", text_font, XBrushes.Black, _
                        New XRect(390, y_rnd, 45, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(cui_firma, bold_font, XBrushes.Black, _
                        New XRect(435, y_rnd, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

        y_rnd = y_rnd - 15

        graph.DrawString("Nr. ORC/an: ", text_font, XBrushes.Black, _
                        New XRect(390, y_rnd, 60, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(J_firma_TB.Text, bold_font, XBrushes.Black, _
                        New XRect(450, y_rnd, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

        Dim firma_string As String = firma_TB.Text & " " & f_jur_TB.Text
        If firma_string.ToString.Length < 22 Then
            y_rnd = y_rnd - 15
        Else
            y_rnd = y_rnd - 30
        End If

        graph.DrawString("Cumparator: ", text_font, XBrushes.Black, _
                         New XRect(390, y_rnd, 45, pdfPage.Height.Point), XStringFormats.TopLeft)
        tfL.DrawString("                          " & firma_TB.Text & " " & f_jur_TB.Text, bold_font, XBrushes.Black, _
                         New XRect(390, y_rnd, 190, 70), XStringFormats.TopLeft)



        'graph.DrawLine(pen, 13, 130, 13, 150) ' V
        'graph.DrawLine(pen, 380, 62, 380, 110) ' V

        graph.DrawLine(pen, 13, 145, 582, 145) ' H


        graph.DrawLine(pen, 13, 145, 13, 180) ' V
        tfC.DrawString("Nr. Crt", mic_font, XBrushes.Black, _
                         New XRect(20, 146, 20, 30), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(20, 146, 20, 30))

        graph.DrawLine(pen, 47, 145, 47, 180) ' V

        tfC.DrawString("Denumirea produselor sau a serviciilor", mic_font, XBrushes.Black, _
                        New XRect(93, 146, 100, 30), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(93, 146, 100, 30))

        graph.DrawLine(pen, 240, 145, 240, 180) ' V

        tfC.DrawString("U.M.", mic_font, XBrushes.Black, _
                        New XRect(240, 146, 40, 30), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(240, 146, 40, 30))

        graph.DrawLine(pen, 280, 145, 280, 180) ' V

        tfC.DrawString("Cantitatea", mic_font, XBrushes.Black, _
                        New XRect(280, 146, 80, 30), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(280, 146, 80, 30))

        graph.DrawLine(pen, 360, 145, 360, 180) ' V

        tfC.DrawString("Pretul unitar -lei-", mic_font, XBrushes.Black, _
                        New XRect(380, 146, 60, 30), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(380, 146, 60, 30))

        graph.DrawLine(pen, 460, 145, 460, 180) ' V

        tfC.DrawString("Valoarea -lei-", mic_font, XBrushes.Black, _
                        New XRect(495, 146, 50, 30), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(495, 146, 50, 30))

        graph.DrawLine(pen, 582, 145, 582, 180) ' V


        graph.DrawLine(pen, 13, 171, 582, 171) ' H

        tfC.DrawString("0", mic_font, XBrushes.Black, _
                         New XRect(20, 169, 20, 30), XStringFormats.TopLeft)
        tfC.DrawString("1", mic_font, XBrushes.Black, _
                         New XRect(93, 169, 100, 30), XStringFormats.TopLeft)
        tfC.DrawString("2", mic_font, XBrushes.Black, _
                         New XRect(240, 169, 40, 30), XStringFormats.TopLeft)
        tfC.DrawString("3", mic_font, XBrushes.Black, _
                         New XRect(280, 169, 80, 30), XStringFormats.TopLeft)
        tfC.DrawString("4", mic_font, XBrushes.Black, _
                         New XRect(380, 169, 60, 30), XStringFormats.TopLeft)
        tfC.DrawString("5(3x4)", mic_font, XBrushes.Black, _
                         New XRect(495, 169, 50, 30), XStringFormats.TopLeft)

        tf.DrawString(observatii, mic_font, XBrushes.Black, _
                         New XRect(15, 315, 560, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(15, 315, 560, 15))

        graph.DrawLine(pen, 13, 180, 582, 180) ' H
        graph.DrawLine(pen, 13, 330, 13, 400) ' V

        graph.DrawLine(pen, 13, 330, 582, 330) ' H
        graph.DrawLine(pen, 582, 330, 582, 400) ' V

        tfL.DrawString("Semnatura si stampila furnizorului", micro_font, XBrushes.Black, _
                        New XRect(20, 333, 50, 60), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(20, 333, 50, 60))

        graph.DrawLine(pen, 130, 330, 130, 400) ' V

        tfL.DrawString("Date privind expeditia", pico_font, XBrushes.Black, _
                        New XRect(133, 333, 130, pdfPage.Height.Point), XStringFormats.TopLeft)

        '-
        tfL.DrawString("Numele delegatului ", pico_font, XBrushes.Black, _
                        New XRect(133, 343, 60, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(133, 343, 60, 8))

        tfL.DrawString(StrConv(nume_TB.Text, VbStrConv.ProperCase), pico_bold_font, XBrushes.Black, _
                        New XRect(193, 343, 130, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(193, 343, 130, 8))

        '-

        tfL.DrawString("B.I./C.I. seria ", pico_font, XBrushes.Black, _
                       New XRect(133, 351, 40, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(133, 351, 40, 8))

        tfL.DrawString(StrConv(seria_TB.Text, VbStrConv.Uppercase), pico_bold_font, XBrushes.Black, _
                        New XRect(173, 351, 20, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(173, 351, 20, 8))

        tfL.DrawString("nr ", pico_font, XBrushes.Black, _
                       New XRect(193, 351, 20, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(193, 351, 20, 8))

        tfL.DrawString(nr_CI_TB.Text, pico_bold_font, XBrushes.Black, _
                        New XRect(213, 351, 30, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(213, 351, 30, 8))

        tfL.DrawString("eliberata", pico_font, XBrushes.Black, _
                        New XRect(243, 351, 35, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(243, 351, 35, 8))

        tfL.DrawString(eliber_TB.Text, pico_bold_font, XBrushes.Black, _
                        New XRect(278, 351, 40, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(278, 351, 40, 8))

        '-
        tfL.DrawString("Mijlocul de transport  ", pico_font, XBrushes.Black, _
                       New XRect(133, 359, 65, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(133, 359, 65, 8))

        tfL.DrawString("", pico_bold_font, XBrushes.Black, _
                        New XRect(198, 359, 130, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(198, 359, 130, 8))

        '-

        tfL.DrawString("Expeditia s-a efectuat in prezenta noastra la data de", pico_font, XBrushes.Black, _
                       New XRect(133, 367, 155, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(133, 367, 155, 8))

        tfL.DrawString(data_factura, pico_bold_font, XBrushes.Black, _
                       New XRect(288, 367, 40, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(288, 367, 40, 8))

        '-

        tfL.DrawString("Semnaturile ..................................................................", pico_font, XBrushes.Black, _
                       New XRect(133, 385, 195, pdfPage.Height.Point), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(133, 385, 195, 8))

        graph.DrawLine(pen, 360, 330, 360, 400) ' V

        tfL.DrawString("Total din care: accize", micro_font, XBrushes.Black, _
                       New XRect(380, 333, 35, 50), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(380, 333, 35, 50))

        tf.DrawString(tot_valoare_TB.Text, bold_font, XBrushes.Black, _
                       New XRect(460, 333, 100, 10), XStringFormats.TopLeft)



        graph.DrawLine(pen, 460, 330, 460, 365) ' V
        graph.DrawLine(pen, 460, 348, 582, 348) ' H
        tfC.DrawString("X", mic_font, XBrushes.Black, _
                       New XRect(460, 348, 122, 10), XStringFormats.TopLeft)

        graph.DrawLine(pen, 360, 365, 582, 365) ' H

        tfL.DrawString("Semnatura de primire", micro_font, XBrushes.Black, _
                       New XRect(380, 368, 40, 50), XStringFormats.TopLeft)
        'graph.DrawRectangle(pen, New XRect(380, 368, 40, 50))

        graph.DrawLine(pen, 13, 400, 582, 400) ' H


        ''graph.DrawLine(pen, strtH, strtV, length, stopV)
        'graph.DrawLine(pen, 45, 132, 797, 132) ' H

        Dim nxtRow As Integer = 185
        Dim DGV As DataGridView = DataGridView1
        For i = 0 To DGV.Rows.Count - 1
            Dim crt As String = DGV.Rows(i).Cells("crt").Value.ToString
            Dim produs As String = DGV.Rows(i).Cells("produs").Value.ToString
            Dim buc As String = DGV.Rows(i).Cells("buc").Value.ToString
            Dim cant As Decimal = CDec(DGV.Rows(i).Cells("cant").Value.ToString)
            Dim pret_unitar As Decimal = CDec(DGV.Rows(i).Cells("pret_unitar").Value.ToString)
            Dim valoare As Decimal = cant * pret_unitar


            ''graph.DrawLine(pen, 13, 145, 13, 171) ' V

            tfC.DrawString(crt, text_font, XBrushes.Black, _
                             New XRect(20, nxtRow, 20, pdfPage.Height.Point), XStringFormats.TopLeft)

            ''graph.DrawLine(pen, 47, 145, 47, 171) ' V


            tfL.DrawString(produs, text_font, XBrushes.Black, _
                             New XRect(65, nxtRow, 1750, pdfPage.Height.Point), XStringFormats.TopLeft)
            ''graph.DrawLine(pen, 240, 145, 240, 171) ' V

            tfC.DrawString(buc, text_font, XBrushes.Black, _
                             New XRect(240, nxtRow, 40, pdfPage.Height.Point), XStringFormats.TopLeft)
            ''graph.DrawLine(pen, 280, 145, 280, 171) ' V

            tf.DrawString(cant, text_font, XBrushes.Black, _
                             New XRect(280, nxtRow, 65, pdfPage.Height.Point), XStringFormats.TopLeft)
            ''graph.DrawLine(pen, 360, 145, 360, 171) ' V

            tf.DrawString(Format(pret_unitar, "#,#0.00"), text_font, XBrushes.Black, _
                             New XRect(360, nxtRow, 80, pdfPage.Height.Point), XStringFormats.TopLeft)

            ''graph.DrawLine(pen, 460, 145, 460, 171) ' V


            tf.DrawString(Format(valoare, "#,#0.00"), text_font, XBrushes.Black, _
                             New XRect(460, nxtRow, 102, pdfPage.Height.Point), XStringFormats.TopLeft)

            ''graph.DrawLine(pen, 582, 145, 582, 171) ' V
            nxtRow = nxtRow + 18
        Next

        graph.Dispose()

        'Draw the form on the page of the document in its original size
        Dim dashpen As XPen = New XPen(Color.Black, 0.5)

        dashpen.DashPattern = {20, 40}
        'dashpen.DashStyle = XDashStyle.DashDot
        gfx.DrawImage(form, 0, 0)
        gfx.DrawLine(dashpen, 0, 421, 595, 421)
        gfx.DrawImage(form, 0, 440)


        Dim pdfFilename As String = folder_facturi & "Factura_" & nr_factura.PadLeft(3, "0") & "_" & Format(CDate(data_factura), "yyyyMMdd") & ".pdf"

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


    Private Sub edit_BU_Click(sender As Object, e As EventArgs) Handles edit_BU.Click
        ' Dim cell As DataGridViewCell = Firme_DGV.CurrentCell
        'Dim i As Integer = cell.ColumnIndex
        'I'f i > -1 Then
        'Dim col_name = Firme_DGV.Columns(i).HeaderText
        ' Dim row As DataGridViewRow = Firme_DGV.CurrentRow

        Dim id As String = ""
        'Dim firma As String = ""
        'Dim forma_juridica As String = ""
        'Dim cui As String = ""
        'Dim tva As Boolean = False
        'Dim j As String = ""
        'Dim adresa As String = ""
        'Dim localitate As String = ""
        'Dim judet As String = ""
        Dim tip As String = ""
        Dim status As String = ""
        'Dim cont As String = ""
        'Dim banca As String = ""


        'id = row.Cells("id").Value.ToString
        Dim data_factura As String = Format(data_DTP.Value, "dd.MM.yyyy")
        Dim nr_factura As String = nr_fact_TB.Text
        id = id_Lbl.Text
        Dim firma As String = firma_TB.Text
        Dim forma_juridica As String = f_jur_TB.Text

        Dim ro As String = ""
        If ro_CB.CheckState = CheckState.Checked Then
            ro = "RO"
        ElseIf ro_CB.CheckState = CheckState.Unchecked Then
            ro = ""
        End If
        Dim cui As String = ro & cif_firma_TB.Text
        Dim j As String = J_firma_TB.Text
        Dim adresa As String = adresa_tb.Text
        Dim localitate As String = localitate_TB.Text
        Dim judet As String = judet_TB.Text
        Dim cont As String = cont_TB.Text
        Dim banca As String = banca_TB.Text
        With Form_firme_introd
            .Text = "MODIFICA FIRMA"

            .firma_text.Text = firma
            .forma_juridica_text.Text = forma_juridica
            .cui_text.Text = cif_firma_TB.Text
            If tva = True Then
                .CheckBox1.CheckState = CheckState.Checked
            Else
                .CheckBox1.CheckState = CheckState.Unchecked
            End If
            .j_text.Text = j
            .adresa_text.Text = adresa
            .localitate_text.Text = localitate
            .judet_text.Text = judet
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
            Form_principal.Load_Firme()
            firma_TB.Focus()

        End If
        'Else : MsgBox("Nu s-a modificat nimic")

        'End If
        Form_firme_introd.Close()
    End Sub

    Private Sub nume_TB_Leave(sender As Object, e As EventArgs) Handles nume_TB.Leave
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        If dbconn.State = ConnectionState.Closed Then
            dbconn.Open()
        End If
        dbcomm = New MySqlCommand("SELECT id,nume,serie,numar,data_elib FROM delegati WHERE nume LIKE '" & nume_TB.Text & "%'", dbconn)

        dbread = dbcomm.ExecuteReader()
        dbread.Read()
        'If Trim(f_jur_TB.Text) = Nothing Or Trim(firma_TB.Text) = Nothing Then
        If dbread.HasRows = False Or Trim(nume_TB.Text) = Nothing Then
            dbread.Close() '---------------

        ElseIf dbread.HasRows = True Then
            'dbread = dbcomm.ExecuteReader()
            dbread.Read()
            'id_Lbl.Text = dbread("id").ToString
            nume_TB.Text = dbread("nume").ToString
            seria_TB.Text = dbread("serie").ToString
            nr_CI_TB.Text = dbread("numar").ToString()
            eliber_TB.Text = dbread("data_elib").ToString
            'If IsDBNull(dbread("tva")) = True Then
            '    ro_CB.CheckState = CheckState.Unchecked
            'ElseIf IsDBNull(dbread("tva")) = False Then
            '    If CBool(dbread("tva")) = True Then
            '        ro_CB.CheckState = CheckState.Checked
            '    Else 'If dbread("ro").ToString = False Then
            '        ro_CB.CheckState = CheckState.Unchecked
            '    End If
            'End If


            dbread.Close() '---------------
        End If

        dbread.Close() '---------------
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles plata_BU.Click
        'dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")


        Dim nr_fact As String = nr_fact_TB.Text
        Dim dat As String = data_DTP.Value
        Dim firma As String = firma_TB.Text
        Dim cif_firma As String = cif_firma_TB.Text
        Dim mag As String = ComboBox3.SelectedValue
        Dim valoare As Decimal = CDec(tot_valoare_TB.Text)
        Dim plata As String = ""


        Form_aleg_Plata.ShowDialog()
        If DialogResult.OK Then

            Form_Bon.ShowDialog()
            Form_Bon.data_bon_DTP.Value = data_DTP.Value

            If DialogResult.OK Then
                Dim maga As String = ""
                If Form_Bon.ComboBox3.SelectedValue = "PM" Then
                    maga = ", magazin Petru Maior 9"
                ElseIf Form_Bon.ComboBox3.SelectedValue = "MV" Then
                    maga = ", magazin Mihai Viteazu 28"
                End If
                plata = "Plata conform bon fiscal nr. " & Form_Bon.nr_bon.Text & " din data de " & Format(Form_Bon.data_bon_DTP.Value, "dd.MM.yyy") & maga
            End If
            obs_TB.Text = plata
        End If
        ''MsgBox(plata)
        'If dbconn.State = ConnectionState.Closed Then
        '    dbconn.Open()
        'End If
        'Dim transaction As MySqlTransaction = dbconn.BeginTransaction()

        'Dim sql_fact As String = "INSERT INTO facturi (numar,data,valoare,cui_firma,tva,plata) VALUES(@numar,@data,@valoare,@cui_firma,@tva,@plata)"

        'Try

        '    dbcomm = New MySqlCommand(sql_fact, dbconn)
        '    dbcomm.Parameters.AddWithValue("@numar", nr_fact)
        '    dbcomm.Parameters.AddWithValue("@data", data_DTP.Value.Date)
        '    dbcomm.Parameters.AddWithValue("@valoare", valoare)
        '    dbcomm.Parameters.AddWithValue("@cui_firma", cif_firma)
        '    dbcomm.Parameters.AddWithValue("@tva", tva)
        '    dbcomm.Parameters.AddWithValue("@plata", plata)
        '    dbcomm.ExecuteNonQuery()


        'Catch ex As Exception
        '    MsgBox("Failed to insert into facturi: " & ex.Message.ToString())
        'End Try
    End Sub
End Class
