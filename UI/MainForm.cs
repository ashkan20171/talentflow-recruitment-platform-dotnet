using System;
using System.Drawing;
using System.Windows.Forms;
using AshkanJobCenter.Core;

namespace AshkanJobCenter.UI
{
    public class MainForm : Form
    {
        Panel content, nav, top;
        FlowLayoutPanel navButtons;
        Label pageTitle, userInfo, brand, roleBadge;
        Button collapseButton;
        bool collapsed;
        string currentPage = "dashboard";
        readonly Color navColor = Color.FromArgb(24, 53, 78);
        readonly Color navHover = Color.FromArgb(35, 75, 105);
        readonly Color navActive = Color.FromArgb(31, 120, 155);

        public MainForm()
        {
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1100, 700);
            Text = L.T("app");
            Build();
            Theme.Apply(this);
            // Theme.Apply changes ForeColor recursively; restore navigation contrast afterwards.
            StyleNavigation();
            Resize += delegate { LayoutShell(); };
            Shown += delegate { LayoutShell(); };
            ShowPage("dashboard");
        }

        void Build()
        {
            SuspendLayout();
            nav = new Panel { BackColor = navColor, Padding = new Padding(12, 14, 12, 12) };
            brand = new Label { Text = L.Fa ? "مرکز کاریابی اشکان" : "ASHKAN JOB CENTER", ForeColor = Color.White, Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = false, Height = 58, TextAlign = ContentAlignment.MiddleCenter };
            roleBadge = new Label { Text = RoleCaption(), ForeColor = Color.FromArgb(190, 214, 232), Font = new Font("Segoe UI", 9), AutoSize = false, Height = 30, TextAlign = ContentAlignment.MiddleCenter };
            navButtons = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, BackColor = navColor, Padding = new Padding(0, 8, 0, 8) };
            nav.Controls.Add(brand); nav.Controls.Add(roleBadge); nav.Controls.Add(navButtons);
            Controls.Add(nav);

            top = new Panel { BackColor = Theme.Surface, Padding = new Padding(18, 12, 18, 10) };
            collapseButton = Ui.Button("☰", false); collapseButton.Size = new Size(46, 42); collapseButton.FlatAppearance.BorderColor = Theme.Border; collapseButton.Click += delegate { ToggleNavigation(); };
            pageTitle = Ui.Label("", 17, true); pageTitle.AutoSize = false; pageTitle.TextAlign = L.Fa ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft;
            userInfo = Ui.Label(AppSession.FullName + "  •  " + RoleCaption(), 10); userInfo.AutoSize = false; userInfo.ForeColor = Theme.Muted; userInfo.TextAlign = L.Fa ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleRight;
            top.Controls.Add(collapseButton); top.Controls.Add(pageTitle); top.Controls.Add(userInfo);
            Controls.Add(top);

            content = new Panel { BackColor = Theme.Background, Padding = new Padding(22) };
            Controls.Add(content);
            BuildNavigation();
            RightToLeft = L.Fa ? RightToLeft.Yes : RightToLeft.No;
            // Do not mirror WinForms docking/bounds. We position RTL/LTR explicitly in LayoutShell.
            RightToLeftLayout = false;
            ResumeLayout();
        }

        string RoleCaption()
        {
            if (!L.Fa) return AppSession.Role;
            switch (AppSession.Role)
            {
                case "Candidate": return "کارجو";
                case "Recruiter": return "کارشناس جذب";
                case "Employer": return "کارفرما";
                case "Admin": return "مدیر سامانه";
                default: return AppSession.Role;
            }
        }

        bool Candidate { get { return string.Equals(AppSession.Role, "Candidate", StringComparison.OrdinalIgnoreCase); } }

        void BuildNavigation()
        {
            navButtons.Controls.Clear();
            AddNav("dashboard", L.T("dashboard"), "Dashboard.View", "⌂");
            AddNav("quickactions", L.Fa ? "اقدامات سریع" : "Quick actions", "Dashboard.View", "⚡");
            AddNav("jobs", L.T("jobs"), "Jobs.View", "▦");
            if (Candidate) AddNav("savedjobs", L.Fa ? "فرصت‌های ذخیره‌شده" : "Saved jobs", "Jobs.View", "★");
            if (Candidate) AddNav("jobalerts", L.Fa ? "هشدارهای شغلی" : "Job alerts", "Jobs.View", "●");
            AddNav("applications", L.T("applications"), "Applications.View", "✓");
            if (Candidate) AddNav("timeline", L.Fa ? "تایم‌لاین درخواست‌ها" : "Application timeline", "Applications.View", "↻");
            AddNav("interviews", L.T("interviews"), "Applications.View", "◷");
            AddNav("notifications", L.T("notifications"), "Dashboard.View", "●");
            if (Candidate)
            {
                AddNav("resume", L.T("resume"), "Dashboard.View", "▤");
                AddNav("documents", L.Fa ? "مدارک من" : "My documents", "Dashboard.View", "□");
                AddNav("profile", L.Fa ? "پروفایل من" : "My profile", "Dashboard.View", "☺");
                AddNav("settings", L.T("settings"), "Dashboard.View", "⚙");
                return;
            }
            AddNav("overview", L.Fa ? "نمای مدیریتی" : "Executive overview", "Dashboard.View", "◫");
            AddNav("recruiterworkspace", L.Fa ? "میزکار جذب" : "Recruiter workspace", "Candidates.View", "◈");
            AddNav("tasks", L.Fa ? "کارهای جذب" : "Recruitment tasks", "Applications.Manage", "☑");
            AddNav("compare", L.Fa ? "مقایسه کارجوها" : "Compare candidates", "Candidates.View", "⇄");
            AddNav("talentpool", L.Fa ? "استخر استعداد" : "Talent pool", "Candidates.View", "♢");
            AddNav("hiringinsights", L.Fa ? "بینش استخدام" : "Hiring insights", "Dashboard.View", "◒");
            AddNav("employerdashboard", L.Fa ? "داشبورد کارفرما" : "Employer dashboard", "Jobs.Manage", "▣");
            AddNav("offerapproval", L.Fa ? "تایید پیشنهادها" : "Offer approvals", "Applications.Manage", "✓");
            AddNav("candidates", L.T("candidates"), "Candidates.View", "♟");
            AddNav("kanban", L.Fa ? "برد استخدام" : "Recruitment board", "Applications.View", "▥");
            AddNav("candidate360", L.Fa ? "پروفایل ۳۶۰" : "Candidate 360", "Candidates.View", "◎");
            AddNav("calendar", L.T("calendar"), "Applications.View", "▣");
            AddNav("scorecards", L.Fa ? "ارزیابی مصاحبه" : "Interview scorecards", "Applications.Manage", "★");
            AddNav("talent", L.T("talent"), "Candidates.View", "◇");
            AddNav("matching", L.T("matching"), "Candidates.View", "↔");
            AddNav("offers", L.T("offers"), "Applications.Manage", "◆");
            AddNav("advancedsearch", L.Fa ? "جستجوی پیشرفته" : "Advanced search", "Candidates.View", "⌕");
            AddNav("reports", L.Fa ? "گزارش‌ها و خروجی" : "Reports & export", "Dashboard.View", "⇩");
            AddNav("companies", L.T("companies"), "Companies.View", "▧");
            AddNav("documents", L.Fa ? "مرکز اسناد" : "Document center", "Candidates.View", "□");
            AddNav("analytics", L.T("analytics"), "Dashboard.View", "⌁");
            AddNav("activity", L.T("activity"), "Users.Manage", "≡");
            AddNav("users", L.T("users"), "Users.Manage", "♙");
            AddNav("permissions", L.Fa ? "ماتریس دسترسی" : "Permission matrix", "Users.Manage", "⊞");
            AddNav("templates", L.Fa ? "قالب‌های ایمیل" : "Email templates", "Users.Manage", "✉");
            AddNav("features", "Feature flags", "Users.Manage", "⚑");
            AddNav("security", L.Fa ? "مرکز امنیت" : "Security center", "Users.Manage", "◆");
            AddNav("sessionsecurity", L.Fa ? "امنیت نشست" : "Session security", "Dashboard.View", "◉");
            AddNav("health", L.T("health"), "Users.Manage", "♥");
            AddNav("backup", L.T("backup"), "Users.Manage", "⇅");
            AddNav("command", L.T("command"), "Dashboard.View", "⌘");
            AddNav("profile", L.Fa ? "پروفایل من" : "My profile", "Dashboard.View", "☺");
            AddNav("settings", L.T("settings"), "Dashboard.View", "⚙");
        }

        void AddNav(string key, string text, string permission, string icon)
        {
            if (!AppSession.Can(permission)) return;
            var b = new Button { Name = "nav_" + key, Tag = key, AccessibleDescription = icon, Text = icon + "   " + text, Height = 44, Width = 214, Margin = new Padding(0, 2, 0, 2), FlatStyle = FlatStyle.Flat, BackColor = navColor, ForeColor = Color.White, Cursor = Cursors.Hand, Font = new Font("Segoe UI", 10), TextAlign = L.Fa ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft, Padding = L.Fa ? new Padding(0,0,12,0) : new Padding(12,0,0,0), UseVisualStyleBackColor = false };
            b.FlatAppearance.BorderSize = 0; b.FlatAppearance.MouseOverBackColor = navHover; b.FlatAppearance.MouseDownBackColor = navActive;
            b.Click += delegate { ShowPage(key); };
            navButtons.Controls.Add(b);
        }

        void StyleNavigation()
        {
            nav.BackColor = navColor; brand.ForeColor = Color.White; roleBadge.ForeColor = Color.FromArgb(190,214,232); navButtons.BackColor = navColor;
            foreach (Control c in navButtons.Controls) { var b = c as Button; if (b != null) { b.ForeColor = Color.White; if (Convert.ToString(b.Tag) != currentPage) b.BackColor = navColor; } }
        }

        void LayoutShell()
        {
            if (ClientSize.Width <= 0 || ClientSize.Height <= 0) return;
            int navW = collapsed ? 78 : 260, topH = 72;
            int navX = L.Fa ? ClientSize.Width - navW : 0;
            nav.SetBounds(navX, 0, navW, ClientSize.Height);
            int mainX = L.Fa ? 0 : navW;
            int mainW = ClientSize.Width - navW;
            top.SetBounds(mainX, 0, mainW, topH);
            content.SetBounds(mainX, topH, mainW, ClientSize.Height - topH);
            brand.SetBounds(0, 12, nav.ClientSize.Width, 54);
            roleBadge.SetBounds(0, 65, nav.ClientSize.Width, 28);
            navButtons.SetBounds(12, 98, Math.Max(48, nav.ClientSize.Width - 24), Math.Max(100, nav.ClientSize.Height - 110));
            int buttonW = collapsed ? 48 : Math.Max(150, navButtons.ClientSize.Width - 22);
            foreach (Control c in navButtons.Controls) c.Width = buttonW;
            collapseButton.Location = L.Fa ? new Point(top.ClientSize.Width - 64, 14) : new Point(18, 14);
            int titleX = L.Fa ? top.ClientSize.Width - 430 : 78;
            pageTitle.SetBounds(Math.Max(80,titleX), 12, 350, 45);
            userInfo.SetBounds(L.Fa ? 18 : Math.Max(450, top.ClientSize.Width - 360), 14, 340, 42);
        }

        void ToggleNavigation()
        {
            collapsed = !collapsed;
            brand.Text = collapsed ? "AJC" : (L.Fa ? "مرکز کاریابی اشکان" : "ASHKAN JOB CENTER");
            roleBadge.Visible = !collapsed;
            foreach (Control c in navButtons.Controls)
            {
                var b = c as Button; if (b == null) continue;
                string icon = b.AccessibleDescription ?? "•";
                b.Text = collapsed ? icon : icon + "   " + NavText(Convert.ToString(b.Tag));
                b.TextAlign = collapsed ? ContentAlignment.MiddleCenter : (L.Fa ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft);
                b.Padding = collapsed ? new Padding(0) : (L.Fa ? new Padding(0,0,12,0) : new Padding(12,0,0,0));
            }
            LayoutShell();
        }

        string NavText(string key)
        {
            switch (key)
            {
                case "quickactions": return L.Fa ? "اقدامات سریع" : "Quick actions"; case "profile": return L.Fa ? "پروفایل من" : "My profile";
                case "overview": return L.Fa ? "نمای مدیریتی" : "Executive overview"; case "recruiterworkspace": return L.Fa ? "میزکار جذب" : "Recruiter workspace"; case "tasks": return L.Fa ? "کارهای جذب" : "Recruitment tasks"; case "compare": return L.Fa ? "مقایسه کارجوها" : "Compare candidates"; case "talentpool": return L.Fa ? "استخر استعداد" : "Talent pool"; case "hiringinsights": return L.Fa ? "بینش استخدام" : "Hiring insights"; case "employerdashboard": return L.Fa ? "داشبورد کارفرما" : "Employer dashboard"; case "offerapproval": return L.Fa ? "تایید پیشنهادها" : "Offer approvals"; case "kanban": return L.Fa ? "برد استخدام" : "Recruitment board";
                case "candidate360": return L.Fa ? "پروفایل ۳۶۰" : "Candidate 360"; case "scorecards": return L.Fa ? "ارزیابی مصاحبه" : "Interview scorecards";
                case "permissions": return L.Fa ? "ماتریس دسترسی" : "Permission matrix"; case "documents": return Candidate ? (L.Fa?"مدارک من":"My documents") : (L.Fa ? "مرکز اسناد" : "Document center");
                case "templates": return L.Fa ? "قالب‌های ایمیل" : "Email templates"; case "features": return "Feature flags";
                case "security": return L.Fa ? "مرکز امنیت" : "Security center"; case "sessionsecurity": return L.Fa ? "امنیت نشست" : "Session security";
                case "savedjobs": return L.Fa ? "فرصت‌های ذخیره‌شده" : "Saved jobs"; case "jobalerts": return L.Fa ? "هشدارهای شغلی" : "Job alerts"; case "timeline": return L.Fa ? "تایم‌لاین درخواست‌ها" : "Application timeline";
                case "advancedsearch": return L.Fa ? "جستجوی پیشرفته" : "Advanced search"; case "reports": return L.Fa ? "گزارش‌ها و خروجی" : "Reports & export";
                default: return L.T(key);
            }
        }

        public void ShowPage(string key)
        {
            currentPage = key; content.Controls.Clear(); Control c;
            switch (key)
            {
                case "quickactions": c = new QuickActionsPage(this); break; case "profile": c = new UserProfilePage(); break; case "overview": c = new ExecutiveOverviewPage(); break; case "recruiterworkspace": c = new RecruiterWorkspacePage(); break; case "tasks": c = new RecruitmentTasksPage(); break; case "compare": c = new CandidateComparisonPage(); break; case "employerdashboard": c = new EmployerDashboardPage(); break; case "offerapproval": c = new OfferApprovalPage(); break; case "talentpool": c = new TalentPoolPage(); break; case "hiringinsights": c = new HiringInsightsPage(); break;
                case "jobs": c = new JobsPage(); break; case "savedjobs": c = new SavedJobsPage(); break; case "jobalerts": c = new JobAlertsPage(); break; case "timeline": c = new ApplicationTimelinePage(); break; case "candidates": c = new CandidatesPage(); break; case "applications": c = new ApplicationsPage(); break;
                case "kanban": c = new KanbanPage(); break; case "candidate360": c = new Candidate360Page(); break; case "companies": c = new CompaniesPage(); break;
                case "interviews": c = new InterviewsPage(); break; case "talent": c = new TalentPage(); break; case "notifications": c = new NotificationsPage(); break;
                case "matching": c = new MatchingPage(); break; case "offers": c = new OffersPage(); break; case "analytics": c = new AnalyticsPage(); break;
                case "activity": c = new ActivityPage(); break; case "users": c = new UsersPage(); break; case "permissions": c = new PermissionMatrixPage(); break;
                case "scorecards": c = new InterviewScorecardPage(); break; case "documents": c = new DocumentCenterPage(); break; case "templates": c = new EmailTemplatePage(); break;
                case "security": c = new SecurityCenterPage(); break; case "advancedsearch": c = new AdvancedSearchPage(); break; case "reports": c = new ReportsPage(); break;
                case "features": c = new FeatureFlagsPage(); break; case "sessionsecurity": c = new SessionSecurityPage(); break; case "command": c = new CommandCenterPage(); break;
                case "calendar": c = new CalendarPage(); break; case "resume": c = new ResumeBuilderPage(); break; case "health": c = new SystemHealthPage(); break;
                case "backup": c = new BackupRestorePage(); break; case "settings": c = new SettingsPage(); break; default: c = Candidate ? (Control)new CandidateHomePage() : new DashboardPage(); break;
            }
            pageTitle.Text = NavText(key); c.Dock = DockStyle.Fill; content.Controls.Add(c);
            foreach (Control n in navButtons.Controls) { var b=n as Button; if(b!=null) b.BackColor=Convert.ToString(b.Tag)==key?navActive:navColor; }
            StyleNavigation();
        }
    }
}
