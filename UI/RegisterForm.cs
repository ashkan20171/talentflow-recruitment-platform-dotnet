using System;
using System.Drawing;
using System.Windows.Forms;
using AshkanJobCenter.Core;
using AshkanJobCenter.Services;

namespace AshkanJobCenter.UI
{
    public class RegisterForm : Form
    {
        private TextBox fullName;
        private TextBox username;
        private TextBox email;
        private TextBox password;
        private Label status;

        public RegisterForm()
        {
            ClientSize = new Size(560, 620);
            MinimumSize = new Size(520, 590);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Theme.Background;
            Font = Theme.Font;
            AutoScaleMode = AutoScaleMode.Dpi;
            Build();
        }

        private void Build()
        {
            bool fa = L.Fa;
            Text = fa ? "ایجاد حساب کاربری" : "Create account";
            RightToLeft = fa ? RightToLeft.Yes : RightToLeft.No;

            var card = new Panel { BackColor = Theme.Surface, Size = new Size(450, 520), Location = new Point(55, 35) };
            Controls.Add(card);
            var title = new Label { AutoSize = false, Location = new Point(35, 28), Size = new Size(380, 38), Font = new Font("Segoe UI", 20f, FontStyle.Bold), ForeColor = Theme.Text, Text = fa ? "ثبت‌نام کارجو" : "Candidate registration", TextAlign = fa ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft };
            var hint = new Label { AutoSize = false, Location = new Point(35, 70), Size = new Size(380, 44), ForeColor = Theme.Muted, Text = fa ? "حساب جدید با دسترسی کارجو ایجاد می‌شود." : "New accounts are created with Candidate access.", TextAlign = fa ? ContentAlignment.TopRight : ContentAlignment.TopLeft };
            card.Controls.Add(title); card.Controls.Add(hint);

            fullName = AddField(card, fa ? "نام و نام خانوادگی" : "Full name", 125, false);
            username = AddField(card, fa ? "نام کاربری" : "Username", 200, false);
            email = AddField(card, "Email", 275, false);
            password = AddField(card, fa ? "رمز عبور (حداقل ۸ کاراکتر)" : "Password (8+ characters)", 350, true);

            var create = Ui.Button(fa ? "ایجاد حساب" : "Create account");
            create.SetBounds(35, 425, 380, 42);
            create.AutoSize = false;
            create.Click += delegate
            {
                status.Text = string.Empty;
                string error;
                if (AuthService.Register(fullName.Text.Trim(), username.Text.Trim(), email.Text.Trim(), password.Text, out error))
                {
                    MessageBox.Show(fa ? "حساب با موفقیت ایجاد شد. اکنون می‌توانید وارد شوید." : "Account created successfully. You can sign in now.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else status.Text = error;
            };
            card.Controls.Add(create);

            status = new Label { AutoSize = false, Location = new Point(35, 475), Size = new Size(380, 30), ForeColor = Theme.Danger, TextAlign = fa ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft };
            card.Controls.Add(status);
            AcceptButton = create;
        }

        private TextBox AddField(Control parent, string caption, int top, bool secret)
        {
            bool fa = L.Fa;
            var label = new Label { Text = caption, AutoSize = false, Location = new Point(35, top), Size = new Size(380, 23), Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), ForeColor = Theme.Text, TextAlign = fa ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft };
            var input = new TextBox { Location = new Point(35, top + 26), Size = new Size(380, 34), Font = new Font("Segoe UI", 11f), BorderStyle = BorderStyle.FixedSingle, UseSystemPasswordChar = secret };
            parent.Controls.Add(label); parent.Controls.Add(input);
            return input;
        }
    }
}
