Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization
Imports System.IO

Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing.Layout
Imports System.Reflection.Emit
Imports System.Diagnostics.Eventing.Reader
Imports MigraDoc.DocumentObjectModel.Tables
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Runtime.InteropServices.ComTypes
Imports Microsoft.VisualBasic.Logging

Public Class Form_Inventar
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Public dbconn As New MySqlConnection
    Public dbread2 As MySqlDataReader
    Private Sub Form_Inventar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Add("Marfuri")
        ComboBox1.Items.Add("Obiecte Inventar")
        ComboBox1.Items.Add("Consumabile")
        ComboBox1.Items.Add("Mijloace Fixe")
        ComboBox1.Items.Add("Auxiliare")
        ComboBox1.SelectedIndex = 0
        Add_Columns()
        Load_Produse()


    End Sub
    Private Sub Add_Columns()
        Dim wid As Double = 0
        Dim DGV As DataGridView = Me.DataGridView1
        Dim dgvW As Double = 0
        Dim vsb As Double = SystemInformation.VerticalScrollBarWidth
        dgvW = (DGV.Width - DGV.RowHeadersWidth - vsb) / 100
        For i = 0 To DGV.ColumnCount - 1
            wid += DGV.Rows(0).Cells(i).Size.Width
        Next

        'Dim colid As New DataGridViewTextBoxColumn
        'With colid
        '    .DataPropertyName = "PropertyName"
        '    .HeaderText = "ID"
        '    .Name = "id"
        '    .Width = 8 * dgvW '50
        '    .Visible = False

        'End With
        'datagridview1.Columns.Add(colid)

        Dim col2 As New DataGridViewTextBoxColumn With {
            .DataPropertyName = "PropertyName",
            .HeaderText = "Produs",
            .Name = "produs",
            .Width = 30 * dgvW '50
            }
        DataGridView1.Columns.Add(col2)

        Dim col3 As New DataGridViewTextBoxColumn With {
            .DataPropertyName = "PropertyName",
            .HeaderText = "Ultima Data",
            .Name = "u_data",
            .Width = 20 * dgvW '50
            }
        DataGridView1.Columns.Add(col3)


        Dim col3_1 As New DataGridViewTextBoxColumn With {
            .DataPropertyName = "PropertyName",
            .HeaderText = "Pret",
            .Name = "pret",
            .Width = 10 * dgvW '50
            }
        DataGridView1.Columns.Add(col3_1)

        Dim col4 As New DataGridViewTextBoxColumn With {
            .DataPropertyName = "PropertyName",
            .HeaderText = "Stoc",
            .Name = "stoc",
            .Width = 10 * dgvW '150
            }
        DataGridView1.Columns.Add(col4)

        Dim col5 As New DataGridViewTextBoxColumn With {
            .DataPropertyName = "PropertyName",
            .HeaderText = "Unitati Inventar",
        .Name = "unit_inv",
        .Width = 10 * dgvW
            }
        DataGridView1.Columns.Add(col5)


        Dim col5_2 As New DataGridViewButtonColumn With {
            .DataPropertyName = "PropertyName",
            .HeaderText = "-",
        .Name = "minus",
        .Width = 5 * dgvW
            }
        DataGridView1.Columns.Add(col5_2)

        Dim col5_1 As New DataGridViewButtonColumn With {
            .DataPropertyName = "PropertyName",
            .HeaderText = "+",
        .Name = "plus",
        .Width = 5 * dgvW
            }
        DataGridView1.Columns.Add(col5_1)


        Dim col6 As New DataGridViewTextBoxColumn
        With col6
            .DataPropertyName = "PropertyName"
            .HeaderText = "Total"
            .Name = "total"
            .Width = 10 * dgvW '50
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
        DataGridView1.Columns.Add(col6)


        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

    End Sub
    Private Sub Load_Produse()
        Dim DGV As DataGridView = DataGridView1

        DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DGV.AllowUserToAddRows = False
        DGV.AllowUserToResizeRows = False
        'DGV.CellBorderStyle = DataGridViewCellBorderStyle.None
        DGV.RowHeadersVisible = False

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource

            Dim tot_sql As String
            If ComboBox1.SelectedItem.ToString = "Marfuri" Then
                tot_sql = "SELECT produs,pret,sum(bucati) as bucati, MAX(luna) as luna, MAX(an) as an from 
                            (
                                SELECT produs, pret, sum(bucati) as bucati,  MONTH(MAX(data)) as luna, YEAR(MAX(data)) as an from marfa mf WHERE YEAR(data)='2023' GROUP BY produs, pret
                                UNION ALL
                                SELECT produs, pret, sum(bucati) as bucati,  MONTH(MAX(data_inv)) as luna, YEAR(MAX(data_inv)) as an from marfa_inventar inv GROUP BY produs, pret
                            ) t
                            GROUP BY produs, pret"
                'tot_sql = "SELECT produs,pret,sum(bucati) as bucati, MONTH(MAX(data_inv)) as luna, YEAR(MAX(data_inv)) as an from marfa_inventar GROUP BY produs, pret"
            ElseIf ComboBox1.SelectedItem.ToString = "Obiecte Inventar" Then
                tot_sql = "SELECT produs,pret,bucati, data as luna, data as an from obiecte_inventar WHERE tip='OI' ORDER BY data DESC"
            ElseIf ComboBox1.SelectedItem.ToString = "Mijloace Fixe" Then
                tot_sql = "SELECT produs,pret,bucati, data as luna, data as an from obiecte_inventar WHERE tip='MF' ORDER BY data DESC"
            ElseIf ComboBox1.SelectedItem.ToString = "Auxiliare" Then
                tot_sql = "SELECT produs,pret,bucati, data as luna, data as an from obiecte_inventar WHERE tip='AX' ORDER BY data DESC"
            End If

            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(tot_sql, dbconn)

            Try

                dbread = dbcomm.ExecuteReader()
                Dim rand As Integer = 0
                Dim tot As Decimal = 0
                While dbread.Read()
                    DGV.Rows.Add()
                    Dim produs As String = dbread("produs")
                    Dim data As String = dbread("luna") & "." & dbread("an")
                    Dim pret As Decimal = CDec(dbread("pret"))
                    Dim stoc As Integer = CInt(dbread("bucati"))
                    Dim unitati As Decimal = stoc
                    'If dbread("an") = (CDec(Today.Year) - 1).ToString Then
                    '    unitati += 2
                    'ElseIf dbread("an") = (CDec(Today.Year) - 2).ToString Then
                    '    unitati += 1
                    'End If

                    Dim total As Decimal = pret * unitati
                    tot += total

                    DGV.Rows(rand).Cells("produs").Value = produs
                    DGV.Rows(rand).Cells("pret").Value = pret
                    DGV.Rows(rand).Cells("u_data").Value = data
                    DGV.Rows(rand).Cells("stoc").Value = stoc
                    DGV.Rows(rand).Cells("unit_inv").Value = unitati
                    DGV.Rows(rand).Cells("plus").Value = "+"
                    DGV.Rows(rand).Cells("minus").Value = "-"
                    DGV.Rows(rand).Cells("total").Value = total
                    'DGV.Rows.Add(produs, data, pret, stoc, unitati, total)

                    rand += 1

                End While
                TextBox1.Text = tot
            Catch ex As Exception
                MsgBox("Problem loading toti: " & ex.Message.ToString)
            End Try
        End Using

        'AddHandler DataGridView1.CellValueChanged, AddressOf DataGridView1_CellValueChanged
    End Sub
    Private Sub DataGridView1_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles DataGridView1.CurrentCellDirtyStateChanged
        'If (DataGridView1.IsCurrentCellDirty) Then
        '    ' -- commit
        '    DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
        '    Dim tot As Decimal = 0
        '    Dim rand As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
        '    Dim total As Decimal = CDec(rand.Cells("total").Value)
        '    Dim unitAct As Decimal = CDec(rand.Cells("unit_inv").Value)
        '    Dim pret As Decimal = CDec(rand.Cells("pret").Value)
        '    rand.Cells("unit_inv").Value = unitAct + 1
        '    total = pret * unitAct
        '    For i = 0 To DataGridView1.Rows.Count - 1
        '        tot += CDec(DataGridView1.Rows(i).Cells("total").Value)

        '    Next
        '    TextBox1.Text = tot
        'End If
    End Sub
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If e.RowIndex < 0 Then
            Exit Sub
        End If

        Dim grid = DirectCast(sender, DataGridView)

        If TypeOf grid.Columns(e.ColumnIndex) Is DataGridViewButtonColumn Then

            Dim rand As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            Dim total As Decimal = CDec(rand.Cells("total").Value)

            If grid.Columns(e.ColumnIndex).Name = "plus" Then



                Dim unitAct As Decimal = CDec(rand.Cells("unit_inv").Value) + 1
                Dim pret As Decimal = CDec(rand.Cells("pret").Value)
                rand.Cells("unit_inv").Value = unitAct
                total = pret * unitAct




            End If

            If grid.Columns(e.ColumnIndex).Name = "minus" Then
                Dim unitAct As Decimal = CDec(rand.Cells("unit_inv").Value) - 1
                Dim pret As Decimal = CDec(rand.Cells("pret").Value)
                rand.Cells("unit_inv").Value = unitAct
                total = pret * unitAct


            End If
            rand.Cells("total").Value = total
        End If


    End Sub
    Private Sub DataGridView1_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged
        Dim rand As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
        Dim tot As Decimal = 0
        For i = 0 To DataGridView1.Rows.Count - 1
            Dim unitAct As Decimal = CDec(DataGridView1.Rows(i).Cells("unit_inv").Value)

            Dim pret As Decimal = CDec(DataGridView1.Rows(i).Cells("pret").Value)
            Dim total As Decimal = pret * unitAct

            tot += CDec(DataGridView1.Rows(i).Cells("total").Value)
        Next
        TextBox1.Text = tot
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' -------------------- LOAD setari

        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")
        Dim folder_nir As String = ""
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


        Dim titlu As String = "Inventar"
        Dim tip_inventar As String = ""
        If ComboBox1.SelectedItem.ToString = "Marfuri" Then
            titlu = "Inventar Marfuri"
            tip_inventar = "Marfuri"
        ElseIf ComboBox1.SelectedItem.ToString = "Obiecte Inventar" Then
            titlu = "Inventar Obiecte Inventar"
            tip_inventar = "Obiecte Inventar"
        ElseIf ComboBox1.SelectedItem.ToString = "Mijloace Fixe" Then
            titlu = "Inventar Mijloace Fixe"
            tip_inventar = "Mijloace Fixe"
        ElseIf ComboBox1.SelectedItem.ToString = "Auxiliare" Then
            titlu = "Inventar Materiale Auxiliare"
            tip_inventar = "Materiale Auxiliare"
        End If

        Dim pdf As PdfSharp.Pdf.PdfDocument = New PdfSharp.Pdf.PdfDocument
        pdf.Info.Title = titlu
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

        Dim nrPg As Integer = 1

        DrawBackground(pdfPage, nrPg, "31.12.2023", tip_inventar, graph)

        Dim DGV As DataGridView = DataGridView1
        Dim total As Decimal = 0
        Dim crt As Integer = 1
        Dim rand As Integer = 113
        Dim dist As Integer = mic_font.Height + 2
        Dim xpt As Integer = 45

        If dbconn.State = 0 Then
            dbconn.Open()
        End If
        'Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
        For i = 0 To DGV.Rows.Count - 1
            If CDec(DGV.Rows(i).Cells("unit_inv").Value.ToString) > 0 Then

                Dim produs As String = DGV.Rows(i).Cells("produs").Value.ToString
                Dim buc As String = "b"
                Dim cant As Decimal = CDec(DGV.Rows(i).Cells("unit_inv").Value.ToString)
                Dim pret_unitar As Decimal = CDec(DGV.Rows(i).Cells("pret").Value.ToString)
                Dim valoare_contabila As Decimal = cant * pret_unitar
                Dim valoare_depreciere As String = "-"
                Dim valoare_adevar As Decimal = valoare_contabila
                total += valoare_contabila

                Dim codul As String = "-"
                Dim faptice As Decimal = cant
                Dim scriptice As Decimal = cant
                Dim plus As String = "-"
                Dim minus As String = "-"
                Dim dif_plus As String = "-"
                Dim dif_minus As String = "-"
                Dim dep_val As String = "-"
                Dim dep_motiv As String = "-"

                Dim sql_marf As String = "INSERT INTO marfa_inventar (produs,pret,bucati,data_inv,magazin) VALUES(@produs,@pret,@bucati,@data_inv,@magazin)"

                Try

                    dbcomm = New MySqlCommand(sql_marf, dbconn)

                    dbcomm.Parameters.AddWithValue("@produs", produs)
                    dbcomm.Parameters.AddWithValue("@data_inv", DateTimePicker1.Value.Date)
                    dbcomm.Parameters.AddWithValue("@pret", CDec(pret_unitar))
                    dbcomm.Parameters.AddWithValue("@bucati", cant)

                    'dbcomm.Parameters.AddWithValue("@pret_intrare", CDec(pr_intrare))
                    dbcomm.Parameters.AddWithValue("@magazin", "PM")

                    dbcomm.ExecuteNonQuery()


                Catch ex As Exception
                    MsgBox("Failed to insert into marfa: " & ex.Message.ToString())
                End Try

                Dim lng As Integer


                'NR CRT
                lng = 30
                graph.DrawString(crt, mic_font, XBrushes.Black,
                         New XRect(xpt, rand, lng, pdfPage.Height.Point), XStringFormats.TopCenter)
                xpt += lng

                'Produs
                lng = 190
                graph.DrawString(produs, mic_font, XBrushes.Black,
                         New XRect(xpt, rand, lng, pdfPage.Height.Point), XStringFormats.TopCenter)
                xpt += lng

                'Codul
                lng = 30
                graph.DrawString(codul, mic_font, XBrushes.Black,
                         New XRect(xpt, rand, lng, pdfPage.Height.Point), XStringFormats.TopCenter)
                xpt += lng

                'U.M.
                lng = 30
                graph.DrawString(buc, mic_font, XBrushes.Black,
                         New XRect(xpt, rand, lng, pdfPage.Height.Point), XStringFormats.TopCenter)
                xpt += lng

                'Faptice
                lng = 40
                graph.DrawString(faptice, mic_font, XBrushes.Black,
                         New XRect(xpt, rand, lng, pdfPage.Height.Point), XStringFormats.TopCenter)
                xpt += lng

                'Scriptice
                lng = 40
                graph.DrawString(scriptice, mic_font, XBrushes.Black,
                         New XRect(xpt, rand, lng, pdfPage.Height.Point), XStringFormats.TopCenter)
                xpt += lng

                'Plus
                lng = 40
                graph.DrawString(plus, mic_font, XBrushes.Black,
                         New XRect(xpt, rand, lng, pdfPage.Height.Point), XStringFormats.TopCenter)
                xpt += lng

                'Minus
                lng = 40
                graph.DrawString(minus, mic_font, XBrushes.Black,
                         New XRect(xpt, rand, lng, pdfPage.Height.Point), XStringFormats.TopCenter)
                xpt += lng

                'Pret Unitar
                lng = 45
                tf.DrawString(Format(pret_unitar, "#,#0.00"), mic_font, XBrushes.Black,
                         New XRect(xpt, rand, lng, pdfPage.Height.Point), XStringFormats.TopLeft)
                xpt += lng

                'Valoarea
                lng = 45
                tf.DrawString(Format(valoare_contabila, "#,#0.00"), mic_font, XBrushes.Black,
                         New XRect(xpt, rand, lng, pdfPage.Height.Point), XStringFormats.TopLeft)
                xpt += lng


                'Dif Plus
                lng = 45
                graph.DrawString(dif_plus, mic_font, XBrushes.Black,
                         New XRect(xpt, rand, lng, pdfPage.Height.Point), XStringFormats.TopCenter)
                xpt += lng

                'Dif Minus
                lng = 45
                graph.DrawString(dif_minus, mic_font, XBrushes.Black,
                         New XRect(xpt, rand, lng, pdfPage.Height.Point), XStringFormats.TopCenter)
                xpt += lng

                'Valoare Adevar
                lng = 45
                tf.DrawString(Format(valoare_adevar, "#,#0.00"), mic_font, XBrushes.Black,
                         New XRect(xpt, rand, lng, pdfPage.Height.Point), XStringFormats.TopLeft)
                xpt += lng

                'Dep Valoarea
                lng = 45
                graph.DrawString(valoare_depreciere, mic_font, XBrushes.Black,
                         New XRect(xpt, rand, lng, pdfPage.Height.Point), XStringFormats.TopCenter)
                xpt += lng

                'Dep Motivul
                lng = 45
                graph.DrawString(dep_motiv, mic_font, XBrushes.Black,
                         New XRect(xpt, rand, lng, pdfPage.Height.Point), XStringFormats.TopCenter)
                xpt += lng

                crt += 1
                rand += dist
                xpt = 45
                If rand > 485 Then
                    pdfPage = pdf.AddPage
                    nrPg += 1
                    rand = 112
                    graph = XGraphics.FromPdfPage(pdfPage)
                    tf = New XTextFormatter(graph)
                    tf.Alignment = XParagraphAlignment.Right
                    DrawBackground(pdfPage, nrPg, "31.12.2023", tip_inventar, graph)
                End If
            End If
        Next
        graph.DrawString("Total: " & Format(total, "#,#0.00"), top_font, XBrushes.Black,
                         New XRect(620, 545, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        'transaction.Commit()
        Dim pdfFilename As String = folder_nir & "INVENTAR_" & tip_inventar & "_" & Format(CDate("31.12.2023"), "yyyyMMdd") & ".pdf"

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
    Private Function DrawBackground(pdfpage As PdfSharp.Pdf.PdfPage, pagina As Integer, data_inv As String, gestiunea As String, graph As XGraphics)

        pdfpage.Orientation = PageOrientation.Landscape
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

        ''A4 LANDSCAPE = 8.27x11.69" x72points/inch = 842X595 points

        '=============  HEADER ======================
        Dim xSTG As Integer = 0
        Dim xPt As Integer = 0
        Dim yPt As Integer = 0
        Dim lng As Integer = 0
        Dim lngTot As Integer

        xSTG = 45
        xPt = 45
        yPt = 20
        lng = 50

        graph.DrawString("Unitatea: ", text_font, XBrushes.Black,
                         New XRect(xPt, yPt, lng, pdfpage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("MILICOM CAZ S.R.L.", top_font, XBrushes.Black,
                         New XRect(xPt + lng, yPt, pdfpage.Width.Point, pdfpage.Height.Point), XStringFormats.TopLeft)


        graph.DrawString("Magazia: ", text_font, XBrushes.Black,
                        New XRect(xPt, yPt + 17, lng, pdfpage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("Reghin", top_font, XBrushes.Black,
                        New XRect(xPt + lng, yPt + 17, pdfpage.Width.Point, pdfpage.Height.Point), XStringFormats.TopLeft)


        xPt = 145
        lng = 552
        graph.DrawString("LISTA DE INVENTARIERE", titlu_font, XBrushes.Black,
                         New XRect(xPt, yPt, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("Data: " & data_inv, text_font, XBrushes.Black,
                        New XRect(xPt, yPt + 17, lng, pdfpage.Height.Point), XStringFormats.TopCenter)

        xPt = 667
        lng = 150

        Dim cont As String = "-"
        Dim lngGest As Integer = 0
        If gestiunea = "Marfuri" Then
            cont = 3710
            lngGest = 42
        ElseIf gestiunea = "Obiecte Inventar" Then
            cont = 303
            lngGest = 90
        ElseIf gestiunea = "Mijloace Fixe" Then
            cont = "-"
            lngGest = 90
        ElseIf gestiunea = "Materiale Auxiliare" Then
            cont = 30210
            lngGest = 100
        End If
        Dim xCapat As Integer = CInt(pdfpage.Width) - 45
        graph.DrawString(pagina, titlu_font, XBrushes.Black,
                         New XRect(xCapat - 40, yPt + 5, 40, pdfpage.Height.Point), XStringFormats.TopCenter)

        graph.DrawString(gestiunea, top_font, XBrushes.Black,
                        New XRect(xCapat - 40 - lngGest, yPt, lngGest, pdfpage.Height.Point), XStringFormats.TopLeft)
        lng = 60
        tf.DrawString("Gestiunea: ", text_font, XBrushes.Black,
                         New XRect(xCapat - 40 - lng - lngGest - 3, yPt, lng, pdfpage.Height.Point), XStringFormats.TopLeft)

        lng = 100
        graph.DrawString(cont, top_font, XBrushes.Black,
                        New XRect(xCapat - 40 - lngGest, yPt + 17, pdfpage.Width.Point, pdfpage.Height.Point), XStringFormats.TopLeft)
        tf.DrawString("Loc de depozitare: ", text_font, XBrushes.Black,
                         New XRect(xCapat - 40 - lng - lngGest - 3, yPt + 17, lng, pdfpage.Height.Point), XStringFormats.TopLeft)



        '---------------- CAP TABEL ----------------------
        xPt = 45
        yPt = 55

        Dim linV = 100
        Dim rnd1 As Integer = yPt + 3
        Dim rnd2 As Integer = rnd1 + 14
        Dim rnd3 As Integer = rnd2 + 14
        Dim rnd4 As Integer = rnd3 + 14
        Dim under As Integer = 0   'distanta sub rand
        Dim nrCol As Integer
        graph.DrawLine(pen, xPt, yPt, xPt, linV) ' V

        lng = 30
        lngTot += lng
        graph.DrawLine(pen, xPt, yPt, xPt, linV + mic_font.Height) ' V
        graph.DrawString("NR.", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd1, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("CRT.", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd1 + 10, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        nrCol = 0
        graph.DrawString(nrCol, mic_font, XBrushes.Black,
                         New XRect(xPt, rnd4, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd3 + mic_font.Height + under, xPt + lng, rnd3 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt, rnd4 + mic_font.Height + under, xPt + lng, rnd4 + mic_font.Height + under) ' H
        xPt += lng
        graph.DrawLine(pen, xPt, yPt, xPt, linV + mic_font.Height) ' V


        lng = 190
        lngTot += lng
        graph.DrawString("Denumirea bunurilor", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd1, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        nrCol += 1
        graph.DrawString(nrCol, mic_font, XBrushes.Black,
                         New XRect(xPt, rnd4, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd3 + mic_font.Height + under, xPt + lng, rnd3 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt, rnd4 + mic_font.Height + under, xPt + lng, rnd4 + mic_font.Height + under) ' H
        xPt += lng
        graph.DrawLine(pen, xPt, yPt, xPt, linV + mic_font.Height) ' V

        lng = 30
        lngTot += lng
        graph.DrawString("Codul", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd1, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        nrCol += 1
        graph.DrawString(nrCol, mic_font, XBrushes.Black,
                         New XRect(xPt, rnd4, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd3 + mic_font.Height + under, xPt + lng, rnd3 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt, rnd4 + mic_font.Height + under, xPt + lng, rnd4 + mic_font.Height + under) ' H
        xPt += lng
        graph.DrawLine(pen, xPt, yPt, xPt, linV + mic_font.Height) ' V

        lng = 30
        lngTot += lng
        graph.DrawString("U.M.", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd1, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        nrCol += 1
        graph.DrawString(nrCol, mic_font, XBrushes.Black,
                         New XRect(xPt, rnd4, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd3 + mic_font.Height + under, xPt + lng, rnd3 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt, rnd4 + mic_font.Height + under, xPt + lng, rnd4 + mic_font.Height + under) ' H
        xPt += lng
        graph.DrawLine(pen, xPt, yPt, xPt, linV + mic_font.Height) ' V

        lng = 40 ' -----Latime coloane

        graph.DrawString("CANTITATI", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd1, lng * 4, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd1 + mic_font.Height + under, xPt + lng * 4, rnd1 + mic_font.Height + under) ' H
        graph.DrawString("Stocuri", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd2, lng * 2, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd2 + mic_font.Height + under, xPt + lng * 4, rnd2 + mic_font.Height + under) ' H
        lngTot += lng
        graph.DrawString("Faptice", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd3, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        nrCol += 1
        graph.DrawString(nrCol, mic_font, XBrushes.Black,
                         New XRect(xPt, rnd4, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd3 + mic_font.Height + under, xPt + lng, rnd3 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt, rnd4 + mic_font.Height + under, xPt + lng, rnd4 + mic_font.Height + under) ' H

        xPt += lng
        graph.DrawLine(pen, xPt, rnd3 + under, xPt, linV) ' V
        graph.DrawLine(pen, xPt, rnd4, xPt, rnd4 + mic_font.Height) ' V

        lngTot += lng
        graph.DrawString("Scriptice", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd3, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        nrCol += 1
        graph.DrawString(nrCol, mic_font, XBrushes.Black,
                         New XRect(xPt, rnd4, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd3 + mic_font.Height + under, xPt + lng, rnd3 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt, rnd4 + mic_font.Height + under, xPt + lng, rnd4 + mic_font.Height + under) ' H
        xPt += lng
        graph.DrawLine(pen, xPt, rnd2 + under, xPt, linV) ' V
        graph.DrawLine(pen, xPt, rnd4, xPt, rnd4 + mic_font.Height) ' V

        graph.DrawString("Diferente", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd2, lng * 2, pdfpage.Height.Point), XStringFormats.TopCenter)

        lngTot += lng
        graph.DrawString("Plus", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd3, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        nrCol += 1
        graph.DrawString(nrCol, mic_font, XBrushes.Black,
                         New XRect(xPt, rnd4, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd3 + mic_font.Height + under, xPt + lng, rnd3 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt, rnd4 + mic_font.Height + under, xPt + lng, rnd4 + mic_font.Height + under) ' H

        xPt += lng
        graph.DrawLine(pen, xPt, rnd3 + under, xPt, linV) ' V
        graph.DrawLine(pen, xPt, rnd4, xPt, rnd4 + mic_font.Height) ' V

        lngTot += lng
        graph.DrawString("Minus", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd3, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        nrCol += 1
        graph.DrawString(nrCol, mic_font, XBrushes.Black,
                         New XRect(xPt, rnd4, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd3 + mic_font.Height + under, xPt + lng, rnd3 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt, rnd4 + mic_font.Height + under, xPt + lng, rnd4 + mic_font.Height + under) ' H
        xPt += lng
        graph.DrawLine(pen, xPt, yPt, xPt, linV) ' V
        graph.DrawLine(pen, xPt, rnd4, xPt, rnd4 + mic_font.Height) ' V

        lng = 45
        lngTot += lng
        graph.DrawString("PRET", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd1, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("UNITAR", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd2, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        nrCol += 1
        graph.DrawString(nrCol, mic_font, XBrushes.Black,
                         New XRect(xPt, rnd4, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd3 + mic_font.Height + under, xPt + lng, rnd3 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt, rnd4 + mic_font.Height + under, xPt + lng, rnd4 + mic_font.Height + under) ' H

        xPt += lng
        graph.DrawLine(pen, xPt, yPt, xPt, linV) ' V
        graph.DrawLine(pen, xPt, rnd4, xPt, rnd4 + mic_font.Height) ' V

        'lng = 60
        graph.DrawString("VALOAREA CONTABILA", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd1, lng * 3, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd1 + mic_font.Height + under, xPt + lng * 3, rnd1 + mic_font.Height + under) ' H
        lngTot += lng
        graph.DrawString("Valoare", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd2, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        nrCol += 1
        graph.DrawString(nrCol, mic_font, XBrushes.Black,
                         New XRect(xPt, rnd4, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd3 + mic_font.Height + under, xPt + lng, rnd3 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt, rnd4 + mic_font.Height + under, xPt + lng, rnd4 + mic_font.Height + under) ' H

        xPt += lng
        graph.DrawLine(pen, xPt, rnd2 + under, xPt, linV) ' V
        graph.DrawLine(pen, xPt, rnd4, xPt, rnd4 + mic_font.Height) ' V

        'lng = 60
        graph.DrawString("DIFERENTE", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd2, lng * 2, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd2 + mic_font.Height + under, xPt + lng * 2, rnd2 + mic_font.Height + under) ' H
        lngTot += lng
        graph.DrawString("Plus", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd3, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        nrCol += 1
        graph.DrawString(nrCol, mic_font, XBrushes.Black,
                         New XRect(xPt, rnd4, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd3 + mic_font.Height + under, xPt + lng, rnd3 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt, rnd4 + mic_font.Height + under, xPt + lng, rnd4 + mic_font.Height + under) ' H

        xPt += lng
        graph.DrawLine(pen, xPt, rnd3 + under, xPt, linV) ' V
        graph.DrawLine(pen, xPt, rnd4, xPt, rnd4 + mic_font.Height) ' V

        lngTot += lng
        graph.DrawString("Minus", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd3, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        nrCol += 1
        graph.DrawString(nrCol, mic_font, XBrushes.Black,
                         New XRect(xPt, rnd4, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd3 + mic_font.Height + under, xPt + lng, rnd3 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt, rnd4 + mic_font.Height + under, xPt + lng, rnd4 + mic_font.Height + under) ' H

        xPt += lng
        graph.DrawLine(pen, xPt, yPt, xPt, linV + mic_font.Height) ' V
        graph.DrawLine(pen, xPt, rnd4, xPt, rnd4 + mic_font.Height) ' V

        lngTot += lng
        graph.DrawString("Valoare", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd1, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("Adevar", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd2, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        nrCol += 1
        graph.DrawString(nrCol, mic_font, XBrushes.Black,
                         New XRect(xPt, rnd4, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd3 + mic_font.Height + under, xPt + lng, rnd3 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt, rnd4 + mic_font.Height + under, xPt + lng, rnd4 + mic_font.Height + under) ' H
        xPt += lng
        graph.DrawLine(pen, xPt, yPt, xPt, linV) ' V
        graph.DrawLine(pen, xPt, yPt, xPt, linV + mic_font.Height) ' V

        'lng = 60
        graph.DrawString("DEPRECIEREA", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd1, lng * 2, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd2 + mic_font.Height + under, xPt + lng * 2, rnd2 + mic_font.Height + under) ' H
        lngTot += lng
        graph.DrawString("Valoarea", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd3, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        nrCol += 1
        graph.DrawString(nrCol, mic_font, XBrushes.Black,
                         New XRect(xPt, rnd4, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd3 + mic_font.Height + under, xPt + lng, rnd3 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt, rnd4 + mic_font.Height + under, xPt + lng, rnd4 + mic_font.Height + under) ' H

        xPt += lng
        graph.DrawLine(pen, xPt, rnd3 + under, xPt, linV) ' V
        graph.DrawLine(pen, xPt, rnd4, xPt, rnd4 + mic_font.Height) ' V

        lngTot += lng
        graph.DrawString("Motivul", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd3, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        nrCol += 1
        graph.DrawString(nrCol, mic_font, XBrushes.Black,
                         New XRect(xPt, rnd4, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd3 + mic_font.Height + under, xPt + lng, rnd3 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt, rnd4 + mic_font.Height + under, xPt + lng, rnd4 + mic_font.Height + under) ' H


        xPt += lng
        graph.DrawLine(pen, xPt, yPt, xPt, linV) ' V
        graph.DrawLine(pen, xPt, rnd4, xPt, rnd4 + mic_font.Height) ' V

        graph.DrawLine(pen, xSTG, yPt, xSTG + lngTot, yPt) 'H linie orizontala Sus

        '----------------FIN-- CAP TABEL --FIN----------------------

        '---------------- FOOTER TABEL ----------------------

        xPt = 45
        yPt = 500

        rnd1 = yPt + 3
        rnd2 = rnd1 + 20
        rnd3 = rnd2 + 20
        rnd4 = rnd3 + 20
        linV = rnd4 + mic_font.Height
        under = 1   'distanta sub rand

        graph.DrawLine(pen, xPt, yPt, xPt, linV) ' V
        graph.DrawLine(pen, xSTG, yPt, xSTG + lngTot, yPt) 'H - orizontala sus
        lng = 180

        graph.DrawString("Numele si prenumele", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd1, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd2 + mic_font.Height + under, xPt + lng, rnd2 + mic_font.Height + under) ' H
        graph.DrawString("Semnatura", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd3, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        xPt += lng
        graph.DrawLine(pen, xPt, yPt, xPt, linV) ' V

        lng = 50 * 3
        graph.DrawString("Comisia de inventariere", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd1, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd2 + mic_font.Height + under, xPt + lng, rnd2 + mic_font.Height + under) ' H
        graph.DrawLine(pen, xPt + lng / 3, rnd2 + mic_font.Height + under, xPt + lng / 3, linV) ' V
        graph.DrawLine(pen, xPt + lng / 3 * 2, rnd2 + mic_font.Height + under, xPt + lng / 3 * 2, linV) ' V
        xPt += lng
        graph.DrawLine(pen, xPt, yPt, xPt, linV) ' V

        lng = 50 * 3
        'graph.DrawLine(pen, xPt, yPt, xPt + lng, yPt) 'H
        graph.DrawString("Gestionar", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd1, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd2 + mic_font.Height + under, xPt + lng, rnd2 + mic_font.Height + under) ' H
        xPt += lng
        graph.DrawLine(pen, xPt, yPt, xPt, linV) ' V

        lng = lngTot - xPt + xSTG
        'graph.DrawLine(pen, xPt, yPt, xPt + lng, yPt) 'H
        graph.DrawString("Contabilitate", mic_font, XBrushes.Black,
                         New XRect(xPt, rnd1, lng, pdfpage.Height.Point), XStringFormats.TopCenter)
        graph.DrawLine(pen, xPt, rnd2 + mic_font.Height + under, xPt + lng, rnd2 + mic_font.Height + under) ' H
        xPt += lng
        graph.DrawLine(pen, xSTG + lngTot, yPt, xSTG + lngTot, linV) ' V

        graph.DrawLine(pen, xSTG, linV, xSTG + lngTot, linV) 'H - orizontala jos
        '----------------FIN-- CAP TABEL --FIN----------------------


    End Function

    Private Sub ComboBox1_DropDownClosed(sender As Object, e As EventArgs) Handles ComboBox1.DropDownClosed
        DataGridView1.Rows.Clear()
        Load_Produse()
    End Sub
End Class
