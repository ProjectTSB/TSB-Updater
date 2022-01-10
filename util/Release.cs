﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TSB_Updater.util
{
    // リリース情報
    public class Release
    {
        public string Version { get; set; } // バージョン
        public string Details { get; set; } // 詳細(bodyにあたるマークダウン)
        public string Url { get; set; } // リリースのURL
        public string DatapackUrl { get; set; } // データパックのダウンロードURL
        public string WorldUrl { get; set; } // ワールドファイルのダウンロードURL

    }
}