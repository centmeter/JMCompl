using JM.Camera;
using JM.Image;
using System;
using System.Drawing;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public RawImage _img;

    public RawImage _imgSnapshot;

    private string _camId;

    private bool _initDone;

    private JMCamera _camera;

    private void Awake()
    {
        Loom.Initialize();

        JMCameraManager.Instance.Initialize(() =>
        {
            _initDone = true;
        });


    }

    private void OnGUI()
    {
        if (!_initDone)
        {
            return;
        }

        GUILayout.BeginVertical();

        _camId = GUILayout.TextField(_camId);

        if (GUILayout.Button("打开摄像头"))
        {
            int camId = 0;

            int.TryParse(_camId, out camId);

            _camera = JMCameraManager.Instance.GetCamera(camId);

            _img.texture = _camera.CameraTexture;

            if (_camera != null)
            {
                _camera.Open((int)_img.rectTransform.rect.width, (int)_img.rectTransform.rect.height);

                Debug.Log(_camera);
            }
        }

        if (GUILayout.Button("关闭摄像头"))
        {
            if (_camera != null)
            {
                _camera.Close();
            }
        }

        if (GUILayout.Button("重连摄像头"))
        {
            if (_camera != null)
            {
                _camera.ReConnect((int)_img.rectTransform.rect.width, (int)_img.rectTransform.rect.height);
            }
        }

        if (GUILayout.Button("拍照"))
        {
            if (_camera != null)
            {
                _camera.Snapshot((Color32[] colors) =>
                {
                    System.Drawing.Color[,] resColors = GetColors(colors, (int)_img.rectTransform.rect.width, (int)_img.rectTransform.rect.height);

                    System.Threading.ThreadPool.QueueUserWorkItem((state) =>
                    {
                        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

                        sw.Start();

                        JMColorsHandler handler = new JMColorsHandler(resColors);

                        handler.Effect(JMImageEffect.Relief, System.Drawing.Imaging.ImageFormat.Png, (datas) =>
                         {
                             Loom.QueueOnMainThread(() =>
                             {
                                 SetTexture(datas);
                             });

                         });

                        sw.Stop();

                        Debug.Log(sw.ElapsedMilliseconds);
                    });
                });
            }
        }

        GUILayout.EndVertical();
    }

    private void SetTexture(byte[] datas)
    {
        if (datas != null)
        {
            Loom.QueueOnMainThread(() =>
            {
                try
                {
                    Texture2D texture = new Texture2D((int)_img.rectTransform.rect.width, (int)_img.rectTransform.rect.height);

                    texture.LoadImage(datas);

                    texture.Apply();

                    _imgSnapshot.texture = texture;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            });
        }
    }

    private System.Drawing.Color[,] GetColors(Color32[] colors, int width, int height)
    {
        System.Drawing.Color[,] res = null;

        if (colors != null && colors.Length == width * height)
        {
            res = new System.Drawing.Color[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color32 color32 = colors[i + width * j];

                    res[i, height - j - 1] = System.Drawing.Color.FromArgb(color32.a, color32.r, color32.g, color32.b);
                }
            }

        }
        else
        {
            Debug.LogError("Colors input wrong");
        }

        return res;
    }
}
