<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_Aviz
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
        Me.suma_Label = New System.Windows.Forms.Label()
        Me.nr_Label = New System.Windows.Forms.Label()
        Me.data_Label = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.suma_Textbox = New System.Windows.Forms.TextBox()
        Me.nr_nir_Textbox = New System.Windows.Forms.TextBox()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'suma_Label
        '
        Me.suma_Label.Location = New System.Drawing.Point(141, 137)
        Me.suma_Label.Name = "suma_Label"
        Me.suma_Label.Size = New System.Drawing.Size(72, 20)
        Me.suma_Label.TabIndex = 17
        Me.suma_Label.Text = "Suma"
        Me.suma_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nr_Label
        '
        Me.nr_Label.Location = New System.Drawing.Point(157, 73)
        Me.nr_Label.Name = "nr_Label"
        Me.nr_Label.Size = New System.Drawing.Size(86, 20)
        Me.nr_Label.TabIndex = 16
        Me.nr_Label.Text = "Nr NIR"
        Me.nr_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'data_Label
        '
        Me.data_Label.Location = New System.Drawing.Point(141, 35)
        Me.data_Label.Name = "data_Label"
        Me.data_Label.Size = New System.Drawing.Size(106, 20)
        Me.data_Label.TabIndex = 15
        Me.data_Label.Text = "Data"
        Me.data_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 176)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(123, 23)
        Me.Button1.TabIndex = 14
        Me.Button1.Text = "Salveaza"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'suma_Textbox
        '
        Me.suma_Textbox.Location = New System.Drawing.Point(12, 138)
        Me.suma_Textbox.Name = "suma_Textbox"
        Me.suma_Textbox.Size = New System.Drawing.Size(123, 20)
        Me.suma_Textbox.TabIndex = 13
        '
        'nr_nir_Textbox
        '
        Me.nr_nir_Textbox.Location = New System.Drawing.Point(12, 73)
        Me.nr_nir_Textbox.Name = "nr_nir_Textbox"
        Me.nr_nir_Textbox.Size = New System.Drawing.Size(123, 20)
        Me.nr_nir_Textbox.TabIndex = 12
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker1.Location = New System.Drawing.Point(12, 35)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(123, 20)
        Me.DateTimePicker1.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Label1"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(96, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Label2"
        '
        'Form_Aviz
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.suma_Label)
        Me.Controls.Add(Me.nr_Label)
        Me.Controls.Add(Me.data_Label)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.suma_Textbox)
        Me.Controls.Add(Me.nr_nir_Textbox)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Name = "Form_Aviz"
        Me.Text = "Form_Aviz"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents suma_Label As System.Windows.Forms.Label
    Friend WithEvents nr_Label As System.Windows.Forms.Label
    Friend WithEvents data_Label As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents suma_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents nr_nir_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
