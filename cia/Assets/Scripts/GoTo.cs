using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoTo : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        CarregaDados.LoadURL();
        Debug.Log("URL = " + MessageSender.serverURL);
    }

    public void LoadCena(string cena)
    {
        // Definindo o comportamento do jogo baseado no PlayerPrefs
        if (PlayerPrefs.GetInt("LoadCaseId") == 99 && cena == "TelaCasos" && PlayerPrefs.GetInt("PrimeiroTutorial") == 1)
        {
            cena = "MenuPrincipal";
            PlayerPrefs.SetInt("LoadCaseId", 100); // Evitar loop da narrativa 
        }
        else if (PlayerPrefs.GetInt("LoadCaseId") == 99 && cena == "TelaCasos" && PlayerPrefs.GetInt("PrimeiroTutorial") == 3)
        {
            cena = "TelaCasos";
            PlayerPrefs.SetInt("LoadCaseId", 100);
        }

        // Coletar dados para a mensagem
        int playerID = PlayerPrefs.GetInt("PlayerID", 1); // ID do jogador, ou qualquer outra lógica que defina
        int gameID = PlayerPrefs.GetInt("GameID", 123); // ID do jogo
        int resourceID = PlayerPrefs.GetInt("ResourceID", 456); // ID do recurso

        // Criar a mensagem para o servidor
        PlayGameMessage message = new PlayGameMessage(playerID, gameID, resourceID);

        TimeStatsMessage timeStatsMessage = new TimeStatsMessage(playerID, gameID, resourceID, 0, 0); 
     StartCoroutine(MessageSender.Instance.Send(timeStatsMessage));

        // Enviar a mensagem usando o MessageSender
        StartCoroutine(MessageSender.Instance.Send(message));

        // Armazenar o índice da cena atual
        int index = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("Index", index);

        // Garantir que o jogo esteja em velocidade normal
        Time.timeScale = 1;

        // Carregar a cena
        SceneManager.LoadScene(cena);
    }
}

