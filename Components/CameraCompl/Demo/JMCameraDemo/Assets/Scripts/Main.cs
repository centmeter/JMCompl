using JM.Camera;
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
                _camera.Open(800,600);

                Debug.Log(_camera.CamInfo);
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
                _camera.ReConnect(1280, 720);
            }
        }

        if (GUILayout.Button("拍照"))
        {
            if (_camera != null)
            {
                _camera.Snapshot((tex) =>
                {
                    _imgSnapshot.texture = tex;
                    
                });
            }
        }

        GUILayout.EndVertical();
    }
}
