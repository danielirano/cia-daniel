using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;

public class CarregaDados : MonoBehaviour
{
    
    public static void LoadURL()
    {
        BetterStreamingAssets.Initialize();

        MessageSender.serverURL = BetterStreamingAssets.ReadAllLines("url.txt")[0];
    }

}