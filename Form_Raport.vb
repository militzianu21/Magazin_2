Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization

Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing.Layout
Public Class Form_Raport
    Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Public dbread2 As MySqlDataReader
    Public folder_raport As String
    Public AppDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
    Private Sub Form_luna_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        'If Today.Day > 5 Then
        'ComboBox1.SelectedItem = Today.Month
        'Else
        'ComboBox1.SelectedItem = Today.Month - 1
        'End If
        'ComboBox2.SelectedItem = Today.Year

        ComboBox1.SelectedItem = Form_principal.ComboBox1.SelectedItem
        ComboBox2.SelectedItem = Form_principal.ComboBox2.SelectedItem
        Label1.Text = MonthName(ComboBox1.SelectedItem) & " " & ComboBox2.SelectedItem

        ComboBox3.DisplayMember = "Text"
        ComboBox3.ValueMember = "Value"
        Dim tb As New DataTable
        tb.Columns.Add("Text", GetType(String))
        tb.Columns.Add("Value", GetType(String))
        tb.Rows.Add("1. Petru Maior", "PM")
        tb.Rows.Add("2. Mihai Viteazu", "MV")

        ComboBox3.DataSource = tb
        ComboBox3.SelectedValue = Form_principal.ComboBox3.SelectedValue

        TextBox2.Clear()


        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")


        Dim luna_rap As Date = Date.Parse(Date.DaysInMonth(ComboBox2.SelectedItem, ComboBox1.SelectedItem) & "." & ComboBox1.SelectedItem & "." & ComboBox2.SelectedItem)
        Dim luna_prec As Date = DateAdd(DateInterval.Month, -1, luna_rap)
        Dim data_ultima As Date = Date.Parse(Date.DaysInMonth(luna_prec.Year, luna_prec.Month) & "." & luna_prec.Month & "." & luna_prec.Year)

        '------------ solduri luna precedenta
        Try
            Dim sql_read As String = "SELECT * FROM solduri_gestiune WHERE data=@data AND magazin='" & ComboBox3.SelectedValue & "'"
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_read, dbconn)
            dbcomm.Parameters.AddWithValue("@data", data_ultima)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows Then
                TextBox2.Text = (dbread("gestiune_sold_final").ToString)
            Else
                TextBox2.Text = 0
            End If
        Catch ex As Exception
            MsgBox("Problem loading data prec: " & ex.Message.ToString)
        End Try
        dbread.Close()
        dbconn.Close()
        Load_DGV()
    End Sub

    Private Sub Load_DGV()


        Dim luna_rap As Date = Date.Parse(Date.DaysInMonth(ComboBox2.SelectedItem, ComboBox1.SelectedItem) & "." & ComboBox1.SelectedItem & "." & ComboBox2.SelectedItem)
        Dim luna_prec As Date = DateAdd(DateInterval.Month, -1, luna_rap)
        Dim data_ultima As Date = Date.Parse(Date.DaysInMonth(luna_prec.Year, luna_prec.Month) & "." & luna_prec.Month & "." & luna_prec.Year)

        If DialogResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        Dim punct_lucru As String = ""
        If ComboBox3.SelectedValue = "PM" Then
            punct_lucru = "Magazin: Petru Maior nr. 9"
        ElseIf ComboBox3.SelectedValue = "MV" Then
            punct_lucru = "Magazin: Mihai Viteazu nr. 28"
        End If

        Dim mag As String = ComboBox3.SelectedValue

        Dim datault As Date = Today
        Dim dataInc As Date = Date.Parse("01." & luna_rap.Month & "." & luna_rap.Year)


        Dim sold_initial As Double = 0
        If IsDBNull(TextBox2.Text) = False Or TextBox2.Text <> "" Or TextBox2.Text <> Nothing Then
            sold_initial = CDbl(TextBox2.Text)
        End If
        Dim sold_prec As Double = sold_initial

        Dim incas_tot As Double = 0
        Dim chelt_tot As Double = 0

        Dim DGV As DataGridView = DataGridView1
        DGV.DataSource = Nothing
        DGV.Rows.Clear()
        DGV.Columns.Clear()
        DGV.RowHeadersVisible = False
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        Dim Col_0 As New DataGridViewTextBoxColumn
        With Col_0
            .HeaderText = "Data"
            .Name = "Col_0"
            .Width = DGV.Width * 15 / 100
            .ReadOnly = True
        End With

        Dim Col_1 As New DataGridViewTextBoxColumn
        With Col_1
            .HeaderText = "Numar"
            .Name = "Col_1"
            .Width = DGV.Width * 15 / 100
            .ReadOnly = True
        End With

        Dim Col_2 As New DataGridViewTextBoxColumn
        With Col_2
            .HeaderText = "Explicatii"
            .Name = "Col_2"
            .Width = DGV.Width * 35 / 100
            .ReadOnly = True
        End With
        Dim Col_3 As New DataGridViewTextBoxColumn
        With Col_3
            .HeaderText = "Intrari"
            .Name = "Col_3"
            .Width = DGV.Width * 15 / 100
            .ReadOnly = True
        End With
        Dim Col_4 As New DataGridViewTextBoxColumn
        With Col_4
            .HeaderText = "Vanzari"
            .Name = "Col_4"
            .Width = DGV.Width * 15 / 100
            .ReadOnly = True
        End With

        DGV.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Col_0, Col_1, Col_2, Col_3, Col_4})
        'A4 = 8.27x11.69" x72points/inch = 595x842 points

        '-----------------HEADER ----------------------------

        RichTextBox1.Clear()
        RichTextBox1.Text = "Raport Gestiune" & vbNewLine & punct_lucru & vbNewLine & Format(luna_rap, "MMMM yyyy")
        Dim reg_str As String = "Raport Gestiune"
        Dim mag_str As String = punct_lucru
        Dim dat_str As String = Format(luna_rap, "MMMM yyyy")

        RichTextBox1.Select(0, Len(reg_str))
        RichTextBox1.SelectionFont = New Font("Arial", 16, FontStyle.Bold, GraphicsUnit.Point)

        RichTextBox1.Select(Len(reg_str) + 1, Len(mag_str))
        RichTextBox1.SelectionFont = New Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point)

        RichTextBox1.Select(Len(reg_str) + Len(mag_str) + 2, Len(dat_str))
        RichTextBox1.SelectionFont = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        RichTextBox1.SelectAll()
        RichTextBox1.SelectionAlignment = HorizontalAlignment.Center

        Dim row As Integer = 0

        DGV.Rows.Add()
        DataGridView1.Rows(row).Cells(2).Value = "Sold initial"
        DataGridView1.Rows(row).Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleRight
        'DataGridView1.Rows(row).Cells(2).AdjustCellBorderStyle(DGV.AdvancedCellBorderStyle, DGV.AdvancedCellBorderStyle, False, True, True, True)
        DataGridView1.Rows(row).Cells(3).Value = FormatNumber(CDec(sold_prec), 2, TriState.True, TriState.False, TriState.True)
        DataGridView1.Rows(row).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.Rows(row).DefaultCellStyle.Font = New Font(DGV.Font, FontStyle.Bold)
        'DataGridView1.Rows(row).DefaultCellStyle.BackColor = Color.PaleTurquoise
        row = row + 1
        '^^^-----------------HEADER ---------------------------^^^



        Dim sold_zi As Double = 0
        Dim sold_rand As Double = 0


        Dim nxtRow As Double = 130
        Dim nxtRow2 As Double = 130
        Dim suma_int As Double = 0
        Dim suma_ies As Double = 0
        Dim intrari_total As Double = 0
        Dim iesiri_total As Double = 0

        For d = 1 To Date.DaysInMonth(luna_rap.Year, luna_rap.Month)

            Dim data As String = Format(Date.Parse(d & "." & luna_rap.Month & "." & luna_rap.Year), "yyyy-MM-dd")

            Dim datatext As Date = data.ToString

            datault = datatext
            '---------------IESIRI/VANZARI >>>INCASARI<<<<
            Try
                Dim sql_chl As String = "SELECT * FROM intrari WHERE data='" & data & "' AND magazin='" & mag & "'"
                If ComboBox3.SelectedValue = "TOT" Then
                    sql_chl = "SELECT * FROM intrari WHERE data='" & data & "'"
                End If
                If ComboBox3.SelectedValue = "COM" Then
                    sql_chl = "SELECT * FROM intrari WHERE data='" & data & "'"
                End If
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If

                Dim intr As Boolean = False
                dbcomm = New MySqlCommand(sql_chl, dbconn)
                dbread2 = dbcomm.ExecuteReader()
                If dbread2.HasRows = True Then
                    intr = True
                End If
                dbread2.Close()

                Dim sql_inc As String = "SELECT * FROM iesiri WHERE data='" & data & "' AND magazin='" & mag & "'"
                If ComboBox3.SelectedValue = "TOT" Then
                    sql_inc = "SELECT * FROM iesiri WHERE data='" & data & "'"
                End If
                If ComboBox3.SelectedValue = "COM" Then
                    sql_inc = "SELECT * FROM iesiri WHERE data='" & data & "'"
                End If
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(sql_inc, dbconn)
                dbread = dbcomm.ExecuteReader()

                If dbread.HasRows = True Or intr = True Then
                    suma_ies = 0

                    While dbread.Read

                        iesiri_total = iesiri_total + CDbl(dbread("suma"))
                        suma_ies = suma_ies + CDbl(dbread("suma"))

                        Dim suma_incasari As String = FormatNumber(CDec(dbread("suma")), 2, TriState.True, TriState.False, TriState.True)

                        DGV.Rows.Add()
                        DataGridView1.Rows(row).Cells(0).Value = Format(CDate(dbread("data")), "dd.MM.yyy")
                        DataGridView1.Rows(row).Cells(0).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                        DataGridView1.Rows(row).Cells(1).Value = dbread("nr_rzf")
                        DataGridView1.Rows(row).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                        DataGridView1.Rows(row).Cells(2).Value = vbTab & dbread("tip_incasare")
                        DataGridView1.Rows(row).Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                        DataGridView1.Rows(row).Cells(3).Value = "-"
                        DataGridView1.Rows(row).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        DataGridView1.Rows(row).Cells(4).Value = suma_incasari
                        DataGridView1.Rows(row).Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        'DataGridView1.Rows(row).DefaultCellStyle.Font = New Font(DGV.Font, FontStyle.Bold)
                        'If row Mod 2 = 0 Then
                        '    DataGridView1.Rows(row).DefaultCellStyle.BackColor = Color.LightGray
                        'Else
                        '    DataGridView1.Rows(row).DefaultCellStyle.BackColor = Color.White
                        'End If

                        row = row + 1
                    End While

                    dbread.Close()

                    '---------------INTRARI >>>GESTIUNE<<<<
                    Dim sql_gest As String = "SELECT * FROM intrari WHERE data='" & data & "' AND magazin='" & mag & "'"
                    If ComboBox3.SelectedValue = "TOT" Then
                        sql_gest = "SELECT * FROM intrari WHERE data='" & data & "'"
                    End If
                    If ComboBox3.SelectedValue = "COM" Then
                        sql_gest = "SELECT * FROM intrari WHERE data='" & data & "'"
                    End If
                    dbcomm = New MySqlCommand(sql_gest, dbconn)
                    dbread = dbcomm.ExecuteReader()

                    suma_int = 0
                    While dbread.Read

                        intrari_total = intrari_total + CDbl(dbread("suma"))
                        suma_int = suma_int + CDbl(dbread("suma"))

                        Dim suma_cheltuiala As String = FormatNumber(CDec(dbread("suma")), 2, TriState.True, TriState.False, TriState.True)

                        DGV.Rows.Add()
                        DataGridView1.Rows(row).Cells(0).Value = Format(CDate(dbread("data")), "dd.MM.yyy")
                        DataGridView1.Rows(row).Cells(0).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                        DataGridView1.Rows(row).Cells(1).Value = dbread("nr_nir")
                        DataGridView1.Rows(row).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                        DataGridView1.Rows(row).Cells(2).Value = vbTab & dbread("explicatii")
                        DataGridView1.Rows(row).Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                        DataGridView1.Rows(row).Cells(3).Value = suma_cheltuiala
                        DataGridView1.Rows(row).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        DataGridView1.Rows(row).Cells(4).Value = "-"
                        DataGridView1.Rows(row).Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        row = row + 1

                    End While

                    sold_zi = sold_prec - suma_ies + suma_int
                    sold_prec = sold_zi
                End If
                dbread.Close()

            Catch ex As Exception
                MsgBox("Problem loading date: " & ex.Message.ToString)
            End Try

        Next
        ''-----------------FOOTER ----------------------------
        TextBox4.Text = FormatNumber(CDec(sold_zi), 2, TriState.True, TriState.False, TriState.True)

        ''-----------------FOOTER ----------------------------

        For i = 1 To DGV.Rows.Count - 1
            If DGV.Rows(i).Cells(0).Value = DGV.Rows(i - 1).Cells(0).Value Then
                DGV.Rows(i).DefaultCellStyle.BackColor = DGV.Rows(i - 1).DefaultCellStyle.BackColor
            ElseIf DGV.Rows(i).Cells(0).Value <> DGV.Rows(i - 1).Cells(0).Value And DGV.Rows(i - 1).DefaultCellStyle.BackColor <> Color.LightGray Then
                DGV.Rows(i).DefaultCellStyle.BackColor = Color.LightGray
            End If
        Next

        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
        Else : PictureBox1.BackColor = Color.LightYellow
        End If




    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        ' -------------------- LOAD setari

        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")

        Try
            Dim set_sql As String = "SELECT * FROM setari WHERE setare='path_raport_gestiune'"
            dbconn.Open()
            dbcomm = New MySqlCommand(set_sql, dbconn)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = False Then
                SaveFileDialog1.Title = "Selectati Folderul pt Raportul de Gestiune"
                SaveFileDialog1.FileName = "Selectati Folderul"
                SaveFileDialog1.Filter = "pdf Files (*.pdf*)|*.pdf"

                If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim folder As String = System.IO.Path.GetDirectoryName(SaveFileDialog1.FileName) & "\"
                    Dim dbconn As MySqlConnection = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
                    Try
                        If dbconn.State = 0 Then
                            dbconn.Open()
                        End If
                        Dim set_path_sql As String = "REPLACE INTO setari(setare,valoare) VALUES('path_raport_gestiune',@folder)"
                        Using dbcomm As MySqlCommand = New MySqlCommand(set_path_sql, dbconn)
                            dbcomm.Parameters.AddWithValue("@folder", folder)
                            dbcomm.ExecuteNonQuery()
                        End Using
                    Catch ex As Exception
                        MsgBox("Problem Nonquery: " & ex.Message.ToString)
                    End Try
                End If

            ElseIf dbread.HasRows = True Then
                folder_raport = dbread("valoare")
            End If

        Catch ex As Exception
            MsgBox("Problem loading setari savefile: " & ex.Message.ToString)
        End Try
        dbread.Close()
        '--------------------------------

        'Form_luna_Raport.ShowDialog()
        'Form_luna_Raport.ComboBox1.SelectedItem = ComboBox1.SelectedItem
        Dim luna_rap As Date = Date.Parse(Date.DaysInMonth(ComboBox2.SelectedItem, ComboBox1.SelectedItem) & "." & ComboBox1.SelectedItem & "." & ComboBox2.SelectedItem)
        Dim pdf As PdfSharp.Pdf.PdfDocument = New PdfSharp.Pdf.PdfDocument
        ' Dim luna_reg As Date = CDate("01.03.2017")
        'If Form_luna_Raport.DialogResult = Windows.Forms.DialogResult.Cancel Then
        '    Exit Sub
        'End If
        Dim punct_lucru As String = ""
        If ComboBox3.SelectedValue = "PM" Then
            punct_lucru = "Magazin: Petru Maior nr. 9"
        ElseIf ComboBox3.SelectedValue = "MV" Then
            punct_lucru = "Magazin: Mihai Viteazu nr. 28"
        End If

        Dim mag As String = ComboBox3.SelectedValue

        pdf.Info.Title = "Raport Gestiune"
        Dim pdfPage As PdfSharp.Pdf.PdfPage = pdf.AddPage


        Dim graph As XGraphics = XGraphics.FromPdfPage(pdfPage)


        Dim titlu_font As XFont = New XFont("Verdana", 16, XFontStyle.Bold)
        Dim top_font As XFont = New XFont("Verdana", 12, XFontStyle.Bold)
        Dim report_font As XFont = New XFont("Verdana", 10, XFontStyle.Bold)
        Dim report_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim text_font As XFont = New XFont("Verdana", 10, XFontStyle.Regular)
        Dim text_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim bottom_font As XFont = New XFont("Verdana", 12, XFontStyle.Bold)

        Dim datault As Date = Today
        Dim dataInc As Date = Date.Parse("01." & luna_rap.Month & "." & luna_rap.Year)


        Dim sold_initial As Double = CDbl(TextBox2.Text)
        Dim sold_prec As Double = sold_initial

        Dim incas_tot As Double = 0
        Dim chelt_tot As Double = 0
        'A4 = 8.27x11.69" x72points/inch = 595x842 points

        '-----------------HEADER ----------------------------
        graph.DrawString("MILICOM CAZ SRL", report_font, XBrushes.Black, _
                         New XRect(20, 20, 100, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("CIF: 31240216", text_font, XBrushes.Black, _
                         New XRect(20, 35, 100, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(punct_lucru, report_font, XBrushes.Black, _
                         New XRect(415, 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("RAPORT DE GESTIUNE PERIODIC", titlu_font, XBrushes.Black, _
                         New XRect(20, 50, 575, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawString("de la " & dataInc, top_font, XBrushes.Black, _
                          New XRect(185, 73, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawLine(XPens.Black, 20, 100, 575, 100)
        graph.DrawLine(XPens.Black, 20, 129, 575, 129)


        graph.DrawString("Document", top_font, XBrushes.Black, _
                         New XRect(20, 100, 140, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("Data", top_font, XBrushes.Black, _
                         New XRect(20, 115, 70, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("Numar", top_font, XBrushes.Black, _
                         New XRect(90, 115, 50, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawString("Explicatii".ToString, top_font, XBrushes.Black, _
                         New XRect(200, 105, 240, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawString("Marfuri".ToString, top_font, XBrushes.Black, _
                         New XRect(450, 100, 120, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("Intrari".ToString, top_font, XBrushes.Black, _
                         New XRect(450, 115, 60, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("Vanzari".ToString, top_font, XBrushes.Black, _
                         New XRect(510, 115, 60, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawString("Sold precedent".ToString, report_font, XBrushes.Black, _
                        New XRect(360, 131, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(FormatNumber(CDec(sold_prec), 2, TriState.True, TriState.False, TriState.True), report_font, XBrushes.Black, _
                        New XRect(450, 131, 120, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(XPens.Black, 20, 145, 575, 145)
        '^^^-----------------HEADER ---------------------------^^^



        Dim sold_zi As Double = 0
        Dim sold_rand As Double = 0


        Dim nxtRow As Double = 130
        Dim nxtRow2 As Double = 130
        Dim suma_int As Double = 0
        Dim suma_ies As Double = 0
        Dim intrari_total As Double = 0
        Dim iesiri_total As Double = 0

        For d = 1 To Date.DaysInMonth(luna_rap.Year, luna_rap.Month)

            Dim data As String = Format(Date.Parse(d & "." & luna_rap.Month & "." & luna_rap.Year), "yyyy-MM-dd")

            Dim datatext As Date = data.ToString

            datault = datatext
            '---------------IESIRI/VANZARI >>>INCASARI<<<<
            Try
                Dim sql_chl As String = "SELECT * FROM intrari WHERE data='" & data & "' AND magazin='" & mag & "'"


                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If

                Dim intr As Boolean = False
                dbcomm = New MySqlCommand(sql_chl, dbconn)
                dbread2 = dbcomm.ExecuteReader()
                If dbread2.HasRows = True Then
                    intr = True
                End If
                dbread2.Close()

                Dim sql_inc As String = "SELECT * FROM iesiri WHERE data='" & data & "' AND magazin='" & mag & "'"

                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(sql_inc, dbconn)
                dbread = dbcomm.ExecuteReader()

                If dbread.HasRows = True Or intr = True Then
                    suma_ies = 0

                    While dbread.Read

                        Dim tf As XTextFormatter = New XTextFormatter(graph)
                        tf.Alignment = XParagraphAlignment.Right
                        nxtRow = nxtRow + 20

                        If nxtRow > 750 Then

                            graph.DrawString("pana la " & datatext, top_font, XBrushes.Black, _
                          New XRect(305, 73, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

                            'vvvv-----------------FOOTER ---------------------------vvvv

                            graph.DrawLine(XPens.Black, 20, 774, 575, 774)
                            graph.DrawLine(XPens.Black, 20, 790, 575, 790)

                            graph.DrawString("Sold final(report)".ToString, report_font, XBrushes.Black, _
                                             New XRect(360, 776, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

                            Dim sold_pag As Double = sold_prec + suma_ies
                            graph.DrawString(FormatNumber(CDec(sold_pag), 2, TriState.True, TriState.False, TriState.True), report_font, XBrushes.Black, _
                                           New XRect(450, 776, 120, pdfPage.Height.Point), XStringFormats.TopCenter)
                            sold_prec = sold_pag

                            graph.DrawString("Gestionar".ToString, text_font, XBrushes.Black, _
                                            New XRect(20, 795, 257, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString("Verificat".ToString, text_font, XBrushes.Black, _
                                            New XRect(278, 795, 297, pdfPage.Height.Point), XStringFormats.TopCenter)

                            '^^^^-----------------FOOTER ---------------------------^^^^

                            pdfPage = pdf.AddPage()
                            graph = XGraphics.FromPdfPage(pdfPage)
                            tf = New XTextFormatter(graph)
                            tf.Alignment = XParagraphAlignment.Right

                            '                '-----------------HEADER ----------------------------


                            graph.DrawString("MILICOM CAZ SRL", report_font, XBrushes.Black, _
                             New XRect(20, 20, 100, pdfPage.Height.Point), XStringFormats.TopLeft)
                            graph.DrawString("CIF: 31240216", text_font, XBrushes.Black, _
                                             New XRect(20, 35, 100, pdfPage.Height.Point), XStringFormats.TopLeft)
                            graph.DrawString(punct_lucru, report_font, XBrushes.Black, _
                                             New XRect(415, 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

                            graph.DrawString("RAPORT DE GESTIUNE PERIODIC", titlu_font, XBrushes.Black, _
                                             New XRect(20, 50, 575, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString("de la " & datatext, top_font, XBrushes.Black, _
                                             New XRect(185, 73, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

                            graph.DrawLine(XPens.Black, 20, 100, 575, 100)
                            graph.DrawLine(XPens.Black, 20, 129, 575, 129)


                            graph.DrawString("Document", top_font, XBrushes.Black, _
                                             New XRect(20, 100, 140, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString("Data", top_font, XBrushes.Black, _
                                             New XRect(20, 115, 70, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString("Numar", top_font, XBrushes.Black, _
                                             New XRect(90, 115, 50, pdfPage.Height.Point), XStringFormats.TopCenter)

                            graph.DrawString("Explicatii".ToString, top_font, XBrushes.Black, _
                                             New XRect(200, 105, 240, pdfPage.Height.Point), XStringFormats.TopCenter)

                            graph.DrawString("Marfuri".ToString, top_font, XBrushes.Black, _
                                             New XRect(450, 100, 120, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString("Intrari".ToString, top_font, XBrushes.Black, _
                                             New XRect(450, 115, 60, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString("Vanzari".ToString, top_font, XBrushes.Black, _
                                             New XRect(510, 115, 60, pdfPage.Height.Point), XStringFormats.TopCenter)

                            graph.DrawString("Sold precedent".ToString, report_font, XBrushes.Black, _
                                            New XRect(360, 131, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)
                            graph.DrawString((FormatNumber(CDec(sold_pag), 2, TriState.True, TriState.False, TriState.True)), report_font, XBrushes.Black, _
                                            New XRect(450, 131, 120, pdfPage.Height.Point), XStringFormats.TopCenter)

                            graph.DrawLine(XPens.Black, 20, 145, 575, 145)
                            '                '-----------------HEADER ---------------------------

                            nxtRow = 150

                        End If

                        iesiri_total = iesiri_total + CDbl(dbread("suma"))
                        suma_ies = suma_ies + CDbl(dbread("suma"))
                        graph.DrawString(dbread("data"), text_font, XBrushes.Black, _
                                         New XRect(20, nxtRow, 70, pdfPage.Height.Point), XStringFormats.TopCenter)
                        graph.DrawString(dbread("nr_rzf"), text_font, XBrushes.Black, _
                                         New XRect(90, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)


                        graph.DrawString(dbread("tip_incasare"), text_font, XBrushes.Black, _
                                         New XRect(200, nxtRow, 240, pdfPage.Height.Point), XStringFormats.TopCenter) 'explicatii

                        Dim suma_incasari As String = FormatNumber(CDec(dbread("suma")), 2, TriState.True, TriState.False, TriState.True)

                        tf.DrawString("-", text_font, XBrushes.Black, _
                                      New XRect(450, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopLeft)
                        tf.DrawString(suma_incasari, text_font, XBrushes.Black, New XRect(520, nxtRow, 55, pdfPage.Height.Point), XStringFormats.TopLeft)
                    End While

                    dbread.Close()

                    '---------------INTRARI >>>GESTIUNE<<<<
                    Dim sql_gest As String = "SELECT * FROM intrari WHERE data='" & data & "' AND magazin='" & mag & "'"

                    dbcomm = New MySqlCommand(sql_gest, dbconn)
                    dbread = dbcomm.ExecuteReader()

                    suma_int = 0
                    While dbread.Read

                        Dim tf As XTextFormatter = New XTextFormatter(graph)
                        tf.Alignment = XParagraphAlignment.Right


                        nxtRow = nxtRow + 20

                        If nxtRow > 750 Then
                            graph.DrawString("pana la " & datatext, top_font, XBrushes.Black, _
                         New XRect(303, 73, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

                            '-----------------FOOTER ----------------------------

                            graph.DrawLine(XPens.Black, 20, 774, 575, 774)
                            graph.DrawLine(XPens.Black, 20, 790, 575, 790)

                            graph.DrawString("Sold final(report)".ToString, report_font, XBrushes.Black, _
                                            New XRect(360, 776, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)
                            Dim sold_pag As Double = sold_prec + suma_int - suma_ies
                            graph.DrawString(FormatNumber(CDec(sold_pag), 2, TriState.True, TriState.False, TriState.True), report_font, XBrushes.Black, _
                                           New XRect(450, 776, 120, pdfPage.Height.Point), XStringFormats.TopCenter)

                            graph.DrawString("Gestionar".ToString, text_font, XBrushes.Black, _
                                            New XRect(20, 795, 257, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString("Verificat".ToString, text_font, XBrushes.Black, _
                                            New XRect(278, 795, 297, pdfPage.Height.Point), XStringFormats.TopCenter)
                            '                '-----------------FOOTER ----------------------------

                            pdfPage = pdf.AddPage()
                            graph = XGraphics.FromPdfPage(pdfPage)
                            tf = New XTextFormatter(graph)
                            tf.Alignment = XParagraphAlignment.Right

                            ' VVVV-----------------HEADER ------------------------VVVV


                            graph.DrawString("MILICOM CAZ SRL", report_font, XBrushes.Black, _
                             New XRect(20, 20, 100, pdfPage.Height.Point), XStringFormats.TopLeft)
                            graph.DrawString("CIF: 31240216", text_font, XBrushes.Black, _
                                             New XRect(20, 35, 100, pdfPage.Height.Point), XStringFormats.TopLeft)
                            graph.DrawString(punct_lucru, report_font, XBrushes.Black, _
                                             New XRect(415, 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

                            graph.DrawString("RAPORT DE GESTIUNE PERIODIC", titlu_font, XBrushes.Black, _
                                             New XRect(20, 50, 575, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString("de la " & datatext, top_font, XBrushes.Black, _
                         New XRect(185, 73, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

                            graph.DrawLine(XPens.Black, 20, 100, 575, 100)
                            graph.DrawLine(XPens.Black, 20, 129, 575, 129)


                            graph.DrawString("Document", top_font, XBrushes.Black, _
                                             New XRect(20, 100, 140, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString("Data", top_font, XBrushes.Black, _
                                             New XRect(20, 115, 70, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString("Numar", top_font, XBrushes.Black, _
                                             New XRect(90, 115, 50, pdfPage.Height.Point), XStringFormats.TopCenter)

                            graph.DrawString("Explicatii".ToString, top_font, XBrushes.Black, _
                                             New XRect(200, 105, 240, pdfPage.Height.Point), XStringFormats.TopCenter)

                            graph.DrawString("Marfuri".ToString, top_font, XBrushes.Black, _
                                             New XRect(450, 100, 120, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString("Intrari".ToString, top_font, XBrushes.Black, _
                                             New XRect(450, 115, 60, pdfPage.Height.Point), XStringFormats.TopCenter)
                            graph.DrawString("Vanzari".ToString, top_font, XBrushes.Black, _
                                             New XRect(510, 115, 60, pdfPage.Height.Point), XStringFormats.TopCenter)

                            graph.DrawString("Sold precedent".ToString, report_font, XBrushes.Black, _
                                            New XRect(360, 131, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

                            graph.DrawString((FormatNumber(CDec(sold_pag), 2, TriState.True, TriState.False, TriState.True)), report_font, XBrushes.Black, _
                                            New XRect(450, 131, 120, pdfPage.Height.Point), XStringFormats.TopCenter)

                            graph.DrawLine(XPens.Black, 20, 145, 575, 145)
                            '                '-----------------HEADER ---------------------------

                            nxtRow = 150

                        End If

                        intrari_total = intrari_total + CDbl(dbread("suma"))
                        suma_int = suma_int + CDbl(dbread("suma"))
                        nxtRow = nxtRow

                        graph.DrawString(dbread("data"), text_font, XBrushes.Black, _
                                         New XRect(20, nxtRow, 70, pdfPage.Height.Point), XStringFormats.TopCenter)
                        graph.DrawString(dbread("nr_nir"), text_font, XBrushes.Black, _
                                         New XRect(90, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)

                        graph.DrawString(dbread("explicatii"), text_font, XBrushes.Black, _
                                         New XRect(200, nxtRow, 240, pdfPage.Height.Point), XStringFormats.TopCenter)

                        Dim suma_cheltuiala As String = FormatNumber(CDec(dbread("suma")), 2, TriState.True, TriState.False, TriState.True)
                        tf.DrawString(suma_cheltuiala, text_font, XBrushes.Black, _
                                      New XRect(450, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopLeft)
                        tf.DrawString("-", text_font, XBrushes.Black, _
                                      New XRect(520, nxtRow, 55, pdfPage.Height.Point), XStringFormats.TopLeft)

                    End While

                    sold_zi = sold_prec - suma_ies + suma_int
                    sold_prec = sold_zi
                End If
                dbread.Close()

            Catch ex As Exception
                MsgBox("Problem loading data: " & ex.Message.ToString)
            End Try

        Next
        graph.DrawString("pana la " & datault, top_font, XBrushes.Black, _
                         New XRect(305, 73, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)
        ''-----------------FOOTER ----------------------------

        graph.DrawLine(XPens.Black, 20, 774, 575, 774)
        graph.DrawLine(XPens.Black, 20, 790, 575, 790)

        graph.DrawString("Sold final(report)".ToString, report_font, XBrushes.Black, _
                         New XRect(360, 776, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)


        graph.DrawString(FormatNumber(CDec(sold_zi), 2, TriState.True, TriState.False, TriState.True), report_font, XBrushes.Black, _
                        New XRect(450, 776, 120, pdfPage.Height.Point), XStringFormats.TopCenter)


        graph.DrawString("Gestionar".ToString, text_font, XBrushes.Black, _
                        New XRect(20, 795, 257, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("Verificat".ToString, text_font, XBrushes.Black, _
                        New XRect(278, 795, 297, pdfPage.Height.Point), XStringFormats.TopCenter)
        ''-----------------FOOTER ----------------------------


        'Try
        '------------solduri luna RAPORT
        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If

            dbcomm = New MySqlCommand("SELECT * FROM solduri_gestiune WHERE data=@data AND magazin='" & ComboBox3.SelectedValue & "'", dbconn)
            dbcomm.Parameters.AddWithValue("@data", luna_rap)

            dbread2 = dbcomm.ExecuteReader()
            dbread2.Read()

            If dbread2.HasRows = True AndAlso dbread2("permanent") = True Then
                MsgBox("Atentie!!! Soldurile nu se pot modifica")
                dbread2.Close()
            ElseIf dbread2.HasRows = False Or (dbread2.HasRows = True AndAlso dbread2("permanent") = False) Then
                dbread2.Close()
                Dim sql_act As String = "INSERT INTO solduri_gestiune (data,gestiune_sold_initial,intrari,iesiri,gestiune_sold_final,magazin) " _
                                   & "VALUES(@data,@gestiune_sold_initial,@intrari,@iesiri,@gestiune_sold_final,@magazin) ON DUPLICATE KEY UPDATE " _
                                   & "gestiune_sold_initial=@gestiune_sold_initial,intrari=@intrari,iesiri=@iesiri,gestiune_sold_final=@gestiune_sold_final,magazin=@magazin"


                Dim data_act As Date = Format(Date.Parse(Date.DaysInMonth(luna_rap.Year, luna_rap.Month) & "." & luna_rap.Month & "." & luna_rap.Year), "yyyy-MM-dd")
                Try

                    If dbconn.State = 0 Then
                        dbconn.Open()
                    End If
                    dbcomm = New MySqlCommand(sql_act, dbconn)
                    Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
                    dbcomm.Parameters.AddWithValue("@data", data_act)
                    dbcomm.Parameters.AddWithValue("@gestiune_sold_initial", sold_initial)
                    dbcomm.Parameters.AddWithValue("@intrari", intrari_total)
                    dbcomm.Parameters.AddWithValue("@iesiri", iesiri_total)
                    dbcomm.Parameters.AddWithValue("@gestiune_sold_final", sold_prec)
                    dbcomm.Parameters.AddWithValue("@magazin", mag)

                    dbcomm.ExecuteNonQuery()
                    transaction.Commit()
                    dbconn.Close()
                Catch ex As Exception
                    MsgBox("Failed to insert data: " & ex.Message.ToString())
                End Try
            End If


            Dim sql_prec As String = "INSERT INTO solduri_gestiune (data,gestiune_sold_final,magazin) " _
                               & "VALUES(@data,@gestiune_sold_final,@magazin) ON DUPLICATE KEY UPDATE gestiune_sold_final=@gestiune_sold_final"

            Dim luna_prec As Date = DateAdd(DateInterval.Month, -1, luna_rap)
            Dim data_prec As Date = Format(Date.Parse(Date.DaysInMonth(luna_prec.Year, luna_prec.Month) & "." & luna_prec.Month & "." & luna_prec.Year), "yyyy-MM-dd")
            Try
                If dbconn.State = 0 Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(sql_prec, dbconn)
                Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
                dbcomm.Parameters.AddWithValue("@data", data_prec)
                dbcomm.Parameters.AddWithValue("@gestiune_sold_final", CDec(TextBox2.Text))
                dbcomm.Parameters.AddWithValue("@magazin", ComboBox3.SelectedValue)


                dbcomm.ExecuteNonQuery()
                transaction.Commit()
                dbconn.Close()
            Catch ex As Exception
                MsgBox("Failed to insert data in solduri_gestiune : " & ex.Message.ToString())
            End Try

            Form_principal.Load_Solduri()
            Dim mag_id As String = ""
            If mag = "PM" Then
                mag_id = "PM"
            ElseIf mag = "MV" Then
                mag_id = "MV"

            End If
            Dim pdfFilename As String = folder_raport & "Raport Gestiune_" & luna_rap.Year & "_" & Format(luna_rap, "MM") & "_" & mag_id & ".pdf"
            If System.IO.File.Exists(pdfFilename) = True Then
                Dim OkCancel As Integer = MsgBox("Fisierul exista. Inlocuiti?", MsgBoxStyle.OkCancel)
                If OkCancel = DialogResult.Cancel Then
                    Exit Sub
                ElseIf OkCancel = DialogResult.OK Then
                    pdf.Save(pdfFilename)
                    Process.Start(pdfFilename)
                    Form_principal.Load_Registru_Listview()
                End If
            Else : pdf.Save(pdfFilename)
                Process.Start(pdfFilename)
                Form_principal.Load_Registru_Listview()
            End If

        End Using
    End Sub


    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
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
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

        If TextBox2.Text = "" Then
            TextBox2.Text = 0
        End If

    End Sub
    Private Sub TextBox1_Keyup(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox2.KeyUp
        Timer1.Interval = 1000
        Timer1.Stop()
        Timer1.Start()
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Load_DGV()
    End Sub
    Private Sub ComboBox3_DropDownClosed(sender As Object, e As EventArgs) Handles ComboBox3.DropDownClosed
        Label1.Text = MonthName(ComboBox1.SelectedItem) & " " & ComboBox2.SelectedItem
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")

        Dim luna_rap As Date = Date.Parse(Date.DaysInMonth(ComboBox2.SelectedItem, ComboBox1.SelectedItem) & "." & ComboBox1.SelectedItem & "." & ComboBox2.SelectedItem)
        Dim luna_prec As Date = DateAdd(DateInterval.Month, -1, luna_rap)
        Dim data_ultima As Date = Format(Date.Parse(Date.DaysInMonth(luna_prec.Year, luna_prec.Month) & "." & luna_prec.Month & "." & luna_prec.Year), "yyyy-MM-dd")

        Dim sql_read As String = "SELECT * FROM solduri_gestiune WHERE data=@data and magazin='" & ComboBox3.SelectedValue & "'"

        Try
            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_read, dbconn)
            dbcomm.Parameters.AddWithValue("@data", data_ultima)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows Then
                TextBox2.Text = (dbread("gestiune_sold_final").ToString)
                If dbread("permanent") = True Then
                    TextBox2.Enabled = False
                ElseIf dbread("permanent") = False Then
                    TextBox2.Enabled = True
                End If
            Else
                TextBox2.Enabled = True
                TextBox2.Text = 0
            End If

        Catch ex As Exception
            MsgBox("Problem loading data: " & ex.Message.ToString)

        End Try
        dbread.Close()
        dbconn.Close()
        'Load_Raport()
        Load_DGV()

    End Sub

    Private Sub ComboBox1_2_DropDownClosed(sender As Object, e As EventArgs) Handles ComboBox1.DropDownClosed, ComboBox2.DropDownClosed

        Dim luna_rap As Date = Date.Parse(Date.DaysInMonth(ComboBox2.SelectedItem, ComboBox1.SelectedItem) & "." & ComboBox1.SelectedItem & "." & ComboBox2.SelectedItem)
        Dim luna_prec As Date = DateAdd(DateInterval.Month, -1, luna_rap)
        Dim data_ultima As Date = Date.Parse(Date.DaysInMonth(luna_prec.Year, luna_prec.Month) & "." & luna_prec.Month & "." & luna_prec.Year)



        Label1.Text = MonthName(ComboBox1.SelectedItem) & " " & ComboBox2.SelectedItem

        Using dbconn As New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")

            Dim sql_read As String = "SELECT * FROM solduri_gestiune WHERE data=@data AND magazin='" & ComboBox3.SelectedValue & "'"


            '------------ solduri luna precedenta
            Try

                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(sql_read, dbconn)
                dbcomm.Parameters.AddWithValue("@data", data_ultima)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If dbread.HasRows Then
                    TextBox2.Text = (dbread("gestiune_sold_final").ToString)
                Else
                    TextBox2.Text = 0
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
                dbcomm.Parameters.AddWithValue("@data", luna_rap)
                dbread2 = dbcomm.ExecuteReader()
                dbread2.Read()
                If dbread2.HasRows Then
                    If dbread2("permanent") = True Then
                        TextBox2.Enabled = False
                        If TextBox2.Text = 0 Then
                            TextBox2.Text = (dbread2("gestiune_sold_initial").ToString)
                            'MsgBox((dbread2("gestiune_sold_initial").ToString))
                        End If
                    ElseIf dbread2("permanent") = False Then
                        TextBox2.Enabled = True
                    End If
                Else
                    TextBox2.Enabled = True
                End If

            Catch ex As Exception
                MsgBox("Problem loading data raport: " & ex.Message.ToString)
                dbread2.Close()
            End Try
        End Using
        TextBox2.Select()
        TextBox2.Focus()



        Load_DGV()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

End Class