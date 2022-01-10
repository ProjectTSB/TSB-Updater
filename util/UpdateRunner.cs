using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;

namespace TSB_Updater.util
{
    public class UpdateRunner : IDisposable
    {
        public event EventHandler onComplete;
        public event EventHandler<UpdateProgressArgs> onChangeUpdateProgress;

        private WebClient wc = new WebClient();

        public void Dispose()
        {
            wc.Dispose();
        }

        public Task Run(string folderPath, Release release)
        {
            return Task.Run(() =>
            {
                string zipPath = $@"{Path.GetTempPath()}\datapacks.zip";
                // datapacks.zipダウンロード
                wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += delegate (object sender, System.ComponentModel.AsyncCompletedEventArgs e)
                {
                    // datapacksフォルダ削除
                    Directory.Delete($@"{folderPath}\datapacks", true);
                    // datapacks.zip 解凍
                    var zipProgress = new Progress<ZipProgress>();
                    zipProgress.ProgressChanged += zipProgress_ProgressChanged;
                    var zip = new ZipArchive(File.OpenRead(zipPath));
                    zip.ExtractToDirectory($@"{folderPath}\datapacks", zipProgress);
                    zip.Dispose();
                    // バージョンファイル書き換え
                    File.WriteAllText($@"{folderPath}\version", release.Version);
                    // 完了
                    onComplete(this, null);
                };
                wc.DownloadFileAsync(new Uri(release.DatapackUrl), zipPath);
                wc.Dispose();
            });
        }

        private void zipProgress_ProgressChanged(object sender, ZipProgress e)
        {
            var updateProgressAges = new UpdateProgressArgs(UpdateState.Extracting, (int)(e.Processed / (double)e.Total * 100));
            onChangeUpdateProgress(this, updateProgressAges);
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var updateProgressAges = new UpdateProgressArgs(UpdateState.Downloading, e.ProgressPercentage);
            onChangeUpdateProgress(this, updateProgressAges);
        }
    }

    public class UpdateProgressArgs
    {
        public int State { get; set; }
        public int ProgressPercentage { get; set; }

        public UpdateProgressArgs(int state, int progressPercentage)
        {
            this.State = state;
            this.ProgressPercentage = progressPercentage;
        }
    }

    public class UpdateState
    {
        public const int Downloading = 0;
        public const int Extracting = 1;
        public const int Completed = 2;
    }
}
