using FairyGUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#if URP
using UnityEngine.Rendering.Universal;

public class FGUIURPRenderFeature : ScriptableRendererFeature
{
    [Serializable]
    class Setting
    {
        public RenderPassEvent Event = RenderPassEvent.AfterRenderingPostProcessing;
        public LayerMask LayerMask;
    }

    [SerializeField]
    Setting setting = new();
    FGUIURPRenderPass pass;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game && renderingData.cameraData.camera == Camera.main)
        {
            renderer.EnqueuePass(pass);
            pass.ApplyChange();
        }
    }

    public override void Create()
    {
        pass = new FGUIURPRenderPass(setting);
        pass.renderPassEvent = setting.Event;
    }

    class FGUIURPRenderPass : ScriptableRenderPass
    {
        public FGUIURPRenderPass(Setting setting)
        {
            base.profilingSampler = new ProfilingSampler(nameof(FGUIURPRenderPass));
            this.setting = setting;
        }

        Setting setting;
        int width = 0;
        int heigth = 0;
        Matrix4x4 worldToCameraMatrix;
        Matrix4x4 projectionMatrix;
        Matrix4x4 cullingMatrix;
        Plane[] planes = new Plane[6];

        ShaderTagId[] tags = new ShaderTagId[3] { new ShaderTagId("SRPDefaultUnlit"), new ShaderTagId("UniversalForward"), new ShaderTagId("UniversalForwardOnly") };
        FilteringSettings fs = new FilteringSettings(RenderQueueRange.transparent);

        ProfilingSampler _profilingSampler = new ProfilingSampler(nameof(FGUIURPRenderPass));

        public void ApplyChange()
        {
            if (width == Screen.width && heigth == Screen.height)
                return;
            width = Screen.width;
            heigth = Screen.height;
            var v3 = new Vector3(width * 5f / heigth, -5, -10);
            worldToCameraMatrix = Matrix4x4.TRS(-v3, Quaternion.identity, new Vector3(1, 1, 1));
            projectionMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(2 / (v3.x * 2 - 0), 2 / (v3.y * 2 - 0), 0));
            cullingMatrix = projectionMatrix * worldToCameraMatrix;
            GeometryUtility.CalculateFrustumPlanes(cullingMatrix, planes);
            if (StageCamera.main)
            {
                StageCamera.main.enabled = true;
                StageCamera.main.enabled = false;
            }
        }
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, _profilingSampler))
            {
                DrawingSettings ds = CreateDrawingSettings(tags, ref renderingData, SortingCriteria.CommonTransparent);
                renderingData.cameraData.camera.TryGetCullingParameters(out var culling);
                culling.cullingMask = (uint)setting.LayerMask.value;
                culling.cullingMatrix = cullingMatrix;
                for (int i = 0; i < 6; i++)
                    culling.SetCullingPlane(i, planes[i]);
                var cull = context.Cull(ref culling);
                RenderingUtils.SetViewAndProjectionMatrices(cmd, worldToCameraMatrix, projectionMatrix, false);
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                context.DrawRenderers(cull, ref ds, ref fs);
                RenderingUtils.SetViewAndProjectionMatrices(cmd, renderingData.cameraData.GetViewMatrix(), renderingData.cameraData.GetGPUProjectionMatrix(), false);
            }
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}

#endif