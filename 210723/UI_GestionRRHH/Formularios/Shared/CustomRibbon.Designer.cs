namespace UI_GestionRRHH.Formularios.Shared
{
    partial class CustomRibbon
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
            this.description = new System.Windows.Forms.Label();
            this.SuspendLayout();
            this.description.AutoSize = false;
            this.description.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.description.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular);
            this.description.Text = "Description";
            this.description.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.description.Name = "description";
            this.description.Height = 14;
            this.description.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
            this.description.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);

            components = new System.ComponentModel.Container();

            this.BackColor = System.Drawing.Color.White;
            this.Size = new System.Drawing.Size(192, 78);
            this.Name = "CustomRibbon";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Label description;
    }
}
