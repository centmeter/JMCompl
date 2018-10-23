﻿using System;
using System.IO;
using UnityEngine;

namespace JMEditor.Resource
{
    internal static class StrDef
    {
        /// <summary>
        /// Roaming下应用的文件夹
        /// </summary>
        private static string ROAMING_PRODUCT = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.productName);

        /// <summary>
        /// 构建AB包
        /// </summary>
        public const string JM_BUILD_ASSETBUNDLE = "Tools/JM/AssetBundle/Build #B";

        /// <summary>
        /// 设置打AB包的配置
        /// </summary>
        public const string JM_SETTING_ASSETBUNDLE = "Tools/JM/AssetBundle/Setting #S";

        /// <summary>
        /// AB包构建配置路径
        /// </summary>
        public static string JM_ASSETBUNDLE_SETTING_PATH = Path.Combine(ROAMING_PRODUCT, "JM/Editor/AssetBundleBuilderSetting.dat");

        /// <summary>
        /// AB包默认输出目录
        /// </summary>
        public static string JM_ASSETBUNDLE_DEFAULT_OUTPUTDIR = Path.Combine(Application.streamingAssetsPath, "AssetBundle");

        public const string ASSETBUNDLE_OPTION_NONE = "Default";

        public const string ASSETBUNDLE_OPTION_UNCOMPRESSED = "Uncompressed";

        public const string ASSETBUNDLE_OPTION_DEPENDECY = "CollectDependencies";

        public const string ASSETBUNDLE_OPTION_CHUNKBASE = "ChunkBasedCompressed";
    }
}
