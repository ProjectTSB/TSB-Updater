using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace TSB_Updater.util
{
    public static class GitHubHelper
    {
        private static string TSBRepository = "ProjectTSB/TheSkyBlessing";
        private static string ResourcePackRepository = "ProjectTSB/TSB-ResourcePack";

        // リリース一覧を取得する バージョンが新しい順に返す
        public static List<Release> GetReleases()
        {
            var releases = new List<Release>() { };
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.142 Safari/537.36");
            HttpResponseMessage response = client.GetAsync($"https://api.github.com/repos/{TSBRepository}/releases").Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string res = response.Content.ReadAsStringAsync().Result;
                var resJson = ((JArray)JsonConvert.DeserializeObject(res)).ToObject<List<Dictionary<string, dynamic>>>();
                resJson.ForEach(delegate (Dictionary<string, dynamic> e)
                {
                    var release = new Release
                    {
                        Version = ((string)e["tag_name"]).Substring(1),
                        Details = e["body"]
                    };
                    release.DatapackUrl = $"https://github.com/{TSBRepository}/releases/download/v{release.Version}/datapacks.zip";
                    release.WorldUrl = $"https://github.com/{TSBRepository}/releases/download/v{release.Version}/TheSkyBlessing.zip";
                    release.Url = $"https://github.com/{TSBRepository}/releases/tag/v{release.Version}";
                    release.ResorcePackUrl = $"https://github.com/{ResourcePackRepository}/releases/download/v{release.Version}/resources.zip";
                    releases.Add(release);
                });
            }
            releases.Sort((a, b) =>
            {
                return Version.Parse(b.Version).CompareTo(Version.Parse(a.Version));
            });
            return releases;
        }
    }
}
