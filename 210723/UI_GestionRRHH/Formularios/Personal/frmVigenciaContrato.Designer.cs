
namespace UI_GestionRRHH.Formularios.Personal
{
    partial class frmVigenciaContrato
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.bsTrabajador = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.gcTrabajador = new DevExpress.XtraGrid.GridControl();
            this.gvTrabajador = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colcod_trabajador = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldsc_nombres_completos = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcod_sede_empresa = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rslkpsedeempresa = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colcod_area = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rslkparea = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colcod_cargo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rslkpcargo = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colflg_afectoSCTR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rsLkpsino = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colflg_AfectoVidaLey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colfch_firma = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rsfch_fechas = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.colfch_vencimiento = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colimp_sueldo_base = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rstxtImporte = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.colimp_movilidad = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colimp_alimentacion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colimp_escolaridad = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colflg_asignacionfamiliar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colimp_asig_familiar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcod_empresa = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rlkpCECO = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.divFooter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTrabajador)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
            this.layoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTrabajador)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTrabajador)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rslkpsedeempresa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rslkparea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rslkpcargo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rsLkpsino)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rsfch_fechas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rsfch_fechas.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rstxtImporte)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlkpCECO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // divFooter
            // 
            this.divFooter.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.divFooter.Appearance.Options.UseBackColor = true;
            this.divFooter.Location = new System.Drawing.Point(0, 570);
            this.divFooter.Size = new System.Drawing.Size(1270, 10);
            // 
            // bsTrabajador
            // 
            this.bsTrabajador.DataSource = typeof(BE_GestionRRHH.eTrabajador.eInfoLaboral_Trabajador);
            // 
            // gridView1
            // 
            this.gridView1.Name = "gridView1";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.layoutControl2);
            this.layoutControl1.Controls.Add(this.gcTrabajador);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 34);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1270, 536);
            this.layoutControl1.TabIndex = 3;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // layoutControl2
            // 
            this.layoutControl2.Controls.Add(this.simpleButton1);
            this.layoutControl2.Location = new System.Drawing.Point(6, 495);
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.Root = this.layoutControlGroup1;
            this.layoutControl2.Size = new System.Drawing.Size(1258, 35);
            this.layoutControl2.TabIndex = 1;
            this.layoutControl2.Text = "layoutControl2";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton1.Appearance.Options.UseBackColor = true;
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Location = new System.Drawing.Point(496, 6);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(205, 23);
            this.simpleButton1.StyleController = this.layoutControl2;
            this.simpleButton1.TabIndex = 4;
            this.simpleButton1.Text = "Guardar";
            this.simpleButton1.Click += new System.EventHandler(this.btnguardar_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3,
            this.emptySpaceItem1,
            this.emptySpaceItem2});
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1258, 35);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.simpleButton1;
            this.layoutControlItem3.Location = new System.Drawing.Point(490, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(209, 27);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(490, 27);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(699, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(551, 27);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // gcTrabajador
            // 
            this.gcTrabajador.DataSource = this.bsTrabajador;
            this.gcTrabajador.Location = new System.Drawing.Point(6, 6);
            this.gcTrabajador.MainView = this.gvTrabajador;
            this.gcTrabajador.Name = "gcTrabajador";
            this.gcTrabajador.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rstxtImporte,
            this.rsfch_fechas,
            this.rsLkpsino,
            this.rslkparea,
            this.rslkpcargo,
            this.rslkpsedeempresa,
            this.rlkpCECO});
            this.gcTrabajador.Size = new System.Drawing.Size(1258, 485);
            this.gcTrabajador.TabIndex = 4;
            this.gcTrabajador.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTrabajador});
            this.gcTrabajador.Click += new System.EventHandler(this.gcTrabajador_Click);
            // 
            // gvTrabajador
            // 
            this.gvTrabajador.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvTrabajador.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvTrabajador.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colcod_trabajador,
            this.coldsc_nombres_completos,
            this.colcod_sede_empresa,
            this.colcod_area,
            this.colcod_cargo,
            this.colflg_afectoSCTR,
            this.colflg_AfectoVidaLey,
            this.colfch_firma,
            this.colfch_vencimiento,
            this.colimp_sueldo_base,
            this.colimp_movilidad,
            this.colimp_alimentacion,
            this.colimp_escolaridad,
            this.colflg_asignacionfamiliar,
            this.colimp_asig_familiar,
            this.colcod_empresa});
            this.gvTrabajador.GridControl = this.gcTrabajador;
            this.gvTrabajador.Name = "gvTrabajador";
            this.gvTrabajador.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            this.gvTrabajador.OptionsSelection.MultiSelect = true;
            this.gvTrabajador.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gvTrabajador.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.gvTrabajador.OptionsView.ShowGroupPanel = false;
            this.gvTrabajador.OptionsView.ShowIndicator = false;
            this.gvTrabajador.RowHeight = 15;
            this.gvTrabajador.CustomDrawColumnHeader += new DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventHandler(this.gvTrabajador_CustomDrawColumnHeader);
            this.gvTrabajador.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gvTrabajador_CustomDrawCell);
            this.gvTrabajador.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gvTrabajador_RowStyle);
            this.gvTrabajador.ShownEditor += new System.EventHandler(this.gvTrabajador_ShownEditor);
            this.gvTrabajador.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvTrabajador_CellValueChanged);
            // 
            // colcod_trabajador
            // 
            this.colcod_trabajador.AppearanceHeader.Options.UseTextOptions = true;
            this.colcod_trabajador.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colcod_trabajador.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colcod_trabajador.Caption = "Cod. Trabajador";
            this.colcod_trabajador.FieldName = "cod_trabajador";
            this.colcod_trabajador.Name = "colcod_trabajador";
            this.colcod_trabajador.Visible = true;
            this.colcod_trabajador.VisibleIndex = 1;
            this.colcod_trabajador.Width = 74;
            // 
            // coldsc_nombres_completos
            // 
            this.coldsc_nombres_completos.AppearanceHeader.Options.UseTextOptions = true;
            this.coldsc_nombres_completos.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.coldsc_nombres_completos.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.coldsc_nombres_completos.Caption = "Nombres Completos";
            this.coldsc_nombres_completos.FieldName = "dsc_nombres_completos";
            this.coldsc_nombres_completos.Name = "coldsc_nombres_completos";
            this.coldsc_nombres_completos.Visible = true;
            this.coldsc_nombres_completos.VisibleIndex = 2;
            this.coldsc_nombres_completos.Width = 191;
            // 
            // colcod_sede_empresa
            // 
            this.colcod_sede_empresa.AppearanceHeader.Options.UseTextOptions = true;
            this.colcod_sede_empresa.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colcod_sede_empresa.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colcod_sede_empresa.Caption = "Sede Empresa";
            this.colcod_sede_empresa.ColumnEdit = this.rslkpsedeempresa;
            this.colcod_sede_empresa.FieldName = "cod_sede_empresa";
            this.colcod_sede_empresa.Name = "colcod_sede_empresa";
            this.colcod_sede_empresa.Visible = true;
            this.colcod_sede_empresa.VisibleIndex = 3;
            this.colcod_sede_empresa.Width = 82;
            // 
            // rslkpsedeempresa
            // 
            this.rslkpsedeempresa.AutoHeight = false;
            this.rslkpsedeempresa.AutoSearchColumnIndex = 1;
            this.rslkpsedeempresa.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rslkpsedeempresa.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dsc_sede_empresa", "Descripcion")});
            this.rslkpsedeempresa.DisplayMember = "dsc_sede_empresa";
            this.rslkpsedeempresa.Name = "rslkpsedeempresa";
            this.rslkpsedeempresa.ValueMember = "cod_sede_empresa";
            // 
            // colcod_area
            // 
            this.colcod_area.AppearanceHeader.Options.UseTextOptions = true;
            this.colcod_area.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colcod_area.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colcod_area.Caption = "Area";
            this.colcod_area.ColumnEdit = this.rslkparea;
            this.colcod_area.FieldName = "cod_area";
            this.colcod_area.Name = "colcod_area";
            this.colcod_area.Visible = true;
            this.colcod_area.VisibleIndex = 4;
            this.colcod_area.Width = 82;
            // 
            // rslkparea
            // 
            this.rslkparea.AutoHeight = false;
            this.rslkparea.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rslkparea.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dsc_area", "Descripcion")});
            this.rslkparea.DisplayMember = "dsc_area";
            this.rslkparea.Name = "rslkparea";
            this.rslkparea.ValueMember = "cod_area";
            // 
            // colcod_cargo
            // 
            this.colcod_cargo.AppearanceHeader.Options.UseTextOptions = true;
            this.colcod_cargo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colcod_cargo.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colcod_cargo.Caption = "Cargo";
            this.colcod_cargo.ColumnEdit = this.rslkpcargo;
            this.colcod_cargo.FieldName = "cod_cargo";
            this.colcod_cargo.Name = "colcod_cargo";
            this.colcod_cargo.Visible = true;
            this.colcod_cargo.VisibleIndex = 5;
            this.colcod_cargo.Width = 82;
            // 
            // rslkpcargo
            // 
            this.rslkpcargo.AutoHeight = false;
            this.rslkpcargo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rslkpcargo.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dsc_cargo", "Nombre8")});
            this.rslkpcargo.DisplayMember = "dsc_cargo";
            this.rslkpcargo.Name = "rslkpcargo";
            this.rslkpcargo.ValueMember = "cod_cargo";
            // 
            // colflg_afectoSCTR
            // 
            this.colflg_afectoSCTR.AppearanceHeader.Options.UseTextOptions = true;
            this.colflg_afectoSCTR.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colflg_afectoSCTR.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colflg_afectoSCTR.Caption = "Afecto SCTR";
            this.colflg_afectoSCTR.ColumnEdit = this.rsLkpsino;
            this.colflg_afectoSCTR.FieldName = "flg_afectoSCTR";
            this.colflg_afectoSCTR.Name = "colflg_afectoSCTR";
            this.colflg_afectoSCTR.Visible = true;
            this.colflg_afectoSCTR.VisibleIndex = 6;
            this.colflg_afectoSCTR.Width = 53;
            // 
            // rsLkpsino
            // 
            this.rsLkpsino.AutoHeight = false;
            this.rsLkpsino.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rsLkpsino.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dsc_sino", "Descripcion")});
            this.rsLkpsino.DisplayMember = "dsc_sino";
            this.rsLkpsino.Name = "rsLkpsino";
            this.rsLkpsino.ValueMember = "cod_sino";
            // 
            // colflg_AfectoVidaLey
            // 
            this.colflg_AfectoVidaLey.AppearanceHeader.Options.UseTextOptions = true;
            this.colflg_AfectoVidaLey.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colflg_AfectoVidaLey.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colflg_AfectoVidaLey.Caption = "Afceto Vida Ley";
            this.colflg_AfectoVidaLey.ColumnEdit = this.rsLkpsino;
            this.colflg_AfectoVidaLey.FieldName = "flg_AfectoVidaLey";
            this.colflg_AfectoVidaLey.Name = "colflg_AfectoVidaLey";
            this.colflg_AfectoVidaLey.Visible = true;
            this.colflg_AfectoVidaLey.VisibleIndex = 7;
            this.colflg_AfectoVidaLey.Width = 53;
            // 
            // colfch_firma
            // 
            this.colfch_firma.AppearanceCell.Options.UseTextOptions = true;
            this.colfch_firma.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colfch_firma.AppearanceHeader.Options.UseTextOptions = true;
            this.colfch_firma.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colfch_firma.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colfch_firma.Caption = "Fch. Inicio Contrato";
            this.colfch_firma.ColumnEdit = this.rsfch_fechas;
            this.colfch_firma.FieldName = "fch_firma";
            this.colfch_firma.Name = "colfch_firma";
            this.colfch_firma.Visible = true;
            this.colfch_firma.VisibleIndex = 8;
            this.colfch_firma.Width = 64;
            // 
            // rsfch_fechas
            // 
            this.rsfch_fechas.AutoHeight = false;
            this.rsfch_fechas.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rsfch_fechas.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rsfch_fechas.Name = "rsfch_fechas";
            // 
            // colfch_vencimiento
            // 
            this.colfch_vencimiento.AppearanceCell.Options.UseTextOptions = true;
            this.colfch_vencimiento.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colfch_vencimiento.AppearanceHeader.Options.UseTextOptions = true;
            this.colfch_vencimiento.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colfch_vencimiento.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colfch_vencimiento.Caption = "Fch. Fin Contrato";
            this.colfch_vencimiento.ColumnEdit = this.rsfch_fechas;
            this.colfch_vencimiento.FieldName = "fch_vencimiento";
            this.colfch_vencimiento.Name = "colfch_vencimiento";
            this.colfch_vencimiento.Visible = true;
            this.colfch_vencimiento.VisibleIndex = 9;
            this.colfch_vencimiento.Width = 64;
            // 
            // colimp_sueldo_base
            // 
            this.colimp_sueldo_base.AppearanceCell.Options.UseTextOptions = true;
            this.colimp_sueldo_base.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colimp_sueldo_base.AppearanceHeader.Options.UseTextOptions = true;
            this.colimp_sueldo_base.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colimp_sueldo_base.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colimp_sueldo_base.Caption = "Sueldo Base";
            this.colimp_sueldo_base.ColumnEdit = this.rstxtImporte;
            this.colimp_sueldo_base.FieldName = "imp_sueldo_base";
            this.colimp_sueldo_base.Name = "colimp_sueldo_base";
            this.colimp_sueldo_base.Visible = true;
            this.colimp_sueldo_base.VisibleIndex = 10;
            this.colimp_sueldo_base.Width = 53;
            // 
            // rstxtImporte
            // 
            this.rstxtImporte.AutoHeight = false;
            this.rstxtImporte.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rstxtImporte.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.rstxtImporte.MaskSettings.Set("mask", "n3");
            this.rstxtImporte.MaskSettings.Set("valueType", null);
            this.rstxtImporte.Name = "rstxtImporte";
            this.rstxtImporte.NullText = "0.00";
            // 
            // colimp_movilidad
            // 
            this.colimp_movilidad.AppearanceCell.Options.UseTextOptions = true;
            this.colimp_movilidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colimp_movilidad.AppearanceHeader.Options.UseTextOptions = true;
            this.colimp_movilidad.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colimp_movilidad.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colimp_movilidad.Caption = "Imp. Movilidad";
            this.colimp_movilidad.ColumnEdit = this.rstxtImporte;
            this.colimp_movilidad.FieldName = "imp_movilidad";
            this.colimp_movilidad.Name = "colimp_movilidad";
            this.colimp_movilidad.Visible = true;
            this.colimp_movilidad.VisibleIndex = 11;
            this.colimp_movilidad.Width = 66;
            // 
            // colimp_alimentacion
            // 
            this.colimp_alimentacion.AppearanceCell.Options.UseTextOptions = true;
            this.colimp_alimentacion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colimp_alimentacion.AppearanceHeader.Options.UseTextOptions = true;
            this.colimp_alimentacion.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colimp_alimentacion.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colimp_alimentacion.Caption = "Imp. Alimentación";
            this.colimp_alimentacion.ColumnEdit = this.rstxtImporte;
            this.colimp_alimentacion.FieldName = "imp_alimentacion";
            this.colimp_alimentacion.Name = "colimp_alimentacion";
            this.colimp_alimentacion.Visible = true;
            this.colimp_alimentacion.VisibleIndex = 12;
            this.colimp_alimentacion.Width = 80;
            // 
            // colimp_escolaridad
            // 
            this.colimp_escolaridad.AppearanceCell.Options.UseTextOptions = true;
            this.colimp_escolaridad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colimp_escolaridad.AppearanceHeader.Options.UseTextOptions = true;
            this.colimp_escolaridad.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colimp_escolaridad.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colimp_escolaridad.Caption = "Imp. Escolaridad";
            this.colimp_escolaridad.ColumnEdit = this.rstxtImporte;
            this.colimp_escolaridad.FieldName = "imp_escolaridad";
            this.colimp_escolaridad.Name = "colimp_escolaridad";
            this.colimp_escolaridad.Visible = true;
            this.colimp_escolaridad.VisibleIndex = 13;
            this.colimp_escolaridad.Width = 72;
            // 
            // colflg_asignacionfamiliar
            // 
            this.colflg_asignacionfamiliar.AppearanceCell.Options.UseTextOptions = true;
            this.colflg_asignacionfamiliar.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colflg_asignacionfamiliar.AppearanceHeader.Options.UseTextOptions = true;
            this.colflg_asignacionfamiliar.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colflg_asignacionfamiliar.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colflg_asignacionfamiliar.Caption = "Flg, Asignación Familiar";
            this.colflg_asignacionfamiliar.ColumnEdit = this.rsLkpsino;
            this.colflg_asignacionfamiliar.FieldName = "flg_asignacionfamiliar";
            this.colflg_asignacionfamiliar.Name = "colflg_asignacionfamiliar";
            this.colflg_asignacionfamiliar.Visible = true;
            this.colflg_asignacionfamiliar.VisibleIndex = 14;
            this.colflg_asignacionfamiliar.Width = 72;
            // 
            // colimp_asig_familiar
            // 
            this.colimp_asig_familiar.AppearanceCell.Options.UseTextOptions = true;
            this.colimp_asig_familiar.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colimp_asig_familiar.AppearanceHeader.Options.UseTextOptions = true;
            this.colimp_asig_familiar.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colimp_asig_familiar.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colimp_asig_familiar.Caption = "Imp. Asignación Familiar";
            this.colimp_asig_familiar.ColumnEdit = this.rstxtImporte;
            this.colimp_asig_familiar.FieldName = "imp_asig_familiar";
            this.colimp_asig_familiar.Name = "colimp_asig_familiar";
            this.colimp_asig_familiar.OptionsColumn.AllowEdit = false;
            this.colimp_asig_familiar.Visible = true;
            this.colimp_asig_familiar.VisibleIndex = 15;
            this.colimp_asig_familiar.Width = 100;
            // 
            // colcod_empresa
            // 
            this.colcod_empresa.Caption = "Empresa";
            this.colcod_empresa.FieldName = "cod_empresa";
            this.colcod_empresa.Name = "colcod_empresa";
            // 
            // rlkpCECO
            // 
            this.rlkpCECO.AutoHeight = false;
            this.rlkpCECO.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rlkpCECO.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dsc_ceco", "Descripción")});
            this.rlkpCECO.DisplayMember = "dsc_CECO";
            this.rlkpCECO.Name = "rlkpCECO";
            this.rlkpCECO.ValueMember = "cod_CECO";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.Root.Size = new System.Drawing.Size(1270, 536);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gcTrabajador;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1262, 489);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.layoutControl2;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 489);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(1262, 39);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // frmVigenciaContrato
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.ClientSize = new System.Drawing.Size(1270, 580);
            this.Controls.Add(this.layoutControl1);
            this.Name = "frmVigenciaContrato";
            this.Text = "Renovacion de Vigencia de Contratos";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmVigenciaContrato_Load);
            this.Controls.SetChildIndex(this.divFooter, 0);
            this.Controls.SetChildIndex(this.layoutControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.divFooter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTrabajador)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
            this.layoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTrabajador)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTrabajador)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rslkpsedeempresa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rslkparea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rslkpcargo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rsLkpsino)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rsfch_fechas.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rsfch_fechas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rstxtImporte)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlkpCECO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.BindingSource bsTrabajador;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraGrid.GridControl gcTrabajador;
        private DevExpress.XtraGrid.Views.Grid.GridView gvTrabajador;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Columns.GridColumn colcod_trabajador;
        private DevExpress.XtraGrid.Columns.GridColumn coldsc_nombres_completos;
        private DevExpress.XtraGrid.Columns.GridColumn colcod_sede_empresa;
        private DevExpress.XtraGrid.Columns.GridColumn colcod_area;
        private DevExpress.XtraGrid.Columns.GridColumn colcod_cargo;
        private DevExpress.XtraGrid.Columns.GridColumn colflg_afectoSCTR;
        private DevExpress.XtraGrid.Columns.GridColumn colflg_AfectoVidaLey;
        private DevExpress.XtraGrid.Columns.GridColumn colfch_firma;
        private DevExpress.XtraGrid.Columns.GridColumn colfch_vencimiento;
        private DevExpress.XtraGrid.Columns.GridColumn colimp_sueldo_base;
        private DevExpress.XtraGrid.Columns.GridColumn colimp_movilidad;
        private DevExpress.XtraGrid.Columns.GridColumn colimp_alimentacion;
        private DevExpress.XtraGrid.Columns.GridColumn colimp_escolaridad;
        private DevExpress.XtraGrid.Columns.GridColumn colflg_asignacionfamiliar;
        private DevExpress.XtraGrid.Columns.GridColumn colimp_asig_familiar;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rslkpsedeempresa;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rslkparea;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rslkpcargo;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit rsfch_fechas;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rstxtImporte;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rsLkpsino;
        private DevExpress.XtraGrid.Columns.GridColumn colcod_empresa;
        private DevExpress.XtraLayout.LayoutControl layoutControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rlkpCECO;
    }
}