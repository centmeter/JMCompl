using System;

namespace JM.ThreadCompl
{
    /// <summary>
    /// 线程管理
    /// </summary>
    public class JMThreadManager
    {
        #region Singleton

        private static JMThreadManager _instance;
        private static object _locker = new object();
        /// <summary>
        /// 实例
        /// </summary>
        public static JMThreadManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new JMThreadManager();
                        }
                    }
                }
                return _instance;
            }
        }
        private JMThreadManager()
        {

        }

        #endregion

        #region Private Variable

        /// <summary>
        /// 初始化完成标识
        /// </summary>
        private bool _initDone = false;

        /// <summary>
        /// 线程池
        /// </summary>
        private JMThreadPool _threadPool;

        #endregion

        #region Event

        /// <summary>
        /// 线程模块异常事件 arg:异常描述
        /// </summary>
        public event Action<string> OnExceptionEvent;

        /// <summary>
        /// 线程模块初始化成功事件
        /// </summary>
        public event Action OnInitSuccessEvent;

        #endregion

        #region Public Func

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize(int maxCount = 25)
        {
            if (!_initDone)
            {
                _threadPool = new JMThreadPool(maxCount);
                _initDone = true;
                if (OnInitSuccessEvent != null)
                {
                    OnInitSuccessEvent.Invoke();
                }
            }
            else
            {
                if (OnExceptionEvent != null)
                {
                    string arg = "## JM Error ## cls:JMThreadManager func:Initialize info:Has initialized";
                    OnExceptionEvent.Invoke(arg);
                }
            }
        }

        /// <summary>
        /// 运行线程
        /// </summary>
        /// <param name="threadAct">线程方法</param>
        /// <returns>线程唯一标识Id</returns>
        public string RunThread(Action threadAct)
        {
            string id = string.Empty;
            if (_initDone)
            {
                id = _threadPool.CreateThread(threadAct, (error) =>
                {
                    if (OnExceptionEvent != null)
                    {
                        OnExceptionEvent.Invoke(error);
                    }
                });
            }
            return id;
        }

        /// <summary>
        /// 强制结束线程
        /// </summary>
        public void AbortThread(string threadId)
        {
            if (_initDone)
            {
                _threadPool.AbortThread(threadId, (error) =>
                 {
                     if (OnExceptionEvent != null)
                     {
                         OnExceptionEvent.Invoke(error);
                     }
                 });
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        public void DoRelease()
        {
            if (_initDone)
            {
                OnExceptionEvent = null;
                OnInitSuccessEvent = null;
                _threadPool.Destroy();
            }
        }
        #endregion
    }

}
