using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class WObjectManager : Manager<WObjectManager>
    {
        [Msg(OuterOpcode.G2C_EnterMap)]
        void EnterMap(IMessage message)
        {
            G2C_EnterMap rep = message as G2C_EnterMap;
            this.AddWObjects(rep.Units);
        }
        [Msg(OuterOpcode.M2C_CreateUnits)]
        void CreateUnits(IMessage message)
        {
            M2C_CreateUnits rep = message as M2C_CreateUnits;
            this.AddWObjects(rep.Units);
        }
        [Msg(OuterOpcode.M2C_PathfindingResult)]
        void PathfindingResult(IMessage message)
        {
            M2C_PathfindingResult rep = message as M2C_PathfindingResult;
            List<Vector3> path = new List<Vector3>(rep.Xs.Count);
            for (int i = 0; i < rep.Xs.Count; i++)
                path.Add(new Vector3(rep.Xs[i], rep.Ys[i], rep.Zs[i]));
            WRole role = WRoot.Inst.GetChild(rep.Id) as WRole;
            role.MovePath(path);
        }

        [Msg(OuterOpcode.M2C_Stop)]
        void M2C_Stop(IMessage message)
        {
            M2C_Stop rep = message as M2C_Stop;
            WRole role = WRoot.Inst.GetChild(rep.Id) as WRole;
            role.Stop(new Vector3(rep.X, rep.Y, rep.Z));
        }

        public void AddWObjects(List<UnitInfo> Units)
        {
            for (int i = 0; i < Units.Count; i++)
                AddWObject(Units[i]);
        }
        public void AddWObject(UnitInfo Unit)
        {
            WRole role = WRoot.Inst.GetChild(Unit.UnitId) as WRole;
            if (role == null)
            {
                role = new WRole(Unit.UnitId, GameObject.CreatePrimitive(PrimitiveType.Cube));
                WRoot.Inst.AddChild(role);
            }
            role.UpdateUnitInfo(Unit);
        }
    }
}
