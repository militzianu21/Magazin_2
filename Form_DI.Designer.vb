<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_DI
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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.data_Label = New System.Windows.Forms.Label()
        Me.tip_Label = New System.Windows.Forms.Label()
        Me.nr_Label = New System.Windows.Forms.Label()
        Me.explicatii_Label = New System.Windows.Forms.Label()
        Me.suma_Label = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.StergeInregistrareaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PrinteazaDIToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ComboBox3 = New System.Windows.Forms.ComboBox()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker1.Location = New System.Drawing.Point(12, 60)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(123, 20)
        Me.DateTimePicker1.TabIndex = 0
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(12, 98)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(123, 21)
        Me.ComboBox1.TabIndex = 1
        '
        'nr_rzf_Textbox
        '
        Me.nr_rzf_Textbox.Location = New System.Drawing.Point(12, 138)
        Me.nr_rzf_Textbox.Name = "nr_rzf_Textbox"
        Me.nr_rzf_Textbox.Size = New System.Drawing.Size(123, 20)
        Me.nr_rzf_Textbox.TabIndex = 2
        '
        'explicatii_Textbox
        '
        Me.explicatii_Textbox.Location = New System.Drawing.Point(12, 175)
        Me.explicatii_Textbox.Name = "explicatii_Textbox"
        Me.explicatii_Textbox.Size = New System.Drawing.Size(235, 20)
        Me.explicatii_Textbox.TabIndex = 3
        '
        'suma_Textbox
        '
        Me.suma_Textbox.Location = New System.Drawing.Point(12, 222)
        Me.suma_Textbox.Name = "suma_Textbox"
        Me.suma_Textbox.Size = New System.Drawing.Size(123, 20)
        Me.suma_Textbox.TabIndex = 4
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 296)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(123, 23)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Salveaza"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'data_Label
        '
        Me.data_Label.Location = New System.Drawing.Point(141, 60)
        Me.data_Label.Name = "data_Label"
        Me.data_Label.Size = New System.Drawing.Size(106, 20)
        Me.data_Label.TabIndex = 6
        Me.data_Label.Text = "Data"
        Me.data_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tip_Label
        '
        Me.tip_Label.Location = New System.Drawing.Point(141, 98)
        Me.tip_Label.Name = "tip_Label"
        Me.tip_Label.Size = New System.Drawing.Size(106, 21)
        Me.tip_Label.TabIndex = 7
        Me.tip_Label.Text = "Tip Incasare"
        Me.tip_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nr_Label
        '
        Me.nr_Label.Location = New System.Drawing.Point(141, 138)
        Me.nr_Label.Name = "nr_Label"
        Me.nr_Label.Size = New System.Drawing.Size(106, 20)
        Me.nr_Label.TabIndex = 8
        Me.nr_Label.Text = "Nr DI"
        Me.nr_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'explicatii_Label
        '
        Me.explicatii_Label.Location = New System.Drawing.Point(9, 198)
        Me.explicatii_Label.Name = "explicatii_Label"
        Me.explicatii_Label.Size = New System.Drawing.Size(126, 21)
        Me.explicatii_Label.TabIndex = 9
        Me.explicatii_Label.Text = "Explicatii"
        Me.explicatii_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'suma_Label
        '
        Me.suma_Label.Location = New System.Drawing.Point(141, 222)
        Me.suma_Label.Name = "suma_Label"
        Me.suma_Label.Size = New System.Drawing.Size(72, 20)
        Me.suma_Label.TabIndex = 10
        Me.suma_Label.Text = "Suma"
        Me.suma_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StergeInregistrareaToolStripMenuItem, Me.PrinteazaDIToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(176, 48)
        '
        'StergeInregistrareaToolStripMenuItem
        '
        Me.StergeInregistrareaToolStripMenuItem.Name = "StergeInregistrareaToolStripMenuItem"
        Me.StergeInregistrareaToolStripMenuItem.Size = New System.Drawing.Size(175, 22)
        Me.StergeInregistrareaToolStripMenuItem.Text = "Sterge Inregistrarea"
        '
        'PrinteazaDIToolStripMenuItem
        '
        Me.PrinteazaDIToolStripMenuItem.Name = "PrinteazaDIToolStripMenuItem"
        Me.PrinteazaDIToolStripMenuItem.Size = New System.Drawing.Size(175, 22)
        Me.PrinteazaDIToolStripMenuItem.Text = "Printeaza DI"
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.DataGridView1.Location = New System.Drawing.Point(253, 11)
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
        Me.Label1.Location = New System.Drawing.Point(12, 335)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 20)
        Me.Label1.TabIndex = 14
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.Location = New System.Drawing.Point(12, 40)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(235, 12)
        Me.PictureBox1.TabIndex = 30
        Me.PictureBox1.TabStop = False
        '
        'ComboBox3
        '
        Me.ComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ComboBox3.FormattingEnabled = True
        Me.ComboBox3.Location = New System.Drawing.Point(12, 11)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(235, 23)
        Me.ComboBox3.TabIndex = 29
        '
        'Form_DI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.ClientSize = New System.Drawing.Size(690, 362)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.ComboBox3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.suma_Label)
        Me.Controls.Add(Me.explicatii_Label)
        Me.Controls.Add(Me.nr_Label)
        Me.Controls.Add(Me.tip_Label)
        Me.Controls.Add(Me.data_Label)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.suma_Textbox)
        Me.Controls.Add(Me.explicatii_Textbox)
        Me.Controls.Add(Me.nr_rzf_Textbox)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name = "Form_DI"
        Me.Text = "Form_DI"
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents nr_rzf_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents explicatii_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents suma_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents data_Label As System.Windows.Forms.Label
    Friend WithEvents tip_Label As System.Windows.Forms.Label
    Friend WithEvents nr_Label As System.Windows.Forms.Label
    Friend WithEvents explicatii_Label As System.Windows.Forms.Label
    Friend WithEvents suma_Label As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents StergeInregistrareaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PrinteazaDIToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
End Class
