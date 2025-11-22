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

        // Armazenar o índice da cena atual
        int index = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("Index", index);

        // Garantir que o jogo esteja em velocidade normal
        Time.timeScale = 1;

        // Carregar a cena
        SceneManager.LoadScene(cena);
    }
}

