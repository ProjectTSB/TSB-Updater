using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace TSB_Updater.util
{
    public class UpdateRunner : IDisposable
    {
        public event EventHandler Completed;
        public event EventHandler<UpdateProgressArgs> UpdateProgressChanged;

        public bool IsDisposed { get; private set; }
        public string WorldFolderPath { get; private set; }
        public Release Release { get; private set; }

        private WebClient wc = new WebClient();
        private Task task;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        public UpdateRunner(string folderPath, Release release)
        {
            this.WorldFolderPath = folderPath;
            this.Release = release;
        }

        public void Dispose()
        {
            wc.Dispose();
            this.IsDisposed = true;
        }

        public void Cancel()
        {
            tokenSource.Cancel();
        }

        public Task Run()
        {
            var token = tokenSource.Token;
            task = new Task(() =>
            {
                // データパックのダウンロード開始
                StartDownloadDatapacks();
                if (token.IsCancellationRequested)
                {
                    return;
                }
            }, token);
            task.Start();
            return task;
        }

        private void StartDownloadDatapacks()
        {
            wc.DownloadProgressChanged += DownloadDatapacks_ProgressChanged;
            wc.DownloadFileCompleted += DownloadDatapacks_Completed;
            wc.DownloadFileAsync(new Uri(this.Release.DatapackUrl), $@"{Path.GetTempPath()}\datapacks.zip");
        }

        private void StartExtractDatapacks()
        {
            // datapacksフォルダ削除
            Directory.Delete($@"{WorldFolderPath}\datapacks", true);
            // 解凍
            var zipProgress = new Progress<ZipProgress>();
            zipProgress.ProgressChanged += zipProgress_ProgressChanged;
            var zip = new ZipArchive(File.OpenRead($@"{Path.GetTempPath()}\datapacks.zip"));
            zip.ExtractToDirectory($@"{this.WorldFolderPath}\datapacks", zipProgress);
            zip.Dispose();
            // 完了
            Completed(this, null);
        }

        private void StartDownloadResorcepack()
        {
            wc.DownloadProgressChanged += DownloadResourcepack_ProgressChanged;
            wc.DownloadFileCompleted += DownloadResourcepack_Completed;
            wc.DownloadFileAsync(new Uri(this.Release.ResorcePackUrl), $@"{this.WorldFolderPath}\resources.zip");
        }

        private void zipProgress_ProgressChanged(object sender, ZipProgress e)
        {
            var updateProgressAges = new UpdateProgressArgs(UpdateState.Extracting, e.Total, e.Processed);
            UpdateProgressChanged(this, updateProgressAges);
        }

        private void DownloadDatapacks_ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var updateProgressAges = new UpdateProgressArgs(UpdateState.DownloadingDatapacks, e.TotalBytesToReceive, e.BytesReceived);
            UpdateProgressChanged(this, updateProgressAges);
        }

        private void DownloadDatapacks_Completed(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            // 削除
            wc.DownloadProgressChanged -= DownloadDatapacks_ProgressChanged;
            wc.DownloadFileCompleted -= DownloadDatapacks_Completed;
            // resources.zip ダウンロード開始
            StartDownloadResorcepack();
        }

        private void DownloadResourcepack_ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var updateProgressAges = new UpdateProgressArgs(UpdateState.DownloadingResoursepack, e.TotalBytesToReceive, e.BytesReceived);
            UpdateProgressChanged(this, updateProgressAges);
        }

        private void DownloadResourcepack_Completed(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            StartExtractDatapacks();
        }

    }

    public class UpdateProgressArgs
    {
        public int State { get; set; }
        public long Total { get; set; }
        public long Processed { get; set; }

        public UpdateProgressArgs(int state, long total, long processed)
        {
            this.State = state;
            this.Total = total;
            this.Processed = processed;
        }
    }

    public class UpdateState
    {
        public const int DownloadingDatapacks = 0;
        public const int DownloadingResoursepack = 1;
        public const int Extracting = 2;
        public const int Completed = 3;
    }
}
