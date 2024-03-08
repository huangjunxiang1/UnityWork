using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using UnityEngine;

namespace Game
{
    public enum SGameObjectType
    {
        Static = 0,//场景静态类型
        Resource,//资源类型
        LogicRoot,//根节点是空节点 下面挂资源
    }
    public class SGameObject : STree
    {
        public SGameObject(long rpc = 0, SGameObjectType style = SGameObjectType.LogicRoot) : base(rpc)
        {
            this.GameObjectType = style;
            if (GameObjectType == SGameObjectType.LogicRoot)
            {
                this.GameRoot = new GameObject();
#if UNITY_EDITOR
                if (rpc != 0)
                    this.Name = $"{this.GetType().Name}_rpc={rpc}";
                else
                    this.Name = $"{this.GetType().Name}";
#endif
                if (this is not SWorld)
                {
                    this.GameRoot.transform.SetParent(GameM.World.GameRoot.transform);
                    GameM.World.AddChild(this);
                }
            }
        }

        string _url;
        int _resVersion;

        /// <summary>
        /// 逻辑节点 WObjectLoadStyle.Resource模式 Root==Res
        /// </summary>
        public GameObject GameRoot { get; private set; }

        /// <summary>
        /// 资源模型
        /// </summary>
        public GameObject GameObject { get; private set; }

        public AttributeComponent Attribute { get; internal set; }

        public virtual string Name
        {
            get => GameRoot?.name;
            set
            {
                if (!GameRoot)
                    return;
                GameRoot.name = value;
            }
        }

        /// <summary>
        ///  
        /// </summary>
        public SGameObjectType GameObjectType { get; }

        /// <summary>
        /// 坐标
        /// </summary>
        public Vector3 Position
        {
            get
            {
                if (this.GameRoot)
                    return this.GameRoot.transform.position;
                return default;
            }
            set
            {
                if (GameObjectType == SGameObjectType.Static)
                {
                    Loger.Error("静态物体不能设置坐标");
                    return;
                }
                if (this.GameRoot)
                    this.GameRoot.transform.position = value;
            }
        }


        /// <summary>
        /// 设置加载的资源模型
        /// </summary>
        /// <param name="res"></param>
        public virtual void SetGameObject(GameObject res, bool release = true)
        {
            ++_resVersion;
            switch (GameObjectType)
            {
                case SGameObjectType.Static:
                    if (this.GameRoot)
                    {
                        Loger.Error("已set 游戏对象");
                        return;
                    }
                    this.GameRoot = res;
                    this.GameObject = res;
                    break;
                case SGameObjectType.Resource:
                    if (res == this.GameObject)
                        return;

                    if (this.GameObject)
                    {
                        if (res)
                        {
                            res.transform.parent = this.GameObject.transform.parent;
                            res.transform.SetPositionAndRotation(this.GameObject.transform.position, this.GameObject.transform.rotation);
                        }
                        if (release)
                            SAsset.Release(this.GameObject);
                        else
                            this.GameObject.transform.parent = GameM.World.GameRoot.transform;
                    }
                    else
                    {
                        if (res)
                        {
                            res.transform.SetParent(GameM.World.GameRoot.transform);
                            res.transform.SetPositionAndRotation(default, Quaternion.identity);
                        }
                    }

                    this.GameRoot = res;
                    this.GameObject = res;
                    break;
                case SGameObjectType.LogicRoot:
                    if (res == this.GameObject)
                        return;

                    if (res)
                    {
                        res.transform.parent = this.GameRoot.transform;
                        res.transform.SetLocalPositionAndRotation(default, Quaternion.identity);
                    }

                    if (this.GameObject)
                    {
                        if (release)
                            SAsset.Release(this.GameObject);
                        else
                            this.GameObject.transform.parent = GameM.World.GameRoot.transform;
                    }
                    this.GameObject = res;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 主动加载资源
        /// </summary>
        /// <param name="url"></param>
        public void LoadGameObject(string url, ReleaseMode releaseMode = ReleaseMode.PutToPool)
        {
            if (this.GameObjectType == SGameObjectType.Static)
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
            if (this.GameObjectType == SGameObjectType.Static)
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

        /// <summary>
        /// 释放对象 取消所有已注册的事件
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            switch (this.GameObjectType)
            {
                case SGameObjectType.Static:
                    break;
                case SGameObjectType.Resource:
                    if (this.GameObject)
                        SAsset.Release(this.GameObject);
                    break;
                case SGameObjectType.LogicRoot:
                    if (this.GameObject)
                        SAsset.Release(this.GameObject);
                    GameObject.DestroyImmediate(this.GameRoot);
                    break;
                default:
                    break;
            }
        }
    }
}
