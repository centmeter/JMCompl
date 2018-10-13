
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
        /// 有效性
        /// </summary>
        public bool IsValid
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
                    Debug.LogErrorFormat("## JM Error ## Cls:JMCamera Func:IsValid CamId:{0} Info:Invalid", _camId);
                }

                return res;
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

            _camTex = new WebCamTexture();
        }

        /// <summary>
        /// 打开
        /// </summary>
        public void Open(int resolutionX = 1920, int resolutionY = 1080, int fps = 30)
        {
            if (!IsValid)
            {
                return;
            }

            try
            {
                if (_camTex != null)
                {
                    if (!_camTex.isPlaying)
                    {
                        _camTex.requestedWidth = resolutionX;

                        _camTex.requestedHeight = resolutionY;

                        _camTex.requestedFPS = fps;

                        _camTex.deviceName = WebCamTexture.devices[_camId].name;

                        _camTex.Play();
                    }
                    else
                    {
                        Debug.LogWarningFormat("## JM Warning ## Cls:JMCamera Func:Open CamId:{0} Info:Has opened", _camId);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("## JM Error ## Cls:JMCamera Func:Open CamId:{0} Info:{1}", _camId, e);
            }

        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            try
            {
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
            catch (Exception e)
            {
                Debug.LogErrorFormat("## JM Error ## Cls:JMCamera Func:Close CamId:{0} Info:{1}", _camId, e);
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

            try
            {
                if (_camTex != null)
                {
                    if (_camTex.isPlaying)
                    {
                        _camTex.Stop();
                    }

                    _camTex.requestedWidth = resolutionX;

                    _camTex.requestedHeight = resolutionY;

                    _camTex.requestedFPS = fps;

                    _camTex.deviceName = WebCamTexture.devices[_camId].name;

                    _camTex.Play();
                }
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("## JM Error ## Cls:JMCamera Func:ReConnect CamId:{0} Info:{1}", _camId, e);
            }

        }

        /// <summary>
        /// 摄像头快照
        /// </summary>
        public void Snapshot(Action<Texture2D> callback)
        {
            Texture2D res = null;

            try
            {
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
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("## JM Error ## Cls:JMCamera Func:Snapshot CamId:{0} Info:{1}", _camId, e);
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

            try
            {
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
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("## JM Error ## Cls:JMCamera Func:Snapshot CamId:{0} Info:{1}", _camId, e);
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

            try
            {
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
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("## JM Error ## Cls:JMCamera Func:Snapshot CamId:{0} Info:{1}", _camId, e);
            }

            if (callback != null)
            {
                callback.Invoke(res);
            }
        }

        #endregion

    }
}

