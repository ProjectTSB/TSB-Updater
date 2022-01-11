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
        private bool updating = true;

        private UpdateRunner updateRunner;

        public UpdatingForm(string worldPath, Release release)
        {
            InitializeComponent();
            this.worldPath = worldPath;
            this.release = release;
        }

        private async void UpdatingForm_Load(object sender, EventArgs e)
        {
            updating = true;
            updateRunner = new UpdateRunner();
            updateRunner.UpdateProgressChanged += UpdateRunner_onChangeUpdateProgress;
            updateRunner.Completed += UpdateRunner_onComplete;
            try
            {
                await updateRunner.Run(worldPath, release);
            }
            catch (Exception)
            {
                MessageBox.Show("エラーが発生しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Result = UpdateResult.FAILED;
                this.Close();
            }

        }

        private void UpdateRunner_onComplete(object sender, EventArgs e)
        {
            updating = false;
            updateRunner.Dispose();
            this.Invoke((MethodInvoker)delegate ()
            {
                this.Result = UpdateResult.OK;
                this.Close();
            });
        }

        private void UpdateRunner_onChangeUpdateProgress(object sender, UpdateProgressArgs e)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                switch (e.State)
                {
                    case UpdateState.Downloading:
                        infoLabel.Text = $"更新をダウンロード中 {Decimal.Divide(e.Processed, e.Total) * 100:0}% ({e.Processed / 1000000.0d:0.0}/{e.Total / 1000000.0d:0.0} MB)";
                        break;
                    case UpdateState.Extracting:
                        infoLabel.Text = $"更新を適用中 {Decimal.Divide(e.Processed, e.Total) * 100:0}% ({e.Processed}/{e.Total})";
                        break;
                }
                progressBar.Value = (int)(Decimal.Divide(e.Processed, e.Total) * 100);
            });
        }

        private void UpdatingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (updating)
            {
                DialogResult dr = MessageBox.Show("更新を中断してよろしいですか？", "確認", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
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
