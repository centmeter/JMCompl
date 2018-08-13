using System;
using UnityEngine;
using UnityEditor;
namespace JMEditor.UISearcher
{
    public class JMUISeacher
    {
        #region Public Func

        /// <summary>
        /// 查询UI节点路径(复制到剪切板)
        /// </summary>
        [MenuItem("GameObject/JMEditor/UISearcher", false, 11)]
        public static void Search()
        {
            Transform selectedTrans = Selection.activeTransform;
            string path = string.Empty;
            GetNodePath(selectedTrans, ref path);
            Debug.LogFormat("## Uni Log ## Author:<Ming> Cls:JMUISeacher Func:Search Node:{0} Path:{1}", selectedTrans.name, path);


        }

        #endregion

        #region Private Func

        /// <summary>
        /// 获取节点路径
        /// </summary>
        private static void GetNodePath(Transform nodeTrans, ref string path)
        {
            if (nodeTrans != null)
            {
                string curNodeName = nodeTrans.name;
                path = string.Format("{0}/{1}", curNodeName, path);

                Transform parentNode = nodeTrans.parent;
                if (parentNode != null)
                {
                    GetNodePath(parentNode, ref path);
                }
            }
        }

        /// <summary>
        /// 复制字符串进剪切板
        /// </summary>
        private void CopyContent(string content)
        {
            TextEditor textEditor = new TextEditor();
            textEditor.text = content;
            textEditor.OnFocus();
            textEditor.Copy();
        }


        #endregion

    }
}
