using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CaseController : MonoBehaviour
{

    public string[] data_sentences;
    public int line;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private TextAsset casoDetalhesFile;
    [SerializeField] private TextAsset tamanhoGridFile;
    [SerializeField] private TextAsset numeroCasosPrincipaisFile;
    public List<string> caseDetails;
    public List<string> caseSize;
    public string data_string;
    public int caseID = 0;
    //public string  caseSize;
    public TMP_Text caseText;
    public TMP_Text caseTitle;
    public TMP_Text recordCaseText;
    //private float[] medalTimes = new float[] {240,210,180,210,180,150,180,150,120};
    private float[] mTimes = new float[] { 60, 90, 120, 90, 120, 150, 120, 150, 180 };
    private float[] mTimesExtra = new float[] { 40, 60, 90, 60, 90, 120, 90, 120, 150 };

    [SerializeField] TMP_Text caseSizeText;
    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text silverText;
    [SerializeField] TMP_Text bronzeText;
    [SerializeField] TMP_Text showMode;
    [SerializeField] Image medalImage;
    [SerializeField] GameObject casoAberto;
    [SerializeField] GameObject casoFechado;
    [SerializeField] GameObject filtro;
    [SerializeField] GameObject fadein;
    private PresetsController presets;
    [SerializeField] GameObject recordBox;
    private int countMainCases = 0;
    public int mainCasesNumber;
    public GameObject recordShow;
    public GameObject medalInfo;
    public GameObject avisoCasoExtra;
    public GameObject playButton;
    public CertificateController certificateController;



    // Start is called before the first frame update
    private void Awake()
    {
        presets = GameObject.Find("PresetsController").GetComponent<PresetsController>();
        certificateController = GameObject.Find("CertificateController").GetComponent<CertificateController>();

        Read();
        
    }
    void Start()
    {
        
        ShowCase();
        SearchForUnfinished();
        CheckNarrative();
        CheckCertificate();
        



    }


    public void ShowCase()
    {
        presets.LoadPreferences();
        if (presets.presetTempo == 0 && presets.presetPreco == 0 && presets.presetDiagonal == 0 && presets.presetInvertida ==0)
        {
            showMode.text = "Modo selecionado: Livre";
        }
        else if (presets.presetTempo == 1 && presets.presetPreco == 1 && presets.presetDiagonal == 0 && presets.presetInvertida == 0)
        {
            showMode.text = "Modo selecionado: Padrão";
        }
        else if (presets.presetTempo == 1 && presets.presetPreco == 1 && presets.presetDiagonal == 1 && presets.presetInvertida == 1)
        {
            showMode.text = "Modo selecionado: Desafiador";
        }
        else {
            showMode.text = "Modo selecionado: Personalizado";
        }

        if (caseID == 0)
        {
            backButton.SetActive(false);
            nextButton.SetActive(true);
        }
        else if (caseID == caseDetails.Count - 1)
        {
            nextButton.SetActive(false);
            backButton.SetActive(true);
        }
        else
        {

                backButton.SetActive(true);
                nextButton.SetActive(true);
            
        }
        

        caseText.text = "";
        caseText.text = caseDetails[caseID];
       
        switch (caseSize[caseID])
        {
            case "P":
                caseSizeText.text = "Tamanho do tabuleiro: Pequeno";
                break;
            case "M":
                caseSizeText.text = "Tamanho do tabuleiro: Médio";
                break;
            case "G":
                caseSizeText.text = "Tamanho do tabuleiro: Grande";
                break;

        }

        //caseSizeText.text = "Tamanho do tabuleiro: " + caseSize[caseID];
        //caseTitle.text = "Caso " + (caseID + 1);
        if (caseID >= mainCasesNumber)
        {
            filtro.SetActive(true);
            caseTitle.text = "Caso extra " + (caseID + 1 - mainCasesNumber);
            if(countMainCases < mainCasesNumber)
            {
                avisoCasoExtra.SetActive(true);
                playButton.SetActive(false);
            }
            else
            {
                avisoCasoExtra.SetActive(false);
                playButton.SetActive(true);
            }

        }
        else
        {
            avisoCasoExtra.SetActive(false);
            playButton.SetActive(true);
            filtro.SetActive(false);
            caseTitle.text = "Caso principal " + (caseID + 1);
        }
        SetTimeObjectives(caseID);
        ShowRecords(caseID);
        CheckCertificate();

    }

    public void ShowRecords(int id)
    {

        string caseIDString = "RecordeCaso" + id + presets.presetTempo.ToString() + presets.presetPreco.ToString() + presets.presetInvertida.ToString() + presets.presetDiagonal.ToString();
        float recordecaso = PlayerPrefs.GetFloat(caseIDString, 3000);
        Debug.Log("o record funcionando" + recordecaso);
        if (presets.presetTempo != 0)
        {
            recordShow.SetActive(true);
            medalInfo.SetActive(true);
            if (recordecaso == 3000)
            {
                recordCaseText.text = "Sem recordes";
                casoFechado.SetActive(false);
                casoAberto.SetActive(true);
            }
            else
            {
                recordCaseText.text = SecondsToMinutes(recordecaso);
                casoFechado.SetActive(true);
                casoAberto.SetActive(false);
            }
            if(id < mainCasesNumber){
                switch (caseSize[id])
                {
                    case "P":
                        if (recordecaso <= mTimes[0])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Ouro").GetComponent<Image>().sprite;

                        }
                        else if (recordecaso <= mTimes[1])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Prata").GetComponent<Image>().sprite;

                        }
                        else if (recordecaso <= mTimes[2])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Bronze").GetComponent<Image>().sprite;

                        }
                        else
                        {
                            medalImage.sprite = GameObject.Find("Transparente").GetComponent<Image>().sprite;
                        }


                        break;
                    case "M":
                        if (recordecaso <= mTimes[3])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Ouro").GetComponent<Image>().sprite;

                        }
                        else if (recordecaso <= mTimes[4])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Prata").GetComponent<Image>().sprite;

                        }
                        else if (recordecaso <= mTimes[5])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Bronze").GetComponent<Image>().sprite;

                        }
                        else
                        {
                            medalImage.sprite = GameObject.Find("Transparente").GetComponent<Image>().sprite;
                        }

                        break;
                    case "G":
                        if (recordecaso <= mTimes[6])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Ouro").GetComponent<Image>().sprite;

                        }
                        else if (recordecaso <= mTimes[7])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Prata").GetComponent<Image>().sprite;

                        }
                        else if (recordecaso <= mTimes[8])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Bronze").GetComponent<Image>().sprite;

                        }
                        else
                        {
                            medalImage.sprite = GameObject.Find("Transparente").GetComponent<Image>().sprite;
                        }

                        break;
                }

            }
            else
            {
                switch (caseSize[id])
                {
                    case "P":
                        if (recordecaso <= mTimesExtra[0])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Ouro").GetComponent<Image>().sprite;

                        }
                        else if (recordecaso <= mTimesExtra[1])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Prata").GetComponent<Image>().sprite;

                        }
                        else if (recordecaso <= mTimesExtra[2])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Bronze").GetComponent<Image>().sprite;

                        }
                        else
                        {
                            medalImage.sprite = GameObject.Find("Transparente").GetComponent<Image>().sprite;
                        }


                        break;
                    case "M":
                        if (recordecaso <= mTimesExtra[3])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Ouro").GetComponent<Image>().sprite;

                        }
                        else if (recordecaso <= mTimesExtra[4])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Prata").GetComponent<Image>().sprite;

                        }
                        else if (recordecaso <= mTimesExtra[5])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Bronze").GetComponent<Image>().sprite;

                        }
                        else
                        {
                            medalImage.sprite = GameObject.Find("Transparente").GetComponent<Image>().sprite;
                        }

                        break;
                    case "G":
                        if (recordecaso <= mTimesExtra[6])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Ouro").GetComponent<Image>().sprite;

                        }
                        else if (recordecaso <= mTimesExtra[7])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Prata").GetComponent<Image>().sprite;

                        }
                        else if (recordecaso <= mTimesExtra[8])
                        {
                            medalImage.sprite = GameObject.Find("Medalha Bronze").GetComponent<Image>().sprite;

                        }
                        else
                        {
                            medalImage.sprite = GameObject.Find("Transparente").GetComponent<Image>().sprite;
                        }

                        break;
                }
            }
        }
        else
        {
            recordShow.SetActive(false);
            medalInfo.SetActive(false);
            if (recordecaso == 3000)
            {
                
                recordCaseText.text = "Não concluído";
                casoFechado.SetActive(false);
                casoAberto.SetActive(true);
            }
            else
            {

                recordCaseText.text = "Concluído com sucesso";
                casoFechado.SetActive(true);
                casoAberto.SetActive(false);
            }
        }


    }


    public void NextId()
    {
        caseID++;
        ShowCase();

    }
    public void BackId()
    {
        caseID--;
        ShowCase();
    }

    //public void StartCase()
    //{
      //  PlayerPrefs.SetInt("LoadCaseId", caseID);
        //PlayerPrefs.SetString("LoadCaseSize", caseSize[caseID]);
        //SceneManager.LoadScene("TelaJogo");
    //}

      public void StartCase()
    {
        PlayerPrefs.SetInt("LoadCaseId", caseID);
        PlayerPrefs.SetString("LoadCaseSize", caseSize[caseID]);

        string timestats = "iniciou o caso";
        int playerID = PlayerPrefs.GetInt("PlayerID", 1);
        int gameID = PlayerPrefs.GetInt("GameID", 123);
        int resourceID = PlayerPrefs.GetInt("ResourceID", 456);
        int idDoCaso = caseID; // campo adicional que você está adicionando

        CaseSelectedMessage message = new CaseSelectedMessage(playerID, gameID, resourceID, timestats, idDoCaso);

        StartCoroutine(MessageSender.Instance.Send(message));

        TimeStatsMessage timeStatsMessage = new TimeStatsMessage(playerID, gameID, resourceID, 1, 0);  

        StartCoroutine(MessageSender.Instance.Send(timeStatsMessage));

        SceneManager.LoadScene("TelaJogo");
}




    void SetTimeObjectives(int id)
    {

        if (presets.presetTempo != 0)
        {
            recordBox.SetActive(true);
            string gold = "";
            string silver = "";
            string bronze = "";

            if (id < mainCasesNumber)
            {

                switch (caseSize[id])
                {
                    case "P":
                        gold = SecondsToMinutes(mTimes[0]);
                        silver = SecondsToMinutes(mTimes[1]);
                        bronze = SecondsToMinutes(mTimes[2]);
                        break;
                    case "M":
                        gold = SecondsToMinutes(mTimes[3]);
                        silver = SecondsToMinutes(mTimes[4]);
                        bronze = SecondsToMinutes(mTimes[5]);
                        break;
                    case "G":
                        gold = SecondsToMinutes(mTimes[6]);
                        silver = SecondsToMinutes(mTimes[7]);
                        bronze = SecondsToMinutes(mTimes[8]);
                        break;

                }
            }
            else
            {
                switch (caseSize[id])
                {
                    case "P":
                        gold = SecondsToMinutes(mTimesExtra[0]);
                        silver = SecondsToMinutes(mTimesExtra[1]);
                        bronze = SecondsToMinutes(mTimesExtra[2]);
                        break;
                    case "M":
                        gold = SecondsToMinutes(mTimesExtra[3]);
                        silver = SecondsToMinutes(mTimesExtra[4]);
                        bronze = SecondsToMinutes(mTimesExtra[5]);
                        break;
                    case "G":
                        gold = SecondsToMinutes(mTimesExtra[6]);
                        silver = SecondsToMinutes(mTimesExtra[7]);
                        bronze = SecondsToMinutes(mTimesExtra[8]);
                        break;

                }
            }

            goldText.text = gold;
            silverText.text = silver;
            bronzeText.text = bronze;
        }
        else
        {
            recordBox.SetActive(false);
        }

    }

    string SecondsToMinutes(float sec)
    {
        string time = "";
        float minutes = Mathf.FloorToInt(sec / 60);
        float seconds = Mathf.FloorToInt(sec % 60);
        if (seconds < 10)
        {
            time = minutes.ToString() + ":0" + seconds.ToString();
        }
        else
        {
            time = minutes.ToString() + ":" + seconds.ToString();
        }

        return time;
    }

    public void CheckNarrative()
    {
        countMainCases = 0;
        presets.LoadPreferences();
        for (int i = 0; i < caseDetails.Count; i++)
        {
            string caseIDString = "RecordeCaso" + i + presets.presetTempo.ToString() + presets.presetPreco.ToString() + presets.presetInvertida.ToString() + presets.presetDiagonal.ToString();
            float recordecaso = PlayerPrefs.GetFloat(caseIDString, 3000);
            if (recordecaso != 3000)
            {
                countMainCases++;
                
            }
          
        }

        if (countMainCases >= (mainCasesNumber) / 2 && PlayerPrefs.GetInt("NarrativaId", 0) == 0) //checa quando entram o segundo e terceiro dialogo; Checagem muda se aumentar número de diálogos
        {
            PlayerPrefs.SetInt("NarrativaId", 1);
            SceneManager.LoadScene("Narrativa");
        }
        else if (countMainCases >= (mainCasesNumber) && PlayerPrefs.GetInt("NarrativaId", 0) == 1)
        {
            PlayerPrefs.SetInt("NarrativaId", 2);
            PlayerPrefs.SetInt("Final", 0);
            SceneManager.LoadScene("Narrativa");
        }
        CheckCertificate();






        }

    public void SearchForUnfinished()
    {
        int id = 0;
        float checarResolvido;
        presets.LoadPreferences();
        do
        {
            string caseIDString = "RecordeCaso" + id + presets.presetTempo.ToString() + presets.presetPreco.ToString() + presets.presetInvertida.ToString() + presets.presetDiagonal.ToString();
            checarResolvido = PlayerPrefs.GetFloat(caseIDString, 3000);
            caseID = id;
            id++;
            Debug.Log(caseIDString);
            Debug.Log("CASE ID" + caseID);
            Debug.Log("ID" + id);
            Debug.Log(checarResolvido);

        } while (checarResolvido != 3000 && caseID != caseDetails.Count - 1);
        ShowCase();
    }

    public void CheckCertificate()
    {
        certificateController.DisableCertifificate();
        if (countMainCases == (caseDetails.Count) && PlayerPrefs.GetInt("NarrativaId", 0) == 2 && PlayerPrefs.GetInt("PrimeiroCertificado", 0) == 0)
        {
            certificateController.EnableCertifificate();
            certificateController.CountMedals();
            PlayerPrefs.SetInt("PrimeiroCertificado", 1);

        }
        else if (countMainCases == (caseDetails.Count) && PlayerPrefs.GetInt("NarrativaId", 0) == 2)
        {
            certificateController.EnableCertifificate();
        }
    }

    void Read()
    {

        data_string = casoDetalhesFile.text;
        caseDetails = new List<string>();
        caseDetails.AddRange(data_string.Split("\n"[0]));

        data_string = tamanhoGridFile.text;
        caseSize = new List<string>();
        caseSize.AddRange(data_string.Split(";"[0]));

        mainCasesNumber = int.Parse(numeroCasosPrincipaisFile.text);



    }
}
