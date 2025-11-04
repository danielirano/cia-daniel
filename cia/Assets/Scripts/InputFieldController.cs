using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InputFieldController : MonoBehaviour
{
    [SerializeField] private TextAsset frasesFile;
    [SerializeField] private TMP_Text phraseTextBox;
    private List<string> eachLine;
    public string data_string;
    public string input;
    public TMP_InputField inputField;
    public string[] eachPhrase;
    public List<string> wordsRead = new List<string>();
    public AudioClip changePhraseSound;
    public AudioClip rightAnswer;
    private AudioManager audioManager;
    private ObjectivesController objController;
    public List<bool> checkPositions = new List<bool>();
    private int ultimoCaso = 2;
    [SerializeField] GameObject grid;
    [SerializeField] GameObject aviso;
    [SerializeField] GameObject powerUpCanvas;
    [SerializeField] GameObject powerUpLButton;
    [SerializeField] TMP_Text detalhesCaso;
    [SerializeField] TMP_Text detalhesCasoFinal;
    public int countErrors = 0;
    public int countErrorsLast = 0;
    [SerializeField] GameObject avisoFree;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject lupinAviso;
    private Animator lupinAnimacao;
    public GameObject erroTela;
    public GameObject acertoTela;
    private TutorialController TutControl;
    StartTutorial startTut;
    string newPhrase;

    int contpw = 0;

    public int phraseId=0;

    // Start is called before the first frame update
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        lupinAnimacao = GameObject.Find("Informação Lupin").GetComponent<Animator>();
        TutControl = GameObject.Find("Camadas tutorial").GetComponent<TutorialController>();

        if (PlayerPrefs.GetInt("LoadCaseId") == 99)
        {
            startTut = GameObject.Find("Start Tutorial").GetComponent<StartTutorial>();
            string s = startTut.SetTutorial();
            eachPhrase = s.Split(';');
        }
        else
        {
            Read();
        }
    }

    private void Start()
    {
        
        //phraseTextBox.text = eachPhrase[phraseId];
        ReplaceUnderline(eachPhrase[phraseId]);
        objController = GameObject.Find("ObjetivosBG").GetComponent<ObjectivesController>();
        UpdateDetails();
        detalhesCasoFinal.text = "<b>Resolução do caso: ???</b> ";
        //inputField.Select();


    }


    private void Update()
    {
        //inputField.Select();
        if (Input.GetKeyDown(KeyCode.Return)) {
            ReadStringInput();
            EventSystem.current.SetSelectedGameObject(null);
            //inputField.Select();
        }
       // AnimatorStateInfo animSI = lupinAnimacao.GetCurrentAnimatorStateInfo(0); //checa se a animação acabou
        //if (animSI.normalizedTime > 1.0f)
       // {
         //   lupinAnimacao.ResetTrigger("Vai");
           // lupinAnimacao.SetTrigger("Para");
            
        //}

        inputField.Select();

    }


    //criação da mensagem

    void SendWordSendMessage(bool palavraCorreta)
    {
        double time = Time.time;
        string timestats = "palavra";
        int id_jogador = PlayerPrefs.GetInt("id_jogador", 1);
        int gameID = PlayerPrefs.GetInt("gameID", 123);
        int resourceID = PlayerPrefs.GetInt("resourceID", 456);

        WordSendMessage message = new WordSendMessage(time, timestats, id_jogador, gameID, resourceID, palavraCorreta);

        StartCoroutine(MessageSender.Instance.Send(message));
    }

    public void ReadStringInput()
    {
        input = inputField.text;
        inputField.text = "";
        bool palavraCorreta = ValidateWords();
        SendWordSendMessage(palavraCorreta);
        //EventSystem.current.SetSelectedGameObject(null);
       

    }

    public void NextCase(){
        phraseId++;
        if (phraseId <= eachPhrase.Length - ultimoCaso) {
            ReplaceUnderline(eachPhrase[phraseId]);
            //phraseTextBox.text = eachPhrase[phraseId];
            audioManager.PlaySFX(changePhraseSound);
        }
        else
        {
            phraseId--;
        }
        if (TutControl.tutId == 2)
        {
            TutControl.nextStepTutorial();
        }

    }

    public void BackCase()
    {
        phraseId--;
        if (phraseId >= 0) {
            //phraseTextBox.text = eachPhrase[phraseId];
            ReplaceUnderline(eachPhrase[phraseId]);
            audioManager.PlaySFX(changePhraseSound);
        }
        else
        {
            phraseId++;
        }
        if (TutControl.tutId == 2)
        {
            TutControl.nextStepTutorial();
        }
    }

    public void PassWords(List<string> w)
    {
        wordsRead = new List<string>(w);
        foreach(string s in wordsRead)
        {
            checkPositions.Add(false);
        }

        
    }

    void Read()
    {

        data_string = frasesFile.text;
        eachLine = new List<string>();
        eachLine.AddRange(data_string.Split("\n"[0]));
        eachPhrase = eachLine[PlayerPrefs.GetInt("LoadCaseId", 0)].Split(';');

    }



    public bool ValidateWords()
{
    if (wordsRead[phraseId] == input.ToLower()) // Palavra correta
    {
        int pos = phraseId;
        string[] updateString = eachPhrase[pos].Split('_');
        eachPhrase[pos] = updateString[0] + input.ToUpper() + updateString[1];
        ReplaceUnderline(eachPhrase[phraseId]);

        if (pos == eachPhrase.Length - 1 && ultimoCaso == 1)
        {
            audioManager.RightAnswer();
            aviso.SetActive(false);
            TutControl.ObjectiveTut();
            objController.Finish();
        }
        else if (checkPositions[pos] == false && pos != eachPhrase.Length - 1)
        {
            checkPositions[pos] = true;
            audioManager.RightAnswer();

            if (TutControl.tutId == 1)
            {
                TutControl.nextStepTutorial();
            }

            StartCoroutine(StartDelay(acertoTela));
        }

        return true; // ✅ Palavra correta
    }
    else
    {
        CheckErrors();
        audioManager.WrongAnswer();
        StartCoroutine(StartDelay(erroTela));

        if (input.ToLower() == Changecharacters(wordsRead[phraseId]))
        {
            lupinAnimacao.ResetTrigger("Vai");
            lupinAnimacao.SetTrigger("Vai");

            lupinAnimacao.ResetTrigger("Para");
            lupinAnimacao.SetTrigger("Para");
        }

        return false; // ❌ Palavra incorreta
    }
}


    private void UpdateDetails()
    {
        detalhesCaso.text = "";
        int i= 0;
        foreach (string phrase in eachPhrase)
        {
            if (i< eachPhrase.Length -1) {
                detalhesCaso.text = detalhesCaso.text + "\n\n" + phrase.Replace("_", "_____");
            }
            i++;
        }
    }

    public void PowerUpW()
    {
        int i = 0;
        while (checkPositions[i] == true)
        {
            i++;
        }
        input = wordsRead[i];
        phraseId = i;
        ReplaceUnderline(eachPhrase[phraseId]);
        //phraseTextBox.text = eachPhrase[phraseId];
        ValidateWords();

    }

    public void PowerUpL()
    {
        string[] updateString = eachPhrase[eachPhrase.Length - 1].Split('_');
        newPhrase = updateString[0];
        for (int i = 0; i <= contpw; i++)
        {
            newPhrase = newPhrase + wordsRead[eachPhrase.Length - 1].ToUpper()[i] ;
        }
        newPhrase = newPhrase +"_" + updateString[1];
        //phraseTextBox.text = eachPhrase[phraseId];
        ReplaceUnderline(newPhrase);
        contpw++;

    }


    void CheckErrors()
    {
        if ((PlayerPrefs.GetInt("PrecoAjuda") == 0))
        {
            if (phraseId == eachPhrase.Length - 1)
            {
                countErrorsLast++;

            }
            else
            {
                countErrors++;
            }

            if (countErrors == 10 || countErrorsLast == 10 )
            {
                avisoFree.SetActive(true);
                canvas.SetActive(false);
            }
        }
    }



    public void LastWord()
    {
        ultimoCaso = 1;
        phraseId = eachPhrase.Length-1;
        //phraseTextBox.text = eachPhrase[eachPhrase.Length - 1];
        ReplaceUnderline(eachPhrase[phraseId]);
        grid.SetActive(false);
        powerUpCanvas.SetActive(false);
        powerUpLButton.SetActive(true);
        detalhesCasoFinal.text = "<b>Resolução do caso:</b> " + eachPhrase[eachPhrase.Length - 1].Replace("_", "_____");
        aviso.SetActive(true);
        TutControl.nextStepTutorial();

    }

    string Changecharacters(string texto)
    {
        string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
        string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

        for (int i = 0; i < comAcentos.Length; i++)
        {
            texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
        }
        return texto;

    }
    public IEnumerator StartDelay(GameObject feedback)
    {
        feedback.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        feedback.SetActive(false);
        if(feedback.name == "Acerto")
        {
            int i = 0;
            while (checkPositions[i] == true && i < wordsRead.Count - 2)
            {
                i++;
            }
            phraseId = i;
            //phraseTextBox.text = eachPhrase[phraseId];
            ReplaceUnderline(eachPhrase[phraseId]);
            objController.CountObjectivePhrase();
            UpdateDetails();
        }
    }

    public void ReplaceUnderline(string phrase)
    {
        
        phraseTextBox.text = phrase.Replace("_", "_____");
    }
}
