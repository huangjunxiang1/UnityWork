using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public abstract class Playing
    {
        public PlayingComponent playing { get; internal set; }
        public abstract void Play(string name, float fade);
        public abstract void SetSpeed();
        public abstract float GetTime(string name);
    }

    public class PlayingComponent : SComponent
    {
        float _speed = 1;

        public float Speed
        {
            get => _speed;
            set
            {
                if (_speed == value) return;
                _speed = value;
                play?.SetSpeed();
                this.SetChange();
            }
        }

        public string name { get; private set; }
        public Playing play { get; private set; }

        public void Play(string name, float fade = 0.2f)
        {
            if (this.name == name) return;
            this.name = name;
            if (play == null || string.IsNullOrEmpty(name)) return;
            play.Play(name, fade);
            play.SetSpeed();
            this.SetChange();
        }
        public void Set(Playing play)
        {
            this.play = play;
            if (play == null) return;
            play.playing = this;
            if (!string.IsNullOrEmpty(name))
                play.Play(name, 0.2f);
            play.SetSpeed();
            //this.SetChange(); 这里不能调用 因为会陷入AnyChange的递归
        }
        public float GetTime(string name)
        {
            if (play == null) return 1;
            return play.GetTime(name);
        }
        public float GetTime() => GetTime(this.name);

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
                    t.t2.Set(new A_Animator() { ani = c3 });
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
            public override void Play(string name, float fade)
            {
                state = ani.TryPlay(name, fade);
            }

            public override void SetSpeed()
            {
                if (state != null)
                    state.Speed = playing.Speed;
            }

            public override float GetTime(string name)
            {
                if (ani.States.TryGet(name, out var s))
                    return s.Length;
                return 1;
            }
        }
#endif
        class A_Animation : Playing
        {
            public UnityEngine.Animation ani;

            public override float GetTime(string name)
            {
                var state = ani[playing.name];
                if (state != null)
                    return state.length;
                return 1;
            }

            public override void Play(string name, float fade)
            {
                ani.CrossFade(name, fade);
            }

            public override void SetSpeed()
            {
                var state = ani[playing.name];
                if (state != null)
                    state.speed = playing.Speed;
            }
        }
        class A_Animator : Playing
        {
            public UnityEngine.Animator ani;

            public override float GetTime(string name)
            {
                AnimationClip[] clips = ani.runtimeAnimatorController.animationClips;
                for (int i = 0; i < clips.Length; i++)
                {
                    if (clips[i].name == name)
                        return clips[i].length;
                }
                return 1;
            }

            public override void Play(string name, float fade)
            {
                ani.CrossFade(name, fade);
            }

            public override void SetSpeed()
            {
                ani.speed = playing.Speed;
            }
        }
#if Spine
        class A_Spine : Playing
        {
            public Spine.Unity.SkeletonAnimation ani;

            public override float GetTime(string name)
            {
                var s = ani.AnimationState.Data.SkeletonData.FindAnimation(name);
                if (s != null) return s.Duration;
                return 1;
            }

            public override void Play(string name, float fade)
            {
                ani.AnimationName = name;
            }

            public override void SetSpeed()
            {
                ani.timeScale = playing.Speed;
            }
        }
#endif
    }
}
