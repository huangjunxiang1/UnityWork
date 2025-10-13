using Core;
using Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public enum SGameObjectType
    {
        Static = 0,//场景静态类型
        Resource,//资源类型
        LogicRoot,//根节点是空节点 下面挂资源
    }
    public class GameObjectComponent : ViewComponent
    {
        public GameObjectComponent() : this(SGameObjectType.LogicRoot) { }
        public GameObjectComponent(SGameObjectType style = SGameObjectType.LogicRoot)
        {
            this.gameObjectType = style;
            if (style == SGameObjectType.LogicRoot)
            {
                this.gameRoot = new GameObject();
                this.gameRoot.transform.SetParent(Client.transform);
            }
        }

        string _url;
        int _resVersion;
        bool _isSelfSet = false;

        public sealed override SObject Entity
        {
            get => base.Entity;
            internal set
            {
                base.Entity = value;
#if UNITY_EDITOR
                if (this.gameRoot && this.gameObjectType != SGameObjectType.Static)
                {
                    string s = this.Entity.GetType().Name;
                    if (this.ActorId != 0)
                        s += $"_actorId={this.ActorId}";
                    if (this.Entity.tid != 0)
                        s += $"_tid={this.Entity.tid}";
                    this.gameRoot.name = s;
                }
#endif
            }
        }

        /// <summary>
        ///  
        /// </summary>
        public SGameObjectType gameObjectType { get; }

        /// <summary>
        /// 逻辑节点 SGameObjectType.Resource模式下 gameRoot==gameObject
        /// </summary>
        public GameObject gameRoot { get; private set; }

        /// <summary>
        /// 资源模型
        /// </summary>
        public GameObject gameObject { get; private set; }

        protected override void View(bool view)
        {
            if (this.gameObjectType == SGameObjectType.Static) return;
            this.gameRoot?.SetActive(view);
        }

        /// <summary>
        /// 设置加载的资源模型
        /// </summary>
        /// <param name="res"></param>
        public virtual void SetGameObject(GameObject res, bool release = true)
        {
            if (!_isSelfSet)
            {
                ++_resVersion;
                _url = null;
            }
            else
                _isSelfSet = false;
            var old = this.gameObject;
            switch (gameObjectType)
            {
                case SGameObjectType.Static:
                    if (this.gameRoot)
                    {
                        Loger.Error("已set 游戏对象");
                        return;
                    }
                    this.gameRoot = res;
                    this.gameObject = res;
                    break;
                case SGameObjectType.Resource:
                    if (res == this.gameObject)
                        return;

                    if (this.gameObject)
                    {
                        if (res)
                        {
                            res.transform.parent = this.gameObject.transform.parent;
                            res.transform.SetPositionAndRotation(this.gameObject.transform.position, this.gameObject.transform.rotation);
                        }
                        if (release)
                            SAsset.Release(this.gameObject);
                        else
                            this.gameObject.transform.parent = Client.transform;
                    }
                    else
                    {
                        if (res)
                        {
                            res.transform.SetParent(Client.transform);
                            res.transform.SetPositionAndRotation(default, Quaternion.identity);
                        }
                    }

                    this.gameRoot = res;
                    this.gameObject = res;
                    break;
                case SGameObjectType.LogicRoot:
                    if (res == this.gameObject)
                        return;

                    if (res)
                    {
                        res.transform.parent = this.gameRoot.transform;
                        res.transform.SetLocalPositionAndRotation(default, Quaternion.identity);
                    }

                    if (this.gameObject)
                    {
                        if (release)
                            SAsset.Release(this.gameObject);
                        else
                            this.gameObject.transform.parent = Client.transform;
                    }
                    this.gameObject = res;
                    break;
                default:
                    break;
            }
            this.gameRoot?.gameObject.SetActive(this.Enable);
            this.SetChange();
#if UNITY_EDITOR
            if (this.gameRoot && this.gameObjectType != SGameObjectType.Resource)
            {
                string s = this.Entity.GetType().Name;
                if (this.ActorId != 0)
                    s += $"_actorId={this.ActorId}";
                if (this.Entity.tid != 0)
                    s += $"_tid={this.Entity.tid}";
                this.gameRoot.name = s;
            }
#endif
            if (this.gameObjectType == SGameObjectType.LogicRoot)
            {
                if (this.gameObject)
                {
                    var info = this.gameObject.GetComponent<SObjectInfo>() ?? this.gameObject.AddComponent<SObjectInfo>();
                    info.gid = this.gid;
                }
            }
            else
            {
                if (this.gameRoot)
                {
                    var info = this.gameRoot.GetComponent<SObjectInfo>() ?? this.gameRoot.AddComponent<SObjectInfo>();
                    info.gid = this.gid;
                }
            }
        }
        public void SetGameObject(string url, ReleaseMode releaseMode = ReleaseMode.PutToPool) => _ = LoadGameObjectAsync(url, releaseMode);

        /// <summary>
        /// 主动加载资源
        /// </summary>
        /// <param name="url"></param>
        public void LoadGameObject(string url, ReleaseMode releaseMode = ReleaseMode.PutToPool)
        {
            if (this.gameObjectType == SGameObjectType.Static)
            {
                Loger.Error("静态类型不能动态加载");
                return;
            }
            if (string.IsNullOrEmpty(url))
            {
                _url = url;
                ++_resVersion;
                SetGameObject(res: null);
                return;
            }
            if (_url == url)
                return;
            _url = url;
            ++_resVersion;
            _isSelfSet = true;
            SetGameObject(SAsset.LoadGameObject(_url, releaseMode));
        }
        public async STask LoadGameObjectAsync(string url, ReleaseMode releaseMode = ReleaseMode.PutToPool)
        {
            if (this.gameObjectType == SGameObjectType.Static)
            {
                Loger.Error("静态类型不能动态加载");
                return;
            }
            if (string.IsNullOrEmpty(url))
            {
                _url = url;
                ++_resVersion;
                SetGameObject(res: null);
                return;
            }
            if (_url == url)
                return;
            _url = url;
            int ver = ++_resVersion;
            GameObject res = await SAsset.LoadGameObjectAsync(_url, releaseMode);
            if (ver != _resVersion)
            {
                SAsset.Release(res);
                return;
            }
            _isSelfSet = true;
            SetGameObject(res);
        }

        [InSystem]
        static void In(GameObjectComponent t)
        {
            t.gameRoot?.SetActive(t.Enable);
        }
        [OutSystem]
        static void Out(GameObjectComponent t)
        {
            ++t._resVersion;
            if (t.Disposed)
            {
                switch (t.gameObjectType)
                {
                    case SGameObjectType.Static:
                        break;
                    case SGameObjectType.Resource:
                        if (t.gameObject)
                            SAsset.Release(t.gameObject);
                        break;
                    case SGameObjectType.LogicRoot:
                        if (t.gameObject)
                        {
                            t.gameRoot.transform.localScale = Vector3.one;
                            SAsset.Release(t.gameObject);
                        }
                        GameObject.Destroy(t.gameRoot);
                        break;
                    default:
                        break;
                }
                t.gameObject = null;
                t.gameRoot = null;
            }
            else
                t.gameRoot?.SetActive(t.Enable);
        }

        [AnyChangeSystem]
        static void AnyChange(GameObjectComponent a, TransformComponent b)
        {
            if (a.gameObjectType == SGameObjectType.Static) return;
            if (a.gameRoot)
            {
                a.gameRoot.transform.position = b.position;
                a.gameRoot.transform.localScale = b.scale;
                a.gameRoot.transform.rotation = b.rotation;
            }
        }
    }
}
