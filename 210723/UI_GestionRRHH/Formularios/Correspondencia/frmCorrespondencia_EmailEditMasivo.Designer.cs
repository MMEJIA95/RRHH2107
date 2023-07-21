namespace UI_GestionRRHH.Formularios.Correspondencia
{
    partial class frmCorrespondencia_EmailEditMasivo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCorrespondencia_EmailEditMasivo));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnValidacion = new DevExpress.XtraEditors.SimpleButton();
            this.gcListadoPlantilla = new DevExpress.XtraGrid.GridControl();
            this.bsListadoPlantilla = new System.Windows.Forms.BindingSource(this.components);
            this.gvListadoPlantilla = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.coldsc_documento = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldsc_nombres = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldsc_mail_1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldsc_mail_2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colflg_resultado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colflg_existe = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtPlantilla = new DevExpress.XtraEditors.TextEdit();
            this.btnCoreoErrado = new DevExpress.XtraEditors.SimpleButton();
            this.btnExportarNoRegistrados = new DevExpress.XtraEditors.SimpleButton();
            this.btnPlantillaImportar = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.pnlAyudaPlantilla = new DevExpress.XtraEditors.PanelControl();
            this.btnCloseAvance = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.divFooter)).BeginInit();
            this.divFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layout_footer)).BeginInit();
            this.layout_footer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcListadoPlantilla)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsListadoPlantilla)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvListadoPlantilla)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlantilla.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAyudaPlantilla)).BeginInit();
            this.pnlAyudaPlantilla.SuspendLayout();
            this.SuspendLayout();
            // 
            // divFooter
            // 
            this.divFooter.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.divFooter.Appearance.Options.UseBackColor = true;
            this.divFooter.Location = new System.Drawing.Point(0, 425);
            this.divFooter.Size = new System.Drawing.Size(968, 43);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger;
            this.btnCancelar.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnCancelar.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.Appearance.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.btnCancelar.Appearance.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Appearance.Options.UseBackColor = true;
            this.btnCancelar.Appearance.Options.UseBorderColor = true;
            this.btnCancelar.Appearance.Options.UseFont = true;
            this.btnCancelar.Appearance.Options.UseForeColor = true;
            this.btnCancelar.ImageOptions.Image = global::UI_GestionRRHH.Properties.Resources.cancel_22px;
            this.btnCancelar.Location = new System.Drawing.Point(442, 2);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.btnGuardar.Appearance.BorderColor = System.Drawing.Color.Lime;
            this.btnGuardar.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.Appearance.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.btnGuardar.Appearance.Options.UseBackColor = true;
            this.btnGuardar.Appearance.Options.UseBorderColor = true;
            this.btnGuardar.Appearance.Options.UseFont = true;
            this.btnGuardar.ImageOptions.Image = global::UI_GestionRRHH.Properties.Resources.save_22px;
            this.btnGuardar.Location = new System.Drawing.Point(555, 2);
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAdd.Appearance.Options.UseBackColor = true;
            this.btnAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.ImageOptions.Image")));
            // 
            // layout_footer
            // 
            this.layout_footer.Size = new System.Drawing.Size(968, 41);
            this.layout_footer.Controls.SetChildIndex(this.btnOpcional, 0);
            this.layout_footer.Controls.SetChildIndex(this.btnGuardar, 0);
            this.layout_footer.Controls.SetChildIndex(this.btnCancelar, 0);
            // 
            // btnOpcional
            // 
            this.btnOpcional.Appearance.BackColor = System.Drawing.Color.Gray;
            this.btnOpcional.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpcional.Appearance.Options.UseBackColor = true;
            this.btnOpcional.Appearance.Options.UseFont = true;
            this.btnOpcional.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnOpcional.ImageOptions.Image")));
            this.btnOpcional.Location = new System.Drawing.Point(327, 2);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.lblProgress);
            this.layoutControl1.Controls.Add(this.btnValidacion);
            this.layoutControl1.Controls.Add(this.gcListadoPlantilla);
            this.layoutControl1.Controls.Add(this.txtPlantilla);
            this.layoutControl1.Controls.Add(this.btnCoreoErrado);
            this.layoutControl1.Controls.Add(this.btnExportarNoRegistrados);
            this.layoutControl1.Controls.Add(this.btnPlantillaImportar);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 34);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(968, 391);
            this.layoutControl1.TabIndex = 2;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // lblProgress
            // 
            this.lblProgress.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblProgress.Location = new System.Drawing.Point(160, 339);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(557, 24);
            this.lblProgress.TabIndex = 12;
            this.lblProgress.Text = "0% Completado";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnValidacion
            // 
            this.btnValidacion.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidacion.Appearance.Options.UseFont = true;
            this.btnValidacion.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnValidacion.ImageOptions.Image")));
            this.btnValidacion.Location = new System.Drawing.Point(8, 339);
            this.btnValidacion.Name = "btnValidacion";
            this.btnValidacion.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnValidacion.Size = new System.Drawing.Size(148, 22);
            this.btnValidacion.StyleController = this.layoutControl1;
            this.btnValidacion.TabIndex = 11;
            this.btnValidacion.Text = "Validar Atributos";
            this.btnValidacion.Click += new System.EventHandler(this.btnValidacion_Click);
            // 
            // gcListadoPlantilla
            // 
            this.gcListadoPlantilla.DataSource = this.bsListadoPlantilla;
            this.gcListadoPlantilla.Location = new System.Drawing.Point(8, 36);
            this.gcListadoPlantilla.MainView = this.gvListadoPlantilla;
            this.gcListadoPlantilla.Name = "gcListadoPlantilla";
            this.gcListadoPlantilla.Size = new System.Drawing.Size(952, 299);
            this.gcListadoPlantilla.TabIndex = 10;
            this.gcListadoPlantilla.UseEmbeddedNavigator = true;
            this.gcListadoPlantilla.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvListadoPlantilla});
            // 
            // bsListadoPlantilla
            // 
            this.bsListadoPlantilla.DataSource = typeof(BE_GestionRRHH.eTrabajadorMatch);
            // 
            // gvListadoPlantilla
            // 
            this.gvListadoPlantilla.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvListadoPlantilla.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvListadoPlantilla.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvListadoPlantilla.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvListadoPlantilla.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvListadoPlantilla.ColumnPanelRowHeight = 35;
            this.gvListadoPlantilla.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.coldsc_documento,
            this.coldsc_nombres,
            this.coldsc_mail_1,
            this.coldsc_mail_2,
            this.colflg_resultado,
            this.colflg_existe});
            this.gvListadoPlantilla.GridControl = this.gcListadoPlantilla;
            this.gvListadoPlantilla.Name = "gvListadoPlantilla";
            this.gvListadoPlantilla.OptionsBehavior.Editable = false;
            this.gvListadoPlantilla.OptionsView.EnableAppearanceEvenRow = true;
            this.gvListadoPlantilla.OptionsView.ShowAutoFilterRow = true;
            this.gvListadoPlantilla.OptionsView.ShowGroupPanel = false;
            this.gvListadoPlantilla.OptionsView.ShowIndicator = false;
            this.gvListadoPlantilla.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gvListadoPlantilla_CustomDrawCell);
            // 
            // coldsc_documento
            // 
            this.coldsc_documento.Caption = "Documento";
            this.coldsc_documento.FieldName = "dsc_documento";
            this.coldsc_documento.Name = "coldsc_documento";
            this.coldsc_documento.OptionsColumn.AllowEdit = false;
            this.coldsc_documento.Visible = true;
            this.coldsc_documento.VisibleIndex = 0;
            this.coldsc_documento.Width = 81;
            // 
            // coldsc_nombres
            // 
            this.coldsc_nombres.Caption = "Nombres";
            this.coldsc_nombres.FieldName = "dsc_nombres";
            this.coldsc_nombres.Name = "coldsc_nombres";
            this.coldsc_nombres.OptionsColumn.AllowEdit = false;
            this.coldsc_nombres.Visible = true;
            this.coldsc_nombres.VisibleIndex = 1;
            this.coldsc_nombres.Width = 257;
            // 
            // coldsc_mail_1
            // 
            this.coldsc_mail_1.Caption = "Email (Anterior)";
            this.coldsc_mail_1.FieldName = "dsc_mail_1";
            this.coldsc_mail_1.Name = "coldsc_mail_1";
            this.coldsc_mail_1.OptionsColumn.AllowEdit = false;
            this.coldsc_mail_1.Visible = true;
            this.coldsc_mail_1.VisibleIndex = 2;
            this.coldsc_mail_1.Width = 225;
            // 
            // coldsc_mail_2
            // 
            this.coldsc_mail_2.Caption = "Email (Nuevo)";
            this.coldsc_mail_2.FieldName = "dsc_mail_2";
            this.coldsc_mail_2.Name = "coldsc_mail_2";
            this.coldsc_mail_2.OptionsColumn.AllowEdit = false;
            this.coldsc_mail_2.Visible = true;
            this.coldsc_mail_2.VisibleIndex = 3;
            this.coldsc_mail_2.Width = 225;
            // 
            // colflg_resultado
            // 
            this.colflg_resultado.AppearanceCell.Options.UseTextOptions = true;
            this.colflg_resultado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colflg_resultado.AppearanceHeader.Options.UseTextOptions = true;
            this.colflg_resultado.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colflg_resultado.Caption = "Error (email)";
            this.colflg_resultado.FieldName = "flg_resultado";
            this.colflg_resultado.Name = "colflg_resultado";
            this.colflg_resultado.OptionsColumn.AllowEdit = false;
            this.colflg_resultado.OptionsColumn.AllowSize = false;
            this.colflg_resultado.Visible = true;
            this.colflg_resultado.VisibleIndex = 4;
            this.colflg_resultado.Width = 84;
            // 
            // colflg_existe
            // 
            this.colflg_existe.Caption = "Existe";
            this.colflg_existe.FieldName = "flg_existe";
            this.colflg_existe.Name = "colflg_existe";
            this.colflg_existe.OptionsColumn.AllowEdit = false;
            this.colflg_existe.OptionsColumn.AllowSize = false;
            this.colflg_existe.Visible = true;
            this.colflg_existe.VisibleIndex = 5;
            this.colflg_existe.Width = 78;
            // 
            // txtPlantilla
            // 
            this.txtPlantilla.EditValue = "Buscar plantilla";
            this.txtPlantilla.Location = new System.Drawing.Point(8, 8);
            this.txtPlantilla.Name = "txtPlantilla";
            this.txtPlantilla.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtPlantilla.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.txtPlantilla.Properties.Appearance.Options.UseBackColor = true;
            this.txtPlantilla.Properties.Appearance.Options.UseFont = true;
            this.txtPlantilla.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtPlantilla.Properties.ReadOnly = true;
            this.txtPlantilla.Size = new System.Drawing.Size(873, 22);
            this.txtPlantilla.StyleController = this.layoutControl1;
            this.txtPlantilla.TabIndex = 5;
            this.txtPlantilla.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtPlantilla_MouseClick);
            // 
            // btnCoreoErrado
            // 
            this.btnCoreoErrado.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCoreoErrado.Appearance.Options.UseFont = true;
            this.btnCoreoErrado.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCoreoErrado.ImageOptions.Image")));
            this.btnCoreoErrado.Location = new System.Drawing.Point(840, 339);
            this.btnCoreoErrado.Name = "btnCoreoErrado";
            this.btnCoreoErrado.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnCoreoErrado.Size = new System.Drawing.Size(120, 24);
            this.btnCoreoErrado.StyleController = this.layoutControl1;
            this.btnCoreoErrado.TabIndex = 14;
            this.btnCoreoErrado.Text = "Correos Errados";
            this.btnCoreoErrado.Click += new System.EventHandler(this.btnCoreoErrado_Click);
            // 
            // btnExportarNoRegistrados
            // 
            this.btnExportarNoRegistrados.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportarNoRegistrados.Appearance.Options.UseFont = true;
            this.btnExportarNoRegistrados.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnExportarNoRegistrados.ImageOptions.Image")));
            this.btnExportarNoRegistrados.Location = new System.Drawing.Point(721, 339);
            this.btnExportarNoRegistrados.Name = "btnExportarNoRegistrados";
            this.btnExportarNoRegistrados.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnExportarNoRegistrados.Size = new System.Drawing.Size(115, 24);
            this.btnExportarNoRegistrados.StyleController = this.layoutControl1;
            this.btnExportarNoRegistrados.TabIndex = 15;
            this.btnExportarNoRegistrados.Text = "No Registrados";
            this.btnExportarNoRegistrados.Click += new System.EventHandler(this.btnExportarNoRegistrados_Click);
            // 
            // btnPlantillaImportar
            // 
            this.btnPlantillaImportar.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlantillaImportar.Appearance.Options.UseFont = true;
            this.btnPlantillaImportar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnPlantillaImportar.ImageOptions.Image")));
            this.btnPlantillaImportar.Location = new System.Drawing.Point(885, 8);
            this.btnPlantillaImportar.Name = "btnPlantillaImportar";
            this.btnPlantillaImportar.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnPlantillaImportar.Size = new System.Drawing.Size(75, 24);
            this.btnPlantillaImportar.StyleController = this.layoutControl1;
            this.btnPlantillaImportar.TabIndex = 16;
            this.btnPlantillaImportar.Text = "Plantilla";
            this.btnPlantillaImportar.Click += new System.EventHandler(this.btnPlantillaImportar_Click);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.simpleLabelItem1,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem5});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.Root.Size = new System.Drawing.Size(968, 391);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txtPlantilla;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(877, 28);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.gcListadoPlantilla;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 28);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(956, 303);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btnValidacion;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 331);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(152, 26);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(152, 26);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(152, 28);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.lblProgress;
            this.layoutControlItem4.Location = new System.Drawing.Point(152, 331);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(561, 28);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // simpleLabelItem1
            // 
            this.simpleLabelItem1.AllowHotTrack = false;
            this.simpleLabelItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 10F);
            this.simpleLabelItem1.AppearanceItemCaption.Options.UseFont = true;
            this.simpleLabelItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleLabelItem1.ImageOptions.Image")));
            this.simpleLabelItem1.Location = new System.Drawing.Point(0, 359);
            this.simpleLabelItem1.Name = "simpleLabelItem1";
            this.simpleLabelItem1.Size = new System.Drawing.Size(956, 20);
            this.simpleLabelItem1.Text = "Nota: Columna (Existe) semáforo rojo; trabajador no registrado, Columna (Error) s" +
    "emáforo rojo; correo errado.";
            this.simpleLabelItem1.TextSize = new System.Drawing.Size(665, 16);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btnCoreoErrado;
            this.layoutControlItem6.Location = new System.Drawing.Point(832, 331);
            this.layoutControlItem6.MaxSize = new System.Drawing.Size(124, 28);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(124, 28);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(124, 28);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btnExportarNoRegistrados;
            this.layoutControlItem7.Location = new System.Drawing.Point(713, 331);
            this.layoutControlItem7.MaxSize = new System.Drawing.Size(119, 28);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(119, 28);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(119, 28);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btnPlantillaImportar;
            this.layoutControlItem5.Location = new System.Drawing.Point(877, 0);
            this.layoutControlItem5.MaxSize = new System.Drawing.Size(79, 28);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(79, 28);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(79, 28);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // pnlAyudaPlantilla
            // 
            this.pnlAyudaPlantilla.Controls.Add(this.btnCloseAvance);
            this.pnlAyudaPlantilla.Controls.Add(this.labelControl8);
            this.pnlAyudaPlantilla.Controls.Add(this.labelControl5);
            this.pnlAyudaPlantilla.Controls.Add(this.labelControl7);
            this.pnlAyudaPlantilla.Controls.Add(this.labelControl4);
            this.pnlAyudaPlantilla.Controls.Add(this.labelControl6);
            this.pnlAyudaPlantilla.Controls.Add(this.labelControl3);
            this.pnlAyudaPlantilla.Controls.Add(this.labelControl2);
            this.pnlAyudaPlantilla.Location = new System.Drawing.Point(273, 191);
            this.pnlAyudaPlantilla.Name = "pnlAyudaPlantilla";
            this.pnlAyudaPlantilla.Size = new System.Drawing.Size(422, 100);
            this.pnlAyudaPlantilla.TabIndex = 3;
            this.pnlAyudaPlantilla.Visible = false;
            // 
            // btnCloseAvance
            // 
            this.btnCloseAvance.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger;
            this.btnCloseAvance.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnCloseAvance.Appearance.Options.UseBackColor = true;
            this.btnCloseAvance.Appearance.Options.UseFont = true;
            this.btnCloseAvance.Location = new System.Drawing.Point(386, 5);
            this.btnCloseAvance.Name = "btnCloseAvance";
            this.btnCloseAvance.Size = new System.Drawing.Size(31, 22);
            this.btnCloseAvance.TabIndex = 9;
            this.btnCloseAvance.Text = "X";
            this.btnCloseAvance.Click += new System.EventHandler(this.btnCloseAvance_Click);
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.BorderColor = System.Drawing.Color.Silver;
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl8.Appearance.Options.UseBorderColor = true;
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl8.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.labelControl8.Location = new System.Drawing.Point(295, 73);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(102, 22);
            this.labelControl8.TabIndex = 0;
            this.labelControl8.Text = " abc@mail.com";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl5.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.labelControl5.Location = new System.Drawing.Point(295, 48);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(102, 22);
            this.labelControl5.TabIndex = 0;
            this.labelControl5.Text = " EMAIL";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.BorderColor = System.Drawing.Color.Silver;
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl7.Appearance.Options.UseBorderColor = true;
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl7.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.labelControl7.Location = new System.Drawing.Point(132, 73);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(164, 22);
            this.labelControl7.TabIndex = 0;
            this.labelControl7.Text = " JUAN PEREZ";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.labelControl4.Location = new System.Drawing.Point(132, 48);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(164, 22);
            this.labelControl4.TabIndex = 0;
            this.labelControl4.Text = " NOMBRE_TRABAJADOR";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.BorderColor = System.Drawing.Color.Silver;
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl6.Appearance.Options.UseBorderColor = true;
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl6.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.labelControl6.Location = new System.Drawing.Point(25, 73);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(108, 22);
            this.labelControl6.TabIndex = 0;
            this.labelControl6.Text = " 00000001";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.labelControl3.Location = new System.Drawing.Point(25, 48);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(108, 22);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = " N°.DOCUMENTO";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Options.UseTextOptions = true;
            this.labelControl2.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl2.ImageOptions.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelControl2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("labelControl2.ImageOptions.Image")));
            this.labelControl2.Location = new System.Drawing.Point(2, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(418, 40);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "           Nota: El archivo Excel debe contener 3 columnas en el siguiente orden." +
    "\r\n           Documento, Nombre del trabajador e Email.";
            // 
            // frmCorrespondencia_EmailEditMasivo
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 468);
            this.Controls.Add(this.pnlAyudaPlantilla);
            this.Controls.Add(this.layoutControl1);
            this.Name = "frmCorrespondencia_EmailEditMasivo";
            this.Text = "frmCorrespondencia_EmailEditMasivo";
            this.TitleForeColor = System.Drawing.Color.Black;
            this.Load += new System.EventHandler(this.frmCorrespondencia_EmailEditMasivo_Load);
            this.Controls.SetChildIndex(this.divFooter, 0);
            this.Controls.SetChildIndex(this.layoutControl1, 0);
            this.Controls.SetChildIndex(this.pnlAyudaPlantilla, 0);
            ((System.ComponentModel.ISupportInitialize)(this.divFooter)).EndInit();
            this.divFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layout_footer)).EndInit();
            this.layout_footer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcListadoPlantilla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsListadoPlantilla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvListadoPlantilla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlantilla.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAyudaPlantilla)).EndInit();
            this.pnlAyudaPlantilla.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.TextEdit txtPlantilla;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.GridControl gcListadoPlantilla;
        private DevExpress.XtraGrid.Views.Grid.GridView gvListadoPlantilla;
        private DevExpress.XtraGrid.Columns.GridColumn coldsc_documento;
        private DevExpress.XtraGrid.Columns.GridColumn coldsc_nombres;
        private DevExpress.XtraGrid.Columns.GridColumn coldsc_mail_1;
        private DevExpress.XtraGrid.Columns.GridColumn coldsc_mail_2;
        private DevExpress.XtraGrid.Columns.GridColumn colflg_resultado;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.SimpleButton btnValidacion;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private System.Windows.Forms.Label lblProgress;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.BindingSource bsListadoPlantilla;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem1;
        private DevExpress.XtraGrid.Columns.GridColumn colflg_existe;
        private DevExpress.XtraEditors.SimpleButton btnCoreoErrado;
        private DevExpress.XtraEditors.SimpleButton btnExportarNoRegistrados;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraEditors.SimpleButton btnPlantillaImportar;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.PanelControl pnlAyudaPlantilla;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnCloseAvance;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl6;
    }
}