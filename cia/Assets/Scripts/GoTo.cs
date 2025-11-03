using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoTo : MonoBehaviour
{
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
        double time = Time.time; // tempo de início do jogo
        int timeType = 0; // 0 - início, 1 - fim
        int id_jogador = PlayerPrefs.GetInt("PlayerID", 1); // ID do jogador, ou qualquer outra lógica que defina
        int gameID = PlayerPrefs.GetInt("GameID", 123); // ID do jogo
        int resourceID = PlayerPrefs.GetInt("ResourceID", 456); // ID do recurso

        // Criar a mensagem para o servidor
        PlayGameMessage message = new PlayGameMessage(time, timeType, id_jogador, gameID, resourceID);

        // Enviar a mensagem usando o MessageSender
        StartCoroutine(MessageSender.Instance.Send(message, "http://localhost:5000/api"));

        // Armazenar o índice da cena atual
        int index = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("Index", index);

        // Garantir que o jogo esteja em velocidade normal
        Time.timeScale = 1;

        // Carregar a cena
        SceneManager.LoadScene(cena);
    }
}

