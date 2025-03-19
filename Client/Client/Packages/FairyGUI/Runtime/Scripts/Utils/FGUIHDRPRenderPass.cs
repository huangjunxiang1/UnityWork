using UnityEngine.Rendering;
using UnityEngine;
using FairyGUI;


#if HDRP
using UnityEngine.Rendering.HighDefinition;
#endif

#if HDRP
class FGUIHDRPRenderPass : CustomPass
{
    public LayerMask layer;

    Vector3 origin;
    Matrix4x4 worldToCameraMatrix;
    Matrix4x4 projectionMatrix;
    Matrix4x4 cullingMatrix;
    Plane[] planes = new Plane[6];

    public void ApplyChange(Transform stage)
    {
        origin = stage.position;
        origin.z = -100;
        worldToCameraMatrix = Matrix4x4.TRS(-origin, Quaternion.identity, new Vector3(1, 1, 1));
        projectionMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(2 / (origin.x * 2 - 0), 2 / -(origin.y * 2 - 0), 0));
        cullingMatrix = projectionMatrix * worldToCameraMatrix;
        GeometryUtility.CalculateFrustumPlanes(cullingMatrix, planes);
        if (StageCamera.main)
        {
            StageCamera.main.enabled = true;
            StageCamera.main.enabled = false;
        }
    }
    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        
    }
    protected override void AggregateCullingParameters(ref ScriptableCullingParameters cullingParameters, HDCamera hdCamera)
    {
        if (hdCamera.camera.cameraType == CameraType.Game)
        {
            cullingParameters.cullingMask |= (uint)layer.value;
            cullingParameters.cullingMatrix = cullingMatrix;

            for (int i = 0; i < 6; i++)
                cullingParameters.SetCullingPlane(i, planes[i]);
        }
    }
    protected override void Execute(CustomPassContext ctx)
    {
        if (ctx.hdCamera.camera.cameraType == CameraType.Game)
        {
            ctx.cmd.SetViewProjectionMatrices(worldToCameraMatrix, projectionMatrix);

            CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer, this.clearFlags);
            CustomPassUtils.DrawRenderers(ctx, layer);

            //还原矩阵
            ctx.cmd.SetViewProjectionMatrices(ctx.hdCamera.camera.worldToCameraMatrix, ctx.hdCamera.camera.projectionMatrix);
        }
    }
    protected override void Cleanup()
    {
        
    }
}
#endif