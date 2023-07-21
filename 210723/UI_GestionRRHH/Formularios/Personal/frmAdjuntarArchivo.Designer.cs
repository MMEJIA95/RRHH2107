namespace UI_GestionRRHH.Formularios.Personal
{
    partial class frmAdjuntarArchivo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdjuntarArchivo));
            this.bshijos = new System.Windows.Forms.BindingSource(this.components);
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.gchijos = new DevExpress.XtraGrid.GridControl();
            this.gvhijos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.coldsc_nombrescompletos_hijo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colflg_CERTIFICADOESTUDIOS_documentofam = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHyperLinkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.colflg_DNI_documentofam = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHyperLinkEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.coldsc_notificacion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lbltrabajador = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.divFooter)).BeginInit();
            this.divFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layout_footer)).BeginInit();
            this.layout_footer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bshijos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gchijos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvhijos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbltrabajador)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // divFooter
            // 
            this.divFooter.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.divFooter.Appearance.Options.UseBackColor = true;
            this.divFooter.Location = new System.Drawing.Point(0, 260);
            this.divFooter.Size = new System.Drawing.Size(656, 43);
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
            this.btnCancelar.Location = new System.Drawing.Point(271, 2);
            this.btnCancelar.Visible = false;
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
            this.btnGuardar.Location = new System.Drawing.Point(384, 2);
            this.btnGuardar.Visible = false;
            // 
            // btnAdd
            // 
            this.btnAdd.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAdd.Appearance.Options.UseBackColor = true;
            this.btnAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.ImageOptions.Image")));
            // 
            // layout_footer
            // 
            this.layout_footer.Size = new System.Drawing.Size(656, 41);
            this.layout_footer.Controls.SetChildIndex(this.btnOpcional, 0);
            this.layout_footer.Controls.SetChildIndex(this.btnGuardar, 0);
            this.layout_footer.Controls.SetChildIndex(this.btnCancelar, 0);
            this.layout_footer.Controls.SetChildIndex(this.btnAdicional, 0);
            // 
            // btnOpcional
            // 
            this.btnOpcional.Appearance.BackColor = System.Drawing.Color.Gray;
            this.btnOpcional.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpcional.Appearance.Options.UseBackColor = true;
            this.btnOpcional.Appearance.Options.UseFont = true;
            this.btnOpcional.Location = new System.Drawing.Point(156, 2);
            // 
            // btnAdicional
            // 
            this.btnAdicional.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Warning;
            this.btnAdicional.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdicional.Appearance.Options.UseBackColor = true;
            this.btnAdicional.Appearance.Options.UseFont = true;
            this.btnAdicional.Location = new System.Drawing.Point(499, 2);
            this.btnAdicional.Size = new System.Drawing.Size(145, 26);
            // 
            // bshijos
            // 
            this.bshijos.DataSource = typeof(BE_GestionRRHH.eTrabajador.eInfoFamiliar_Trabajador);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.gchijos);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 38);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(656, 222);
            this.layoutControl1.TabIndex = 2;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gchijos
            // 
            this.gchijos.DataSource = this.bshijos;
            this.gchijos.Location = new System.Drawing.Point(12, 35);
            this.gchijos.MainView = this.gvhijos;
            this.gchijos.Name = "gchijos";
            this.gchijos.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemHyperLinkEdit1,
            this.repositoryItemHyperLinkEdit2});
            this.gchijos.Size = new System.Drawing.Size(632, 175);
            this.gchijos.TabIndex = 4;
            this.gchijos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvhijos});
            // 
            // gvhijos
            // 
            this.gvhijos.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.coldsc_nombrescompletos_hijo,
            this.colflg_CERTIFICADOESTUDIOS_documentofam,
            this.colflg_DNI_documentofam,
            this.coldsc_notificacion});
            this.gvhijos.GridControl = this.gchijos;
            this.gvhijos.Name = "gvhijos";
            this.gvhijos.OptionsView.ShowGroupPanel = false;
            this.gvhijos.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gvhijos_RowCellClick);
            this.gvhijos.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gvhijos_RowStyle);
            this.gvhijos.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvhijos_CustomRowCellEdit);
            this.gvhijos.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvhijos_FocusedRowChanged);
            // 
            // coldsc_nombrescompletos_hijo
            // 
            this.coldsc_nombrescompletos_hijo.Caption = "Nombres Completos";
            this.coldsc_nombrescompletos_hijo.FieldName = "dsc_nombrescompletos_hijo";
            this.coldsc_nombrescompletos_hijo.Name = "coldsc_nombrescompletos_hijo";
            this.coldsc_nombrescompletos_hijo.Visible = true;
            this.coldsc_nombrescompletos_hijo.VisibleIndex = 0;
            this.coldsc_nombrescompletos_hijo.Width = 327;
            // 
            // colflg_CERTIFICADOESTUDIOS_documentofam
            // 
            this.colflg_CERTIFICADOESTUDIOS_documentofam.AppearanceCell.Options.UseTextOptions = true;
            this.colflg_CERTIFICADOESTUDIOS_documentofam.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colflg_CERTIFICADOESTUDIOS_documentofam.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colflg_CERTIFICADOESTUDIOS_documentofam.AppearanceHeader.Options.UseTextOptions = true;
            this.colflg_CERTIFICADOESTUDIOS_documentofam.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colflg_CERTIFICADOESTUDIOS_documentofam.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colflg_CERTIFICADOESTUDIOS_documentofam.Caption = "Cert. estudios";
            this.colflg_CERTIFICADOESTUDIOS_documentofam.ColumnEdit = this.repositoryItemHyperLinkEdit1;
            this.colflg_CERTIFICADOESTUDIOS_documentofam.FieldName = "flg_CERTIFICADOESTUDIOS_documentofam";
            this.colflg_CERTIFICADOESTUDIOS_documentofam.Name = "colflg_CERTIFICADOESTUDIOS_documentofam";
            this.colflg_CERTIFICADOESTUDIOS_documentofam.Visible = true;
            this.colflg_CERTIFICADOESTUDIOS_documentofam.VisibleIndex = 1;
            this.colflg_CERTIFICADOESTUDIOS_documentofam.Width = 116;
            // 
            // repositoryItemHyperLinkEdit1
            // 
            this.repositoryItemHyperLinkEdit1.AutoHeight = false;
            this.repositoryItemHyperLinkEdit1.LinkColor = System.Drawing.Color.Blue;
            this.repositoryItemHyperLinkEdit1.Name = "repositoryItemHyperLinkEdit1";
            this.repositoryItemHyperLinkEdit1.NullText = "jlaa";
            // 
            // colflg_DNI_documentofam
            // 
            this.colflg_DNI_documentofam.AppearanceCell.Options.UseTextOptions = true;
            this.colflg_DNI_documentofam.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colflg_DNI_documentofam.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colflg_DNI_documentofam.AppearanceHeader.Options.UseTextOptions = true;
            this.colflg_DNI_documentofam.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colflg_DNI_documentofam.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colflg_DNI_documentofam.Caption = "Documento de Identidad";
            this.colflg_DNI_documentofam.ColumnEdit = this.repositoryItemHyperLinkEdit2;
            this.colflg_DNI_documentofam.FieldName = "flg_DNI_documentofam";
            this.colflg_DNI_documentofam.Name = "colflg_DNI_documentofam";
            this.colflg_DNI_documentofam.Visible = true;
            this.colflg_DNI_documentofam.VisibleIndex = 2;
            this.colflg_DNI_documentofam.Width = 117;
            // 
            // repositoryItemHyperLinkEdit2
            // 
            this.repositoryItemHyperLinkEdit2.AutoHeight = false;
            this.repositoryItemHyperLinkEdit2.LinkColor = System.Drawing.Color.Blue;
            this.repositoryItemHyperLinkEdit2.Name = "repositoryItemHyperLinkEdit2";
            // 
            // coldsc_notificacion
            // 
            this.coldsc_notificacion.AppearanceCell.Options.UseTextOptions = true;
            this.coldsc_notificacion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.coldsc_notificacion.AppearanceHeader.Options.UseTextOptions = true;
            this.coldsc_notificacion.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.coldsc_notificacion.Caption = " ";
            this.coldsc_notificacion.FieldName = "dsc_notificacion";
            this.coldsc_notificacion.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("coldsc_notificacion.ImageOptions.Image")));
            this.coldsc_notificacion.Name = "coldsc_notificacion";
            this.coldsc_notificacion.OptionsColumn.AllowFocus = false;
            this.coldsc_notificacion.OptionsColumn.AllowMove = false;
            this.coldsc_notificacion.OptionsColumn.FixedWidth = true;
            this.coldsc_notificacion.OptionsColumn.ShowCaption = false;
            this.coldsc_notificacion.Visible = true;
            this.coldsc_notificacion.VisibleIndex = 3;
            this.coldsc_notificacion.Width = 47;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.ContextImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("repositoryItemTextEdit1.ContextImageOptions.Image")));
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.ContextImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("repositoryItemTextEdit2.ContextImageOptions.Image")));
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lbltrabajador,
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(656, 222);
            this.Root.TextVisible = false;
            // 
            // lbltrabajador
            // 
            this.lbltrabajador.AllowHotTrack = false;
            this.lbltrabajador.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltrabajador.AppearanceItemCaption.ForeColor = System.Drawing.Color.Green;
            this.lbltrabajador.AppearanceItemCaption.Options.UseFont = true;
            this.lbltrabajador.AppearanceItemCaption.Options.UseForeColor = true;
            this.lbltrabajador.Location = new System.Drawing.Point(0, 0);
            this.lbltrabajador.Name = "lbltrabajador";
            this.lbltrabajador.Size = new System.Drawing.Size(636, 23);
            this.lbltrabajador.TextSize = new System.Drawing.Size(151, 19);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gchijos;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 23);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(636, 179);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // frmAdjuntarArchivo
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 303);
            this.Controls.Add(this.layoutControl1);
            this.IconOptions.ShowIcon = false;
            this.Name = "frmAdjuntarArchivo";
            this.Text = "Adjuntar Archivo";
            this.TitleForeColor = System.Drawing.Color.Black;
            this.Load += new System.EventHandler(this.frmAdjuntarArchivo_Load);
            this.Controls.SetChildIndex(this.divFooter, 0);
            this.Controls.SetChildIndex(this.layoutControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.divFooter)).EndInit();
            this.divFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layout_footer)).EndInit();
            this.layout_footer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bshijos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gchijos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvhijos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbltrabajador)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource bshijos;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraGrid.GridControl gchijos;
        private DevExpress.XtraGrid.Views.Grid.GridView gvhijos;
        private DevExpress.XtraGrid.Columns.GridColumn coldsc_nombrescompletos_hijo;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        public DevExpress.XtraLayout.SimpleLabelItem lbltrabajador;
        private DevExpress.XtraGrid.Columns.GridColumn coldsc_notificacion;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn colflg_CERTIFICADOESTUDIOS_documentofam;
        private DevExpress.XtraGrid.Columns.GridColumn colflg_DNI_documentofam;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit2;
    }
}