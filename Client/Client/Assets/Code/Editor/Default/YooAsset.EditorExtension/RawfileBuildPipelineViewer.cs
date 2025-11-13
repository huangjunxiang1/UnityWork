using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using YooAsset.Editor;
using TMPro;

class Ex
{
    static void cull_res(bool deleteVs, string packageName, BuildTarget buildTarget)
    {
        var dir = $"{Application.dataPath}/../Bundles/{buildTarget}/{packageName}";
        var ds = Directory.GetDirectories(dir).Select(t => new DirectoryInfo(t)).Where(t => t.Name.Contains('.')).ToList();
        ds.Sort((x, y) =>
        {
            Version v1 = null;
            Version v2 = null;
            try { v1 = new(x.Name); }
            catch (Exception) { }
            try { v2 = new(y.Name); }
            catch (Exception) { }
            if (v1 < v2) return -1;
            if (v1 > v2) return 1;
            return 0;
        });
        var src = ds.LastOrDefault();
        if (src == null)
            return;
        var cull = src.GetFiles().Select(t => t.Name);
        if (deleteVs)
            File.Delete($"{src}/{packageName}.version");
        HashSet<string> olds = new(100000);
        for (int i = 0; i < ds.Count - 1; i++)
        {
            var target = ds[i];
            foreach (var item in target.GetFiles())
                olds.Add(item.Name);
        }
        foreach (var item in src.GetFiles())
        {
            if (item.Name.EndsWith(".version"))
                continue;
            if (item.Name.EndsWith(".json") || item.Name.EndsWith(".xml") || item.Name.EndsWith(".report"))
                item.Delete();
            if (olds.Contains(item.Name))
                item.Delete();
        }
    }

    // 新增：提取的复用 UI 创建函数（Toggle + Button）
    static void AddCullControls(VisualElement root, string packageName, BuildTarget buildTarget, Func<Task> refreshPending = null)
    {
        // Toggle：是否删除 PackageManifest 文件（默认从 EditorPrefs 读取）
        string prefKey = $"YooAsset.Cull.vs";
        var deleteToggle = new Toggle($"删除 x.version");
        deleteToggle.value = EditorPrefs.GetBool(prefKey, false);
        deleteToggle.style.marginTop = 6;
        deleteToggle.RegisterValueChangedCallback(evt =>
        {
            EditorPrefs.SetBool(prefKey, evt.newValue);
        });
        root.Add(deleteToggle);

        Button myButton = new Button();
        myButton.style.marginTop = 10;
        myButton.text = "剔除重复资源";
        myButton.style.height = 50;
        myButton.AddToClassList("my-button-style");
        myButton.clicked += () =>
        {
            cull_res(deleteToggle.value, packageName, buildTarget);
            // 剔除完成后触发刷新（异步）
            if (refreshPending != null)
                Task.Run(refreshPending);
        };
        root.Add(myButton);
    }

    [BuildPipelineAttribute(nameof(EBuildPipeline.RawFileBuildPipeline))]
    internal class Raw : RawfileBuildpipelineViewer
    {
        protected override string GetDefaultPackageVersion() => getVer(this.BuildTarget, this.PackageName);
        public override void CreateView(VisualElement parent)
        {
            base.CreateView(parent);

            // 使用提取后的复用函数，并传入刷新回调
            Ex.AddCullControls(Root, this.PackageName, this.BuildTarget, () => Ex.PopulatePendingForContainerByRoot(Root, this.BuildTarget, this.PackageName));

            // 添加 FTP 上传面板
            Ex.AddFtpUploader(Root, this.BuildTarget, this.PackageName);
        }
        protected override void ExecuteBuild()
        {
            base.ExecuteBuild();
            // 构建完成后异步刷新未传输列表显示
            Task.Run(() => Ex.PopulatePendingForContainerByRoot(Root, this.BuildTarget, this.PackageName));
        }
    }
    [BuildPipelineAttribute(nameof(EBuildPipeline.ScriptableBuildPipeline))]
    internal class Res : ScriptableBuildPipelineViewer
    {
        protected override string GetDefaultPackageVersion() => getVer(this.BuildTarget, this.PackageName);
        public override void CreateView(VisualElement parent)
        {
            base.CreateView(parent);

            // 使用提取后的复用函数，并传入刷新回调
            Ex.AddCullControls(Root, this.PackageName, this.BuildTarget, () => Ex.PopulatePendingForContainerByRoot(Root, this.BuildTarget, this.PackageName));

            Ex.AddFtpUploader(Root, this.BuildTarget, this.PackageName);
        }
        protected override void ExecuteBuild()
        {
            // 需要引入 TMPro 命名空间: using TMPro;
            string[] guids = AssetDatabase.FindAssets("t:TMP_FontAsset");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<TMPro.TMP_FontAsset>(path);
                if (asset.atlasPopulationMode != AtlasPopulationMode.Static)
                    asset.ClearFontAssetData(true);
            }
            AssetDatabase.Refresh();
            base.ExecuteBuild();

            // 构建完成后异步刷新未传输列表显示
            Task.Run(() => Ex.PopulatePendingForContainerByRoot(Root, this.BuildTarget, this.PackageName));
        }
    }

    static string getVer(BuildTarget BuildTarget, string PackageName)
    {
        var output = $"{AssetBundleBuilderHelper.GetDefaultBuildOutputRoot()}/{BuildTarget}/{PackageName}";
        var min = new Version("1.0.0");
        if (!Directory.Exists(output))
            return min.ToString();
        var dirs = Directory.GetDirectories(output);
        var max = min;
        for (int i = 0; i < dirs.Length; i++)
        {
            try
            {
                var v = new Version(new DirectoryInfo(dirs[i]).Name);
                if (max < v)
                    max = v;
            }
            catch (Exception)
            {
                continue;
            }
        }
        return $"{max.Major}.{max.Minor}.{max.Build + 1}";
    }

    // ---------- FTP 上传相关辅助方法与 UI 组件 ----------

    // 辅助：格式化字节（类级复用）
    static string FormatBytes(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        double kb = bytes / 1024.0;
        if (kb < 1024) return $"{kb:F2} KB";
        double mb = kb / 1024.0;
        if (mb < 1024) return $"{mb:F2} MB";
        double gb = mb / 1024.0;
        return $"{gb:F2} GB";
    }

    // 新增：刷新 pending 列表的通用函数（将 UI 更新逻辑封装为独立函数）
    static void RefreshPendingUI(VisualElement container, List<string> relativePaths, Dictionary<string, long> sizeMap, Dictionary<string, Label> pendingLabelMap = null, int uploadedCount = 0)
    {
        if (container == null) return;
        var pendingScroll = container.Q<ScrollView>("pendingScroll");
        var pendingTitle = container.Q<Label>("pendingTitle");
        var uploadedTitle = container.Q<Label>("uploadedTitle");
        var statusLabel = container.Q<Label>("statusLabel");
        if (pendingScroll == null || pendingTitle == null || uploadedTitle == null || statusLabel == null)
            return;

        pendingScroll.contentContainer.Clear();
        if (pendingLabelMap != null)
            pendingLabelMap.Clear();

        foreach (var rel in relativePaths)
        {
            var sizeText = sizeMap != null && sizeMap.TryGetValue(rel, out var s) ? FormatBytes(s) : "0 B";
            var lbl = new Label($"{rel} ({sizeText})");
            lbl.style.unityFontStyleAndWeight = FontStyle.Normal;
            lbl.style.whiteSpace = WhiteSpace.Normal;
            lbl.style.unityTextAlign = TextAnchor.MiddleLeft;
            lbl.style.marginBottom = 2;
            lbl.name = rel;
            pendingScroll.contentContainer.Add(lbl);
            if (pendingLabelMap != null)
                pendingLabelMap[rel] = lbl;
        }

        pendingTitle.text = $"未传输 ({relativePaths.Count})";
        uploadedTitle.text = $"已传输 ({uploadedCount})";

        if (relativePaths.Count == 0)
        {
            statusLabel.text = "目录为空";
        }
        else
        {
            statusLabel.text = $"找到 {relativePaths.Count} 个文件，准备上传";
        }
    }

    // 新版：FTP 上传功能整体重构（优化 UI 布局与交互）
    static void AddFtpUploader(VisualElement root, BuildTarget buildTarget, string packageName)
    {
        /*
        计划已在外部说明，这里直接实现：
        - 增加失败列表（failedScroll / failedTitle）和“重传失败”按钮 retryFailedBtn
        - 在每个文件上传失败时记录到 failedList 并在 UI 中展示
        - retryFailedBtn 点击后对 failedList 执行重传（仅对这些文件）
        - 为失败列表增加可拖动的下拉分隔条，用于调节失败列表高度
        */

        // 容器
        var container = new VisualElement();
        container.style.marginTop = 10;
        container.style.flexDirection = FlexDirection.Column;
        container.style.paddingLeft = 4;
        container.style.paddingRight = 4;
        container.name = $"YooAsset.Ftp.{buildTarget}.{packageName}";

        // 标题
        var title = new Label("FTP 上传");
        title.style.unityFontStyleAndWeight = FontStyle.Bold;
        title.style.marginTop = 6;
        container.Add(title);

        // EditorPrefs 键前缀（确保唯一）
        string keyPrefix = $"YooAsset.Ftp";
        string keyHost = keyPrefix + ".Host";
        string keyUser = keyPrefix + ".User";
        string keyPass = keyPrefix + ".Pass";
        string keyRemoteRoot = keyPrefix + $".{packageName}.RemoteRoot";
        string keyIncludePlatform = keyPrefix + $".{packageName}.IncludePlatform"; // 是否在远程路径中区分平台

        // 输入：FTP 主机
        var ftpHostField = new TextField("FTP 主机（例如 ftp://example.com 或 example.com）");
        ftpHostField.value = EditorPrefs.GetString(keyHost, "");
        container.Add(ftpHostField);

        // 输入：用户名
        var userField = new TextField("用户名");
        userField.value = EditorPrefs.GetString(keyUser, "");
        container.Add(userField);

        // 输入：密码（密码域）
        var passField = new TextField("密码");
        passField.value = EditorPrefs.GetString(keyPass, "");
        container.Add(passField);

        // 输入：远程根路径
        var remotePathField = new TextField("远程根路径（可选）");
        remotePathField.value = EditorPrefs.GetString(keyRemoteRoot, "");
        container.Add(remotePathField);

        // Toggle 控件：是否在路径中包含平台目录
        var includePlatformToggle = new Toggle("区分平台目录（上传时包含平台名）");
        includePlatformToggle.value = EditorPrefs.GetBool(keyIncludePlatform, false);
        includePlatformToggle.style.marginTop = 6;
        includePlatformToggle.RegisterValueChangedCallback(evt =>
        {
            EditorPrefs.SetBool(keyIncludePlatform, evt.newValue);
        });
        container.Add(includePlatformToggle);

        // 当任意字段变更时保存到 EditorPrefs（本地缓存）
        ftpHostField.RegisterValueChangedCallback(evt =>
        {
            EditorPrefs.SetString(keyHost, evt.newValue ?? "");
        });
        userField.RegisterValueChangedCallback(evt =>
        {
            EditorPrefs.SetString(keyUser, evt.newValue ?? "");
        });
        passField.RegisterValueChangedCallback(evt =>
        {
            EditorPrefs.SetString(keyPass, evt.newValue ?? "");
        });
        remotePathField.RegisterValueChangedCallback(evt =>
        {
            EditorPrefs.SetString(keyRemoteRoot, evt.newValue ?? "");
        });

        // 进度条与状态
        var progressBar = new ProgressBar();
        progressBar.lowValue = 0f;
        progressBar.highValue = 1f;
        progressBar.value = 0f;
        progressBar.style.height = 18;
        progressBar.style.marginTop = 6;
        container.Add(progressBar);

        var statusLabel = new Label("就绪");
        statusLabel.style.marginTop = 4;
        statusLabel.name = "statusLabel";
        container.Add(statusLabel);

        // 上传按钮
        var uploadBtn = new Button();
        uploadBtn.text = "上传到 FTP";
        uploadBtn.style.marginTop = 6;
        uploadBtn.style.height = 30;
        container.Add(uploadBtn);

        // 重传失败按钮（新）
        var retryFailedBtn = new Button();
        retryFailedBtn.text = "重传失败";
        retryFailedBtn.style.marginTop = 6;
        retryFailedBtn.style.height = 30;
        container.Add(retryFailedBtn);

        // 滚动列表区域（左右并排显示 未传输 / 已传输），并支持可拖动调整宽度
        var listsContainer = new VisualElement();
        listsContainer.style.marginTop = 8;
        listsContainer.style.flexDirection = FlexDirection.Row;
        listsContainer.style.alignItems = Align.Stretch;
        listsContainer.style.justifyContent = Justify.FlexStart;
        listsContainer.style.height = 200; // 初始高度，可被分割条拖动改变

        // 左侧：未传输
        var pendingColumn = new VisualElement();
        pendingColumn.style.flexDirection = FlexDirection.Column;
        pendingColumn.style.flexGrow = 0; // 使用固定宽度，支持拖动
        pendingColumn.style.width = new Length(50, LengthUnit.Percent);
        pendingColumn.style.marginRight = 0;
        pendingColumn.style.unityTextAlign = TextAnchor.UpperLeft;

        var pendingTitle = new Label("未传输 (0)");
        pendingTitle.style.unityFontStyleAndWeight = FontStyle.Bold;
        pendingTitle.style.marginBottom = 4;
        pendingTitle.name = "pendingTitle";
        pendingColumn.Add(pendingTitle);

        // pending 列内部结构：top (可为0) + scroll (列表) + divider（放在列表下方）
        var pendingSplit = new VisualElement();
        pendingSplit.style.flexDirection = FlexDirection.Column;
        pendingSplit.style.flexGrow = 1;
        pendingSplit.style.alignItems = Align.Stretch;
        pendingSplit.style.justifyContent = Justify.FlexStart;

        var pendingTop = new VisualElement();
        pendingTop.style.height = new Length(0, LengthUnit.Pixel);
        pendingTop.style.marginBottom = 0;
        pendingTop.style.paddingTop = 0;
        pendingTop.style.backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 0f));
        pendingSplit.Add(pendingTop);

        var pendingScroll = new ScrollView();
        pendingScroll.style.flexGrow = 1;
        pendingScroll.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        pendingScroll.verticalScrollerVisibility = ScrollerVisibility.Auto;
        pendingScroll.name = "pendingScroll";
        pendingScroll.contentContainer.style.paddingTop = 0;
        pendingScroll.contentContainer.style.marginTop = 0;
        pendingSplit.Add(pendingScroll);

        var pendingHDivider = new VisualElement();
        pendingHDivider.style.height = 6;
        pendingHDivider.pickingMode = PickingMode.Position;
        pendingHDivider.style.backgroundColor = new StyleColor(new Color(0.85f, 0.85f, 0.85f));
        pendingHDivider.style.marginTop = 4;
        pendingHDivider.style.marginBottom = 4;
        pendingHDivider.style.alignSelf = Align.Stretch;
        pendingSplit.Add(pendingHDivider);

        pendingColumn.Add(pendingSplit);

        // 分隔条（可拖拽） - 左右
        var divider = new VisualElement();
        divider.style.width = 6;
        divider.pickingMode = PickingMode.Position;
        divider.style.backgroundColor = new StyleColor(new Color(0.85f, 0.85f, 0.85f));
        divider.style.marginLeft = 4;
        divider.style.marginRight = 4;
        divider.style.height = new Length(100, LengthUnit.Percent);
        divider.style.alignSelf = Align.Stretch;

        // 右侧：已传输
        var uploadedColumn = new VisualElement();
        uploadedColumn.style.flexDirection = FlexDirection.Column;
        uploadedColumn.style.flexGrow = 1; // 右侧占剩余空间
        uploadedColumn.style.marginLeft = 0;
        uploadedColumn.style.unityTextAlign = TextAnchor.UpperLeft;

        var uploadedTitle = new Label("已传输 (0)");
        uploadedTitle.style.unityFontStyleAndWeight = FontStyle.Bold;
        uploadedTitle.style.marginBottom = 4;
        uploadedTitle.name = "uploadedTitle";
        uploadedColumn.Add(uploadedTitle);

        // uploaded 列内部：top (可为0) + scroll + divider（divider 放在 scroll 之后）
        var uploadedSplit = new VisualElement();
        uploadedSplit.style.flexDirection = FlexDirection.Column;
        uploadedSplit.style.flexGrow = 1;
        uploadedSplit.style.alignItems = Align.Stretch;
        uploadedSplit.style.justifyContent = Justify.FlexStart;

        var uploadedTop = new VisualElement();
        uploadedTop.style.height = new Length(0, LengthUnit.Pixel);
        uploadedTop.style.marginBottom = 0;
        uploadedTop.style.paddingTop = 0;
        uploadedTop.style.backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 0f));
        uploadedSplit.Add(uploadedTop);

        var uploadedScroll = new ScrollView();
        uploadedScroll.style.flexGrow = 1;
        uploadedScroll.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        uploadedScroll.verticalScrollerVisibility = ScrollerVisibility.Auto;
        uploadedScroll.name = "uploadedScroll";
        uploadedScroll.contentContainer.style.paddingTop = 0;
        uploadedScroll.contentContainer.style.marginTop = 0;
        uploadedSplit.Add(uploadedScroll);

        var uploadedHDivider = new VisualElement();
        uploadedHDivider.style.height = 6;
        uploadedHDivider.pickingMode = PickingMode.Position;
        uploadedHDivider.style.backgroundColor = new StyleColor(new Color(0.85f, 0.85f, 0.85f));
        uploadedHDivider.style.marginTop = 4;
        uploadedHDivider.style.marginBottom = 4;
        uploadedHDivider.style.alignSelf = Align.Stretch;
        uploadedSplit.Add(uploadedHDivider);

        uploadedColumn.Add(uploadedSplit);

        listsContainer.Add(pendingColumn);
        listsContainer.Add(divider);
        listsContainer.Add(uploadedColumn);
        container.Add(listsContainer);

        // 下面新增失败列表区域（单列，放在 listsContainer 之下）
        var failedContainer = new VisualElement();
        failedContainer.style.flexDirection = FlexDirection.Column;
        failedContainer.style.marginTop = 6;

        var failedTitle = new Label("失败 (0)");
        failedTitle.style.unityFontStyleAndWeight = FontStyle.Bold;
        failedTitle.name = "failedTitle";
        failedTitle.style.marginBottom = 4;
        failedContainer.Add(failedTitle);

        // 将失败列表分割条移至列表下方：使用 failedSplit 包含 top + scroll + divider（divider 在下方）
        var failedSplit = new VisualElement();
        failedSplit.style.flexDirection = FlexDirection.Column;
        failedSplit.style.flexGrow = 1;
        failedSplit.style.alignItems = Align.Stretch;
        failedSplit.style.justifyContent = Justify.FlexStart;

        var failedTop = new VisualElement();
        failedTop.style.height = new Length(0, LengthUnit.Pixel);
        failedTop.style.marginBottom = 0;
        failedTop.style.paddingTop = 0;
        failedTop.style.backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 0f));
        failedSplit.Add(failedTop);

        var failedScroll = new ScrollView();
        failedScroll.style.height = 80;
        failedScroll.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        failedScroll.verticalScrollerVisibility = ScrollerVisibility.Auto;
        failedScroll.name = "failedScroll";
        failedScroll.contentContainer.style.paddingTop = 0;
        failedSplit.Add(failedScroll);

        // 失败列表的分割条（现在在列表下方）
        var failedHDivider = new VisualElement();
        failedHDivider.style.height = 6;
        failedHDivider.pickingMode = PickingMode.Position;
        failedHDivider.style.backgroundColor = new StyleColor(new Color(0.85f, 0.85f, 0.85f));
        failedHDivider.style.marginTop = 2;
        failedHDivider.style.marginBottom = 4;
        failedHDivider.style.alignSelf = Align.Stretch;
        failedSplit.Add(failedHDivider);

        failedContainer.Add(failedSplit);

        container.Add(failedContainer);

        root.Add(container);

        // 映射与计数（提前声明以便被初始化逻辑复用）
        var pendingLabelMap = new Dictionary<string, Label>(StringComparer.OrdinalIgnoreCase);
        var failedLabelMap = new Dictionary<string, Label>(StringComparer.OrdinalIgnoreCase);
        var uploadedCount = 0;
        var pendingCount = 0;
        var failedCount = 0;
        var failedList = new List<string>();

        // 可拖动逻辑 - 左右分隔（保持原实现）
        {
            bool isDragging = false;
            int draggingPointerId = -1;
            float startPointerX = 0f;
            float startLeftWidth = 0f;
            const float dividerWidth = 6f;
            const float minColumnWidth = 80f;

            divider.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button != (int)MouseButton.LeftMouse) return;
                isDragging = true;
                draggingPointerId = evt.pointerId;
                startPointerX = evt.position.x;
                startLeftWidth = pendingColumn.layout.width > 0 ? pendingColumn.layout.width : pendingColumn.resolvedStyle.width;
                try { divider.CapturePointer(draggingPointerId); } catch { }
                evt.StopImmediatePropagation();
            });

            divider.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (!isDragging) return;
                if (evt.pointerId != draggingPointerId) return;

                float totalWidth = listsContainer.layout.width;
                if (totalWidth <= 0) return;

                float delta = evt.position.x - startPointerX;
                float newLeft = startLeftWidth + delta;
                float maxLeft = Math.Max(minColumnWidth, totalWidth - minColumnWidth - dividerWidth);
                newLeft = Mathf.Clamp(newLeft, minColumnWidth, maxLeft);

                pendingColumn.style.width = newLeft;
                float rightWidth = Math.Max(minColumnWidth, totalWidth - newLeft - dividerWidth - 8);
                uploadedColumn.style.flexGrow = 0;
                uploadedColumn.style.width = rightWidth;

                evt.StopImmediatePropagation();
            });

            divider.RegisterCallback<PointerUpEvent>(evt =>
            {
                if (!isDragging) return;
                if (evt.pointerId != draggingPointerId) return;
                isDragging = false;
                try { divider.ReleasePointer(draggingPointerId); } catch { }
                draggingPointerId = -1;
                evt.StopImmediatePropagation();
            });

            divider.RegisterCallback<PointerCancelEvent>(evt =>
            {
                if (!isDragging) return;
                isDragging = false;
                try { if (draggingPointerId != -1) divider.ReleasePointer(draggingPointerId); } catch { }
                draggingPointerId = -1;
                evt.StopImmediatePropagation();
            });
        }

        // 可拖动逻辑 - pending 列的分割条（调整 listsContainer 高度）
        {
            bool isDragging = false;
            int draggingPointerId = -1;
            float startPointerYLocal = 0f;
            float startListsHeight = 0f;
            const float minListsHeight = 80f;
            const float maxListsHeight = 800f;

            pendingHDivider.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button != (int)MouseButton.LeftMouse) return;
                isDragging = true;
                draggingPointerId = evt.pointerId;
                var local = listsContainer.WorldToLocal(evt.position);
                startPointerYLocal = local.y;
                startListsHeight = listsContainer.layout.height > 0 ? listsContainer.layout.height : listsContainer.resolvedStyle.height;
                try { pendingHDivider.CapturePointer(draggingPointerId); } catch { }
                evt.StopImmediatePropagation();
            });

            pendingHDivider.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (!isDragging) return;
                if (evt.pointerId != draggingPointerId) return;

                var local = listsContainer.WorldToLocal(evt.position);
                float delta = local.y - startPointerYLocal;
                float newHeight = startListsHeight + delta;
                newHeight = Mathf.Clamp(newHeight, minListsHeight, maxListsHeight);

                listsContainer.style.height = new Length(newHeight, LengthUnit.Pixel);

                evt.StopImmediatePropagation();
            });

            pendingHDivider.RegisterCallback<PointerUpEvent>(evt =>
            {
                if (!isDragging) return;
                if (evt.pointerId != draggingPointerId) return;
                isDragging = false;
                try { pendingHDivider.ReleasePointer(draggingPointerId); } catch { }
                draggingPointerId = -1;
                evt.StopImmediatePropagation();
            });

            pendingHDivider.RegisterCallback<PointerCancelEvent>(evt =>
            {
                if (!isDragging) return;
                isDragging = false;
                try { if (draggingPointerId != -1) pendingHDivider.ReleasePointer(draggingPointerId); } catch { }
                draggingPointerId = -1;
                evt.StopImmediatePropagation();
            });
        }

        // 可拖动逻辑 - uploaded 列的分隔条（同样调整 listsContainer 高度）
        {
            bool isDragging = false;
            int draggingPointerId = -1;
            float startPointerYLocal = 0f;
            float startListsHeight = 0f;
            const float minListsHeight = 80f;
            const float maxListsHeight = 800f;

            uploadedHDivider.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button != (int)MouseButton.LeftMouse) return;
                isDragging = true;
                draggingPointerId = evt.pointerId;
                var local = listsContainer.WorldToLocal(evt.position);
                startPointerYLocal = local.y;
                startListsHeight = listsContainer.layout.height > 0 ? listsContainer.layout.height : listsContainer.resolvedStyle.height;
                try { uploadedHDivider.CapturePointer(draggingPointerId); } catch { }
                evt.StopImmediatePropagation();
            });

            uploadedHDivider.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (!isDragging) return;
                if (evt.pointerId != draggingPointerId) return;

                var local = listsContainer.WorldToLocal(evt.position);
                float delta = local.y - startPointerYLocal;
                float newHeight = startListsHeight + delta;
                newHeight = Mathf.Clamp(newHeight, minListsHeight, maxListsHeight);

                listsContainer.style.height = new Length(newHeight, LengthUnit.Pixel);

                evt.StopImmediatePropagation();
            });

            uploadedHDivider.RegisterCallback<PointerUpEvent>(evt =>
            {
                if (!isDragging) return;
                if (evt.pointerId != draggingPointerId) return;
                isDragging = false;
                try { uploadedHDivider.ReleasePointer(draggingPointerId); } catch { }
                draggingPointerId = -1;
                evt.StopImmediatePropagation();
            });

            uploadedHDivider.RegisterCallback<PointerCancelEvent>(evt =>
            {
                if (!isDragging) return;
                isDragging = false;
                try { if (draggingPointerId != -1) uploadedHDivider.ReleasePointer(draggingPointerId); } catch { }
                draggingPointerId = -1;
                evt.StopImmediatePropagation();
            });
        }

        // 可拖动逻辑 - failed 列的上方分隔条（调整 failedScroll 高度）  —— 注意：分割条在列表下方，但拖动计算仍然基于 failedScroll 高度
        {
            bool isDragging = false;
            int draggingPointerId = -1;
            float startPointerYLocal = 0f;
            float startFailedHeight = 0f;
            const float minFailedHeight = 40f;
            const float maxFailedHeight = 600f;

            failedHDivider.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button != (int)MouseButton.LeftMouse) return;
                isDragging = true;
                draggingPointerId = evt.pointerId;
                var local = failedContainer.WorldToLocal(evt.position);
                startPointerYLocal = local.y;
                startFailedHeight = failedScroll.layout.height > 0 ? failedScroll.layout.height : failedScroll.resolvedStyle.height;
                try { failedHDivider.CapturePointer(draggingPointerId); } catch { }
                evt.StopImmediatePropagation();
            });

            failedHDivider.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (!isDragging) return;
                if (evt.pointerId != draggingPointerId) return;

                var local = failedContainer.WorldToLocal(evt.position);
                float delta = local.y - startPointerYLocal;
                float newHeight = startFailedHeight + delta;
                newHeight = Mathf.Clamp(newHeight, minFailedHeight, maxFailedHeight);

                failedScroll.style.height = new Length(newHeight, LengthUnit.Pixel);

                evt.StopImmediatePropagation();
            });

            failedHDivider.RegisterCallback<PointerUpEvent>(evt =>
            {
                if (!isDragging) return;
                if (evt.pointerId != draggingPointerId) return;
                isDragging = false;
                try { failedHDivider.ReleasePointer(draggingPointerId); } catch { }
                draggingPointerId = -1;
                evt.StopImmediatePropagation();
            });

            failedHDivider.RegisterCallback<PointerCancelEvent>(evt =>
            {
                if (!isDragging) return;
                isDragging = false;
                try { if (draggingPointerId != -1) failedHDivider.ReleasePointer(draggingPointerId); } catch { }
                draggingPointerId = -1;
                evt.StopImmediatePropagation();
            });
        }

        // 异步填充未传输列表（页面初始化时就显示 pending 列表）
        Task.Run(() => PopulatePendingForContainer(container, buildTarget, packageName));

        // upload 按钮逻辑（上传并记录失败）
        uploadBtn.clicked += () =>
        {
            EditorPrefs.SetString(keyHost, ftpHostField.value ?? "");
            EditorPrefs.SetString(keyUser, userField.value ?? "");
            EditorPrefs.SetString(keyPass, passField.value ?? "");
            EditorPrefs.SetString(keyRemoteRoot, remotePathField.value ?? "");
            EditorPrefs.SetBool(keyIncludePlatform, includePlatformToggle.value);

            bool includePlatform = includePlatformToggle.value;

            uploadBtn.SetEnabled(false);
            retryFailedBtn.SetEnabled(false);
            progressBar.value = 0f;
            statusLabel.text = "准备上传...";

            string host = ftpHostField.value?.Trim() ?? "";
            string user = userField.value ?? "";
            string pass = passField.value ?? "";
            string remoteRoot = remotePathField.value?.Trim() ?? "";

            pendingLabelMap.Clear();
            failedLabelMap.Clear();
            uploadedScroll.contentContainer.Clear();
            pendingScroll.contentContainer.Clear();
            failedScroll.contentContainer.Clear();
            uploadedCount = 0;
            pendingCount = 0;
            failedCount = 0;
            failedList.Clear();
            uploadedTitle.text = $"已传输 ({uploadedCount})";
            pendingTitle.text = $"未传输 ({pendingCount})";
            failedTitle.text = $"失败 ({failedCount})";

            Task.Run(async () =>
            {
                try
                {
                    string localRoot = GetLatestOutputDir(buildTarget, packageName);
                    if (string.IsNullOrEmpty(localRoot) || !Directory.Exists(localRoot))
                    {
                        EditorApplication.delayCall += () =>
                        {
                            EditorUtility.DisplayDialog("错误", "未找到最新构建输出目录，无法上传。", "确定");
                            statusLabel.text = "未找到本地目录";
                            uploadBtn.SetEnabled(true);
                            retryFailedBtn.SetEnabled(true);
                        };
                        return;
                    }

                    if (!host.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase) &&
                        !host.StartsWith("ftps://", StringComparison.OrdinalIgnoreCase))
                    {
                        host = "ftp://" + host;
                    }
                    host = host.TrimEnd('/');

                    var files = Directory.GetFiles(localRoot, "*", SearchOption.AllDirectories);
                    if (files.Length == 0)
                    {
                        EditorApplication.delayCall += () =>
                        {
                            EditorUtility.DisplayDialog("提示", "目录为空，无需上传。", "确定");
                            statusLabel.text = "目录为空";
                            uploadBtn.SetEnabled(true);
                            retryFailedBtn.SetEnabled(true);
                        };
                        return;
                    }

                    long totalBytes = 0;
                    var fileInfos = new List<FileInfo>(files.Length);
                    var relativePaths = new List<string>(files.Length);
                    var sizeMap = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                    var relativeToFull = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    for (int i = 0; i < files.Length; i++)
                    {
                        var fi = new FileInfo(files[i]);
                        fileInfos.Add(fi);
                        totalBytes += fi.Length;

                        var relative = files[i].Substring(localRoot.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        var relativeNormalized = relative.Replace('\\', '/');
                        relativePaths.Add(relativeNormalized);

                        sizeMap[relativeNormalized] = fi.Length;
                        relativeToFull[relativeNormalized] = files[i];
                    }

                    EditorApplication.delayCall += () =>
                    {
                        RefreshPendingUI(container, relativePaths, sizeMap, pendingLabelMap, uploadedCount);
                        if (relativePaths.Count == 0)
                        {
                            EditorUtility.DisplayDialog("提示", "目录为空，无需上传。", "确定");
                            statusLabel.text = "目录为空";
                            uploadBtn.SetEnabled(true);
                            retryFailedBtn.SetEnabled(true);
                        }
                    };

                    long uploadedBytes = 0;

                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    for (int i = 0; i < relativePaths.Count; i++)
                    {
                        var relativeNormalized = relativePaths[i];
                        var fileFull = relativeToFull[relativeNormalized];
                        var fi = new FileInfo(fileFull);
                        string remoteRootPart = string.IsNullOrEmpty(remoteRoot) ? "" : remoteRoot.TrimStart('/').TrimEnd('/');
                        string platformSegment = includePlatform ? $"{buildTarget}/" : "";
                        string remotePathAfterHost = string.IsNullOrEmpty(remoteRootPart)
                            ? $"{packageName}/{platformSegment}{relativeNormalized}"
                            : $"{remoteRootPart}/{packageName}/{platformSegment}{relativeNormalized}";

                        string uri = $"{host}/{remotePathAfterHost}";

                        long fileLength = fi.Length;
                        long currentFileSent = 0;

                        var remoteDir = Path.GetDirectoryName(remotePathAfterHost)?.Replace('\\', '/');
                        if (!string.IsNullOrEmpty(remoteDir))
                        {
                            EnsureFtpDirectoryExists(host, user, pass, remoteDir);
                        }

                        bool fileSucceeded = false;
                        try
                        {
                            await UploadFileWithProgress(uri, user, pass, fileFull, (sentBytes) =>
                            {
                                currentFileSent += sentBytes;
                                uploadedBytes += sentBytes;

                                float totalProgress = totalBytes > 0 ? Math.Min(1f, (float)uploadedBytes / totalBytes) : 1f;
                                float currentFileProgress = fileLength > 0 ? Math.Min(1f, (float)currentFileSent / fileLength) : 1f;

                                EditorApplication.delayCall += () =>
                                {
                                    progressBar.value = totalProgress;
                                    statusLabel.text = $"总 {(int)(totalProgress * 100)}%  ({FormatBytes(uploadedBytes)}/{FormatBytes(totalBytes)})  |  {(int)(currentFileProgress * 100)}%  —  {relativeNormalized} ({FormatBytes(currentFileSent)}/{FormatBytes(fileLength)})";
                                };
                            });

                            fileSucceeded = true;
                        }
                        catch (Exception exFile)
                        {
                            // 单文件上传失败，记录到失败列表并继续
                            EditorApplication.delayCall += () =>
                            {
                                if (!failedLabelMap.ContainsKey(relativeNormalized))
                                {
                                    var failLbl = new Label($"{relativeNormalized} ({FormatBytes(fileLength)})  —  错误: {exFile.Message}");
                                    failLbl.style.unityFontStyleAndWeight = FontStyle.Normal;
                                    failLbl.style.whiteSpace = WhiteSpace.Normal;
                                    failLbl.style.unityTextAlign = TextAnchor.MiddleLeft;
                                    failLbl.style.marginBottom = 2;
                                    failedScroll.contentContainer.Add(failLbl);
                                    failedLabelMap[relativeNormalized] = failLbl;
                                    failedList.Add(relativeNormalized);
                                    failedCount++;
                                    failedTitle.text = $"失败 ({failedCount})";
                                    statusLabel.text = $"上传过程中出现 {failedCount} 个失败";
                                }
                            };
                        }

                        if (fileSucceeded)
                        {
                            EditorApplication.delayCall += () =>
                            {
                                if (pendingLabelMap.TryGetValue(relativeNormalized, out var pendingLbl))
                                {
                                    pendingScroll.contentContainer.Remove(pendingLbl);
                                    pendingLabelMap.Remove(relativeNormalized);
                                    pendingCount = Math.Max(0, pendingCount - 1);
                                    pendingTitle.text = $"未传输 ({pendingCount})";
                                }

                                var doneLbl = new Label($"{relativeNormalized} ({FormatBytes(fileLength)})");
                                doneLbl.style.unityFontStyleAndWeight = FontStyle.Normal;
                                doneLbl.style.whiteSpace = WhiteSpace.Normal;
                                doneLbl.style.unityTextAlign = TextAnchor.MiddleLeft;
                                doneLbl.style.marginBottom = 2;
                                uploadedScroll.contentContainer.Add(doneLbl);
                                uploadedCount++;
                                uploadedTitle.text = $"已传输 ({uploadedCount})";

                                uploadedScroll.ScrollTo(doneLbl);

                                float totalProgressAfterFile = totalBytes > 0 ? Math.Min(1f, (float)uploadedBytes / totalBytes) : 1f;
                                progressBar.value = totalProgressAfterFile;
                                statusLabel.text = $"总 {(int)(totalProgressAfterFile * 100)}% ({FormatBytes(uploadedBytes)}/{FormatBytes(totalBytes)}) | 100%  —  {relativeNormalized} ({FormatBytes(fileLength)}/{FormatBytes(fileLength)})";
                            };
                        }
                    }

                    EditorApplication.delayCall += () =>
                    {
                        progressBar.value = 1f;
                        statusLabel.text = "上传完成";
                        if (failedCount == 0)
                            EditorUtility.DisplayDialog("完成", $"FTP 上传完成，失败 {failedCount} 个文件，请检查并使用“重传失败”。", "确定");
                        uploadBtn.SetEnabled(true);
                        retryFailedBtn.SetEnabled(true);
                    };
                }
                catch (Exception ex)
                {
                    EditorApplication.delayCall += () =>
                    {
                        statusLabel.text = "上传失败: " + ex.Message;
                        EditorUtility.DisplayDialog("上传失败", ex.Message, "确定");
                        uploadBtn.SetEnabled(true);
                        retryFailedBtn.SetEnabled(true);
                    };
                }
            });
        };

        // 重传失败按钮逻辑（仅对 failedList）
        retryFailedBtn.clicked += () =>
        {
            if (failedList.Count == 0)
            {
                EditorUtility.DisplayDialog("提示", "当前没有失败项可重传。", "确定");
                return;
            }

            retryFailedBtn.SetEnabled(false);
            uploadBtn.SetEnabled(false);
            progressBar.value = 0f;
            statusLabel.text = "准备重传失败项...";

            string host = EditorPrefs.GetString(keyHost, "").Trim();
            if (!host.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase) &&
                !host.StartsWith("ftps://", StringComparison.OrdinalIgnoreCase))
            {
                host = "ftp://" + host;
            }
            host = host.TrimEnd('/');

            string user = EditorPrefs.GetString(keyUser, "");
            string pass = EditorPrefs.GetString(keyPass, "");
            string remoteRoot = EditorPrefs.GetString(keyRemoteRoot, "");
            bool includePlatform = EditorPrefs.GetBool(keyIncludePlatform, false);

            // 复制失败列表并清 UI
            var toRetry = failedList.ToArray();
            failedList.Clear();
            failedLabelMap.Clear();
            EditorApplication.delayCall += () =>
            {
                failedScroll.contentContainer.Clear();
                failedCount = 0;
                failedTitle.text = $"失败 ({failedCount})";
            };

            Task.Run(async () =>
            {
                try
                {
                    string localRoot = GetLatestOutputDir(buildTarget, packageName);
                    if (string.IsNullOrEmpty(localRoot) || !Directory.Exists(localRoot))
                    {
                        EditorApplication.delayCall += () =>
                        {
                            EditorUtility.DisplayDialog("错误", "未找到最新构建输出目录，无法重传。", "确定");
                            statusLabel.text = "未找到本地目录";
                            retryFailedBtn.SetEnabled(true);
                            uploadBtn.SetEnabled(true);
                        };
                        return;
                    }

                    long totalBytes = 0;
                    var relativeToFull = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    var sizeMap = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                    var validRetries = new List<string>();

                    foreach (var rel in toRetry)
                    {
                        var full = Path.Combine(localRoot, rel.Replace('/', Path.DirectorySeparatorChar));
                        if (File.Exists(full))
                        {
                            var fi = new FileInfo(full);
                            totalBytes += fi.Length;
                            relativeToFull[rel] = full;
                            sizeMap[rel] = fi.Length;
                            validRetries.Add(rel);
                        }
                        else
                        {
                            // 文件不存在，直接标记为失败并显示
                            EditorApplication.delayCall += () =>
                            {
                                var failLbl = new Label($"{rel} (本地文件缺失)");
                                failLbl.style.unityFontStyleAndWeight = FontStyle.Normal;
                                failLbl.style.whiteSpace = WhiteSpace.Normal;
                                failLbl.style.unityTextAlign = TextAnchor.MiddleLeft;
                                failLbl.style.marginBottom = 2;
                                failedScroll.contentContainer.Add(failLbl);
                                failedLabelMap[rel] = failLbl;
                                failedList.Add(rel);
                                failedCount++;
                                failedTitle.text = $"失败 ({failedCount})";
                            };
                        }
                    }

                    if (validRetries.Count == 0)
                    {
                        EditorApplication.delayCall += () =>
                        {
                            statusLabel.text = "无可重传的文件";
                            retryFailedBtn.SetEnabled(true);
                            uploadBtn.SetEnabled(true);
                        };
                        return;
                    }

                    long uploadedBytes = 0;
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    foreach (var rel in validRetries)
                    {
                        var fileFull = relativeToFull[rel];
                        var fileLength = sizeMap[rel];
                        long currentFileSent = 0;

                        string remoteRootPart = string.IsNullOrEmpty(remoteRoot) ? "" : remoteRoot.TrimStart('/').TrimEnd('/');
                        string platformSegment = includePlatform ? $"{buildTarget}/" : "";
                        string remotePathAfterHost = string.IsNullOrEmpty(remoteRootPart)
                            ? $"{packageName}/{platformSegment}{rel}"
                            : $"{remoteRootPart}/{packageName}/{platformSegment}{rel}";

                        string uri = $"{host}/{remotePathAfterHost}";

                        var remoteDir = Path.GetDirectoryName(remotePathAfterHost)?.Replace('\\', '/');
                        if (!string.IsNullOrEmpty(remoteDir))
                        {
                            EnsureFtpDirectoryExists(host, user, pass, remoteDir);
                        }

                        bool succeeded = false;
                        try
                        {
                            await UploadFileWithProgress(uri, user, pass, fileFull, (sentBytes) =>
                            {
                                currentFileSent += sentBytes;
                                uploadedBytes += sentBytes;

                                float totalProgress = totalBytes > 0 ? Math.Min(1f, (float)uploadedBytes / totalBytes) : 1f;
                                float currentFileProgress = fileLength > 0 ? Math.Min(1f, (float)currentFileSent / fileLength) : 1f;

                                EditorApplication.delayCall += () =>
                                {
                                    progressBar.value = totalProgress;
                                    statusLabel.text = $"重传 总 {(int)(totalProgress * 100)}%  ({FormatBytes(uploadedBytes)}/{FormatBytes(totalBytes)})  |  {(int)(currentFileProgress * 100)}%  —  {rel} ({FormatBytes(currentFileSent)}/{FormatBytes(fileLength)})";
                                };
                            });

                            succeeded = true;
                        }
                        catch (Exception exRetry)
                        {
                            EditorApplication.delayCall += () =>
                            {
                                var failLbl = new Label($"{rel} ({FormatBytes(fileLength)})  —  错误: {exRetry.Message}");
                                failLbl.style.unityFontStyleAndWeight = FontStyle.Normal;
                                failLbl.style.whiteSpace = WhiteSpace.Normal;
                                failLbl.style.unityTextAlign = TextAnchor.MiddleLeft;
                                failLbl.style.marginBottom = 2;
                                failedScroll.contentContainer.Add(failLbl);
                                failedLabelMap[rel] = failLbl;
                                failedList.Add(rel);
                                failedCount++;
                                failedTitle.text = $"失败 ({failedCount})";
                                statusLabel.text = $"重传过程中出现 {failedCount} 个失败";
                            };
                        }

                        if (succeeded)
                        {
                            EditorApplication.delayCall += () =>
                            {
                                var doneLbl = new Label($"{rel} ({FormatBytes(fileLength)})");
                                doneLbl.style.unityFontStyleAndWeight = FontStyle.Normal;
                                doneLbl.style.whiteSpace = WhiteSpace.Normal;
                                doneLbl.style.unityTextAlign = TextAnchor.MiddleLeft;
                                doneLbl.style.marginBottom = 2;
                                uploadedScroll.contentContainer.Add(doneLbl);
                                uploadedCount++;
                                uploadedTitle.text = $"已传输 ({uploadedCount})";

                                uploadedScroll.ScrollTo(doneLbl);

                                float totalProgressAfterFile = totalBytes > 0 ? Math.Min(1f, (float)uploadedBytes / totalBytes) : 1f;
                                progressBar.value = totalProgressAfterFile;
                                statusLabel.text = $"重传 总 {(int)(totalProgressAfterFile * 100)}% ({FormatBytes(uploadedBytes)}/{FormatBytes(totalBytes)}) | 100%  —  {rel}";
                            };
                        }
                    }

                    EditorApplication.delayCall += () =>
                    {
                        progressBar.value = 1f;
                        if (failedCount == 0)
                            statusLabel.text = $"重传完成，仍有 {failedCount} 个失败";
                        retryFailedBtn.SetEnabled(true);
                        uploadBtn.SetEnabled(true);
                    };
                }
                catch (Exception ex)
                {
                    EditorApplication.delayCall += () =>
                    {
                        statusLabel.text = "重传失败: " + ex.Message;
                        EditorUtility.DisplayDialog("重传失败", ex.Message, "确定");
                        retryFailedBtn.SetEnabled(true);
                        uploadBtn.SetEnabled(true);
                    };
                }
            });
        };
    }

    // 插入到 Ex 类中（作为静态方法），用于补齐缺失实现并消除编译错误
    static string GetLatestOutputDir(BuildTarget buildTarget, string packageName)
    {
        try
        {
            var outputRoot = $"{AssetBundleBuilderHelper.GetDefaultBuildOutputRoot()}/{buildTarget}/{packageName}";
            if (!Directory.Exists(outputRoot))
                return string.Empty;

            var dirs = Directory.GetDirectories(outputRoot);
            if (dirs == null || dirs.Length == 0)
                return string.Empty;

            Version max = null;
            string maxDir = null;
            foreach (var d in dirs)
            {
                try
                {
                    var name = new DirectoryInfo(d).Name;
                    var v = new Version(name);
                    if (max == null || max < v)
                    {
                        max = v;
                        maxDir = d;
                    }
                }
                catch
                {
                    // 忽略非法目录名
                }
            }
            return maxDir ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    static async Task PopulatePendingForContainer(VisualElement container, BuildTarget buildTarget, string packageName)
    {
        if (container == null) return;

        try
        {
            string localRoot = GetLatestOutputDir(buildTarget, packageName);
            List<string> relativePaths = new List<string>();
            var sizeMap = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

            if (!string.IsNullOrEmpty(localRoot) && Directory.Exists(localRoot))
            {
                var files = Directory.GetFiles(localRoot, "*", SearchOption.AllDirectories);
                foreach (var f in files)
                {
                    var rel = f.Substring(localRoot.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace('\\', '/');
                    relativePaths.Add(rel);
                    try
                    {
                        var fi = new FileInfo(f);
                        sizeMap[rel] = fi.Length;
                    }
                    catch
                    {
                        sizeMap[rel] = 0;
                    }
                }
            }

            // 在主线程更新 UI
            EditorApplication.delayCall += () =>
            {
                RefreshPendingUI(container, relativePaths, sizeMap);
            };
        }
        catch (Exception ex)
        {
            EditorApplication.delayCall += () =>
            {
                Debug.LogError($"PopulatePendingForContainer 异常: {ex.Message}");
            };
        }
    }

    static Task PopulatePendingForContainerByRoot(VisualElement root, BuildTarget buildTarget, string packageName)
    {
        if (root == null)
            return Task.CompletedTask;

        // 容器名与 AddFtpUploader 中一致
        string containerName = $"YooAsset.Ftp.{buildTarget}.{packageName}";
        var container = root.Q<VisualElement>(containerName);
        if (container == null)
            return Task.CompletedTask;

        return PopulatePendingForContainer(container, buildTarget, packageName);
    }

    static void EnsureFtpDirectoryExists(string host, string user, string pass, string remoteDir)
    {
        if (string.IsNullOrEmpty(remoteDir)) return;

        // 分级创建目录：a/b/c -> 尝试创建 a, a/b, a/b/c
        var parts = remoteDir.Replace('\\', '/').Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return;

        string current = "";
        foreach (var p in parts)
        {
            current = string.IsNullOrEmpty(current) ? p : $"{current}/{p}";
            try
            {
                var uri = $"{host}/{current}";
                var req = (FtpWebRequest)WebRequest.Create(uri);
                req.Method = WebRequestMethods.Ftp.MakeDirectory;
                if (!string.IsNullOrEmpty(user))
                    req.Credentials = new NetworkCredential(user, pass ?? "");
                req.UseBinary = true;
                req.KeepAlive = false;
                // 某些服务器会返回错误表示已存在，这里捕获并忽略
                using (var resp = (FtpWebResponse)req.GetResponse())
                {
                    // 无需处理
                }
            }
            catch (WebException wex)
            {
                // 如果目录已存在，FTP 服务器通常返回错误码 550 或类似，忽略之
                // 其他异常也不抛出（以防阻塞上传流程）
                try
                {
                    var resp = wex.Response as FtpWebResponse;
                    if (resp != null)
                    {
                        // 忽略
                    }
                }
                catch { }
            }
            catch
            {
                // 忽略任何异常，保证流程继续
            }
        }
    }

    static Task UploadFileWithProgress(string uri, string user, string pass, string localFile, Action<long> onProgress)
    {
        return Task.Run(async () =>
        {
            const int bufferSize = 81920;
            var request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UseBinary = true;
            request.KeepAlive = false;
            if (!string.IsNullOrEmpty(user))
                request.Credentials = new NetworkCredential(user, pass ?? "");

            using (var fileStream = new FileStream(localFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var requestStream = request.GetRequestStream())
            {
                var buffer = new byte[bufferSize];
                int bytesRead;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    try
                    {
                        await requestStream.WriteAsync(buffer, 0, bytesRead);
                    }
                    catch (Exception e)
                    {
                        EditorApplication.delayCall += () =>
                        {
                            Debug.LogError("传输失败 " + e);
                        };
                    }
                    try { onProgress?.Invoke(bytesRead); } catch { }
                }
            }

            // 获取响应以确保上传完成并捕获可能的服务器端错误
            using (var response = (FtpWebResponse)request.GetResponse())
            {
                EditorApplication.delayCall += () =>
                {
                    Debug.LogError("传输失败 " + response.StatusDescription);
                };
                // 可选：检查 response.StatusDescription
            }
        });
    }
}