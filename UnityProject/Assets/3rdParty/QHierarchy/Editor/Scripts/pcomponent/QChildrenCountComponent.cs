using qtools.qhierarchy.pcomponent.pbase;
using qtools.qhierarchy.pdata;
using UnityEditor;
using UnityEngine;

namespace qtools.qhierarchy.pcomponent
{
    public class QChildrenCountComponent : QBaseComponent
    {
        // PRIVATE
        private GUIStyle labelStyle;
        private GUIStyle hintLabelStyle;

        // CONSTRUCTOR
        public QChildrenCountComponent()
        {
            
            labelStyle = new GUIStyle();
            labelStyle.fontSize = 9;
            labelStyle.clipping = TextClipping.Clip;
            labelStyle.alignment = TextAnchor.MiddleRight;
            
            hintLabelStyle = new GUIStyle();
            hintLabelStyle.normal.textColor = QResources.getInstance().getColor(QColor.Gray);
            hintLabelStyle.fontSize = 11;
            hintLabelStyle.clipping = TextClipping.Clip;

            rect.width = 22;
            rect.height = 16;

            QSettings.getInstance().addEventListener(QSetting.ChildrenCountShow, settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ChildrenCountShowDuringPlayMode, settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ChildrenCountLabelSize, settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ChildrenCountLabelColor, settingsChanged);
            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            enabled = QSettings.getInstance().get<bool>(QSetting.ChildrenCountShow);
            showComponentDuringPlayMode = QSettings.getInstance().get<bool>(QSetting.ChildrenCountShowDuringPlayMode);
            QHierarchySize labelSize = (QHierarchySize) QSettings.getInstance().get<int>(QSetting.ChildrenCountLabelSize);
            labelStyle.normal.textColor = QSettings.getInstance().getColor(QSetting.ChildrenCountLabelColor);
            labelStyle.fontSize = labelSize == QHierarchySize.Normal ? 8 : 9;
            rect.width = labelSize == QHierarchySize.Normal ? 17 : 22;
        }

        // DRAW
        public override QLayoutStatus layout(GameObject gameObject, QObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            if (maxWidth < rect.width)
            {
                return QLayoutStatus.Failed;
            }
            else
            {
                curRect.x -= rect.width + 2;
                rect.x = curRect.x;
                rect.y = curRect.y;
                rect.y += (EditorGUIUtility.singleLineHeight - rect.height) * 0.5f;
                rect.height = EditorGUIUtility.singleLineHeight;
                return QLayoutStatus.Success;
            }
        }

        public override void draw(GameObject gameObject, QObjectList objectList, Rect selectionRect)
        {
            var transform = gameObject.transform;
            int childrenCount = transform.childCount;
            if (childrenCount > 0)
            {
                GUI.Label(rect, childrenCount.ToString(), labelStyle);
                // 2020/6/6 17:09 teddysjwu: 增加显示当前可见节点提示
                if (rect.Contains(Event.current.mousePosition))
                {
                    int visibleChildrenCount = 0;
                    for (int i = 0; i < childrenCount; i++)
                    {
                        if (transform.GetChild(i).gameObject.activeSelf)
                        {
                            visibleChildrenCount++;
                        }
                    }
                    string Tips = $"{visibleChildrenCount}/{childrenCount}";
                    int labelWidth = Mathf.CeilToInt(hintLabelStyle.CalcSize(new GUIContent(Tips)).x);
                    selectionRect.x = rect.x - labelWidth / 2 - 4;
                    selectionRect.width = labelWidth + 8;
                    selectionRect.height -= 1;

                    if (selectionRect.y > 16) selectionRect.y -= 16;
                    else selectionRect.x += labelWidth / 2 + 18;

                    EditorGUI.DrawRect(selectionRect, QResources.getInstance().getColor(QColor.BackgroundDark));
                    selectionRect.x += 4;
                    selectionRect.y += (EditorGUIUtility.singleLineHeight - rect.height) * 0.5f;
                    selectionRect.height = EditorGUIUtility.singleLineHeight;

                    EditorGUI.LabelField(selectionRect, Tips, hintLabelStyle);
                }
            }
        }
    }
}