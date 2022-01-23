using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    class EntityL
    {
        public EntityL() : this(++idValue)
        {
           
        }
        public EntityL(long id)
        {
            this.ID = id;
            SysEvent.RigisterListener(this);
        }

        static long idValue;

        List<TaskAwaiter> _taskLst;

        /// <summary>
        /// ID  可自定义赋值
        /// </summary>
        public long ID { get; }

        /// <summary>
        /// 是否已被销毁
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Dispose()
        {
            if (this.Disposed) return;

            this.Disposed = true;
            SysEvent.RemoveListener(this);
            if (_taskLst != null)
            {
                int len = _taskLst.Count;
                for (int i = 0; i < len; i++)
                    _taskLst[i].TryCancel();
                _taskLst.Clear();
            }
        }

        //创建一个异步并管理起来
        public TaskAwaiter<T> AddTask<T>(TaskAwaiter<T> task)
        {
            if (_taskLst == null) _taskLst = new List<TaskAwaiter>();
            _taskLst.Add(task);
            task.AddWaitCall(() => RemoveTask(task));
            return task;
        }
        public TaskAwaiter AddTask(TaskAwaiter task)
        {
            if (_taskLst == null) _taskLst = new List<TaskAwaiter>();
            _taskLst.Add(task);
            task.AddWaitCall(() => RemoveTask(task));
            return task;
        }
        public void RemoveTask(TaskAwaiter task)
        {
            if (_taskLst == null) return;
            _taskLst.Remove(task);
        }
        public void RemoveTask<T>(TaskAwaiter<T> task)
        {
            if (_taskLst == null) return;
            _taskLst.Remove(task);
        }


        /// <summary>
        /// 每个类里面调用基类异步 方便做释放的管理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected TaskAwaiter<IMessage> SendAsync(IRequest request)
        {
            return SendAsync(0, request);
        }
        protected TaskAwaiter<IMessage> SendAsync(long actorId, IRequest request)
        {
            return AddTask(SysNet.SendAsync(actorId, request));
        }

        /// <summary>
        /// 资源加载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        protected TaskAwaiter<GameObject> LoadPrefabAsyncRef(string path, ref TaskAwaiter<GameObject> task)
        {
            var taskRef = task;
            AssetLoad.PrefabLoader.LoadAsyncRef(path, ref task);
            if (taskRef != task)
            {
                if (taskRef != null)
                    this.RemoveTask(taskRef);
                this.AddTask(task);
            }
            return task;
        }

        protected TaskAwaiter<Texture> LoadTexAsyncRef(string path, ref TaskAwaiter<Texture> task)
        {
            var taskRef = task;
            AssetLoad.TextureLoader.LoadAsyncRef(path, ref task);
            if (taskRef != task)
            {
                if (taskRef != null)
                    this.RemoveTask(taskRef);
                this.AddTask(task);
            }
            return task;
        }
        protected TaskAwaiter<TextAsset> LoadTextAssetAsyncRef(string path, ref TaskAwaiter<TextAsset> task)
        {
            var taskRef = task;
            AssetLoad.TextAssetLoader.LoadAsyncRef(path, ref task);
            if (taskRef != task)
            {
                if (taskRef != null)
                    this.RemoveTask(taskRef);
                this.AddTask(task);
            }
            return task;
        }
        protected TaskAwaiter<AudioClip> LoadAudioAsyncRef(string path, ref TaskAwaiter<AudioClip> task)
        {
            var taskRef = task;
            AssetLoad.AudioLoader.LoadAsyncRef(path, ref task);
            if (taskRef != task)
            {
                if (taskRef != null)
                    this.RemoveTask(taskRef);
                this.AddTask(task);
            }
            return task;
        }
        protected TaskAwaiter<ScriptableObject> LoadScriptableObjectAsyncRef(string path, ref TaskAwaiter<ScriptableObject> task)
        {
            var taskRef = task;
            AssetLoad.ScriptObjectLoader.LoadAsyncRef(path, ref task);
            if (taskRef != task)
            {
                if (taskRef != null)
                    this.RemoveTask(taskRef);
                this.AddTask(task);
            }
            return task;
        }
    }
}
