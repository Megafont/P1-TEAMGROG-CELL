using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// This class handles the game's shutdown logic, if any.
/// </summary>
public static class ShutDown
{
    public static bool ShutDownIsComplete { get; private set; }


    /// <summary>
    /// Handles any shutdown/cleanup logic.
    /// </summary>
    public static void DoShutDownWork()
    {
        ShutDownIsComplete = true;
    }
}
