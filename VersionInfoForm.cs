using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TSB_Updater.util;
using static TSB_Updater.util.UpdaterHelper;

namespace TSB_Updater
{
    public partial class VersionInfoForm : Form
    {

        public int Result = UpdateResult.CANCEL;

        private string worldPath;
        private Release release;

        public VersionInfoForm(string worldPath, Release release)
        {
            InitializeComponent();
            this.worldPath = worldPath;
            this.release = release;
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
            var dialog = new UpdatingForm(worldPath, release);
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
