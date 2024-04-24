<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_cheltuieli
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.nr_rzf_Textbox = New System.Windows.Forms.TextBox()
        Me.explicatii_Textbox = New System.Windows.Forms.TextBox()
        Me.suma_Textbox = New System.Windows.Forms.TextBox()
        Me.save_But = New System.Windows.Forms.Button()
        Me.data_Label = New System.Windows.Forms.Label()
        Me.tip_Label = New System.Windows.Forms.Label()
        Me.nr_Label = New System.Windows.Forms.Label()
        Me.explicatii_Label = New System.Windows.Forms.Label()
        Me.suma_Label = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.TransferaCheltuialaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModificaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.PrinteazaDPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.StergeInregistrareaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.ad_firma_But = New System.Windows.Forms.Button()
        Me.ComboBox3 = New System.Windows.Forms.ComboBox()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.cash_RB = New System.Windows.Forms.RadioButton()
        Me.cont_RB = New System.Windows.Forms.RadioButton()
        Me.edit_Lbl = New System.Windows.Forms.Label()
        Me.id_Lbl = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.renunt_edit_But = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.incas_BUT = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker1.Location = New System.Drawing.Point(12, 66)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(123, 20)
        Me.DateTimePicker1.TabIndex = 0
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(12, 104)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(123, 21)
        Me.ComboBox1.TabIndex = 1
        '
        'nr_rzf_Textbox
        '
        Me.nr_rzf_Textbox.Location = New System.Drawing.Point(12, 144)
        Me.nr_rzf_Textbox.Name = "nr_rzf_Textbox"
        Me.nr_rzf_Textbox.Size = New System.Drawing.Size(123, 20)
        Me.nr_rzf_Textbox.TabIndex = 2
        '
        'explicatii_Textbox
        '
        Me.explicatii_Textbox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.explicatii_Textbox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.explicatii_Textbox.Location = New System.Drawing.Point(12, 204)
        Me.explicatii_Textbox.Name = "explicatii_Textbox"
        Me.explicatii_Textbox.Size = New System.Drawing.Size(235, 20)
        Me.explicatii_Textbox.TabIndex = 3
        '
        'suma_Textbox
        '
        Me.suma_Textbox.Location = New System.Drawing.Point(12, 250)
        Me.suma_Textbox.Name = "suma_Textbox"
        Me.suma_Textbox.Size = New System.Drawing.Size(123, 20)
        Me.suma_Textbox.TabIndex = 4
        '
        'save_But
        '
        Me.save_But.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.save_But.Location = New System.Drawing.Point(12, 288)
        Me.save_But.Name = "save_But"
        Me.save_But.Size = New System.Drawing.Size(123, 23)
        Me.save_But.TabIndex = 5
        Me.save_But.Text = "Salveaza"
        Me.save_But.UseVisualStyleBackColor = True
        '
        'data_Label
        '
        Me.data_Label.Location = New System.Drawing.Point(141, 66)
        Me.data_Label.Name = "data_Label"
        Me.data_Label.Size = New System.Drawing.Size(106, 20)
        Me.data_Label.TabIndex = 6
        Me.data_Label.Text = "Data"
        Me.data_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tip_Label
        '
        Me.tip_Label.Location = New System.Drawing.Point(163, 104)
        Me.tip_Label.Name = "tip_Label"
        Me.tip_Label.Size = New System.Drawing.Size(84, 21)
        Me.tip_Label.TabIndex = 7
        Me.tip_Label.Text = "Tip"
        Me.tip_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nr_Label
        '
        Me.nr_Label.Location = New System.Drawing.Point(141, 144)
        Me.nr_Label.Name = "nr_Label"
        Me.nr_Label.Size = New System.Drawing.Size(106, 20)
        Me.nr_Label.TabIndex = 8
        Me.nr_Label.Text = "Nr. CH"
        Me.nr_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'explicatii_Label
        '
        Me.explicatii_Label.Location = New System.Drawing.Point(9, 180)
        Me.explicatii_Label.Name = "explicatii_Label"
        Me.explicatii_Label.Size = New System.Drawing.Size(126, 21)
        Me.explicatii_Label.TabIndex = 9
        Me.explicatii_Label.Text = "Firma"
        Me.explicatii_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'suma_Label
        '
        Me.suma_Label.Location = New System.Drawing.Point(12, 227)
        Me.suma_Label.Name = "suma_Label"
        Me.suma_Label.Size = New System.Drawing.Size(72, 20)
        Me.suma_Label.TabIndex = 10
        Me.suma_Label.Text = "Suma"
        Me.suma_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TransferaCheltuialaToolStripMenuItem, Me.ModificaToolStripMenuItem, Me.ToolStripSeparator1, Me.PrinteazaDPToolStripMenuItem, Me.ToolStripSeparator2, Me.StergeInregistrareaToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(180, 104)
        '
        'TransferaCheltuialaToolStripMenuItem
        '
        Me.TransferaCheltuialaToolStripMenuItem.Image = Global.Magazin.My.Resources.Resources.A26_CurvedArrow_Orange
        Me.TransferaCheltuialaToolStripMenuItem.Name = "TransferaCheltuialaToolStripMenuItem"
        Me.TransferaCheltuialaToolStripMenuItem.Size = New System.Drawing.Size(179, 22)
        Me.TransferaCheltuialaToolStripMenuItem.Text = "Transfera Cheltuiala"
        '
        'ModificaToolStripMenuItem
        '
        Me.ModificaToolStripMenuItem.Image = Global.Magazin.My.Resources.Resources.edit_query
        Me.ModificaToolStripMenuItem.Name = "ModificaToolStripMenuItem"
        Me.ModificaToolStripMenuItem.Size = New System.Drawing.Size(179, 22)
        Me.ModificaToolStripMenuItem.Text = "Modifica"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(176, 6)
        '
        'PrinteazaDPToolStripMenuItem
        '
        Me.PrinteazaDPToolStripMenuItem.Image = Global.Magazin.My.Resources.Resources.pdf
        Me.PrinteazaDPToolStripMenuItem.Name = "PrinteazaDPToolStripMenuItem"
        Me.PrinteazaDPToolStripMenuItem.Size = New System.Drawing.Size(179, 22)
        Me.PrinteazaDPToolStripMenuItem.Text = "Printeaza DP"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(176, 6)
        '
        'StergeInregistrareaToolStripMenuItem
        '
        Me.StergeInregistrareaToolStripMenuItem.Image = Global.Magazin.My.Resources.Resources.cancel
        Me.StergeInregistrareaToolStripMenuItem.Name = "StergeInregistrareaToolStripMenuItem"
        Me.StergeInregistrareaToolStripMenuItem.Size = New System.Drawing.Size(179, 22)
        Me.StergeInregistrareaToolStripMenuItem.Text = "Sterge Inregistrarea"
        '
        'DataGridView1
        '
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.DataGridView1.Location = New System.Drawing.Point(253, 41)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersWidth = 5
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(471, 273)
        Me.DataGridView1.TabIndex = 13
        '
        'ad_firma_But
        '
        Me.ad_firma_But.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ad_firma_But.Location = New System.Drawing.Point(127, 180)
        Me.ad_firma_But.Name = "ad_firma_But"
        Me.ad_firma_But.Size = New System.Drawing.Size(120, 23)
        Me.ad_firma_But.TabIndex = 14
        Me.ad_firma_But.Text = "Adauga Firma (Ctrl+A)"
        Me.ad_firma_But.UseVisualStyleBackColor = True
        '
        'ComboBox3
        '
        Me.ComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ComboBox3.FormattingEnabled = True
        Me.ComboBox3.Location = New System.Drawing.Point(12, 11)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(235, 23)
        Me.ComboBox3.TabIndex = 21
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(253, 18)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(102, 17)
        Me.CheckBox2.TabIndex = 30
        Me.CheckBox2.Text = "Luna Curenta"
        Me.CheckBox2.UseVisualStyleBackColor = False
        '
        'cash_RB
        '
        Me.cash_RB.AutoSize = True
        Me.cash_RB.Location = New System.Drawing.Point(154, 250)
        Me.cash_RB.Name = "cash_RB"
        Me.cash_RB.Size = New System.Drawing.Size(53, 17)
        Me.cash_RB.TabIndex = 31
        Me.cash_RB.TabStop = True
        Me.cash_RB.Text = "Cash"
        Me.cash_RB.UseVisualStyleBackColor = True
        '
        'cont_RB
        '
        Me.cont_RB.AutoSize = True
        Me.cont_RB.Location = New System.Drawing.Point(154, 273)
        Me.cont_RB.Name = "cont_RB"
        Me.cont_RB.Size = New System.Drawing.Size(51, 17)
        Me.cont_RB.TabIndex = 32
        Me.cont_RB.TabStop = True
        Me.cont_RB.Text = "Cont"
        Me.cont_RB.UseVisualStyleBackColor = True
        '
        'edit_Lbl
        '
        Me.edit_Lbl.AutoSize = True
        Me.edit_Lbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.edit_Lbl.ForeColor = System.Drawing.Color.Red
        Me.edit_Lbl.Location = New System.Drawing.Point(158, 8)
        Me.edit_Lbl.Name = "edit_Lbl"
        Me.edit_Lbl.Size = New System.Drawing.Size(46, 24)
        Me.edit_Lbl.TabIndex = 33
        Me.edit_Lbl.Text = "Edit"
        '
        'id_Lbl
        '
        Me.id_Lbl.AutoSize = True
        Me.id_Lbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.id_Lbl.Location = New System.Drawing.Point(6, 14)
        Me.id_Lbl.Name = "id_Lbl"
        Me.id_Lbl.Size = New System.Drawing.Size(17, 13)
        Me.id_Lbl.TabIndex = 34
        Me.id_Lbl.Text = "id"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.edit_Lbl)
        Me.GroupBox1.Controls.Add(Me.renunt_edit_But)
        Me.GroupBox1.Controls.Add(Me.id_Lbl)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(464, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(260, 35)
        Me.GroupBox1.TabIndex = 36
        Me.GroupBox1.TabStop = False
        '
        'renunt_edit_But
        '
        Me.renunt_edit_But.Image = Global.Magazin.My.Resources.Resources.cancel
        Me.renunt_edit_But.Location = New System.Drawing.Point(228, 10)
        Me.renunt_edit_But.Name = "renunt_edit_But"
        Me.renunt_edit_But.Size = New System.Drawing.Size(26, 23)
        Me.renunt_edit_But.TabIndex = 35
        Me.renunt_edit_But.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Label1.Location = New System.Drawing.Point(250, 317)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 13)
        Me.Label1.TabIndex = 37
        Me.Label1.Text = "Total"
        '
        'incas_BUT
        '
        Me.incas_BUT.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.incas_BUT.Location = New System.Drawing.Point(649, 320)
        Me.incas_BUT.Name = "incas_BUT"
        Me.incas_BUT.Size = New System.Drawing.Size(75, 23)
        Me.incas_BUT.TabIndex = 38
        Me.incas_BUT.Text = "Incasari"
        Me.incas_BUT.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Image = Global.Magazin.My.Resources.Resources.arrow_right
        Me.Button1.Location = New System.Drawing.Point(141, 104)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(16, 21)
        Me.Button1.TabIndex = 39
        Me.Button1.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.Location = New System.Drawing.Point(12, 40)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(235, 12)
        Me.PictureBox1.TabIndex = 28
        Me.PictureBox1.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(320, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 13)
        Me.Label2.TabIndex = 40
        Me.Label2.Text = "Label2"
        '
        'Form_cheltuieli
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(736, 347)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.incas_BUT)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cont_RB)
        Me.Controls.Add(Me.cash_RB)
        Me.Controls.Add(Me.CheckBox2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.ComboBox3)
        Me.Controls.Add(Me.ad_firma_But)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.suma_Label)
        Me.Controls.Add(Me.explicatii_Label)
        Me.Controls.Add(Me.nr_Label)
        Me.Controls.Add(Me.tip_Label)
        Me.Controls.Add(Me.data_Label)
        Me.Controls.Add(Me.save_But)
        Me.Controls.Add(Me.suma_Textbox)
        Me.Controls.Add(Me.explicatii_Textbox)
        Me.Controls.Add(Me.nr_rzf_Textbox)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name = "Form_cheltuieli"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form3"
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents nr_rzf_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents explicatii_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents suma_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents save_But As System.Windows.Forms.Button
    Friend WithEvents data_Label As System.Windows.Forms.Label
    Friend WithEvents tip_Label As System.Windows.Forms.Label
    Friend WithEvents nr_Label As System.Windows.Forms.Label
    Friend WithEvents explicatii_Label As System.Windows.Forms.Label
    Friend WithEvents suma_Label As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents StergeInregistrareaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents PrinteazaDPToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ad_firma_But As System.Windows.Forms.Button
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents TransferaCheltuialaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents cash_RB As System.Windows.Forms.RadioButton
    Friend WithEvents cont_RB As System.Windows.Forms.RadioButton
    Friend WithEvents ModificaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents edit_Lbl As System.Windows.Forms.Label
    Friend WithEvents id_Lbl As System.Windows.Forms.Label
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents renunt_edit_But As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents incas_BUT As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
