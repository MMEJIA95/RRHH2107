namespace UI_GestionRRHH.Formularios.Shared
{
    partial class HeaderForm
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
            this._cusTitle = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // _cusTitle
            // 
            this._cusTitle.Appearance.BackColor = System.Drawing.Color.Silver;
            this._cusTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this._cusTitle.Appearance.ForeColor = System.Drawing.Color.White;
            this._cusTitle.Appearance.Options.UseBackColor = true;
            this._cusTitle.Appearance.Options.UseFont = true;
            this._cusTitle.Appearance.Options.UseForeColor = true;
            this._cusTitle.Appearance.Options.UseTextOptions = true;
            this._cusTitle.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this._cusTitle.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this._cusTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this._cusTitle.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this._cusTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this._cusTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this._cusTitle.Location = new System.Drawing.Point(0, 0);
            this._cusTitle.Name = "_cusTitle";
            this._cusTitle.Size = new System.Drawing.Size(419, 38);
            this._cusTitle.TabIndex = 2;
            this._cusTitle.Text = "Vinculación de Formatos";
            this._cusTitle.Click += new System.EventHandler(this._cusTitle_Click);
            this._cusTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this._cusTitle_MouseDown);
            // 
            // HeaderForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._cusTitle);
            this.Name = "HeaderForm";
            this.Size = new System.Drawing.Size(419, 38);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HeaderForm_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl _cusTitle;
    }
}
