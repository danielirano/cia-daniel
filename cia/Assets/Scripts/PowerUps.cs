using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour
{
    private float coins=0;
    [SerializeField] private TMP_Text coinDisplay;
    [SerializeField] private TextAsset consultaFile;
    private Timer timer;
    private WordHunt wh;
    private ObjectivesController objContr;
    private InputFieldController inpFController;
    private List<string> eachLine;
    public string data_string;
    float cheaperPW = 1;
    public Button[] powerUpButton;
    [SerializeField] GameObject avisoFree;
    private bool[] ajudasUsadas = new bool[] {false, false, false};
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject canvasConsulta;
    [SerializeField] private GameObject canvasDetalhes;
    [SerializeField] private TMP_Text consultaText;
    [SerializeField] private Sprite[] moedas;
    [SerializeField] private SpriteRenderer moedasDisplay;
    private TutorialController TutControl;
    StartTutorial startTut;

    //adicao dos contadores 
        private int countPowerUpTime = 0;
        private int countPowerUpLetter = 0;
        private int countPowerUpWord = 0;
        private int detalhesUsado = 0;
        private int countPowerUpUltimo = 0;



        private int totalPowerUpUsado => countPowerUpTime + countPowerUpLetter + countPowerUpWord + countPowerUpUltimo;


    //funcao para enviar 
    private void SendPowerUpMessage(int powerupType, int usoAtual)
    {
        double timeNow = Time.realtimeSinceStartup;
        string timestats = "usou powerup tipo " + powerupType;
        int idJogador = PlayerPrefs.GetInt("PlayerID", 1);
        int gameID = PlayerPrefs.GetInt("GameID", 123);
        int resourceID = PlayerPrefs.GetInt("resourceID", 456); 
        bool powerupUsado = true;

        PowerUpMessage message = new PowerUpMessage(
            powerupType,
            timeNow,
            timestats,
            idJogador,
            gameID,
            resourceID,
            powerupUsado,
            usoAtual
        );

        StartCoroutine(MessageSender.Instance.Send(message));
    }
    //detalhes do caso
    void SendCaseDetailsMessage(int detalhesUtilizado)
    {
        double time = Time.time;
        string timestats = "acessou os detalhes do caso";
        int id_jogador = PlayerPrefs.GetInt("PlayerID", 1);
        int gameID = PlayerPrefs.GetInt("gameID", 123);
        int resourceID = PlayerPrefs.GetInt("resourceID", 456);

        CaseDetailsMessage message = new CaseDetailsMessage(
            time, timestats, id_jogador, gameID, resourceID, true, detalhesUtilizado
        );

        StartCoroutine(MessageSender.Instance.Send(message));
    }



    void Awake()
    {
        wh = GameObject.Find("WordHunt").GetComponent<WordHunt>();
        inpFController = GameObject.Find("TelaJogo").GetComponent<InputFieldController>();
        objContr = GameObject.Find("ObjetivosBG").GetComponent<ObjectivesController>();
        timer = GameObject.Find("TelaJogo").GetComponent<Timer>();
        moedasDisplay = GameObject.Find("moedas1").GetComponent<SpriteRenderer>();
        TutControl = GameObject.Find("Camadas tutorial").GetComponent<TutorialController>();
        Read();
        if (PlayerPrefs.GetInt("PrecoAjuda") == 0)
        {
            cheaperPW = 0.5f;
        }
        CheckCoins();
        InitButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseCoins(){

        coins = coins + 40;
        coinDisplay.text = coins.ToString();
        CheckCoins();
    }

    public void PowerUpTime()
    {
        if (coins >= 50 * cheaperPW) {
            coins = coins - 50 * cheaperPW;
            timer.timer += 60;
            coinDisplay.text = coins.ToString();
            CheckCoins();

            countPowerUpTime++;
            SendPowerUpMessage(1, totalPowerUpUsado);

        }
    }
    public void PowerUpLetter()
    {
        if (coins >= 100 * cheaperPW)
        {
            coins = coins - 100 * cheaperPW;
            wh.DicaLetra();
            coinDisplay.text = coins.ToString();
            CheckCoins();

            countPowerUpLetter++;
            SendPowerUpMessage(2, totalPowerUpUsado);

            if (TutControl.tutId == 3)
            {
                TutControl.nextStepTutorial();
            }


        }
    }

    public void PowerUpWord()
    {
        if (coins >= 150 * cheaperPW)
        {
            coins = coins - 150 * cheaperPW;
            coinDisplay.text = coins.ToString();
            inpFController.PowerUpW();
            CheckCoins();

            countPowerUpWord++;
            SendPowerUpMessage(3, totalPowerUpUsado);

            if (TutControl.tutId == 3)
            {
                TutControl.nextStepTutorial();
            }
        }
    }

    public void PowerUpLast()
    {
        if (coins >= 200 * cheaperPW)
        {
            coins = coins - 200 * cheaperPW;
            coinDisplay.text = coins.ToString();
            inpFController.PowerUpL();
            CheckCoins();

            countPowerUpUltimo++;
            SendPowerUpMessage(4, totalPowerUpUsado);
        }
        
    }

    public void FreeHint()
    {
        int id = PlayerPrefs.GetInt("LoadCaseId", 0);
        if (TutControl.tutId == 4)
        {
            TutControl.nextStepTutorial();
        }
        if (id == 99)
        {
            startTut = GameObject.Find("Start Tutorial").GetComponent<StartTutorial>();
            consultaText.text = startTut.tutorialFreeHint;
        }
        else
        {
            consultaText.text = eachLine[id];
            
        }
        canvasConsulta.SetActive(true);
        //Application.OpenURL(eachLine[PlayerPrefs.GetInt("LoadCaseId", 0)]);
    }

    public void MostrarDetalhes()
    {
        canvasDetalhes.SetActive(true);
        if (TutControl.tutId == 5)
        {
            TutControl.nextStepTutorial();
        }
         // coleta de dados
             detalhesUsado++;
             SendCaseDetailsMessage(detalhesUsado);
    }

    public void InitButtons()
    {
        TMP_Text a;
        float b;
        if (PlayerPrefs.GetInt("Tempo", 0) == 1)
        {
            a = powerUpButton[0].GetComponentInChildren<TMP_Text>();
            b = 50 * cheaperPW;
            a.text = b.ToString() + " - Mais tempo";
        }
        else
        {
            a = powerUpButton[0].GetComponentInChildren<TMP_Text>();
            a.text =  "Power up indisponível";
        }

        a = powerUpButton[1].GetComponentInChildren<TMP_Text>();
        b = 100 * cheaperPW;
        a.text = b.ToString() + " - Revela uma letra";

        if (objContr.contadorFrases == objContr.totalPalavras)
        {
            a = powerUpButton[2].GetComponentInChildren<TMP_Text>();
            a.text = "Power up indisponível";


        }
        else
        {
            a = powerUpButton[2].GetComponentInChildren<TMP_Text>();
            b = 150 * cheaperPW;
            a.text = b.ToString() + " - Completa uma pista";
        }
        
        a = powerUpButton[3].GetComponentInChildren<TMP_Text>();
        b = 200 * cheaperPW;
        a.text = b.ToString() + " - Revela uma letra da palavra final";
        

    }

    private void CheckCoins()
    {
        if(coins > 0)
        {
            moedasDisplay.sprite = moedas[1];
        }
        else
        {
            moedasDisplay.sprite = moedas[0];
        }

        if(coins >= 50 * cheaperPW && PlayerPrefs.GetInt("Tempo", 0) == 1)
        {
            powerUpButton[0].interactable = true;
            moedasDisplay.sprite = moedas[2];
        }
        else
        {
            powerUpButton[0].interactable = false;
        }

        if (coins >= 100 * cheaperPW && objContr.contadorPalavras != objContr.totalPalavras)
        {
            //Debug.Log(objContr.allwordsfound);
            powerUpButton[1].interactable = true;
            moedasDisplay.sprite = moedas[3];

        }
        else
        {
            powerUpButton[1].interactable = false;
        }

        if (coins >= 150 * cheaperPW && objContr.contadorFrases != objContr.totalPalavras)
        {
            powerUpButton[2].interactable = true;
            moedasDisplay.sprite = moedas[4];

        }
        else
        {
            powerUpButton[2].interactable = false;
            InitButtons();
        }

        if (coins >= 200 * cheaperPW)
        {
            powerUpButton[3].interactable = true;

        }
        else
        {
            powerUpButton[3].interactable = false;
        }
    }

    public void FreePW()
    {
        if(wh.countErrors == 10 && ajudasUsadas[0]== false)
        {

            ajudasUsadas[0] = true;
            coins += 50;
            PowerUpLetter();
        }
        else if (inpFController.countErrors == 10 && ajudasUsadas[1] == false)
        {
            ajudasUsadas[1] = true;
            coins += 75;
            PowerUpWord();
        }
        else if (inpFController.countErrorsLast == 10 && ajudasUsadas[2] == false)
        {
            ajudasUsadas[2] = true;
            coins += 125;
            PowerUpLast();
        }
        canvas.SetActive(true);
        avisoFree.SetActive(false);
    }

    void Read()
    {

        data_string = consultaFile.text;
        eachLine = new List<string>();
        eachLine.AddRange(data_string.Split("|"[0]));
        

    }
}
