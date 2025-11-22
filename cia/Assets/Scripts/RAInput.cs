using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RAInput : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI outputText;

    void Start()
    {
        // Set initial text
        inputField.text = "Digite o RA...";
        outputText.text = "";
        CarregaDados.Load(this);
        Debug.Log("URL = " + CarregaDados.conf.serverURL);
    }

    public void Check()
    {
        string value = inputField.text;

        Debug.Log("Valor digitado: " + value);

        int RA;
        if (int.TryParse(value, out RA))
        {
            Debug.Log("Conversão OK");
            CarregaDados.conf.playerID = RA;
            
            // Criar a mensagem para o servidor
            PlayGameMessage message = new PlayGameMessage();
            
            // Enviar a mensagem usando o MessageSender
            StartCoroutine(MessageSender.Instance.Send(message));
            
            TimeStatsMessage timeStatsMessage = new TimeStatsMessage(TimeEvent.Game, TimeType.Begin); 
            
            StartCoroutine(MessageSender.Instance.Send(timeStatsMessage));

            SceneManager.LoadScene("MenuPrincipal");
        }
        else
        {
            // Conversion failed
            outputText.text = "RA Inválido";
            inputField.text = "Digite o RA...";
        }
    }
}