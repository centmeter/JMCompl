using System;
using System.Collections.Generic;
using System.Threading;

namespace JM.ThreadCompl
{
    internal class JMThreadPool
    {
        #region Private Variable

        /// <summary>
        /// 最大运行线程数
        /// </summary>
        private int _maxCount;

        /// <summary>
        /// 运行线程列表
        /// </summary>
        private List<Thread> _runList = new List<Thread>();

        /// <summary>
        /// 等待线程列表
        /// </summary>
        private List<Thread> _waitList = new List<Thread>();

        /// <summary>
        /// 运行线程列表锁
        /// </summary>
        private object _runListLocker = new object();

        /// <summary>
        /// 等待线程列表锁
        /// </summary>
        private object _waitListLocker = new object();

        #endregion

        #region Event

        #endregion

        #region Internal Func

        internal JMThreadPool()
        {
            _maxCount = 25;
        }

        internal JMThreadPool(int maxCount)
        {
            _maxCount = maxCount;
        }


        /// <summary>
        /// 创建线程
        /// </summary>
        internal string CreateThread(Action threadAct, Action onThreadCompletedCallback = null, Action<string> onThreadExceptionCallback = null)
        {
            string threadId = string.Format("JMThread_{0}", Guid.NewGuid());
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    if (threadAct != null)
                    {
                        threadAct.Invoke();
                    }
                    if (onThreadCompletedCallback != null)
                    {
                        onThreadCompletedCallback.Invoke();
                    }
                    OnRunThreadCompleted();
                }
                catch (Exception e)
                {
                    if (onThreadExceptionCallback != null)
                    {
                        string arg = string.Format("## JM Error ## cls:JMThreadPool func:CreateThread info:{0}", e.ToString());
                        onThreadExceptionCallback.Invoke(arg);
                    }
                }
            }));
            thread.IsBackground = true;
            thread.Name = threadId;

            if (_runList.Count < _maxCount)
            {
                lock (_runListLocker)
                {
                    _runList.Add(thread);
                }
                thread.Start();
            }
            else
            {
                lock (_waitListLocker)
                {
                    _waitList.Add(thread);
                }
            }
            return threadId;

        }

        /// <summary>
        /// 获取线程状态
        /// </summary>
        internal ThreadStatus GetThreadStatus(string threadId)
        {
            ThreadStatus status = ThreadStatus.DoneOrNotExists;

            Thread thread = GetRunThreadById(threadId);
            if (thread != null)
            {
                status = ThreadStatus.Run;
            }
            else
            {
                thread = GetWaitThreadById(threadId);
                if (thread != null)
                {
                    status = ThreadStatus.Wait;
                }
            }
            return status;

        }

        /// <summary>
        /// 销毁线程
        /// </summary>
        internal void DestroyThread(string threadId, Action<string> onDestroyThreadFailCallback = null)
        {
            Thread thread = GetRunThreadById(threadId);
            if (thread != null)
            {
                lock (_runListLocker)
                {
                    _runList.Remove(thread);
                }
                thread.Abort();
                thread = null;
                RunWaitThread();
            }
            else
            {
                thread = GetWaitThreadById(threadId);
                if (thread != null)
                {
                    lock (_waitListLocker)
                    {
                        _waitList.Remove(thread);
                        thread.Abort();
                        thread = null;
                    }
                }
                else
                {
                    if (onDestroyThreadFailCallback != null)
                    {
                        string arg = string.Format("## JM Error ## cls:JMThreadPool func:DestroyThread info:Thread not exists");
                        onDestroyThreadFailCallback.Invoke(arg);
                    }
                }
            }
        }

        /// <summary>
        /// 销毁
        /// </summary>
        internal void Destroy()
        {
            for (int i = 0; i < _runList.Count; i++)
            {
                Thread runThread = _runList[i];
                if (runThread != null)
                {
                    runThread.Abort();
                    runThread = null;
                }
            }
            for (int i = 0; i < _waitList.Count; i++)
            {
                Thread waitThread = _waitList[i];
                if (waitThread != null)
                {
                    waitThread.Abort();
                    waitThread = null;
                }
            }
            _runList.Clear();
            _waitList.Clear();
            _runList = null;
            _waitList = null;

        }

        #endregion

        #region Private Func

        /// <summary>
        /// 线程运行结束处理
        /// </summary>
        private void OnRunThreadCompleted()
        {
            Thread thread = Thread.CurrentThread;
            if (_runList.Contains(thread))
            {
                lock (_runListLocker)
                {
                    _runList.Remove(thread);
                }
            }
            RunWaitThread();
        }

        /// <summary>
        /// 运行等待线程
        /// </summary>
        private void RunWaitThread()
        {
            if (_waitList.Count > 0)
            {
                _waitList[0].Start();
                lock (_runListLocker)
                {
                    _runList.Add(_waitList[0]);
                }
                lock (_waitListLocker)
                {
                    _waitList.RemoveAt(0);
                }

            }
        }

        /// <summary>
        /// 通过线程唯一标识Id获取运行线程实例
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        private Thread GetRunThreadById(string threadId)
        {
            Thread thread = null;
            for (int i = 0; i < _runList.Count; i++)
            {
                if (_runList[i].Name.Equals(threadId))
                {
                    thread = _runList[i];
                    break;
                }
            }
            return thread;
        }

        /// <summary>
        /// 通过线程唯一标识Id获取等待线程实例
        /// </summary>
        private Thread GetWaitThreadById(string threadId)
        {
            Thread thread = null;
            for (int i = 0; i < _waitList.Count; i++)
            {
                if (_waitList[i].Name.Equals(threadId))
                {
                    thread = _waitList[i];
                    break;
                }
            }
            return thread;
        }

        /// <summary>
        /// 通过线程唯一标识Id获取线程实例
        /// </summary>
        private Thread GetThreadById(string threadId)
        {
            Thread thread = GetRunThreadById(threadId);
            if (thread == null)
            {
                thread = GetWaitThreadById(threadId);
            }
            return thread;
        }
        #endregion
    }
}
