using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ObjectivesController : MonoBehaviour
{
    [SerializeField] private TMP_Text palavrasTexto;
    [SerializeField] private TMP_Text frasesTexto;
    [SerializeField] private TMP_Text casoTexto;
    public int contadorPalavras=0;
    public int contadorFrases=0;
    public int totalPalavras = 0;
    [SerializeField]private Timer timer;
    private InputFieldController inputController;
    [SerializeField] GameObject casoEncerrado;
    [SerializeField] GameObject avisoFimTutorial;
    private PowerUps powerUps;
    public GameObject levelChanger;
    
    


    void Start()
    {
        timer = GameObject.Find("TelaJogo").GetComponent<Timer>();
        inputController = GameObject.Find("TelaJogo").GetComponent<InputFieldController>();
        powerUps = GameObject.Find("PowerUp controller").GetComponent<PowerUps>();
        //fade = GameObject.Find("LevelChanger").GetComponent<Animator>();
        palavrasTexto.text = "Ache as palavras (" + contadorPalavras + "/" + totalPalavras + ")";
        frasesTexto.text = "Complete as frases (" + contadorFrases + "/" + totalPalavras + ")";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountObjective(string foundword) //atualiza o número de palavras encontradas no caça
    {
        contadorPalavras++;
        palavrasTexto.text = "Ache as palavras ("+ contadorPalavras +"/" + totalPalavras + ")";
        powerUps.IncreaseCoins();
        if(contadorPalavras == totalPalavras)
        {
            CheckFinish();
        }
    }

    public void CountObjectivePhrase() //atualiza o número de palavras encontradas na frase
    {
        contadorFrases++;
        frasesTexto.text = "Complete as frases (" + contadorFrases+ "/" + totalPalavras + ")";
        powerUps.IncreaseCoins();
        if (contadorFrases == totalPalavras)
        {
            CheckFinish();
        }
    }

    public void SetNumberOfWords(int number)
    {
        totalPalavras = number - 1;
        palavrasTexto.text = "Ache as palavras (0/" + totalPalavras + ")";
    }

    private void CheckFinish()
    {
        if(contadorPalavras == totalPalavras && contadorFrases == totalPalavras)
        {
            inputController.LastWord();
        }
    }

    public void Finish()
    {
        casoTexto.text = "Conclusão do caso (1/1)";

        if (PlayerPrefs.GetInt("LoadCaseId") == 99)
        {
            avisoFimTutorial.SetActive(true);
        }
        else
        {
            StartCoroutine(inputController.StartDelay(inputController.acertoTela));
            casoEncerrado.SetActive(true);
            
            int caseID = PlayerPrefs.GetInt("LoadCaseId", 0);
            TimeStatsMessage timeStatsMessage = new TimeStatsMessage(TimeEvent.Case, TimeType.End, caseID);  
            StartCoroutine(MessageSender.Instance.Send(timeStatsMessage));

            string caseIDString = "RecordeCaso" + PlayerPrefs.GetInt("LoadCaseId", 0) + PlayerPrefs.GetInt("Tempo", 0) + PlayerPrefs.GetInt("PrecoAjuda", 0) + PlayerPrefs.GetInt("PalavrasInvertidas", 0) +
            PlayerPrefs.GetInt("PalavrasDiagonais", 0);
            if (timer.runTimer < PlayerPrefs.GetFloat(caseIDString, 3000))
            {
                PlayerPrefs.SetFloat(caseIDString, timer.runTimer);
            }
            levelChanger.SetActive(true);
            
            StartCoroutine(StartDelay());
        }
        
    }

    public IEnumerator StartDelay()
    {
        //fade.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("TelaCasos");
    }
}
