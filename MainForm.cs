using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TSB_Updater.util;

namespace TSB_Updater
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.icon;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            worldPath.Text = $@"C:\Users\{Environment.UserName}\AppData\Roaming\.minecraft\saves";
        }

        private void browsButton_Click(object sender, EventArgs e)
        {
            worldFolderBrowserDialog.SelectedPath = worldPath.Text;
            if (worldFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                worldPath.Text = worldFolderBrowserDialog.SelectedPath;
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            List<Release> releases; // リリース情報
            string currentVersion;

            if (Process.GetProcessesByName("javaw").Length > 0)
            {
                MessageBox.Show("不具合を避けるため、Minecraftを終了してから実行してください。", "Minecraftの起動を検知しました", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(worldPath.Text))
            {
                MessageBox.Show("フォルダーが存在しません。\nワールドフォルダーを指定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!UpdaterHelper.IsTSBFolder(worldPath.Text))
            {
                MessageBox.Show("The Sky Blessingのワールドフォルダーを指定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            currentVersion = UpdaterHelper.GetCurrentVersion(worldPath.Text);

            if (currentVersion == "unknown")
            {
                MessageBox.Show("バージョン情報の取得に失敗しました。\n指定したフォルダーが正しいか確認してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                releases = GitHubHelper.GetReleases();
            }
            catch (Exception)
            {
                MessageBox.Show("リリース情報の取得に失敗しました。\n手動でアップデートしてください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var latestRelease = UpdaterHelper.GetUpdatableLatestRlease(currentVersion, releases);
            if (latestRelease == null)
            {
                MessageBox.Show("利用可能な更新はありません。\n手動でアップデートしてください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (latestRelease.Version == currentVersion)
            {
                if (Version.Parse(releases[0].Version).CompareTo(Version.Parse(latestRelease.Version)) == 1)
                {
                    DialogResult result = MessageBox.Show($"最新バージョン(v{releases[0].Version})が公開されています。\nこの更新は現在のワールドと互換性が無いため、手動でダウンロードする必要があります。\nダウンロードページを開きますか？", "情報", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        ProcessStartInfo pi = new ProcessStartInfo()
                        {
                            FileName = releases[0].Url,
                            UseShellExecute = true,
                        };
                        Process.Start(pi);
                    }
                    return;
                }
                else
                {
                    MessageBox.Show($"すでに最新バージョン(v{currentVersion})を利用しています。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            var vif = new VersionInfoForm(worldPath.Text, currentVersion, latestRelease);

            vif.ShowDialog();

            if (vif.Result == UpdateResult.OK)
            {
                MessageBox.Show("アップデートが完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (Version.Parse(releases[0].Version).CompareTo(Version.Parse(latestRelease.Version)) == 1)
            {
                DialogResult result = MessageBox.Show($"最新バージョン(v{releases[0].Version})が公開されています。\nこの更新は現在のワールドと互換性が無いため、手動でダウンロードする必要があります。\nダウンロードページを開きますか？", "情報", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    ProcessStartInfo pi = new ProcessStartInfo()
                    {
                        FileName = releases[0].Url,
                        UseShellExecute = true,
                    };
                    Process.Start(pi);
                }
                return;
            }
        }
    }
}
