using System;
using System.Collections.Generic;
using UnityEngine;

namespace JM.UICompl
{
    /// <summary>
    /// UI管理
    /// </summary>
    public class JMUIManager
    {
        #region Singleton

        private static JMUIManager _instance;
        private static object _locker = new object();
        /// <summary>
        /// 实例 
        /// </summary>
        public static JMUIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new JMUIManager();
                        }
                    }
                }
                return _instance;
            }
        }
        private JMUIManager()
        {

        }

        #endregion

        #region Variable

        /// <summary>
        /// 画布
        /// </summary>
        private Transform _canvasTrans;

        /// <summary>
        /// UI界面字典
        /// </summary>
        private Dictionary<string, JMUIBase> _uiDic = new Dictionary<string, JMUIBase>();

        /// <summary>
        /// UI预制体在Resources下的目录
        /// </summary>
        private string _uiPrefabResDir;
        #endregion

        #region Public Func

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize(string prefabResDir = null, Transform canvas = null)
        {
            if (canvas != null)
            {
                this._canvasTrans = canvas;
            }
            else
            {
                GameObject canvasObj = GameObject.Find("Canvas");
                if (canvasObj != null)
                {
                    this._canvasTrans = canvasObj.transform;
                }
                else
                {
                    throw new Exception("## Uni Excepiton ## Author:<Ming> Cls:JMUIManager Func:Initialize Exception:Find canvas is null");
                }
            }
            if (!string.IsNullOrEmpty(prefabResDir))
            {
                this._uiPrefabResDir = prefabResDir;
            }
            else
            {
                this._uiPrefabResDir = "JMUIPrefab";
            }
        }

        /// <summary>
        /// 进入UI界面
        /// </summary>
        public T UIEnter<T>() where T : JMUIBase
        {
            T t = null;
            string name = typeof(T).ToString();
            if (_uiDic.ContainsKey(name))
            {
                t = _uiDic[name] as T;
            }
            else
            {
                GameObject uiObj = Resources.Load<GameObject>(string.Format("{0}/{1}", _uiPrefabResDir, typeof(T).ToString()));
                if (uiObj != null)
                {
                    if (_canvasTrans != null)
                    {
                        uiObj.transform.SetParent(_canvasTrans);
                    }
                    RectTransform rect = uiObj.transform as RectTransform;
                    if (rect != null)
                    {
                        rect.offsetMin = Vector2.zero;
                        rect.offsetMax = Vector2.zero;
                        rect.anchorMin = Vector2.zero;
                        rect.anchorMax = Vector2.one;
                    }
                }
                else
                {
                    throw new Exception(string.Format("## Uni Exception ## Author:<Ming> Cls:JMUIManager Func:UIEnter Exception:[{0}] uiObj is null", typeof(T).ToString()));
                }
            }
        }

        #endregion

    }
}
