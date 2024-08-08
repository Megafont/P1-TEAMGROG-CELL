using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains utility methods for working with GameObjects.
/// </summary>
public static class GameObjectUtils
{
    /// <summary>
    /// This function is similar to Unity's built-in Transform.Find() function, except
    /// that it is recursive. This means it can find a child object by name, even if
    /// it is a child of another child.
    /// </summary>
    /// <remarks>
    /// This is an extension method, meaning you can call it directly on any Transform object.
    /// That is why the this keyword appears in the parameters list.
    /// 
    /// If the passed in object has multiple children who have the specified name,
    /// then this function will return the first one it finds.
    /// </remarks>
    /// <param name="parent">The object whose children will be searched.</param>
    /// <param name="name">The name of the child object to find.</param>
    public static GameObject FindResursive(this Transform parent, string name)
    {
        Transform t = parent.Find(name);
        if (t != null)
            return t.gameObject;

        // We did not find an object with the specified name in the direct
        // children of parent, so search the children of each child.
        foreach (Transform child in parent)
        {
            GameObject g = child.FindResursive(name);
            if (g != null)
                return g;
        }

        // No matching object found, so return null.
        return null;
    }
}
