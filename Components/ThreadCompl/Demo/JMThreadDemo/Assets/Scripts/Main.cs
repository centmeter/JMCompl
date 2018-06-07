using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JM.ThreadCompl;
using System.Threading;

public class Main : MonoBehaviour
{
    private void Start()
    {
        JMThreadManager.Instance.Initialize(3);
        JMThreadManager.Instance.OnExceptionEvent += Instance_OnExceptionEvent;
    }

    private void Instance_OnExceptionEvent(string errorMsg)
    {
        Debug.LogError(errorMsg);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            JMThreadManager.Instance.RunThread(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    Debug.Log(i);
                    Thread.Sleep(2000);
                }
            });
        }

    }
    private void OnApplicationQuit()
    {
        JMThreadManager.Instance.DoRelease();
    }
}
