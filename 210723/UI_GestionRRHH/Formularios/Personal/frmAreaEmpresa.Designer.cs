namespace UI_GestionRRHH.Formularios.Personal
{
    partial class frmAreaEmpresa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAreaEmpresa));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.txtCargo = new DevExpress.XtraEditors.TextEdit();
            this.lkpsede_empresa = new DevExpress.XtraEditors.LookUpEdit();
            this.lkpempresa = new DevExpress.XtraEditors.LookUpEdit();
            this.gcListadoArea = new DevExpress.XtraGrid.GridControl();
            this.bsListaArea = new System.Windows.Forms.BindingSource(this.components);
            this.gvListadoArea = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colcod_area = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldsc_area = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldsc_sede_empresa = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnNuevo = new DevExpress.XtraEditors.SimpleButton();
            this.btneliminar = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.divFooter)).BeginInit();
            this.divFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layout_footer)).BeginInit();
            this.layout_footer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCargo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpsede_empresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpempresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcListadoArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsListaArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvListadoArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).BeginInit();
            this.SuspendLayout();
            // 
            // divFooter
            // 
            this.divFooter.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.divFooter.Appearance.Options.UseBackColor = true;
            this.divFooter.Location = new System.Drawing.Point(0, 585);
            this.divFooter.Size = new System.Drawing.Size(767, 43);
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
            this.btnCancelar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCancelar.ImageOptions.Image")));
            this.btnCancelar.Location = new System.Drawing.Point(374, 2);
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
            this.btnGuardar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnGuardar.ImageOptions.Image")));
            this.btnGuardar.Location = new System.Drawing.Point(487, 2);
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
            this.layout_footer.Size = new System.Drawing.Size(767, 41);
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
            this.btnOpcional.Location = new System.Drawing.Point(259, 2);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.txtCargo);
            this.layoutControl1.Controls.Add(this.lkpsede_empresa);
            this.layoutControl1.Controls.Add(this.lkpempresa);
            this.layoutControl1.Controls.Add(this.gcListadoArea);
            this.layoutControl1.Controls.Add(this.btnNuevo);
            this.layoutControl1.Controls.Add(this.btneliminar);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 38);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Padding = new System.Windows.Forms.Padding(6);
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(767, 547);
            this.layoutControl1.TabIndex = 2;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // txtCargo
            // 
            this.txtCargo.Location = new System.Drawing.Point(68, 519);
            this.txtCargo.Name = "txtCargo";
            this.txtCargo.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCargo.Size = new System.Drawing.Size(691, 20);
            this.txtCargo.StyleController = this.layoutControl1;
            this.txtCargo.TabIndex = 8;
            // 
            // lkpsede_empresa
            // 
            this.lkpsede_empresa.Location = new System.Drawing.Point(68, 495);
            this.lkpsede_empresa.Name = "lkpsede_empresa";
            this.lkpsede_empresa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpsede_empresa.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dsc_sede_empresa", "Descripción")});
            this.lkpsede_empresa.Properties.DisplayMember = "dsc_sede_empresa";
            this.lkpsede_empresa.Properties.NullText = "";
            this.lkpsede_empresa.Properties.PopupSizeable = false;
            this.lkpsede_empresa.Properties.ValueMember = "cod_sede_empresa";
            this.lkpsede_empresa.Size = new System.Drawing.Size(691, 20);
            this.lkpsede_empresa.StyleController = this.layoutControl1;
            this.lkpsede_empresa.TabIndex = 7;
            this.lkpsede_empresa.EditValueChanged += new System.EventHandler(this.lkpsede_empresa_EditValueChanged);
            // 
            // lkpempresa
            // 
            this.lkpempresa.Location = new System.Drawing.Point(68, 471);
            this.lkpempresa.Name = "lkpempresa";
            this.lkpempresa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpempresa.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dsc_empresa", "Descripción")});
            this.lkpempresa.Properties.DisplayMember = "dsc_empresa";
            this.lkpempresa.Properties.NullText = "";
            this.lkpempresa.Properties.PopupSizeable = false;
            this.lkpempresa.Properties.ValueMember = "cod_empresa";
            this.lkpempresa.Size = new System.Drawing.Size(691, 20);
            this.lkpempresa.StyleController = this.layoutControl1;
            this.lkpempresa.TabIndex = 6;
            this.lkpempresa.EditValueChanged += new System.EventHandler(this.lkpempresa_EditValueChanged);
            // 
            // gcListadoArea
            // 
            this.gcListadoArea.DataSource = this.bsListaArea;
            this.gcListadoArea.Location = new System.Drawing.Point(8, 48);
            this.gcListadoArea.MainView = this.gvListadoArea;
            this.gcListadoArea.Name = "gcListadoArea";
            this.gcListadoArea.Size = new System.Drawing.Size(751, 400);
            this.gcListadoArea.TabIndex = 5;
            this.gcListadoArea.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvListadoArea});
            this.gcListadoArea.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gcListadoArea_KeyDown);
            // 
            // bsListaArea
            // 
            this.bsListaArea.DataSource = typeof(BE_GestionRRHH.eTrabajador.eArea);
            // 
            // gvListadoArea
            // 
            this.gvListadoArea.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvListadoArea.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvListadoArea.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvListadoArea.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvListadoArea.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvListadoArea.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colcod_area,
            this.coldsc_area,
            this.coldsc_sede_empresa});
            this.gvListadoArea.GridControl = this.gcListadoArea;
            this.gvListadoArea.GroupCount = 1;
            this.gvListadoArea.Name = "gvListadoArea";
            this.gvListadoArea.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvListadoArea.OptionsBehavior.Editable = false;
            this.gvListadoArea.OptionsView.ShowAutoFilterRow = true;
            this.gvListadoArea.OptionsView.ShowGroupPanel = false;
            this.gvListadoArea.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.coldsc_sede_empresa, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gvListadoArea.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvListadoArea_RowClick);
            this.gvListadoArea.CustomDrawColumnHeader += new DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventHandler(this.gvListadoArea_CustomDrawColumnHeader);
            this.gvListadoArea.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gvListadoArea_RowStyle);
            this.gvListadoArea.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvListadoArea_FocusedRowChanged);
            // 
            // colcod_area
            // 
            this.colcod_area.FieldName = "cod_area";
            this.colcod_area.Name = "colcod_area";
            // 
            // coldsc_area
            // 
            this.coldsc_area.Caption = "Area";
            this.coldsc_area.FieldName = "dsc_area";
            this.coldsc_area.Name = "coldsc_area";
            this.coldsc_area.OptionsColumn.AllowEdit = false;
            this.coldsc_area.Visible = true;
            this.coldsc_area.VisibleIndex = 0;
            this.coldsc_area.Width = 488;
            // 
            // coldsc_sede_empresa
            // 
            this.coldsc_sede_empresa.Caption = "Sede Empresa";
            this.coldsc_sede_empresa.FieldName = "dsc_sede_empresa";
            this.coldsc_sede_empresa.Name = "coldsc_sede_empresa";
            this.coldsc_sede_empresa.OptionsColumn.AllowEdit = false;
            this.coldsc_sede_empresa.Visible = true;
            this.coldsc_sede_empresa.VisibleIndex = 0;
            this.coldsc_sede_empresa.Width = 162;
            // 
            // btnNuevo
            // 
            this.btnNuevo.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnNuevo.Appearance.Options.UseFont = true;
            this.btnNuevo.ImageOptions.Image = global::UI_GestionRRHH.Properties.Resources.add_32x321;
            this.btnNuevo.Location = new System.Drawing.Point(8, 8);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnNuevo.Size = new System.Drawing.Size(87, 36);
            this.btnNuevo.StyleController = this.layoutControl1;
            this.btnNuevo.TabIndex = 9;
            this.btnNuevo.Text = "NUEVO";
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btneliminar
            // 
            this.btneliminar.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btneliminar.Appearance.Options.UseFont = true;
            this.btneliminar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btneliminar.ImageOptions.Image")));
            this.btneliminar.Location = new System.Drawing.Point(99, 8);
            this.btneliminar.Name = "btneliminar";
            this.btneliminar.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btneliminar.Size = new System.Drawing.Size(115, 36);
            this.btneliminar.StyleController = this.layoutControl1;
            this.btneliminar.TabIndex = 10;
            this.btneliminar.Text = "ELIMINAR";
            this.btneliminar.Visible = false;
            this.btneliminar.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.emptySpaceItem1,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.emptySpaceItem5});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.Root.Size = new System.Drawing.Size(767, 547);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gcListadoArea;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 40);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(755, 404);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.lkpempresa;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 463);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(755, 24);
            this.layoutControlItem2.Text = "Empresa :";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(48, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.lkpsede_empresa;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 487);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(755, 24);
            this.layoutControlItem3.Text = "Sede :";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(48, 13);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 444);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(755, 19);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.txtCargo;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 511);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(755, 24);
            this.layoutControlItem4.Text = "Área :";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(48, 13);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btnNuevo;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(91, 40);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btneliminar;
            this.layoutControlItem6.Location = new System.Drawing.Point(91, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(119, 40);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // emptySpaceItem5
            // 
            this.emptySpaceItem5.AllowHotTrack = false;
            this.emptySpaceItem5.Location = new System.Drawing.Point(210, 0);
            this.emptySpaceItem5.Name = "emptySpaceItem5";
            this.emptySpaceItem5.Size = new System.Drawing.Size(545, 40);
            this.emptySpaceItem5.TextSize = new System.Drawing.Size(0, 0);
            // 
            // frmAreaEmpresa
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 628);
            this.Controls.Add(this.layoutControl1);
            this.Name = "frmAreaEmpresa";
            this.Text = "Registro de Area";
            this.TitleForeColor = System.Drawing.Color.Black;
            this.Load += new System.EventHandler(this.frmAreaEmpresa_Load);
            this.Controls.SetChildIndex(this.divFooter, 0);
            this.Controls.SetChildIndex(this.layoutControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.divFooter)).EndInit();
            this.divFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layout_footer)).EndInit();
            this.layout_footer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCargo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpsede_empresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpempresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcListadoArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsListaArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvListadoArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraGrid.GridControl gcListadoArea;
        private DevExpress.XtraGrid.Views.Grid.GridView gvListadoArea;
        private DevExpress.XtraGrid.Columns.GridColumn colcod_area;
        private DevExpress.XtraGrid.Columns.GridColumn coldsc_area;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.LookUpEdit lkpempresa;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.LookUpEdit lkpsede_empresa;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.TextEdit txtCargo;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.BindingSource bsListaArea;
        private DevExpress.XtraEditors.SimpleButton btnNuevo;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem5;
        private DevExpress.XtraGrid.Columns.GridColumn coldsc_sede_empresa;
        private DevExpress.XtraEditors.SimpleButton btneliminar;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
    }
}