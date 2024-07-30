
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;

[CustomEditor(typeof(BakeryDirectLight))]
[CanEditMultipleObjects]
public class ftDirectLightInspector : UnityEditor.Editor
{
    SerializedProperty ftraceLightColor;
    SerializedProperty ftraceLightIntensity;
    SerializedProperty ftraceLightShadowSpread;
    SerializedProperty ftraceLightSamples;
    SerializedProperty ftraceLightBitmask;
    SerializedProperty ftraceLightBakeToIndirect;
    SerializedProperty ftraceLightShadowmask;
    SerializedProperty ftraceLightShadowmaskDenoise;
    SerializedProperty ftraceLightIndirectIntensity;
    SerializedProperty ftraceLightTexture, ftraceLightCSTilingX, ftraceLightCSTilingY, ftraceLightCSOffsetX, ftraceLightCSOffsetY;
    SerializedProperty ftraceLightSupersample;

    ftLightmapsStorage storage;

    static bool projectionMode = false;

    bool isHDRP = false;

    public enum BakeWhat
    {
        DirectAndIndirect = 0,
        IndirectOnly = 1,
        IndirectAndShadowmask = 2,
        DirectIndirectShadowmask = 3
    };

    int texCached = -1;

    void TestPreviewRefreshProperty(ref int cached, int newVal)
    {
        if (cached >= 0)
        {
            if (cached != newVal)
            {
                BakerySkyLight.lightsChanged = 2;
            }
        }
        cached = newVal;
    }

    void TestPreviewRefreshProperty(ref int cached, UnityEngine.Object newVal)
    {
        if (newVal == null)
        {
            TestPreviewRefreshProperty(ref cached, 0);
            return;
        }
        TestPreviewRefreshProperty(ref cached, newVal.GetInstanceID());
    }

    static public string[] directContributionOptions = new string[] {"Direct And Indirect", "Indirect Only", "Shadowmask and Indirect", "Direct, Indirect, Shadowmask (custom lighting only)"};
    static public string[] directContributionIndirectOptions = new string[] {"Direct And Indirect", "Indirect Only", "Shadowmask and Indirect (not applicable in Indirect mode)", "Direct, Indirect, Shadowmask (not applicable in Indirect mode)"};

    static string[] selStrings = new string[] {"0","1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16",
                                                "17","18","19","20","21","22","23","24","25","26","27","28","29","30"};//,"31"};

    void InitSerializedProperties(SerializedObject obj)
    {
        ftraceLightColor = obj.FindProperty("color");
        ftraceLightIntensity = obj.FindProperty("intensity");
        ftraceLightIndirectIntensity = obj.FindProperty("indirectIntensity");
        ftraceLightShadowSpread = obj.FindProperty("shadowSpread");
        ftraceLightSamples = obj.FindProperty("samples");
        ftraceLightBitmask = obj.FindProperty("bitmask");
        ftraceLightBakeToIndirect = obj.FindProperty("bakeToIndirect");
        ftraceLightShadowmask = obj.FindProperty("shadowmask");
        ftraceLightShadowmaskDenoise = obj.FindProperty("shadowmaskDenoise");
        ftraceLightTexture = obj.FindProperty("cloudShadow");
        ftraceLightCSTilingX = obj.FindProperty("cloudShadowTilingX");
        ftraceLightCSTilingY = obj.FindProperty("cloudShadowTilingY");
        ftraceLightCSOffsetX = obj.FindProperty("cloudShadowOffsetX");
        ftraceLightCSOffsetY = obj.FindProperty("cloudShadowOffsetY");
        ftraceLightSupersample = obj.FindProperty("supersample");

        isHDRP = (target as BakeryDirectLight).GetComponent("HDAdditionalLightData") != null;
    }

    void OnEnable()
    {
        InitSerializedProperties(serializedObject);
    }

    void SetHDRPLight(Light l)
    {
        l.intensity *= Mathf.PI;

        var hdrpLight = l.GetComponent("HDAdditionalLightData");
        if (hdrpLight == null)
        {
            Debug.LogWarning("HDRP: no HDAdditionalLightData");
            return;
        }
        var so = new SerializedObject(hdrpLight);
        if (so == null)
        {
            Debug.LogWarning("HDRP: no SerializedObject");
            return;
        }

        SerializedProperty hdrpInt2 = so.FindProperty("m_Intensity");
        if (hdrpInt2 == null)
        {
            Debug.LogWarning("HDRP: no m_Intensity");
            return;
        }
        hdrpInt2.floatValue = l.intensity;

        so.ApplyModifiedProperties();
    }

    void GetLinearLightParameters(Light light, out float lightR, out float lightG, out float lightB, out float lightInt)
    {
        if (PlayerSettings.colorSpace != ColorSpace.Linear)
        {
            lightInt = light.intensity;
            lightR = light.color.r;
            lightG = light.color.g;
            lightB = light.color.b;
            return;
        }

        if (!GraphicsSettings.lightsUseLinearIntensity)
        {
            lightR = Mathf.Pow(light.color.r * light.intensity, 2.2f);
            lightG = Mathf.Pow(light.color.g * light.intensity, 2.2f);
            lightB = Mathf.Pow(light.color.b * light.intensity, 2.2f);
            lightInt = Mathf.Max(Mathf.Max(lightR, lightG), lightB);
            lightR /= lightInt;
            lightG /= lightInt;
            lightB /= lightInt;
        }
        else
        {
            lightInt = light.intensity;
            lightR = light.color.r;
            lightG = light.color.g;
            lightB = light.color.b;

            if (GraphicsSettings.lightsUseColorTemperature)
            {
#if UNITY_2019_3_OR_NEWER
                if (light.useColorTemperature)
#endif
                {
                    var temp = Mathf.CorrelatedColorTemperatureToRGB(light.colorTemperature).gamma;
                    lightR *= temp.r;
                    lightG *= temp.g;
                    lightB *= temp.b;
                }
            }
        }
    }

    protected virtual void OnSceneGUI()
    {
        if (projectionMode)
        {
            var obj = target as BakeryDirectLight;
            var tform = obj.transform;

            Vector3 normal = tform.forward;
            Vector3 binormal, tangent;
            if(Mathf.Abs(normal.x) > Mathf.Abs(normal.z))
            {
                binormal = new Vector3(-normal.y, normal.x, 0);
            }
            else
            {
                binormal = new Vector3(0, -normal.z, normal.y);
            }
            binormal = binormal.normalized;
            tangent = Vector3.Cross(binormal, normal);
            var rot = Quaternion.LookRotation(normal, -binormal);
            var pos = tangent * -obj.cloudShadowOffsetX/obj.cloudShadowTilingX + binormal * -obj.cloudShadowOffsetY/obj.cloudShadowTilingY;
            pos += tangent * 0.5f/obj.cloudShadowTilingX;
            pos += binormal * 0.5f/obj.cloudShadowTilingY;
            var ntangent = tangent;
            var nbinormal = binormal;

            var mtx = new Matrix4x4();
            tangent /= obj.cloudShadowTilingX;
            binormal /= obj.cloudShadowTilingY;
            float depth = 1000.0f;
            normal *= depth;
            //pos += normal * depth * 0.5f;
            mtx.SetColumn(0, new Vector4(tangent.x, tangent.y, tangent.z, 0));
            mtx.SetColumn(1, new Vector4(binormal.x, binormal.y, binormal.z, 0));
            mtx.SetColumn(2, new Vector4(normal.x, normal.y, normal.z, 0));
            mtx.SetColumn(3, new Vector4(pos.x, pos.y, pos.z, 1.0f));

            Handles.color = Color.red;
            Handles.matrix = mtx;// * Matrix4x4.Translate(Vector3.one * 0.5f);
            Handles.DrawWireCube(Vector3.zero, Vector3.one);
            Handles.matrix = Matrix4x4.identity;

            EditorGUI.BeginChangeCheck();
            var newPos = Handles.PositionHandle(pos, rot);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(obj, "Move directional light projection");
                
                newPos -= ntangent * 0.5f/obj.cloudShadowTilingX;
                newPos -= nbinormal * 0.5f/obj.cloudShadowTilingY;
                obj.cloudShadowOffsetX = Vector3.Dot(ntangent, newPos) * -obj.cloudShadowTilingX;
                obj.cloudShadowOffsetY = Vector3.Dot(nbinormal, newPos) * -obj.cloudShadowTilingY;
                Shader.SetGlobalVector("_BakeryProjectionTilingOffset", new Vector4(obj.cloudShadowTilingX, obj.cloudShadowTilingY, obj.cloudShadowOffsetX, obj.cloudShadowOffsetY));
                if (BakeryDirectLight.lightsChanged == 0) BakeryDirectLight.lightsChanged = 1;
            }

            EditorGUI.BeginChangeCheck();
            float size = HandleUtility.GetHandleSize(pos);
            var newScale = Handles.ScaleHandle(new Vector3(obj.cloudShadowTilingX, obj.cloudShadowTilingY, 0), pos - tform.up * size * 0.25f, rot, -size);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(obj, "Scale directional light projection");
                obj.cloudShadowTilingX = newScale.x;
                obj.cloudShadowTilingY = newScale.y;
                Shader.SetGlobalVector("_BakeryProjectionTilingOffset", new Vector4(obj.cloudShadowTilingX, obj.cloudShadowTilingY, obj.cloudShadowOffsetX, obj.cloudShadowOffsetY));
                if (BakeryDirectLight.lightsChanged == 0) BakeryDirectLight.lightsChanged = 1;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        //if (showFtrace)
        //{
            OnEnable();
            serializedObject.Update();

            TestPreviewRefreshProperty(ref texCached, ftraceLightTexture.objectReferenceValue);

            EditorGUILayout.PropertyField(ftraceLightColor, new GUIContent("Color", "Color of the light"));
            EditorGUILayout.PropertyField(ftraceLightIntensity, new GUIContent("Intensity", "Color multiplier (Lux / Pi)"));
            EditorGUILayout.PropertyField(ftraceLightShadowSpread, new GUIContent("Shadow spread", "Controls shadow blurriness from 0 to 1"));
            EditorGUILayout.PropertyField(ftraceLightSamples, new GUIContent("Shadow samples", "The amount of rays tested for this light. Rays are emitted from lightmap texel towards the light, distributed conically. Radius of the cone depends on Shadow Spread."));

            //ftraceLightBitmask.intValue = EditorGUILayout.MaskField(new GUIContent("Bitmask", "Lights only affect renderers with overlapping bits"), ftraceLightBitmask.intValue, selStrings);
            int prevVal = ftraceLightBitmask.intValue;
            int newVal = EditorGUILayout.MaskField(new GUIContent("Bitmask", "Lights only affect renderers with overlapping bits"), ftraceLightBitmask.intValue, selStrings);
            if (prevVal != newVal) ftraceLightBitmask.intValue = newVal;

            /*
            EditorGUILayout.PropertyField(ftraceLightBakeToIndirect, new GUIContent("Bake to indirect", "Add direct contribution from this light to indirect-only lightmaps"));
            if (ftraceLightBakeToIndirect.boolValue && ftraceLightShadowmask.boolValue) ftraceLightShadowmask.boolValue = false;

            EditorGUILayout.PropertyField(ftraceLightShadowmask, new GUIContent("Shadowmask", "Enable mixed lighting. Static shadows from this light will be baked, and real-time light will cast shadows from dynamic objects."));
            if (ftraceLightBakeToIndirect.boolValue && ftraceLightShadowmask.boolValue) ftraceLightBakeToIndirect.boolValue = false;
            */

            if (storage == null) storage = ftRenderLightmap.FindRenderSettingsStorage();
            var rmode = storage.renderSettingsUserRenderMode;
            if (rmode != (int)ftRenderLightmap.RenderMode.FullLighting)
            {
                BakeWhat contrib;
                if (ftraceLightShadowmask.boolValue)
                {
                    if (ftraceLightBakeToIndirect.boolValue)
                    {
                        contrib = BakeWhat.DirectIndirectShadowmask;
                    }
                    else
                    {
                        contrib = BakeWhat.IndirectAndShadowmask;
                    }
                }
                else if (ftraceLightBakeToIndirect.boolValue)
                {
                    contrib = BakeWhat.DirectAndIndirect;
                }
                else
                {
                    contrib = BakeWhat.IndirectOnly;
                }
                var prevContrib = contrib;

                if (rmode == (int)ftRenderLightmap.RenderMode.Indirect)
                {
                    contrib = (BakeWhat)EditorGUILayout.Popup("Baked contribution", (int)contrib, directContributionIndirectOptions);
                }
                else if (rmode == (int)ftRenderLightmap.RenderMode.Shadowmask)
                {
                    contrib = (BakeWhat)EditorGUILayout.Popup("Baked contribution", (int)contrib, directContributionOptions);
                }

                if (prevContrib != contrib)
                {
                    if (contrib == BakeWhat.IndirectOnly)
                    {
                        ftraceLightShadowmask.boolValue = false;
                        ftraceLightBakeToIndirect.boolValue = false;
                    }
                    else if (contrib == BakeWhat.IndirectAndShadowmask)
                    {
                        ftraceLightShadowmask.boolValue = true;
                        ftraceLightBakeToIndirect.boolValue = false;
                    }
                    else if (contrib == BakeWhat.DirectIndirectShadowmask)
                    {
                        ftraceLightShadowmask.boolValue = true;
                        ftraceLightBakeToIndirect.boolValue = true;
                    }
                    else
                    {
                        ftraceLightShadowmask.boolValue = false;
                        ftraceLightBakeToIndirect.boolValue = true;
                    }
                }

                if (ftraceLightShadowmask.boolValue)
                {
                    EditorGUILayout.PropertyField(ftraceLightShadowmaskDenoise, new GUIContent("Denoise shadowmask", "Apply denoising to shadowmask texture. For sharp shadows it may be unnecessary."));
                }
            }

            EditorGUILayout.PropertyField(ftraceLightIndirectIntensity, new GUIContent("Indirect intensity", "Non-physical GI multiplier for this light"));

            EditorGUILayout.PropertyField(ftraceLightSupersample, new GUIContent("Anti-alias", "Performs supersampling for the shadows, using 8 sub-samples."));

            EditorGUILayout.PropertyField(ftraceLightTexture, new GUIContent("Texture projection", "Tiled projected texture"));
            if (ftraceLightTexture.objectReferenceValue != null)
            {
                var obj = target as BakeryDirectLight;

                EditorGUILayout.PropertyField(ftraceLightCSTilingX, new GUIContent("Tiling U", "Cloud shadow U tiling"));
                EditorGUILayout.PropertyField(ftraceLightCSTilingY, new GUIContent("Tiling V", "Cloud shadow V tiling"));
                EditorGUILayout.PropertyField(ftraceLightCSOffsetX, new GUIContent("Offset U", "Cloud shadow U tiling"));
                EditorGUILayout.PropertyField(ftraceLightCSOffsetY, new GUIContent("Offset V", "Cloud shadow V tiling"));

                if (GUILayout.Button("Tweak projection in Scene View"))
                {
                    ftSceneView.ToggleProjMode();
                    projectionMode = ftSceneView.enabled;

                    UnityEditor.EditorWindow.GetWindow<SceneView>();
                    var lastView = SceneView.lastActiveSceneView;
                    if (lastView == null)
                    {
                        Debug.LogError("Can't get lastActiveSceneView");
                    }
                    else
                    {
                        var cam = lastView.camera;
                        if (cam == null)
                        {
                            Debug.LogError("Can't get sceneView camera");
                        }
                        else
                        {
                            var camTform = cam.transform;
                            var tform = obj.transform;
                        }
                    }
                }

                if (projectionMode)
                {
                    EditorGUILayout.Space();
                    if (GUILayout.Button("Render reference frame"))
                    {
                        float far = 1000.0f;
                        int width = 512;
                        int height = 512;

                        var g = new GameObject();
                        g.name = "TempCamera";
                        var cam = g.AddComponent<Camera>();
                        cam.aspect = obj.cloudShadowTilingY / obj.cloudShadowTilingX;
                        cam.farClipPlane = far;
                        cam.orthographic = true;
                        cam.orthographicSize = (1.0f/obj.cloudShadowTilingY) * 0.5f;
                        //cam.cullingMask = renderMapLayers;

                        var tform = obj.transform;
                        Vector3 normal = tform.forward;
                        Vector3 binormal, tangent;
                        if(Mathf.Abs(normal.x) > Mathf.Abs(normal.z))
                        {
                            binormal = new Vector3(-normal.y, normal.x, 0);
                        }
                        else
                        {
                            binormal = new Vector3(0, -normal.z, normal.y);
                        }
                        binormal = binormal.normalized;
                        tangent = Vector3.Cross(binormal, normal);
                        var rot = Quaternion.LookRotation(normal, -binormal);
                        var pos = tangent * -obj.cloudShadowOffsetX/obj.cloudShadowTilingX + binormal * -obj.cloudShadowOffsetY/obj.cloudShadowTilingY;
                        pos += tangent * 0.5f/obj.cloudShadowTilingX;
                        pos += binormal * 0.5f/obj.cloudShadowTilingY;

                        cam.transform.position = pos - normal * far*0.5f;
                        cam.transform.rotation = rot;

                        var rt = new RenderTexture(width, height, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
                        cam.targetTexture = rt;
                        cam.enabled = false;
#if UNITY_2017_1_OR_NEWER
                        cam.cameraType = CameraType.Reflection; // trick for SRP
#endif
                        cam.Render();

                        var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
                        Graphics.SetRenderTarget(rt);
                        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
                        tex.Apply();

                        int index, indexA, indexB;
                        int minSwapped;
                        Color a, b;
                        var pixels = tex.GetPixels();
                        for(int y=0; y<height; y++)
                        {
                            index = y*width;
                            minSwapped = 16384;
                            for(int x=0; x<width; x++)
                            {
                                indexA = index+x;
                                if (minSwapped == indexA) break;
                                indexB = index+(width-1)-x;
                                a = pixels[indexA];
                                b = pixels[indexB];
                                pixels[indexA] = b;
                                pixels[indexB] = a;
                                minSwapped = indexB;
                            }
                        }
                        tex.SetPixels(pixels);

                        RenderTexture.active = null;
                        rt.Release();

                        byte[] _bytes = tex.EncodeToPNG();
                        var path = EditorUtility.SaveFilePanelInProject("Save texture", "", "png", "");
                        if (path.Length != 0)
                        {
                            System.IO.File.WriteAllBytes(path, _bytes);
                        }

                        DestroyImmediate(g);

                        AssetDatabase.Refresh();
                    }

                    Shader.SetGlobalTexture("_BakeryProjectionMap", ftraceLightTexture.objectReferenceValue as Texture2D);
                    Shader.SetGlobalVector("_BakeryProjectionDir", obj.gameObject.transform.forward);
                    Shader.SetGlobalVector("_BakeryProjectionTilingOffset", new Vector4(obj.cloudShadowTilingX, obj.cloudShadowTilingY, obj.cloudShadowOffsetX, obj.cloudShadowOffsetY));
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            serializedObject.ApplyModifiedProperties();
        //}


        bool showError = false;
        string why = "";

        bool shadowmaskNoDynamicLight = false;

        foreach(BakeryDirectLight selectedLight in targets)
        {
            bool match = true;

            var light = selectedLight.GetComponent<Light>();
            if (light == null)
            {
                if (ftraceLightShadowmask.boolValue) shadowmaskNoDynamicLight = true;
                continue;
            }
            if (!light.enabled)
            {
                if (ftraceLightShadowmask.boolValue) shadowmaskNoDynamicLight = true;
            }
            var so = new SerializedObject(selectedLight);
            InitSerializedProperties(so);

            if (light.type != LightType.Directional)
            {
                match = false;
                why = "real-time light is not direct";
            }

            if (light.bounceIntensity != ftraceLightIndirectIntensity.floatValue)
            {
                match = false;
                why = "indirect intensity doesn't match";
            }

            var clr = ftraceLightColor.colorValue;
            float eps = 1.0f / 255.0f;
            float lightR, lightG, lightB, lightInt;
            float fr, fg, fb;
            float fintensity = ftraceLightIntensity.floatValue;
            if (isHDRP) fintensity *= Mathf.PI;
            if (PlayerSettings.colorSpace == ColorSpace.Linear)
            {
                fr = clr.r;// * fintensity;
                fg = clr.g;// * fintensity;
                fb = clr.b;// * fintensity;
            }
            else
            {
                fr = clr.r;
                fg = clr.g;
                fb = clr.b;
            }
            GetLinearLightParameters(light, out lightR, out lightG, out lightB, out lightInt);

            if (GraphicsSettings.lightsUseLinearIntensity || PlayerSettings.colorSpace != ColorSpace.Linear)
            {
                if (Mathf.Abs(lightR - fr) > eps || Mathf.Abs(lightG - fg) > eps || Mathf.Abs(lightB - fb) > eps)
                {
                    match = false;
                    why = "color doesn't match";
                }
                else if (Mathf.Abs(lightInt - fintensity) > eps)
                {
                    match = false;
                    why = "intensity doesn't match";
                }
            }
            else
            {
                eps *= Mathf.Max(lightInt, fintensity);
                if (Mathf.Abs(lightR*lightInt - fr*fintensity) > eps ||
                    Mathf.Abs(lightG*lightInt - fg*fintensity) > eps ||
                    Mathf.Abs(lightB*lightInt - fb*fintensity) > eps)
                {
                    match = false;
                    why = "intensity doesn't match";
                }
            }

            if (!match)
            {
                showError = true;
            }
        }

        if (shadowmaskNoDynamicLight)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Warning: shadowmask needs enabled real-time light to work");
        }

        if (showError)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Real-time light doesn't match lightmap: " + why);
            if (GUILayout.Button("Match lightmapped to real-time"))
            {
                foreach(BakeryDirectLight selectedLight in targets)
                {
                    var light = selectedLight.GetComponent<Light>();
                    if (light == null) continue;
                    //if (!light.enabled) continue;
                    var so = new SerializedObject(selectedLight);
                    InitSerializedProperties(so);

                    float lightR, lightG, lightB, lightInt;
                    GetLinearLightParameters(light, out lightR, out lightG, out lightB, out lightInt);
                    ftraceLightColor.colorValue = new Color(lightR, lightG, lightB);
                    ftraceLightIntensity.floatValue = lightInt;

                    ftraceLightIndirectIntensity.floatValue = light.bounceIntensity;

                    if (isHDRP) ftraceLightIntensity.floatValue /= Mathf.PI;

                    so.ApplyModifiedProperties();
                }
            }
            if (GUILayout.Button("Match real-time to lightmapped"))
            {
                foreach(BakeryDirectLight selectedLight in targets)
                {
                    var light = selectedLight.GetComponent<Light>();
                    if (light == null) continue;
                    //if (!light.enabled) continue;
                    var so = new SerializedObject(selectedLight);
                    InitSerializedProperties(so);

                    Undo.RecordObject(light, "Change light");
                    if (PlayerSettings.colorSpace != ColorSpace.Linear)
                    {
                        light.color = ftraceLightColor.colorValue;
                        light.intensity = ftraceLightIntensity.floatValue;
                    }
                    else if (!GraphicsSettings.lightsUseLinearIntensity)
                    {
                        float fr, fg, fb;
                        float fintensity = ftraceLightIntensity.floatValue;
                        var clr = ftraceLightColor.colorValue;
                        fr = clr.linear.r;// * fintensity;
                        fg = clr.linear.g;// * fintensity;
                        fb = clr.linear.b;// * fintensity;

                        fr = Mathf.Pow(fr * fintensity, 1.0f / 2.2f);
                        fg = Mathf.Pow(fg * fintensity, 1.0f / 2.2f);
                        fb = Mathf.Pow(fb * fintensity, 1.0f / 2.2f);
                        float fint = Mathf.Max(Mathf.Max(fr, fg), fb);
                        fr /= fint;
                        fg /= fint;
                        fb /= fint;
                        light.color = new Color(fr, fg, fb);
                        light.intensity = fint;
                    }
                    else
                    {
                        light.color = ftraceLightColor.colorValue;
                        light.intensity = ftraceLightIntensity.floatValue;
                    }
                    light.colorTemperature = 6570; // neutral in Unity
                    light.type = LightType.Directional;
                    light.bounceIntensity = ftraceLightIndirectIntensity.floatValue;
                    if (isHDRP) SetHDRPLight(light);
                }
            }
        }


        if (PlayerSettings.colorSpace == ColorSpace.Linear)
        {
            if (!GraphicsSettings.lightsUseLinearIntensity)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Warning: project is not set up to use linear light intensity.");
                EditorGUILayout.LabelField("GraphicsSettings.lightsUseLinearIntensity should be TRUE.");
                if (GUILayout.Button("Fix"))
                {
                    GraphicsSettings.lightsUseLinearIntensity = true;
                }
            }
            else
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Project is using linear light intensity. This is nice.");
                if (GUILayout.Button("Change to non-linear"))
                {
                    GraphicsSettings.lightsUseLinearIntensity = false;
                }
            }
        }
    }
}



