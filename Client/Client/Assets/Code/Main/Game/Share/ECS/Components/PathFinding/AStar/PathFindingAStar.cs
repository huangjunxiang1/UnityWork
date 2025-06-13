using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [ExecuteInEditMode]
    class PathFindingAStar : MonoBehaviour
    {
        public float3 size = new(1, 0, 1);
        public int2 aStarSize = new int2(10, 10);
        public string savePath = "Res/Config/raw/Map/AStarData/";

        internal byte[] data;
        internal int2 dataSize;
    }
}
