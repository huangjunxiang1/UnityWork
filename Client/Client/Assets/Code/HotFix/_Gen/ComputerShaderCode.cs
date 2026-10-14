using UnityEngine;
using Unity.Mathematics;

public partial class ComputeShader_PathFinding
{
    public ComputeShader_PathFinding()
    {
        this.Shader = Game.Loader.Load<ComputeShader>(AssetsID.Res.shader.PathFinding);
        CSMain_kernel = Shader.FindKernel("CSMain");
    }

    public ComputeShader Shader { get; private set; }

    int2 _size;
    public int2 size
    {
        get => _size;
        set
        {
            _size = value;
            Shader.SetInts("size", value[0], value[1]);
        }
    }

    public int CSMain_kernel { get; private set; }

    GraphicsBuffer _road;
    public GraphicsBuffer road
    {
        get => _road;
        set
        {
            if (_road == value) return;
            if (_road != null && _road.IsValid())
                _road.Dispose();
            _road = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(CSMain_kernel, "road", value);
            }
        }
    }


    GraphicsBuffer _ps;
    public GraphicsBuffer ps
    {
        get => _ps;
        set
        {
            if (_ps == value) return;
            if (_ps != null && _ps.IsValid())
                _ps.Dispose();
            _ps = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(CSMain_kernel, "ps", value);
            }
        }
    }


    GraphicsBuffer _mark;
    public GraphicsBuffer mark
    {
        get => _mark;
        set
        {
            if (_mark == value) return;
            if (_mark != null && _mark.IsValid())
                _mark.Dispose();
            _mark = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(CSMain_kernel, "mark", value);
            }
        }
    }


    GraphicsBuffer _mvs;
    public GraphicsBuffer mvs
    {
        get => _mvs;
        set
        {
            if (_mvs == value) return;
            if (_mvs != null && _mvs.IsValid())
                _mvs.Dispose();
            _mvs = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(CSMain_kernel, "mvs", value);
            }
        }
    }


    GraphicsBuffer _temp;
    public GraphicsBuffer temp
    {
        get => _temp;
        set
        {
            if (_temp == value) return;
            if (_temp != null && _temp.IsValid())
                _temp.Dispose();
            _temp = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(CSMain_kernel, "temp", value);
            }
        }
    }


    GraphicsBuffer _targetP;
    public GraphicsBuffer targetP
    {
        get => _targetP;
        set
        {
            if (_targetP == value) return;
            if (_targetP != null && _targetP.IsValid())
                _targetP.Dispose();
            _targetP = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(CSMain_kernel, "targetP", value);
            }
        }
    }

    public void CSMain_Dispatch(int2 dispatch_xy) => CSMain_Dispatch(dispatch_xy.x, dispatch_xy.y);
    public void CSMain_Dispatch(int3 dispatch_xyz) => CSMain_Dispatch(dispatch_xyz.x, dispatch_xyz.y, dispatch_xyz.z);
    public void CSMain_Dispatch(int dispatch_x = 1, int dispatch_y = 1, int dispatch_z = 1)
    {
        dispatch_x = (dispatch_x - 1) / 8 + 1;
        dispatch_y = (dispatch_y - 1) / 8 + 1;
        dispatch_z = (dispatch_z - 1) / 1 + 1;
        CSMain_DispatchThreadXYZ(dispatch_x, dispatch_y, dispatch_z);
    }
    public void CSMain_DispatchThreadXYZ(int threadx = 1, int thready = 1, int threadz = 1) => Shader.Dispatch(CSMain_kernel, threadx, thready, threadz);

    public void Dispose()
    {
        if (_road != null && _road.IsValid())
            _road.Dispose();
        if (_ps != null && _ps.IsValid())
            _ps.Dispose();
        if (_mark != null && _mark.IsValid())
            _mark.Dispose();
        if (_mvs != null && _mvs.IsValid())
            _mvs.Dispose();
        if (_temp != null && _temp.IsValid())
            _temp.Dispose();
        if (_targetP != null && _targetP.IsValid())
            _targetP.Dispose();
    }
}
public partial class ComputeShader_GridCulling
{
    public ComputeShader_GridCulling()
    {
        this.Shader = Game.Loader.Load<ComputeShader>(AssetsID.Res.shader.GridCulling);
        Culling_kernel = Shader.FindKernel("Culling");
    }

    public ComputeShader Shader { get; private set; }

    int _maxBatchInstance;
    public int maxBatchInstance
    {
        get => _maxBatchInstance;
        set
        {
            _maxBatchInstance = value;
            Shader.SetInt("maxBatchInstance", value);
        }
    }

    int2 _playerPos_xy;
    public int2 playerPos_xy
    {
        get => _playerPos_xy;
        set
        {
            _playerPos_xy = value;
            Shader.SetInts("playerPos_xy", value[0], value[1]);
        }
    }

    public int Culling_kernel { get; private set; }

    GraphicsBuffer _wall_args;
    public GraphicsBuffer wall_args
    {
        get => _wall_args;
        set
        {
            if (_wall_args == value) return;
            if (_wall_args != null && _wall_args.IsValid())
                _wall_args.Dispose();
            _wall_args = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(Culling_kernel, "wall_args", value);
            }
        }
    }


    GraphicsBuffer _wall_Visible;
    public GraphicsBuffer wall_Visible
    {
        get => _wall_Visible;
        set
        {
            if (_wall_Visible == value) return;
            if (_wall_Visible != null && _wall_Visible.IsValid())
                _wall_Visible.Dispose();
            _wall_Visible = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(Culling_kernel, "wall_Visible", value);
            }
        }
    }


    GraphicsBuffer _wall_datas;
    public GraphicsBuffer wall_datas
    {
        get => _wall_datas;
        set
        {
            if (_wall_datas == value) return;
            if (_wall_datas != null && _wall_datas.IsValid())
                _wall_datas.Dispose();
            _wall_datas = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(Culling_kernel, "wall_datas", value);
            }
        }
    }


    GraphicsBuffer _tree_args;
    public GraphicsBuffer tree_args
    {
        get => _tree_args;
        set
        {
            if (_tree_args == value) return;
            if (_tree_args != null && _tree_args.IsValid())
                _tree_args.Dispose();
            _tree_args = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(Culling_kernel, "tree_args", value);
            }
        }
    }


    GraphicsBuffer _tree_Visible;
    public GraphicsBuffer tree_Visible
    {
        get => _tree_Visible;
        set
        {
            if (_tree_Visible == value) return;
            if (_tree_Visible != null && _tree_Visible.IsValid())
                _tree_Visible.Dispose();
            _tree_Visible = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(Culling_kernel, "tree_Visible", value);
            }
        }
    }


    GraphicsBuffer _tree_datas;
    public GraphicsBuffer tree_datas
    {
        get => _tree_datas;
        set
        {
            if (_tree_datas == value) return;
            if (_tree_datas != null && _tree_datas.IsValid())
                _tree_datas.Dispose();
            _tree_datas = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(Culling_kernel, "tree_datas", value);
            }
        }
    }

    public void Culling_Dispatch(int2 dispatch_xy) => Culling_Dispatch(dispatch_xy.x, dispatch_xy.y);
    public void Culling_Dispatch(int3 dispatch_xyz) => Culling_Dispatch(dispatch_xyz.x, dispatch_xyz.y, dispatch_xyz.z);
    public void Culling_Dispatch(int dispatch_x = 1, int dispatch_y = 1, int dispatch_z = 1)
    {
        dispatch_x = (dispatch_x - 1) / 1 + 1;
        dispatch_y = (dispatch_y - 1) / 1 + 1;
        dispatch_z = (dispatch_z - 1) / 1 + 1;
        Culling_DispatchThreadXYZ(dispatch_x, dispatch_y, dispatch_z);
    }
    public void Culling_DispatchThreadXYZ(int threadx = 1, int thready = 1, int threadz = 1) => Shader.Dispatch(Culling_kernel, threadx, thready, threadz);

    public void Dispose()
    {
        if (_wall_args != null && _wall_args.IsValid())
            _wall_args.Dispose();
        if (_wall_Visible != null && _wall_Visible.IsValid())
            _wall_Visible.Dispose();
        if (_wall_datas != null && _wall_datas.IsValid())
            _wall_datas.Dispose();
        if (_tree_args != null && _tree_args.IsValid())
            _tree_args.Dispose();
        if (_tree_Visible != null && _tree_Visible.IsValid())
            _tree_Visible.Dispose();
        if (_tree_datas != null && _tree_datas.IsValid())
            _tree_datas.Dispose();
    }
}
public partial class ComputeShader_Emerge
{
    public ComputeShader_Emerge()
    {
        this.Shader = Game.Loader.Load<ComputeShader>(AssetsID.Res.shader.Emerge);
        kn_mapReSize_kernel = Shader.FindKernel("kn_mapReSize");
        kn_unitAction_kernel = Shader.FindKernel("kn_unitAction");
        kn_spawnUnit_kernel = Shader.FindKernel("kn_spawnUnit");
    }

    public ComputeShader Shader { get; private set; }

    int __RandomSeed;
    public int _RandomSeed
    {
        get => __RandomSeed;
        set
        {
            __RandomSeed = value;
            Shader.SetInt("_RandomSeed", value);
        }
    }

    int __SpawnSeed;
    public int _SpawnSeed
    {
        get => __SpawnSeed;
        set
        {
            __SpawnSeed = value;
            Shader.SetInt("_SpawnSeed", value);
        }
    }

    float __ExpandChance;
    public float _ExpandChance
    {
        get => __ExpandChance;
        set
        {
            __ExpandChance = value;
            Shader.SetFloat("_ExpandChance", value);
        }
    }

    float __SpawnChance;
    public float _SpawnChance
    {
        get => __SpawnChance;
        set
        {
            __SpawnChance = value;
            Shader.SetFloat("_SpawnChance", value);
        }
    }

    float __GeneralRate;
    public float _GeneralRate
    {
        get => __GeneralRate;
        set
        {
            __GeneralRate = value;
            Shader.SetFloat("_GeneralRate", value);
        }
    }

    public int kn_mapReSize_kernel { get; private set; }
    public int kn_unitAction_kernel { get; private set; }
    public int kn_spawnUnit_kernel { get; private set; }

    GraphicsBuffer _kingLand;
    public GraphicsBuffer kingLand
    {
        get => _kingLand;
        set
        {
            if (_kingLand == value) return;
            if (_kingLand != null && _kingLand.IsValid())
                _kingLand.Dispose();
            _kingLand = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(kn_mapReSize_kernel, "kingLand", value);
                Shader.SetBuffer(kn_unitAction_kernel, "kingLand", value);
                Shader.SetBuffer(kn_spawnUnit_kernel, "kingLand", value);
            }
        }
    }


    GraphicsBuffer _unitLand;
    public GraphicsBuffer unitLand
    {
        get => _unitLand;
        set
        {
            if (_unitLand == value) return;
            if (_unitLand != null && _unitLand.IsValid())
                _unitLand.Dispose();
            _unitLand = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(kn_unitAction_kernel, "unitLand", value);
                Shader.SetBuffer(kn_spawnUnit_kernel, "unitLand", value);
            }
        }
    }


    GraphicsBuffer _unitCenter;
    public GraphicsBuffer unitCenter
    {
        get => _unitCenter;
        set
        {
            if (_unitCenter == value) return;
            if (_unitCenter != null && _unitCenter.IsValid())
                _unitCenter.Dispose();
            _unitCenter = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(kn_unitAction_kernel, "unitCenter", value);
                Shader.SetBuffer(kn_spawnUnit_kernel, "unitCenter", value);
            }
        }
    }


    GraphicsBuffer _unitDir;
    public GraphicsBuffer unitDir
    {
        get => _unitDir;
        set
        {
            if (_unitDir == value) return;
            if (_unitDir != null && _unitDir.IsValid())
                _unitDir.Dispose();
            _unitDir = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(kn_unitAction_kernel, "unitDir", value);
                Shader.SetBuffer(kn_spawnUnit_kernel, "unitDir", value);
            }
        }
    }


    GraphicsBuffer _pheromoneLand;
    public GraphicsBuffer pheromoneLand
    {
        get => _pheromoneLand;
        set
        {
            if (_pheromoneLand == value) return;
            if (_pheromoneLand != null && _pheromoneLand.IsValid())
                _pheromoneLand.Dispose();
            _pheromoneLand = value;
            if (value != null && value.IsValid())
            {
                Shader.SetBuffer(kn_unitAction_kernel, "pheromoneLand", value);
            }
        }
    }

    public void kn_mapReSize_Dispatch(int2 dispatch_xy) => kn_mapReSize_Dispatch(dispatch_xy.x, dispatch_xy.y);
    public void kn_mapReSize_Dispatch(int3 dispatch_xyz) => kn_mapReSize_Dispatch(dispatch_xyz.x, dispatch_xyz.y, dispatch_xyz.z);
    public void kn_mapReSize_Dispatch(int dispatch_x = 1, int dispatch_y = 1, int dispatch_z = 1)
    {
        dispatch_x = (dispatch_x - 1) / 32 + 1;
        dispatch_y = (dispatch_y - 1) / 32 + 1;
        dispatch_z = (dispatch_z - 1) / 1 + 1;
        kn_mapReSize_DispatchThreadXYZ(dispatch_x, dispatch_y, dispatch_z);
    }
    public void kn_mapReSize_DispatchThreadXYZ(int threadx = 1, int thready = 1, int threadz = 1) => Shader.Dispatch(kn_mapReSize_kernel, threadx, thready, threadz);

    public void kn_unitAction_Dispatch(int2 dispatch_xy) => kn_unitAction_Dispatch(dispatch_xy.x, dispatch_xy.y);
    public void kn_unitAction_Dispatch(int3 dispatch_xyz) => kn_unitAction_Dispatch(dispatch_xyz.x, dispatch_xyz.y, dispatch_xyz.z);
    public void kn_unitAction_Dispatch(int dispatch_x = 1, int dispatch_y = 1, int dispatch_z = 1)
    {
        dispatch_x = (dispatch_x - 1) / 32 + 1;
        dispatch_y = (dispatch_y - 1) / 32 + 1;
        dispatch_z = (dispatch_z - 1) / 1 + 1;
        kn_unitAction_DispatchThreadXYZ(dispatch_x, dispatch_y, dispatch_z);
    }
    public void kn_unitAction_DispatchThreadXYZ(int threadx = 1, int thready = 1, int threadz = 1) => Shader.Dispatch(kn_unitAction_kernel, threadx, thready, threadz);

    public void kn_spawnUnit_Dispatch(int2 dispatch_xy) => kn_spawnUnit_Dispatch(dispatch_xy.x, dispatch_xy.y);
    public void kn_spawnUnit_Dispatch(int3 dispatch_xyz) => kn_spawnUnit_Dispatch(dispatch_xyz.x, dispatch_xyz.y, dispatch_xyz.z);
    public void kn_spawnUnit_Dispatch(int dispatch_x = 1, int dispatch_y = 1, int dispatch_z = 1)
    {
        dispatch_x = (dispatch_x - 1) / 32 + 1;
        dispatch_y = (dispatch_y - 1) / 32 + 1;
        dispatch_z = (dispatch_z - 1) / 1 + 1;
        kn_spawnUnit_DispatchThreadXYZ(dispatch_x, dispatch_y, dispatch_z);
    }
    public void kn_spawnUnit_DispatchThreadXYZ(int threadx = 1, int thready = 1, int threadz = 1) => Shader.Dispatch(kn_spawnUnit_kernel, threadx, thready, threadz);

    public void Dispose()
    {
        if (_kingLand != null && _kingLand.IsValid())
            _kingLand.Dispose();
        if (_unitLand != null && _unitLand.IsValid())
            _unitLand.Dispose();
        if (_unitCenter != null && _unitCenter.IsValid())
            _unitCenter.Dispose();
        if (_unitDir != null && _unitDir.IsValid())
            _unitDir.Dispose();
        if (_pheromoneLand != null && _pheromoneLand.IsValid())
            _pheromoneLand.Dispose();
    }
}
