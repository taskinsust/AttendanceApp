namespace AttendanceApp
{
    partial class Log
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
            this.btnLogOut = new System.Windows.Forms.Button();
            this.cmdUsbGlog = new System.Windows.Forms.Button();
            this.cmdUsbSLog = new System.Windows.Forms.Button();
            this.cmdEmptyGLogData = new System.Windows.Forms.Button();
            this.cmdEmptySLogData = new System.Windows.Forms.Button();
            this.cmdGlogData = new System.Windows.Forms.Button();
            this.cmdSLogData = new System.Windows.Forms.Button();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.lblActions = new System.Windows.Forms.Label();
            this.refreshUsersAndData = new System.Windows.Forms.Timer(this.components);
            this.currentStatedataGridView = new System.Windows.Forms.DataGridView();
            this.btnDeleteAttendanceLog = new System.Windows.Forms.Button();
            this.btnSyncAttendance = new System.Windows.Forms.Button();
            this.btnDeActive = new System.Windows.Forms.Button();
            this.btnActive = new System.Windows.Forms.Button();
            this.sr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LogDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentStatedataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLogOut
            // 
            this.btnLogOut.BackColor = System.Drawing.Color.Black;
            this.btnLogOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogOut.ForeColor = System.Drawing.Color.White;
            this.btnLogOut.Location = new System.Drawing.Point(870, 36);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(100, 35);
            this.btnLogOut.TabIndex = 88;
            this.btnLogOut.Text = "Log Out";
            this.btnLogOut.UseVisualStyleBackColor = false;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // cmdUsbGlog
            // 
            this.cmdUsbGlog.BackColor = System.Drawing.SystemColors.Control;
            this.cmdUsbGlog.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdUsbGlog.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdUsbGlog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdUsbGlog.Location = new System.Drawing.Point(572, 412);
            this.cmdUsbGlog.Name = "cmdUsbGlog";
            this.cmdUsbGlog.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdUsbGlog.Size = new System.Drawing.Size(106, 43);
            this.cmdUsbGlog.TabIndex = 68;
            this.cmdUsbGlog.Text = "Read USB GLogData";
            this.cmdUsbGlog.UseVisualStyleBackColor = false;
            this.cmdUsbGlog.Visible = false;
            // 
            // cmdUsbSLog
            // 
            this.cmdUsbSLog.BackColor = System.Drawing.SystemColors.Control;
            this.cmdUsbSLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdUsbSLog.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdUsbSLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdUsbSLog.Location = new System.Drawing.Point(254, 412);
            this.cmdUsbSLog.Name = "cmdUsbSLog";
            this.cmdUsbSLog.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdUsbSLog.Size = new System.Drawing.Size(106, 43);
            this.cmdUsbSLog.TabIndex = 65;
            this.cmdUsbSLog.Text = "Read USB SLogData";
            this.cmdUsbSLog.UseVisualStyleBackColor = false;
            this.cmdUsbSLog.Visible = false;
            // 
            // cmdEmptyGLogData
            // 
            this.cmdEmptyGLogData.BackColor = System.Drawing.SystemColors.Control;
            this.cmdEmptyGLogData.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdEmptyGLogData.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdEmptyGLogData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdEmptyGLogData.Location = new System.Drawing.Point(466, 412);
            this.cmdEmptyGLogData.Name = "cmdEmptyGLogData";
            this.cmdEmptyGLogData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdEmptyGLogData.Size = new System.Drawing.Size(106, 43);
            this.cmdEmptyGLogData.TabIndex = 67;
            this.cmdEmptyGLogData.Text = "Empty GLogData";
            this.cmdEmptyGLogData.UseVisualStyleBackColor = false;
            this.cmdEmptyGLogData.Visible = false;
            // 
            // cmdEmptySLogData
            // 
            this.cmdEmptySLogData.BackColor = System.Drawing.SystemColors.Control;
            this.cmdEmptySLogData.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdEmptySLogData.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdEmptySLogData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdEmptySLogData.Location = new System.Drawing.Point(148, 412);
            this.cmdEmptySLogData.Name = "cmdEmptySLogData";
            this.cmdEmptySLogData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdEmptySLogData.Size = new System.Drawing.Size(106, 43);
            this.cmdEmptySLogData.TabIndex = 64;
            this.cmdEmptySLogData.Text = "Empty SLogData";
            this.cmdEmptySLogData.UseVisualStyleBackColor = false;
            this.cmdEmptySLogData.Visible = false;
            // 
            // cmdGlogData
            // 
            this.cmdGlogData.BackColor = System.Drawing.SystemColors.Control;
            this.cmdGlogData.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdGlogData.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdGlogData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdGlogData.Location = new System.Drawing.Point(360, 412);
            this.cmdGlogData.Name = "cmdGlogData";
            this.cmdGlogData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdGlogData.Size = new System.Drawing.Size(106, 43);
            this.cmdGlogData.TabIndex = 66;
            this.cmdGlogData.Text = "Read GLogData";
            this.cmdGlogData.UseVisualStyleBackColor = false;
            this.cmdGlogData.Visible = false;
            // 
            // cmdSLogData
            // 
            this.cmdSLogData.BackColor = System.Drawing.SystemColors.Control;
            this.cmdSLogData.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdSLogData.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSLogData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdSLogData.Location = new System.Drawing.Point(42, 412);
            this.cmdSLogData.Name = "cmdSLogData";
            this.cmdSLogData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdSLogData.Size = new System.Drawing.Size(106, 43);
            this.cmdSLogData.TabIndex = 63;
            this.cmdSLogData.Text = "Read SLogData";
            this.cmdSLogData.UseVisualStyleBackColor = false;
            this.cmdSLogData.Visible = false;
            // 
            // GroupBox1
            // 
            this.GroupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.GroupBox1.Controls.Add(this.Label6);
            this.GroupBox1.Controls.Add(this.lblTime);
            this.GroupBox1.Controls.Add(this.Label7);
            this.GroupBox1.Controls.Add(this.lblActions);
            this.GroupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox1.Location = new System.Drawing.Point(2, 3);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(188, 119);
            this.GroupBox1.TabIndex = 74;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Latest Activity";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.BackColor = System.Drawing.Color.Transparent;
            this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Label6.Location = new System.Drawing.Point(5, 26);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(124, 15);
            this.Label6.TabIndex = 30;
            this.Label6.Text = "Actions Performed";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblTime.Location = new System.Drawing.Point(5, 94);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(38, 13);
            this.lblTime.TabIndex = 33;
            this.lblTime.Text = "Time ";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.BackColor = System.Drawing.Color.Transparent;
            this.Label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Label7.Location = new System.Drawing.Point(5, 70);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(90, 15);
            this.Label7.TabIndex = 31;
            this.Label7.Text = "Date && Time ";
            // 
            // lblActions
            // 
            this.lblActions.AutoSize = true;
            this.lblActions.BackColor = System.Drawing.Color.Transparent;
            this.lblActions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActions.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblActions.Location = new System.Drawing.Point(5, 49);
            this.lblActions.Name = "lblActions";
            this.lblActions.Size = new System.Drawing.Size(53, 13);
            this.lblActions.TabIndex = 32;
            this.lblActions.Text = "Actions ";
            // 
            // refreshUsersAndData
            // 
            this.refreshUsersAndData.Interval = 900000;
            this.refreshUsersAndData.Tick += new System.EventHandler(this.refreshUsersAndData_Tick);
            // 
            // currentStatedataGridView
            // 
            this.currentStatedataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.currentStatedataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sr,
            this.LogDetails,
            this.CurrentDate,
            this.Status});
            this.currentStatedataGridView.Location = new System.Drawing.Point(2, 128);
            this.currentStatedataGridView.Name = "currentStatedataGridView";
            this.currentStatedataGridView.Size = new System.Drawing.Size(968, 384);
            this.currentStatedataGridView.TabIndex = 89;
            // 
            // btnDeleteAttendanceLog
            // 
            this.btnDeleteAttendanceLog.BackColor = System.Drawing.Color.Crimson;
            this.btnDeleteAttendanceLog.BackgroundImage = global::AttendanceApp.Properties.Resources.btn_del;
            this.btnDeleteAttendanceLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDeleteAttendanceLog.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDeleteAttendanceLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAttendanceLog.Location = new System.Drawing.Point(640, 36);
            this.btnDeleteAttendanceLog.Name = "btnDeleteAttendanceLog";
            this.btnDeleteAttendanceLog.Size = new System.Drawing.Size(100, 35);
            this.btnDeleteAttendanceLog.TabIndex = 87;
            this.btnDeleteAttendanceLog.UseVisualStyleBackColor = false;
            this.btnDeleteAttendanceLog.Click += new System.EventHandler(this.btnDeleteAttendanceLog_Click);
            // 
            // btnSyncAttendance
            // 
            this.btnSyncAttendance.BackColor = System.Drawing.Color.Gold;
            this.btnSyncAttendance.BackgroundImage = global::AttendanceApp.Properties.Resources.btn_sync;
            this.btnSyncAttendance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSyncAttendance.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSyncAttendance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSyncAttendance.Location = new System.Drawing.Point(507, 36);
            this.btnSyncAttendance.Name = "btnSyncAttendance";
            this.btnSyncAttendance.Size = new System.Drawing.Size(127, 35);
            this.btnSyncAttendance.TabIndex = 86;
            this.btnSyncAttendance.UseVisualStyleBackColor = false;
            this.btnSyncAttendance.Click += new System.EventHandler(this.btnSyncAttendance_Click);
            // 
            // btnDeActive
            // 
            this.btnDeActive.BackColor = System.Drawing.Color.Pink;
            this.btnDeActive.BackgroundImage = global::AttendanceApp.Properties.Resources.btn_inactive;
            this.btnDeActive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDeActive.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDeActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeActive.Location = new System.Drawing.Point(387, 36);
            this.btnDeActive.Name = "btnDeActive";
            this.btnDeActive.Size = new System.Drawing.Size(114, 35);
            this.btnDeActive.TabIndex = 85;
            this.btnDeActive.UseVisualStyleBackColor = false;
            this.btnDeActive.Click += new System.EventHandler(this.btnDeActive_Click);
            // 
            // btnActive
            // 
            this.btnActive.BackColor = System.Drawing.Color.Chartreuse;
            this.btnActive.BackgroundImage = global::AttendanceApp.Properties.Resources.btn_active;
            this.btnActive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnActive.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnActive.FlatAppearance.BorderSize = 0;
            this.btnActive.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActive.Location = new System.Drawing.Point(260, 36);
            this.btnActive.Name = "btnActive";
            this.btnActive.Size = new System.Drawing.Size(117, 35);
            this.btnActive.TabIndex = 84;
            this.btnActive.UseVisualStyleBackColor = false;
            this.btnActive.Click += new System.EventHandler(this.btnActive_Click);
            // 
            // sr
            // 
            this.sr.HeaderText = "Sr #";
            this.sr.Name = "sr";
            // 
            // LogDetails
            // 
            this.LogDetails.HeaderText = "Log Details";
            this.LogDetails.Name = "LogDetails";
            this.LogDetails.Width = 500;
            // 
            // CurrentDate
            // 
            this.CurrentDate.HeaderText = "Date & Time";
            this.CurrentDate.Name = "CurrentDate";
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            // 
            // Log
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 514);
            this.Controls.Add(this.currentStatedataGridView);
            this.Controls.Add(this.btnDeleteAttendanceLog);
            this.Controls.Add(this.btnSyncAttendance);
            this.Controls.Add(this.btnDeActive);
            this.Controls.Add(this.btnActive);
            this.Controls.Add(this.btnLogOut);
            this.Controls.Add(this.cmdUsbGlog);
            this.Controls.Add(this.cmdUsbSLog);
            this.Controls.Add(this.cmdEmptyGLogData);
            this.Controls.Add(this.cmdEmptySLogData);
            this.Controls.Add(this.cmdGlogData);
            this.Controls.Add(this.cmdSLogData);
            this.Controls.Add(this.GroupBox1);
            this.Name = "Log";
            this.Text = "Log";
            this.Load += new System.EventHandler(this.Log_Load);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentStatedataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnDeleteAttendanceLog;
        internal System.Windows.Forms.Button btnSyncAttendance;
        internal System.Windows.Forms.Button btnDeActive;
        internal System.Windows.Forms.Button btnActive;
        internal System.Windows.Forms.Button btnLogOut;
        public System.Windows.Forms.Button cmdUsbGlog;
        public System.Windows.Forms.Button cmdUsbSLog;
        public System.Windows.Forms.Button cmdEmptyGLogData;
        public System.Windows.Forms.Button cmdEmptySLogData;
        public System.Windows.Forms.Button cmdGlogData;
        public System.Windows.Forms.Button cmdSLogData;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label lblTime;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label lblActions;
        private System.Windows.Forms.Timer refreshUsersAndData;
        private System.Windows.Forms.DataGridView currentStatedataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn sr;
        private System.Windows.Forms.DataGridViewTextBoxColumn LogDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    }
}