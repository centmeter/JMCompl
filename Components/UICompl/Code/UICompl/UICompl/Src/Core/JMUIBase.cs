using System;
using UnityEngine;

namespace JM.UICompl
{
    /// <summary>
    /// UI基类
    /// </summary>
    public abstract class JMUIBase : MonoBehaviour
    {
        #region Public Func

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize()
        {
            InitNode(this.transform);
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void Show()
        {
            this.gameObject.SetActive(true);
        }
        

        #endregion

        #region Private Func

        /// <summary>
        /// 遍历节点注册
        /// </summary>
        private void InitNode(Transform node)
        {
            if (node != null)
            {
                RegisterNode(node.name, node);

                for (int i = 0; i < node.childCount; i++)
                {
                    InitNode(node.GetChild(i));
                }
            }
            else
            {
                throw new Exception("## Uni Exception ## Author:<Ming> Cls:JMUIBase Func:InitNode Exception:Node is null");
            }

        }

        #endregion

        #region Protected Func

        /// <summary>
        /// 注册节点
        /// </summary>
        protected abstract void RegisterNode(string name, Transform node);

        #endregion
    }
}
