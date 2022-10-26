using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using Main;
using Game;
using Unity.Physics;


[MaterialProperty("_EmissiveColor")]
public struct HDRPMaterialPropertyEmissiveColor1 : IComponentData { public float4 Value; }