using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeSave : MonoBehaviour
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        var config = new FBPPConfig()
        {
            SaveFileName = "save-file.txt",
            AutoSaveData = true,
            ScrambleSaveData = false
        };
        FBPP.Start(config);
    }
}

