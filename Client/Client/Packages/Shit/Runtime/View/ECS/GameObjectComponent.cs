﻿using Core;
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
    public class GameObjectComponent : SComponent
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

        [Sirenix.OdinInspector.ShowInInspector]
        /// <summary>
        ///  
        /// </summary>
        public SGameObjectType gameObjectType { get; }

        [Sirenix.OdinInspector.ShowInInspector]
        /// <summary>
        /// 逻辑节点 WObjectLoadStyle.Resource模式 Root==Res
        /// </summary>
        public GameObject gameRoot { get; private set; }

        [Sirenix.OdinInspector.ShowInInspector]
        /// <summary>
        /// 资源模型
        /// </summary>
        public GameObject gameObject { get; private set; }

        internal Action<GameObject, GameObject> Replace;

        /// <summary>
        /// 设置加载的资源模型
        /// </summary>
        /// <param name="res"></param>
        public virtual void SetGameObject(GameObject res, bool release = true)
        {
            ++_resVersion;
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
            Replace?.Invoke(old, this.gameObject);
            this.SetChange();
#if UNITY_EDITOR
            if (this.gameRoot && this.gameObjectType != SGameObjectType.Static)
            {
                string s = this.Entity.GetType().Name;
                if (this.rpc != 0)
                    s += $"_rpc={this.rpc}";
                if (this.Entity.tid != 0)
                    s += $"_tid={this.Entity.tid}";
                this.gameRoot.name = s;
            }
#endif
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
            if (_url == url)
                return;
            _url = url;
            ++_resVersion;
            SetGameObject(SAsset.LoadGameObject(_url, releaseMode));
        }
        public async STask LoadGameObjectAsync(string url, ReleaseMode releaseMode = ReleaseMode.PutToPool)
        {
            if (this.gameObjectType == SGameObjectType.Static)
            {
                Loger.Error("静态类型不能动态加载");
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
            SetGameObject(res);
        }

        [Event]
        static void Dispose(Dispose<GameObjectComponent> t)
        {
            ++t.t._resVersion;
            switch (t.t.gameObjectType)
            {
                case SGameObjectType.Static:
                    break;
                case SGameObjectType.Resource:
                    if (t.t.gameObject)
                        SAsset.Release(t.t.gameObject);
                    break;
                case SGameObjectType.LogicRoot:
                    if (t.t.gameObject)
                        SAsset.Release(t.t.gameObject);
                    GameObject.DestroyImmediate(t.t.gameRoot);
                    break;
                default:
                    break;
            }
            t.t.gameObject = null;
            t.t.gameRoot = null;
        }

        [Event]
        static void AnyChange(AnyChange<GameObjectComponent, TransformComponent> t)
        {
            if (t.t.gameObjectType == SGameObjectType.Static) return;
            if (t.t.gameRoot)
            {
                t.t.gameRoot.transform.position = t.t2.position;
                t.t.gameRoot.transform.localScale = t.t2.scale;
                t.t.gameRoot.transform.rotation = t.t2.rotation;
                t.t.gameRoot.transform.forward = t.t2.forward;
            }
        }
        [Event]
        static void In(In<GameObjectComponent> t)
        {
            if (t.t.gameObjectType == SGameObjectType.Static) return;
            t.t.gameRoot?.SetActive(true);
        }
        [Event]
        static void Out(Out<GameObjectComponent> t)
        {
            if (t.t.gameObjectType == SGameObjectType.Static) return;
            t.t.gameRoot?.SetActive(false);
        }
    }
}