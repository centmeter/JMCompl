using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace JM.Camera
{
    public class JMCameraManager : MonoBehaviour
    {
        #region Singleton

        private static JMCameraManager _instance;

        private static object _locker = new object();

        public static JMCameraManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            JMCameraManager[] instances = FindObjectsOfType<JMCameraManager>();

                            if (instances != null && instances.Length > 0)
                            {
                                _instance = instances[0];
                            }
                            else if (instances != null && instances.Length > 1)
                            {
                                Debug.LogError("## JM Error ## Cls:JMCameraManager Func:Instance Info:Have more than one instance.Return null");
                            }
                            else
                            {
                                _instance = new GameObject("JMCameraManager(Singleton)").AddComponent<JMCameraManager>();
                            }
                        }
                    }
                }
                return _instance;
            }
        }

        private JMCameraManager()
        {

        }

        #endregion

        #region Variable

        /// <summary>
        /// 摄像头字典
        /// </summary>
        private Dictionary<int, JMCamera> _cameraDic = new Dictionary<int, JMCamera>();

        /// <summary>
        /// 初始化标识
        /// </summary>
        private bool _initDone = false;

        #endregion

        #region Property

        /// <summary>
        /// 设备数
        /// </summary>
        public int DeviceCount
        {
            get
            {
                return WebCamTexture.devices.Length;
            }
        }

        /// <summary>
        /// 设备名集合
        /// </summary>
        public string[] DeviceNames
        {
            get
            {
                WebCamDevice[] devices = WebCamTexture.devices;

                string[] res = new string[devices.Length];

                for (int i = 0; i < devices.Length; i++)
                {
                    res[i] = devices[i].name;
                }

                return res;
            }
        }

        #endregion

        #region Public Func

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize(Action callback = null)
        {
            StartCoroutine(AsyncAuthorize((success) =>
           {
               _initDone = true;

               Debug.Log("## JM Log ## Cls:JMCameraManager Func:Initialize Info:Init success");

               if (callback != null)
               {
                   callback.Invoke();
               }
           }));
        }

        /// <summary>
        /// 获取摄像头
        /// </summary>
        public JMCamera GetCamera(int camId)
        {
            if (!_initDone)
            {
                Debug.LogError("## JM Error ## Cls:JMCameraManager Func:GetCamera Info:Uninitialize");

                return null;
            }

            JMCamera res = null;

            if (_cameraDic != null)
            {
                if (_cameraDic.ContainsKey(camId))
                {
                    res = _cameraDic[camId];
                }
                else
                {
                    res = new JMCamera(camId);

                    _cameraDic[camId] = res;
                }
            }

            return res;
        }

        #endregion

        #region Pirvate Func

        /// <summary>
        /// 异步授权
        /// </summary>
        private IEnumerator AsyncAuthorize(Action<bool> callback = null)
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

            bool success = Application.HasUserAuthorization(UserAuthorization.WebCam);

            if (callback != null)
            {
                callback.Invoke(success);
            }
        }

        #endregion
    }
}
