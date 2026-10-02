using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AshkanJobCenter.Core
{
    /// <summary>
    /// Central visual asset manager. Stage 19 intentionally uses a text-free photographic
    /// background: application copy is rendered only by WinForms controls, never baked into images.
    /// </summary>
    public static class VisualAssets
    {
        private static Image workspaceBackground;

        public static Image WorkspaceBackground
        {
            get
            {
                if (workspaceBackground != null) return workspaceBackground;
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "talentflow-workspace-background.png");
                if (!File.Exists(path)) return null;
                using (var source = Image.FromFile(path)) workspaceBackground = new Bitmap(source);
                return workspaceBackground;
            }
        }

        public static void ApplyWorkspaceBackground(Control control)
        {
            if (control == null || WorkspaceBackground == null) return;
            control.BackgroundImage = WorkspaceBackground;
            control.BackgroundImageLayout = ImageLayout.Stretch;
        }

        /// <summary>
        /// Keeps the page canvas transparent so the photo can be seen, but deliberately leaves
        /// cards, panels, grids and other content surfaces opaque. This prevents UI text from
        /// visually colliding with the photograph and preserves accessibility/readability.
        /// </summary>
        public static void PrepareReadablePage(Control root)
        {
            if (root == null) return;
            root.BackColor = Color.Transparent;
        }

        // Backward-compatible alias used by older pages/stages. No recursive transparency.
        public static void RevealBackground(Control root)
        {
            PrepareReadablePage(root);
        }
    }
}
