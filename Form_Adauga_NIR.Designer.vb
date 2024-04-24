<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_adauga_nir
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
        Me.nir_TB = New System.Windows.Forms.TextBox()
        Me.fact_TB = New System.Windows.Forms.TextBox()
        Me.firma_TB = New System.Windows.Forms.TextBox()
        Me.produs_TB_1 = New System.Windows.Forms.TextBox()
        Me.buc_TB_1 = New System.Windows.Forms.TextBox()
        Me.cant_TB_1 = New System.Windows.Forms.TextBox()
        Me.pret_ach_TB_1 = New System.Windows.Forms.TextBox()
        Me.pret_ach_tva_TB_1 = New System.Windows.Forms.TextBox()
        Me.pret_vanzare_TB_1 = New System.Windows.Forms.TextBox()
        Me.valoare_TB_1 = New System.Windows.Forms.TextBox()
        Me.chk_1 = New System.Windows.Forms.CheckBox()
        Me.TextBox10 = New System.Windows.Forms.TextBox()
        Me.TextBox11 = New System.Windows.Forms.TextBox()
        Me.TextBox12 = New System.Windows.Forms.TextBox()
        Me.TextBox13 = New System.Windows.Forms.TextBox()
        Me.TextBox14 = New System.Windows.Forms.TextBox()
        Me.TextBox15 = New System.Windows.Forms.TextBox()
        Me.TextBox16 = New System.Windows.Forms.TextBox()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker()
        Me.tva_TB = New System.Windows.Forms.TextBox()
        Me.save_Bu = New System.Windows.Forms.Button()
        Me.ComboBox3 = New System.Windows.Forms.ComboBox()
        Me.tot_valoare_TB = New System.Windows.Forms.TextBox()
        Me.tot_pret_TB = New System.Windows.Forms.TextBox()
        Me.cif_firma_TB = New System.Windows.Forms.TextBox()
        Me.f_jur_TB = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.crt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.produs = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.buc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cant = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pr_ach = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pr_ach_tva = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pret = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.valoare = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.chk = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.edit = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.del = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.crt_Lbl = New System.Windows.Forms.Label()
        Me.ad_prod_But = New System.Windows.Forms.Button()
        Me.tot_cant_TB = New System.Windows.Forms.TextBox()
        Me.upd_int_CHK = New System.Windows.Forms.CheckBox()
        Me.upd_chelt_CHK = New System.Windows.Forms.CheckBox()
        Me.adaos_Lbl = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PrintNir_BU = New System.Windows.Forms.Button()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.tip_nir_CB = New System.Windows.Forms.ComboBox()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'nir_TB
        '
        Me.nir_TB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.nir_TB.Location = New System.Drawing.Point(102, 12)
        Me.nir_TB.Name = "nir_TB"
        Me.nir_TB.Size = New System.Drawing.Size(115, 20)
        Me.nir_TB.TabIndex = 0
        '
        'fact_TB
        '
        Me.fact_TB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.fact_TB.Location = New System.Drawing.Point(102, 38)
        Me.fact_TB.Name = "fact_TB"
        Me.fact_TB.Size = New System.Drawing.Size(115, 20)
        Me.fact_TB.TabIndex = 2
        '
        'firma_TB
        '
        Me.firma_TB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.firma_TB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.firma_TB.Location = New System.Drawing.Point(102, 64)
        Me.firma_TB.Name = "firma_TB"
        Me.firma_TB.Size = New System.Drawing.Size(251, 20)
        Me.firma_TB.TabIndex = 4
        '
        'produs_TB_1
        '
        Me.produs_TB_1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.produs_TB_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.produs_TB_1.Location = New System.Drawing.Point(34, 8)
        Me.produs_TB_1.Name = "produs_TB_1"
        Me.produs_TB_1.Size = New System.Drawing.Size(93, 20)
        Me.produs_TB_1.TabIndex = 8
        '
        'buc_TB_1
        '
        Me.buc_TB_1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.buc_TB_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.buc_TB_1.Location = New System.Drawing.Point(133, 9)
        Me.buc_TB_1.Name = "buc_TB_1"
        Me.buc_TB_1.Size = New System.Drawing.Size(33, 20)
        Me.buc_TB_1.TabIndex = 9
        Me.buc_TB_1.TabStop = False
        '
        'cant_TB_1
        '
        Me.cant_TB_1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cant_TB_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.cant_TB_1.Location = New System.Drawing.Point(172, 8)
        Me.cant_TB_1.Name = "cant_TB_1"
        Me.cant_TB_1.Size = New System.Drawing.Size(38, 20)
        Me.cant_TB_1.TabIndex = 10
        '
        'pret_ach_TB_1
        '
        Me.pret_ach_TB_1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.pret_ach_TB_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.pret_ach_TB_1.Location = New System.Drawing.Point(216, 8)
        Me.pret_ach_TB_1.Name = "pret_ach_TB_1"
        Me.pret_ach_TB_1.Size = New System.Drawing.Size(88, 20)
        Me.pret_ach_TB_1.TabIndex = 11
        '
        'pret_ach_tva_TB_1
        '
        Me.pret_ach_tva_TB_1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.pret_ach_tva_TB_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.pret_ach_tva_TB_1.Location = New System.Drawing.Point(310, 8)
        Me.pret_ach_tva_TB_1.Name = "pret_ach_tva_TB_1"
        Me.pret_ach_tva_TB_1.Size = New System.Drawing.Size(88, 20)
        Me.pret_ach_tva_TB_1.TabIndex = 12
        Me.pret_ach_tva_TB_1.TabStop = False
        '
        'pret_vanzare_TB_1
        '
        Me.pret_vanzare_TB_1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.pret_vanzare_TB_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.pret_vanzare_TB_1.Location = New System.Drawing.Point(404, 8)
        Me.pret_vanzare_TB_1.Name = "pret_vanzare_TB_1"
        Me.pret_vanzare_TB_1.Size = New System.Drawing.Size(88, 20)
        Me.pret_vanzare_TB_1.TabIndex = 13
        '
        'valoare_TB_1
        '
        Me.valoare_TB_1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.valoare_TB_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.valoare_TB_1.Location = New System.Drawing.Point(498, 8)
        Me.valoare_TB_1.Name = "valoare_TB_1"
        Me.valoare_TB_1.Size = New System.Drawing.Size(88, 20)
        Me.valoare_TB_1.TabIndex = 14
        Me.valoare_TB_1.TabStop = False
        '
        'chk_1
        '
        Me.chk_1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.chk_1.AutoSize = True
        Me.chk_1.Location = New System.Drawing.Point(592, 11)
        Me.chk_1.Name = "chk_1"
        Me.chk_1.Size = New System.Drawing.Size(15, 14)
        Me.chk_1.TabIndex = 200
        Me.chk_1.TabStop = False
        Me.chk_1.UseVisualStyleBackColor = True
        '
        'TextBox10
        '
        Me.TextBox10.Enabled = False
        Me.TextBox10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TextBox10.Location = New System.Drawing.Point(536, 123)
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.Size = New System.Drawing.Size(88, 20)
        Me.TextBox10.TabIndex = 207
        Me.TextBox10.TabStop = False
        Me.TextBox10.Text = "Valoare"
        Me.TextBox10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox11
        '
        Me.TextBox11.Enabled = False
        Me.TextBox11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TextBox11.Location = New System.Drawing.Point(442, 123)
        Me.TextBox11.Name = "TextBox11"
        Me.TextBox11.Size = New System.Drawing.Size(88, 20)
        Me.TextBox11.TabIndex = 206
        Me.TextBox11.TabStop = False
        Me.TextBox11.Text = "Pret"
        Me.TextBox11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox12
        '
        Me.TextBox12.Enabled = False
        Me.TextBox12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TextBox12.Location = New System.Drawing.Point(348, 123)
        Me.TextBox12.Name = "TextBox12"
        Me.TextBox12.Size = New System.Drawing.Size(88, 20)
        Me.TextBox12.TabIndex = 205
        Me.TextBox12.TabStop = False
        Me.TextBox12.Text = "Pret cu TVA"
        Me.TextBox12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox13
        '
        Me.TextBox13.Enabled = False
        Me.TextBox13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TextBox13.Location = New System.Drawing.Point(254, 123)
        Me.TextBox13.Name = "TextBox13"
        Me.TextBox13.Size = New System.Drawing.Size(88, 20)
        Me.TextBox13.TabIndex = 204
        Me.TextBox13.TabStop = False
        Me.TextBox13.Text = "Pret fara TVA"
        Me.TextBox13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox14
        '
        Me.TextBox14.Enabled = False
        Me.TextBox14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TextBox14.Location = New System.Drawing.Point(210, 123)
        Me.TextBox14.Name = "TextBox14"
        Me.TextBox14.Size = New System.Drawing.Size(38, 20)
        Me.TextBox14.TabIndex = 202
        Me.TextBox14.TabStop = False
        Me.TextBox14.Text = "Cant"
        Me.TextBox14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox15
        '
        Me.TextBox15.Enabled = False
        Me.TextBox15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TextBox15.Location = New System.Drawing.Point(171, 123)
        Me.TextBox15.Name = "TextBox15"
        Me.TextBox15.Size = New System.Drawing.Size(33, 20)
        Me.TextBox15.TabIndex = 201
        Me.TextBox15.TabStop = False
        Me.TextBox15.Text = "Buc"
        Me.TextBox15.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox16
        '
        Me.TextBox16.Enabled = False
        Me.TextBox16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TextBox16.Location = New System.Drawing.Point(72, 123)
        Me.TextBox16.Name = "TextBox16"
        Me.TextBox16.Size = New System.Drawing.Size(93, 20)
        Me.TextBox16.TabIndex = 200
        Me.TextBox16.TabStop = False
        Me.TextBox16.Text = "Produs"
        Me.TextBox16.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker1.Location = New System.Drawing.Point(223, 9)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(118, 23)
        Me.DateTimePicker1.TabIndex = 1
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.DateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker2.Location = New System.Drawing.Point(223, 38)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(118, 23)
        Me.DateTimePicker2.TabIndex = 3
        '
        'tva_TB
        '
        Me.tva_TB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.tva_TB.Location = New System.Drawing.Point(72, 93)
        Me.tva_TB.Name = "tva_TB"
        Me.tva_TB.Size = New System.Drawing.Size(28, 20)
        Me.tva_TB.TabIndex = 7
        '
        'save_Bu
        '
        Me.save_Bu.Location = New System.Drawing.Point(603, 379)
        Me.save_Bu.Name = "save_Bu"
        Me.save_Bu.Size = New System.Drawing.Size(75, 23)
        Me.save_Bu.TabIndex = 16
        Me.save_Bu.Text = "Salveaza"
        Me.save_Bu.UseVisualStyleBackColor = True
        '
        'ComboBox3
        '
        Me.ComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ComboBox3.FormattingEnabled = True
        Me.ComboBox3.Location = New System.Drawing.Point(513, 9)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(155, 23)
        Me.ComboBox3.TabIndex = 17
        '
        'tot_valoare_TB
        '
        Me.tot_valoare_TB.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.tot_valoare_TB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.tot_valoare_TB.Location = New System.Drawing.Point(433, 353)
        Me.tot_valoare_TB.Name = "tot_valoare_TB"
        Me.tot_valoare_TB.Size = New System.Drawing.Size(88, 20)
        Me.tot_valoare_TB.TabIndex = 208
        Me.tot_valoare_TB.TabStop = False
        '
        'tot_pret_TB
        '
        Me.tot_pret_TB.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.tot_pret_TB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.tot_pret_TB.Location = New System.Drawing.Point(315, 353)
        Me.tot_pret_TB.Name = "tot_pret_TB"
        Me.tot_pret_TB.Size = New System.Drawing.Size(88, 20)
        Me.tot_pret_TB.TabIndex = 209
        Me.tot_pret_TB.TabStop = False
        '
        'cif_firma_TB
        '
        Me.cif_firma_TB.Location = New System.Drawing.Point(470, 64)
        Me.cif_firma_TB.Name = "cif_firma_TB"
        Me.cif_firma_TB.Size = New System.Drawing.Size(100, 20)
        Me.cif_firma_TB.TabIndex = 6
        Me.cif_firma_TB.TabStop = False
        '
        'f_jur_TB
        '
        Me.f_jur_TB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.f_jur_TB.Location = New System.Drawing.Point(359, 64)
        Me.f_jur_TB.Name = "f_jur_TB"
        Me.f_jur_TB.Size = New System.Drawing.Size(63, 20)
        Me.f_jur_TB.TabIndex = 5
        Me.f_jur_TB.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Label1.Location = New System.Drawing.Point(106, 93)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(21, 17)
        Me.Label1.TabIndex = 211
        Me.Label1.Text = "%"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.crt, Me.produs, Me.buc, Me.cant, Me.pr_ach, Me.pr_ach_tva, Me.pret, Me.valoare, Me.chk, Me.edit, Me.del})
        Me.DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke
        Me.DataGridView1.Location = New System.Drawing.Point(38, 180)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersWidth = 10
        Me.DataGridView1.Size = New System.Drawing.Size(640, 167)
        Me.DataGridView1.TabIndex = 212
        '
        'crt
        '
        Me.crt.HeaderText = "crt"
        Me.crt.Name = "crt"
        Me.crt.ReadOnly = True
        '
        'produs
        '
        Me.produs.HeaderText = "Produs"
        Me.produs.Name = "produs"
        '
        'buc
        '
        Me.buc.HeaderText = "Buc"
        Me.buc.Name = "buc"
        '
        'cant
        '
        Me.cant.HeaderText = "Cant"
        Me.cant.Name = "cant"
        '
        'pr_ach
        '
        Me.pr_ach.HeaderText = "Pret fara TVA"
        Me.pr_ach.Name = "pr_ach"
        '
        'pr_ach_tva
        '
        Me.pr_ach_tva.HeaderText = "Pret cu TVA"
        Me.pr_ach_tva.Name = "pr_ach_tva"
        '
        'pret
        '
        Me.pret.HeaderText = "Pret"
        Me.pret.Name = "pret"
        '
        'valoare
        '
        Me.valoare.HeaderText = "Valoare"
        Me.valoare.Name = "valoare"
        '
        'chk
        '
        Me.chk.HeaderText = "bun"
        Me.chk.Name = "chk"
        Me.chk.ReadOnly = True
        '
        'edit
        '
        Me.edit.HeaderText = "edit"
        Me.edit.Name = "edit"
        Me.edit.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.edit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'del
        '
        Me.del.HeaderText = "del"
        Me.del.Name = "del"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.crt_Lbl)
        Me.GroupBox1.Controls.Add(Me.produs_TB_1)
        Me.GroupBox1.Controls.Add(Me.buc_TB_1)
        Me.GroupBox1.Controls.Add(Me.cant_TB_1)
        Me.GroupBox1.Controls.Add(Me.pret_ach_TB_1)
        Me.GroupBox1.Controls.Add(Me.pret_ach_tva_TB_1)
        Me.GroupBox1.Controls.Add(Me.pret_vanzare_TB_1)
        Me.GroupBox1.Controls.Add(Me.valoare_TB_1)
        Me.GroupBox1.Controls.Add(Me.chk_1)
        Me.GroupBox1.Controls.Add(Me.ad_prod_But)
        Me.GroupBox1.Location = New System.Drawing.Point(38, 141)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(639, 39)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        '
        'crt_Lbl
        '
        Me.crt_Lbl.AutoSize = True
        Me.crt_Lbl.Location = New System.Drawing.Point(6, 12)
        Me.crt_Lbl.Name = "crt_Lbl"
        Me.crt_Lbl.Size = New System.Drawing.Size(0, 13)
        Me.crt_Lbl.TabIndex = 214
        '
        'ad_prod_But
        '
        Me.ad_prod_But.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ad_prod_But.Image = Global.Magazin.My.Resources.Resources.ic_input_add
        Me.ad_prod_But.Location = New System.Drawing.Point(613, 8)
        Me.ad_prod_But.Name = "ad_prod_But"
        Me.ad_prod_But.Size = New System.Drawing.Size(26, 25)
        Me.ad_prod_But.TabIndex = 15
        Me.ad_prod_But.UseVisualStyleBackColor = True
        '
        'tot_cant_TB
        '
        Me.tot_cant_TB.Location = New System.Drawing.Point(215, 353)
        Me.tot_cant_TB.Name = "tot_cant_TB"
        Me.tot_cant_TB.Size = New System.Drawing.Size(33, 20)
        Me.tot_cant_TB.TabIndex = 214
        '
        'upd_int_CHK
        '
        Me.upd_int_CHK.AutoSize = True
        Me.upd_int_CHK.Location = New System.Drawing.Point(313, 385)
        Me.upd_int_CHK.Name = "upd_int_CHK"
        Me.upd_int_CHK.Size = New System.Drawing.Size(90, 17)
        Me.upd_int_CHK.TabIndex = 215
        Me.upd_int_CHK.Text = "Update Intrari"
        Me.upd_int_CHK.UseVisualStyleBackColor = True
        '
        'upd_chelt_CHK
        '
        Me.upd_chelt_CHK.AutoSize = True
        Me.upd_chelt_CHK.Location = New System.Drawing.Point(449, 385)
        Me.upd_chelt_CHK.Name = "upd_chelt_CHK"
        Me.upd_chelt_CHK.Size = New System.Drawing.Size(106, 17)
        Me.upd_chelt_CHK.TabIndex = 216
        Me.upd_chelt_CHK.Text = "Update Cheltuieli"
        Me.upd_chelt_CHK.UseVisualStyleBackColor = True
        '
        'adaos_Lbl
        '
        Me.adaos_Lbl.AutoSize = True
        Me.adaos_Lbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.adaos_Lbl.Location = New System.Drawing.Point(533, 356)
        Me.adaos_Lbl.Name = "adaos_Lbl"
        Me.adaos_Lbl.Size = New System.Drawing.Size(42, 13)
        Me.adaos_Lbl.TabIndex = 217
        Me.adaos_Lbl.Text = "Adaos"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(35, 96)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(28, 13)
        Me.Label6.TabIndex = 226
        Me.Label6.Text = "TVA"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(35, 67)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(58, 13)
        Me.Label4.TabIndex = 225
        Me.Label4.Text = "Societatea"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(35, 41)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 224
        Me.Label3.Text = "Nr. Factura"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(35, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 13)
        Me.Label2.TabIndex = 223
        Me.Label2.Text = "Nr. NIR"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(439, 67)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(25, 13)
        Me.Label5.TabIndex = 227
        Me.Label5.Text = "CUI"
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.Location = New System.Drawing.Point(513, 38)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(155, 12)
        Me.PictureBox1.TabIndex = 30
        Me.PictureBox1.TabStop = False
        '
        'PrintNir_BU
        '
        Me.PrintNir_BU.Location = New System.Drawing.Point(38, 371)
        Me.PrintNir_BU.Name = "PrintNir_BU"
        Me.PrintNir_BU.Size = New System.Drawing.Size(75, 23)
        Me.PrintNir_BU.TabIndex = 228
        Me.PrintNir_BU.Text = "Button1"
        Me.PrintNir_BU.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(149, 93)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(81, 17)
        Me.CheckBox1.TabIndex = 229
        Me.CheckBox1.Text = "CheckBox1"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'tip_nir_CB
        '
        Me.tip_nir_CB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tip_nir_CB.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.tip_nir_CB.FormattingEnabled = True
        Me.tip_nir_CB.Location = New System.Drawing.Point(359, 10)
        Me.tip_nir_CB.Name = "tip_nir_CB"
        Me.tip_nir_CB.Size = New System.Drawing.Size(126, 23)
        Me.tip_nir_CB.TabIndex = 230
        '
        'Form_adauga_nir
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(690, 406)
        Me.Controls.Add(Me.tip_nir_CB)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.PrintNir_BU)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.adaos_Lbl)
        Me.Controls.Add(Me.upd_chelt_CHK)
        Me.Controls.Add(Me.upd_int_CHK)
        Me.Controls.Add(Me.tot_cant_TB)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.f_jur_TB)
        Me.Controls.Add(Me.cif_firma_TB)
        Me.Controls.Add(Me.tot_pret_TB)
        Me.Controls.Add(Me.tot_valoare_TB)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.ComboBox3)
        Me.Controls.Add(Me.save_Bu)
        Me.Controls.Add(Me.tva_TB)
        Me.Controls.Add(Me.DateTimePicker2)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Controls.Add(Me.TextBox10)
        Me.Controls.Add(Me.TextBox11)
        Me.Controls.Add(Me.TextBox12)
        Me.Controls.Add(Me.TextBox13)
        Me.Controls.Add(Me.TextBox14)
        Me.Controls.Add(Me.TextBox15)
        Me.Controls.Add(Me.TextBox16)
        Me.Controls.Add(Me.firma_TB)
        Me.Controls.Add(Me.fact_TB)
        Me.Controls.Add(Me.nir_TB)
        Me.Name = "Form_adauga_nir"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form_adauga_nir"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents nir_TB As System.Windows.Forms.TextBox
    Friend WithEvents fact_TB As System.Windows.Forms.TextBox
    Friend WithEvents firma_TB As System.Windows.Forms.TextBox
    Friend WithEvents produs_TB_1 As System.Windows.Forms.TextBox
    Friend WithEvents ad_prod_But As System.Windows.Forms.Button
    Friend WithEvents valoare_TB_1 As System.Windows.Forms.TextBox
    Friend WithEvents pret_vanzare_TB_1 As System.Windows.Forms.TextBox
    Friend WithEvents pret_ach_tva_TB_1 As System.Windows.Forms.TextBox
    Friend WithEvents pret_ach_TB_1 As System.Windows.Forms.TextBox
    Friend WithEvents cant_TB_1 As System.Windows.Forms.TextBox
    Friend WithEvents buc_TB_1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox10 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox11 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox12 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox13 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox14 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox15 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox16 As System.Windows.Forms.TextBox
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimePicker2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents tva_TB As System.Windows.Forms.TextBox
    Friend WithEvents save_Bu As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Friend WithEvents tot_valoare_TB As System.Windows.Forms.TextBox
    Friend WithEvents tot_pret_TB As System.Windows.Forms.TextBox
    Friend WithEvents cif_firma_TB As System.Windows.Forms.TextBox
    Friend WithEvents f_jur_TB As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chk_1 As System.Windows.Forms.CheckBox
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents crt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents produs As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents buc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cant As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents pr_ach As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents pr_ach_tva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents pret As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents valoare As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chk As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents edit As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents del As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents crt_Lbl As System.Windows.Forms.Label
    Friend WithEvents tot_cant_TB As System.Windows.Forms.TextBox
    Friend WithEvents upd_int_CHK As System.Windows.Forms.CheckBox
    Friend WithEvents upd_chelt_CHK As System.Windows.Forms.CheckBox
    Friend WithEvents adaos_Lbl As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents PrintNir_BU As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents tip_nir_CB As System.Windows.Forms.ComboBox
End Class
