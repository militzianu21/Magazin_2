Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization
Imports System.IO

Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing.Layout

Public Class Form_DI
    Public dbconn As New MySqlConnection
    Public dbcomm As New MySqlCommand
    Public dbread As MySqlDataReader
    Public AppDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
    Public folder_di As String
    Sub Nonquery(ByVal sql As String)
        If dbconn.State = 0 Then
            dbconn.Open()
        End If
        Try
            dbcomm = New MySqlCommand(sql, dbconn)
            dbcomm.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox("Problem loading data functie Nonquery: " & ex.Message.ToString)
        End Try
    End Sub
    Private Sub Form_DI_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Application.CurrentCulture = New CultureInfo("ro-RO")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("ro-RO")
        '-------------------
        ComboBox3.DisplayMember = "Text"
        ComboBox3.ValueMember = "Value"
        Dim tb As New DataTable
        tb.Columns.Add("Text", GetType(String))
        tb.Columns.Add("Value", GetType(String))
        tb.Rows.Add("1. Petru Maior", "PM")
        tb.Rows.Add("2. Mihai Viteazu", "MV")
        ComboBox3.DataSource = tb


        dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")

        ComboBox1.Items.AddRange({"DI"})
        ComboBox1.SelectedItem = "DI"
        explicatii_Textbox.Text = "Creditare Societate"


        Dim sql_tot As String = "SELECT * FROM incasari ORDER BY data DESC"
        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource
        Try
            dbconn.Open()
            dbcomm = New MySqlCommand(sql_tot, dbconn)

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

        Dim sql_read As String = "SELECT nr_rzf,data FROM incasari where tip_incasare='DI' ORDER BY DATA DESC LIMIT 1"
        Try
            dbconn.Open()
            dbcomm = New MySqlCommand(sql_read, dbconn)

            dbread = dbcomm.ExecuteReader()
            dbread.Read()
            If dbread.HasRows Then
                nr_rzf_Textbox.Text = (dbread("nr_rzf").ToString) + 1
                data_Label.Text = StrConv(Format(DateTimePicker1.Value, "dddd"), vbProperCase)
            End If

        Catch ex As Exception
            MsgBox("Problem loading data: " & ex.Message.ToString)

        End Try
        dbread.Close()
        dbconn.Close()
        ComboBox3.SelectedIndex = Form_Registru.ComboBox3.SelectedIndex
        'suma_Textbox.Text = "0"
        With suma_Textbox
            .Focus()
            .Select()
        End With
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

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

        If tip_incasare = "RZF" Then
            nr_rzf = "Z " & nr_rzf_Textbox.Text
            explicatii_Textbox.Text = "Incasari vanzari"
        End If
        If tip_incasare = "DI" Then
            explicatii_Textbox.Text = "Creditare Societate"
        End If

        Dim sql As String = "INSERT INTO incasari(data,tip_incasare,nr_rzf,explicatii,suma_cash,magazin) " _
                            & "VALUES(@data,@tip_incasare,@nr_rzf,@explicatii,@suma_cash,@magazin)"
        Try
            dbconn.Open()
            dbcomm = New MySqlCommand(sql, dbconn)
            Dim transaction As MySqlTransaction = dbconn.BeginTransaction()
            dbcomm.Parameters.AddWithValue("@data", DateTimePicker1.Value.Date)
            dbcomm.Parameters.AddWithValue("@tip_incasare", tip_incasare)
            dbcomm.Parameters.AddWithValue("@nr_rzf", nr_rzf)
            dbcomm.Parameters.AddWithValue("@explicatii", explicatii_Textbox.Text)
            dbcomm.Parameters.AddWithValue("@suma_cash", suma)
            dbcomm.Parameters.AddWithValue("@magazin", ComboBox3.SelectedValue)
            dbcomm.ExecuteNonQuery()
            transaction.Commit()
        Catch ex As Exception
            MsgBox("Failed to insert data: " & ex.Message.ToString())
        End Try
        dbconn.Close()

        Dim sql_read As String = "SELECT * FROM incasari ORDER BY data DESC"
        Dim sda As New MySqlDataAdapter
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource
        Try
            dbconn.Open()
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

        nr_rzf_Textbox.Text = nr_rzf_Textbox.Text + 1
        suma_Textbox.Text = 0
        suma_Textbox.Select()
        suma_Textbox.Focus()

        'Form_principal.Registru_Button.PerformClick()
    End Sub
    Private Sub TextBox_Suma_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles suma_Textbox.KeyPress
        If Not Char.IsDigit(e.KeyChar) Then e.Handled = True
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = "." Or e.KeyChar = ",") And (sender.Text.IndexOf(".") = -1 And sender.Text.IndexOf(",") = -1)))
        If Asc(e.KeyChar) = 44 Then
            e.KeyChar = ChrW(46)
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedValueChanged
        Dim tip_incasare As String = ComboBox1.SelectedItem

        Dim nr_rzf As String = nr_rzf_Textbox.Text
        Dim explicatii As String = explicatii_Textbox.Text
        Dim suma As String = suma_Textbox.Text

        If tip_incasare = "DI" Then
            Dim sql_read As String = "SELECT nr_rzf,data FROM incasari where tip_incasare='DI' ORDER BY data DESC LIMIT 1"
            Try
                dbconn.Open()
                dbcomm = New MySqlCommand(sql_read, dbconn)
                dbread = dbcomm.ExecuteReader()
                dbread.Read()
                If dbread.HasRows Then
                    nr_rzf_Textbox.Text = dbread("nr_rzf").ToString + 1
                    DateTimePicker1.Select()
                    DateTimePicker1.Focus()
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
            explicatii_Textbox.Text = "Creditare Societate"
        End If

    End Sub
    Private Sub StergeInregistrareaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StergeInregistrareaToolStripMenuItem.Click

        Dim yesno As Integer = MsgBox("Stergi Inregistrarea?", MsgBoxStyle.YesNo)
        Dim row As DataGridViewRow = DataGridView1.CurrentRow
        If yesno = DialogResult.No Then
        ElseIf yesno = DialogResult.Yes Then
            Dim id As Integer = row.Cells("id").Value
            Dim data As String = Format(CDate(row.Cells("data").Value), "yyyy-MM-dd")
            Dim nr_rzf As String = row.Cells("nr_rzf").Value.ToString
            Dim tip_incasare As String = row.Cells("tip_incasare").Value.ToString
            Dim explicatii As String = row.Cells("explicatii").Value
            Dim suma As String = row.Cells("suma_cash").Value

            Try
                Dim sql_del As String = "DELETE FROM incasari WHERE id='" & id & "' AND data='" & data & "' AND nr_rzf='" & nr_rzf & "' AND tip_incasare='" & tip_incasare & "'"
                explicatii_Textbox.Text = sql_del
                Dim dbconn As New MySqlConnection
                Dim dbcomm As New MySqlCommand
                dbconn = New MySqlConnection("Data Source=localhost;user id=root;password=*******;port=3306;database=magazin")

                If dbconn.State = 0 Then
                    dbconn.Open()
                End If
                dbcomm = New MySqlCommand(sql_del, dbconn)
                dbcomm.ExecuteNonQuery()
                dbconn.Close()
            Catch ex As Exception
                MsgBox("Nu s-a sters: " & ex.Message.ToString)
            End Try
            explicatii_Textbox.Text = AppDataFolder & "\Magazin\magazin.s3db"
            Dim sql_read As String = "SELECT * FROM incasari ORDER BY data DESC"
            Dim sda As New MySqlDataAdapter
            Dim dbdataset As New DataTable
            Dim bsource As New BindingSource
            Try
                dbconn.Open()
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
    Private Sub PrinteazaDIToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrinteazaDIToolStripMenuItem.Click

        'Dim yesno As Integer = MsgBox("Printezi Dispozitia de incasare?", MsgBoxStyle.YesNo)
        'If yesno = DialogResult.No Then
        'ElseIf yesno = DialogResult.Yes Then
        Dim row As DataGridViewRow = DataGridView1.CurrentRow

        Dim data As String = row.Cells("data").Value
        Dim nr_rzf As String = row.Cells("nr_rzf").Value.ToString
        Dim tip_incasare As String = row.Cells("tip_incasare").Value.ToString
        Dim explicatii As String = row.Cells("explicatii").Value
        Dim suma As String = row.Cells("suma_cash").Value

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

        graph.DrawString("Unitatea: MILICOM CAZ SRL", text_font, XBrushes.Black, _
                         New XRect(45, 20, 100, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawLine(pen, 40, 35, 555, 35)

        graph.DrawString("DISPOZITIE DE INCASARE CATRE CASIERIE", text_font, XBrushes.Black, _
                         New XRect(45, 37, 300, pdfPage.Height.Point), XStringFormats.TopCenter)

        graph.DrawLine(pen, 40, 52, 555, 52)

        graph.DrawLine(pen, 40, 35, 40, 232)
        graph.DrawLine(pen, 555, 35, 555, 232)


        graph.DrawString("Nr. " & nr_rzf & " din " & data, text_font, XBrushes.Black, _
                         New XRect(45, 67, 300, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Numele si prenumele:", text_font, XBrushes.Black, _
                         New XRect(45, 97, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("CAZAN MIHAI", text_font, XBrushes.Black, _
                         New XRect(195, 97, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Functia (calitatea):", text_font, XBrushes.Black, _
                         New XRect(45, 112, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("ADMINISTRATOR", text_font, XBrushes.Black, _
                         New XRect(195, 112, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Suma:", text_font, XBrushes.Black, _
                         New XRect(45, 127, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(suma & " lei (" & NrCuv(suma) & " lei)", text_font, XBrushes.Black, _
                         New XRect(195, 127, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Scopul incasarii:", text_font, XBrushes.Black, _
                         New XRect(45, 142, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("CREDITARE SOCIETATE", text_font, XBrushes.Black, _
                         New XRect(195, 142, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawRectangle(pen, New XRect(40, 172, 75, 60))
        graph.DrawString("Semnatura", mic_font, XBrushes.Black, _
                         New XRect(40, 172, 75, 80), XStringFormats.TopCenter)

        graph.DrawRectangle(pen, New XRect(115, 172, 146, 20))
        graph.DrawString("Conducatorul unitatii", mic_font, XBrushes.Black, _
                         New XRect(117, 172, 146, 20), XStringFormats.Center)
        graph.DrawRectangle(pen, New XRect(115, 192, 146, 40))

        graph.DrawRectangle(pen, New XRect(261, 172, 146, 20))
        graph.DrawString("Viza de control financiar-preventiv", mic_font, XBrushes.Black, _
                         New XRect(261, 172, 146, 20), XStringFormats.Center)
        graph.DrawRectangle(pen, New XRect(261, 192, 146, 40))

        graph.DrawRectangle(pen, New XRect(407, 172, 148, 20))
        graph.DrawString("Compartiment financiar-contabil", mic_font, XBrushes.Black, _
                         New XRect(409, 172, 146, 20), XStringFormats.Center)
        graph.DrawRectangle(pen, New XRect(407, 192, 148, 40))


        graph.DrawLine(pen, 40, 247, 555, 247)


        graph.DrawString("CASIER,", text_font, XBrushes.Black, _
                         New XRect(45, 249, 300, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("Incasat suma de", text_font, XBrushes.Black, _
                        New XRect(45, 264, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(suma & " lei", text_font, XBrushes.Black, _
                         New XRect(195, 264, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawString("Data:", text_font, XBrushes.Black, _
                         New XRect(45, 279, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString(Format(CDate(row.Cells("data").Value), "dd MMMM yyyy"), text_font, XBrushes.Black, _
                         New XRect(195, 279, 150, pdfPage.Height.Point), XStringFormats.TopLeft)
        graph.DrawString("Semnatura:", text_font, XBrushes.Black, _
                        New XRect(315, 279, 150, pdfPage.Height.Point), XStringFormats.TopLeft)

        graph.DrawLine(pen, 40, 294, 555, 294)
        graph.DrawLine(pen, 40, 247, 40, 294)
        graph.DrawLine(pen, 555, 247, 555, 294)


        Dim pdfFilename As String = folder_di & "Dispozitie Incasare_" & nr_rzf & "_" & Format(CDate(data), "yyyyMMdd") & ".pdf"
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
        'End If
    End Sub
    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        Dim row As DataGridViewRow = DataGridView1.CurrentRow
        Dim tip_incasare As String = row.Cells("tip_incasare").Value.ToString
        If tip_incasare = "DI" Then
            PrinteazaDIToolStripMenuItem.Enabled = True
        Else : PrinteazaDIToolStripMenuItem.Enabled = False
        End If
    End Sub
    Private Sub ComboBox3_DropDownClosed(sender As Object, e As EventArgs) Handles ComboBox3.DropDownClosed

        If ComboBox3.SelectedValue = "PM" Then
            PictureBox1.BackColor = Color.OrangeRed
        ElseIf ComboBox3.SelectedValue = "MV" Then
            PictureBox1.BackColor = Color.Turquoise
        Else : PictureBox1.BackColor = Color.LightYellow
        End If
    End Sub
End Class
