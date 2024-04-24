<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_aleg_Plata
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
        Me.bon_BU = New System.Windows.Forms.Button()
        Me.ch_Bu = New System.Windows.Forms.Button()
        Me.nu_Bu = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'bon_BU
        '
        Me.bon_BU.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bon_BU.Location = New System.Drawing.Point(12, 74)
        Me.bon_BU.Name = "bon_BU"
        Me.bon_BU.Size = New System.Drawing.Size(346, 31)
        Me.bon_BU.TabIndex = 0
        Me.bon_BU.Text = "Bon Fiscal"
        Me.bon_BU.UseVisualStyleBackColor = True
        '
        'ch_Bu
        '
        Me.ch_Bu.DialogResult = System.Windows.Forms.DialogResult.Retry
        Me.ch_Bu.Location = New System.Drawing.Point(12, 111)
        Me.ch_Bu.Name = "ch_Bu"
        Me.ch_Bu.Size = New System.Drawing.Size(346, 31)
        Me.ch_Bu.TabIndex = 1
        Me.ch_Bu.Text = "Chitanta"
        Me.ch_Bu.UseVisualStyleBackColor = True
        '
        'nu_Bu
        '
        Me.nu_Bu.DialogResult = System.Windows.Forms.DialogResult.No
        Me.nu_Bu.Location = New System.Drawing.Point(12, 145)
        Me.nu_Bu.Name = "nu_Bu"
        Me.nu_Bu.Size = New System.Drawing.Size(346, 31)
        Me.nu_Bu.TabIndex = 2
        Me.nu_Bu.Text = "Nu"
        Me.nu_Bu.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(142, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(86, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Platesti Factura?"
        '
        'Form_aleg_Plata
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(367, 182)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.nu_Bu)
        Me.Controls.Add(Me.ch_Bu)
        Me.Controls.Add(Me.bon_BU)
        Me.Name = "Form_aleg_Plata"
        Me.Text = "Form_Plata"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents bon_BU As System.Windows.Forms.Button
    Friend WithEvents ch_Bu As System.Windows.Forms.Button
    Friend WithEvents nu_Bu As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
