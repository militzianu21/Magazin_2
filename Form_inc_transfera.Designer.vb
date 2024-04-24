<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_inc_transfera
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
        Me.id_Lbl = New System.Windows.Forms.Label()
        Me.suma_Lbl = New System.Windows.Forms.Label()
        Me.cash_TB = New System.Windows.Forms.TextBox()
        Me.pos_TB = New System.Windows.Forms.TextBox()
        Me.save_Bu = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'id_Lbl
        '
        Me.id_Lbl.AutoSize = True
        Me.id_Lbl.Location = New System.Drawing.Point(32, 9)
        Me.id_Lbl.Name = "id_Lbl"
        Me.id_Lbl.Size = New System.Drawing.Size(15, 13)
        Me.id_Lbl.TabIndex = 0
        Me.id_Lbl.Text = "id"
        '
        'suma_Lbl
        '
        Me.suma_Lbl.AutoSize = True
        Me.suma_Lbl.Location = New System.Drawing.Point(120, 9)
        Me.suma_Lbl.Name = "suma_Lbl"
        Me.suma_Lbl.Size = New System.Drawing.Size(32, 13)
        Me.suma_Lbl.TabIndex = 1
        Me.suma_Lbl.Text = "suma"
        '
        'cash_TB
        '
        Me.cash_TB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.cash_TB.Location = New System.Drawing.Point(12, 36)
        Me.cash_TB.Name = "cash_TB"
        Me.cash_TB.Size = New System.Drawing.Size(146, 23)
        Me.cash_TB.TabIndex = 2
        '
        'pos_TB
        '
        Me.pos_TB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.pos_TB.Location = New System.Drawing.Point(235, 36)
        Me.pos_TB.Name = "pos_TB"
        Me.pos_TB.Size = New System.Drawing.Size(146, 23)
        Me.pos_TB.TabIndex = 3
        '
        'save_Bu
        '
        Me.save_Bu.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.save_Bu.Location = New System.Drawing.Point(371, 65)
        Me.save_Bu.Name = "save_Bu"
        Me.save_Bu.Size = New System.Drawing.Size(100, 23)
        Me.save_Bu.TabIndex = 4
        Me.save_Bu.Text = "Salveaza"
        Me.save_Bu.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 71)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Cash"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(232, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "POS"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(4, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(22, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Id :"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(80, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(34, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Total:"
        '
        'Form_inc_transfera
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.ClientSize = New System.Drawing.Size(483, 93)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.save_Bu)
        Me.Controls.Add(Me.pos_TB)
        Me.Controls.Add(Me.cash_TB)
        Me.Controls.Add(Me.suma_Lbl)
        Me.Controls.Add(Me.id_Lbl)
        Me.Name = "Form_inc_transfera"
        Me.Text = "Form_inc_transfera"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents id_Lbl As System.Windows.Forms.Label
    Friend WithEvents suma_Lbl As System.Windows.Forms.Label
    Friend WithEvents cash_TB As System.Windows.Forms.TextBox
    Friend WithEvents pos_TB As System.Windows.Forms.TextBox
    Friend WithEvents save_Bu As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
