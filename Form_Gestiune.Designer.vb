<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_gestiune
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
        Me.nr_nir_Textbox = New System.Windows.Forms.TextBox()
        Me.explicatii_Textbox = New System.Windows.Forms.TextBox()
        Me.suma_Textbox = New System.Windows.Forms.TextBox()
        Me.save_BU = New System.Windows.Forms.Button()
        Me.data_Label = New System.Windows.Forms.Label()
        Me.tip_Label = New System.Windows.Forms.Label()
        Me.nr_Label = New System.Windows.Forms.Label()
        Me.explicatii_Label = New System.Windows.Forms.Label()
        Me.suma_Label = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ModificaIntrareToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.StergeIntrareaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ComboBox3 = New System.Windows.Forms.ComboBox()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.mag_v_Lbl = New System.Windows.Forms.Label()
        Me.rzf_v_Lbl = New System.Windows.Forms.Label()
        Me.edit_Lbl = New System.Windows.Forms.Label()
        Me.renunt_edit_But = New System.Windows.Forms.Button()
        Me.id_Lbl = New System.Windows.Forms.Label()
        Me.ArataModificaNIRToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker1.Location = New System.Drawing.Point(12, 64)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(123, 20)
        Me.DateTimePicker1.TabIndex = 0
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(12, 102)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(123, 21)
        Me.ComboBox1.TabIndex = 1
        '
        'nr_nir_Textbox
        '
        Me.nr_nir_Textbox.Location = New System.Drawing.Point(12, 142)
        Me.nr_nir_Textbox.Name = "nr_nir_Textbox"
        Me.nr_nir_Textbox.Size = New System.Drawing.Size(123, 20)
        Me.nr_nir_Textbox.TabIndex = 2
        '
        'explicatii_Textbox
        '
        Me.explicatii_Textbox.Location = New System.Drawing.Point(12, 229)
        Me.explicatii_Textbox.Name = "explicatii_Textbox"
        Me.explicatii_Textbox.Size = New System.Drawing.Size(235, 20)
        Me.explicatii_Textbox.TabIndex = 3
        '
        'suma_Textbox
        '
        Me.suma_Textbox.Location = New System.Drawing.Point(12, 276)
        Me.suma_Textbox.Name = "suma_Textbox"
        Me.suma_Textbox.Size = New System.Drawing.Size(123, 20)
        Me.suma_Textbox.TabIndex = 4
        '
        'save_BU
        '
        Me.save_BU.Location = New System.Drawing.Point(12, 315)
        Me.save_BU.Name = "save_BU"
        Me.save_BU.Size = New System.Drawing.Size(123, 23)
        Me.save_BU.TabIndex = 5
        Me.save_BU.Text = "Salveaza"
        Me.save_BU.UseVisualStyleBackColor = True
        '
        'data_Label
        '
        Me.data_Label.Location = New System.Drawing.Point(141, 64)
        Me.data_Label.Name = "data_Label"
        Me.data_Label.Size = New System.Drawing.Size(106, 20)
        Me.data_Label.TabIndex = 6
        Me.data_Label.Text = "Data"
        Me.data_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tip_Label
        '
        Me.tip_Label.Location = New System.Drawing.Point(141, 102)
        Me.tip_Label.Name = "tip_Label"
        Me.tip_Label.Size = New System.Drawing.Size(106, 21)
        Me.tip_Label.TabIndex = 7
        Me.tip_Label.Text = "Tip Document"
        Me.tip_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nr_Label
        '
        Me.nr_Label.Location = New System.Drawing.Point(144, 142)
        Me.nr_Label.Name = "nr_Label"
        Me.nr_Label.Size = New System.Drawing.Size(86, 20)
        Me.nr_Label.TabIndex = 8
        Me.nr_Label.Text = "Nr NIR"
        Me.nr_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'explicatii_Label
        '
        Me.explicatii_Label.Location = New System.Drawing.Point(13, 205)
        Me.explicatii_Label.Name = "explicatii_Label"
        Me.explicatii_Label.Size = New System.Drawing.Size(104, 21)
        Me.explicatii_Label.TabIndex = 9
        Me.explicatii_Label.Text = "Firma"
        Me.explicatii_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'suma_Label
        '
        Me.suma_Label.Location = New System.Drawing.Point(141, 275)
        Me.suma_Label.Name = "suma_Label"
        Me.suma_Label.Size = New System.Drawing.Size(72, 20)
        Me.suma_Label.TabIndex = 10
        Me.suma_Label.Text = "Suma"
        Me.suma_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ModificaIntrareToolStripMenuItem, Me.ArataModificaNIRToolStripMenuItem, Me.ToolStripSeparator1, Me.StergeIntrareaToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(177, 98)
        '
        'ModificaIntrareToolStripMenuItem
        '
        Me.ModificaIntrareToolStripMenuItem.Image = Global.Magazin.My.Resources.Resources.edit_query
        Me.ModificaIntrareToolStripMenuItem.Name = "ModificaIntrareToolStripMenuItem"
        Me.ModificaIntrareToolStripMenuItem.Size = New System.Drawing.Size(176, 22)
        Me.ModificaIntrareToolStripMenuItem.Text = "Modifica"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(173, 6)
        '
        'StergeIntrareaToolStripMenuItem
        '
        Me.StergeIntrareaToolStripMenuItem.Image = Global.Magazin.My.Resources.Resources.cancel
        Me.StergeIntrareaToolStripMenuItem.Name = "StergeIntrareaToolStripMenuItem"
        Me.StergeIntrareaToolStripMenuItem.Size = New System.Drawing.Size(176, 22)
        Me.StergeIntrareaToolStripMenuItem.Text = "Sterge Inregistrarea"
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.DataGridView1.Location = New System.Drawing.Point(257, 40)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersWidth = 5
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(421, 273)
        Me.DataGridView1.TabIndex = 13
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(149, 315)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 20)
        Me.Label1.TabIndex = 14
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.CheckBox1.Location = New System.Drawing.Point(12, 168)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(79, 17)
        Me.CheckBox1.TabIndex = 15
        Me.CheckBox1.Text = "Consecutiv"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Button2.Location = New System.Drawing.Point(123, 204)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(124, 23)
        Me.Button2.TabIndex = 16
        Me.Button2.Text = "Adauga Firma (Ctrl+A)"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'ComboBox3
        '
        Me.ComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ComboBox3.FormattingEnabled = True
        Me.ComboBox3.Location = New System.Drawing.Point(12, 11)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(235, 23)
        Me.ComboBox3.TabIndex = 22
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(253, 17)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(102, 17)
        Me.CheckBox2.TabIndex = 31
        Me.CheckBox2.Text = "Luna Curenta"
        Me.CheckBox2.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.SpringGreen
        Me.Label2.Location = New System.Drawing.Point(155, 311)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 24)
        Me.Label2.TabIndex = 35
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
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(605, 325)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 37
        Me.Button1.Text = "Marfa"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.mag_v_Lbl)
        Me.GroupBox1.Controls.Add(Me.rzf_v_Lbl)
        Me.GroupBox1.Controls.Add(Me.edit_Lbl)
        Me.GroupBox1.Controls.Add(Me.renunt_edit_But)
        Me.GroupBox1.Controls.Add(Me.id_Lbl)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(418, -1)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(260, 35)
        Me.GroupBox1.TabIndex = 38
        Me.GroupBox1.TabStop = False
        '
        'mag_v_Lbl
        '
        Me.mag_v_Lbl.AutoSize = True
        Me.mag_v_Lbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.mag_v_Lbl.Location = New System.Drawing.Point(101, 14)
        Me.mag_v_Lbl.Name = "mag_v_Lbl"
        Me.mag_v_Lbl.Size = New System.Drawing.Size(30, 13)
        Me.mag_v_Lbl.TabIndex = 37
        Me.mag_v_Lbl.Text = "mag"
        '
        'rzf_v_Lbl
        '
        Me.rzf_v_Lbl.AutoSize = True
        Me.rzf_v_Lbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.rzf_v_Lbl.Location = New System.Drawing.Point(54, 14)
        Me.rzf_v_Lbl.Name = "rzf_v_Lbl"
        Me.rzf_v_Lbl.Size = New System.Drawing.Size(21, 13)
        Me.rzf_v_Lbl.TabIndex = 36
        Me.rzf_v_Lbl.Text = "rzf"
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
        'renunt_edit_But
        '
        Me.renunt_edit_But.Image = Global.Magazin.My.Resources.Resources.cancel
        Me.renunt_edit_But.Location = New System.Drawing.Point(228, 10)
        Me.renunt_edit_But.Name = "renunt_edit_But"
        Me.renunt_edit_But.Size = New System.Drawing.Size(26, 23)
        Me.renunt_edit_But.TabIndex = 35
        Me.renunt_edit_But.UseVisualStyleBackColor = True
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
        'ArataModificaNIRToolStripMenuItem
        '
        Me.ArataModificaNIRToolStripMenuItem.Name = "ArataModificaNIRToolStripMenuItem"
        Me.ArataModificaNIRToolStripMenuItem.Size = New System.Drawing.Size(176, 22)
        Me.ArataModificaNIRToolStripMenuItem.Text = "Arata\Modifica NIR"
        '
        'Form_gestiune
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(690, 359)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.CheckBox2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.ComboBox3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.suma_Label)
        Me.Controls.Add(Me.explicatii_Label)
        Me.Controls.Add(Me.nr_Label)
        Me.Controls.Add(Me.tip_Label)
        Me.Controls.Add(Me.data_Label)
        Me.Controls.Add(Me.save_BU)
        Me.Controls.Add(Me.suma_Textbox)
        Me.Controls.Add(Me.explicatii_Textbox)
        Me.Controls.Add(Me.nr_nir_Textbox)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name = "Form_gestiune"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form_gestiune"
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents nr_nir_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents explicatii_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents suma_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents save_BU As System.Windows.Forms.Button
    Friend WithEvents data_Label As System.Windows.Forms.Label
    Friend WithEvents tip_Label As System.Windows.Forms.Label
    Friend WithEvents nr_Label As System.Windows.Forms.Label
    Friend WithEvents explicatii_Label As System.Windows.Forms.Label
    Friend WithEvents suma_Label As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents StergeIntrareaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents ModificaIntrareToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents mag_v_Lbl As System.Windows.Forms.Label
    Friend WithEvents rzf_v_Lbl As System.Windows.Forms.Label
    Friend WithEvents edit_Lbl As System.Windows.Forms.Label
    Friend WithEvents renunt_edit_But As System.Windows.Forms.Button
    Friend WithEvents id_Lbl As System.Windows.Forms.Label
    Friend WithEvents ArataModificaNIRToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
