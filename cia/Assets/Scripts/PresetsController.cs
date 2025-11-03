using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;


public class PresetsController : MonoBehaviour
{
    // Start is called before the first frame update
    public int presetTempo = 0;
    public int presetPreco = 0;
    public int presetInvertida = 0;
    public int presetDiagonal = 0;
    private int[] salvarpadrao;
    
    [SerializeField] private GameObject _canvas;
    private CaseController caseController;
    [SerializeField]private GameObject canvasPersonalizar;
    [SerializeField] private GameObject canvasPreset;
    public ToggleGroup tempoGroup;
    public ToggleGroup ajudaGroup;
    public ToggleGroup invertidasGroup;
    public ToggleGroup diagonalGroup;


    void Awake()
    {
        if (PlayerPrefs.GetInt("LoadCaseId") == 100){
            canvasPreset.SetActive(true);
        }

    }

    void Start()
    {
        
       caseController = GameObject.Find("CaseController").GetComponent<CaseController>();
       checkPresetChoice();
        caseController.SearchForUnfinished();

    }

    // Update is called once per frame
    void Update()
    {
       
    }



    public void RadioButtonSave()
    {
        Toggle toggle1 = tempoGroup.ActiveToggles().FirstOrDefault();
        Toggle toggle2 = ajudaGroup.ActiveToggles().FirstOrDefault();
        Toggle toggle3 = invertidasGroup.ActiveToggles().FirstOrDefault();
        Toggle toggle4 = diagonalGroup.ActiveToggles().FirstOrDefault();
        
        if (toggle1.name == "Sem Tempo")
        {
            PlayerPrefs.SetInt("Tempo", 0);
        }
        else if (toggle1.name == "Tempo padrão")
        {
            PlayerPrefs.SetInt("Tempo", 1);
        }
        

        if (toggle2.name == "Preço reduzido")
        {
            PlayerPrefs.SetInt("PrecoAjuda", 0);
        }
        else if (toggle2.name == "Preço padrão")
        {
            PlayerPrefs.SetInt("PrecoAjuda", 1);
        }

        if (toggle3.name == "Desabilitado")
        {
            PlayerPrefs.SetInt("PalavrasInvertidas", 0);
        }
        else if (toggle3.name == "Habilitado")
        {
            PlayerPrefs.SetInt("PalavrasInvertidas", 1);
        }

        if (toggle4.name == "Desabilitado")
        {
            PlayerPrefs.SetInt("PalavrasDiagonais", 0);
        }
        else if (toggle4.name == "Habilitado")
        {
            PlayerPrefs.SetInt("PalavrasDiagonais", 1);
        }

        SavePresetButton();
    }

    public void PresetButton()
    {


        string selectedPreset = EventSystem.current.currentSelectedGameObject.name;
        int gameMode = 0; //Cria o gameMode para ser coletado de acordo com cada caso

        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Preset1":
                PlayerPrefs.SetInt("Tempo", 0);
                PlayerPrefs.SetInt("PrecoAjuda", 0);
                PlayerPrefs.SetInt("PalavrasInvertidas", 0);
                PlayerPrefs.SetInt("PalavrasDiagonais", 0);
                gameMode = 1; //Modo livre
                break;

            case "Preset2":
                PlayerPrefs.SetInt("Tempo", 1);
                PlayerPrefs.SetInt("PrecoAjuda", 1);
                PlayerPrefs.SetInt("PalavrasInvertidas", 0);
                PlayerPrefs.SetInt("PalavrasDiagonais", 0);
                gameMode = 2; //Modo padrão
               

                break;

            case "Preset3":
                PlayerPrefs.SetInt("Tempo", 1);
                PlayerPrefs.SetInt("PrecoAjuda", 1);
                PlayerPrefs.SetInt("PalavrasInvertidas", 1);
                PlayerPrefs.SetInt("PalavrasDiagonais", 1);
                gameMode = 3; //Modo Desafiador
                break;

        }
        PlayerPrefs.Save();
        LoadPreferences();
        // Coleta de dados
            double time = Time.time;
            int id_jogador = PlayerPrefs.GetInt("PlayerID", 1);
            int gameID = PlayerPrefs.GetInt("GameID", 123);
            int resourceID = PlayerPrefs.GetInt("ResourceID", 456);
        //Envio da message
            GameModeMessage message = new GameModeMessage(time, gameMode, id_jogador, gameID, resourceID);
            StartCoroutine(MessageSender.Instance.Send(message, "http://localhost:5000/api"));

        canvasPreset.SetActive(false);
        _canvas.SetActive(true);
        caseController.CheckNarrative();
        caseController.SearchForUnfinished();
        caseController.ShowCase();
    }
    public void BackButton()
    {

        
        this.gameObject.SetActive(false);
        _canvas.SetActive(true);
      

    }

    public void  LoadPreferences(){
        presetTempo = PlayerPrefs.GetInt("Tempo", 0);
        presetPreco = PlayerPrefs.GetInt("PrecoAjuda", 0);
        presetInvertida = PlayerPrefs.GetInt("PalavrasInvertidas", 0);
        presetDiagonal = PlayerPrefs.GetInt("PalavrasDiagonais", 0);
        
    }

 

    public void SavePresetButton()
    {
        LoadPreferences();
        caseController.ShowCase();
        caseController.CheckNarrative();
        canvasPersonalizar.SetActive(false);
        _canvas.SetActive(true);
    }

    void OpenCustomization()
    {
        canvasPreset.SetActive(false);
        canvasPersonalizar.SetActive(true);
        tempoGroup = GetComponent<ToggleGroup>();
        ajudaGroup = GetComponent<ToggleGroup>();
        invertidasGroup = GetComponent<ToggleGroup>();
        diagonalGroup = GetComponent<ToggleGroup>();
    }

    void checkPresetChoice()
    {
        if (PlayerPrefs.GetInt("Tempo", 10) ==10)
        {
            _canvas.SetActive(false);
            canvasPreset.SetActive(true);
        }
    }

}
