using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Windows.Forms;
using TSB_Updater.util;

namespace TSB_Updater
{
    public partial class UpdatingForm : Form
    {
        public int Result = UpdateResult.CANCEL;

        private string worldPath;
        private Release release;
        private bool asServer;
        private bool updating = true;

        private UpdateRunner updateRunner;

        public UpdatingForm(string worldPath, Release release, bool asServer = false)
        {
            InitializeComponent();
            this.worldPath = worldPath;
            this.release = release;
            this.asServer = asServer;
        }

        private async void UpdatingForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            updating = true;
            updateRunner = new UpdateRunner(worldPath, release, asServer);
            updateRunner.UpdateProgressChanged += UpdateRunner_onChangeUpdateProgress;
            updateRunner.Completed += UpdateRunner_Completed;
            try
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                await updateRunner.Run();
            }
            catch (Exception)
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
                MessageBox.Show("エラーが発生しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.updating = false;
                this.Result = UpdateResult.FAILED;
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                this.Close();
            }

        }

        private void UpdateRunner_Completed(object sender, EventArgs e)
        {
            updating = false;
            updateRunner.Dispose();
            this.Invoke((MethodInvoker)delegate ()
            {
                this.Result = UpdateResult.OK;
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                this.Close();
            });
        }

        private void UpdateRunner_onChangeUpdateProgress(object sender, UpdateProgressArgs e)
        {
            if (!((UpdateRunner)sender).IsDisposed)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    switch (e.State)
                    {
                        case UpdateState.DownloadingDatapacks:
                            infoLabel.Text = $"更新をダウンロード中 {Decimal.Divide(e.Processed, e.Total) * 100:0}% ({e.Processed / 1000000.0d:0.0}/{e.Total / 1000000.0d:0.0} MB)";
                            break;
                        case UpdateState.Extracting:
                            infoLabel.Text = $"更新を適用中 {Decimal.Divide(e.Processed, e.Total) * 100:0}% ({e.Processed}/{e.Total})";
                            break;
                        case UpdateState.DownloadingResoursepack:
                            infoLabel.Text = $"リソースパックをダウンロード中 {Decimal.Divide(e.Processed, e.Total) * 100:0}% ({e.Processed / 1000000.0d:0.0}/{e.Total / 1000000.0d:0.0} MB)";
                            break;
                    }
                    progressBar.Value = (int)(Decimal.Divide(e.Processed, e.Total) * 100);
                    TaskbarManager.Instance.SetProgressValue((int)(Decimal.Divide(e.Processed, e.Total) * 100), 100);
                });
            }
        }

        private void UpdatingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (updating)
            {
                DialogResult dr = MessageBox.Show("更新を中断してよろしいですか？", "確認", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    updateRunner.Cancel();
                    this.Result = UpdateResult.CANCEL;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
