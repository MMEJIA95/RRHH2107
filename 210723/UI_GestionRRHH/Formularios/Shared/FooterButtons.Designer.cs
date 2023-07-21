namespace UI_GestionRRHH.Formularios.Shared
{
    partial class FooterButtons
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FooterButtons));
            this._cusLine = new DevExpress.XtraEditors.LabelControl();
            this._cusSave = new DevExpress.XtraEditors.SimpleButton();
            this._cusCancel = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // _cusLine
            // 
            this._cusLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cusLine.Appearance.BackColor = System.Drawing.Color.Silver;
            this._cusLine.Appearance.Options.UseBackColor = true;
            this._cusLine.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this._cusLine.Location = new System.Drawing.Point(15, 2);
            this._cusLine.Name = "_cusLine";
            this._cusLine.Size = new System.Drawing.Size(266, 3);
            this._cusLine.TabIndex = 0;
            // 
            // _cusSave
            // 
            this._cusSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this._cusSave.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(127)))), ((int)(((byte)(215)))));
            this._cusSave.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(193)))), ((int)(((byte)(247)))));
            this._cusSave.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this._cusSave.Appearance.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this._cusSave.Appearance.Options.UseBackColor = true;
            this._cusSave.Appearance.Options.UseBorderColor = true;
            this._cusSave.Appearance.Options.UseFont = true;
            this._cusSave.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("_cusSave.ImageOptions.Image")));
            this._cusSave.Location = new System.Drawing.Point(153, 11);
            this._cusSave.Name = "_cusSave";
            this._cusSave.Size = new System.Drawing.Size(97, 28);
            this._cusSave.TabIndex = 0;
            this._cusSave.Text = "GUARDAR";
            this._cusSave.Click += new System.EventHandler(this._cusSave_Click);
            // 
            // _cusCancel
            // 
            this._cusCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this._cusCancel.Appearance.BackColor = System.Drawing.Color.Silver;
            this._cusCancel.Appearance.BorderColor = System.Drawing.Color.LightGray;
            this._cusCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this._cusCancel.Appearance.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this._cusCancel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._cusCancel.Appearance.Options.UseBackColor = true;
            this._cusCancel.Appearance.Options.UseBorderColor = true;
            this._cusCancel.Appearance.Options.UseFont = true;
            this._cusCancel.Appearance.Options.UseForeColor = true;
            this._cusCancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("_cusCancel.ImageOptions.Image")));
            this._cusCancel.Location = new System.Drawing.Point(46, 11);
            this._cusCancel.Name = "_cusCancel";
            this._cusCancel.Size = new System.Drawing.Size(101, 28);
            this._cusCancel.TabIndex = 1;
            this._cusCancel.Text = "CANCELAR";
            this._cusCancel.Click += new System.EventHandler(this._cusCancel_Click);
            // 
            // FooterButtons
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._cusCancel);
            this.Controls.Add(this._cusSave);
            this.Controls.Add(this._cusLine);
            this.Name = "FooterButtons";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(296, 52);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FooterButtons_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl _cusLine;
        public DevExpress.XtraEditors.SimpleButton _cusSave;
        public DevExpress.XtraEditors.SimpleButton _cusCancel;
    }
}
