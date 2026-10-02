using System;
using System.Drawing;
using System.Windows.Forms;
using AshkanJobCenter.Core;
using AshkanJobCenter.Data;

namespace AshkanJobCenter.UI
{
    public class ExecutiveOverviewPage : UserControl
    {
        public ExecutiveOverviewPage()
        {
            Dock = DockStyle.Fill;
            BackColor = Theme.Background;
            Padding = new Padding(8);

            var header = new Panel { Dock = DockStyle.Top, Height = 78 };
            var title = Ui.Label(L.Fa ? "نمای مدیریتی استخدام" : "Recruitment executive overview", 20, true);
            title.Location = new Point(8, 8);
            var subtitle = Ui.Label(L.Fa ? "تصویر لحظه‌ای از قیف جذب، مصاحبه‌ها و پیشنهادهای استخدام" : "Live view of the hiring funnel, interviews and offers", 9);
            subtitle.ForeColor = Theme.Muted;
            subtitle.Location = new Point(8, 42);
            header.Controls.Add(title); header.Controls.Add(subtitle);
            Controls.Add(header);

            var metrics = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 150, AutoScroll = true, WrapContents = false, Padding = new Padding(0, 6, 0, 6) };
            metrics.Controls.Add(Metric(L.Fa ? "فرصت باز" : "Open jobs", "SELECT COUNT(*) FROM Jobs WHERE Status='Open'", Theme.Primary));
            metrics.Controls.Add(Metric(L.Fa ? "در حال بررسی" : "In pipeline", "SELECT COUNT(*) FROM Applications WHERE Stage NOT IN ('Hired','Rejected')", Theme.Accent));
            metrics.Controls.Add(Metric(L.Fa ? "مصاحبه آتی" : "Upcoming interviews", "SELECT COUNT(*) FROM Interviews WHERE ScheduledAt>=GETDATE() AND Status='Scheduled'", Theme.Success));
            metrics.Controls.Add(Metric(L.Fa ? "استخدام شده" : "Hired", "SELECT COUNT(*) FROM Applications WHERE Stage='Hired'", Theme.Success));
            Controls.Add(metrics);

            var body = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Padding = new Padding(0, 8, 0, 0) };
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            body.Controls.Add(Funnel(), 0, 0);
            body.Controls.Add(Upcoming(), 1, 0);
            Controls.Add(body);
            body.BringToFront();
        }

        Panel Metric(string caption, string sql, Color accent)
        {
            var p = Ui.Card(); p.Width = 225; p.Height = 120;
            var bar = new Panel { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            var value = Ui.Label(Convert.ToString(Database.Scalar(sql)), 26, true); value.Location = new Point(22, 45); value.ForeColor = accent;
            var label = Ui.Label(caption, 10, true); label.Location = new Point(22, 17);
            p.Controls.Add(value); p.Controls.Add(label); p.Controls.Add(bar); return p;
        }

        Panel Funnel()
        {
            var p = Ui.Card(); p.Dock = DockStyle.Fill;
            var title = Ui.Label(L.Fa ? "قیف استخدام" : "Hiring funnel", 13, true); title.Dock = DockStyle.Top; title.Height = 36;
            var grid = Ui.Grid();
            grid.DataSource = Database.Query("SELECT Stage AS [Stage], COUNT(*) AS [Count], CAST(AVG(CAST(ISNULL(Score,0) AS FLOAT)) AS DECIMAL(10,1)) AS [Avg Score] FROM Applications GROUP BY Stage ORDER BY COUNT(*) DESC");
            p.Controls.Add(grid); p.Controls.Add(title); return p;
        }

        Panel Upcoming()
        {
            var p = Ui.Card(); p.Dock = DockStyle.Fill;
            var title = Ui.Label(L.Fa ? "مصاحبه‌های پیش رو" : "Upcoming interviews", 13, true); title.Dock = DockStyle.Top; title.Height = 36;
            var grid = Ui.Grid();
            grid.DataSource = Database.Query("SELECT TOP 10 c.FullName AS [Candidate], j.Title AS [Job], i.ScheduledAt AS [When], i.Interviewer AS [Interviewer] FROM Interviews i JOIN Applications a ON a.Id=i.ApplicationId JOIN Candidates c ON c.Id=a.CandidateId JOIN Jobs j ON j.Id=a.JobId WHERE i.ScheduledAt>=GETDATE() ORDER BY i.ScheduledAt");
            p.Controls.Add(grid); p.Controls.Add(title); return p;
        }
    }
}
