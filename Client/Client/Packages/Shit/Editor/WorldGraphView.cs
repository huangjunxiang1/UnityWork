using Core;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

namespace Shit.Editor
{
    /*
    详细计划（伪代码）:
    1. 目标：当字段值为 System.Collections.IEnumerable（且不是 string）时，不再只显示数量，而是展开展示每个元素。
    2. 修改点：
       - 将 `CreateValueElement(object value, Type valueType)` 方法扩展为 `CreateValueElement(object value, Type valueType, int depth = 0)`，增加深度限制参数以避免无限递归。
       - 在可枚举类型分支中：
         a. 将枚举转换为 List<object>（安全捕获异常），获得元素数量。
         b. 使用一个 `Foldout` 作为可展开容器，标题显示类型与数量。
         c. 使用 `ScrollView` 或竖直容器承载每个元素的展示行（索引标签 + 递归调用 `CreateValueElement` 用于元素本身）。
         d. 对递归深度设定最大值（如 4），超过则显示深度限制提示。
         e. 对元素数量设置上限（如 200）避免 UI 崩溃，超出显示省略提示。
       - 更新对 `CreateValueElement` 的所有调用处，传入初始深度 0。
    3. 兼容性与表现：
       - 保持原有其它类型的展示逻辑（ObjectField、Toggle、IntegerField 等）。
       - 对可能抛异常的枚举操作使用 try/catch，保证编辑器稳定。
       - 为元素展示行设置合适的样式，使其在节点中垂直排列，并可滚动查看较多元素。
    4. 实施：
       - 修改方法签名并替换方法体。
       - 更新 `MakeNode` 中对字段/属性创建值元素的调用，传入 depth 参数。
    */

    public class WorldGraphWindow : EditorWindow
    {
        private WorldGraphView _graphView;
        private List<object> _rootAsset;

        [MenuItem("Shit/World Graph")]
        public static void OpenWindow()
        {
            var wnd = GetWindow<WorldGraphWindow>();
            wnd.titleContent = new GUIContent("World Graph");
            wnd.minSize = new Vector2(600, 300);
            wnd.Show();
        }

        private void OnEnable()
        {
            reload();
            SObject.objChange -= reload;
            SObject.objChange += reload;
            World.Close -= reload;
            World.Close += reload;
        }

        void reload()
        {
            ThreadSynchronizationContext.MainThread?.Post(s =>
            {
                ConstructUI();
                LoadWorldRootAndBuild();
            });
        }

        private void OnDisable()
        {
            SObject.objChange -= reload;
            World.Close -= reload;
            if (_graphView != null)
            {
                rootVisualElement.Remove(_graphView);
                _graphView.Dispose();
                _graphView = null;
            }
        }

        private void ConstructUI()
        {
            rootVisualElement.Clear();

            // Left: GraphView
            _graphView = new WorldGraphView(this)
            {
                name = "world-graph-view",
                style =
                {
                    flexGrow = 1,
                }
            };

            rootVisualElement.Add(_graphView);
        }

        private void LoadWorldRootAndBuild()
        {
            _rootAsset = new();
            _rootAsset.AddRange(World.Worlds.Select(t => t.Root));
            _rootAsset.Add(Client.Data);

            _graphView.ClearGraph();

            if (_rootAsset != null)
            {
                // 将根节点显示为 GraphView 的一个中心节点，并展开 children
                _graphView.BuildFromRoot(_rootAsset);
            }
        }

        internal void OnNodeSelected(object nodeObject)
        {
            Repaint();
        }

        private void DrawObjectFieldsRecursive(object obj, int indentLevel, HashSet<int> visited, int maxDepth = 8)
        {
            if (obj == null)
            {
                GUILayout.Label(new string(' ', indentLevel * 2) + "<null>");
                return;
            }

            if (indentLevel > maxDepth)
            {
                GUILayout.Label(new string(' ', indentLevel * 2) + "... depth limit ...");
                return;
            }

            var hash = obj.GetHashCode();
            if (visited.Contains(hash))
            {
                GUILayout.Label(new string(' ', indentLevel * 2) + $"<循环引用 {obj.GetType().Name}>");
                return;
            }
            visited.Add(hash);

            var type = obj.GetType();
            GUILayout.Label(new string(' ', indentLevel * 2) + $"[{type.Name}]");

            // 如果是字符串或值类型或 UnityEngine.Object，直接显示 ToString()
            if (type == typeof(string) || type.IsPrimitive || type.IsEnum || typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                GUILayout.Label(new string(' ', (indentLevel + 1) * 2) + obj.ToString());
                return;
            }

            // 如果是 IEnumerable，列出元素
            if (obj is IEnumerable && !(obj is string))
            {
                var en = (obj as IEnumerable).GetEnumerator();
                int i = 0;
                while (en.MoveNext())
                {
                    var el = en.Current;
                    GUILayout.Label(new string(' ', (indentLevel + 1) * 2) + $"[{i}] {(el != null ? el.GetType().Name : "null")}");
                    DrawObjectFieldsRecursive(el, indentLevel + 2, visited, maxDepth);
                    i++;
                }
                return;
            }

            // 反射显示字段与属性（公共与非公共实例）
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var fields = type.GetFields(flags);
            foreach (var f in fields)
            {
                object value = null;
                try { value = f.GetValue(obj); } catch { value = "<get error>"; }
                GUILayout.Label(new string(' ', (indentLevel + 1) * 2) + $"{f.Name}: {(value != null ? value.ToString() : "null")}");
                if (value != null && !(value is string) && !f.FieldType.IsPrimitive)
                {
                    DrawObjectFieldsRecursive(value, indentLevel + 2, visited, maxDepth);
                }
            }

            var props = type.GetProperties(flags).Where(p => p.GetIndexParameters().Length == 0);
            foreach (var p in props)
            {
                object value = null;
                try { value = p.GetValue(obj, null); } catch { value = "<get error>"; }
                GUILayout.Label(new string(' ', (indentLevel + 1) * 2) + $"{p.Name}: {(value != null ? value.ToString() : "null")}");
                if (value != null && !(value is string) && !p.PropertyType.IsPrimitive)
                {
                    DrawObjectFieldsRecursive(value, indentLevel + 2, visited, maxDepth);
                }
            }
        }
    }

    internal class WorldGraphView : GraphView, IDisposable
    {
        private WorldGraphWindow _owner;
        private readonly List<Node> _createdNodes = new List<Node>();

        public WorldGraphView(WorldGraphWindow owner)
        {
            _owner = owner;
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();
        }

        public void ClearGraph()
        {
            foreach (var n in _createdNodes.ToList())
            {
                RemoveElement(n);
            }
            _createdNodes.Clear();
        }

        // 伪代码（详细计划）:
        // 1. 目标：对任意树形结构递归布局，父节点在左，子节点在右，同级（兄弟）竖直堆叠。
        // 2. 思路：先递归计算每个节点的“子树高度”（subtreeHeight），规则为：
        //      subtreeHeight(node) = max(nodeHeight, sum(subtreeHeight(child_i)) + verticalSpacing*(n-1))
        //    这样可以保证父节点至少占据 nodeHeight，高度不足时由子树高度决定布局空间。
        // 3. 布局：从根开始，给定根的中心 y（rootCenterY），将根放在 (x, rootCenterY - nodeHeight/2)。
        //    对根的所有子节点计算总高度 totalChildrenHeight，并以根的中心垂直居中排列子块：
        //      startY = rootCenterY - totalChildrenHeight/2
        //    对每个子：childCenterY = startY + childSubtreeHeight/2；递归地在 (x + nodeWidth + horizontalSpacing, childCenterY) 布局子树。
        // 4. 节点/端口/连线：在创建节点时使用 MakeNode；为每个节点在右侧（titleContainer 末尾）创建一个 Output 端口（多重），
        //    为每个子节点在左侧（titleContainer 起始）创建一个 Input 端口（单一）。父的输出端口连接至子输入端口。
        // 5. 防环/重复：使用 visited HashSet 来避免因循环引用导致无限递归（若发现已访问则不展开该分支）。
        // 6. 存储：使用字典 mapObjectToNode 保存对象到 Node 的映射，维护 _createdNodes 列表以便后续清理。
        // 7. 交互：为每个节点注册点击回调，保留节点可拖拽/可选择能力。
        // 8. 宽度/高度常量：使用合理默认值（nodeWidth/nodeHeight），允许未来从内容测量替代。
        // 9. 兼容性：尽量复用已有 MakeNode、CreateValueElement、ExtractChildren 等方法；端口 style.alignSelf = Align.Center。
        // 10. 最后刷新所有节点 Ports/State，完成布局并将所有元素添加到 GraphView。
        public void BuildFromRoot(System.Object rootAsset)
        {
            ClearGraph();

            if (rootAsset == null) return;

            // 常量配置（可以按需调整）
            float rootX = 100f;
            float rootY = 200f; // 参考起始 Y，多个根将围绕此处垂直居中排列
            float nodeWidth = 220f;
            float nodeHeight = 120f;
            float horizontalSpacing = 50f; // 父-子间水平间距
            float verticalSpacing = 20f;   // 兄弟节点间垂直间距
            float verticalSpacingBetweenRoots = 40f; // 根与根之间的间距

            // 存储映射与缓存
            var mapObjToNode = new Dictionary<object, Node>();
            var subtreeHeight = new Dictionary<object, float>();
            var visitedForLayout = new HashSet<int>(); // 使用 GetHashCode 做防环检测，减少引用比较成本
            var createdObjects = new List<object>();

            // 将输入统一为 roots 列表（支持传入 IEnumerable 作为多根）
            List<object> roots = null;
            try
            {
                if (rootAsset is System.Collections.IEnumerable && !(rootAsset is string))
                {
                    roots = new List<object>();
                    foreach (var r in (System.Collections.IEnumerable)rootAsset)
                    {
                        if (r != null) roots.Add(r);
                    }
                }
            }
            catch
            {
                // 如果遍历失败，则退化为单根处理
                roots = null;
            }
            if (roots == null)
            {
                roots = new List<object> { rootAsset };
            }

            // 提取子节点通用封装（与原逻辑一致）
            System.Func<object, IEnumerable<object>> getChildren = o =>
            {
                if (o == null) return null;
                if (o is STree tree) return tree._children?.Cast<object>() ?? Enumerable.Empty<object>();
                // 尝试更多可能的集合属性（如果需要可以扩展）
                return null;
            };

            // 递归计算子树高度
            System.Func<object, float> computeSubtreeHeight = null;
            computeSubtreeHeight = (obj) =>
            {
                if (obj == null) return nodeHeight;
                int hash = obj.GetHashCode();
                if (visitedForLayout.Contains(hash))
                {
                    // 遇到循环引用：作为叶子处理
                    return nodeHeight;
                }
                visitedForLayout.Add(hash);

                var children = getChildren(obj)?.ToList();
                if (children == null || children.Count == 0)
                {
                    subtreeHeight[obj] = nodeHeight;
                    return subtreeHeight[obj];
                }

                float sum = 0f;
                for (int i = 0; i < children.Count; i++)
                {
                    var c = children[i];
                    float h = computeSubtreeHeight(c);
                    sum += h;
                    if (i > 0) sum += verticalSpacing;
                }

                float result = Math.Max(nodeHeight, sum);
                subtreeHeight[obj] = result;
                return result;
            };

            // 布局并创建节点、端口与连接（递归）
            Action<object, float, float> layoutNode = null;
            layoutNode = (obj, x, centerY) =>
            {
                if (obj == null) return;
                int hash = obj.GetHashCode();
                // 创建或获取节点
                Node node;
                if (!mapObjToNode.TryGetValue(obj, out node))
                {
                    node = MakeNode(GetDisplayName(obj), obj);
                    node.capabilities |= Capabilities.Movable;
                    node.capabilities |= Capabilities.Selectable;

                    AddElement(node);
                    _createdNodes.Add(node);
                    mapObjToNode[obj] = node;
                    createdObjects.Add(obj);
                }

                // 放置节点（以 centerY 为中心）
                float top = centerY - nodeHeight * 0.5f;
                node.SetPosition(new Rect(x, top, nodeWidth, nodeHeight));

                // 确保节点 ports 与 foldouts 刷新（防止 UI 不更新）
                node.RefreshExpandedState();
                node.RefreshPorts();

                // 为节点右侧添加（或获取已有）输出端口（多连接）
                Port outPort = null;
                outPort = node.outputContainer.Children().OfType<Port>().FirstOrDefault(p => p.direction == Direction.Output);
                if (outPort == null)
                {
                    outPort = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
                    outPort.portName = "";
                    outPort.style.alignSelf = Align.Center;
                    node.titleContainer.Add(outPort);
                    node.RefreshExpandedState();
                    node.RefreshPorts();
                }

                // 处理子节点
                var children = getChildren(obj)?.ToList();
                if (children == null || children.Count == 0) return;

                // 计算子块总高度（使用之前缓存）
                float totalChildrenHeight = 0f;
                for (int i = 0; i < children.Count; i++)
                {
                    var c = children[i];
                    float h = subtreeHeight.ContainsKey(c) ? subtreeHeight[c] : computeSubtreeHeight(c);
                    totalChildrenHeight += h;
                    if (i > 0) totalChildrenHeight += verticalSpacing;
                }

                float startY = centerY - totalChildrenHeight * 0.5f;
                float childX = x + nodeWidth + horizontalSpacing;

                float cursorY = startY;
                foreach (var child in children)
                {
                    float childSubH = subtreeHeight.ContainsKey(child) ? subtreeHeight[child] : computeSubtreeHeight(child);
                    float childCenterY = cursorY + childSubH * 0.5f;

                    // 递归布局子节点
                    layoutNode(child, childX, childCenterY);

                    // 连接 outPort -> child inPort（确保 childNode 已创建）
                    var childNode = mapObjToNode[child];
                    Port inPort = childNode.titleContainer.Children().OfType<Port>().FirstOrDefault(p => p.direction == Direction.Input);
                    if (inPort == null)
                    {
                        inPort = childNode.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
                        inPort.portName = "";
                        inPort.style.alignSelf = Align.Center;
                        childNode.titleContainer.Insert(0, inPort);
                        childNode.RefreshExpandedState();
                        childNode.RefreshPorts();
                    }

                    // 创建连接（避免重复创建相同连接）
                    bool exists = this.edges.ToList().Any(e =>
                    {
                        return (e.input == inPort && e.output == outPort) || (e.input == outPort && e.output == inPort);
                    });
                    if (!exists)
                    {
                        var edge = outPort.ConnectTo(inPort);
                        AddElement(edge);
                    }

                    // 注册点击回调
                    RegisterNodeClick(childNode, child);

                    cursorY += childSubH + verticalSpacing;
                }

                // 注册当前节点点击
                RegisterNodeClick(node, obj);
            };

            // 重新计算 visitedForLayout 用于 compute，并保证递归能跑通（独立集合）
            visitedForLayout.Clear();

            // 对每个根计算子树高度（共享 subtreeHeight 缓存）
            foreach (var r in roots)
            {
                try
                {
                    computeSubtreeHeight(r);
                }
                catch
                {
                    // 忽略单个根计算错误，继续处理其他根
                }
            }

            // 计算所有根的总高度并按顺序上下排列（垂直堆叠）
            float totalRootsHeight = 0f;
            for (int i = 0; i < roots.Count; i++)
            {
                var r = roots[i];
                float h = subtreeHeight.ContainsKey(r) ? subtreeHeight[r] : nodeHeight;
                totalRootsHeight += h;
                if (i > 0) totalRootsHeight += verticalSpacingBetweenRoots;
            }

            // 根的中心基准（保留原 rootY 为参考中点）
            float globalCenterY = rootY + nodeHeight * 0.5f;
            float startYAll = globalCenterY - totalRootsHeight * 0.5f;

            float cursor = startYAll;
            foreach (var r in roots)
            {
                float rh = subtreeHeight.ContainsKey(r) ? subtreeHeight[r] : nodeHeight;
                float centerY = cursor + rh * 0.5f;
                layoutNode(r, rootX, centerY);
                cursor += rh + verticalSpacingBetweenRoots;
            }

            // 最终刷新所有节点 Ports
            foreach (var kv in mapObjToNode)
            {
                try
                {
                    kv.Value.RefreshExpandedState();
                    kv.Value.RefreshPorts();
                }
                catch { }
            }
        }

        private string GetDisplayName(object obj)
        {
            if (obj == null) return "null";
            if (obj is UnityEngine.Object uo) return $"{uo.name} : {obj.GetType().Name}";
            return obj.GetType().Name;
        }

        /*
        详细计划（伪代码）:
        1. 目标：为每个节点在其主容器中增加一行文本，显示该节点的 ToString() 结果（只读）。
        2. 在现有的 `MakeNode(string title, object userData)` 方法内：
           a. 在构建组件 Foldout 并包装成 `compWrapper` 之后、将 `compWrapper` 添加到 `node.mainContainer` 之前，
              创建一个新的 `VisualElement` 行 `toStringRow`。
           b. 在 `toStringRow` 中创建 `Label toStringLabel`，文本为 `userData?.ToString()`（安全捕获异常并回退）。
           c. 设置样式：左内边距、顶部间距、文本左对齐、`flexGrow=1` 以便长文本占据可用宽度并换行（在不同 Unity 版本上可能行为不同）。
           d. 将 `toStringRow` 添加到 `node.mainContainer`（放在组件折叠面板之前，以便优先显示）。
        3. 兼容性与鲁棒性：
           - 对 `ToString()` 调用用 try/catch，出现异常时显示 "<ToString error>"。
           - 确保不会破坏现有 UI 布局与端口功能。
        4. 实施：直接替换 `MakeNode` 方法实现，保留原有逻辑，仅插入上面步骤的代码块。
        */

        private Node MakeNode(string title, object userData)
        {
            /*
            详细伪代码（按步骤执行）:
            1. 保留原有节点创建与组件折叠逻辑，不改变现有布局与功能。
            2. 在创建完 components Foldout 后，创建一个 EcsSystemMethod 实例并调用 Get(userData) 填充其内部列表。
            3. 使用反射遍历 EcsSystemMethod 的公开实例字段（BindingFlags.Instance | BindingFlags.Public），
               对于每个字段：
               a. 如果字段值实现了 IList（例如 List<temp>），则创建一个 Foldout 显示字段名与数量。
               b. 对列表中的每个元素（期望类型为 EcsSystemMethod.temp），创建一行显示索引、Method 与 Parameter 字符串（按字段垂直排列）。
               c. 行使用与组件字段相同的样式（水平排列、左侧固定最小宽度索引标签、右侧内容竖直排列）。
               d. 对反射取值与类型强转使用 try/catch，保证编辑器稳定。
               e. 对列表元素数量设上限显示防止 UI 卡顿（例如 500）。
            4. 将 sys 的 Foldout 添加到 compFoldout（组件列表之后），风格与组件折叠统一。
            5. 保持其余原有逻辑不变，最后返回节点。
            */
            var node = new Node
            {
                title = title
            };
            node.style.backgroundColor = new Color(0.12f, 0.12f, 0.12f, 1f);
            node.userData = userData;
            node.RefreshExpandedState();
            node.RefreshPorts();

            // 确保节点默认可移动与可选择（避免遗漏）
            node.capabilities |= Capabilities.Movable;
            node.capabilities |= Capabilities.Selectable;

            try
            {
                // 新增：在添加组件折叠面板之前，插入一行显示 userData.ToString() 的文本
                try
                {
                    var toStringRow = new VisualElement();
                    toStringRow.style.backgroundColor = new StyleColor(new Color(0.12f, 0.12f, 0.12f, 1f));
                    toStringRow.style.flexDirection = FlexDirection.Row;
                    toStringRow.style.alignItems = Align.Center;
                    toStringRow.style.marginLeft = 6;
                    toStringRow.style.marginTop = 4;

                    if (userData is SObject ss)
                    {
                        var toStringLabel = new Label($"Actor={ss.ActorId}\nGid={ss.gid}");
                        toStringLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
                        toStringLabel.style.flexGrow = 1;
                        toStringRow.Add(toStringLabel);

                        node.mainContainer.Add(toStringRow);
                    }
                    else
                    {
                        var toStringLabel = new Label(userData == null ? "null" : userData.ToString());
                        toStringLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
                        toStringLabel.style.flexGrow = 1;
                        toStringRow.Add(toStringLabel);

                        node.mainContainer.Add(toStringRow);
                    }
                }
                catch
                {
                    // 忽略 UI 构建异常，避免影响节点创建
                }

                var compFoldout = new Foldout { text = "Components", value = false };
                node.mainContainer.Add(compFoldout);

                IDictionary entries = null;
                if (userData is SObject s)
                    entries = s._components;
                else if (userData is Data d)
                    entries = d._dataMap;

                if (entries == null || entries.Count == 0)
                {
                    compFoldout.Add(new Label("无组件"));
                }
                else
                {
                    foreach (DictionaryEntry e in entries)
                    {
                        var compType = e.Key as Type;
                        var compInstance = e.Value;

                        var singleCompFold = new Foldout { text = compType.Name, value = false };
                        singleCompFold.style.marginLeft = 4;
                        singleCompFold.style.marginTop = 2;

                        bool hasField = false;
                        try
                        {
                            // 包含 NonPublic，以便我们能检测带 ShowInInspector 的私有字段/属性
                            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                            var fields = compType.GetFields(flags);
                            foreach (var f in fields)
                            {
                                // 只显示公开字段或打了 ShowInInspector 的私有/非公开字段
                                if ((!f.IsPublic && f.GetCustomAttribute<ShowInInspector>() == null) || f.GetCustomAttribute<HideInInspector>() != null) continue;

                                object val = null;
                                try { val = f.GetValue(compInstance); } catch { val = "<get error>"; }

                                // 行容器：字段名 + 值控件（尽量不要纯文本）
                                var row = new VisualElement();
                                row.style.flexDirection = FlexDirection.Row;
                                row.style.alignItems = Align.Center;
                                row.style.marginLeft = 6;
                                row.style.marginTop = 2;

                                var nameLabel = new Label(f.Name) { style = { minWidth = 80 } };
                                nameLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
                                row.Add(nameLabel);

                                var valElem = CreateValueElement(val, f.FieldType, 0);
                                row.Add(valElem);

                                singleCompFold.Add(row);
                                hasField = true;
                            }

                            var props = compType.GetProperties(flags).Where(p => p.GetIndexParameters().Length == 0);
                            foreach (var p in props)
                            {
                                // 只显示可公开读取的属性，或带 ShowInInspector 的非公开属性

                                var getMethod = p.GetGetMethod(true);
                                bool isPublicGetter = getMethod != null && getMethod.IsPublic;
                                if ((!isPublicGetter && p.GetCustomAttribute<ShowInInspector>() == null) || p.GetCustomAttribute<HideInInspector>() != null) continue;

                                object val = null;
                                try { val = p.GetValue(compInstance, null); } catch { val = "<get error>"; }

                                var row = new VisualElement();
                                row.style.flexDirection = FlexDirection.Row;
                                row.style.alignItems = Align.Center;
                                row.style.marginLeft = 6;
                                row.style.marginTop = 2;

                                var nameLabel = new Label(p.Name) { style = { minWidth = 80 } };
                                nameLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
                                row.Add(nameLabel);

                                var valElem = CreateValueElement(val, p.PropertyType, 0);
                                row.Add(valElem);

                                singleCompFold.Add(row);
                                hasField = true;
                            }
                        }
                        catch
                        {
                            // 忽略反射获取字段可能出现的异常，保证编辑器不崩溃
                        }

                        if (!hasField)
                        {
                            var noFields = new Label("无字段");
                            noFields.style.unityTextAlign = TextAnchor.MiddleLeft;
                            noFields.style.marginLeft = 6;
                            singleCompFold.Add(noFields);
                        }

                        compFoldout.Add(singleCompFold);
                    }
                }

                // 预先声明 sysFoldout 但不要直接添加到 compFoldout —— 我们要把它作为与 Components 平级的元素
                Foldout sysFoldout = new Foldout { text = "Systems", value = false };
                node.mainContainer.Add(sysFoldout);

                EcsSystemMethod sys = new();
                sys.Get(userData as SObject);

                var sysType = typeof(EcsSystemMethod);
                var sysFields = sysType.GetFields(BindingFlags.Instance | BindingFlags.Public);

                foreach (var f in sysFields)
                {
                    object fieldVal = null;
                    try { fieldVal = f.GetValue(sys); } catch { fieldVal = null; }

                    if (fieldVal is System.Collections.IList list)
                    {
                        int count = list.Count;
                        var fieldFold = new Foldout { text = $"{f.Name} (count={count})", value = false };
                        fieldFold.style.marginLeft = 6;
                        fieldFold.style.marginTop = 2;

                        if (count == 0)
                        {
                            var emptyLbl = new Label("空");
                            emptyLbl.style.marginLeft = 6;
                            fieldFold.Add(emptyLbl);
                        }
                        else
                        {
                            const int maxItemsToShow = 500;
                            int idx = 0;
                            foreach (var item in list)
                            {
                                if (idx >= maxItemsToShow)
                                {
                                    var more = new Label("... 更多 ...");
                                    more.style.marginLeft = 6;
                                    fieldFold.Add(more);
                                    break;
                                }

                                // 尽量使用已知类型 EcsSystemMethod.temp
                                EcsSystemMethod.temp t = null;
                                try { t = item as EcsSystemMethod.temp; } catch { t = null; }

                                var row = new VisualElement();
                                row.style.flexDirection = FlexDirection.Row;
                                row.style.alignItems = Align.Center;
                                row.style.marginLeft = 6;
                                row.style.marginTop = 2;

                                var idxLabel = new Label($"[{idx}]") { style = { minWidth = 40 } };
                                idxLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
                                row.Add(idxLabel);

                                string methodStr = "<unknown>";
                                string paramStr = "";
                                if (t != null)
                                {
                                    try { methodStr = t.Method ?? "<null>"; } catch { methodStr = "<err>"; }
                                    try { paramStr = t.Parameter ?? ""; } catch { paramStr = ""; }
                                }
                                else
                                {
                                    // 退回到反射读取常见属性（Method / Parameter）
                                    try
                                    {
                                        var mi = item?.GetType().GetProperty("Method", BindingFlags.Instance | BindingFlags.Public);
                                        var pi = item?.GetType().GetProperty("Parameter", BindingFlags.Instance | BindingFlags.Public);
                                        methodStr = mi != null ? (mi.GetValue(item) as string ?? "<null>") : "<noMethod>";
                                        paramStr = pi != null ? (pi.GetValue(item) as string ?? "") : "";
                                    }
                                    catch
                                    {
                                        methodStr = "<toString>" + (item?.ToString() ?? "");
                                    }
                                }

                                // 右侧内容容器：竖直排列 Method 与 Parameter（分别为只读 TextField）
                                var contentContainer = new VisualElement();
                                contentContainer.style.flexDirection = FlexDirection.Column;
                                contentContainer.style.alignItems = Align.FlexStart;
                                contentContainer.style.flexGrow = 1;

                                var methodField = new TextField();
                                try { methodField.SetValueWithoutNotify(methodStr); } catch { methodField.SetValueWithoutNotify("<toString error>"); }
                                methodField.SetEnabled(true);
                                methodField.style.flexGrow = 1;
                                methodField.style.marginRight = 2;
                                methodField.style.marginBottom = 2;
                                contentContainer.Add(methodField);

                                var paramField = new TextField();
                                try { paramField.SetValueWithoutNotify(paramStr); } catch { paramField.SetValueWithoutNotify(""); }
                                paramField.SetEnabled(true);
                                paramField.style.flexGrow = 1;
                                contentContainer.Add(paramField);

                                row.Add(contentContainer);

                                fieldFold.Add(row);
                                idx++;
                            }
                        }

                        sysFoldout.Add(fieldFold);
                    }
                    else
                    {
                        // 如果字段不是列表但有值，简单显示其 ToString
                        if (fieldVal != null)
                        {
                            var simpleFold = new Foldout { text = $"{f.Name}", value = false };
                            simpleFold.style.marginLeft = 6;
                            simpleFold.style.marginTop = 2;

                            var lbl = new TextField();
                            try { lbl.SetValueWithoutNotify(fieldVal.ToString()); } catch { lbl.SetValueWithoutNotify("<toString error>"); }
                            lbl.SetEnabled(false);
                            lbl.style.flexGrow = 1;
                            simpleFold.Add(lbl);

                            sysFoldout.Add(simpleFold);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            // 确保标题栏（顶部）与扩展区（底部）是横向排列并左右分布，以便端口在左右两侧显示
            node.titleContainer.style.flexDirection = FlexDirection.Row;
            node.titleContainer.style.justifyContent = Justify.SpaceBetween;
            node.titleContainer.style.alignItems = Align.Center;
            node.extensionContainer.style.flexDirection = FlexDirection.Row;
            node.extensionContainer.style.justifyContent = Justify.Center;

            return node;
        }

        private VisualElement CreateValueElement(object value, Type valueType, int depth = 0)
        {
            // 返回一个非纯文本的 VisualElement，用合适的 UI 控件来展示常见类型（只读）
            try
            {
                // 深度限制，防止无限递归
                const int maxDepth = 4;
                if (depth > maxDepth)
                {
                    var depthLbl = new Label("...depth limit...");
                    depthLbl.style.unityTextAlign = TextAnchor.MiddleLeft;
                    return depthLbl;
                }

                // 容器用于限制宽度和间距
                var container = new VisualElement();
                container.style.flexGrow = 1;
                container.style.flexDirection = FlexDirection.Row;
                container.style.alignItems = Align.Center;
                container.style.marginLeft = 6;

                if (value == null)
                {
                    var lbl = new Label("null");
                    lbl.style.unityTextAlign = TextAnchor.MiddleLeft;
                    container.Add(lbl);
                    return container;
                }

                // UnityEngine.Object -> 可显示为 ObjectField（只读）
                if (value is UnityEngine.Object uo)
                {
                    try
                    {
                        var of = new UnityEditor.UIElements.ObjectField();
                        of.objectType = typeof(UnityEngine.Object);
                        of.SetValueWithoutNotify(uo);
                        of.SetEnabled(false);
                        of.style.flexGrow = 1;
                        container.Add(of);
                        return container;
                    }
                    catch
                    {
                        // 兼容性降级到 Label
                        var lbl = new Label(uo != null ? uo.name : "null");
                        lbl.style.unityTextAlign = TextAnchor.MiddleLeft;
                        container.Add(lbl);
                        return container;
                    }
                }

                // 布尔 -> Toggle（只读）
                if (valueType == typeof(bool) || value is bool)
                {
                    var t = new Toggle();
                    try { t.SetValueWithoutNotify(Convert.ToBoolean(value)); } catch { t.SetValueWithoutNotify(false); }
                    t.SetEnabled(false);
                    container.Add(t);
                    return container;
                }

                // 整数类型 -> IntegerField（只读）
                if (valueType == typeof(int) || valueType == typeof(long) || valueType == typeof(short) || valueType == typeof(byte)
                    || value is int || value is long || value is short || value is byte)
                {
                    var intField = new LongField();
                    try { intField.SetValueWithoutNotify(Convert.ToInt64(value)); } catch { intField.SetValueWithoutNotify(0); }
                    intField.SetEnabled(false);
                    intField.style.flexGrow = 1;
                    container.Add(intField);
                    return container;
                }

                // 浮点类型 -> FloatField（只读）
                if (valueType == typeof(float) || valueType == typeof(double) || valueType == typeof(decimal)
                    || value is float || value is double || value is decimal)
                {
                    var floatField = new DoubleField();
                    try { floatField.SetValueWithoutNotify(Convert.ToDouble(value)); } catch { floatField.SetValueWithoutNotify(0f); }
                    floatField.SetEnabled(false);
                    floatField.style.flexGrow = 1;
                    container.Add(floatField);
                    return container;
                }

                // 字符串 -> TextField（只读）
                if (valueType == typeof(string) || value is string)
                {
                    var tf = new TextField();
                    try { tf.SetValueWithoutNotify(value as string); } catch { tf.SetValueWithoutNotify(value?.ToString() ?? ""); }
                    tf.SetEnabled(false);
                    tf.style.flexGrow = 1;
                    container.Add(tf);
                    return container;
                }

                // 枚举 -> TextField（显示枚举名，避免复杂依赖）
                if (valueType.IsEnum || value is Enum)
                {
                    var tf = new TextField();
                    try { tf.SetValueWithoutNotify(value.ToString()); } catch { tf.SetValueWithoutNotify(""); }
                    tf.SetEnabled(false);
                    tf.style.flexGrow = 1;
                    container.Add(tf);
                    return container;
                }

                // IEnumerable（非字符串） -> 展示每个元素（折叠 + 列表）
                if (value is System.Collections.IEnumerable && !(value is string))
                {
                    // 将枚举转换成列表（安全捕获异常）
                    var list = new List<object>();
                    try
                    {
                        foreach (var el in (System.Collections.IEnumerable)value)
                        {
                            list.Add(el);
                        }
                    }
                    catch
                    {
                        // 如果遍历失败，则退化为显示类型名
                        var tfErr = new TextField();
                        tfErr.SetValueWithoutNotify("IEnumerable (enumeration error)");
                        tfErr.SetEnabled(false);
                        tfErr.style.flexGrow = 1;
                        container.Add(tfErr);
                        return container;
                    }

                    var fold = new Foldout();
                    fold.text = $"{value.GetType().Name} (count={list.Count})";
                    fold.value = false;

                    // 使用 ScrollView 承载元素列表，避免占用过多空间
                    var sv = new ScrollView();
                    sv.style.flexDirection = FlexDirection.Column;
                    sv.style.alignContent = Align.FlexStart;
                    sv.style.alignItems = Align.FlexStart;
                    sv.style.minHeight = 20;
                    sv.style.maxHeight = 220;
                    sv.verticalScrollerVisibility =  ScrollerVisibility.AlwaysVisible;
                    sv.horizontalScrollerVisibility =  ScrollerVisibility.AlwaysVisible;

                    int idx = 0;
                    const int maxItemsToShow = 200;
                    foreach (var el in list)
                    {
                        var row = new VisualElement();
                        row.style.flexDirection = FlexDirection.Row;
                        row.style.alignItems = Align.Center;
                        row.style.marginLeft = 6;
                        row.style.marginTop = 2;

                        var nameLabel = new Label($"[{idx}]") { style = { minWidth = 40 } };
                        nameLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
                        row.Add(nameLabel);

                        var elElem = CreateValueElement(el, el?.GetType() ?? typeof(object), depth + 1);
                        // 保证元素显示区域可以扩展
                        elElem.style.flexGrow = 1;
                        row.Add(elElem);

                        sv.Add(row);

                        idx++;
                        if (idx >= maxItemsToShow)
                        {
                            var more = new Label("... more ...");
                            more.style.marginLeft = 6;
                            sv.Add(more);
                            break;
                        }
                    }

                    fold.Add(sv);

                    // 折叠默认关闭，但将折叠放入容器时需要将其作为单列元素
                    var wrapper = new VisualElement();
                    wrapper.style.flexDirection = FlexDirection.Column;
                    wrapper.Add(fold);

                    return wrapper;
                }

                // 默认降级为只读 TextField（避免纯 Label）
                var defaultField = new TextField();
                try { defaultField.SetValueWithoutNotify(value.ToString()); } catch { defaultField.SetValueWithoutNotify("<toString error>"); }
                defaultField.SetEnabled(false);
                defaultField.style.flexGrow = 1;
                container.Add(defaultField);
                return container;
            }
            catch
            {
                // 最后兜底：一个简单的 Label（尽量不发生）
                var lbl = new Label(value != null ? value.ToString() : "null");
                lbl.style.unityTextAlign = TextAnchor.MiddleLeft;
                return lbl;
            }
        }
        private void RegisterNodeClick(Node node, object obj)
        {
            node.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button == 0)
                {
                    _owner.OnNodeSelected(obj);
                    // 不再 StopPropagation，以免阻断 GraphView 的拖拽/选择等交互处理
                }
            });
        }

        public void Dispose()
        {
            
        }
    }
    internal class EcsSystemMethod
    {
        public List<temp> In = new();
        public List<temp> Out = new();
        public List<temp> Change = new();
        public List<temp> AnyChange = new();
        public List<temp> Update = new();
        public List<temp> KVWatcher = new();
        public List<temp> EventWatcher = new();
        public class temp
        {
            public string Method;
            public string Parameter;
        }

        public void Get(SObject obj)
        {
            if (obj == null) return;
            addView(obj._In, In);
            addView(obj._Out, Out);
            addView(obj._Other.FindAll(t => t.type == SystemType.Change), Change);
            addView(obj._Other.FindAll(t => t.type == SystemType.AnyChange), AnyChange);
            addView(obj._Other.FindAll(t => t.type == SystemType.Update), Update);
            addView(obj._Other.FindAll(t => t.type == SystemType.KvWatcher), KVWatcher);
            addView(obj._EventWatcher, EventWatcher);
        }
        void addView(List<ComponentFilter> ds, List<temp> ret)
        {
            foreach (var d in ds)
                foreach (var d2 in (IList)d.system.GetActions())
                    addToList(ret, ((Delegate)d2).Method);
        }
        void addView(HashSet<__SystemHandle> ds, List<temp> ret)
        {
            foreach (var d in ds)
                foreach (var d2 in (IList)d.GetActions())
                    addToList(ret, ((Delegate)d2).Method);
        }

        void addToList(List<temp> ret, MethodInfo d)
        {
            temp s = new();
            var method = d;

            s.Method = $"{d.ReflectedType.Assembly.GetName().Name}:{method.ReflectedType.Name}:{method.Name}";
            s.Parameter = GetTypeName(method.GetParameters());

            ret.Add(s);
        }
        static StringBuilder str = new(100);
        static string GetTypeName(ParameterInfo[] ps)
        {
            str.Clear();
            for (int i = 0; i < ps.Length; i++)
            {
                str.Append(GetTypeName(ps[i].ParameterType));
                if (i < ps.Length - 1)
                    str.Append("+");
            }
            return str.ToString();
        }
        static string GetTypeName(Type type)
        {
            string name;
            if (type.IsGenericType)
            {
                name = type.Name[..^2];
                name += "<";
                foreach (var item2 in type.GetGenericArguments())
                    name += $"{GetTypeName(item2)},";
                name = name[..^1];
                name += ">";
            }
            else
                name = type.Name;
            return name;
        }
    }
}


