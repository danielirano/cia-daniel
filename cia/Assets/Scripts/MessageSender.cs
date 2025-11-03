using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class MessageSender : MonoBehaviour
{
    // Singleton
    private static MessageSender _instance;

    public static string cookieValue = "";
    public static MessageSender Instance {
        get {
            if (_instance == null)
            {
                GameObject go = new GameObject("MessageSender");
                _instance = go.AddComponent<MessageSender>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // Padrão Singleton
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    // Enviar a mensagem no formato JSON para o servidor
    public IEnumerator Send<T>(T message, string url) where T : Message
    {
        // Serializa a mensagem para JSON
        string jsonMessage = JsonUtility.ToJson(message);

        // Cria uma requisição POST para enviar o JSON
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonMessage);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json"); // Cabeçalho para JSON

            // Envia a requisição
            yield return request.SendWebRequest();

            // Verifica se houve erro na requisição
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erro na requisição HTTP (JSON): " + request.error);
            }
            else
            {
                Debug.Log("Requisição enviada com sucesso.");
                Debug.Log("Resposta do servidor: " + request.downloadHandler.text);
            }
        }
    }
}

