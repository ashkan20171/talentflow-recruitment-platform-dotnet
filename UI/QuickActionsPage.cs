using System;
using System.Drawing;
using System.Windows.Forms;
using AshkanJobCenter.Core;

namespace AshkanJobCenter.UI
{
    public class QuickActionsPage : UserControl
    {
        readonly MainForm host;
        FlowLayoutPanel flow;
        public QuickActionsPage(MainForm owner)
        {
            host=owner; Dock=DockStyle.Fill; BackColor=Theme.Background; Padding=new Padding(20); Build();
        }
        void Build()
        {
            var header=new Panel{Dock=DockStyle.Top,Height=92,BackColor=Theme.Background};
            var title=Ui.Label(L.Fa?"چه کاری می‌خواهید انجام دهید؟":"What would you like to do?",22,true); title.AutoSize=false; title.Dock=DockStyle.Top; title.Height=44; title.TextAlign=L.Fa?ContentAlignment.MiddleRight:ContentAlignment.MiddleLeft;
            var sub=Ui.Label(L.Fa?"دسترسی‌های پرکاربرد متناسب با نقش شما":"Frequently used actions tailored to your role",10); sub.AutoSize=false; sub.Dock=DockStyle.Bottom; sub.Height=36; sub.ForeColor=Theme.Muted; sub.TextAlign=L.Fa?ContentAlignment.MiddleRight:ContentAlignment.MiddleLeft;
            header.Controls.Add(sub);header.Controls.Add(title);Controls.Add(header);
            flow=new FlowLayoutPanel{Dock=DockStyle.Fill,AutoScroll=true,WrapContents=true,BackColor=Theme.Background,Padding=new Padding(0,8,0,0)};Controls.Add(flow);flow.BringToFront();header.BringToFront();
            if(string.Equals(AppSession.Role,"Candidate",StringComparison.OrdinalIgnoreCase))
            {
                Add("▦",L.Fa?"فرصت‌های شغلی":"Browse jobs",L.Fa?"فرصت‌های باز را ببینید و موقعیت مناسب را پیدا کنید":"Explore open positions and find a suitable role","jobs");
                Add("✓",L.Fa?"درخواست‌های من":"My applications",L.Fa?"وضعیت درخواست‌ها و مراحل استخدام را پیگیری کنید":"Track your applications and hiring stages","applications");
                Add("◷",L.Fa?"مصاحبه‌ها":"Interviews",L.Fa?"زمان و جزئیات مصاحبه‌های پیش رو را مرور کنید":"Review upcoming interview details","interviews");
                Add("▤",L.Fa?"تکمیل رزومه":"Build resume",L.Fa?"مهارت‌ها و اطلاعات حرفه‌ای خود را تکمیل کنید":"Keep your skills and professional profile up to date","resume");
                Add("●",L.Fa?"اعلان‌ها":"Notifications",L.Fa?"پیام‌ها و تغییرات مهم فرایند استخدام را ببینید":"See important hiring updates and messages","notifications");
                Add("☺",L.Fa?"پروفایل من":"My profile",L.Fa?"اطلاعات حساب و تنظیمات شخصی را مدیریت کنید":"Manage account and personal settings","profile");
            }
            else
            {
                if(AppSession.Can("Jobs.Manage")) Add("＋",L.Fa?"فرصت شغلی جدید":"New job",L.Fa?"یک فرصت جدید ایجاد و فرایند جذب را آغاز کنید":"Create a vacancy and start recruiting","jobs");
                if(AppSession.Can("Candidates.View")) Add("◎",L.Fa?"جستجوی استعداد":"Talent search",L.Fa?"کارجویان را بر اساس مهارت و تجربه بررسی کنید":"Search candidates by skill and experience","talent");
                if(AppSession.Can("Applications.View")) Add("▥",L.Fa?"برد استخدام":"Recruitment board",L.Fa?"مراحل جاری فرایند استخدام را مدیریت کنید":"Manage the current hiring pipeline","kanban");
                if(AppSession.Can("Applications.View")) Add("◷",L.Fa?"مصاحبه‌های امروز":"Today's interviews",L.Fa?"برنامه و ارزیابی مصاحبه‌ها را مرور کنید":"Review schedule and interview scorecards","interviews");
                Add("⇩",L.Fa?"گزارش و خروجی":"Reports & export",L.Fa?"گزارش‌های عملیاتی و خروجی داده تهیه کنید":"Create operational reports and data exports","reports");
                if(AppSession.Can("Users.Manage")) Add("⚙",L.Fa?"کنترل دسترسی":"Access control",L.Fa?"نقش‌ها و مجوزهای سامانه را مدیریت کنید":"Manage roles and permissions","permissions");
            }
        }
        void Add(string icon,string name,string desc,string target)
        {
            var p=new Panel{Width=330,Height=160,BackColor=Color.White,Margin=new Padding(8),Padding=new Padding(18),Cursor=Cursors.Hand};
            var i=Ui.Label(icon,24,true);i.ForeColor=Theme.Primary;i.Dock=DockStyle.Top;i.Height=42;i.AutoSize=false;
            var n=Ui.Label(name,13,true);n.Dock=DockStyle.Top;n.Height=34;n.AutoSize=false;n.TextAlign=L.Fa?ContentAlignment.MiddleRight:ContentAlignment.MiddleLeft;
            var d=Ui.Label(desc,9);d.ForeColor=Theme.Muted;d.Dock=DockStyle.Fill;d.AutoSize=false;d.TextAlign=L.Fa?ContentAlignment.TopRight:ContentAlignment.TopLeft;
            p.Controls.Add(d);p.Controls.Add(n);p.Controls.Add(i);
            EventHandler go=delegate{host.ShowPage(target);};p.Click+=go;i.Click+=go;n.Click+=go;d.Click+=go;
            p.MouseEnter+=delegate{p.BackColor=Color.FromArgb(249,252,255);};p.MouseLeave+=delegate{p.BackColor=Color.White;};
            flow.Controls.Add(p);
        }
    }
}
