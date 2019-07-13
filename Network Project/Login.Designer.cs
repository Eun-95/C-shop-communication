namespace Network_Project
{
    partial class Login
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.UserID = new System.Windows.Forms.TextBox();
            this.UserPw = new System.Windows.Forms.TextBox();
            this.User_Login = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LoginClose = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("배달의민족 도현", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(11, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "아이디:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("배달의민족 도현", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(11, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "암호:";
            // 
            // UserID
            // 
            this.UserID.Location = new System.Drawing.Point(79, 26);
            this.UserID.Name = "UserID";
            this.UserID.Size = new System.Drawing.Size(192, 27);
            this.UserID.TabIndex = 3;
            // 
            // UserPw
            // 
            this.UserPw.Location = new System.Drawing.Point(79, 52);
            this.UserPw.Name = "UserPw";
            this.UserPw.PasswordChar = '*';
            this.UserPw.Size = new System.Drawing.Size(192, 27);
            this.UserPw.TabIndex = 4;
            // 
            // User_Login
            // 
            this.User_Login.BackColor = System.Drawing.Color.Black;
            this.User_Login.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.User_Login.Location = new System.Drawing.Point(39, 85);
            this.User_Login.Name = "User_Login";
            this.User_Login.Size = new System.Drawing.Size(112, 32);
            this.User_Login.TabIndex = 5;
            this.User_Login.Text = "로그인";
            this.User_Login.UseVisualStyleBackColor = false;
            this.User_Login.Click += new System.EventHandler(this.User_Login_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LoginClose);
            this.groupBox1.Controls.Add(this.User_Login);
            this.groupBox1.Controls.Add(this.UserPw);
            this.groupBox1.Controls.Add(this.UserID);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("배달의민족 도현", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(42, 246);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(305, 139);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "접속 정보";
            // 
            // LoginClose
            // 
            this.LoginClose.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.LoginClose.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LoginClose.Location = new System.Drawing.Point(157, 85);
            this.LoginClose.Name = "LoginClose";
            this.LoginClose.Size = new System.Drawing.Size(114, 32);
            this.LoginClose.TabIndex = 7;
            this.LoginClose.Text = "종료";
            this.LoginClose.UseVisualStyleBackColor = false;
            this.LoginClose.Click += new System.EventHandler(this.LoginClose_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(42, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(305, 229);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(399, 397);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "Login";
            this.Text = "로그인";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox UserID;
        private System.Windows.Forms.TextBox UserPw;
        private System.Windows.Forms.Button User_Login;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button LoginClose;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

