
using System;
using UnityEngine;

namespace JM.Camera
{
    public class JMCamera
    {
        #region Variable

        /// <summary>
        /// 摄像头标识ID
        /// </summary>
        private int _camId;

        /// <summary>
        /// 摄像头纹理
        /// </summary>
        private WebCamTexture _camTex;

        #endregion

        #region Property

        /// <summary>
        /// 有效性
        /// </summary>
        private bool IsValid
        {
            get
            {
                bool res = false;

                WebCamDevice[] devices = WebCamTexture.devices;

                if (devices != null && _camId >= 0 && _camId < devices.Length)
                {
                    res = true;
                }
                else
                {
                    Debug.LogWarningFormat("## JM Warning ## Cls:JMCamera Func:IsValid CamId:{0} Info:Invalid", _camId);
                }

                return res;
            }
        }

        /// <summary>
        /// 设备名字
        /// </summary>
        private string DeviceName
        {
            get
            {
                string res = string.Empty;

                if (IsValid)
                {
                    res = WebCamTexture.devices[_camId].name;
                }

                return res;
            }
        }

        /// <summary>
        /// 摄像头标识ID
        /// </summary>
        public int CamId
        {
            get
            {
                return _camId;
            }
        }

        /// <summary>
        /// 摄像头纹理
        /// </summary>
        public WebCamTexture CameraTexture
        {
            get
            {
                return _camTex;
            }
        }

        #endregion

        #region Public Func

        internal JMCamera(int camId)
        {
            _camId = camId;

            _camTex = new WebCamTexture(DeviceName);
        }

        /// <summary>
        /// 打开
        /// </summary>
        public void Open()
        {
            if (!IsValid)
            {
                return;
            }

            if (_camTex != null)
            {
                if (!_camTex.isPlaying)
                {
                    _camTex.Play();
                }
                else
                {
                    Debug.LogWarningFormat("## JM Warning ## Cls:JMCamera Func:Open CamId:{0} Info:Has opened", _camId);
                }
            }
        }

        /// <summary>
        /// 打开
        /// </summary>
        public void Open(int resolutionX, int resolutionY, int fps = 30)
        {
            if (!IsValid)
            {
                return;
            }

            if (_camTex != null)
            {
                if (!_camTex.isPlaying)
                {
                    _camTex.requestedWidth = resolutionX;

                    _camTex.requestedHeight = resolutionY;

                    _camTex.requestedFPS = fps;

                    _camTex.Play();
                }
                else
                {
                    Debug.LogWarningFormat("## JM Warning ## Cls:JMCamera Func:Open CamId:{0} Info:Has opened", _camId);
                }
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            if (!IsValid)
            {
                return;
            }

            if (_camTex != null)
            {
                if (_camTex.isPlaying)
                {
                    _camTex.Stop();
                }
                else
                {
                    Debug.LogWarningFormat("## JM Warning ## Cls:JMCamera Func:Close CamId:{0} Info:Has closed", _camId);
                }
            }
        }

        /// <summary>
        /// 重连
        /// </summary>
        public void ReConnect(int resolutionX = 1920, int resolutionY = 1080, int fps = 30)
        {
            if (!IsValid)
            {
                return;
            }

            if (_camTex != null)
            {
                if (_camTex.isPlaying)
                {
                    _camTex.Stop();
                }

                _camTex.requestedWidth = resolutionX;

                _camTex.requestedHeight = resolutionY;

                _camTex.requestedFPS = fps;

                _camTex.Play();
            }
        }

        /// <summary>
        /// 摄像头快照
        /// </summary>
        public void Snapshot(Action<Texture2D> callback)
        {
            Texture2D res = null;

            if (_camTex != null)
            {
                if (_camTex.isPlaying)
                {
                    Color[] colors = _camTex.GetPixels();

                    res = new Texture2D(_camTex.width, _camTex.height);

                    res.SetPixels(colors);

                    res.Apply();
                }
                else
                {
                    Debug.LogErrorFormat("## JM Error ## Cls:JMCamera Func:Snapshot CamId:{0} Info:Camera not open", _camId);
                }
            }

            if (callback != null)
            {
                callback.Invoke(res);
            }
        }

        /// <summary>
        /// 摄像头快照
        /// </summary>
        public void Snapshot(Action<Color[]> callback)
        {
            Color[] res = null;

            if (_camTex != null)
            {
                if (_camTex.isPlaying)
                {
                    res = _camTex.GetPixels();
                }
                else
                {
                    Debug.LogErrorFormat("## JM Error ## Cls:JMCamera Func:Snapshot CamId:{0} Info:Camera not open", _camId);
                }
            }

            if (callback != null)
            {
                callback.Invoke(res);
            }
        }

        /// <summary>
        /// 摄像头快照
        /// </summary>
        /// <param name="callback"></param>
        public void Snapshot(Action<Color32[]> callback)
        {
            Color32[] res = null;

            if (_camTex != null)
            {
                if (_camTex.isPlaying)
                {
                    res = _camTex.GetPixels32();
                }
                else
                {
                    Debug.LogErrorFormat("## JM Error ## Cls:JMCamera Func:Snapshot CamId:{0} Info:Camera not open", _camId);
                }
            }

            if (callback != null)
            {
                callback.Invoke(res);
            }
        }

        #endregion

    }
}

