using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class CarregaDados : MonoBehaviour
{
    public static Configuracao conf;

    public static void Load(MonoBehaviour behaviour)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            behaviour.StartCoroutine(GetByHTTP());
        #else
            GetByBSA();
        #endif
    }

    private static IEnumerator GetByHTTP() {
        Debug.Log(Application.streamingAssetsPath);
        string URL = Path.Combine(Application.streamingAssetsPath, "dados.json");
        Debug.Log(URL);
        using(UnityWebRequest www = UnityWebRequest.Get(URL)) {
        	yield return www.SendWebRequest();
 
        	if (www.result == UnityWebRequest.Result.ProtocolError || 
                www.result == UnityWebRequest.Result.ConnectionError) {
            	Debug.Log(www.error);
        	}
        	else {
            	
                Debug.Log(www.downloadHandler.text);
                var jsonText = www.downloadHandler.text;
                conf = JsonUtility.FromJson<Configuracao>(jsonText);
            }
        }
    }

    private static void GetByBSA() {
        BetterStreamingAssets.Initialize();
        var jsonText = BetterStreamingAssets.ReadAllText("dados.json");
        Debug.Log(jsonText);
        conf = JsonUtility.FromJson<Configuracao>(jsonText);
    }
}

[System.Serializable]
public class Configuracao
{
    public string serverURL;
    public int resourceID;
    public int gameID;
}