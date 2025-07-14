using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TEngine
{
    /// <summary>
    /// UI模块。
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class UIModule : Module
    {
        [SerializeField] private Transform m_InstanceRoot = null;

        [SerializeField] private Transform m_DefaultUI = null;

        [SerializeField] private bool m_dontDestroyUIRoot = true;

        [SerializeField] private Camera m_UICamera = null;

        private readonly List<UIWindow> _stack = new List<UIWindow>(100);

        public const int LAYER_DEEP = 2000;
        public const int WINDOW_DEEP = 100;
        public const int WINDOW_HIDE_LAYER = 2; // Ignore Raycast
        public const int WINDOW_SHOW_LAYER = 5; // UI

        /// <summary>
        /// UI根节点。
        /// </summary>
        public Transform UIRoot => m_InstanceRoot;

        /// <summary>
        /// 默认UI
        /// </summary>
        public Transform DefaultUI => m_DefaultUI;

        public static Transform UIRootStatic;

        /// <summary>
        /// UI根节点。
        /// </summary>
        public Camera UICamera => m_UICamera;

        private UIModuleImpl _uiModuleImpl;

        public override void Init()
        {
            base.Init();

            RootModule rootModule = ModuleSystem.GetModule<RootModule>();
            if (rootModule == null)
            {
                Log.Fatal("Base component is invalid.");
                return;
            }

            _uiModuleImpl = ModuleImpSystem.GetModule<UIModuleImpl>();
            _uiModuleImpl.Initialize(_stack);

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = new GameObject("UI Form Instances").transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }
            else if (m_dontDestroyUIRoot)
            {
                DontDestroyOnLoad(m_InstanceRoot.parent != null ? m_InstanceRoot.parent : m_InstanceRoot);
            }

            m_InstanceRoot.gameObject.layer = LayerMask.NameToLayer("UI");
            UIRootStatic = m_InstanceRoot;

            AdapterScaler();
        }

        private void Update()
        {
            if (_orientation != Screen.orientation)
            {
                _orientation = Screen.orientation;
                OnScreenOrientation?.Invoke();
            }
        }

        private void OnDestroy()
        {
            CloseAll();
            if (m_InstanceRoot != null && m_InstanceRoot.parent != null)
            {
                Destroy(m_InstanceRoot.parent.gameObject);
            }
        }

        #region 屏幕适配

        private ScreenOrientation _orientation;
        public ScreenOrientation Orientation {get{return _orientation;}}

        public delegate void ScreenOrientationHandler();
        public event ScreenOrientationHandler OnScreenOrientation;

        public void AdapterScaler()
        {
            var scaler = GetCanvasScaler();
            // 从 CanvasScaler 中读取参考分辨率
            Vector2 referenceResolution = scaler.referenceResolution;
            float screenRatio = (float)Screen.width / Screen.height;
            float uiRatio = referenceResolution.x / referenceResolution.y;
            // 根据屏幕宽高比调整 matchWidthOrHeight
            scaler.matchWidthOrHeight = screenRatio >= uiRatio ? 1f : 0f;
        }

        public CanvasScaler GetCanvasScaler()
        {
            return UIRoot.GetComponentInChildren<CanvasScaler>();
        }

        private Vector2 _screenSize;
        /// <summary>
        /// 屏幕尺寸
        /// </summary>
        /// <returns></returns>
        public Vector2 GetScreenSize()
        {
            if (_screenSize == Vector2.zero)
            {
                _screenSize = new Vector2(Screen.width, Screen.height);
            }

#if UNITY_EDITOR
            _screenSize = UnityEditor.Handles.GetMainGameViewSize();
#endif
            return _screenSize;
        }

        private Vector2 _devScreenSize;
        /// <summary>
        /// 开发尺寸
        /// </summary>
        /// <returns></returns>
        public Vector2 GetDevScreenSize()
        {
            if (_devScreenSize == Vector2.zero)
            {
                var canvasScaler = UIRoot.GetComponentInChildren<CanvasScaler>();
                _devScreenSize = canvasScaler.referenceResolution;
            }
            return _devScreenSize;
        }

        /// <summary>
        /// 屏幕分辨率比率
        /// </summary>
        /// <returns></returns>
        public float GetScreenRatio()
        {
            var screenSize = GetScreenSize();
            return screenSize.x / screenSize.y;
        }

        /// <summary>
        /// 开发屏幕比率
        /// </summary>
        /// <returns></returns>
        public float GetDevScreenRatio()
        {
            var devSize = GetDevScreenSize();
            return devSize.x / devSize.y;
        }

        private Vector2 _safeArea;
        private float _safeScaleRatio = 0.65f;
        public Vector2 GetSafeArea()
        {
            if (_safeArea == Vector2.zero)
            {
                // TODO 机型特殊处理
                _safeArea = new Vector2(Screen.safeArea.x, Screen.safeArea.y) * _safeScaleRatio;
            }

            return _safeArea;
        }

        #endregion

        /// <summary>
        /// 获取所有层级下顶部的窗口名称。
        /// </summary>
        public string GetTopWindow()
        {
            if (_stack.Count == 0)
            {
                return string.Empty;
            }

            UIWindow topWindow = _stack[^1];
            return topWindow.WindowName;
        }

        /// <summary>
        /// 获取指定层级下顶部的窗口名称。
        /// </summary>
        public string GetTopWindow(int layer)
        {
            UIWindow lastOne = null;
            for (int i = 0; i < _stack.Count; i++)
            {
                if (_stack[i].WindowLayer == layer)
                    lastOne = _stack[i];
            }

            if (lastOne == null)
                return string.Empty;

            return lastOne.WindowName;
        }

        /// <summary>
        /// 是否有任意窗口正在加载。
        /// </summary>
        public bool IsAnyLoading()
        {
            for (int i = 0; i < _stack.Count; i++)
            {
                var window = _stack[i];
                if (window.IsLoadDone == false)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 查询窗口是否存在。
        /// </summary>
        /// <typeparam name="T">界面类型。</typeparam>
        /// <returns>是否存在。</returns>
        public bool HasWindow<T>()
        {
            return HasWindow(typeof(T));
        }

        /// <summary>
        /// 查询窗口是否存在。
        /// </summary>
        /// <param name="type">界面类型。</param>
        /// <returns>是否存在。</returns>
        public bool HasWindow(Type type)
        {
            return IsContains(type.FullName);
        }

       /// <summary>
        /// 异步打开窗口。
        /// </summary>
        /// <param name="userDatas">用户自定义数据。</param>
        /// <returns>打开窗口操作句柄。</returns>
        public void ShowUIAsync<T>(params System.Object[] userDatas) where T : UIWindow
        {
            ShowUIImp(typeof(T), true, userDatas);
        }

        /// <summary>
        /// 异步打开窗口。
        /// </summary>
        /// <param name="type">界面类型。</param>
        /// <param name="userDatas">用户自定义数据。</param>
        /// <returns>打开窗口操作句柄。</returns>
        public void ShowUIAsync(Type type, params System.Object[] userDatas)
        {
            ShowUIImp(type, true, userDatas);
        }

        /// <summary>
        /// 同步打开窗口。
        /// </summary>
        /// <typeparam name="T">窗口类。</typeparam>
        /// <param name="userDatas">用户自定义数据。</param>
        /// <returns>打开窗口操作句柄。</returns>
        public void ShowUI<T>(params System.Object[] userDatas) where T : UIWindow
        {
            ShowUIImp(typeof(T), false, userDatas);
        }

        /// <summary>
        /// 同步打开窗口。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userDatas"></param>
        /// <returns>打开窗口操作句柄。</returns>
        public void ShowUI(Type type, params System.Object[] userDatas)
        {
            ShowUIImp(type, false, userDatas);
        }

        private void ShowUIImp(Type type, bool isAsync, params System.Object[] userDatas)
        {
            string windowName = type.FullName;

            // 如果窗口已经存在
            if (IsContains(windowName))
            {
                UIWindow window = GetWindow(windowName);
                Pop(window); //弹出窗口
                Push(window); //重新压入
                window.TryInvoke(OnWindowPrepare, userDatas);
            }
            else
            {
                UIWindow window = CreateInstance(type);
                Push(window); //首次压入
                window.InternalLoad(window.AssetName, OnWindowPrepare, isAsync, userDatas).Forget();
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void CloseUI<T>() where T : UIWindow
        {
            CloseUI(typeof(T));
        }

        public void CloseUI(Type type)
        {
            string windowName = type.FullName;
            UIWindow window = GetWindow(windowName);
            if (window == null)
                return;

            window.InternalDestroy();
            Pop(window);
            OnSortWindowDepth(window.WindowLayer);
            OnSetWindowVisible();
        }

        public void HideUI<T>() where T : UIWindow
        {
            HideUI(typeof(T));
        }

        public void HideUI(Type type)
        {
            string windowName = type.FullName;
            UIWindow window = GetWindow(windowName);
            if (window == null)
            {
                return;
            }

            if (window.HideTimeToClose <= 0)
            {
                CloseUI(type);
                return;
            }

            window.Visible = false;
            window.HideTimerId = GameModule.Timer.AddTimer((arg) =>
            {
                CloseUI(type);
            },window.HideTimeToClose);
        }

        /// <summary>
        /// 关闭所有窗口。
        /// </summary>
        public void CloseAll()
        {
            for (int i = 0; i < _stack.Count; i++)
            {
                UIWindow window = _stack[i];
                window.InternalDestroy();
            }

            _stack.Clear();
        }

        /// <summary>
        /// 关闭所有窗口除了。
        /// </summary>
        public void CloseAllWithOut(UIWindow withOut)
        {
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                UIWindow window = _stack[i];
                if (window == withOut)
                {
                    continue;
                }

                window.InternalDestroy();
                _stack.RemoveAt(i);
            }
        }

        /// <summary>
        /// 关闭所有窗口除了。
        /// </summary>
        public void CloseAllWithOut<T>() where T : UIWindow
        {
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                UIWindow window = _stack[i];
                if (window.GetType() == typeof(T))
                {
                    continue;
                }

                window.InternalDestroy();
                _stack.RemoveAt(i);
            }
        }

        private void OnWindowPrepare(UIWindow window)
        {
            OnSortWindowDepth(window.WindowLayer);
            window.InternalCreate();
            window.InternalRefresh();
            OnSetWindowVisible();
        }

        private void OnSortWindowDepth(int layer)
        {
            int depth = layer * LAYER_DEEP;
            for (int i = 0; i < _stack.Count; i++)
            {
                if (_stack[i].WindowLayer == layer)
                {
                    _stack[i].Depth = depth;
                    depth += WINDOW_DEEP;
                }
            }
        }

        private void OnSetWindowVisible()
        {
            bool isHideNext = false;
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                UIWindow window = _stack[i];
                if (isHideNext == false)
                {
                    window.Visible = true;
                    if (window.IsPrepare && window.FullScreen)
                    {
                        isHideNext = true;
                    }
                }
                else
                {
                    window.Visible = false;
                }
            }
        }

        private UIWindow CreateInstance(Type type)
        {
            UIWindow window = Activator.CreateInstance(type) as UIWindow;
            WindowAttribute attribute = Attribute.GetCustomAttribute(type, typeof(WindowAttribute)) as WindowAttribute;

            if (window == null)
                throw new GameFrameworkException($"Window {type.FullName} create instance failed.");

            if (attribute != null)
            {
                string assetName = string.IsNullOrEmpty(attribute.Location) ? type.Name : attribute.Location;
                window.Init(type.FullName, attribute.WindowLayer, attribute.FullScreen, assetName, attribute.FromResources, attribute.HideTimeToClose);
            }
            else
            {
                window.Init(type.FullName, (int)UILayer.UI, fullScreen: window.FullScreen, assetName: type.Name, fromResources: false, hideTimeToClose: 10);
            }

            return window;
        }

        private UIWindow GetWindow(string windowName)
        {
            for (int i = 0; i < _stack.Count; i++)
            {
                UIWindow window = _stack[i];
                if (window.WindowName == windowName)
                {
                    return window;
                }
            }

            return null;
        }

        private bool IsContains(string windowName)
        {
            for (int i = 0; i < _stack.Count; i++)
            {
                UIWindow window = _stack[i];
                if (window.WindowName == windowName)
                {
                    return true;
                }
            }

            return false;
        }

        private void Push(UIWindow window)
        {
            // 如果已经存在
            if (IsContains(window.WindowName))
                throw new System.Exception($"Window {window.WindowName} is exist.");

            // 获取插入到所属层级的位置
            int insertIndex = -1;
            for (int i = 0; i < _stack.Count; i++)
            {
                if (window.WindowLayer == _stack[i].WindowLayer)
                {
                    insertIndex = i + 1;
                }
            }

            // 如果没有所属层级，找到相邻层级
            if (insertIndex == -1)
            {
                for (int i = 0; i < _stack.Count; i++)
                {
                    if (window.WindowLayer > _stack[i].WindowLayer)
                    {
                        insertIndex = i + 1;
                    }
                }
            }

            // 如果是空栈或没有找到插入位置
            if (insertIndex == -1)
            {
                insertIndex = 0;
            }

            // 最后插入到堆栈
            _stack.Insert(insertIndex, window);
        }

        private void Pop(UIWindow window)
        {
            // 从堆栈里移除
            _stack.Remove(window);
        }
    }

    [UpdateModule]
    internal sealed partial class UIModuleImpl : ModuleImp
    {
        private List<UIWindow> _stack;

        internal void Initialize(List<UIWindow> stack)
        {
            _stack = stack;
        }

        internal override void Shutdown()
        {
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (_stack == null)
            {
                return;
            }

            int count = _stack.Count;
            for (int i = 0; i < _stack.Count; i++)
            {
                if (_stack.Count != count)
                {
                    break;
                }

                var window = _stack[i];
                window.InternalUpdate();
            }
        }
    }
}