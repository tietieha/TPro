// #define POLYBRUSH_DEBUG

using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Polybrush;
using Debug = UnityEngine.Debug;

namespace UnityEditor.Polybrush
{
    /// <summary>
    /// Splat texture painter brush mode.
    /// Similar to BrushModePaint, except it packs blend information into Splat Texture
    /// </summary>
    internal class BrushModeSplatTexture : BrushModeTex
    {
        static class Styles
        {
            internal static readonly string[] k_TexIndex =
            {
                "Control1",
                "Control2",
            };
        }

        static Color BlackAlpah0 = new Color(0, 0, 0, 0);

        enum PanelView
        {
            Paint,
            Configuration,
        }

        class EditableObjectData
        {
            public EditableObject CacheTarget;
            public List<Material> CacheMaterials;
            public MixInfo mixInfo;
        }

        internal SplatWeight brushColor = null;


        [SerializeField] int m_SelectedAttributeIndex = 0;

        int selectedAttributeIndex
        {
            get { return m_SelectedAttributeIndex; }
            set { m_SelectedAttributeIndex = Mathf.Clamp(value, 0, meshAttributes.Length - 1); }
        }

        int selectedTexChannel = 0;

        bool m_LikelySupportsTextureBlending = true;

        AttributeLayoutContainer m_MeshAttributesContainer = null;
        List<AttributeLayoutContainer> m_MeshAttributesContainers = new List<AttributeLayoutContainer>();

        string[] m_AvailableMaterialsAsString = { };
        EditableObject m_MainCacheTarget = null;
        List<Material> m_MainCacheMaterials = new List<Material>();

        Dictionary<EditableObject, EditableObjectData> m_EditableObjectsData =
            new Dictionary<EditableObject, EditableObjectData>();

        internal int m_CurrentMeshACIndex = 0;

        PanelView m_CurrentPanelView = PanelView.Paint;

        MeshCollider m_MeshCollider;
        bool m_NeedRemoveCollider;

        internal AttributeLayout[] meshAttributes
        {
            get { return m_MeshAttributesContainer != null ? m_MeshAttributesContainer.attributes : null; }
        }

        internal List<AttributeLayout> controlAttributes = new List<AttributeLayout>();
        internal List<AttributeLayout> baseAttributes = new List<AttributeLayout>();

        // The message that will accompany Undo commands for this brush.  Undo/Redo is handled by PolyEditor.
        internal override string UndoMessage
        {
            get { return "Paint Brush"; }
        }

        protected override string ModeSettingsHeader
        {
            get { return "Texture Paint Settings"; }
        }

        protected override string DocsLink
        {
            get { return PrefUtility.documentationTextureBrushLink; }
        }

        internal override void OnEnable()
        {
            base.OnEnable();

            m_CurrentPanelView = PanelView.Paint;

            m_LikelySupportsTextureBlending = false;
            m_MeshAttributesContainer = null;
            brushColor = null;

            RebuildMaterialCaches();

            if (meshAttributes != null)
                OnMaterialSelected();

            foreach (GameObject go in Selection.gameObjects)
                m_LikelySupportsTextureBlending = CheckForTextureBlendSupport(go);
        }

        internal override bool SetDefaultSettings()
        {
            return true;
        }

        // Inspector GUI shown in the Editor window.  Base class shows BrushSettings by default
        internal override void DrawGUI(BrushSettings brushSettings)
        {
            base.DrawGUI(brushSettings);

            GUILayout.Space(4);

            // Selection dropdown for material (for submeshes)
            if (m_AvailableMaterialsAsString.Count() > 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.LabelField("Material", GUILayout.Width(60));
                if (m_CurrentPanelView == PanelView.Configuration)
                    GUI.enabled = false;

                m_CurrentMeshACIndex =
                    EditorGUILayout.Popup(m_CurrentMeshACIndex, m_AvailableMaterialsAsString, "Popup");

                if (m_CurrentPanelView == PanelView.Configuration)
                    GUI.enabled = true;

                // Buttons to switch between Paint and Configuration views
                if (m_CurrentPanelView == PanelView.Paint && GUILayout.Button("Configure", GUILayout.Width(70)))
                    OpenConfiguration();
                else if (m_CurrentPanelView == PanelView.Configuration)
                {
                    if (GUILayout.Button("Revert", GUILayout.Width(70)))
                        CloseConfiguration(false);
                    if (GUILayout.Button("Save", GUILayout.Width(70)))
                    {
                        CloseConfiguration(true);
                        GUIUtility.ExitGUI();
                    }
                }

                if (EditorGUI.EndChangeCheck())
                {
                    m_MeshAttributesContainer = m_MeshAttributesContainers[m_CurrentMeshACIndex];
                    OnMaterialSelected();
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            if (m_CurrentPanelView == PanelView.Paint)
            {
                DrawGUIPaintView();
            }
            else if (m_CurrentPanelView == PanelView.Configuration)
            {
                Material selectedMat = m_MainCacheMaterials[m_CurrentMeshACIndex];

                string[] names = selectedMat.GetTexturePropertyNames();

                using (new GUILayout.VerticalScope())
                {
                    for (int i = 0; i < names.Length; ++i)
                    {
                        string n = names[i];
                        if (selectedMat.HasProperty(n))
                            DrawConfigurationPanel(GetPropertyInfo(n), n, selectedMat);
                    }
                }
            }
        }

        struct MaterialPropertyInfo
        {
            public string PropertyName;
            public bool LinkedToAttributesLayout;
            public bool IsVisible;
        }

        List<MaterialPropertyInfo> materialPropertiesCache = new List<MaterialPropertyInfo>();

        AttributeLayoutContainer m_LoadedAttributes = null;

        private MaterialPropertyInfo GetPropertyInfo(string name)
        {
            MaterialPropertyInfo res = default(MaterialPropertyInfo);
            foreach (MaterialPropertyInfo p in materialPropertiesCache)
            {
                if (p.PropertyName == name)
                {
                    res = p;
                    break;
                }
            }

            return res;
        }

        private void UpdatePropertyInfo(MaterialPropertyInfo pUpdate)
        {
            int index = materialPropertiesCache.FindIndex(0, p => p.PropertyName == pUpdate.PropertyName);
            if (index >= 0)
                materialPropertiesCache[index] = pUpdate;
        }

        private void OnMaterialSelected()
        {
            Material selectedMat = m_MainCacheMaterials[m_CurrentMeshACIndex];
            string[] names = selectedMat.GetTexturePropertyNames();

            materialPropertiesCache.Clear();

            foreach (string n in names)
            {
                if (selectedMat.HasProperty(n))
                {
                    materialPropertiesCache.Add(new MaterialPropertyInfo()
                    {
                        PropertyName = n,
                        LinkedToAttributesLayout = false,
                        IsVisible = true
                    });
                }
            }
        }

        private void OpenConfiguration()
        {
            m_CurrentPanelView = PanelView.Configuration;

            if (m_MainCacheMaterials == null || m_CurrentMeshACIndex < 0 ||
                m_CurrentMeshACIndex >= m_MainCacheMaterials.Count)
                return;
            Material mat = m_MainCacheMaterials[m_CurrentMeshACIndex];
            Shader shader = mat.shader;

            if (ShaderMetaDataUtility.IsValidShader(shader))
            {
#pragma warning disable 0618
                // Data conversion between Polybrush Beta and Polybrush 1.0.
                string path = ShaderMetaDataUtility.FindPolybrushMetaDataForShader(shader);
                if (!string.IsNullOrEmpty(path))
                    ShaderMetaDataUtility.ConvertMetaDataToNewFormat(shader);
#pragma warning restore 0618

                m_LoadedAttributes = ShaderMetaDataUtility.LoadShaderMetaData(shader);
            }
        }

        private void CloseConfiguration(bool saveOnDisk)
        {
            if (saveOnDisk)
            {
                Material mat = m_MainCacheMaterials[m_CurrentMeshACIndex];
                Shader shader = mat.shader;

                ShaderMetaDataUtility.SaveShaderMetaData(shader, m_LoadedAttributes);
                foreach (GameObject go in Selection.gameObjects)
                    m_LikelySupportsTextureBlending = CheckForTextureBlendSupport(go);
            }

            m_LoadedAttributes = null;
            m_CurrentPanelView = PanelView.Paint;
        }

        private void DrawGUIPaintView()
        {
            if (meshAttributes != null)
            {
                RefreshPreviewTextureCache();
                int prevSelectedAttributeIndex = selectedAttributeIndex;

                SplatWeightEditor.OnInspectorGUI(-1, ref brushColor, controlAttributes.ToArray());

                selectedAttributeIndex =
                    SplatWeightEditor.OnInspectorGUI(selectedAttributeIndex, ref brushColor, baseAttributes.ToArray());

                if (prevSelectedAttributeIndex != selectedAttributeIndex)
                    SetBrushColorWithAttributeIndex(selectedAttributeIndex);

#if POLYBRUSH_DEBUG
				GUILayout.BeginHorizontal();

				GUILayout.FlexibleSpace();

				if(GUILayout.Button("MetaData", EditorStyles.miniButton))
				{
					Debug.Log(meshAttributes.ToString("\n"));

					string str = EditorUtility.FindPolybrushMetaDataForShader(meshAttributesContainer.shader);

					if(!string.IsNullOrEmpty(str))
					{
						TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);

						if(asset != null)
							EditorGUIUtility.PingObject(asset);
						else
							Debug.LogWarning("No MetaData found for Shader \"" + meshAttributesContainer.shader.name + "\"");
					}
					else
					{
						Debug.LogWarning("No MetaData found for Shader \"" + meshAttributesContainer.shader.name + "\"");
					}
				}

				GUILayout.EndHorizontal();

				GUILayout.Space(4);

				if(GUILayout.Button("rebuild  targets"))
					RebuildColorTargets(brushColor, brushSettings.strength);


				GUILayout.Label(brushColor != null ? brushColor.ToString() : "brush color: null\n");
#endif
            }
            else
            {
                if (!m_LikelySupportsTextureBlending)
                {
                    EditorGUILayout.HelpBox(
                        "It doesn't look like any of the materials on this object support texture blending!\n\nSee the readme for information on creating custom texture blend shaders.",
                        MessageType.Warning);
                }
            }
        }

        private void DrawConfigurationPanel(MaterialPropertyInfo guiInfo, string propertyName, Material mat)
        {
            using (new GUILayout.HorizontalScope("box"))
            {
                using (new GUILayout.VerticalScope())
                {
                    EditorGUI.BeginChangeCheck();
                    using (new GUILayout.HorizontalScope())
                    {
                        if (!m_LoadedAttributes.HasAttributes(propertyName))
                        {
                            GUILayout.Label(propertyName, GUILayout.Width(100));
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("Create attributes", GUILayout.Width(120), GUILayout.Height(16)))
                            {
                                AttributeLayout newAttr = new AttributeLayout();
                                newAttr.propertyTarget = propertyName;
                                m_LoadedAttributes.AddAttribute(newAttr);
                            }
                        }
                        else
                        {
                            guiInfo.IsVisible = EditorGUILayout.Foldout(guiInfo.IsVisible, propertyName, true);
                            if (GUILayout.Button("Erase attributes", GUILayout.ExpandWidth(true),
                                    GUILayout.MinWidth(60), GUILayout.MaxWidth(120), GUILayout.Height(16)))
                                m_LoadedAttributes.RemoveAttribute(propertyName);
                        }
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        UpdatePropertyInfo(guiInfo);
                        RefreshPreviewTextureCache();
                    }

                    if (m_LoadedAttributes.HasAttributes(propertyName) && guiInfo.IsVisible)
                    {
                        AttributeLayout attr = m_LoadedAttributes.GetAttributes(propertyName);

                        EditorGUILayout.Space();

                        using (new GUILayout.HorizontalScope())
                        {
                            using (new GUILayout.VerticalScope(GUILayout.Width(70), GUILayout.ExpandWidth(false)))
                            {
                                Texture tex = mat.GetTexture(propertyName);
                                EditorGUI.DrawPreviewTexture(
                                    EditorGUILayout.GetControlRect(GUILayout.Width(64), GUILayout.Height(64)),
                                    (tex != null) ? tex : Texture2D.blackTexture);
                            }

                            using (new GUILayout.VerticalScope())
                            {
                                GUILayout.Label("Is Control Texture");
                                GUILayout.Label("TexIndex");
                                if (!attr.isControlTexture)
                                {
                                    GUILayout.Label("Channel");
                                    // GUILayout.Label("Range");
                                    // GUILayout.Label("Group");
                                    GUILayout.Label("Is Base Texture");
                                }
                            }

                            GUILayout.FlexibleSpace();

                            EditorGUI.BeginChangeCheck();
                            using (new GUILayout.VerticalScope())
                            {
                                attr.isControlTexture = EditorGUILayout.Toggle(attr.isControlTexture);
                                // Channel selection
                                attr.texIndex = EditorGUILayout.Popup(attr.texIndex, Styles.k_TexIndex,
                                    GUILayout.Width(140));
                                if (!attr.isControlTexture)
                                {
                                    // Index selection
                                    attr.index = (ComponentIndex) GUILayout.Toolbar((int) attr.index,
                                        ComponentIndexUtility.ComponentIndexPopupDescriptions, GUILayout.Width(140));

                                    // // Value range
                                    // attr.range = EditorGUILayout.Vector2Field("", attr.range, GUILayout.Width(140));
                                    //
                                    // // Group selection
                                    // attr.mask = EditorGUILayout.Popup(attr.mask, AttributeLayout.DefaultMaskDescriptions, GUILayout.Width(140));

                                    attr.isBaseTexture = EditorGUILayout.Toggle(attr.isBaseTexture);
                                }
                            }

                            if (EditorGUI.EndChangeCheck())
                            {
                                RefreshPreviewTextureCache();
                            }
                        }
                    }
                }
            }
        }

        internal override void OnBrushSettingsChanged(BrushTarget target, BrushSettings settings)
        {
            // base.OnBrushSettingsChanged(target, settings);
            // RebuildColorTargets(target?.editableObject, brushColor, settings.strength);
            // RebuildColorTargets(brushColor, settings.strength);
        }

        /// <summary>
        /// Test a gameObject and it's mesh renderers for compatible shaders, and if one is found
        /// load it's attribute data into meshAttributes.
        /// </summary>
        /// <param name="go">The GameObject being checked for texture blend support</param>
        /// <returns></returns>
        internal bool CheckForTextureBlendSupport(GameObject go)
        {
            bool supports = false;
            var materials = Util.GetMaterials(go);
            m_MeshAttributesContainers.Clear();
            Material mat;
            List<int> indexes = new List<int>();
            for (int i = 0; i < materials.Count; i++)
            {
                mat = materials[i];
                if (ShaderMetaDataUtility.IsValidShader(mat.shader))
                {
                    AttributeLayoutContainer detectedMeshAttributes =
                        ShaderMetaDataUtility.LoadShaderMetaData(mat.shader);
                    {
                        if (detectedMeshAttributes != null)
                        {
                            m_MeshAttributesContainers.Add(detectedMeshAttributes);
                            indexes.Add(i);
                            m_MainCacheMaterials.Add(mat);
                            supports = true;
                        }
                    }
                }
            }

            if (supports)
            {
                m_MeshAttributesContainer = m_MeshAttributesContainers.First();
                foreach (int i in indexes)
                    ArrayUtility.Add<string>(ref m_AvailableMaterialsAsString, materials[i].name);
            }

            if (meshAttributes == null)
                supports = false;

            return supports;
        }

        internal void SetBrushColorWithAttributeIndex(int index)
        {
            if (baseAttributes.Count > 0)
                selectedTexChannel = baseAttributes[index].texIndex * 4 + (int) baseAttributes[index].index;

            // if(	brushColor == null ||
            // 	meshAttributes == null)
            // 	return;
            //
            //          brushColor.SetAttributeValue(meshAttributes[index], meshAttributes[index].max);
        }

        // Called when the mouse begins hovering an editable object.
        internal override void OnBrushEnter(EditableObject target, BrushSettings settings)
        {
            base.OnBrushEnter(target, settings);

            if (target.originalMesh == null)
                return;

            MeshCollider collider = target.gameObjectAttached.GetComponent<MeshCollider>();
            if (collider == null)
            {
                m_NeedRemoveCollider = true;
                m_MeshCollider = target.gameObjectAttached.AddComponent<MeshCollider>();
            }

            bool refresh = (m_MainCacheTarget != null && !m_MainCacheTarget.Equals(target)) ||
                           m_MainCacheTarget == null;

            if (m_MainCacheTarget != null && m_MainCacheTarget.Equals(target))
            {
                var targetMaterials = target.gameObjectAttached.GetMaterials();
                refresh = !targetMaterials.SequenceEqual(m_MainCacheMaterials);
            }

            if (refresh)
            {
                SetActiveObject(target);
                RebuildMaterialCaches();
                PolybrushEditor.instance.Repaint();
            }
            // if (m_LikelySupportsTextureBlending && (brushColor == null || !brushColor.MatchesAttributes(meshAttributes)))
            // {
            //     brushColor = new SplatWeight(SplatWeight.GetChannelMap(meshAttributes));
            //     SetBrushColorWithAttributeIndex(selectedAttributeIndex);
            // }

            RefreshPreviewTextureCache();

            // RebuildColorTargets(target, brushColor, settings.strength);
            // control 贴图
            RebuildTexMixInfo(target);
            if (m_LikelySupportsTextureBlending)
            {
                SetBrushColorWithAttributeIndex(selectedAttributeIndex);
            }
        }

        void RebuildMaterialCaches()
        {
            ArrayUtility.Clear(ref m_AvailableMaterialsAsString);
            m_CurrentMeshACIndex = 0;
            m_MainCacheMaterials.Clear();
            if (m_MainCacheTarget == null)
                return;
            m_MeshAttributesContainer = null;
            m_CurrentMeshACIndex = 0;
            m_LikelySupportsTextureBlending = CheckForTextureBlendSupport(m_MainCacheTarget.gameObjectAttached);
        }

        // Called whenever the brush is moved.  Note that @target may have a null editableObject.
        internal override void OnBrushMove(BrushTarget target, BrushSettings settings)
        {
            base.OnBrushMove(target, settings);

            if (!Util.IsValid(target) || !m_LikelySupportsTextureBlending || meshAttributes.Length == 0)
                return;

            if (!m_EditableObjectsData.ContainsKey(target.editableObject))
                return;

            var data = m_EditableObjectsData[target.editableObject];

            if (data.mixInfo == null)
                return;

            if (data.mixInfo.mixMap == null)
                return;

            if (!data.CacheMaterials.Contains(m_MainCacheMaterials[m_CurrentMeshACIndex]))
                return;

            bool invert = settings.isUserHoldingControl;
            data.mixInfo.ResetMixMap();
            PreViewPaint(target.textureCoord, data.mixInfo, settings);
        }

        // Called when the mouse exits hovering an editable object.
        internal override void OnBrushExit(EditableObject target)
        {
            base.OnBrushExit(target);

            if (!m_EditableObjectsData.ContainsKey(target))
                return;

            var data = m_EditableObjectsData[target];
            if (data.mixInfo != null)
                data.mixInfo.SaveMixMap();

            if (m_MainCacheTarget != null && data.CacheTarget.Equals(m_MainCacheTarget))
                m_MainCacheTarget = null;

            m_EditableObjectsData.Remove(target);

            if (m_MeshCollider != null && m_NeedRemoveCollider)
                DestroyImmediate(m_MeshCollider);
        }

        // Called every time the brush should apply itself to a valid target.  Default is on mouse move.
        internal override void OnBrushApply(BrushTarget target, BrushSettings settings)
        {
            if (!Util.IsValid(target) || !m_LikelySupportsTextureBlending || meshAttributes.Length == 0)
                return;

            var data = m_EditableObjectsData[target.editableObject];
            // data.SplatCurrent.CopyTo(data.SplatCache);
            // data.SplatCache.Apply(target.editableObject.editMesh);

            // MeshChannel channelsChanged =
            //     MeshChannel.Color | MeshChannel.Tangent | MeshChannel.UV0 | MeshChannel.UV2 | MeshChannel.UV3 | MeshChannel.UV4;
            // target.editableObject.modifiedChannels |= channelsChanged;
            // base.OnBrushApply(target, settings);

            //
            // if(!m_EditableObjectsData.ContainsKey(target.editableObject))
            //     return;
            if (data.mixInfo == null)
                return;

            if (data.mixInfo.mixMap == null)
                return;

            if (!data.CacheMaterials.Contains(m_MainCacheMaterials[m_CurrentMeshACIndex]))
                return;

            // 画贴图
            Paint(target.textureCoord, data.mixInfo, settings);
            PolybrushEditor.instance.Repaint();
        }

        // set mesh splat_current back to their original state before registering for undo
        internal override void RegisterUndo(BrushTarget brushTarget)
        {
            //          if(!m_EditableObjectsData.ContainsKey(brushTarget.editableObject))
            //              return;
            //
            //          var data = m_EditableObjectsData[brushTarget.editableObject];
            // if(data.SplatCache != null)
            // {
            // 	data.SplatCache.Apply(brushTarget.editableObject.editMesh);
            // 	brushTarget.editableObject.ApplyMeshAttributes();
            // }
            //
            // base.RegisterUndo(brushTarget);
        }

        internal override void UndoRedoPerformed(List<GameObject> modified)
        {
            // base.UndoRedoPerformed(modified);
            // foreach(var data in m_EditableObjectsData)
            //     RebuildCaches(data.Value);
        }

        internal override void DrawGizmos(BrushTarget target, BrushSettings settings)
        {
            base.DrawGizmos(target, settings);
        }

        private void Paint(Vector2 uv, MixInfo mixInfo, BrushSettings brushSettings)
        {
            if (mixInfo.mixMap == null)
                return;

            mixInfo.ResetAll();

            // 提前计算常用值
            int texWidth = mixInfo.mixMap.width;
            int texHeight = mixInfo.mixMap.height;
            float brushSize = brushSettings.texBrushSize;

            int centerX = Mathf.RoundToInt(uv.x * texWidth);
            int centerY = Mathf.RoundToInt(uv.y * texHeight);
            int halfSize = Mathf.RoundToInt(brushSize / 2);
            int sizeMod = Mathf.RoundToInt(brushSize % 2);

            int startX = Mathf.Clamp(centerX - halfSize, 0, texWidth - 1);
            int startY = Mathf.Clamp(centerY - halfSize, 0, texHeight - 1);
            int endX = Mathf.Clamp(centerX + halfSize + sizeMod, 0, texWidth);
            int endY = Mathf.Clamp(centerY + halfSize + sizeMod, 0, texHeight);

            int width = endX - startX;
            int height = endY - startY;

            // 只获取需要修改的像素区域
            Color[] pixels = mixInfo.mixMap.GetPixels(startX, startY, width, height, 0);
            Color targetColor = brushSettings.GetColor(selectedTexChannel);
            int totalPixels = width * height;

            // 优化循环计算
            int originOffsetX = startX - (centerX - halfSize + sizeMod);
            int originOffsetY = startY - (centerY - halfSize + sizeMod);

            // 并行处理像素（适用于大画笔）
            if (totalPixels > 128) // 设置合适的阈值
            {
                Parallel.For(0, totalPixels, i =>
                {
                    int y = i / width;
                    int x = i % width;
                    ProcessPixel(ref pixels[i], x, y, originOffsetX, originOffsetY, targetColor, brushSettings);
                });
            }
            else // 小区域使用串行处理
            {
                for (int i = 0; i < totalPixels; i++)
                {
                    int y = i / width;
                    int x = i % width;
                    ProcessPixel(ref pixels[i], x, y, originOffsetX, originOffsetY, targetColor, brushSettings);
                }
            }

            // 一次性设置所有像素
            mixInfo.mixMap.SetPixels(startX, startY, width, height, pixels);

            // 异步应用纹理更改
            mixInfo.mixMap.Apply(false);
        }

// 独立的像素处理方法
        private void ProcessPixel(ref Color pixel, int x, int y,
            int offsetX, int offsetY,
            Color targetColor, BrushSettings settings)
        {
            int ix = x + offsetX;
            int iy = y + offsetY;

            float blendFactor = settings.GetStrengthInt(ix, iy);

            switch (settings.texBrushBlendMode)
            {
                case TEX_BLEND_MODE.REPLACE:
                    pixel = Color.Lerp(pixel, targetColor, blendFactor);
                    break;

                case TEX_BLEND_MODE.MIXED:
                    Color blendValue = Color.Lerp(BlackAlpah0, targetColor, blendFactor);
                    pixel = settings.texBrushEraser ? pixel - blendValue : pixel + blendValue;
                    break;
            }
        }

        internal void PreViewPaint(Vector2 uv, MixInfo mixInfo, BrushSettings brushSettings)
        {
            if (mixInfo.mixMap == null)
            {
                return;
            }

            float xCenterNormalized = uv.x;
            float yCenterNormalized = uv.y;

            //int size = (int)brushSettings.texbrushsi;
            int num = Mathf.RoundToInt(xCenterNormalized * mixInfo.mixMap.width);
            int num2 = Mathf.RoundToInt(yCenterNormalized * mixInfo.mixMap.height);
            int num3 = Mathf.RoundToInt(brushSettings.texBrushSize / 2);
            int num4 = Mathf.RoundToInt(brushSettings.texBrushSize % 2);
            int x = Mathf.Clamp(num - num3, 0, mixInfo.mixMap.width - 1);
            int y = Mathf.Clamp(num2 - num3, 0, mixInfo.mixMap.height - 1);
            int num7 = Mathf.Clamp((num + num3) + num4, 0, mixInfo.mixMap.width);
            int num8 = Mathf.Clamp((num2 + num3) + num4, 0, mixInfo.mixMap.height);
            int width = num7 - x;
            int height = num8 - y;
            mixInfo.realMixColor = mixInfo.mixMap.GetPixels(x, y, width, height, 0);
            Color[] srcPixels = mixInfo.mixMap.GetPixels(x, y, width, height, 0);
            Color targetColor = brushSettings.GetColor(selectedTexChannel);

            mixInfo.realMixX = x;
            mixInfo.realMixY = y;
            mixInfo.realMixWidth = width;
            mixInfo.realMixHeight = height;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int ix = (x + j) - ((num - num3) + num4);
                    int iy = (y + i) - ((num2 - num3) + num4);
                    int index = (i * width) + j;
                    float blendFactor = brushSettings.GetStrengthInt(ix, iy);
                    // srcPixels[index] = Color.Lerp(srcPixels[index], targetColor, blendFactor);
                    if (brushSettings.texBrushBlendMode == TEX_BLEND_MODE.REPLACE)
                    {
                        srcPixels[index] = Color.Lerp(srcPixels[index], targetColor, blendFactor);
                    }
                    else if (brushSettings.texBrushBlendMode == TEX_BLEND_MODE.MIXED)
                    {
                        if (brushSettings.texBrushEraser)
                        {
                            srcPixels[index] -= Color.Lerp(BlackAlpah0, targetColor, blendFactor);
                        }
                        else
                        {
                            srcPixels[index] += Color.Lerp(BlackAlpah0, targetColor, blendFactor);
                        }
                    }
                }
            }

            mixInfo.mixMap.SetPixels(x, y, width, height, srcPixels, 0);
            mixInfo.mixMap.Apply();
        }

        internal void RefreshPreviewTextureCache()
        {
            controlAttributes.Clear();
            baseAttributes.Clear();
            if (meshAttributes != null
                && m_MainCacheMaterials != null)
            {
                for (int i = 0; i < meshAttributes.Length; ++i)
                {
                    AttributeLayout attributes = meshAttributes[i];
                    Texture2D tex = (Texture2D) m_MainCacheMaterials[m_CurrentMeshACIndex]
                        .GetTexture(attributes.propertyTarget);
                    if (tex != null)
                    {
                        attributes.previewTexture = tex;

                        if (attributes.isControlTexture)
                            controlAttributes.Add(attributes);
                        else
                            baseAttributes.Add(attributes);
                    }
                }
            }
        }

        void RebuildTexMixInfo(EditableObject target)
        {
            if (target == null)
                return;

            if (meshAttributes == null)
                return;

            var data = m_EditableObjectsData[target];

            var attrib = meshAttributes[selectedAttributeIndex];
            if (m_MainCacheMaterials[m_CurrentMeshACIndex].GetTexture(attrib.propertyTarget) == null)
                return;
            var controlAttr = controlAttributes.FirstOrDefault(item => item.texIndex == attrib.texIndex);
            if (controlAttr != null)
            {
                if (data.mixInfo == null)
                    data.mixInfo = new MixInfo();
                data.mixInfo.SetMixMap(
                    m_MainCacheMaterials[m_CurrentMeshACIndex].GetTexture(controlAttr.propertyTarget) as Texture2D);
            }
        }

        void SetActiveObject(EditableObject activeObject)
        {
            m_MainCacheTarget = activeObject;

            EditableObjectData data;

            if (!m_EditableObjectsData.TryGetValue(activeObject, out data))
            {
                data = new EditableObjectData();
                m_EditableObjectsData.Add(activeObject, data);
            }

            data.CacheTarget = activeObject;
            data.CacheMaterials = activeObject.gameObjectAttached.GetMaterials();
        }
    }
}
