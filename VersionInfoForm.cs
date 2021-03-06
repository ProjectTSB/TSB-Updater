using System;
using System.Diagnostics;
using System.Windows.Forms;
using TSB_Updater.util;

namespace TSB_Updater
{
    public partial class VersionInfoForm : Form
    {

        public int Result = UpdateResult.CANCEL;

        private string worldPath;
        private Release release;
        private bool asServer;

        public VersionInfoForm(string worldPath, string currentVersion, Release release, bool asServer = false)
        {
            InitializeComponent();
            this.worldPath = worldPath;
            this.release = release;
            this.asServer = asServer;
            currentVersionLabel.Text = $"現在のバージョン v{currentVersion}";
            label1.Text = $"新しいバージョン(v{release.Version})が利用可能です";
        }

        private async void VersionInfoForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            var html = Markdig.Markdown.ToHtml($"# v{release.Version}  \n" + release.Details);
            await webView.EnsureCoreWebView2Async();
            webView.NavigateToString(html);
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            var dialog = new UpdatingForm(worldPath, release, asServer);
            dialog.ShowDialog();
            this.Result = dialog.Result;
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo pi = new ProcessStartInfo()
            {
                FileName = release.Url,
                UseShellExecute = true,
            };

            Process.Start(pi);
        }
    }
}
