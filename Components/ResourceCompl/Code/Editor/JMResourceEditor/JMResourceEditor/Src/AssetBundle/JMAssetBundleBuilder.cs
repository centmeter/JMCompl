﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace JMEditor.Resource.AssetBundle
{
    internal static class JMAssetBundleBuilder
    {
        /// <summary>
        /// 配置路径
        /// </summary>
        private static string _configPath = StrDef.JM_ASSETBUNDLE_SETTING_PATH;

        /// <summary>
        /// 设置模型
        /// </summary>
        private static JMAssetBundleSettingModel _settingModel;

        /// <summary>
        /// 构建AB包
        /// </summary>
        [MenuItem(StrDef.JM_BUILD_ASSETBUNDLE)]
        private static void BuildAssetBundles()
        {
            LoadConfig((setting) =>
            {
                if (!Directory.Exists(setting.outputDir))
                {
                    Directory.CreateDirectory(setting.outputDir);
                }

                BuildAssetBundleOptions options = BuildAssetBundleOptions.None;

                for (int i = 0; i < setting.options.Length; i++)
                {
                    options |= (BuildAssetBundleOptions)setting.options[i];
                }

                BuildTarget buildTarget = (BuildTarget)setting.targetPlatform;

                if (buildTarget != EditorUserBuildSettings.activeBuildTarget)
                {
                    Debug.LogWarningFormat("## JM Warning ## Cls:JMAssetBundleBuilder Func:BuildAssetBundles Info:AssetBundle buildTarget isn't current project's buildTarget.");
                }

                BuildPipeline.BuildAssetBundles(setting.outputDir, options, (BuildTarget)setting.targetPlatform);
            });
        }

        /// <summary>
        /// 设置AB包
        /// </summary>
        [MenuItem(StrDef.JM_SETTING_ASSETBUNDLE)]
        private static void SettingAssetBundle()
        {
            if (_settingModel != null && _settingModel.View != null)
            {
                ViewManager.Close(_settingModel);
            }

            LoadConfig((setting) =>
            {
                _settingModel = new JMAssetBundleSettingModel();

                _settingModel.OutputDir = setting.outputDir;

                List<BuildAssetBundleOptions> optionList = new List<BuildAssetBundleOptions>();

                for (int i = 0; i < setting.options.Length; i++)
                {
                    optionList.Add((BuildAssetBundleOptions)setting.options[i]);
                }

                _settingModel.Options = optionList;

                _settingModel.Target = (BuildTarget)setting.targetPlatform;

                _settingModel.OnSaveEvent += () =>
                {
                    SaveConfig();

                    ViewManager.Close(_settingModel);

                    _settingModel = null;
                };

                _settingModel.OnSaveBuildEvent += () =>
                {
                    SaveConfig();

                    ViewManager.Close(_settingModel);

                    _settingModel = null;

                    BuildAssetBundles();
                };

                ViewManager.Show<JMAssetBundleSettingView>(_settingModel);
            });
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        private static void LoadConfig(Action<JMAssetBundleSetting> callback)
        {
            JMAssetBundleSetting res = GetDefaultConfig();

            try
            {
                if (File.Exists(_configPath))
                {
                    string configStr = File.ReadAllText(_configPath);

                    string[] configs = configStr.Split(';');

                    if (configs != null && configs.Length == 3)
                    {
                        string lineOption = configs[1];

                        string[] lineOptions = lineOption.Split(',');

                        int[] options = new int[lineOptions.Length];

                        for (int i = 0; i < lineOptions.Length; i++)
                        {
                            options[i] = int.Parse(lineOptions[i]);
                        }

                        res = new JMAssetBundleSetting()
                        {
                            outputDir = configs[0],
                            options = options,
                            targetPlatform = int.Parse(configs[2])
                        };
                    }
                }
            }
            catch
            {
                Debug.LogWarningFormat("## JM Warning ## Cls:JMAssetBundleBuilder Func:LoadConfig Info:Load setting fail,use default setting.Please set again.");
            }

            if (callback != null)
            {
                callback.Invoke(res);
            }
        }

        /// <summary>
        /// 获取默认设置
        /// </summary>
        /// <returns></returns>
        private static JMAssetBundleSetting GetDefaultConfig()
        {
            JMAssetBundleSetting res = new JMAssetBundleSetting()
            {
                outputDir = StrDef.JM_ASSETBUNDLE_DEFAULT_OUTPUTDIR,

                options = new int[] { (int)BuildAssetBundleOptions.None },

                targetPlatform = (int)BuildTarget.StandaloneWindows
            };

            return res;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private static void SaveConfig()
        {
            try
            {
                if (_settingModel != null)
                {
                    string outputDir = _settingModel.OutputDir;

                    List<BuildAssetBundleOptions> options = _settingModel.Options;

                    StringBuilder optionsBuilder = new StringBuilder();

                    for (int i = 0; i < options.Count; i++)
                    {
                        optionsBuilder.Append((int)options[i]);

                        if (i < options.Count - 1)
                        {
                            optionsBuilder.Append(",");
                        }
                    }

                    int targetPlatform = (int)_settingModel.Target;

                    string configStr = string.Format("{0};{1};{2}", outputDir, optionsBuilder, targetPlatform);

                    string configDir = Path.GetDirectoryName(_configPath);

                    if (!Directory.Exists(configDir))
                    {
                        Directory.CreateDirectory(configDir);
                    }

                    File.WriteAllText(_configPath, configStr);
                }
            }
            catch
            {
                Debug.LogErrorFormat("## JM Error ## Cls:JMAssetBundleBuilder Func:SaveConfig Info:Save setting fail.");
            }
        }
    }
}

/// <summary>
/// AB包构建设置
/// </summary>
internal struct JMAssetBundleSetting
{
    /// <summary>
    /// 输出目录
    /// </summary>
    public string outputDir;

    /// <summary>
    /// 打包选项
    /// </summary>
    public int[] options;

    /// <summary>
    /// 目标平台
    /// </summary>
    public int targetPlatform;

    public override string ToString()
    {
        StringBuilder res = new StringBuilder();

        res.Append("OutputDir:");

        res.Append(outputDir);

        res.Append("\r\n");

        res.Append("BuildAssetBundleOptions:");

        if (options != null)
        {
            for (int i = 0; i < options.Length; i++)
            {
                res.Append("\r\n");

                res.Append((BuildAssetBundleOptions)options[i]);
            }
        }

        res.Append("\r\n");

        res.Append("TargetPlatform:");

        res.Append((BuildTarget)targetPlatform);

        return res.ToString();

    }
}
