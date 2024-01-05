using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlowManager
{
    public static void SelectFlow<T>(T ins) where T : IFlowController
    {
        ins.Init();
    }
}