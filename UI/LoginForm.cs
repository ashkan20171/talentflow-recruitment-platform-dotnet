using System;
using System.Drawing;
using System.Windows.Forms;
using AshkanJobCenter.Core;
using AshkanJobCenter.Services;

namespace AshkanJobCenter.UI
{
    public class LoginForm : Form
    {
        private readonly Panel contentPanel = new Panel();
        private readonly Panel brandPanel = new Panel();
        private readonly Panel card = new Panel();
        private readonly Label title = new Label();
        private readonly Label subtitle = new Label();
        private readonly Label userLabel = new Label();
        private readonly Label passLabel = new Label();
        private readonly Label statusLabel = new Label();
        private readonly Label capsLabel = new Label();
        private readonly Label securityHint = new Label();
        private readonly Label versionLabel = new Label();
        private readonly TextBox user = new TextBox();
        private readonly TextBox pass = new TextBox();
        private readonly CheckBox showPassword = new CheckBox();
        private readonly CheckBox remember = new CheckBox();
        private readonly Button login = new Button();
        private readonly Button register = new Button();
        private readonly Button forgot = new Button();
        private readonly Button lang = new Button();
        private readonly Label brandTitle = new Label();
        private readonly Label brandText = new Label();
        private readonly Label brandBadge = new Label();

        public LoginForm()
        {
            Text = L.T("app");
            ClientSize = new Size(1180, 760);
            MinimumSize = new Size(920, 620);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            BackColor = Color.FromArgb(244, 248, 252);
            Font = Theme.Font;
            AutoScaleMode = AutoScaleMode.Dpi;
            KeyPreview = true;
            DoubleBuffered = true;

            Build();
            ApplyLanguage();
            Resize += delegate { ArrangeLayout(); };
            Shown += delegate { ArrangeLayout(); user.Focus(); };
            KeyUp += delegate { UpdateCapsLock(); };
            KeyDown += delegate { UpdateCapsLock(); };
        }

        private void Build()
        {
            // Do not mix Fill/Right docking here. In RTL/DPI scenarios WinForms can
            // calculate the Fill panel against the whole client area and the login
            // card then slides underneath the branding panel. Stage 13 owns both
            // regions explicitly in ArrangeLayout().
            contentPanel.Dock = DockStyle.None;
            contentPanel.BackColor = Color.FromArgb(244, 248, 252);
            Controls.Add(contentPanel);

            brandPanel.Dock = DockStyle.None;
            brandPanel.Width = 430;
            brandPanel.BackColor = Color.FromArgb(22, 67, 105);
            Controls.Add(brandPanel);
            brandPanel.BringToFront();

            BuildBrandPanel();
            BuildLoginCard();
            AcceptButton = login;
        }

        private void BuildBrandPanel()
        {
            var logoBox = new Panel
            {
                Size = new Size(72, 72),
                Location = new Point(54, 100),
                BackColor = Color.FromArgb(255, 255, 255)
            };
            var logo = new Label
            {
                Text = "AJC",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(22, 67, 105),
                Font = new Font("Segoe UI", 19f, FontStyle.Bold)
            };
            logoBox.Controls.Add(logo);

            brandBadge.AutoSize = false;
            brandBadge.Size = new Size(205, 30);
            brandBadge.Location = new Point(54, 202);
            brandBadge.TextAlign = ContentAlignment.MiddleCenter;
            brandBadge.BackColor = Color.FromArgb(36, 91, 132);
            brandBadge.ForeColor = Color.FromArgb(224, 241, 251);
            brandBadge.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            brandTitle.AutoSize = false;
            brandTitle.Size = new Size(320, 60);
            brandTitle.Location = new Point(54, 252);
            brandTitle.ForeColor = Color.White;
            brandTitle.Font = new Font("Segoe UI", 22f, FontStyle.Bold);

            brandText.AutoSize = false;
            brandText.Size = new Size(320, 150);
            brandText.Location = new Point(54, 325);
            brandText.ForeColor = Color.FromArgb(220, 234, 244);
            brandText.Font = new Font("Segoe UI", 11f);

            var divider = new Panel
            {
                Location = new Point(54, 500),
                Size = new Size(320, 1),
                BackColor = Color.FromArgb(65, 112, 149)
            };
            securityHint.AutoSize = false;
            securityHint.Location = new Point(54, 520);
            securityHint.Size = new Size(320, 74);
            securityHint.ForeColor = Color.FromArgb(193, 218, 235);
            securityHint.Font = new Font("Segoe UI", 9.5f);

            versionLabel.AutoSize = false;
            versionLabel.Location = new Point(54, 640);
            versionLabel.Size = new Size(320, 28);
            versionLabel.ForeColor = Color.FromArgb(145, 184, 210);
            versionLabel.Font = new Font("Segoe UI", 8.5f);

            brandPanel.Controls.Add(logoBox);
            brandPanel.Controls.Add(brandBadge);
            brandPanel.Controls.Add(brandTitle);
            brandPanel.Controls.Add(brandText);
            brandPanel.Controls.Add(divider);
            brandPanel.Controls.Add(securityHint);
            brandPanel.Controls.Add(versionLabel);
        }

        private void BuildLoginCard()
        {
            card.Size = new Size(500, 590);
            card.BackColor = Color.White;
            card.Padding = new Padding(48, 40, 48, 36);
            card.Paint += delegate(object sender, PaintEventArgs e)
            {
                using (var pen = new Pen(Color.FromArgb(224, 232, 240)))
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };
            contentPanel.Controls.Add(card);

            title.AutoSize = false;
            title.Location = new Point(48, 40);
            title.Size = new Size(404, 48);
            title.Font = new Font("Segoe UI", 23f, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(27, 43, 58);

            subtitle.AutoSize = false;
            subtitle.Location = new Point(48, 92);
            subtitle.Size = new Size(404, 52);
            subtitle.Font = new Font("Segoe UI", 10f);
            subtitle.ForeColor = Color.FromArgb(101, 117, 133);

            SetupLabel(userLabel, 158);
            SetupTextBox(user, 185, false);
            SetupLabel(passLabel, 247);
            SetupTextBox(pass, 274, true);
            pass.KeyUp += delegate { UpdateCapsLock(); };
            pass.KeyDown += delegate { UpdateCapsLock(); };

            capsLabel.AutoSize = false;
            capsLabel.Location = new Point(48, 315);
            capsLabel.Size = new Size(404, 22);
            capsLabel.ForeColor = Color.FromArgb(176, 116, 32);
            capsLabel.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            capsLabel.Visible = false;

            showPassword.Location = new Point(48, 344);
            showPassword.AutoSize = true;
            showPassword.ForeColor = Theme.Muted;
            showPassword.CheckedChanged += delegate { pass.UseSystemPasswordChar = !showPassword.Checked; };

            remember.Location = new Point(260, 344);
            remember.AutoSize = true;
            remember.ForeColor = Theme.Muted;

            forgot.FlatStyle = FlatStyle.Flat;
            forgot.FlatAppearance.BorderSize = 0;
            forgot.BackColor = Color.White;
            forgot.ForeColor = Theme.Primary;
            forgot.Cursor = Cursors.Hand;
            forgot.Location = new Point(48, 378);
            forgot.Size = new Size(190, 30);
            forgot.TextAlign = ContentAlignment.MiddleLeft;
            forgot.Click += delegate
            {
                MessageBox.Show(
                    L.Fa ? "برای بازیابی رمز عبور با مدیر سامانه تماس بگیرید. زیرساخت بازیابی ایمیلی در نسخه بعدی قابل اتصال است." : "Contact your administrator to reset your password. Email recovery can be connected in the next deployment.",
                    L.Fa ? "بازیابی رمز عبور" : "Password recovery",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            };

            SetupButton(login, 418, true);
            login.Click += Login;
            SetupButton(register, 472, false);
            register.Click += delegate
            {
                using (var form = new RegisterForm()) form.ShowDialog(this);
            };

            lang.FlatStyle = FlatStyle.Flat;
            lang.FlatAppearance.BorderColor = Color.FromArgb(214, 224, 233);
            lang.BackColor = Color.White;
            lang.ForeColor = Theme.Primary;
            lang.Size = new Size(118, 34);
            lang.Location = new Point(48, 530);
            lang.Cursor = Cursors.Hand;
            lang.Click += delegate
            {
                AppSession.Language = L.Fa ? "en" : "fa";
                ApplyLanguage();
            };

            statusLabel.AutoSize = false;
            statusLabel.Location = new Point(176, 526);
            statusLabel.Size = new Size(276, 42);
            statusLabel.ForeColor = Theme.Danger;

            card.Controls.Add(title);
            card.Controls.Add(subtitle);
            card.Controls.Add(userLabel);
            card.Controls.Add(user);
            card.Controls.Add(passLabel);
            card.Controls.Add(pass);
            card.Controls.Add(capsLabel);
            card.Controls.Add(showPassword);
            card.Controls.Add(remember);
            card.Controls.Add(forgot);
            card.Controls.Add(login);
            card.Controls.Add(register);
            card.Controls.Add(lang);
            card.Controls.Add(statusLabel);
        }

        private static void SetupLabel(Label label, int top)
        {
            label.AutoSize = false;
            label.Location = new Point(48, top);
            label.Size = new Size(404, 23);
            label.ForeColor = Color.FromArgb(42, 57, 72);
            label.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        }

        private static void SetupTextBox(TextBox textBox, int top, bool password)
        {
            textBox.Location = new Point(48, top);
            textBox.Size = new Size(404, 38);
            textBox.Font = new Font("Segoe UI", 11f);
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.BackColor = Color.FromArgb(251, 253, 255);
            textBox.UseSystemPasswordChar = password;
        }

        private static void SetupButton(Button button, int top, bool primary)
        {
            button.Location = new Point(48, top);
            button.Size = new Size(404, 44);
            button.FlatStyle = FlatStyle.Flat;
            button.Cursor = Cursors.Hand;
            button.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            button.BackColor = primary ? Color.FromArgb(27, 82, 126) : Color.White;
            button.ForeColor = primary ? Color.White : Color.FromArgb(27, 82, 126);
            button.FlatAppearance.BorderColor = primary ? Color.FromArgb(27, 82, 126) : Color.FromArgb(210, 222, 232);
        }

        private void ArrangeLayout()
        {
            int width = Math.Max(1, ClientSize.Width);
            int height = Math.Max(1, ClientSize.Height);
            bool showBrand = width >= 980;

            brandPanel.Visible = showBrand;
            int brandWidth = showBrand ? Math.Max(390, Math.Min(470, width / 3)) : 0;

            // Branding is always physically on the right. The content region ends
            // exactly where branding begins, so the card can never overlap it.
            if (showBrand)
            {
                brandPanel.SetBounds(width - brandWidth, 0, brandWidth, height);
                brandPanel.BringToFront();
            }

            contentPanel.SetBounds(0, 0, width - brandWidth, height);
            contentPanel.SendToBack();

            // Keep the card fully inside the white workspace at every supported DPI.
            int horizontalMargin = 28;
            int verticalMargin = 24;
            int maxCardWidth = Math.Max(360, contentPanel.ClientSize.Width - (horizontalMargin * 2));
            int targetWidth = Math.Min(500, maxCardWidth);
            if (card.Width != targetWidth)
            {
                card.Width = targetWidth;
                ResizeCardChildren(targetWidth);
            }

            int left = Math.Max(horizontalMargin, (contentPanel.ClientSize.Width - card.Width) / 2);
            int top = Math.Max(verticalMargin, (contentPanel.ClientSize.Height - card.Height) / 2);
            card.Location = new Point(left, top);
            versionLabel.Top = Math.Max(600, brandPanel.ClientSize.Height - 58);
        }

        private void ResizeCardChildren(int cardWidth)
        {
            int innerWidth = Math.Max(260, cardWidth - 96);
            title.Width = innerWidth;
            subtitle.Width = innerWidth;
            userLabel.Width = innerWidth;
            passLabel.Width = innerWidth;
            user.Width = innerWidth;
            pass.Width = innerWidth;
            capsLabel.Width = innerWidth;
            login.Width = innerWidth;
            register.Width = innerWidth;

            // Keep the secondary controls inside the card as it becomes compact.
            remember.Left = Math.Max(48, cardWidth - remember.Width - 48);
            statusLabel.Left = Math.Min(176, 48 + Math.Max(0, innerWidth - 276));
            statusLabel.Width = Math.Max(140, cardWidth - statusLabel.Left - 48);
        }

        private void UpdateCapsLock()
        {
            capsLabel.Visible = pass.Focused && Control.IsKeyLocked(Keys.CapsLock);
            capsLabel.Text = L.Fa ? "⚠ Caps Lock روشن است" : "⚠ Caps Lock is on";
        }

        private void ApplyLanguage()
        {
            bool fa = L.Fa;
            Text = fa ? "مرکز کاریابی اشکان - ورود" : "Ashkan Job Center - Sign in";
            title.Text = fa ? "خوش آمدید" : "Welcome back";
            subtitle.Text = fa ? "برای ادامه، وارد فضای امن مدیریت استخدام شوید." : "Sign in to your secure recruitment workspace.";
            userLabel.Text = fa ? "نام کاربری" : "Username";
            passLabel.Text = fa ? "رمز عبور" : "Password";
            showPassword.Text = fa ? "نمایش رمز عبور" : "Show password";
            remember.Text = fa ? "مرا به خاطر بسپار" : "Remember me";
            forgot.Text = fa ? "رمز عبور را فراموش کرده‌اید؟" : "Forgot password?";
            login.Text = fa ? "ورود به فضای کاری" : "Sign in to workspace";
            register.Text = fa ? "ایجاد حساب کارجو" : "Create candidate account";
            lang.Text = fa ? "English" : "فارسی";
            brandBadge.Text = fa ? "RECRUITMENT WORKSPACE" : "RECRUITMENT WORKSPACE";
            brandTitle.Text = fa ? "استخدام هوشمند، مدیریت یکپارچه" : "Smarter hiring, one workspace";
            brandText.Text = fa
                ? "فرصت‌های شغلی، استعدادها، مصاحبه‌ها و پیشنهادهای استخدام را در یک فضای حرفه‌ای و امن مدیریت کنید."
                : "Manage jobs, talent, interviews and offers in one professional, secure workspace.";
            securityHint.Text = fa
                ? "✓ دسترسی مبتنی بر نقش\r\n✓ ثبت رویدادهای امنیتی\r\n✓ مدیریت یکپارچه فرآیند استخدام"
                : "✓ Role-based access\r\n✓ Security audit trail\r\n✓ End-to-end recruitment workflow";
            versionLabel.Text = fa ? "Ashkan Job Center  •  Stage 13 Layout & Workspace Polish" : "Ashkan Job Center  •  Stage 13 Layout & Workspace Polish";

            RightToLeft = fa ? RightToLeft.Yes : RightToLeft.No;
            title.TextAlign = fa ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft;
            subtitle.TextAlign = fa ? ContentAlignment.TopRight : ContentAlignment.TopLeft;
            userLabel.TextAlign = fa ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft;
            passLabel.TextAlign = fa ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft;
            brandTitle.TextAlign = fa ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft;
            brandText.TextAlign = fa ? ContentAlignment.TopRight : ContentAlignment.TopLeft;
            securityHint.TextAlign = fa ? ContentAlignment.TopRight : ContentAlignment.TopLeft;
            statusLabel.TextAlign = fa ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft;
            forgot.TextAlign = fa ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft;
            capsLabel.TextAlign = fa ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft;
            statusLabel.Text = string.Empty;
            UpdateCapsLock();
            ArrangeLayout();
        }

        private void Login(object sender, EventArgs e)
        {
            statusLabel.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(user.Text) || string.IsNullOrEmpty(pass.Text))
            {
                statusLabel.Text = L.Fa ? "نام کاربری و رمز عبور را وارد کنید." : "Enter username and password.";
                return;
            }

            login.Enabled = false;
            login.Text = L.Fa ? "در حال بررسی اطلاعات..." : "Verifying credentials...";
            Cursor = Cursors.WaitCursor;
            try
            {
                string error;
                if (AuthService.Login(user.Text.Trim(), pass.Text, out error))
                {
                    Hide();
                    using (var main = new MainForm()) main.ShowDialog();
                    Close();
                }
                else
                {
                    statusLabel.Text = error;
                    pass.SelectAll();
                    pass.Focus();
                }
            }
            finally
            {
                Cursor = Cursors.Default;
                if (!IsDisposed)
                {
                    login.Enabled = true;
                    login.Text = L.Fa ? "ورود به فضای کاری" : "Sign in to workspace";
                }
            }
        }
    }
}
