using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;

namespace Game
{
    public static class Map 
    {

        [Msg(OuterOpcode.G2C_EnterMap)]
        static void EnterMap(IMessage message)
        {
            G2C_EnterMap rep = message as G2C_EnterMap;
            AddWObjects(rep.Units);
        }
        [Msg(OuterOpcode.M2C_CreateUnits)]
        static void CreateUnits(IMessage message)
        {
            M2C_CreateUnits rep = message as M2C_CreateUnits;
            AddWObjects(rep.Units);
        }
        [Msg(OuterOpcode.M2C_DestroyUnit)]
        static void DestroyUnit(IMessage message)
        {
            M2C_DestroyUnit rep = message as M2C_DestroyUnit;
            GameM.World.GetChildCid(rep.Units)?.Dispose();
        }
        [Msg(OuterOpcode.M2C_PathfindingResult)]
        static void PathfindingResult(IMessage message)
        {
            M2C_PathfindingResult rep = message as M2C_PathfindingResult;
            List<Vector3> path = new List<Vector3>(rep.Xs.Count);
            for (int i = 0; i < rep.Xs.Count; i++)
                path.Add(new Vector3(rep.Xs[i], rep.Ys[i], rep.Zs[i]));
            WRole role = GameM.World.GetChildCid(rep.Id) as WRole;
            role.MovePath(path);
        }

        [Msg(OuterOpcode.M2C_Stop)]
        static void M2C_Stop(IMessage message)
        {
            M2C_Stop rep = message as M2C_Stop;
            WRole role = GameM.World.GetChildCid(rep.Id) as WRole;
            role.Stop(new Vector3(rep.X, rep.Y, rep.Z));
        }

        public static void AddWObjects(List<UnitInfo> Units)
        {
            for (int i = 0; i < Units.Count; i++)
                AddWObject(Units[i]);
        }
        public static void AddWObject(UnitInfo Unit)
        {
            WRole role = GameM.World.GetChildCid(Unit.UnitId) as WRole;
            if (role == null)
            {
                role = new WRole(Unit.UnitId);
                GameM.World.AddChild(role);
                role.LoadRes("3D/Model/Unit/Cube.prefab");
            }
            role.UpdateUnitInfo(Unit);
        }
    }
}
