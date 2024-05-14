using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class Playing
    {
        public abstract void Play(string name, float fade);

    }

    public class PlayingComponent : SComponent
    {
        public string name { get; private set; }
        public Playing play { get; private set; }

        public void Play(string name, float fade = 0.2f)
        {
            if (this.name == name) return;
            this.name = name;
            if (play == null || string.IsNullOrEmpty(name)) return;
            play.Play(name, fade);
            this.SetChange();
        }
        public void Set(Playing play)
        {
            this.play = play;
            if (play == null || string.IsNullOrEmpty(name)) return;
            play?.Play(name, 0.2f);
        }

        [Event]
        static void AnyChange(AnyChange<GameObjectComponent, PlayingComponent> t)
        {
            if (t.t.gameObject)
            {
                if (t.t.gameObject.TryGetComponent<Animancer.NamedAnimancerComponent>(out var c))
                    t.t2.Set(new A_Animancer() { ani = c });
                else if (t.t.gameObject.TryGetComponent<UnityEngine.Animation>(out var c2))
                    t.t2.Set(new A_Animation() { ani = c2 });
                else if (t.t.gameObject.TryGetComponent<UnityEngine.Animator>(out var c3))
                    t.t2.Set(new A_Animator() { ani = c3 });
                else if (t.t.gameObject.TryGetComponent<Spine.Unity.SkeletonAnimation>(out var c4))
                    t.t2.Set(new A_Spine() { ani = c4 });
                else
                    t.t2.Set(null);
            }
            else
                t.t2.Set(null);
        }

        class A_Animancer : Playing
        {
            public Animancer.NamedAnimancerComponent ani;
            public override void Play(string name, float fade)
            {
                ani.TryPlay(name, fade);
            }
        }
        class A_Animation : Playing
        {
            public UnityEngine.Animation ani;
            public override void Play(string name, float fade)
            {
                ani.CrossFade(name, fade);
            }
        }
        class A_Animator : Playing
        {
            public UnityEngine.Animator ani;
            public override void Play(string name, float fade)
            {
                ani.CrossFade(name, fade);
            }
        }
        class A_Spine : Playing
        {
            public Spine.Unity.SkeletonAnimation ani;
            public override void Play(string name, float fade)
            {
                ani.AnimationName = name;
            }
        }
    }
}
