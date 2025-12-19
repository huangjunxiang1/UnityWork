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
    static void cull_res(string packageName, BuildTarget buildTarget)
    {
        var dir = $"{AssetBundleBuilderHelper.GetDefaultBuildOutputRoot()}/{buildTarget}/{packageName}";
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
    static void AddCullControls(VisualElement root, string packageName, BuildTarget buildTarget, Action refreshPending = null)
    {
        Button myButton = new Button();
        myButton.style.marginTop = 10;
        myButton.text = "剔除重复资源";
        myButton.style.height = 50;
        myButton.AddToClassList("my-button-style");
        myButton.clicked += () =>
        {
            cull_res(packageName, buildTarget);
            refreshPending();
        };
        root.Add(myButton);
    }

    // 计划（伪代码）：
    // 1. 在 Raw.CreateView 和 Res.CreateView 中，当 base.CreateView(parent) 执行并且 _buildVersionField 可用时，注册 value 变化回调。
    // 2. 回调内容：当 _buildVersionField 的 value 变化时，异步调用 Ex.PopulatePendingForContainerByRoot(...) 以刷新 pending / uploaded 列表。
    // 3. 为避免重复注册，使用 _buildVersionField.userData 标记已注册回调，保证同一实例只注册一次。
    // 4. 保持原有逻辑不变：仍然添加剔除按钮与 FTP 面板，并在构建后异步触发刷新。
    // 5. 回调使用 Task.Run 执行以避免阻塞 UI 线程，同时 Populate 方法会在主线程通过 EditorApplication.delayCall 更新 UI。

    [BuildPipelineAttribute(nameof(EBuildPipeline.RawFileBuildPipeline))]
    internal class Raw : RawfileBuildpipelineViewer
    {
        protected override string GetDefaultPackageVersion() => getVer(this.BuildTarget, this.PackageName);
        public override void CreateView(VisualElement parent)
        {
            base.CreateView(parent);

            // 使用提取后的复用函数，并传入刷新回调（传递 buildVersionField）
            Ex.AddCullControls(Root, this.PackageName, this.BuildTarget, () => Ex.PopulatePendingForContainerByRoot(Root, this.BuildTarget, this.PackageName, this._buildVersionField));

            // 添加 FTP 上传面板，传入版本字段实例
            Ex.AddFtpUploader(Root, this.BuildTarget, this.PackageName, this._buildVersionField);

            this._buildVersionField.RegisterValueChangedCallback((evt) =>
            {
                // 使用 Task.Run 异步触发刷新，Populate 方法在主线程更新 UI
                PopulatePendingForContainerByRoot(Root, this.BuildTarget, this.PackageName, this._buildVersionField);
            });
        }
        protected override void ExecuteBuild()
        {
            base.ExecuteBuild();
            // 构建完成后异步刷新未传输列表显示，传入 _buildVersionField
            PopulatePendingForContainerByRoot(Root, this.BuildTarget, this.PackageName, this._buildVersionField);
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
            Ex.AddCullControls(Root, this.PackageName, this.BuildTarget, () => Ex.PopulatePendingForContainerByRoot(Root, this.BuildTarget, this.PackageName, this._buildVersionField));

            Ex.AddFtpUploader(Root, this.BuildTarget, this.PackageName, this._buildVersionField);

            this._buildVersionField.RegisterValueChangedCallback((evt) =>
            {
                PopulatePendingForContainerByRoot(Root, this.BuildTarget, this.PackageName, this._buildVersionField);
            });
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
            PopulatePendingForContainerByRoot(Root, this.BuildTarget, this.PackageName, this._buildVersionField);
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
    static void RefreshPendingUI(VisualElement container, List<string> pendingRelativePaths, Dictionary<string, long> sizeMap, Dictionary<string, Label> pendingLabelMap = null, List<string> uploadedRelativePaths = null)
    {
        if (container == null) return;
        var pendingScroll = container.Q<ScrollView>("pendingScroll");
        var pendingTitle = container.Q<Label>("pendingTitle");
        var uploadedTitle = container.Q<Label>("uploadedTitle");
        var statusLabel = container.Q<Label>("statusLabel");
        var uploadedScroll = container.Q<ScrollView>("uploadedScroll");
        if (pendingScroll == null || pendingTitle == null || uploadedTitle == null || statusLabel == null || uploadedScroll == null)
            return;

        pendingScroll.contentContainer.Clear();
        uploadedScroll.contentContainer.Clear();

        if (pendingLabelMap != null)
            pendingLabelMap.Clear();

        // pending 列
        foreach (var rel in pendingRelativePaths)
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

        // uploaded 列（以 record.txt 为准，uploadedRelativePaths 可为 null）
        if (uploadedRelativePaths != null)
        {
            foreach (var rel in uploadedRelativePaths)
            {
                var sizeText = sizeMap != null && sizeMap.TryGetValue(rel, out var s) ? FormatBytes(s) : "0 B";
                var lbl = new Label($"{rel} ({sizeText})");
                lbl.style.unityFontStyleAndWeight = FontStyle.Normal;
                lbl.style.whiteSpace = WhiteSpace.Normal;
                lbl.style.unityTextAlign = TextAnchor.MiddleLeft;
                lbl.style.marginBottom = 2;
                uploadedScroll.contentContainer.Add(lbl);
            }
        }

        pendingTitle.text = $"未传输 ({pendingRelativePaths.Count})";
        uploadedTitle.text = $"已传输 ({(uploadedRelativePaths != null ? uploadedRelativePaths.Count : 0)})";

        if (pendingRelativePaths.Count == 0)
        {
            statusLabel.text = "目录为空或全部已传输";
        }
        else
        {
            statusLabel.text = $"找到 {pendingRelativePaths.Count} 个待传文件，准备上传";
        }
    }

    // 新版：FTP 上传功能整体重构（优化 UI 布局与交互），移除失败/重传
    // 新增参数 TextField versionField：优先使用此字段中的版本号作为本地构建目录
    static void AddFtpUploader(VisualElement root, BuildTarget buildTarget, string packageName, TextField versionField = null)
    {
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

        root.Add(container);

        // 映射与计数（提前声明以便被初始化逻辑复用）
        var pendingLabelMap = new Dictionary<string, Label>(StringComparer.OrdinalIgnoreCase);
        var uploadedCount = 0;
        var pendingCount = 0;

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

        // 异步填充未传输列表（页面初始化时就显示 pending 列表和已传列表）
        PopulatePendingForContainer(container, buildTarget, packageName, versionField);

        // upload 按钮逻辑（上传并记录成功项到 record.txt）
        uploadBtn.clicked += () =>
        {
            EditorPrefs.SetString(keyHost, ftpHostField.value ?? "");
            EditorPrefs.SetString(keyUser, userField.value ?? "");
            EditorPrefs.SetString(keyPass, passField.value ?? "");
            EditorPrefs.SetString(keyRemoteRoot, remotePathField.value ?? "");
            EditorPrefs.SetBool(keyIncludePlatform, includePlatformToggle.value);

            bool includePlatform = includePlatformToggle.value;

            uploadBtn.SetEnabled(false);
            progressBar.value = 0f;
            statusLabel.text = "准备上传...";

            string host = ftpHostField.value?.Trim() ?? "";
            string user = userField.value ?? "";
            string pass = passField.value ?? "";
            string remoteRoot = remotePathField.value?.Trim() ?? "";

            pendingLabelMap.Clear();
            uploadedScroll.contentContainer.Clear();
            pendingScroll.contentContainer.Clear();
            uploadedCount = 0;
            pendingCount = 0;

            Task.Run(async () =>
            {
                try
                {
                    // 使用优先从 UI 获取的版本目录（优先使用传入的 versionField）
                    string localRoot = $"{AssetBundleBuilderHelper.GetDefaultBuildOutputRoot()}/{buildTarget}/{packageName}/{versionField.value}";
                    if (string.IsNullOrEmpty(localRoot) || !Directory.Exists(localRoot))
                    {
                        EditorApplication.delayCall += () =>
                        {
                            EditorUtility.DisplayDialog("错误", "未找到最新构建输出目录，无法上传。", "确定");
                            statusLabel.text = "未找到本地目录";
                            uploadBtn.SetEnabled(true);
                        };
                        return;
                    }

                    if (!host.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase) &&
                        !host.StartsWith("ftps://", StringComparison.OrdinalIgnoreCase))
                    {
                        host = "ftp://" + host;
                    }
                    host = host.TrimEnd('/');

                    // 过滤掉 record.txt 本身，避免上传记录文件
                    var allFiles = Directory.GetFiles(localRoot, "*", SearchOption.AllDirectories);
                    var files = allFiles.Where(f => !string.Equals(Path.GetFileName(f), "record.txt", StringComparison.OrdinalIgnoreCase)).ToArray();

                    if (files.Length == 0)
                    {
                        EditorApplication.delayCall += () =>
                        {
                            EditorUtility.DisplayDialog("提示", "目录为空，无需上传。", "确定");
                            statusLabel.text = "目录为空";
                            uploadBtn.SetEnabled(true);
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

                    // 读取 record.txt，计算 pending / uploaded
                    var uploadedSet = LoadUploadedFromRecord(GetRecordFilePath(localRoot));
                    var uploadedList = uploadedSet.Where(r => relativeToFull.ContainsKey(r)).ToList();
                    var pendingList = relativePaths.Except(uploadedList).ToList();

                    EditorApplication.delayCall += () =>
                    {
                        RefreshPendingUI(container, pendingList, sizeMap, pendingLabelMap, uploadedList);
                        if (pendingList.Count == 0)
                        {
                            EditorUtility.DisplayDialog("提示", "没有待上传的文件。", "确定");
                            statusLabel.text = "无待上传文件";
                            uploadBtn.SetEnabled(true);
                        }
                    };

                    long uploadedBytes = 0;

                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    for (int i = 0; i < pendingList.Count; i++)
                    {
                        var relativeNormalized = pendingList[i];
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
                            // 每个文件上传使用 5000ms 超时
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
                            }, 5000);

                            fileSucceeded = true;
                        }
                        catch (Exception exFile)
                        {
                            // 上传单个文件失败：记录日志，并继续下一个文件（不做失败列表）
                            EditorApplication.delayCall += () =>
                            {
                                Debug.LogError($"上传失败: {relativeNormalized} -> {exFile.Message}");
                                statusLabel.text = $"上传失败: {relativeNormalized}";
                            };
                        }

                        if (fileSucceeded)
                        {
                            // 追加到 record.txt（本地文件）
                            try
                            {
                                AppendUploadedRecord(GetRecordFilePath(localRoot), relativeNormalized);
                            }
                            catch (Exception exAppend)
                            {
                                Debug.LogError("写入 record.txt 失败: " + exAppend.Message);
                            }

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
                        uploadBtn.SetEnabled(true);
                    };
                }
                catch (Exception ex)
                {
                    EditorApplication.delayCall += () =>
                    {
                        statusLabel.text = "上传失败: " + ex.Message;
                        EditorUtility.DisplayDialog("上传失败", ex.Message, "确定");
                        uploadBtn.SetEnabled(true);
                    };
                }
            });
        };
    }

    // record.txt 相关辅助：获取路径、读取、追加
    static string GetRecordFilePath(string localRoot)
    {
        if (string.IsNullOrEmpty(localRoot)) return string.Empty;
        try
        {
            return Path.Combine(localRoot, "record.txt");
        }
        catch
        {
            return string.Empty;
        }
    }

    static HashSet<string> LoadUploadedFromRecord(string recordFilePath)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        try
        {
            if (string.IsNullOrEmpty(recordFilePath)) return set;
            if (!File.Exists(recordFilePath)) return set;
            var lines = File.ReadAllLines(recordFilePath);
            foreach (var l in lines)
            {
                var t = (l ?? "").Trim();
                if (string.IsNullOrEmpty(t)) continue;
                // 规范化为使用 '/' 的相对路径
                var norm = t.Replace('\\', '/');
                if (!set.Contains(norm))
                    set.Add(norm);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("读取 record.txt 失败: " + ex.Message);
        }
        return set;
    }

    static void AppendUploadedRecord(string recordFilePath, string relativePath)
    {
        if (string.IsNullOrEmpty(recordFilePath) || string.IsNullOrEmpty(relativePath)) return;
        var dir = Path.GetDirectoryName(recordFilePath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        // 先读取现有内容判断是否已存在，避免重复追加
        bool exists = false;
        if (File.Exists(recordFilePath))
        {
            var existing = new HashSet<string>(File.ReadAllLines(recordFilePath).Select(s => s.Replace('\\', '/')), StringComparer.OrdinalIgnoreCase);
            exists = existing.Contains(relativePath.Replace('\\', '/'));
        }
        if (!exists)
        {
            using (var sw = new StreamWriter(recordFilePath, true, System.Text.Encoding.UTF8))
            {
                sw.WriteLine(relativePath.Replace('\\', '/'));
            }
        }
    }

    // 更新：增加 versionField 参数以便从 UI 获取版本号
    static void PopulatePendingForContainer(VisualElement container, BuildTarget buildTarget, string packageName, TextField versionField = null)
    {
        if (container == null) return;

        try
        {
            // 使用优先从 UI 获取的版本目录（优先使用传入的 versionField）
            string localRoot = $"{AssetBundleBuilderHelper.GetDefaultBuildOutputRoot()}/{buildTarget}/{packageName}/{versionField.value}";
            List<string> allRelativePaths = new List<string>();
            var sizeMap = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

            if (!string.IsNullOrEmpty(localRoot) && Directory.Exists(localRoot))
            {
                var files = Directory.GetFiles(localRoot, "*", SearchOption.AllDirectories);
                foreach (var f in files)
                {
                    var rel = f.Substring(localRoot.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace('\\', '/');
                    allRelativePaths.Add(rel);
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

            // 读取 record.txt
            var recordPath = GetRecordFilePath(localRoot);
            var uploadedSet = LoadUploadedFromRecord(recordPath);
            // 只保留在当前目录中存在的已传项，保持显示一致性
            var uploadedList = uploadedSet.Where(r => allRelativePaths.Contains(r)).ToList();
            var pendingList = allRelativePaths.Except(uploadedList).ToList();

            RefreshPendingUI(container, pendingList, sizeMap, null, uploadedList);
        }
        catch (Exception ex)
        {
            Debug.LogError($"PopulatePendingForContainer 异常: {ex.Message}");
        }
    }

    // 更新：允许传入 versionField 参数
    static void PopulatePendingForContainerByRoot(VisualElement root, BuildTarget buildTarget, string packageName, TextField versionField = null)
    {
        if (root == null)
            return;

        // 容器名与 AddFtpUploader 中一致
        string containerName = $"YooAsset.Ftp.{buildTarget}.{packageName}";
        var container = root.Q<VisualElement>(containerName);
        if (container == null)
            return;

        PopulatePendingForContainer(container, buildTarget, packageName, versionField);
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
                req.Timeout = 5000;
                req.ReadWriteTimeout = 5000;
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

    static async Task UploadFileWithProgress(string uri, string user, string pass, string localFile, Action<long> onProgress, int timeoutMs = 5000)
    {
        const int bufferSize = 81920;
        var request = (FtpWebRequest)WebRequest.Create(uri);
        request.Method = WebRequestMethods.Ftp.UploadFile;
        request.UseBinary = true;
        request.KeepAlive = false;
        request.Timeout = timeoutMs;
        request.ReadWriteTimeout = timeoutMs;
        if (!string.IsNullOrEmpty(user))
            request.Credentials = new NetworkCredential(user, pass ?? "");

        using (var fileStream = new FileStream(localFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var requestStream = request.GetRequestStream())
        {
            var buffer = new byte[bufferSize];
            int bytesRead;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                // 写入并设置超时监测：如果在 timeoutMs 内未完成写入则认为超时
                var writeTask = requestStream.WriteAsync(buffer, 0, bytesRead);
                var completed = await Task.WhenAny(writeTask, Task.Delay(timeoutMs));
                if (completed != writeTask)
                {
                    try { request.Abort(); } catch { }
                    throw new TimeoutException($"上传超时 {timeoutMs}ms");
                }
                // 确保捕获写入异常
                await writeTask;

                try { onProgress?.Invoke(bytesRead); } catch { }
            }
        }

        // 获取响应以确保上传完成并捕获可能的服务器端错误
        using (var response = (FtpWebResponse)request.GetResponse())
        {
            // response 可用于日志检查
            // 不在此记录为错误（上传成功返回正常状态）
        }
    }
}