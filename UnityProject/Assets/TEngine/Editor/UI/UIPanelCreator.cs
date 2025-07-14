// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Text;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.UI;
// using XLua;
//
// namespace TEngine.Editor.UI
// {
//     public class BuildPanelParameter
//     {
//         private string m_SourceName;    // prefab name
//         private string m_AssetPath;
//         private string m_ScriptName;
//         private string m_LuaInjectContent;
//         private string m_ButtonAddListenerContent;
//         private string m_ButtonRemoveListenerContent;
//         private string m_ButtonEventHandlersContent;
//         private string m_OutputPath;
//
//         public string SourceName => m_SourceName;
//         public string AssetPath => m_AssetPath;
//         public string ScriptName => m_ScriptName;
//         public string LuaInjectContent => m_LuaInjectContent;
//         public string ButtonAddListenerContent => m_ButtonAddListenerContent;
//         public string ButtonRemoveListenerContent => m_ButtonRemoveListenerContent;
//         public string ButtonEventHandlersContent => m_ButtonEventHandlersContent;
//         public string OutputPath => m_OutputPath;
//
//         public BuildPanelParameter(
//             string sourceName,
//             string assetPath,
//             string scriptName,
//             string luaInjectContent,
//             string addListenerContent,
//             string removeListenerContent,
//             string eventHandlerContent,
//             string outputPath)
//         {
//             m_SourceName                    = sourceName;
//             m_AssetPath                     = assetPath;
//             m_ScriptName                    = scriptName;
//             m_LuaInjectContent              = luaInjectContent;
//             m_ButtonAddListenerContent      = addListenerContent;
//             m_ButtonRemoveListenerContent   = removeListenerContent;
//             m_ButtonEventHandlersContent    = eventHandlerContent;
//             m_OutputPath                    = outputPath;
//         }
//     }
//
//     public class UIPanelCreator
//     {
//         private static string scriptTemplatePath    = "Assets/Editor/Template/UIPanelLuaTemplate.lua.txt";
//         private static string outputPrefix          = "GameAsset/Lua/GameLogic/";
//
//         private static string parseStart            = "PARSE_START";
//         private static string parseEnd              = "PARSE_END";
//         private static string addListenerStart      = "ADD_LISTENER_START";
//         private static string addListenerEnd        = "ADD_LISTENER_END";
//         private static string removeListenerStart   = "REMOVE_LISTENER_START";
//         private static string removeListenerEnd     = "REMOVE_LISTENER_END";
//         private static string eventHandlerStart     = "EVENT_HANDLER_START";
//         private static string eventHandlerEnd       = "EVENT_HANDLER_END";
//
//         [MenuItem("Assets/UIPanel/新建Panel脚本")]
//         private static void CreateUIPanel()
//         {
//             GameObject panelPrefab = Selection.activeGameObject;
//             if (panelPrefab == null)
//             {
//                 Debug.LogError("未选中Panel预制体");
//                 return;
//             }
//
//             if(PrefabUtility.GetPrefabAssetType(panelPrefab) != PrefabAssetType.Regular)
//             {
//                 Debug.LogError("选择的不是预制体，选择的是： " + panelPrefab.name);
//                 return;
//             }
//
//             // 文件命名以及脚本的命名
//             string viewName = panelPrefab.name;
//             if(!viewName.ToLower().StartsWith("ui"))
//             {
//                 viewName = "UI" + viewName;
//             }
//             if(!viewName.ToLower().EndsWith("view"))
//             {
//                 viewName = viewName + "View";
//             }
//
//             // LuaBehavior
//
//             string prefabPath = AssetDatabase.GetAssetPath(panelPrefab);
//             GameObject prefab = PrefabUtility.LoadPrefabContents(prefabPath);
//             LuaBehaviour luaComponent = prefab.GetComponent<LuaBehaviour>();
//             if (luaComponent == null)
//             {
//                 luaComponent = prefab.AddComponent<LuaBehaviour>();
//                 PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
//             }
//
//             List<Transform> list = new List<Transform>();
//             // 遍历prefab
//             GetAllChildren(prefab.transform, list);
//
//             StringBuilder luaInjectContent      = new StringBuilder();
//             StringBuilder addListenerString     = new StringBuilder();
//             StringBuilder removeListenerString  = new StringBuilder();
//             StringBuilder eventHandlerString    = new StringBuilder();
//
//             luaInjectContent.AppendLine();
//             addListenerString.AppendLine();
//             removeListenerString.AppendLine();
//             eventHandlerString.AppendLine();
//
//             // 解析GameObject命名，并解析类型
//             List<LuaObjectReference> objectReferences = new List<LuaObjectReference>();
//             foreach (Transform t in list)
//             {
//                 var tType = ParseType(t.name);
//                 string toVarName = Rename(t.name);
//                 LuaObjectReference objectReference = new LuaObjectReference()
//                 {
//                     Component   = t,
//                     Name        = toVarName,
//                     Object      = t.gameObject,
//                     TypeIndex   = CalculateComponentIndex(t, tType),
//                 };
//                 objectReferences.Add(objectReference);
//                 if(tType == typeof(UnityEngine.UI.Button))
//                 {
//                     addListenerString.AppendLine($"\tself.{objectReference.Name}.onClick:AddListener(handler(self, self._onClick{objectReference.Name.Replace("btn", "")}))");
//                     removeListenerString.AppendLine($"\tself.{objectReference.Name}.onClick:RemoveAllListeners()");
//                     eventHandlerString.AppendLine($"function M:_onClick{objectReference.Name.Replace("btn", "")}()\n\nend\n");
//                 }
//                 luaInjectContent.AppendLine($"\t-- self.{objectReference.Name}  \t = nil    {tType.Name}");
//             }
//
//             luaComponent.references = objectReferences.ToArray();
//             string[] names = prefabPath.Split('/');
//             string moduleName = names[names.Length - 2];
//             if(moduleName.ToLower().StartsWith("ui"))
//             {
//                 moduleName = moduleName.Substring(2);
//             }
//
//             string output = Path.Combine(Application.dataPath, outputPrefix, $"{moduleName}/UI/");
//             //// 写脚本
//             BuildPanelParameter parameter = new BuildPanelParameter(prefab.name,
//                 prefabPath, viewName,
//                 luaInjectContent.ToString(), addListenerString.ToString(), removeListenerString.ToString(), eventHandlerString.ToString(),
//                 output);
//             CreateAssetFromTemplete(parameter);
//
//             PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
//
//             PrefabUtility.UnloadPrefabContents(prefab);
//
//             AssetDatabase.Refresh();
//         }
//
//         private static void GetAllChildren(Transform node, List<Transform> list)
//         {
//             if(node == null)
//             {
//                 return;
//             }
//
//             foreach(Transform item in node.transform)
//             {
//                 // 踢出prefab
//                 if (PrefabUtility.GetPrefabAssetType(item) == PrefabAssetType.NotAPrefab)
//                 {
//
//                     if (CheckIsValidName(item.name))
//                     {
//                         list.Add(item);
//                     }
//
//                     GetAllChildren(item, list);
//                 }
//             }
//         }
//
//         private static bool CheckIsValidName(string goName)
//         {
//             if (string.IsNullOrEmpty(goName))
//             {
//                 return false;
//             }
//
//             if(!goName.StartsWith("m_") && !goName.StartsWith("M_"))
//             {
//                 return false;
//             }
//
//             return true;
//         }
//
//         public static System.Type ParseType(string goName)
//         {
//             if(goName.StartsWith("m_") || goName.StartsWith("M_"))
//             {
//                 string[] splits = goName.Split('_');
//                 if(splits != null && splits.Length > 1)
//                 {
//                     string type = splits[1].ToLower();
//                     if(type.Equals("img"))
//                     {
//                         return typeof(UnityEngine.UI.Image);
//                     }
//                     else if(type.Equals("btn"))
//                     {
//                         return typeof(UnityEngine.UI.Button);
//                     }
//                     else if(type.Equals("txt"))
//                     {
//                         return typeof(TMPro.TextMeshProUGUI);
//                     }
//                     else if(type.Equals("go"))
//                     {
//                         return typeof(GameObject);
//                     }
//                     else if(type.Equals("scr"))
//                     {
//                         return typeof(UIMultiScroller);
//                     }
//                     else if(type.Equals("tra"))
//                     {
//                         return typeof(Transform);
//                     }
//                     else if(type.Equals("rtra"))
//                     {
//                         return typeof(RectTransform);
//                     }
//                     else if(type.Equals("sld"))
//                     {
//                         return typeof(Slider);
//                     }
//                     else if(type.Equals("tog"))
//                     {
//                         return typeof(Toggle);
//                     }
//                     else if(type.Equals("toggr"))
//                     {
//                         return typeof(ToggleGroup);
//                     }
//                 }
//             }
//
//             // 默认用GameObject
//             return typeof(GameObject);
//         }
//
//         public static LuaObjectReference CreateObjItem(GameObject item)
//         {
//             Type tType = ParseType(item.name);
//             string toVarName = Rename(item.name);
//             return new LuaObjectReference()
//             {
//                 Component   = CalculateComponent(item,tType) ,
//                 Name        = toVarName,
//                 Object      = item.gameObject,
//                 TypeIndex   = CalculateComponentIndex(item, tType),
//             };
//         }
//         public static string Rename(string name)
//         {
//             string toVarName = string.Empty;
//             if (name.StartsWith("m_") || name.StartsWith("M_"))
//             {
//                 toVarName = name[1..];
//                 string[] varSplits = toVarName.Split('_');
//                 if (varSplits != null)
//                 {
//                     for (int i = 2; i < varSplits.Length; i++)
//                     {
//                         varSplits[i] = varSplits[i][..1].ToUpper() + varSplits[i][1..];
//                     }
//
//                     toVarName = "";
//                     for (int i = 0; i < varSplits.Length; i++)
//                     {
//                         toVarName += varSplits[i];
//                     }
//
//                     toVarName = "_" + toVarName;
//                 }
//             }
//
//             return toVarName.IsNullOrEmpty() ? $"_{name}" : toVarName;
//         }
//         public static int CalculateComponentIndex(Transform node, Type type)
//         {
//             Component[] components = node.GetComponents<Component>();
//             if (components == null || components.Length == 0)
//             {
//                 return 0;
//             }
//
//             if (type == typeof(GameObject))
//             {
//                 return components.Length;
//             }
//             else
//             {
//                 for (int i = 0; i < components.Length; i++)
//                 {
//                     Component comp = components[i];
//                     if (type == comp.GetType())
//                     {
//                         return i;
//                     }
//                 }
//             }
//
//             return 0;
//         }
//         public static int CalculateComponentIndex(GameObject node, Type type)
//         {
//             Component[] components = node.GetComponents<Component>();
//             if (components == null || components.Length == 0)
//             {
//                 return 0;
//             }
//
//             if (type == typeof(GameObject))
//             {
//                 return components.Length;
//             }
//             else
//             {
//                 for (int i = 0; i < components.Length; i++)
//                 {
//                     Component comp = components[i];
//                     if (type == comp.GetType())
//                     {
//                         return i;
//                     }
//                 }
//             }
//
//             return 0;
//         }
//         public static UnityEngine.Object CalculateComponent(GameObject node, Type type)
//         {
//             Component[] components = node.GetComponents<Component>();
//             if (components == null || components.Length == 0)
//             {
//                 return node;
//             }
//
//             if (type == typeof(GameObject))
//             {
//                 return node;
//             }
//             else
//             {
//                 for (int i = 0; i < components.Length; i++)
//                 {
//                     Component comp = components[i];
//                     if (type == comp.GetType())
//                     {
//                         return comp;
//                     }
//                 }
//             }
//
//             return node;
//         }
//         private static void CreateAssetFromTemplete(BuildPanelParameter parameter)
//         {
//             string fullName = Path.GetFullPath(parameter.OutputPath);
//             if(!Directory.Exists(fullName))
//             {
//                 Directory.CreateDirectory(fullName);
//             }
//
//             string fullPath = Path.Combine(fullName, $"{parameter.ScriptName}.lua.txt");
//
//             string content = "";
//
//             bool isFromExist = false;
//             // 如果文件存在，去除需要修改的部分
//             if (File.Exists(fullPath))
//             {
//                 using (StreamReader sReader = new StreamReader(fullPath))
//                 {
//                     content = sReader.ReadToEnd();
//                     sReader.Close();
//                 }
//
//                 // ONPARSE
//                 int parseStartIndex = content.IndexOf(parseStart);
//                 int parseEndIndex = content.IndexOf(parseEnd);
//                 if(parseStartIndex > 0 &&  parseEndIndex > 0)
//                 {
//                     // 替换掉中间的内容
//                     int startIndex = parseStartIndex + parseStart.Length;
//                     string subStr = content.Substring(startIndex, parseEndIndex - startIndex - 2 -4);
//                     content = content.Replace(subStr, "\n#ONPARSE\n");
//                 }
//
//                 // BUTTON_EVENT_ADD_LISTENER
//                 int addListenerStartIndex = content.IndexOf(addListenerStart);
//                 int addListenerEndIndex = content.IndexOf(addListenerEnd);
//                 if (addListenerStartIndex > 0 && addListenerEndIndex > 0)
//                 {
//                     // 替换掉中间的内容
//                     int startIndex = addListenerStartIndex + addListenerStart.Length;
//                     string subStr = content.Substring(startIndex, addListenerEndIndex - startIndex - 2 -4);
//                     content = content.Replace(subStr, "\n#BUTTON_EVENT_ADD_LISTENER\n");
//                 }
//
//                 // BUTTON_EVENT_REMOVE_LISTENER
//                 int removeListenerStartIndex = content.IndexOf(removeListenerStart);
//                 int removeListenerEndIndex = content.IndexOf(removeListenerEnd);
//                 if (removeListenerStartIndex > 0 && removeListenerEndIndex > 0)
//                 {
//                     // 替换掉中间的内容
//                     int startIndex = removeListenerStartIndex + removeListenerStart.Length;
//                     string subStr = content.Substring(startIndex, removeListenerEndIndex - startIndex - 2 - 4);
//                     content = content.Replace(subStr, "\n#BUTTON_EVENT_REMOVE_LISTENER\n");
//                 }
//
//                 // BUTTON_EVENT_HANDLER
//                 int eventHandlerStartIndex = content.IndexOf(eventHandlerStart);
//                 int eventHandlerEndIndex = content.IndexOf(eventHandlerEnd);
//                 if (eventHandlerStartIndex > 0 && eventHandlerEndIndex > 0)
//                 {
//                     // 替换掉中间的内容
//                     int startIndex = eventHandlerStartIndex + eventHandlerStart.Length;
//                     string subStr = content.Substring(startIndex, eventHandlerEndIndex - startIndex - 2);
//                     content = content.Replace(subStr, "\n#BUTTON_EVENT_HANDLER\n");
//                 }
//                 isFromExist = true;
//             }
//             else
//             {
//                 using (StreamReader reader = new StreamReader(scriptTemplatePath))
//                 {
//                     content = reader.ReadToEnd();
//                     reader.Close();
//                 }
//             }
//             if (string.IsNullOrEmpty(content))
//             {
//                 return;
//             }
//
//             if (!isFromExist)
//             {
//                 content = content.Replace("#TIME",          DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss dddd"));
//                 content = content.Replace("#SOURCE",        parameter.SourceName);
//                 content = content.Replace("#SCRIPT_NAME",   parameter.ScriptName);
//                 content = content.Replace("#ASSET_PATH",    parameter.AssetPath);
//             }
//             content = content.Replace("#ONPARSE",                       parameter.LuaInjectContent);
//             content = content.Replace("#BUTTON_EVENT_ADD_LISTENER",     parameter.ButtonAddListenerContent);
//             content = content.Replace("#BUTTON_EVENT_REMOVE_LISTENER",  parameter.ButtonRemoveListenerContent);
//             content = content.Replace("#BUTTON_EVENT_HANDLER",          parameter.ButtonEventHandlersContent);
//
//             using (StreamWriter writer = new StreamWriter(fullPath, false, new System.Text.UTF8Encoding(false)))
//             {
//                 writer.Write(content);
//                 writer.Close();
//             }
//             AssetDatabase.Refresh();
//         }
//     }
// }
