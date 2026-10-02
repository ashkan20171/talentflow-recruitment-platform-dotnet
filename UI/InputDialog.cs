using System;
using System.Drawing;
using System.Windows.Forms;
using AshkanJobCenter.Core;

namespace AshkanJobCenter.UI
{
    public sealed class InputDialog : Form
    {
        private readonly TextBox input;
        public string Value { get { return input.Text.Trim(); } }

        public InputDialog(string title, string prompt, string initialValue = "")
        {
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(460, 180);
            BackColor = Theme.Background;
            RightToLeft = L.Fa ? RightToLeft.Yes : RightToLeft.No;
            RightToLeftLayout = L.Fa;

            var card = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Surface, Padding = new Padding(22) };
            var label = Ui.Label(prompt, 10, true);
            label.Dock = DockStyle.Top;
            label.Height = 30;
            input = Ui.Text();
            input.Text = initialValue ?? "";
            input.Dock = DockStyle.Top;
            input.Margin = new Padding(0, 8, 0, 12);

            var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 52, FlowDirection = L.Fa ? FlowDirection.RightToLeft : FlowDirection.LeftToRight };
            var ok = Ui.Button(L.Fa ? "تأیید" : "OK");
            var cancel = Ui.Button(L.Fa ? "انصراف" : "Cancel", false);
            ok.DialogResult = DialogResult.OK;
            cancel.DialogResult = DialogResult.Cancel;
            actions.Controls.Add(ok);
            actions.Controls.Add(cancel);

            card.Controls.Add(actions);
            card.Controls.Add(input);
            card.Controls.Add(label);
            Controls.Add(card);
            AcceptButton = ok;
            CancelButton = cancel;
            Shown += delegate { input.Focus(); input.SelectAll(); };
            Theme.Apply(this);
        }

        public static string ShowValue(IWin32Window owner, string title, string prompt, string initialValue = "")
        {
            using (var dialog = new InputDialog(title, prompt, initialValue))
                return dialog.ShowDialog(owner) == DialogResult.OK ? dialog.Value : null;
        }
    }
}
