using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeveloperScreen
{
    private static bool debugSSAO = false;

    public static bool DebugSSAO
    {
        get
        {
            return debugSSAO;
        }
        set
        {
            debugSSAO = value;
        }
    }
}
