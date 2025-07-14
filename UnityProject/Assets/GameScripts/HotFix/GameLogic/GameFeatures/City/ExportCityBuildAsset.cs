using System;
using System.Collections;
using System.Collections.Generic;
using BitBenderGames;
using GameLogic.GameFeatures.City;
using Sirenix.OdinInspector;
using TEngine;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

[ExecuteAlways]
public class ExportCityBuildAsset : MonoBehaviour
{
    public enum FocusType
    {
        [LabelText("关闭")]
        Close,
        [LabelText("菜单时聚焦")]
        MenuFocus,
        [LabelText("界面时聚焦")]
        UpgradeFocus,
        [LabelText("名牌时聚焦")]
        HUDFocus,
    }
    
#if UNITY_EDITOR && ODIN_INSPECTOR
    [LabelText("种族"), ValueDropdown("FactionArray"), PropertyOrder(-1)] 
    public string Faction;

    private string[] FactionArray = new string[] 
    {
        "空",
        "城堡",
        "壁垒",
        "塔楼",
        "地狱",
        "墓园",
        "地下城",
        "据点",
        "要塞",
        "元素",
    };
    
    [LabelText("摄像机"), PropertyOrder(-1)] 
    public Transform Camera;
    
    [LabelText("UI摄像机"), PropertyOrder(-1)] 
    public Transform UICamera;
    
    [LabelText("建筑根节点"), PropertyOrder(-1)]
    public Transform BuildRoot;

    [LabelText("场景UI根节点"), PropertyOrder(-1)]
    public Transform WorldUIRoot;
    
    [LabelText("2DUI根节点"), PropertyOrder(-1)]
    public Transform UIRoot;
        
    [LabelText("建筑命名tag"), PropertyOrder(-1)]
    public string NameTag = "MainCity_";

    [LabelText("CityAsset文件"), PropertyOrder(-1)] 
    public CityBuildAsset BuildAsset;
    
    [Header("需要美术/策划调整的相关数据")]
    
    [LabelText("摄像机初始位置"), PropertyOrder(1)] 
    public Vector3 InitCameraPos;
    
    [LabelText("摄像机初始角度"), PropertyOrder(1)] 
    public Vector3 InitCameraEuler;
    
    [LabelText(("建筑名牌缩放基准值")), PropertyOrder(1)]
    public float HudScale = 0.15f;
    
    [LabelText("相机最左边边界X值"), PropertyOrder(1)]
    public float CameraBoundaryMinX;
    
    [LabelText("相机最右边边界X值"), PropertyOrder(1)]
    public float CameraBoundaryMaxX;
    
    [LabelText("场景Volume"), PropertyOrder(1)] 
    public Volume SceneVolume;

    private FocusType m_FocusType;
    private FocusType EnableFocusType
    {
        set
        {
            m_FocusType =  value;
            _UpdateFocusType();
        }
        get { return m_FocusType; }
    }
    
    #region 生命周期
    
    private void Update()
    {
        _CheckModelRoot();
        _UpdateMenuOrHudTag();
    }
    
    #endregion

    [PropertySpace(10)]
    [LabelText("当前操作的建筑"), PropertyOrder(20)] 
    public GameObject FocusCity;
    
    #region 相机相关 40 - 49

    [Title("修复工具")]
    [PropertySpace(10)]
    
    [Button("相机归位，界面归位", ButtonSizes.Large), PropertyOrder(40)]
    public void Init()
    {
        Camera.position = InitCameraPos;
        Camera.eulerAngles = InitCameraEuler;
        EnableFocusType = FocusType.Close;
    }
    
    [Button("修正不可修改节点的数据", ButtonSizes.Large), PropertyOrder(40)]
    public void FixBuildsTransform()
    {
        foreach (Transform child in BuildRoot)
        {
            if (!child.name.ToLower().Contains(NameTag.ToLower()))
            {
                continue;
            }

            // container 只能挪位置
            if (child.transform.localEulerAngles != Vector3.zero)
            {
                child.transform.localEulerAngles = Vector3.zero;

                return;
            }

            if (child.transform.localScale != Vector3.one)
            {
                child.transform.localScale = Vector3.one;


                return;
            }

            // modelRoot 都不能改
            var modelRoot = child.Find("modelRoot");
            if (modelRoot.transform.localPosition != Vector3.zero || modelRoot.transform.eulerAngles != Vector3.zero ||
                modelRoot.transform.localScale != Vector3.one)
            {
                modelRoot.transform.localPosition = Vector3.zero;
                modelRoot.transform.localEulerAngles = Vector3.zero;
                modelRoot.transform.localScale = Vector3.one;


                return;
            }

            // 模型 只能改缩放和旋转
            var model = child.Find("modelRoot/" + child.name);
            if (model != null && model.transform.localPosition != Vector3.zero)
            {
                model.transform.localPosition = Vector3.zero;


                return;
            }

            //hudRoot 都不能改
            var hudRoot = child.Find("hudTag");
            if (hudRoot.transform.localEulerAngles != Vector3.zero ||
                hudRoot.transform.localScale != Vector3.one)
            {
                hudRoot.transform.localEulerAngles = Vector3.zero;
                hudRoot.transform.localScale = Vector3.one;

                return;
            }
        }
    }
    #endregion
    
    #region 升级相关 50 - 59

    private Color upColor = new Color(0.4f, 0.8f, 1f);
    [Title("升级相关操作")]
    [PropertySpace(10)]
    [Button("调整升级时聚焦位置", ButtonSizes.Large), PropertyOrder(51)]
    [GUIColor("upColor")]
    public void SelectFocusTag()
    {
        if (FocusCity == null)
        {
            EditorUtility.DisplayDialog("警告", "未设置当前调整的建筑", "确认");
            return;
        }

        EnableFocusType = FocusType.UpgradeFocus;
        
        var focusTag = FocusCity.transform.Find("focusTag");
        if (focusTag == null)
        {
            var focusGo = new GameObject("focusTag");
            focusGo.transform.SetParent(FocusCity.transform);
            var focusPos = FocusCity.transform.position;
            var distance = Vector3.Distance(InitCameraPos, focusPos);
            var dir = Vector3.Normalize(InitCameraPos - focusPos);
            var targetPos = InitCameraPos - dir * distance * 0.7f;
            Camera.position = targetPos;
            Camera.eulerAngles = InitCameraEuler;
            focusGo.transform.position = targetPos;
            focusGo.transform.eulerAngles = InitCameraEuler;

            focusTag = focusGo.transform;
            
            EditorUtility.DisplayDialog("警告", "当前建筑未找到focusTag节点，已使用默认设置", "确认");
        }
        else
        {
            Camera.position = focusTag.position;
            Camera.eulerAngles = focusTag.eulerAngles;
        }
        
        EditorGUIUtility.PingObject(focusTag.gameObject);
        Selection.activeGameObject = Camera.gameObject;
    }
    
    [Button("保存升级时聚焦数据 - （相机数据存储到focusTag上）", ButtonSizes.Large), PropertyOrder(52)]
    [GUIColor("upColor")]
    public void SaveFocusTag()
    {
        if (FocusCity == null)
        {
            EditorUtility.DisplayDialog("警告", "未设置当前调整的建筑", "确认");
            return;
        }

        var focusTag = FocusCity.transform.Find("focusTag");
        
        focusTag.position = Camera.position;
        focusTag.eulerAngles = Camera.eulerAngles;
    }

    [Button("调整升级时景深参数", ButtonSizes.Large), PropertyOrder(58)]
    [GUIColor("upColor")]
    public void SelectVolume()
    {
        if (FocusCity == null)
        {
            EditorUtility.DisplayDialog("警告", "未设置当前调整的建筑", "确认");
            return;
        }

        if (SceneVolume == null)
        {
            EditorUtility.DisplayDialog("警告", "未设置当前调整的建筑", "确认");
            return;
        }
        
        EnableFocusType = FocusType.UpgradeFocus;
        
        EditorGUIUtility.PingObject(SceneVolume.gameObject);
    }
    
    [Button("保存升级时景深参数", ButtonSizes.Large), PropertyOrder(59)]
    [GUIColor("upColor")]
    public void SaveDepthData()
    {
        if (FocusCity == null)
        {
            EditorUtility.DisplayDialog("警告", "未设置当前调整的建筑", "确认");
            return;
        }

        var focusTag = FocusCity.transform.Find("focusTag");
        var depthData = focusTag.GetOrAddComponent<DepthOfFieldDataMono>();
        SceneVolume.profile.TryGet<DepthOfField>(out var depthComponent);
        depthData.FocusDistance = depthComponent.focusDistance.value;
        depthData.FocalLength = depthComponent.focalLength.value;
        depthData.Aperture = depthComponent.aperture.value;
    }
    
    #endregion
    
    #region Menu相关 60 - 69

    private Color menuColor = new Color(0.78f, 0.63f, 0.42f);
    [Title("功能菜单相关操作")]
    [PropertySpace(10)]
    [Button("调整菜单聚焦节点", ButtonSizes.Large), PropertyOrder(67)]
    [GUIColor("menuColor")]
    public void SelectMenuFocusTag()
    {
        if (FocusCity == null)
        {
            EditorUtility.DisplayDialog("警告", "未设置当前调整的建筑", "确认");
            return;
        }

        EnableFocusType = FocusType.MenuFocus;
        
        var menuFocusTag = FocusCity.transform.Find("menuFocusTag");
        if (menuFocusTag == null)
        {
            var focusGo = new GameObject("menuFocusTag");
            focusGo.transform.SetParent(FocusCity.transform);
            
            menuFocusTag = focusGo.transform;
            menuFocusTag.localPosition = Vector3.zero;
            menuFocusTag.localEulerAngles = Vector3.zero;
            menuFocusTag.localScale = Vector3.one;
            
            Camera.position = InitCameraPos;
            Camera.eulerAngles = InitCameraEuler;

            
            EditorUtility.DisplayDialog("警告", "当前建筑未找到menuFocusTag节点，已使用默认设置", "确认");
        }
        else
        {
            Camera.position = menuFocusTag.position;
            Camera.eulerAngles = menuFocusTag.eulerAngles;
        }
        
        EditorGUIUtility.PingObject(menuFocusTag.gameObject); 
        Selection.activeGameObject = Camera.gameObject;
        
        var menuTag = FocusCity.transform.Find("menuTag");
        _RefreshMenuPanel(menuTag);
    }
    
    [Button("保存菜单聚焦位置 - (当前相机数据存到menuFocusTag上)", ButtonSizes.Large), PropertyOrder(68)]
    [GUIColor("menuColor")]
    public void SaveMenuFocusTag()
    {
        if (FocusCity == null)
        {
            EditorUtility.DisplayDialog("警告", "未设置当前调整的建筑", "确认");
            return;
        }
        
        var menuFocusTag = FocusCity.transform.Find("menuFocusTag");
        menuFocusTag.position = Camera.position;
        menuFocusTag.eulerAngles = Camera.eulerAngles;
    }
    
    [Button("调整功能菜单节点", ButtonSizes.Large), PropertyOrder(69)]
    [GUIColor("menuColor")]
    public void SelectMenuTag()
    {
        if (FocusCity == null)
        {
            EditorUtility.DisplayDialog("警告", "未设置当前调整的建筑", "确认");
            return;
        }

        EnableFocusType = FocusType.MenuFocus;
        
        var menuTag = FocusCity.transform.Find("menuTag");
        if (menuTag)
        {
            EditorGUIUtility.PingObject(menuTag.gameObject); 
        }
    }

    [Button("刷新菜单栏", ButtonSizes.Large), PropertyOrder(69)]
    [GUIColor("menuColor")]
    public void RefreshMenuFocusTag()
    {
        if (FocusCity == null)
        {
            EditorUtility.DisplayDialog("警告", "未设置当前调整的建筑", "确认");
            return;
        }
        
        EnableFocusType = FocusType.MenuFocus;
        
        var menuTag = FocusCity.transform.Find("menuTag");
        _RefreshMenuPanel(menuTag);
    }
    
    #endregion

    #region hud相关 70 - 79
    [Title("建筑名牌相关操作")]
    [PropertySpace(10)]
    [Button("调整名牌位置", ButtonSizes.Large), PropertyOrder(78)]
    public void SelectHudTag()
    {
        if (FocusCity == null)
        {
            EditorUtility.DisplayDialog("警告", "未设置当前调整的建筑", "确认");
            return;
        }

        Init();
        Camera.transform.position = new Vector3(FocusCity.transform.position.x, InitCameraPos.y, InitCameraPos.z);
        EnableFocusType = FocusType.HUDFocus;
        
        var hudTag = FocusCity.transform.Find("hudTag");
        if (hudTag)
        {
            EditorGUIUtility.PingObject(hudTag.gameObject); 
        }
    }
    
    [Button("创建/刷新建筑名牌", ButtonSizes.Large), PropertyOrder(79)]
    public void CreateBuildLabel()
    {
        if (WorldUIRoot == null || BuildRoot == null)
        {
            EditorUtility.DisplayDialog("警告", "请先选择建筑/UI根节点", "确认");
            return;
        }

        GameObject buildLabelPrefab =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameAssets/UI/Prefabs/CityBuilding/UIBuildHud.prefab");
        if (buildLabelPrefab == null)
        {
            EditorUtility.DisplayDialog("警告", "未找到建筑名牌预制体", "确认");
            return;
        }

        foreach (Transform child in BuildRoot)
        {
            if (!child.name.ToLower().Contains(NameTag.ToLower()))
            {
                continue;
            }
            
            var hudTag = child.Find("hudTag") as Transform;
            
            var id = _getBuildId(child.name);
            var hudName = "UIBuildHud_" + id;
            var hud = WorldUIRoot.Find(hudName) as Transform;
            if (hud == null)
            {
                hud = Instantiate(buildLabelPrefab, WorldUIRoot).transform;
                hud.name = hudName;
                
                hud.GetComponentInChildren<TextMeshProUGUI>().text = id;
            }
            
            var scale = HudScale * (child.transform.position.z - Camera.position.z) / Camera.position.z;
            hud.localScale = new Vector3(scale, scale, scale);
            hud.localEulerAngles = new Vector3(Camera.eulerAngles.x, Camera.eulerAngles.y, Camera.eulerAngles.z);
            hud.position = hudTag.position;
        }
    }

    #endregion
    
    #region CityAsset order 80 - 100
    
    [Title("城建数据相关操作")]
    [PropertySpace(10)]
    
    [Button("选择当前种族的CityAsset文件", ButtonSizes.Large), PropertyOrder(99)]
    public void SelectCityAsset()
    {
        string openPath = EditorUtility.OpenFilePanel("选择文件", "Assets/GameAssets/SoAsset/CityBuildAssets", "asset");
        if (!string.IsNullOrEmpty(openPath))
        {
            int subLen = Application.dataPath.Length - 6;
            string assetRelativePath = openPath.Substring(subLen, openPath.Length - subLen).Replace("\\", "/");
            var assets = AssetDatabase.LoadAssetAtPath(assetRelativePath, typeof(CityBuildAsset));
            if (assets != null)
            {
                BuildAsset = assets as CityBuildAsset;
            }
        }
    }
    
    [Button("Ping CityAsset文件", ButtonSizes.Large), PropertyOrder(99)]
    public void PingCityAsset()
    {
        if (BuildAsset != null)
        {
            EditorGUIUtility.PingObject(BuildAsset);
        }
    }
    
    [Button("保存建筑数据", ButtonSizes.Gigantic), PropertyOrder(100)]
    public void ExportCityBuildAssetData()
    {
        if (BuildAsset == null)
        {
            EditorUtility.DisplayDialog("警告", "请先选择导出的资源", "确认");
            return;
        }
        
        if (BuildAsset.BuildInfos == null)
        {
            BuildAsset.BuildInfos = new List<CityBuildAsset.BuildInfo>();
        }
        BuildAsset.BuildInfos.Clear();
        
        foreach (Transform child in BuildRoot)
        {
            if (!child.name.ToLower().Contains(NameTag.ToLower()))
            {
                continue;
            }

            
            CityBuildAsset.BuildInfo buildInfo = new CityBuildAsset.BuildInfo();

            buildInfo.BuildName = "Sid_" + child.name.Trim();
            buildInfo.ContainerPosX = child.localPosition.x;
            buildInfo.ContainerPosY = child.localPosition.y;
            buildInfo.ContainerPosZ = child.localPosition.z;

            var model = child.Find("modelRoot/" + child.name);
            if (model != null)
            {
                buildInfo.ModelEulerX = model.localEulerAngles.x;
                buildInfo.ModelEulerY = model.localEulerAngles.y;
                buildInfo.ModelEulerZ = model.localEulerAngles.z;
                buildInfo.ModelScaleX = model.localScale.x;
                buildInfo.ModelScaleY = model.localScale.y;
                buildInfo.ModelScaleZ = model.localScale.z;
            }
            
            var hudPath = "UIBuildHud_" + _getBuildId(child.name);
            var buildLabel = WorldUIRoot.Find(hudPath) as Transform;
            if (buildLabel)
            {
                buildInfo.HudPosX = buildLabel.position.x;
                buildInfo.HudPosY = buildLabel.position.y;
                buildInfo.HudPosZ = buildLabel.position.z;

                buildInfo.HudScaleX = buildLabel.localScale.x;
                buildInfo.HudScaleY = buildLabel.localScale.y;
                buildInfo.HudScaleZ = buildLabel.localScale.z;
                
                buildInfo.HudRotateX = buildLabel.localEulerAngles.x;
                buildInfo.HudRotateY = buildLabel.localEulerAngles.y;
                buildInfo.HudRotateZ = buildLabel.localEulerAngles.z;
            }
            
            var menuTag = child.Find("menuTag") as Transform;
            if (menuTag)
            {
                buildInfo.MenuTagPosX = menuTag.position.x;
                buildInfo.MenuTagPosY = menuTag.position.y;
                buildInfo.MenuTagPosZ = menuTag.position.z;
            }

            var focusTag = child.Find("focusTag");
            if (focusTag)
            {
                buildInfo.FocusPosX = focusTag.position.x;
                buildInfo.FocusPosY = focusTag.position.y;
                buildInfo.FocusPosZ = focusTag.position.z;
                
                buildInfo.FocusRotateX = focusTag.eulerAngles.x;
                buildInfo.FocusRotateY = focusTag.eulerAngles.y;
                buildInfo.FocusRotateZ = focusTag.eulerAngles.z;

                var depthData = focusTag.GetComponent<DepthOfFieldDataMono>();
                if (depthData)
                {
                    buildInfo.SetDepthData(depthData);
                }
            }
            
            var menuFocusTag = child.Find("menuFocusTag");
            if (menuFocusTag)
            {
                buildInfo.MenuFocusPosX = menuFocusTag.position.x;
                buildInfo.MenuFocusPosY = menuFocusTag.position.y;
                buildInfo.MenuFocusPosZ = menuFocusTag.position.z;
                
                buildInfo.MenuFocusRotateX = menuFocusTag.eulerAngles.x;
                buildInfo.MenuFocusRotateY = menuFocusTag.eulerAngles.y;
                buildInfo.MenuFocusRotateZ = menuFocusTag.eulerAngles.z;
            }
            
            BuildAsset.BuildInfos.Add(buildInfo);
        }

	    if(Camera != null)
	    {
	        BuildAsset.CameraPosition = InitCameraPos;
       	 	BuildAsset.CameraEuler = InitCameraEuler;
            
            var cameraComponent = Camera.GetComponent<Camera>();
            BuildAsset.CameraNearClip = cameraComponent.nearClipPlane;
            BuildAsset.CameraFarClip = cameraComponent.farClipPlane;
            BuildAsset.CameraFieldOfView = cameraComponent.fieldOfView;
            
            var touchCamera = Camera.GetComponent<MobileTouchCamera>();
            if (touchCamera != null)
            {
                BuildAsset.CameraBoundaryMinX = touchCamera.BoundaryMin.x;
                BuildAsset.CameraBoundaryMaxX = touchCamera.BoundaryMax.x;
            }
	    }

        
        EditorUtility.SetDirty(BuildAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    #endregion
    
    #region 私有方法
    private void _CheckModelRoot()
    {
        var selectGo = Selection.activeGameObject;

        if (selectGo == null)
        {
            return;
        }

        if (selectGo.transform.parent == null)
            return;
        
        if (selectGo.transform.parent.name == "Object")
        {
            var child = selectGo;
            // container 只能挪位置
            if (child.transform.localEulerAngles != Vector3.zero)
            {
                child.transform.localEulerAngles = Vector3.zero;

                EditorUtility.DisplayDialog("警告", "根节点不能有旋转！", "确认");
                return;
            }
            if (child.transform.localScale != Vector3.one)
            {
                child.transform.localScale = Vector3.one;
                EditorUtility.DisplayDialog("警告", "根节点不能有缩放！", "确认");
                return;
            }
        }
        
        if (selectGo.name == "modelRoot")
        {
            // modelRoot 都不能改
            var modelRoot = selectGo;
            if (modelRoot.transform.localPosition != Vector3.zero || modelRoot.transform.localEulerAngles != Vector3.zero || modelRoot.transform.localScale != Vector3.one)
            {
                modelRoot.transform.localPosition = Vector3.zero;
                modelRoot.transform.localEulerAngles = Vector3.zero;
                modelRoot.transform.localScale = Vector3.one;

                EditorUtility.DisplayDialog("警告", "modelRoot节点不可修改！", "确认");
                return;
            }
        }

        // 模型 只能改缩放和旋转
        if (selectGo.transform.parent.name == "modelRoot")
        {
            var model = selectGo;
            if (model != null && model.transform.localPosition != Vector3.zero)
            {
                model.transform.localPosition = Vector3.zero;
                EditorUtility.DisplayDialog("警告", "模型位置需要归零！", "确认");
                return;
            }
        }
        
        //hudRoot 都不能改
        if (selectGo.name == "hudTag")
        {
            var hudRoot = selectGo;
            if (hudRoot.transform.localEulerAngles != Vector3.zero || hudRoot.transform.localScale != Vector3.one)
            {
                hudRoot.transform.localEulerAngles = Vector3.zero;
                hudRoot.transform.localScale = Vector3.one;
                EditorUtility.DisplayDialog("警告", "hudTag节点只能修改位置！", "确认");
                return;
            }
        }
    }

    private void _RefreshMenuPanel(Transform menuTag)
    {
        if (menuTag == null)
        {
            return;
        }
        
        var menuPanel = UIRoot.Find("UIBuildingMenuPanel");
        var center = menuPanel.Find("center");
        var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.GetComponent<Camera>(), menuTag.position);
        var state= RectTransformUtility.ScreenPointToLocalPointInRectangle(menuPanel.GetComponent<RectTransform>(),
            screenPos, UICamera.GetComponent<Camera>(), out var uiPos);
        center.GetComponent<RectTransform>().anchoredPosition = uiPos;
    }
    
    private string _getBuildId(string name)
    {
        var strings = name.Split("_");
        var id = strings[strings.Length - 1];
        return id;
    }
    
    private void _UpdateFocusType()
    {
        var menuPanel = UIRoot.Find("UIBuildingMenuPanel");
        var upgradePanel = UIRoot.Find("UIBuildingControlPanel");
        if(m_FocusType == FocusType.MenuFocus)
        {
            if (FocusCity)
            {
                var menuTag = FocusCity.transform.Find("menuTag");
                if (menuTag)
                {
                    _RefreshMenuPanel(menuTag);
                }
            }
                
        }
        
        menuPanel.gameObject.SetActive(m_FocusType == FocusType.MenuFocus);
        upgradePanel.gameObject.SetActive(m_FocusType == FocusType.UpgradeFocus);
        SceneVolume.profile.TryGet<DepthOfField>(out var _component);
        _component.active = m_FocusType == FocusType.UpgradeFocus;
        WorldUIRoot.gameObject.SetActive(m_FocusType == FocusType.HUDFocus);
    }
    
    private void _UpdateMenuOrHudTag()
    {
        var selGo = Selection.activeGameObject;
        if (selGo)
        {
            if (selGo.name == "hudTag")
            {
                var build = selGo.transform.parent;
                var id = _getBuildId(build.name);
                var hudName = "UIBuildHud_" + id;
                var hudTag = selGo.transform;
                var hud = WorldUIRoot.Find(hudName) as Transform;
                var scale = HudScale * (build.transform.position.z - Camera.position.z) / Camera.position.z;
                hud.localScale = new Vector3(scale, scale, scale);
                hud.localEulerAngles = new Vector3(Camera.eulerAngles.x, Camera.eulerAngles.y, Camera.eulerAngles.z);
                hud.position = hudTag.position;
            }
        
            if (selGo.name == "menuTag")
            {
                var menuTag = selGo.transform;
                _RefreshMenuPanel(menuTag);
            }
        }
    }
    
    #endregion

#endif
}
