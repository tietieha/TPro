using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TEngine
{
    /// <summary>
    /// 调试器模块。
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class DebuggerModule : Module
    {
        /// <summary>
        /// 默认调试器漂浮框大小。
        /// </summary>
        internal static readonly Rect DefaultIconRect = new Rect(10f, 10f, 60f, 60f);

        /// <summary>
        /// 默认调试器窗口大小。
        /// </summary>
        internal static Rect DefaultWindowRect = new Rect(10f, 10f, 640f, 480f);

        /// <summary>
        /// 默认调试器窗口缩放比例。
        /// </summary>
        internal static readonly float DefaultWindowScale = 1.5f;

        private static TextEditor s_TextEditor = null;
        private IDebuggerManager _debuggerManager = null;
        private Rect m_DragRect = new Rect(0f, 0f, float.MaxValue, 25f);
        private Rect m_IconRect = DefaultIconRect;
        private Rect m_WindowRect = DefaultWindowRect;
        private float m_WindowScale = DefaultWindowScale;

        private float m_ToolbarHeight = 50f;

        [SerializeField]
        private GUISkin m_Skin = null;

        [SerializeField]
        private DebuggerActiveWindowType m_ActiveWindow = DebuggerActiveWindowType.AlwaysOpen;

        public DebuggerActiveWindowType ActiveWindowType => m_ActiveWindow;

        [SerializeField]
        private bool m_ShowFullWindow = false;

        [SerializeField]
        private ConsoleWindow m_ConsoleWindow = new ConsoleWindow();

        private SystemInformationWindow m_SystemInformationWindow = new SystemInformationWindow();
        private EnvironmentInformationWindow m_EnvironmentInformationWindow = new EnvironmentInformationWindow();
        private ScreenInformationWindow m_ScreenInformationWindow = new ScreenInformationWindow();
        private GraphicsInformationWindow m_GraphicsInformationWindow = new GraphicsInformationWindow();
        private InputSummaryInformationWindow m_InputSummaryInformationWindow = new InputSummaryInformationWindow();
        private InputTouchInformationWindow m_InputTouchInformationWindow = new InputTouchInformationWindow();
        private InputLocationInformationWindow m_InputLocationInformationWindow = new InputLocationInformationWindow();
        private InputAccelerationInformationWindow m_InputAccelerationInformationWindow = new InputAccelerationInformationWindow();
        private InputGyroscopeInformationWindow m_InputGyroscopeInformationWindow = new InputGyroscopeInformationWindow();
        private InputCompassInformationWindow m_InputCompassInformationWindow = new InputCompassInformationWindow();
        private PathInformationWindow m_PathInformationWindow = new PathInformationWindow();
        private SceneInformationWindow m_SceneInformationWindow = new SceneInformationWindow();
        private TimeInformationWindow m_TimeInformationWindow = new TimeInformationWindow();
        private QualityInformationWindow m_QualityInformationWindow = new QualityInformationWindow();
        private ProfilerInformationWindow m_ProfilerInformationWindow = new ProfilerInformationWindow();
        private RuntimeMemorySummaryWindow m_RuntimeMemorySummaryWindow = new RuntimeMemorySummaryWindow();
        private RuntimeMemoryInformationWindow<Object> m_RuntimeMemoryAllInformationWindow = new RuntimeMemoryInformationWindow<Object>();
        private RuntimeMemoryInformationWindow<Texture> m_RuntimeMemoryTextureInformationWindow = new RuntimeMemoryInformationWindow<Texture>();
        private RuntimeMemoryInformationWindow<Mesh> m_RuntimeMemoryMeshInformationWindow = new RuntimeMemoryInformationWindow<Mesh>();
        private RuntimeMemoryInformationWindow<Material> m_RuntimeMemoryMaterialInformationWindow = new RuntimeMemoryInformationWindow<Material>();
        private RuntimeMemoryInformationWindow<Shader> m_RuntimeMemoryShaderInformationWindow = new RuntimeMemoryInformationWindow<Shader>();
        private RuntimeMemoryInformationWindow<AnimationClip> m_RuntimeMemoryAnimationClipInformationWindow = new RuntimeMemoryInformationWindow<AnimationClip>();
        private RuntimeMemoryInformationWindow<AudioClip> m_RuntimeMemoryAudioClipInformationWindow = new RuntimeMemoryInformationWindow<AudioClip>();
        private RuntimeMemoryInformationWindow<Font> m_RuntimeMemoryFontInformationWindow = new RuntimeMemoryInformationWindow<Font>();
        private RuntimeMemoryInformationWindow<TextAsset> m_RuntimeMemoryTextAssetInformationWindow = new RuntimeMemoryInformationWindow<TextAsset>();
        private RuntimeMemoryInformationWindow<ScriptableObject> m_RuntimeMemoryScriptableObjectInformationWindow = new RuntimeMemoryInformationWindow<ScriptableObject>();
        private ObjectPoolInformationWindow m_ObjectPoolInformationWindow = new ObjectPoolInformationWindow();
        private MemoryPoolPoolInformationWindow m_MemoryPoolPoolInformationWindow = new MemoryPoolPoolInformationWindow();
        // private NetworkInformationWindow m_NetworkInformationWindow = new NetworkInformationWindow();
        private SettingsWindow m_SettingsWindow = new SettingsWindow();
        private OperationsWindow m_OperationsWindow = new OperationsWindow();

        private FpsCounter m_FpsCounter = null;

        private bool _initIfNeeded = false;
        /// <summary>
        /// 获取或设置调试器窗口是否激活。
        /// </summary>
        public bool ActiveWindow
        {
            get => _debuggerManager.ActiveWindow;
            set
            {
                _debuggerManager.ActiveWindow = value;
                enabled = value;
            }
        }

        /// <summary>
        /// 获取或设置是否显示完整调试器界面。
        /// </summary>
        public bool ShowFullWindow
        {
            get => m_ShowFullWindow;
            set
            {
                if (_eventSystem != null)
                {
                    _eventSystem.SetActive(!value,ref _eventSystemActive);
                }
                m_ShowFullWindow = value;
            }
        }

        /// <summary>
        /// 获取或设置调试器漂浮框大小。
        /// </summary>
        public Rect IconRect
        {
            get => m_IconRect;
            set => m_IconRect = value;
        }

        /// <summary>
        /// 获取或设置调试器窗口大小。
        /// </summary>
        public Rect WindowRect
        {
            get => m_WindowRect;
            set => m_WindowRect = value;
        }

        /// <summary>
        /// 获取或设置调试器窗口缩放比例。
        /// </summary>
        public float WindowScale
        {
            get => m_WindowScale;
            set => m_WindowScale = value;
        }

        private SettingModule _settingModule = null;

        private bool _eventSystemActive = true;
        private GameObject _eventSystem;
        /// <summary>
        /// 游戏框架模块初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            s_TextEditor = new TextEditor();
            _eventSystem = GameObject.Find("UIRoot/EventSystem");
            _debuggerManager = ModuleImpSystem.GetModule<IDebuggerManager>();
            if (_debuggerManager == null)
            {
                Log.Fatal("Debugger manager is invalid.");
                return;
            }

            m_FpsCounter = new FpsCounter(0.5f);
        }

        private void OnDestroy()
        {
            if (_settingModule == null)
            {
                Log.Fatal("Setting component is invalid.");
                return;
            }
            
            _settingModule.Save();
        }

        private void Initialize()
        {
            _settingModule = ModuleSystem.GetModule<SettingModule>();
            if (_settingModule == null)
            {
                Log.Fatal("Setting component is invalid.");
                return;
            }

            if (!_settingModule.IsInit)
                _settingModule.Init();

            DefaultWindowRect = new Rect(0, 0, Screen.width / WindowScale, Screen.height / WindowScale);
            var lastIconX = _settingModule.GetFloat("Debugger.Icon.X", DefaultIconRect.x);
            var lastIconY = _settingModule.GetFloat("Debugger.Icon.Y", DefaultIconRect.y);
            var lastWindowX = _settingModule.GetFloat("Debugger.Window.X", DefaultWindowRect.x);
            var lastWindowY = _settingModule.GetFloat("Debugger.Window.Y", DefaultWindowRect.y);
            var lastWindowWidth = _settingModule.GetFloat("Debugger.Window.Width", DefaultWindowRect.width);
            var lastWindowHeight = _settingModule.GetFloat("Debugger.Window.Height", DefaultWindowRect.height);
            m_WindowScale = _settingModule.GetFloat("Debugger.Window.Scale", DefaultWindowScale);
            m_WindowRect = new Rect(lastIconX, lastIconY, DefaultIconRect.width, DefaultIconRect.height);
            m_WindowRect = new Rect(lastWindowX, lastWindowY, lastWindowWidth, lastWindowHeight);
        }

        public override void Init()
        {
            base.Init();

            Initialize();
            RegisterDebuggerWindow("Console", m_ConsoleWindow);

            switch (m_ActiveWindow)
            {
                case DebuggerActiveWindowType.AlwaysOpen:
                    ActiveWindow = true;
                    break;

                case DebuggerActiveWindowType.OnlyOpenWhenDevelopment:
                    ActiveWindow = Debug.isDebugBuild;
                    break;

                case DebuggerActiveWindowType.OnlyOpenInEditor:
                    ActiveWindow = Application.isEditor;
                    break;

                default:
                    ActiveWindow = false;
                    break;
            }
        }

        private void InitIfNeeded()
        {
            _initIfNeeded = true;
            m_SystemInformationWindow = new SystemInformationWindow();
            m_EnvironmentInformationWindow = new EnvironmentInformationWindow();
            m_ScreenInformationWindow = new ScreenInformationWindow();
            m_GraphicsInformationWindow = new GraphicsInformationWindow();
            m_InputSummaryInformationWindow = new InputSummaryInformationWindow();
            m_InputTouchInformationWindow = new InputTouchInformationWindow();
            m_InputLocationInformationWindow = new InputLocationInformationWindow();
            m_InputAccelerationInformationWindow = new InputAccelerationInformationWindow();
            m_InputGyroscopeInformationWindow = new InputGyroscopeInformationWindow();
            m_InputCompassInformationWindow = new InputCompassInformationWindow();
            m_PathInformationWindow = new PathInformationWindow();
            m_SceneInformationWindow = new SceneInformationWindow();
            m_TimeInformationWindow = new TimeInformationWindow();
            m_QualityInformationWindow = new QualityInformationWindow();
            m_ProfilerInformationWindow = new ProfilerInformationWindow();
            m_RuntimeMemorySummaryWindow = new RuntimeMemorySummaryWindow();
            m_RuntimeMemoryAllInformationWindow = new RuntimeMemoryInformationWindow<Object>();
            m_RuntimeMemoryTextureInformationWindow = new RuntimeMemoryInformationWindow<Texture>();
            m_RuntimeMemoryMeshInformationWindow = new RuntimeMemoryInformationWindow<Mesh>();
            m_RuntimeMemoryMaterialInformationWindow = new RuntimeMemoryInformationWindow<Material>();
            m_RuntimeMemoryShaderInformationWindow = new RuntimeMemoryInformationWindow<Shader>();
            m_RuntimeMemoryAnimationClipInformationWindow = new RuntimeMemoryInformationWindow<AnimationClip>();
            m_RuntimeMemoryAudioClipInformationWindow = new RuntimeMemoryInformationWindow<AudioClip>();
            m_RuntimeMemoryFontInformationWindow = new RuntimeMemoryInformationWindow<Font>();
            m_RuntimeMemoryTextAssetInformationWindow = new RuntimeMemoryInformationWindow<TextAsset>();
            m_RuntimeMemoryScriptableObjectInformationWindow = new RuntimeMemoryInformationWindow<ScriptableObject>();
            m_ObjectPoolInformationWindow = new ObjectPoolInformationWindow();
            m_MemoryPoolPoolInformationWindow = new MemoryPoolPoolInformationWindow();
            //  m_NetworkInformationWindow = new NetworkInformationWindow();
            m_SettingsWindow = new SettingsWindow();
            m_OperationsWindow = new OperationsWindow();

            RegisterDebuggerWindow("Information/System", m_SystemInformationWindow);
            RegisterDebuggerWindow("Information/Environment", m_EnvironmentInformationWindow);
            RegisterDebuggerWindow("Information/Screen", m_ScreenInformationWindow);
            RegisterDebuggerWindow("Information/Graphics", m_GraphicsInformationWindow);
            RegisterDebuggerWindow("Information/Input/Summary", m_InputSummaryInformationWindow);
            RegisterDebuggerWindow("Information/Input/Touch", m_InputTouchInformationWindow);
            RegisterDebuggerWindow("Information/Input/Location", m_InputLocationInformationWindow);
            RegisterDebuggerWindow("Information/Input/Acceleration", m_InputAccelerationInformationWindow);
            RegisterDebuggerWindow("Information/Input/Gyroscope", m_InputGyroscopeInformationWindow);
            RegisterDebuggerWindow("Information/Input/Compass", m_InputCompassInformationWindow);
            RegisterDebuggerWindow("Information/Other/Scene", m_SceneInformationWindow);
            RegisterDebuggerWindow("Information/Other/Path", m_PathInformationWindow);
            RegisterDebuggerWindow("Information/Other/Time", m_TimeInformationWindow);
            RegisterDebuggerWindow("Information/Other/Quality", m_QualityInformationWindow);
            RegisterDebuggerWindow("Profiler/Summary", m_ProfilerInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/Summary", m_RuntimeMemorySummaryWindow);
            RegisterDebuggerWindow("Profiler/Memory/All", m_RuntimeMemoryAllInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/Texture", m_RuntimeMemoryTextureInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/Mesh", m_RuntimeMemoryMeshInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/Material", m_RuntimeMemoryMaterialInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/Shader", m_RuntimeMemoryShaderInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/AnimationClip", m_RuntimeMemoryAnimationClipInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/AudioClip", m_RuntimeMemoryAudioClipInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/Font", m_RuntimeMemoryFontInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/TextAsset", m_RuntimeMemoryTextAssetInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/ScriptableObject", m_RuntimeMemoryScriptableObjectInformationWindow);
            RegisterDebuggerWindow("Profiler/Object Pool", m_ObjectPoolInformationWindow);
            RegisterDebuggerWindow("Profiler/Reference Pool", m_MemoryPoolPoolInformationWindow);
            // RegisterDebuggerWindow("Profiler/Network", m_NetworkInformationWindow);
            RegisterDebuggerWindow("Other/Settings", m_SettingsWindow);
            RegisterDebuggerWindow("Other/Operations", m_OperationsWindow);
        }

        private void Update()
        {
            m_FpsCounter.Update(GameTime.deltaTime, GameTime.unscaledDeltaTime);
        }

        private void OnGUI()
        {
            if (_debuggerManager == null || !_debuggerManager.ActiveWindow)
            {
                return;
            }

            if (!_initIfNeeded)
            {
                InitIfNeeded();
            }

            GUISkin cachedGuiSkin = GUI.skin;
            Matrix4x4 cachedMatrix = GUI.matrix;

            GUI.skin = m_Skin;
            GUI.matrix = Matrix4x4.Scale(new Vector3(m_WindowScale, m_WindowScale, 1f));

            if (m_ShowFullWindow)
            {
                m_WindowRect.width = Screen.width / m_WindowScale;
                m_WindowRect.height = Screen.height / m_WindowScale;
                // m_WindowRect = GUILayout.Window(0, m_WindowRect, DrawWindow, "<b>TENGINE DEBUGGER</b>");
                // draw bg
                GUI.DrawTexture(WindowRect, Styles.BlackTex);
                DrawDebuggerWindowGroup(_debuggerManager.DebuggerWindowRoot, WindowRect, WindowScale);
            }
            else
            {
                m_IconRect = GUILayout.Window(0, m_IconRect, DrawDebuggerWindowIcon, "<b>DEBUGGER</b>");
            }

            GUI.matrix = cachedMatrix;
            GUI.skin = cachedGuiSkin;
        }

        /// <summary>
        /// 注册调试器窗口。
        /// </summary>
        /// <param name="path">调试器窗口路径。</param>
        /// <param name="debuggerWindow">要注册的调试器窗口。</param>
        /// <param name="args">初始化调试器窗口参数。</param>
        public void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow, params object[] args)
        {
            _debuggerManager.RegisterDebuggerWindow(path, debuggerWindow, args);
        }

        /// <summary>
        /// 解除注册调试器窗口。
        /// </summary>
        /// <param name="path">调试器窗口路径。</param>
        /// <returns>是否解除注册调试器窗口成功。</returns>
        public bool UnregisterDebuggerWindow(string path)
        {
            return _debuggerManager.UnregisterDebuggerWindow(path);
        }

        /// <summary>
        /// 获取调试器窗口。
        /// </summary>
        /// <param name="path">调试器窗口路径。</param>
        /// <returns>要获取的调试器窗口。</returns>
        public IDebuggerWindow GetDebuggerWindow(string path)
        {
            return _debuggerManager.GetDebuggerWindow(path);
        }

        /// <summary>
        /// 选中调试器窗口。
        /// </summary>
        /// <param name="path">调试器窗口路径。</param>
        /// <returns>是否成功选中调试器窗口。</returns>
        public bool SelectDebuggerWindow(string path)
        {
            return _debuggerManager.SelectDebuggerWindow(path);
        }

        /// <summary>
        /// 还原调试器窗口布局。
        /// </summary>
        public void ResetLayout()
        {
            IconRect = DefaultIconRect;
            WindowRect = DefaultWindowRect;
            WindowScale = DefaultWindowScale;
        }

        /// <summary>
        /// 获取记录的所有日志。
        /// </summary>
        /// <param name="results">要获取的日志。</param>
        public void GetRecentLogs(List<LogNode> results)
        {
            m_ConsoleWindow.GetRecentLogs(results);
        }

        /// <summary>
        /// 获取记录的最近日志。
        /// </summary>
        /// <param name="results">要获取的日志。</param>
        /// <param name="count">要获取最近日志的数量。</param>
        public void GetRecentLogs(List<LogNode> results, int count)
        {
            m_ConsoleWindow.GetRecentLogs(results, count);
        }

        private void DrawWindow(int windowId)
        {
            // GUI.DragWindow(m_DragRect);
            DrawDebuggerWindowGroup(_debuggerManager.DebuggerWindowRoot, WindowRect, WindowScale);
        }

        private void DrawDebuggerWindowGroup(IDebuggerWindowGroup debuggerWindowGroup, Rect contentRect, float windowScale)
        {
            if (debuggerWindowGroup == null)
            {
                return;
            }

            List<string> names = new List<string>();
            string[] debuggerWindowNames = debuggerWindowGroup.GetDebuggerWindowNames();
            for (int i = 0; i < debuggerWindowNames.Length; i++)
            {
                names.Add(Utility.Text.Format("<b>{0}</b>", debuggerWindowNames[i]));
            }

            if (debuggerWindowGroup == _debuggerManager.DebuggerWindowRoot)
            {
                names.Add("<b>Close</b>");
            }

            var toolbarHeight = contentRect.height * 0.05f * windowScale;
            Rect toolbarRect = new Rect(contentRect.x, contentRect.y, contentRect.width, toolbarHeight);
            GUILayout.BeginArea(toolbarRect);
            // int toolbarIndex = GUI.Toolbar(toolbarRect, debuggerWindowGroup.SelectedIndex, names.ToArray());
            int toolbarIndex = GUILayout.Toolbar(debuggerWindowGroup.SelectedIndex, names.ToArray(), GUILayout.MinWidth(30f), GUILayout.Height(toolbarHeight), GUILayout.MaxWidth(Screen.width));
            GUILayout.EndArea();
            if (toolbarIndex >= debuggerWindowGroup.DebuggerWindowCount)
            {
                ShowFullWindow = false;
                return;
            }
            if (debuggerWindowGroup.SelectedWindow == null)
            {
                return;
            }

            if (debuggerWindowGroup.SelectedIndex != toolbarIndex)
            {
                debuggerWindowGroup.SelectedWindow.OnLeave();
                debuggerWindowGroup.SelectedIndex = toolbarIndex;
                debuggerWindowGroup.SelectedWindow.OnEnter();
            }
            Rect remainRect = new Rect(contentRect.x, toolbarRect.y + toolbarRect.height, contentRect.width, contentRect.height - toolbarRect.height);
            // GUILayout.BeginArea(remainRect);
            IDebuggerWindowGroup subDebuggerWindowGroup = debuggerWindowGroup.SelectedWindow as IDebuggerWindowGroup;
            if (subDebuggerWindowGroup != null)
            {
                DrawDebuggerWindowGroup(subDebuggerWindowGroup, remainRect, windowScale);
            }
            else
            {
                getDownPos();
                Vector2 drag = getDrag();
                Vector4 touch = new Vector4(drag.x, drag.y, downPos.x, downPos.y);
                debuggerWindowGroup?.SelectedWindow?.OnDraw(remainRect, touch, windowScale);
            }
            // GUILayout.EndArea();
        }

        private void DrawDebuggerWindowIcon(int windowId)
        {
            GUI.DragWindow(m_DragRect);
            GUILayout.Space(5);
            Color32 color = Color.white;
            m_ConsoleWindow.RefreshCount();
            if (m_ConsoleWindow.FatalCount > 0)
            {
                color = m_ConsoleWindow.GetLogStringColor(LogType.Exception);
            }
            else if (m_ConsoleWindow.ErrorCount > 0)
            {
                color = m_ConsoleWindow.GetLogStringColor(LogType.Error);
            }
            else if (m_ConsoleWindow.WarningCount > 0)
            {
                color = m_ConsoleWindow.GetLogStringColor(LogType.Warning);
            }
            else
            {
                color = m_ConsoleWindow.GetLogStringColor(LogType.Log);
            }

            string title = Utility.Text.Format("<color=#{0:x2}{1:x2}{2:x2}{3:x2}><b>FPS: {4:F2}</b></color>", color.r, color.g, color.b, color.a, m_FpsCounter.CurrentFps);
            if (GUILayout.Button(title, GUILayout.Width(100f), GUILayout.Height(40f)))
            {
                ShowFullWindow = true;
            }
        }

        private static void CopyToClipboard(string content)
        {
            s_TextEditor.text = content;
            s_TextEditor.OnFocus();
            s_TextEditor.Copy();
            s_TextEditor.text = string.Empty;
        }

        //calculate  pos of first click on screen
        Vector2 startPos;

        Vector2 downPos;
        /// <summary>
        /// 获取按下位置。 y 轴坐标需要转换。
        /// </summary>
        /// <returns></returns>
        Vector2 getDownPos()
        {
            if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer)
            {

                if (Input.touches.Length == 1 && Input.touches[0].phase == TouchPhase.Began)
                {
                    downPos = Input.touches[0].position;
                    downPos.y = Screen.height - downPos.y;
                    downPos = downPos / WindowScale;
                    return downPos;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    downPos.x = Input.mousePosition.x;
                    downPos.y = Screen.height - Input.mousePosition.y;
                    downPos = downPos / WindowScale;
                    return downPos;
                }
            }

            return Vector2.zero;
        }
        //calculate drag amount , this is used for scrolling

        Vector2 dragPos;
        Vector2 getDrag()
        {
            if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (Input.touches.Length != 1)
                {
                    return Vector2.zero;
                }
                dragPos = Input.touches[0].position;
                dragPos.y = Screen.height - dragPos.y;
                dragPos = dragPos / WindowScale;
                return downPos - dragPos;
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    dragPos.x = Input.mousePosition.x;
                    dragPos.y = Screen.height - Input.mousePosition.y;
                    dragPos = dragPos / WindowScale;
                    return downPos - dragPos;
                }
                else
                {
                    return Vector2.zero;
                }
            }
        }

        public static class Styles
        {
            private static Texture2D _redTex;
            public static Texture2D RedTex
            {
                get
                {
                    if (_redTex == null)
                    {
                        _redTex = MakeTex(2, 2, Color.red);
                    }

                    return _redTex;
                }
            }

            private static Texture2D _greenTex;
            public static Texture2D GreenTex
            {
                get
                {
                    if (_greenTex == null)
                    {
                        _greenTex = MakeTex(2, 2, Color.green);
                    }

                    return _greenTex;
                }
            }

            private static Texture2D _blackTex;
            public static Texture2D BlackTex
            {
                get
                {
                    if (_blackTex == null)
                    {
                        _blackTex = MakeTex(2, 2, new Color(0.0f, 0.0f, 0.0f, 0.5f));
                    }

                    return _blackTex;
                }
            }

            private static Texture2D MakeTex(int width, int height, Color col)
            {
                var pix = new Color[width * height];

                for (var i = 0; i < pix.Length; i++)
                    pix[i] = col;

                var result = new Texture2D(width, height);
                result.SetPixels(pix);
                result.Apply();

                return result;
            }
        }
    }
}
