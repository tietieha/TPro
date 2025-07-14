using System;
using UnityEngine;

public static class ComponentEx
{
    public static T GetOrAddComponent<T>(this Component comp, bool set_enable = false) where T : Component
    {
        if (comp == null)
            return null;

        return GetOrAddComponent<T>(comp.gameObject, set_enable);
    }

    public static T GetOrAddComponent<T>(this GameObject go, bool set_enable = false) where T : Component
    {
        if (go == null)
            return null;

        T result = go.GetComponent<T>();
        if (result == null)
            result = go.AddComponent<T>();

        var bcomp = result as Behaviour;
        if (set_enable && bcomp != null)
            bcomp.enabled = set_enable;

        return result;
    }

    public static Component GetOrAddComponent(this Component comp, Type type, bool set_enable = false)
    {
        if (comp == null)
            return null;

        return GetOrAddComponent(comp.gameObject, type, set_enable);
    }

    public static Component GetOrAddComponent(this GameObject go, Type type, bool set_enable = false)
    {
        if (go == null)
            return null;

        Component result = go.GetComponent(type);
        if (result == null)
            result = go.AddComponent(type);

        var bcomp = result as Behaviour;
        if (set_enable && bcomp != null)
            bcomp.enabled = set_enable;

        return result;
    }
}

public static class TransformExtentions
{
    public static Transform GetOrAddTransform(this Transform parent, string childName, Vector3 position, Vector3 roll)
    {
        Transform t = GetOrAddTransform(parent, childName);
        if (t != null)
        {
            t.localPosition = position;
            t.localRotation = Quaternion.Euler(roll);
        }

        return t;
    }

    public static Transform GetOrAddTransform(this Transform parent, string childName, Vector3 position,
        Quaternion rotation)
    {
        Transform t = GetOrAddTransform(parent, childName);
        if (t != null)
        {
            t.localPosition = position;
            t.localRotation = rotation;
        }

        return t;
    }

    public static Transform GetOrAddTransform(this Transform parent, string childName)
    {
        if (string.IsNullOrEmpty(childName))
            return parent;

        Transform t = parent?.Find(childName);
        if (t == null)
        {
            int index = childName.IndexOf('/');
            if (index >= 0)
            {
                string folder = childName.Substring(0, index);
                childName = childName.Substring(index + 1);

                return GetOrAddTransform(GetOrAddTransform(parent, folder), childName);
            }

            t = new GameObject(childName).transform;
            t.SetParent(parent, false);
        }

        return t;
    }
}