using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Main;

namespace Game
{
    public class WUnit : WObject
    {
        public WUnit(long unitId, GameObject gameObject) : base(unitId, gameObject)
        {
            this.Animator = gameObject.GetComponent<Animator>();
        }

        List<Vector3> _totalPos;
        List<long> _totalTimes;
        long _startUtc;
        TaskAwaiter<GameObject> _pathLineTask;
        GameObject _pathLine;


        public Animator Animator { get; }
        public float Speed { get; set; } = 1;
        public UnitInfo unitInfo { get; private set; }


        public void UpdateUnitInfo(UnitInfo info)
        {
            this.unitInfo = info;
            int idx = info.Ks.FindIndex(t => t == 1000);
            this.Position = new Vector3(info.X, info.Y, info.Z);
            Speed = info.Vs[idx] / 10000f;
        }
        public void MoveTo(Vector3 pos)
        {
            MovePath(new List<Vector3>(2) { this.Position, pos });
        }
        public async void MovePath(List<Vector3> posLst)
        {
            _totalPos = posLst;
            _totalTimes = new List<long>(posLst.Count - 1);
            for (int i = 0; i < posLst.Count - 1; i++)
            {
                float distance = Vector3.Distance(posLst[i], posLst[i + 1]);
                _totalTimes.Add((long)((distance / Speed) * 1000));
            }
            _startUtc = Timer.ServerTime;
            if (!Timer.Contains(_moveUpdate))
                Timer.Add(0, -1, _moveUpdate);

            if (!_pathLine)
                _pathLine = await LoadPrefabAsyncRef("3D/Util/pathLine.prefab", ref _pathLineTask);
            _pathLine.transform.SetParent(WRoot.Inst.GameObject.transform);
            _pathLine.transform.rotation = Quaternion.Euler(90, 0, 0);
            LineRenderer line = _pathLine.GetComponent<LineRenderer>();
            line.positionCount = posLst.Count;
            line.SetPositions(posLst.ToArray());
        }
        public void Stop(Vector3 pos)
        {
            this.Position = pos;
            Timer.Remove(_moveUpdate);
            if (_pathLine)
            {
                AssetLoad.PrefabLoader.Release(_pathLine);
                _pathLine = null;
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            if (_pathLine)
            {
                AssetLoad.PrefabLoader.Release(_pathLine);
                _pathLine = null;
            }
        }

        void _moveUpdate()
        {
            long offsetTime = Timer.ServerTime - _startUtc;
            long t = 0;
            bool set = false;
            for (int i = 0; i < _totalTimes.Count; i++)
            {
                if (offsetTime <= t + _totalTimes[i])
                {
                    float v = Mathf.Clamp01((offsetTime - t) / (float)_totalTimes[i]);
                    this.Position = Vector3.Lerp(_totalPos[i], _totalPos[i + 1], v);
                    set = true;
                    break;
                }
                t += _totalTimes[i];
            }
            //已经移动完毕
            if (!set)
            {
                this.Position = _totalPos[_totalPos.Count - 1];
                Timer.Remove(_moveUpdate);
                if (_pathLine)
                {
                    AssetLoad.PrefabLoader.Release(_pathLine);
                    _pathLine = null;
                }
            }
        }
    }
}
