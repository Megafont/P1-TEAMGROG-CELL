using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// This class handles the game's bootup logic (if any), such as loading assets.
/// </summary>
public static class StartUp
{
    public static bool StartUpIsComplete { get; private set; }


    /// <summary>
    /// Handles any startup logic.
    /// </summary>
    public static void DoStartUpWork()
    {
        StartUpIsComplete = true;
    }
}
