using TEngine;
using UnityEngine;

namespace XLua
{
    public class LuaBehaviour : MonoBehaviour
    {
        private static Object s_Null = null;

        public string tableName;
        public LuaObjectReference[] references;

        private bool m_Injected;
        private LuaTable m_LuaTable;

        public void Inject(LuaTable target)
        {
            if (target == null)
            {
                Log.Error("target is null, inject failed", this);
                return;
            }

            if (m_Injected)
            {
                Log.Error($"target is injected, not duplicated inject {name} {tableName}", this);
                return;
            }

            m_Injected = true;
            m_LuaTable = target;
            foreach (var reference in references)
            {
                if (reference.Object == null) continue;
                string injectName = string.IsNullOrEmpty(reference.Name) ? reference.Component.name : reference.Name;
                if(reference.Component == null)
                {
                    Log.Error($"Error {gameObject.name } 上的LuaBehaviour组件 injectName = {injectName} 找不到组件,请重新刷新赋值");
                    RefreshObjectReference(reference);
                }
                m_LuaTable.Set(injectName, reference.Component);
            }
        }

        public void Eject()
        {
            if (!m_Injected || m_LuaTable == null)
            {
                return;
            }

            foreach (var reference in references)
            {
                if (reference.Object == null) continue;
                string injectName = string.IsNullOrEmpty(reference.Name) ? reference.Component.name : reference.Name;
                m_LuaTable.Set(injectName, s_Null);
            }

            m_Injected = false;
            m_LuaTable = null;
        }

        public LuaTable GetLuaScript()
        {
            return m_LuaTable;
        }


        //刷新所有引用对象的Component信息，这个为工具使用
        public void RefreshAllObjectReference()
        {
            for (int i = 0; i < references.Length; i++)
            {
                RefreshObjectReference(references[i]);
            }
        }
        //刷新引用对象的Component信息，这个为工具使用
        public void RefreshObjectReference(LuaObjectReference reference)
        {
            if(reference.Object == null) return;
            Component[] components = reference.Object.GetComponents<Component>();
            var count = components.Length;
            if (reference.TypeIndex < count)
            {
                reference.Component = components[reference.TypeIndex];
            }
            else
            {
                reference.Component = reference.Object;
                reference.TypeIndex = count;
            }
        }

        private void OnDestroy()
        {
            Eject();
        }
    }
}
