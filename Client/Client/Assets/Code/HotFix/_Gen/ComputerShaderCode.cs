using Game;
using UnityEngine;
using Unity.Mathematics;

public partial class ComputeShader_PathFinding
{
    public ComputeShader_PathFinding()
    {
        this.Shader = SAsset.Load<ComputeShader>("shader_PathFinding");
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

    GraphicsBuffer _CSMain_road;
    public GraphicsBuffer CSMain_road
    {
        get => _CSMain_road;
        set
        {
            if (_CSMain_road != null && _CSMain_road.IsValid())
                _CSMain_road.Dispose();
            _CSMain_road = value;
            if (value != null && value.IsValid())
                Shader.SetBuffer(CSMain_kernel, "road", value);
        }
    }


    GraphicsBuffer _CSMain_ps;
    public GraphicsBuffer CSMain_ps
    {
        get => _CSMain_ps;
        set
        {
            if (_CSMain_ps != null && _CSMain_ps.IsValid())
                _CSMain_ps.Dispose();
            _CSMain_ps = value;
            if (value != null && value.IsValid())
                Shader.SetBuffer(CSMain_kernel, "ps", value);
        }
    }


    GraphicsBuffer _CSMain_mark;
    public GraphicsBuffer CSMain_mark
    {
        get => _CSMain_mark;
        set
        {
            if (_CSMain_mark != null && _CSMain_mark.IsValid())
                _CSMain_mark.Dispose();
            _CSMain_mark = value;
            if (value != null && value.IsValid())
                Shader.SetBuffer(CSMain_kernel, "mark", value);
        }
    }


    GraphicsBuffer _CSMain_mvs;
    public GraphicsBuffer CSMain_mvs
    {
        get => _CSMain_mvs;
        set
        {
            if (_CSMain_mvs != null && _CSMain_mvs.IsValid())
                _CSMain_mvs.Dispose();
            _CSMain_mvs = value;
            if (value != null && value.IsValid())
                Shader.SetBuffer(CSMain_kernel, "mvs", value);
        }
    }


    GraphicsBuffer _CSMain_temp;
    public GraphicsBuffer CSMain_temp
    {
        get => _CSMain_temp;
        set
        {
            if (_CSMain_temp != null && _CSMain_temp.IsValid())
                _CSMain_temp.Dispose();
            _CSMain_temp = value;
            if (value != null && value.IsValid())
                Shader.SetBuffer(CSMain_kernel, "temp", value);
        }
    }


    GraphicsBuffer _CSMain_targetP;
    public GraphicsBuffer CSMain_targetP
    {
        get => _CSMain_targetP;
        set
        {
            if (_CSMain_targetP != null && _CSMain_targetP.IsValid())
                _CSMain_targetP.Dispose();
            _CSMain_targetP = value;
            if (value != null && value.IsValid())
                Shader.SetBuffer(CSMain_kernel, "targetP", value);
        }
    }

    public void CSMain_Dispatch(int threadx = 8, int thready = 8, int threadz = 1) => Shader.Dispatch(CSMain_kernel, threadx, thready, threadz);

    public void Dispose()
    {
        if (_CSMain_road != null && _CSMain_road.IsValid())
            _CSMain_road.Dispose();
        if (_CSMain_ps != null && _CSMain_ps.IsValid())
            _CSMain_ps.Dispose();
        if (_CSMain_mark != null && _CSMain_mark.IsValid())
            _CSMain_mark.Dispose();
        if (_CSMain_mvs != null && _CSMain_mvs.IsValid())
            _CSMain_mvs.Dispose();
        if (_CSMain_temp != null && _CSMain_temp.IsValid())
            _CSMain_temp.Dispose();
        if (_CSMain_targetP != null && _CSMain_targetP.IsValid())
            _CSMain_targetP.Dispose();
    }
}
public partial class ComputeShader_GridCulling
{
    public ComputeShader_GridCulling()
    {
        this.Shader = SAsset.Load<ComputeShader>("shader_GridCulling");
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

    GraphicsBuffer _Culling_grid_args;
    public GraphicsBuffer Culling_grid_args
    {
        get => _Culling_grid_args;
        set
        {
            if (_Culling_grid_args != null && _Culling_grid_args.IsValid())
                _Culling_grid_args.Dispose();
            _Culling_grid_args = value;
            if (value != null && value.IsValid())
                Shader.SetBuffer(Culling_kernel, "grid_args", value);
        }
    }


    GraphicsBuffer _Culling_grid_datas;
    public GraphicsBuffer Culling_grid_datas
    {
        get => _Culling_grid_datas;
        set
        {
            if (_Culling_grid_datas != null && _Culling_grid_datas.IsValid())
                _Culling_grid_datas.Dispose();
            _Culling_grid_datas = value;
            if (value != null && value.IsValid())
                Shader.SetBuffer(Culling_kernel, "grid_datas", value);
        }
    }


    GraphicsBuffer _Culling_tree_args;
    public GraphicsBuffer Culling_tree_args
    {
        get => _Culling_tree_args;
        set
        {
            if (_Culling_tree_args != null && _Culling_tree_args.IsValid())
                _Culling_tree_args.Dispose();
            _Culling_tree_args = value;
            if (value != null && value.IsValid())
                Shader.SetBuffer(Culling_kernel, "tree_args", value);
        }
    }


    GraphicsBuffer _Culling_tree_datas;
    public GraphicsBuffer Culling_tree_datas
    {
        get => _Culling_tree_datas;
        set
        {
            if (_Culling_tree_datas != null && _Culling_tree_datas.IsValid())
                _Culling_tree_datas.Dispose();
            _Culling_tree_datas = value;
            if (value != null && value.IsValid())
                Shader.SetBuffer(Culling_kernel, "tree_datas", value);
        }
    }

    public void Culling_Dispatch(int threadx = 1, int thready = 1, int threadz = 1) => Shader.Dispatch(Culling_kernel, threadx, thready, threadz);

    public void Dispose()
    {
        if (_Culling_grid_args != null && _Culling_grid_args.IsValid())
            _Culling_grid_args.Dispose();
        if (_Culling_grid_datas != null && _Culling_grid_datas.IsValid())
            _Culling_grid_datas.Dispose();
        if (_Culling_tree_args != null && _Culling_tree_args.IsValid())
            _Culling_tree_args.Dispose();
        if (_Culling_tree_datas != null && _Culling_tree_datas.IsValid())
            _Culling_tree_datas.Dispose();
    }
}
