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
                            JMCameraManager[] jMCameras = FindObjectsOfType<JMCameraManager>();

                            if (jMCameras != null && jMCameras.Length > 0)
                            {
                                _instance = jMCameras[0];

                                if (jMCameras.Length > 1)
                                {
                                    Debug.LogError("## JM Error ## Cls:JMCameraManager Func:Instance Info:JMCameraController is not singleton");
                                }
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

        #endregion

        #region Variable

        /// <summary>
        /// 摄像头字典
        /// </summary>
        private Dictionary<int, JMCamera> _cameraDic;

        /// <summary>
        /// 初始化标识
        /// </summary>
        private bool _initDone = false;

        #endregion

        #region Public Func

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize(Action callback = null)
        {
            _cameraDic = new Dictionary<int, JMCamera>();

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
