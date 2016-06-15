namespace dongle
{
	partial class Versions
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
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.ModuleName = new System.Windows.Forms.Label();
			this.ModuleRevision = new System.Windows.Forms.Label();
			this.AvailableRevisions = new System.Windows.Forms.ListBox();
			this.VersionsUpdate = new System.Windows.Forms.Button();
			this.VersionsCancel = new System.Windows.Forms.Button();
			this.VersionsNext = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(73, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Module Name";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(13, 31);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(86, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Module Revision";
			// 
			// ModuleName
			// 
			this.ModuleName.AutoSize = true;
			this.ModuleName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ModuleName.Location = new System.Drawing.Point(118, 9);
			this.ModuleName.Name = "ModuleName";
			this.ModuleName.Size = new System.Drawing.Size(84, 13);
			this.ModuleName.TabIndex = 4;
			this.ModuleName.Text = "Module Name";
			// 
			// ModuleRevision
			// 
			this.ModuleRevision.AutoSize = true;
			this.ModuleRevision.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ModuleRevision.Location = new System.Drawing.Point(118, 31);
			this.ModuleRevision.Name = "ModuleRevision";
			this.ModuleRevision.Size = new System.Drawing.Size(101, 13);
			this.ModuleRevision.TabIndex = 5;
			this.ModuleRevision.Text = "Module Revision";
			// 
			// AvailableRevisions
			// 
			this.AvailableRevisions.FormattingEnabled = true;
			this.AvailableRevisions.Location = new System.Drawing.Point(121, 81);
			this.AvailableRevisions.Name = "AvailableRevisions";
			this.AvailableRevisions.Size = new System.Drawing.Size(98, 173);
			this.AvailableRevisions.TabIndex = 6;
			// 
			// VersionsUpdate
			// 
			this.VersionsUpdate.Location = new System.Drawing.Point(16, 81);
			this.VersionsUpdate.Name = "VersionsUpdate";
			this.VersionsUpdate.Size = new System.Drawing.Size(75, 23);
			this.VersionsUpdate.TabIndex = 7;
			this.VersionsUpdate.Text = "Update";
			this.VersionsUpdate.UseVisualStyleBackColor = true;
			this.VersionsUpdate.Click += new System.EventHandler(this.VersionsUpdate_Click);
			// 
			// VersionsCancel
			// 
			this.VersionsCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.VersionsCancel.Location = new System.Drawing.Point(16, 230);
			this.VersionsCancel.Name = "VersionsCancel";
			this.VersionsCancel.Size = new System.Drawing.Size(75, 23);
			this.VersionsCancel.TabIndex = 8;
			this.VersionsCancel.Text = "Cancel";
			this.VersionsCancel.UseVisualStyleBackColor = true;
			this.VersionsCancel.Click += new System.EventHandler(this.VersionsCancel_Click);
			// 
			// VersionsNext
			// 
			this.VersionsNext.Location = new System.Drawing.Point(16, 122);
			this.VersionsNext.Name = "VersionsNext";
			this.VersionsNext.Size = new System.Drawing.Size(75, 23);
			this.VersionsNext.TabIndex = 9;
			this.VersionsNext.Text = "Next";
			this.VersionsNext.UseVisualStyleBackColor = true;
			this.VersionsNext.Click += new System.EventHandler(this.VersionsNext_Click);
			// 
			// Versions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(234, 266);
			this.Controls.Add(this.VersionsNext);
			this.Controls.Add(this.VersionsCancel);
			this.Controls.Add(this.VersionsUpdate);
			this.Controls.Add(this.AvailableRevisions);
			this.Controls.Add(this.ModuleRevision);
			this.Controls.Add(this.ModuleName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Name = "Versions";
			this.Text = "Versions";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label ModuleName;
		private System.Windows.Forms.Label ModuleRevision;
		private System.Windows.Forms.ListBox AvailableRevisions;
		private System.Windows.Forms.Button VersionsUpdate;
		private System.Windows.Forms.Button VersionsCancel;
		private System.Windows.Forms.Button VersionsNext;
	}
}