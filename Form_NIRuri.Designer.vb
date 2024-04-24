<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_NIRuri
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.StergeInregistrareaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ArataNIRToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ad_nir_But = New System.Windows.Forms.Button()
        Me.tip_nir_CB = New System.Windows.Forms.ComboBox()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ArataNIRToolStripMenuItem, Me.StergeInregistrareaToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(181, 70)
        '
        'StergeInregistrareaToolStripMenuItem
        '
        Me.StergeInregistrareaToolStripMenuItem.Name = "StergeInregistrareaToolStripMenuItem"
        Me.StergeInregistrareaToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.StergeInregistrareaToolStripMenuItem.Text = "Sterge Inregistrarea"
        '
        'ArataNIRToolStripMenuItem
        '
        Me.ArataNIRToolStripMenuItem.Name = "ArataNIRToolStripMenuItem"
        Me.ArataNIRToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ArataNIRToolStripMenuItem.Text = "Arata/Modifica NIR"
        '
        'DataGridView1
        '
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.DataGridView1.Location = New System.Drawing.Point(13, 12)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersWidth = 5
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(574, 340)
        Me.DataGridView1.TabIndex = 13
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(10, 329)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 13)
        Me.Label1.TabIndex = 14
        '
        'ad_nir_But
        '
        Me.ad_nir_But.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ad_nir_But.Location = New System.Drawing.Point(593, 329)
        Me.ad_nir_But.Name = "ad_nir_But"
        Me.ad_nir_But.Size = New System.Drawing.Size(104, 23)
        Me.ad_nir_But.TabIndex = 31
        Me.ad_nir_But.Text = "Adauga NIR"
        Me.ad_nir_But.UseVisualStyleBackColor = True
        '
        'tip_nir_CB
        '
        Me.tip_nir_CB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tip_nir_CB.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.tip_nir_CB.FormattingEnabled = True
        Me.tip_nir_CB.Location = New System.Drawing.Point(593, 12)
        Me.tip_nir_CB.Name = "tip_nir_CB"
        Me.tip_nir_CB.Size = New System.Drawing.Size(94, 23)
        Me.tip_nir_CB.TabIndex = 231
        '
        'Form_NIRuri
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(699, 359)
        Me.Controls.Add(Me.tip_nir_CB)
        Me.Controls.Add(Me.ad_nir_But)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Name = "Form_NIRuri"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form_Niruri"
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents StergeInregistrareaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ad_nir_But As System.Windows.Forms.Button
    Friend WithEvents ArataNIRToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tip_nir_CB As ComboBox
End Class
