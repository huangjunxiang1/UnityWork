using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    public abstract class Playing
    {
        bool _enable = true;
        internal float _speed = 1;
        internal int _layer = 0;
        internal string name;
        public PlayingComponent playing { get; internal set; }
        public bool enable
        {
            get => _enable;
            set
            {
                if (_enable == value)
                    return;
                _enable = value;
                this.SetEnable();
            }
        }
        public abstract float length { get; }
        public abstract float time { get; set; }
        public abstract float time01 { get; set; }
        public virtual float Speed
        {
            get => _speed;
            set
            {
                if (_speed == value)
                    return;
                _speed = value;
                this.SetSpeed();
            }
        }
        public abstract void SetSpeed();
        public virtual void SetEnable() { }
        public virtual void Play(string name, float fade) => this.name = name;
        public virtual void SetAvatarMask(AvatarMask mask, int layer) { }
        public abstract float GetTime(string name);
        public abstract Playing Clone(int layer);
    }

    public class PlayingComponent : SComponent
    {
        Playing[] _plays = new Playing[1];

        public int Layer
        {
            get => _plays.Length;
            set
            {
                value = math.max(value, 1);

                int len = _plays.Length;
                Array.Resize(ref _plays, value);
                for (int i = value; i < len; i++)
                    _plays[i].enable = false;
                for (int i = len; i < value; i++)
                {
                    if (_plays[i] == null && _plays[0] != null)
                        _plays[i] = _plays[0].Clone(i);
                }

                this.SetChangeFlag();
            }
        }
        public float speed
        {
            get => _plays[0].Speed;
            set => _plays[0].Speed = value;
        }
        public float length => _plays[0].length;
        public float time
        {
            get
            {
                if (_plays[0] == null) return 0;
                return _plays[0].time;
            }
            set
            {
                if (_plays[0] == null) return;
                _plays[0].time = value;
            }
        }
        public float time01
        {
            get
            {
                if (_plays[0] == null) return 0;
                return _plays[0].time01;
            }
            set
            {
                if (_plays[0] == null) return;
                _plays[0].time01 = value;
            }
        }

        public string name => play[0].name;
        public Playing[] play { get; private set; }

        public void Set(Playing play, int layer = 0)
        {
            this.play[layer] = play;
            if (play == null) return;
            play.playing = this;
            if (!string.IsNullOrEmpty(name))
                play.Play(name, 0.2f);
            play.SetSpeed();
            //this.SetChange(); 这里不能调用 因为会陷入AnyChange的递归
        }
        public void EnableLayer(int layer, bool enable) => _plays[layer].enable = enable;
        public void Play(string name, float fade = 0.2f, int layer = 0)
        {
            if (play[layer].name == name) return;
            if (name == null || string.IsNullOrEmpty(name)) return;
            play[layer].Play(name, fade);
            play[layer].SetSpeed();
            this.SetChangeFlag();
        }

        public void SetSpeed(float speed, int layer = 0) => _plays[layer].Speed = speed;
        public float GetSpeed(int layer = 0) => _plays[layer].Speed;

        public float GetLength(int layer = 0) => _plays[layer].length;

        public void SetTime(float time, int layer = 0) => _plays[layer].time = time;
        public float GetTime(int layer) => _plays[layer].time;

        public void SetTime01(float time, int layer = 0) => _plays[layer].time01 = time;
        public float GetTime01(int layer) => _plays[layer].time01;

        public float GetTime(string name, int layer = 0)
        {
            if (play[layer] == null) return 1;
            return play[layer].GetTime(name);
        }

        [Event]
        static void AnyChange(AnyChange<GameObjectComponent, PlayingComponent> t)
        {
            if (t.t.gameObject)
            {
#if Animancer
                if (t.t.gameObject.TryGetComponent<Animancer.NamedAnimancerComponent>(out var c))
                    t.t2.Set(new A_Animancer() { ani = c });
                else
#endif
                if (t.t.gameObject.TryGetComponent<UnityEngine.Animation>(out var c2))
                    t.t2.Set(new A_Animation() { ani = c2 });
                else if (t.t.gameObject.TryGetComponent<UnityEngine.Animator>(out var c3))
                    t.t2.Set(new A_Animator() { ani = c3, clips = c3.runtimeAnimatorController.animationClips });
#if Spine
                else if (t.t.gameObject.TryGetComponent<Spine.Unity.SkeletonAnimation>(out var c4))
                    t.t2.Set(new A_Spine() { ani = c4 });
#endif
                else
                    t.t2.Set(null);
            }
            else
                t.t2.Set(null);
        }

#if Animancer
        class A_Animancer : Playing
        {
            Animancer.AnimancerState state;
            public Animancer.NamedAnimancerComponent ani;

            public override float length => state == null ? 1 : state.Length;
            public override float time
            {
                get
                {
                    if (state == null) return 0;
                    return state.Time;
                }
                set
                {
                    if (state == null) return;
                    state.Time = value;
                }
            }
            public override float time01
            {
                get
                {
                    if (state == null) return 0;
                    return state.Time % length / length;
                }
                set
                {
                    if (state == null) return;
                    state.Time = value * length;
                }
            }


            public override void Play(string name, float fade)
            {
                base.Play(name, fade);
                state = ani.Layers[_layer].TryPlay(name, fade);
            }

            public override void SetAvatarMask(AvatarMask mask, int layer) => ani.Layers[_layer].SetMask(mask);

            public override void SetSpeed()
            {
                if (state != null)
                    state.Speed = Speed;
            }

            public override void SetEnable() => ani.Layers[_layer].IsAdditive = this.enable;

            public override float GetTime(string name)
            {
                if (ani.States.TryGet(name, out var s))
                    return s.Length;
                return 1;
            }

            public override Playing Clone(int layer) => new A_Animancer { ani = this.ani, _layer = layer };
        }
#endif
        class A_Animation : Playing
        {
            public UnityEngine.Animation ani;
            UnityEngine.AnimationState state;

            public override float length => state == null ? 1 : state.length;
            public override float time
            {
                get
                {
                    if (state == null) return 0;
                    return state.time;
                }
                set
                {
                    if (state == null) return;
                    state.time = value;
                }
            }
            public override float time01
            {
                get
                {
                    if (state == null) return 0;
                    return state.time % length / length;
                }
                set
                {
                    if (state == null) return;
                    state.time = value * length;
                }
            }

            public override float GetTime(string name)
            {
                var state = ani[playing.name];
                if (state != null)
                    return state.length;
                return 1;
            }

            public override void Play(string name, float fade)
            {
                base.Play(name, fade);
                ani.CrossFade(name, fade);
                state = ani[playing.name];
            }

            public override void SetSpeed()
            {
                if (state != null)
                    state.speed = Speed;
            }
            public override Playing Clone(int layer) => new A_Animation { ani = this.ani, _layer = layer };
        }
        class A_Animator : Playing
        {
            public UnityEngine.Animator ani;
            public AnimationClip[] clips;
            AnimationClip clip;

            public override float length => clip == null ? 1 : clip.length;
            public override float time
            {
                get => 0;
                set => ani.Play(playing.name, -1, value / length);
            }
            public override float time01
            {
                get => 0;
                set => ani.Play(playing.name, -1, value);
            }

            public override float GetTime(string name)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    if (clips[i].name == name)
                        return clips[i].length;
                }
                return 1;
            }

            public override void Play(string name, float fade)
            {
                base.Play(name, fade);
                ani.CrossFade(name, fade);
                for (int i = 0; i < clips.Length; i++)
                {
                    if (clips[i].name == name)
                    {
                        clip = clips[i];
                        break;
                    }
                }
            }

            public override void SetSpeed() => ani.speed = Speed;
            public override Playing Clone(int layer) => new A_Animator { ani = this.ani, clips = this.clips, _layer = layer };
        }
#if Spine
        class A_Spine : Playing
        {
            public Spine.Unity.SkeletonAnimation ani;
            Spine.Animation animation;

            public override float length => animation == null ? 1 : animation.Duration;
            public override float time
            {
                get
                {
                    if (animation == null) return 0;
                    var track = ani.AnimationState.GetCurrent(0);
                    if (track != null)
                        return track.TrackTime;
                    return 0;
                }
                set
                {
                    var track = ani.AnimationState.GetCurrent(0);
                    if (track != null)
                        track.TrackTime = value;
                }
            }
            public override float time01
            {
                get
                {
                    var track = ani.AnimationState.GetCurrent(0);
                    if (track != null)
                        return track.TrackTime / length;
                    return 0;
                }
                set
                {
                    var track = ani.AnimationState.GetCurrent(0);
                    if (track != null)
                        track.TrackTime = value * length;
                }
            }

            public override float GetTime(string name)
            {
                var s = ani.AnimationState.Data.SkeletonData.FindAnimation(name);
                if (s != null) return s.Duration;
                return 1;
            }

            public override void Play(string name, float fade)
            {
                base.Play(name, fade);
                ani.AnimationName = name;
                animation = ani.AnimationState.Data.SkeletonData.FindAnimation(name);
            }

            public override void SetSpeed() => ani.timeScale = Speed;
            public override Playing Clone(int layer) => new A_Spine { ani = this.ani, _layer = layer };
        }
#endif
    }
}
