using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSB_Updater.util
{
    public static class UpdaterHelper
    {
        public static bool IsTSBFolder(string folderPath)
        {
            return (Directory.Exists($@"{folderPath}\datapacks\TheSkyBlock") || Directory.Exists($@"{folderPath}\datapacks\TheSkyBlessing")) && File.Exists($@"{folderPath}\Readme.txt");
        }
        public static string GetCurrentVersion(string folderPath)
        {
            var TSBDFolder = "TheSkyBlock";
            // v0.0.3以降
            if (Directory.Exists($@"{folderPath}\datapacks\TheSkyBlessing"))
            {
                TSBDFolder = "TheSkyBlessing";
            }
            var path = $@"{folderPath}\datapacks\{TSBDFolder}\data\core\functions\load_once.mcfunction"; // バージョン取得用ファイル
            var triggerString = "data modify storage global GameVersion set value "; // バージョン取得用
            foreach (string line in File.ReadLines(path))
            {
                var i = line.IndexOf(triggerString);
                if (i != -1)
                {
                    string[] e = line.Split(triggerString);
                    return e[1].Replace("\"", "").Replace("v", "");
                }
            }
            return "unknown";
        }

        public static bool CheckUsedServer(string folderPath)
        {
            string outerPath = folderPath.Substring(0, folderPath.LastIndexOf(@"\"));
            return File.Exists($@"{outerPath}\server.properties");
        }

        public static Release GetUpdatableLatestRlease(string currentVersion, List<Release> releases)
        {
            var cv = Version.Parse(currentVersion);
            foreach (var e in releases)
            {
                var ev = Version.Parse(e.Version);
                int c = cv.CompareTo(ev);
                if (c != 1 && cv.Major == ev.Major && cv.Minor == cv.Minor)
                {
                    return e;
                }
            }
            return null;
        }

        public static Task Update(string folderPath, Release release, Label infoLabel, ProgressBar progressBar, Action callback)
        {
            return Task.Run(() =>
            {
                var wc = new WebClient();
                string zipPath = $@"{Path.GetTempPath()}\datapacks.zip";
                // datapacks.zipダウンロード
                wc.DownloadFile(new Uri(release.DatapackUrl), zipPath);
                // datapacksフォルダ削除
                Directory.Delete($@"{folderPath}\datapacks", true);
                // datapacks.zip 解凍
                ZipFile.ExtractToDirectory(zipPath, $@"{folderPath}\datapacks", true);
                // 完了時callback
                callback();
            });
        }
    }

    public static class UpdateResult
    {
        public const int OK = 0;
        public const int FAILED = 1;
        public const int CANCEL = 2;
    }
}
