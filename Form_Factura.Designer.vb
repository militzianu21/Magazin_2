<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_adauga_factura
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
        Me.nr_fact_TB = New System.Windows.Forms.TextBox()
        Me.firma_TB = New System.Windows.Forms.TextBox()
        Me.produs_TB_1 = New System.Windows.Forms.TextBox()
        Me.buc_TB_1 = New System.Windows.Forms.TextBox()
        Me.cant_TB_1 = New System.Windows.Forms.TextBox()
        Me.pret_unitar_TB = New System.Windows.Forms.TextBox()
        Me.valoare_TB = New System.Windows.Forms.TextBox()
        Me.chk_1 = New System.Windows.Forms.CheckBox()
        Me.TextBox10 = New System.Windows.Forms.TextBox()
        Me.TextBox13 = New System.Windows.Forms.TextBox()
        Me.TextBox14 = New System.Windows.Forms.TextBox()
        Me.TextBox15 = New System.Windows.Forms.TextBox()
        Me.TextBox16 = New System.Windows.Forms.TextBox()
        Me.data_DTP = New System.Windows.Forms.DateTimePicker()
        Me.save_Bu = New System.Windows.Forms.Button()
        Me.ComboBox3 = New System.Windows.Forms.ComboBox()
        Me.cif_firma_TB = New System.Windows.Forms.TextBox()
        Me.f_jur_TB = New System.Windows.Forms.TextBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.crt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.produs = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.buc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cant = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pret_unitar = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.valoare = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.chk = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.edit = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.del = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.crt_Lbl = New System.Windows.Forms.Label()
        Me.ad_prod_But = New System.Windows.Forms.Button()
        Me.tot_cant_TB = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Print_BU = New System.Windows.Forms.Button()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.J_firma_TB = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.adresa_tb = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.localitate_TB = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.judet_TB = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cont_TB = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.banca_TB = New System.Windows.Forms.TextBox()
        Me.tot_valoare_TB = New System.Windows.Forms.TextBox()
        Me.ro_CB = New System.Windows.Forms.CheckBox()
        Me.edit_BU = New System.Windows.Forms.Button()
        Me.id_Lbl = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.seria_TB = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.nume_TB = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.nr_CI_TB = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.eliber_TB = New System.Windows.Forms.MaskedTextBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.obs_TB = New System.Windows.Forms.TextBox()
        Me.plata_BU = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'nr_fact_TB
        '
        Me.nr_fact_TB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.nr_fact_TB.Location = New System.Drawing.Point(103, 6)
        Me.nr_fact_TB.Name = "nr_fact_TB"
        Me.nr_fact_TB.Size = New System.Drawing.Size(115, 20)
        Me.nr_fact_TB.TabIndex = 2
        '
        'firma_TB
        '
        Me.firma_TB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.firma_TB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.firma_TB.Location = New System.Drawing.Point(103, 32)
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
        Me.produs_TB_1.TabIndex = 10
        '
        'buc_TB_1
        '
        Me.buc_TB_1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.buc_TB_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.buc_TB_1.Location = New System.Drawing.Point(133, 9)
        Me.buc_TB_1.Name = "buc_TB_1"
        Me.buc_TB_1.Size = New System.Drawing.Size(33, 20)
        Me.buc_TB_1.TabIndex = 11
        Me.buc_TB_1.TabStop = False
        '
        'cant_TB_1
        '
        Me.cant_TB_1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cant_TB_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.cant_TB_1.Location = New System.Drawing.Point(172, 8)
        Me.cant_TB_1.Name = "cant_TB_1"
        Me.cant_TB_1.Size = New System.Drawing.Size(38, 20)
        Me.cant_TB_1.TabIndex = 12
        '
        'pret_unitar_TB
        '
        Me.pret_unitar_TB.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.pret_unitar_TB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.pret_unitar_TB.Location = New System.Drawing.Point(216, 8)
        Me.pret_unitar_TB.Name = "pret_unitar_TB"
        Me.pret_unitar_TB.Size = New System.Drawing.Size(88, 20)
        Me.pret_unitar_TB.TabIndex = 13
        '
        'valoare_TB
        '
        Me.valoare_TB.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.valoare_TB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.valoare_TB.Location = New System.Drawing.Point(498, 8)
        Me.valoare_TB.Name = "valoare_TB"
        Me.valoare_TB.Size = New System.Drawing.Size(88, 20)
        Me.valoare_TB.TabIndex = 14
        Me.valoare_TB.TabStop = False
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
        Me.TextBox10.Location = New System.Drawing.Point(536, 219)
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.Size = New System.Drawing.Size(88, 20)
        Me.TextBox10.TabIndex = 207
        Me.TextBox10.TabStop = False
        Me.TextBox10.Text = "Valoare"
        Me.TextBox10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox13
        '
        Me.TextBox13.Enabled = False
        Me.TextBox13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TextBox13.Location = New System.Drawing.Point(254, 219)
        Me.TextBox13.Name = "TextBox13"
        Me.TextBox13.Size = New System.Drawing.Size(88, 20)
        Me.TextBox13.TabIndex = 204
        Me.TextBox13.TabStop = False
        Me.TextBox13.Text = "Pret unitar"
        Me.TextBox13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox14
        '
        Me.TextBox14.Enabled = False
        Me.TextBox14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TextBox14.Location = New System.Drawing.Point(210, 219)
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
        Me.TextBox15.Location = New System.Drawing.Point(171, 219)
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
        Me.TextBox16.Location = New System.Drawing.Point(72, 219)
        Me.TextBox16.Name = "TextBox16"
        Me.TextBox16.Size = New System.Drawing.Size(93, 20)
        Me.TextBox16.TabIndex = 200
        Me.TextBox16.TabStop = False
        Me.TextBox16.Text = "Produs"
        Me.TextBox16.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'data_DTP
        '
        Me.data_DTP.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.data_DTP.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.data_DTP.Location = New System.Drawing.Point(224, 6)
        Me.data_DTP.Name = "data_DTP"
        Me.data_DTP.Size = New System.Drawing.Size(118, 23)
        Me.data_DTP.TabIndex = 3
        '
        'save_Bu
        '
        Me.save_Bu.Location = New System.Drawing.Point(603, 469)
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
        Me.ComboBox3.Location = New System.Drawing.Point(473, 9)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(195, 23)
        Me.ComboBox3.TabIndex = 17
        '
        'cif_firma_TB
        '
        Me.cif_firma_TB.Location = New System.Drawing.Point(103, 58)
        Me.cif_firma_TB.Name = "cif_firma_TB"
        Me.cif_firma_TB.Size = New System.Drawing.Size(100, 20)
        Me.cif_firma_TB.TabIndex = 6
        Me.cif_firma_TB.TabStop = False
        '
        'f_jur_TB
        '
        Me.f_jur_TB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.f_jur_TB.Location = New System.Drawing.Point(360, 32)
        Me.f_jur_TB.Name = "f_jur_TB"
        Me.f_jur_TB.Size = New System.Drawing.Size(63, 20)
        Me.f_jur_TB.TabIndex = 5
        Me.f_jur_TB.TabStop = False
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.crt, Me.produs, Me.buc, Me.cant, Me.pret_unitar, Me.valoare, Me.chk, Me.edit, Me.del})
        Me.DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke
        Me.DataGridView1.Location = New System.Drawing.Point(38, 281)
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
        'pret_unitar
        '
        Me.pret_unitar.HeaderText = "Pret unitar"
        Me.pret_unitar.Name = "pret_unitar"
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
        Me.GroupBox1.Controls.Add(Me.pret_unitar_TB)
        Me.GroupBox1.Controls.Add(Me.valoare_TB)
        Me.GroupBox1.Controls.Add(Me.chk_1)
        Me.GroupBox1.Controls.Add(Me.ad_prod_But)
        Me.GroupBox1.Location = New System.Drawing.Point(38, 237)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(639, 39)
        Me.GroupBox1.TabIndex = 10
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
        Me.tot_cant_TB.Location = New System.Drawing.Point(215, 454)
        Me.tot_cant_TB.Name = "tot_cant_TB"
        Me.tot_cant_TB.Size = New System.Drawing.Size(33, 20)
        Me.tot_cant_TB.TabIndex = 214
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(36, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(58, 13)
        Me.Label4.TabIndex = 225
        Me.Label4.Text = "Societatea"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(36, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 224
        Me.Label3.Text = "Nr. Factura"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(69, 61)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(25, 13)
        Me.Label5.TabIndex = 227
        Me.Label5.Text = "CUI"
        '
        'Print_BU
        '
        Me.Print_BU.Location = New System.Drawing.Point(38, 472)
        Me.Print_BU.Name = "Print_BU"
        Me.Print_BU.Size = New System.Drawing.Size(75, 23)
        Me.Print_BU.TabIndex = 228
        Me.Print_BU.Text = "Button1"
        Me.Print_BU.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(85, 87)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(12, 13)
        Me.Label2.TabIndex = 231
        Me.Label2.Text = "J"
        '
        'J_firma_TB
        '
        Me.J_firma_TB.Location = New System.Drawing.Point(103, 84)
        Me.J_firma_TB.Name = "J_firma_TB"
        Me.J_firma_TB.Size = New System.Drawing.Size(100, 20)
        Me.J_firma_TB.TabIndex = 230
        Me.J_firma_TB.TabStop = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(274, 58)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(40, 13)
        Me.Label7.TabIndex = 233
        Me.Label7.Text = "Adresa"
        '
        'adresa_tb
        '
        Me.adresa_tb.Location = New System.Drawing.Point(320, 55)
        Me.adresa_tb.Name = "adresa_tb"
        Me.adresa_tb.Size = New System.Drawing.Size(348, 20)
        Me.adresa_tb.TabIndex = 232
        Me.adresa_tb.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(257, 84)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(59, 13)
        Me.Label8.TabIndex = 235
        Me.Label8.Text = "Localitatea"
        '
        'localitate_TB
        '
        Me.localitate_TB.Location = New System.Drawing.Point(320, 81)
        Me.localitate_TB.Name = "localitate_TB"
        Me.localitate_TB.Size = New System.Drawing.Size(100, 20)
        Me.localitate_TB.TabIndex = 234
        Me.localitate_TB.TabStop = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(275, 110)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(41, 13)
        Me.Label9.TabIndex = 237
        Me.Label9.Text = "Judetul"
        '
        'judet_TB
        '
        Me.judet_TB.Location = New System.Drawing.Point(320, 107)
        Me.judet_TB.Name = "judet_TB"
        Me.judet_TB.Size = New System.Drawing.Size(100, 20)
        Me.judet_TB.TabIndex = 236
        Me.judet_TB.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(451, 84)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(29, 13)
        Me.Label10.TabIndex = 239
        Me.Label10.Text = "Cont"
        '
        'cont_TB
        '
        Me.cont_TB.Location = New System.Drawing.Point(486, 81)
        Me.cont_TB.Name = "cont_TB"
        Me.cont_TB.Size = New System.Drawing.Size(182, 20)
        Me.cont_TB.TabIndex = 238
        Me.cont_TB.TabStop = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(442, 110)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(38, 13)
        Me.Label11.TabIndex = 241
        Me.Label11.Text = "Banca"
        '
        'banca_TB
        '
        Me.banca_TB.Location = New System.Drawing.Point(486, 106)
        Me.banca_TB.Name = "banca_TB"
        Me.banca_TB.Size = New System.Drawing.Size(182, 20)
        Me.banca_TB.TabIndex = 240
        Me.banca_TB.TabStop = False
        '
        'tot_valoare_TB
        '
        Me.tot_valoare_TB.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.tot_valoare_TB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.tot_valoare_TB.Location = New System.Drawing.Point(497, 469)
        Me.tot_valoare_TB.Name = "tot_valoare_TB"
        Me.tot_valoare_TB.Size = New System.Drawing.Size(88, 20)
        Me.tot_valoare_TB.TabIndex = 208
        Me.tot_valoare_TB.TabStop = False
        '
        'ro_CB
        '
        Me.ro_CB.AutoSize = True
        Me.ro_CB.Location = New System.Drawing.Point(211, 61)
        Me.ro_CB.Name = "ro_CB"
        Me.ro_CB.Size = New System.Drawing.Size(42, 17)
        Me.ro_CB.TabIndex = 242
        Me.ro_CB.Text = "RO"
        Me.ro_CB.UseVisualStyleBackColor = True
        '
        'edit_BU
        '
        Me.edit_BU.Location = New System.Drawing.Point(12, 58)
        Me.edit_BU.Name = "edit_BU"
        Me.edit_BU.Size = New System.Drawing.Size(52, 46)
        Me.edit_BU.TabIndex = 243
        Me.edit_BU.Text = "Edit"
        Me.edit_BU.UseVisualStyleBackColor = True
        '
        'id_Lbl
        '
        Me.id_Lbl.AutoSize = True
        Me.id_Lbl.Location = New System.Drawing.Point(100, 113)
        Me.id_Lbl.Name = "id_Lbl"
        Me.id_Lbl.Size = New System.Drawing.Size(45, 13)
        Me.id_Lbl.TabIndex = 244
        Me.id_Lbl.Text = "Label12"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(67, 173)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 13)
        Me.Label1.TabIndex = 248
        Me.Label1.Text = "Seria"
        '
        'seria_TB
        '
        Me.seria_TB.Location = New System.Drawing.Point(104, 170)
        Me.seria_TB.Name = "seria_TB"
        Me.seria_TB.Size = New System.Drawing.Size(23, 20)
        Me.seria_TB.TabIndex = 7
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(18, 151)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(80, 13)
        Me.Label6.TabIndex = 246
        Me.Label6.Text = "Nume Prenume"
        '
        'nume_TB
        '
        Me.nume_TB.Location = New System.Drawing.Point(104, 144)
        Me.nume_TB.Name = "nume_TB"
        Me.nume_TB.Size = New System.Drawing.Size(133, 20)
        Me.nume_TB.TabIndex = 6
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(130, 173)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(18, 13)
        Me.Label12.TabIndex = 252
        Me.Label12.Text = "Nr"
        '
        'nr_CI_TB
        '
        Me.nr_CI_TB.Location = New System.Drawing.Point(149, 170)
        Me.nr_CI_TB.Name = "nr_CI_TB"
        Me.nr_CI_TB.Size = New System.Drawing.Size(88, 20)
        Me.nr_CI_TB.TabIndex = 8
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(56, 199)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(42, 13)
        Me.Label13.TabIndex = 250
        Me.Label13.Text = "Eliberat"
        '
        'eliber_TB
        '
        Me.eliber_TB.Location = New System.Drawing.Point(103, 193)
        Me.eliber_TB.Mask = "00/00/0000"
        Me.eliber_TB.Name = "eliber_TB"
        Me.eliber_TB.Size = New System.Drawing.Size(100, 20)
        Me.eliber_TB.TabIndex = 9
        Me.eliber_TB.ValidatingType = GetType(Date)
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.Location = New System.Drawing.Point(473, 38)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(195, 12)
        Me.PictureBox1.TabIndex = 30
        Me.PictureBox1.TabStop = False
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(260, 196)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(54, 13)
        Me.Label14.TabIndex = 254
        Me.Label14.Text = "Observatii"
        '
        'obs_TB
        '
        Me.obs_TB.Location = New System.Drawing.Point(320, 192)
        Me.obs_TB.Name = "obs_TB"
        Me.obs_TB.Size = New System.Drawing.Size(348, 20)
        Me.obs_TB.TabIndex = 253
        '
        'plata_BU
        '
        Me.plata_BU.Location = New System.Drawing.Point(380, 466)
        Me.plata_BU.Name = "plata_BU"
        Me.plata_BU.Size = New System.Drawing.Size(75, 23)
        Me.plata_BU.TabIndex = 255
        Me.plata_BU.Text = "Plateste"
        Me.plata_BU.UseVisualStyleBackColor = True
        '
        'Form_adauga_factura
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(690, 504)
        Me.Controls.Add(Me.plata_BU)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.obs_TB)
        Me.Controls.Add(Me.eliber_TB)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.nr_CI_TB)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.seria_TB)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.nume_TB)
        Me.Controls.Add(Me.id_Lbl)
        Me.Controls.Add(Me.edit_BU)
        Me.Controls.Add(Me.ro_CB)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.banca_TB)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.cont_TB)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.judet_TB)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.localitate_TB)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.adresa_tb)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.J_firma_TB)
        Me.Controls.Add(Me.Print_BU)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tot_cant_TB)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.f_jur_TB)
        Me.Controls.Add(Me.cif_firma_TB)
        Me.Controls.Add(Me.tot_valoare_TB)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.ComboBox3)
        Me.Controls.Add(Me.save_Bu)
        Me.Controls.Add(Me.data_DTP)
        Me.Controls.Add(Me.TextBox10)
        Me.Controls.Add(Me.TextBox13)
        Me.Controls.Add(Me.TextBox14)
        Me.Controls.Add(Me.TextBox15)
        Me.Controls.Add(Me.TextBox16)
        Me.Controls.Add(Me.firma_TB)
        Me.Controls.Add(Me.nr_fact_TB)
        Me.Name = "Form_adauga_factura"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form_adauga_nir"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents nr_fact_TB As System.Windows.Forms.TextBox
    Friend WithEvents firma_TB As System.Windows.Forms.TextBox
    Friend WithEvents produs_TB_1 As System.Windows.Forms.TextBox
    Friend WithEvents ad_prod_But As System.Windows.Forms.Button
    Friend WithEvents valoare_TB As System.Windows.Forms.TextBox
    Friend WithEvents pret_unitar_TB As System.Windows.Forms.TextBox
    Friend WithEvents cant_TB_1 As System.Windows.Forms.TextBox
    Friend WithEvents buc_TB_1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox10 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox13 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox14 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox15 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox16 As System.Windows.Forms.TextBox
    Friend WithEvents data_DTP As System.Windows.Forms.DateTimePicker
    Friend WithEvents save_Bu As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Friend WithEvents cif_firma_TB As System.Windows.Forms.TextBox
    Friend WithEvents f_jur_TB As System.Windows.Forms.TextBox
    Friend WithEvents chk_1 As System.Windows.Forms.CheckBox
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents crt_Lbl As System.Windows.Forms.Label
    Friend WithEvents tot_cant_TB As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Print_BU As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents J_firma_TB As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents adresa_tb As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents localitate_TB As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents judet_TB As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cont_TB As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents banca_TB As System.Windows.Forms.TextBox
    Friend WithEvents crt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents produs As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents buc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cant As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents pret_unitar As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents valoare As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chk As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents edit As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents del As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents tot_valoare_TB As System.Windows.Forms.TextBox
    Friend WithEvents ro_CB As System.Windows.Forms.CheckBox
    Friend WithEvents edit_BU As System.Windows.Forms.Button
    Friend WithEvents id_Lbl As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents seria_TB As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents nume_TB As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents nr_CI_TB As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents eliber_TB As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents obs_TB As System.Windows.Forms.TextBox
    Friend WithEvents plata_BU As System.Windows.Forms.Button
End Class
