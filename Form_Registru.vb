Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization

Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing.Layout
Public Class Form_Registru
    Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Public dbread2 As MySqlDataReader
    Public folder_registru As String
    Public AppDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)


    Private Sub Form_luna_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Alege Data Registru/Raport"
        Me.StartPosition = FormStartPosition.CenterScreen
        Application.CurrentCulture = New CultureInfo("ro-RO")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("ro-RO")

        RichTextBox1.Clear()

        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()
        For m = 1 To 12
            ComboBox1.Items.Add(m)
        Next
        For y = 2013 To Today.Year
            ComboBox2.Items.Add(y)
        Next

        'If Today.Day > 5 Then
        '    ComboBox1.SelectedItem = Today.Month
        'Else
        '    ComboBox1.SelectedItem = Today.Month - 1
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


        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
        Dim luna_reg As Date = Date.Parse(Date.DaysInMonth(ComboBox2.SelectedItem, ComboBox1.SelectedItem) & "." & ComboBox1.SelectedItem & "." & ComboBox2.SelectedItem)
        Dim luna_prec As Date = DateAdd(DateInterval.Month, -1, luna_reg)
        Dim data_ultima As Date = Date.Parse(Date.DaysInMonth(luna_prec.Year, luna_prec.Month) & "." & luna_prec.Month & "." & luna_prec.Year)

        Dim sql_read As String = "SELECT * FROM solduri_casa WHERE data=@data AND magazin='" & ComboBox3.SelectedValue & "'"
        Try
            dbconn.Open()
            dbcomm = New MySqlCommand(sql_read, dbconn)
            dbcomm.Parameters.AddWithValue("@data", data_ultima)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows Then
                TextBox1.Text = dbread("casa_sold_final").ToString
            Else
                TextBox1.Text = 0
            End If

        Catch ex As Exception
            MsgBox("Problem loading data: " & ex.Message.ToString)

        End Try
        dbread.Close()
        '------------solduri luna Registru
        Try

            dbcomm = New MySqlCommand("SELECT * FROM solduri_casa WHERE data=@data AND magazin='" & ComboBox3.SelectedValue & "'", dbconn)
            dbcomm.Parameters.AddWithValue("@data", luna_reg)
            dbread2 = dbcomm.ExecuteReader()
            dbread2.Read()
            If dbread2.HasRows Then
                If dbread2("permanent") = True Then
                    TextBox1.Enabled = False
                    If TextBox1.Text = "0" Then
                        TextBox1.Text = (dbread2("casa_sold_initial").ToString)
                    End If
                ElseIf dbread2("permanent") = False Then
                    TextBox1.Enabled = True
                End If
            Else
                TextBox1.Enabled = True
            End If
        Catch ex As Exception
            MsgBox("Problem loading data registru: " & ex.Message.ToString)
            'dbread2.Close()
        End Try
        dbread2.Close()
        dbconn.Close()
        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
        End If
        'Load_Registru()
        Load_DGV()
    End Sub
    Private Sub Form_luna_Registru_Leave(sender As Object, e As EventArgs) Handles Me.Leave
        RichTextBox1.Clear()
    End Sub
    Private Sub print_But_Click(sender As Object, e As EventArgs) Handles print_But.Click
        ' -------------------- LOAD setari

        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
        Dim folder_pdf As String = ""
        Try
            Dim set_sql As String = "SELECT * FROM setari WHERE setare='path_registru'"
            dbconn.Open()
            dbcomm = New MySqlCommand(set_sql, dbconn)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows = False Then
                SaveFileDialog1.Title = "Selectati Folderul pt Registru"
                SaveFileDialog1.FileName = "Selectati Folderul"
                SaveFileDialog1.Filter = "pdf Files (*.pdf*)|*.pdf"

                If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim folder As String = System.IO.Path.GetDirectoryName(SaveFileDialog1.FileName) & "\"
                    Dim dbconn As MySqlConnection = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")
                    Try
                        If dbconn.State = 0 Then
                            dbconn.Open()
                        End If

                        Dim set_path_sql As String = "REPLACE INTO setari(setare,valoare) VALUES('path_registru',@folder)"
                        Using dbcomm As MySqlCommand = New MySqlCommand(set_path_sql, dbconn)
                            dbcomm.Parameters.AddWithValue("@folder", folder)
                            dbcomm.ExecuteNonQuery()
                        End Using
                    Catch ex As Exception
                        MsgBox("Problem Nonquery: " & ex.Message.ToString)
                    End Try
                End If

            ElseIf dbread.HasRows = True Then
                folder_pdf = dbread("valoare")
            End If

        Catch ex As Exception
            MsgBox("Problem loading setari savefile: " & ex.Message.ToString)
        End Try
        dbread.Close()
        '--------------------------------

        Dim luna_reg As Date = Date.Parse(Date.DaysInMonth(ComboBox2.SelectedItem, ComboBox1.SelectedItem) & "." & ComboBox1.SelectedItem & "." & ComboBox2.SelectedItem)
        Dim pdf As PdfSharp.Pdf.PdfDocument = New PdfSharp.Pdf.PdfDocument


        Dim punct_lucru As String = ""
        If ComboBox3.SelectedValue = "PM" Then
            punct_lucru = "Magazin: Petru Maior nr. 9"
        ElseIf ComboBox3.SelectedValue = "MV" Then
            punct_lucru = "Magazin: Mihai Viteazu nr. 28"
        End If

        Dim mag As String = ComboBox3.SelectedValue
        pdf.Info.Title = "Registru Casa"
        Dim pdfPage As PdfSharp.Pdf.PdfPage = pdf.AddPage


        Dim graph As XGraphics = XGraphics.FromPdfPage(pdfPage)


        Dim titlu_font As XFont = New XFont("Verdana", 16, XFontStyle.Bold)
        Dim top_font As XFont = New XFont("Verdana", 12, XFontStyle.Bold)
        Dim report_font As XFont = New XFont("Verdana", 10, XFontStyle.Bold)
        Dim report_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim text_font As XFont = New XFont("Verdana", 10, XFontStyle.Regular)
        Dim text_italic_font As XFont = New XFont("Verdana", 10, XFontStyle.Italic)
        Dim bottom_font As XFont = New XFont("Verdana", 12, XFontStyle.Bold)

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

        graph.DrawString("REGISTRU DE CASA", titlu_font, XBrushes.Black, _
                         New XRect(20, 50, 575, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(XPens.Black, 20, 85, 575, 85)
        graph.DrawLine(XPens.Black, 20, 129, 575, 129)


        graph.DrawString("Nr", top_font, XBrushes.Black, _
                         New XRect(20, 92, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("crt.", top_font, XBrushes.Black, _
                         New XRect(20, 105, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("Nr act", top_font, XBrushes.Black, _
                         New XRect(70, 92, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("casa", top_font, XBrushes.Black, _
                         New XRect(70, 105, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("Nr anexa", top_font, XBrushes.Black, _
                         New XRect(120, 100, 50, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("Explicatii".ToString, top_font, XBrushes.Black, _
                         New XRect(190, 100, 300, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("Incasari".ToString, top_font, XBrushes.Black, _
                         New XRect(450, 100, 60, pdfPage.Height.Point), XStringFormats.TopCenter)
        graph.DrawString("Plati".ToString, top_font, XBrushes.Black, _
                         New XRect(520, 100, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
        '-----------------HEADER ---------------------------



        Dim sold_initial As Double = CDbl(TextBox1.Text)
        Dim sold_prec As Double = sold_initial
        Dim sold_zi As Double = 0

        Dim nxtRow As Double = 120
        Dim suma_inc As Double = 0
        Dim suma_che As Double = 0

        For d = 1 To Date.DaysInMonth(luna_reg.Year, luna_reg.Month)

            Dim data As String = Format(Date.Parse(d & "." & luna_reg.Month & "." & luna_reg.Year), "yyyy-MM-dd")

            Dim datatext As Date = data.ToString

            suma_inc = 0
            suma_che = 0
            Try
                Dim sql_chl As String = "SELECT * FROM cheltuieli WHERE data='" & data & "' AND magazin='" & mag & "' AND cash=TRUE"
                Dim chelt As Boolean = False
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(sql_chl, dbconn)
                dbread2 = dbcomm.ExecuteReader()
                If dbread2.HasRows = True Then
                    chelt = True
                End If
                dbread2.Close()
                Dim sql_inc As String = "SELECT * FROM incasari WHERE data='" & data & "' AND magazin='" & mag & "'"
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(sql_inc, dbconn)
                dbread = dbcomm.ExecuteReader()
                If dbread.HasRows = True Or chelt = True Then

                    nxtRow = nxtRow + 20

                    If nxtRow > 650 Then
                        '-----------------FOOTER ----------------------------
                        ' MsgBox("footer")
                        graph.DrawLine(XPens.Black, 20, 770, 575, 770)
                        graph.DrawString("Report pagina/total".ToString, top_font, XBrushes.Black, _
                                         New XRect(190, 775, 300, pdfPage.Height.Point), XStringFormats.TopCenter)


                        Dim tf2 As XTextFormatter = New XTextFormatter(graph)
                        tf2.Alignment = XParagraphAlignment.Right

                        tf2.DrawString(FormatNumber(CDec(incas_tot), 2, TriState.True, TriState.False, TriState.True), report_italic_font, XBrushes.Black, _
                                       New XRect(450, 775, 60, pdfPage.Height.Point), XStringFormats.TopLeft)

                        tf2.DrawString(FormatNumber(CDec(chelt_tot), 2, TriState.True, TriState.False, TriState.True), report_italic_font, XBrushes.Black, _
                                       New XRect(520, 775, 55, pdfPage.Height.Point), XStringFormats.TopLeft)

                        graph.DrawLine(XPens.Black, 20, 800, 575, 800)
                        graph.DrawString("Casier".ToString, text_font, XBrushes.Black, _
                                         New XRect(20, 805, 300, pdfPage.Height.Point), XStringFormats.TopLeft)
                        graph.DrawString("Compartiment Financiar - Contabil".ToString, text_font, XBrushes.Black, _
                                         New XRect(370, 805, pdfPage.Height.Point, pdfPage.Height.Point), XStringFormats.TopLeft)
                        '-----------------FOOTER ----------------------------

                        pdfPage = pdf.AddPage()
                        graph = XGraphics.FromPdfPage(pdfPage)

                        '-----------------HEADER ----------------------------
                        'MsgBox("header")

                        graph.DrawString("MILICOM CAZ SRL", report_font, XBrushes.Black, _
                                         New XRect(20, 20, 100, pdfPage.Height.Point), XStringFormats.TopLeft)
                        graph.DrawString("CIF: 31240216", text_font, XBrushes.Black, _
                                         New XRect(20, 35, 100, pdfPage.Height.Point), XStringFormats.TopLeft)
                        graph.DrawString(punct_lucru, report_font, XBrushes.Black, _
                                         New XRect(415, 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)

                        graph.DrawString("REGISTRU DE CASA", titlu_font, XBrushes.Black, _
                                         New XRect(20, 50, 575, pdfPage.Height.Point), XStringFormats.TopCenter)

                        graph.DrawLine(XPens.Black, 20, 85, 575, 85)
                        graph.DrawLine(XPens.Black, 20, 129, 575, 129)
                        graph.DrawString("Nr", top_font, XBrushes.Black, _
                                         New XRect(20, 92, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
                        graph.DrawString("crt.", top_font, XBrushes.Black, _
                                         New XRect(20, 105, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
                        graph.DrawString("Nr act", top_font, XBrushes.Black, _
                                         New XRect(70, 92, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
                        graph.DrawString("casa", top_font, XBrushes.Black, _
                                         New XRect(70, 105, 30, pdfPage.Height.Point), XStringFormats.TopCenter)
                        graph.DrawString("Nr anexa", top_font, XBrushes.Black, _
                                         New XRect(120, 100, 50, pdfPage.Height.Point), XStringFormats.TopLeft)
                        graph.DrawString("Explicatii".ToString, top_font, XBrushes.Black, _
                                         New XRect(190, 100, 300, pdfPage.Height.Point), XStringFormats.TopCenter)
                        graph.DrawString("Incasari".ToString, top_font, XBrushes.Black, _
                                         New XRect(450, 100, 60, pdfPage.Height.Point), XStringFormats.TopCenter)
                        graph.DrawString("Plati".ToString, top_font, XBrushes.Black, _
                                         New XRect(520, 100, 55, pdfPage.Height.Point), XStringFormats.TopCenter)
                        '-----------------HEADER ---------------------------

                        nxtRow = 140
                    End If

                    graph.DrawString("Report/Sold ziua Precedenta", report_font, XBrushes.Black, _
                                     New XRect(20, nxtRow, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)
                    graph.DrawString("Sold initial", report_font, XBrushes.Black, _
                                     New XRect(270, nxtRow, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)
                    graph.DrawString(datatext, report_font, XBrushes.Black, _
                                     New XRect(350, nxtRow, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft)
                    'graph.DrawString(sold_prec.ToString, report_font, XBrushes.Black, _
                    '                 New XRect(460, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)

                    Dim tf As XTextFormatter = New XTextFormatter(graph)
                    tf.Alignment = XParagraphAlignment.Right
                    tf.DrawString(FormatNumber(CDec(sold_prec), 2, TriState.True, TriState.False, TriState.True), report_font, XBrushes.Black, _
                                  New XRect(450, nxtRow, 60, pdfPage.Height.Point), XStringFormats.TopLeft)
                    Dim crt As Integer = 0

                    While dbread.Read
                        crt = crt + 1
                        suma_inc = suma_inc + CDbl(dbread("suma_cash"))
                        nxtRow = nxtRow + 15

                        incas_tot = incas_tot + CDbl(dbread("suma_cash"))
                        incas_tot = incas_tot
                        graph.DrawString((crt) & ".", text_font, XBrushes.Black, _
                                         New XRect(20, nxtRow, 30, pdfPage.Height.Point), XStringFormats.TopCenter)

                        graph.DrawString(dbread("tip_incasare"), text_font, XBrushes.Black, _
                                         New XRect(70, nxtRow, 30, pdfPage.Height.Point), XStringFormats.TopCenter)

                        graph.DrawString(dbread("nr_rzf"), text_font, XBrushes.Black, _
                                         New XRect(125, nxtRow, 45, pdfPage.Height.Point), XStringFormats.TopLeft)

                        graph.DrawString(dbread("explicatii"), text_font, XBrushes.Black, _
                                         New XRect(200, nxtRow, 240, pdfPage.Height.Point), XStringFormats.TopLeft)

                        Dim suma_incasari As String = FormatNumber(CDec(dbread("suma_cash")), 2, TriState.True, TriState.False, TriState.True)
                        tf.DrawString(suma_incasari, text_font, XBrushes.Black, New XRect(450, nxtRow, 60, pdfPage.Height.Point), XStringFormats.TopLeft)
                        tf.DrawString("-", text_font, XBrushes.Black, _
                                      New XRect(520, nxtRow, 55, pdfPage.Height.Point), XStringFormats.TopLeft)

                    End While

                    dbread.Close()

                    Dim sql_che As String = "SELECT * FROM cheltuieli WHERE data='" & data & "' AND magazin='" & mag & "' AND cash=TRUE"
                    dbcomm = New MySqlCommand(sql_che, dbconn)
                    dbread = dbcomm.ExecuteReader()

                    While dbread.Read
                        crt = crt + 1
                        suma_che = suma_che + CDbl(dbread("suma"))

                        nxtRow = nxtRow + 15

                        chelt_tot = chelt_tot + CDbl(dbread("suma"))
                        chelt_tot = chelt_tot

                        graph.DrawString((crt) & ".", text_font, XBrushes.Black, _
                                         New XRect(20, nxtRow, 30, pdfPage.Height.Point), XStringFormats.TopCenter)

                        graph.DrawString(dbread("tip_cheltuiala"), text_font, XBrushes.Black, _
                                         New XRect(70, nxtRow, 30, pdfPage.Height.Point), XStringFormats.TopCenter)

                        graph.DrawString(dbread("nr_chitanta"), text_font, XBrushes.Black, _
                                         New XRect(125, nxtRow, 45, pdfPage.Height.Point), XStringFormats.TopLeft)

                        graph.DrawString(dbread("explicatii"), text_font, XBrushes.Black, _
                                         New XRect(200, nxtRow, 240, pdfPage.Height.Point), XStringFormats.TopLeft)

                        '    graph.DrawString("-", text_font, XBrushes.Black, _
                        'New XRect(460, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
                        tf.DrawString("-", text_font, XBrushes.Black, _
                                      New XRect(450, nxtRow, 60, pdfPage.Height.Point), XStringFormats.TopLeft)

                        '    graph.DrawString(dbread("suma"), text_font, XBrushes.Black, _
                        'New XRect(530, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
                        Dim suma_cheltuiala As String = FormatNumber(CDec(dbread("suma")), 2, TriState.True, TriState.False, TriState.True)
                        tf.DrawString(suma_cheltuiala, text_font, XBrushes.Black, _
                                      New XRect(520, nxtRow, 55, pdfPage.Height.Point), XStringFormats.TopLeft)

                    End While

                    dbread.Close()

                    sold_zi = suma_inc - suma_che
                    sold_prec = CDec(sold_zi) + CDec(sold_prec)
                    nxtRow = nxtRow + 20

                    graph.DrawString("RULAJ ZI", report_font, XBrushes.Black, _
                                     New XRect(300, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopLeft)
                    'graph.DrawString(suma_inc, report_italic_font, XBrushes.Black, _
                    '                 New XRect(460, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
                    tf.DrawString(FormatNumber(CDec(suma_inc), 2, TriState.True, TriState.False, TriState.True), report_italic_font, XBrushes.Black, _
                                    New XRect(450, nxtRow, 60, pdfPage.Height.Point), XStringFormats.TopLeft)

                    'graph.DrawString(suma_che, report_italic_font, XBrushes.Black, _
                    '                 New XRect(530, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
                    tf.DrawString(FormatNumber(CDec(suma_che), 2, TriState.True, TriState.False, TriState.True), report_italic_font, XBrushes.Black, _
                                     New XRect(520, nxtRow, 55, pdfPage.Height.Point), XStringFormats.TopLeft)

                    nxtRow = nxtRow + 15
                    graph.DrawString("SOLD FINAL", report_font, XBrushes.Black, _
                                     New XRect(300, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopLeft)
                    'graph.DrawString(sold_prec, report_font, XBrushes.Black, _
                    '                 New XRect(460, nxtRow, 50, pdfPage.Height.Point), XStringFormats.TopCenter)
                    If sold_prec < 0 Then

                        With Form_DI

                            .Label1.Text = "Adauga DI in data de " & datatext & " cu suma de " & sold_prec * (-1) & ""

                            .DateTimePicker1.Value = datatext
                            .suma_Textbox.Text = Math.Ceiling(sold_prec * (-1)).ToString
                            .ShowDialog()
                        End With

                        Exit Sub
                    End If
                    tf.DrawString(FormatNumber(CDec(sold_prec), 2, TriState.True, TriState.False, TriState.True), report_font, XBrushes.Black, _
                                    New XRect(450, nxtRow, 60, pdfPage.Height.Point), XStringFormats.TopLeft)
                End If
                dbread.Close()
                dbconn.Close()
            Catch ex As Exception
                MsgBox("Problem loading data: " & ex.Message.ToString)
            End Try

        Next
        '-----------------FOOTER ----------------------------

        graph.DrawLine(XPens.Black, 20, 770, 575, 770)
        graph.DrawString("Report pagina/total".ToString, top_font, XBrushes.Black, _
                         New XRect(190, 775, 300, pdfPage.Height.Point), XStringFormats.TopCenter)




        Dim tf1 As XTextFormatter = New XTextFormatter(graph)
        tf1.Alignment = XParagraphAlignment.Right
        tf1.DrawString(FormatNumber(CDec(incas_tot), 2, TriState.True, TriState.False, TriState.True), report_italic_font, XBrushes.Black, _
                       New XRect(450, 775, 60, pdfPage.Height.Point), XStringFormats.TopLeft)

        tf1.DrawString(FormatNumber(CDec(chelt_tot), 2, TriState.True, TriState.False, TriState.True), report_italic_font, XBrushes.Black, _
                       New XRect(520, 775, 55, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawLine(XPens.Black, 20, 800, 575, 800)
        graph.DrawString("Casier".ToString, text_font, XBrushes.Black, _
                         New XRect(20, 805, 300, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("Compartiment Financiar - Contabil".ToString, text_font, XBrushes.Black, _
                         New XRect(370, 805, pdfPage.Height.Point, pdfPage.Height.Point), XStringFormats.TopLeft)
        '-----------------FOOTER ----------------------------
        dbread.Dispose()
        dbread2.Dispose()

        '------------solduri luna RAPORT
        If dbconn.State = ConnectionState.Closed Then
            dbconn.Open()
        End If

        dbcomm = New MySqlCommand("SELECT * FROM solduri_casa WHERE data=@data AND magazin='" & ComboBox3.SelectedValue & "'", dbconn)
        dbcomm.Parameters.AddWithValue("@data", luna_reg)


        Dim dbread3 As MySqlDataReader
        dbread3 = dbcomm.ExecuteReader()
        dbread3.Read()
        If dbread3.HasRows = True AndAlso dbread3("permanent") = True Then
            MsgBox("Atentie!!! Soldurile nu se pot modifica")
            dbread3.Close()
        ElseIf dbread3.HasRows = False Or (dbread3.HasRows = True AndAlso dbread3("permanent") = False) Then
            dbread3.Close()
            Dim sql_ins As String = "INSERT INTO solduri_casa(data,casa_sold_initial,incasari,cheltuieli,casa_sold_final,magazin) " _
                               & "VALUES(@data,@casa_sold_initial,@incasari,@cheltuieli,@casa_sold_final,@magazin) ON DUPLICATE KEY UPDATE casa_sold_initial=@casa_sold_initial,incasari=@incasari,cheltuieli=@cheltuieli,casa_sold_final=@casa_sold_final,magazin=@magazin"
            Dim data_actuala As Date = Format(Date.Parse(Date.DaysInMonth(luna_reg.Year, luna_reg.Month) & "." & luna_reg.Month & "." & luna_reg.Year), "yyyy-MM-dd")

            Try
                If dbconn.State = 0 Then
                    dbconn.Open()
                End If
                chelt_tot = FormatNumber(CDec(chelt_tot), 2, TriState.True, TriState.False, TriState.True)
                dbcomm = New MySqlCommand(sql_ins, dbconn)
                Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
                dbcomm.Parameters.AddWithValue("@data", data_actuala)
                dbcomm.Parameters.AddWithValue("@casa_sold_initial", sold_initial)
                dbcomm.Parameters.AddWithValue("@incasari", incas_tot)
                dbcomm.Parameters.AddWithValue("@cheltuieli", chelt_tot)
                dbcomm.Parameters.AddWithValue("@casa_sold_final", sold_prec)
                dbcomm.Parameters.AddWithValue("@magazin", mag)
                'MsgBox(chelt_tot & " " & FormatNumber(CDec(chelt_tot), 2, TriState.True, TriState.False, TriState.True))
                dbcomm.ExecuteNonQuery()
                transaction.Commit()
                dbconn.Close()
            Catch ex As Exception
                MsgBox("Failed to insert solduri precedente: " & ex.Message.ToString())
            End Try
            dbread.Close()
            dbread3.Close()
        End If
        Dim sql As String = "INSERT INTO solduri_casa (data,casa_sold_final,magazin) " _
                           & "VALUES(@data,@casa_sold_final,@magazin) ON DUPLICATE KEY UPDATE casa_sold_final=@casa_sold_final"
        luna_reg = Date.Parse(Date.DaysInMonth(ComboBox2.SelectedItem, ComboBox1.SelectedItem) & "." & ComboBox1.SelectedItem & "." & ComboBox2.SelectedItem)
        Dim luna_prec As Date = DateAdd(DateInterval.Month, -1, luna_reg)
        Dim data_precedenta As Date = Date.Parse(Date.DaysInMonth(luna_prec.Year, luna_prec.Month) & "." & luna_prec.Month & "." & luna_prec.Year)
        Try
            If dbconn.State = 0 Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql, dbconn)
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            dbcomm.Parameters.AddWithValue("@data", data_precedenta)
            dbcomm.Parameters.AddWithValue("@casa_sold_final", CDec(TextBox1.Text))
            dbcomm.Parameters.AddWithValue("@magazin", ComboBox3.SelectedValue)

            dbcomm.ExecuteNonQuery()
            transaction.Commit()
            dbconn.Close()
        Catch ex As Exception
            MsgBox("Failed to insert data in solduri_luna : " & ex.Message.ToString())
        End Try

        Form_principal.Load_Solduri()
        Dim mag_id As String = ""
        If mag = "PM" Then
            mag_id = "PM"
        ElseIf mag = "MV" Then
            mag_id = "MV"
        End If

        Dim pdfFilename As String = folder_pdf & "Registru Casa_" & luna_reg.Year & "_" & Format(luna_reg, "MM") & "_" & mag_id & ".pdf"
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



    End Sub


    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
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
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text = "" Then
            TextBox1.Text = 0
        End If
        If TextBox2.Text = "0" Then
            TextBox2.SelectAll()
        End If
    End Sub
    Private Sub TextBox1_Keyup(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyUp
        Timer1.Interval = 1000
        Timer1.Stop()
        Timer1.Start()
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Load_DGV()
    End Sub

    Private Sub ComboBox_1_2_3_DropDownClosed(sender As Object, e As EventArgs) Handles ComboBox1.DropDownClosed, ComboBox2.DropDownClosed, ComboBox3.DropDownClosed

        Label1.Text = MonthName(ComboBox1.SelectedItem) & " " & ComboBox2.SelectedItem
        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=me21s86;port=3306;database=magazin")

        Dim luna_reg As Date = Date.Parse(Date.DaysInMonth(ComboBox2.SelectedItem, ComboBox1.SelectedItem) & "." & ComboBox1.SelectedItem & "." & ComboBox2.SelectedItem)
        Dim luna_prec As Date = DateAdd(DateInterval.Month, -1, luna_reg)
        Dim data_ultima As Date = Date.Parse(Date.DaysInMonth(luna_prec.Year, luna_prec.Month) & "." & luna_prec.Month & "." & luna_prec.Year)

        Dim sql_read As String = "SELECT * FROM solduri_casa WHERE data=@data and magazin='" & ComboBox3.SelectedValue & "'"
        Try

            If dbconn.State = ConnectionState.Closed Then
                dbconn.Open()
            End If
            dbcomm = New MySqlCommand(sql_read, dbconn)
            dbcomm.Parameters.AddWithValue("@data", data_ultima)
            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows Then
                TextBox1.Text = (dbread("casa_sold_final").ToString)
            Else
                TextBox1.Text = 0
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
            dbcomm.Parameters.AddWithValue("@data", luna_reg)
            dbread2 = dbcomm.ExecuteReader()
            dbread2.Read()
            If dbread2.HasRows Then
                If dbread2("permanent") = True Then
                    TextBox1.Enabled = False
                    If TextBox1.Text = 0 Then
                        TextBox1.Text = (dbread2("casa_sold_initial").ToString)
                        'MsgBox((dbread2("gestiune_sold_initial").ToString))
                    End If
                ElseIf dbread2("permanent") = False Then
                    TextBox1.Enabled = True
                End If
            Else
                TextBox1.Enabled = True
            End If

        Catch ex As Exception
            MsgBox("Problem loading data raport: " & ex.Message.ToString)
            dbread2.Close()
        End Try
        'dbread2.Close()
        'TextBox2.Enabled = True
        'TextBox2.Text = 0
        TextBox1.Select()
        TextBox1.Focus()
        dbconn.Close()
        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
        End If
        'Load_Registru()
        Load_DGV()
    End Sub
   
    Private Sub Load_DGV()
        Dim DGV As DataGridView = DataGridView1

        DGV.DataSource = Nothing
        DGV.Rows.Clear()
        DGV.Columns.Clear()
        DGV.RowHeadersVisible = False
        DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter


        Dim Col_0 As New DataGridViewTextBoxColumn
        With Col_0
            .HeaderText = "Nr. Crt"
            .Name = "Col_0"
            .Width = DGV.Width * 10 / 100
            .ReadOnly = True
        End With

        Dim Col_1 As New DataGridViewTextBoxColumn
        With Col_1
            .HeaderText = "Nr. act casa"
            .Name = "Col_1"
            .Width = DGV.Width * 10 / 100
            .ReadOnly = True
        End With

        Dim Col_2 As New DataGridViewTextBoxColumn
        With Col_2
            .HeaderText = "Nr. anexa"
            .Name = "Col_2"
            .Width = DGV.Width * 10 / 100
            .ReadOnly = True
        End With
        Dim Col_3 As New DataGridViewTextBoxColumn
        With Col_3
            .HeaderText = "Explicatii"
            .Name = "Col_3"
            .Width = DGV.Width * 35 / 100
            .ReadOnly = True
        End With
        Dim Col_4 As New DataGridViewTextBoxColumn
        With Col_4
            .HeaderText = "Incasari"
            .Name = "Col_4"
            .Width = DGV.Width * 15 / 100
            .ReadOnly = True
        End With
        Dim Col_5 As New DataGridViewTextBoxColumn
        With Col_5
            .HeaderText = "Plati"
            .Name = "Col_5"
            .Width = DGV.Width * 15 / 100
            .ReadOnly = True
        End With

        DGV.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Col_0, Col_1, Col_2, Col_3, Col_4, Col_5})

        status_Lbl.Text = "Status Casa: OK"
        status_Lbl.ForeColor = Color.Green
        data_TB.Text = ""
        suma_minus_TB.Text = ""
        data_TB.Enabled = False
        suma_minus_TB.Enabled = False
        adauga_DI_But.Enabled = False

        Dim luna_reg As Date = Date.Parse(Date.DaysInMonth(ComboBox2.SelectedItem, ComboBox1.SelectedItem) & "." & ComboBox1.SelectedItem & "." & ComboBox2.SelectedItem)


        If DialogResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        Dim punct_lucru As String = ""
        If ComboBox3.SelectedValue = "PM" Then
            punct_lucru = "Petru Maior nr. 9"
        ElseIf ComboBox3.SelectedValue = "MV" Then
            punct_lucru = "Mihai Viteazu nr. 28"
        End If

        Dim mag As String = ComboBox3.SelectedValue

        Dim incas_tot As Double = 0
        Dim chelt_tot As Double = 0

        Dim row As Integer = 0
        Dim rand As DataGridViewRow = DataGridView1.Rows(row)

        RichTextBox1.Clear()
        RichTextBox1.Text = "REGISTRU CASA" & vbNewLine & "Magazin: " & punct_lucru & vbNewLine & Format(luna_reg, "MMMM yyyy")
        Dim reg_str As String = "REGISTRU CASA"
        Dim mag_str As String = "Magazin: " & punct_lucru
        Dim dat_str As String = Format(luna_reg, "MMMM yyyy")

        RichTextBox1.Select(0, Len(reg_str))
        RichTextBox1.SelectionFont = New Font("Arial", 16, FontStyle.Bold, GraphicsUnit.Point)

        RichTextBox1.Select(Len(reg_str) + 1, Len(mag_str))
        RichTextBox1.SelectionFont = New Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point)

        RichTextBox1.Select(Len(reg_str) + Len(mag_str) + 2, Len(dat_str))
        RichTextBox1.SelectionFont = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        RichTextBox1.SelectAll()
        RichTextBox1.SelectionAlignment = HorizontalAlignment.Center

        '-----------------HEADER ----------------------------

        '-----------------HEADER ---------------------------

        Dim sold_initial As Double = CDbl(TextBox1.Text)
        Dim sold_prec As Double = sold_initial
        Dim sold_zi As Double = 0

        Dim nxtRow As Double = 120
        Dim suma_inc As Double = 0
        Dim suma_che As Double = 0

        For d = 1 To Date.DaysInMonth(luna_reg.Year, luna_reg.Month)

            Dim data As String = Format(Date.Parse(d & "." & luna_reg.Month & "." & luna_reg.Year), "yyyy-MM-dd")

            Dim datatext As String = Format(Date.Parse(data), "dd.MM.yyy")

            suma_inc = 0
            suma_che = 0
            Try
                Dim sql_chl As String = "SELECT * FROM cheltuieli WHERE data='" & data & "' AND magazin='" & mag & "'"
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If

                Dim chelt As Boolean = False
                dbcomm = New MySqlCommand(sql_chl, dbconn)
                dbread2 = dbcomm.ExecuteReader()
                If dbread2.HasRows = True Then
                    chelt = True
                End If
                dbread2.Close()

                Dim sql_inc As String = "SELECT * FROM incasari WHERE data='" & data & "' AND magazin='" & mag & "'"
                If dbconn.State = ConnectionState.Closed Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(sql_inc, dbconn)
                dbread = dbcomm.ExecuteReader()
                If dbread.HasRows = True Or chelt = True Then

                    DGV.Rows.Add()
                    DataGridView1.Rows(row).Cells(0).Value = "Report/"
                    DataGridView1.Rows(row).Cells(1).Value = "Sold ziua "
                    DataGridView1.Rows(row).Cells(2).Value = "precedenta"
                    DataGridView1.Rows(row).Cells(3).Value = datatext '& " Sold initial"
                    DataGridView1.Rows(row).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    DataGridView1.Rows(row).Cells(4).Value = FormatNumber(CDec(sold_prec), 2, TriState.True, TriState.False, TriState.True)
                    DataGridView1.Rows(row).Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    DataGridView1.Rows(row).DefaultCellStyle.Font = New Font(DGV.Font, FontStyle.Bold)
                    DataGridView1.Rows(row).DefaultCellStyle.BackColor = Color.PaleTurquoise
                    row = row + 1

                    Dim crt As Integer = 0

                    While dbread.Read
                        crt = crt + 1
                        suma_inc = suma_inc + CDbl(dbread("suma_cash"))

                        incas_tot = incas_tot + CDbl(dbread("suma_cash"))
                        incas_tot = incas_tot

                        Dim suma_incasari As String = FormatNumber(CDec(dbread("suma_cash")), 2, TriState.True, TriState.False, TriState.True)

                        DGV.Rows.Add()
                        DataGridView1.Rows(row).Cells(0).Value = crt & "."
                        DataGridView1.Rows(row).Cells(1).Value = dbread("tip_incasare")
                        DataGridView1.Rows(row).Cells(2).Value = dbread("nr_rzf")
                        DataGridView1.Rows(row).Cells(3).Value = dbread("explicatii")
                        DataGridView1.Rows(row).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                        DataGridView1.Rows(row).Cells(4).Value = suma_incasari
                        DataGridView1.Rows(row).Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        DataGridView1.Rows(row).Cells(5).Value = "-"
                        DataGridView1.Rows(row).Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        row = row + 1
                    End While
                    dbread.Close()

                    Dim sql_che As String = "SELECT * FROM cheltuieli WHERE data='" & data & "' AND magazin='" & mag & "' AND cash=TRUE"
                    dbcomm = New MySqlCommand(sql_che, dbconn)
                    dbread = dbcomm.ExecuteReader()

                    While dbread.Read
                        crt = crt + 1
                        suma_che = suma_che + CDbl(dbread("suma"))
                        chelt_tot = chelt_tot + CDbl(dbread("suma"))
                        chelt_tot = chelt_tot

                        Dim suma_cheltuiala As String = FormatNumber(CDec(dbread("suma")), 2, TriState.True, TriState.False, TriState.True)

                        DGV.Rows.Add()
                        DataGridView1.Rows(row).Cells(0).Value = crt & "."
                        DataGridView1.Rows(row).Cells(1).Value = dbread("tip_cheltuiala")
                        DataGridView1.Rows(row).Cells(2).Value = dbread("nr_chitanta")
                        DataGridView1.Rows(row).Cells(3).Value = dbread("explicatii")
                        DataGridView1.Rows(row).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                        DataGridView1.Rows(row).Cells(4).Value = "-"
                        DataGridView1.Rows(row).Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        DataGridView1.Rows(row).Cells(5).Value = suma_cheltuiala
                        DataGridView1.Rows(row).Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        row = row + 1
                    End While
                    dbread.Close()

                    sold_zi = suma_inc - suma_che
                    sold_prec = CDec(sold_zi) + CDec(sold_prec)
                    nxtRow = nxtRow + 20

                    DGV.Rows.Add()
                    DataGridView1.Rows(row).Cells(3).Value = "RULAJ ZI"
                    DataGridView1.Rows(row).Cells(4).Value = FormatNumber(CDec(suma_inc), 2, TriState.True, TriState.False, TriState.True)
                    DataGridView1.Rows(row).Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    DataGridView1.Rows(row).Cells(5).Value = FormatNumber(CDec(suma_che), 2, TriState.True, TriState.False, TriState.True)
                    DataGridView1.Rows(row).Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    DataGridView1.Rows(row).Cells(3).Style.Font = New Font(DGV.Font, FontStyle.Bold)
                    row = row + 1

                    DGV.Rows.Add()
                    DataGridView1.Rows(row).Cells(3).Value = "SOLD FINAL"
                    DataGridView1.Rows(row).Cells(4).Value = FormatNumber(CDec(sold_prec), 2, TriState.True, TriState.False, TriState.True)
                    DataGridView1.Rows(row).Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    DataGridView1.Rows(row).DefaultCellStyle.Font = New Font(DGV.Font, FontStyle.Bold)
                    row = row + 1

                    DGV.Rows.Add()
                    row = row + 1
                End If
                dbread.Close()
                dbconn.Close()
            Catch ex As Exception
                MsgBox("Problem loading data: " & ex.Message.ToString)
            End Try

        Next
        '-----------------FOOTER ----------------------------

        '-----------------FOOTER ----------------------------
        TextBox3.Text = FormatNumber(CDec(incas_tot), 2, TriState.True, TriState.False, TriState.True)
        TextBox3.TextAlign = HorizontalAlignment.Right
        TextBox3.Font = New Font(TextBox3.Font, FontStyle.Bold)
        TextBox2.Text = FormatNumber(CDec(chelt_tot), 2, TriState.True, TriState.False, TriState.True)
        TextBox2.TextAlign = HorizontalAlignment.Right
        TextBox2.Font = New Font(TextBox2.Font, FontStyle.Bold)
        TextBox4.Text = FormatNumber(CDec(sold_prec), 2, TriState.True, TriState.False, TriState.True)
        TextBox4.TextAlign = HorizontalAlignment.Right
        TextBox4.Font = New Font(TextBox4.Font, FontStyle.Bold)


        Dim ind_data As Integer = 0
        For i = 0 To DGV.Rows.Count - 1
            If IsDate(DGV.Rows(i).Cells(3).Value) Then
                ind_data = i
            End If
            If IsNumeric(DGV.Rows(i).Cells(4).Value) AndAlso CDec(DGV.Rows(i).Cells(4).Value) < 0 Then 'And status_Lbl.Text = "Status Casa: OK"
                status_Lbl.Text = "Atentie!!!! Casa pe minus. Data: "
                status_Lbl.ForeColor = Color.Red
                data_TB.Text = DGV.Rows(ind_data).Cells(3).Value
                data_TB.ForeColor = Color.Red
                suma_minus_TB.Text = CDec(DGV.Rows(i).Cells(4).Value)
                data_TB.Enabled = True
                suma_minus_TB.Enabled = True
                adauga_DI_But.Enabled = True

                DGV.ClearSelection()
                DGV.FirstDisplayedScrollingRowIndex = ind_data
                DGV.Rows(i).DefaultCellStyle.BackColor = Color.Red
                Exit For
            End If
        Next
    End Sub
    Private Sub adauga_DI_But_Click(sender As Object, e As EventArgs) Handles adauga_DI_But.Click

        With Form_DI

            .Label1.Text = "Adauga DI in data de " & Date.Parse(data_TB.Text) & " cu suma de " & CDec(suma_minus_TB.Text) * (-1) & ""

            .DateTimePicker1.Value = Date.Parse(data_TB.Text)
            .suma_Textbox.Text = Math.Ceiling(CDec(suma_minus_TB.Text) * (-1)).ToString
            .ComboBox3.SelectedValue = ComboBox3.SelectedValue
            .ShowDialog()
        End With
    End Sub

    Private Sub refresh_BU_Click(sender As Object, e As EventArgs) Handles refresh_BU.Click
        'Load_Registru()
        Load_DGV()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Form_cheltuieli.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form_Incasari.Show()
    End Sub

    Private Sub exit_But_Click(sender As Object, e As EventArgs) Handles exit_But.Click
        Me.Close()
    End Sub

End Class