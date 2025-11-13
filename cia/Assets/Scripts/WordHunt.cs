using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class WordHunt : MonoBehaviour {

    public static WordHunt instance;

    //public TextAsset theme;

    [SerializeField] private TextAsset palavrasFile;

    private CanvasGroup canvas;
    private AudioManager audioManager;

    public delegate void VisualEvents(RectTransform original, RectTransform final);
    public static event VisualEvents FoundWord;

    public delegate void Events();
    public static event Events Finish;

    private ObjectivesController objController;
    private string[,] lettersGrid;
    private Transform[,] lettersTransforms;
    private string alphabet = "abcdefghijklmnopqrstuvwxyz";
    private InputFieldController inpFController;
    StartTutorial startTut;



    [Header("Settings")]
    public bool invertedWordsAreValid;
    public bool diagonalWordsAreValid;

    [Header("Text Asset")]
    private List<string> eachLine;
    public String[] casewords;
    public string data_string;
    public bool filterBadWords;
    public TextAsset badWordsSource;
    [Space]

    [Header("List of Words")]
    public List<string> words = new List<string>();
    public List<string> insertedWords = new List<string>();
    [Header("Grid Settings")]
    public Vector2 gridSize;
    [Space]

    [Header("Cell Settings")]
    public Vector2 cellSize;
    public Vector2 cellSpacing;
    [Space]

    [Header("Public References")]
    public GameObject letterPrefab;
    public Transform gridTransform;
    [Space]

    [Header("Game Detection")]
    public string word;
    public Vector2 orig;
    public Vector2 dir;
    public bool activated;

    public List<int> matrizDicaX = new List<int>();
    public List<int> matrizDicaY = new List<int>();
    private int contDica;
    public List<bool> checkPaint = new List<bool>();
    public List<string> wordsCopy = new List<string>();
    public int countErrors=0;
    [SerializeField] GameObject avisoFree;
    [SerializeField] private GameObject canvasPrincipal;
    [SerializeField] private GameObject molduraLetra;
    [SerializeField] private GameObject pularTutorial;
    private TutorialController TutControl;




    [HideInInspector]
    public List<Transform> highlightedObjects = new List<Transform>();

    private void Awake()
    {
        Time.timeScale = 1;
        objController = GameObject.Find("ObjetivosBG").GetComponent<ObjectivesController>();
        inpFController = GameObject.Find("TelaJogo").GetComponent<InputFieldController>();
        TutControl = GameObject.Find("Camadas tutorial").GetComponent<TutorialController>();

        // 
        audioManager =  GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); 
        TutControl.nextStepTutorial();
        
        //wordsSource = theme;
        Setup();


        //canvas.alpha = 0;
        //canvas.blocksRaycasts = false;

        //miniMenu.DOMoveY(0, .6f).SetEase(Ease.OutBack);
        instance = this;
    }

    public void Setup(){
        if (PlayerPrefs.GetInt("LoadCaseId") == 99)
        {
            startTut = GameObject.Find("Start Tutorial").GetComponent<StartTutorial>();
            pularTutorial.SetActive(true);
            
            words = startTut.tutorialWords.Split(';').ToList();
            
        }
        else
        {
            Read(); 
        }
        GetGridSize();

        PrepareWords();

        InitializeGrid();

        InsertWordsOnGrid();

        RandomizeEmptyCells();

        if (TutControl.tutId == 0)
        {
            DicaLetra();
        }


        //DisplaySelectedWords();

    }

    void GetGridSize()
    {
        
        switch (PlayerPrefs.GetString("LoadCaseSize"))
        {
            case "P":
                gridSize.x = 11;
                gridSize.y = 7;
                cellSize.x = 27;
                cellSize.y = 27;
                cellSpacing.x = 7;
                cellSpacing.y = 7;
                break;
            case "M":
                gridSize.x = 14;
                gridSize.y = 8;
                cellSize.x = 24;
                cellSize.y = 24;
                cellSpacing.x = 5;
                cellSpacing.y = 5;
                break;
            case "G":
                gridSize.x = 16;
                gridSize.y = 9;
                cellSize.x = 23;
                cellSize.y = 23;
                cellSpacing.x = 3;
                cellSpacing.y = 3;
                break;

        }
    }

    private void PrepareWords()
    {
        //Pegar lista de palavras
        
        if(PlayerPrefs.GetInt("PalavrasInvertidas", 0) == 1) //lê das preferências se as palavras invertidas estão habilitadas
        {
            invertedWordsAreValid = true;
        }
        else
        {
            invertedWordsAreValid = false;
        }

        if (PlayerPrefs.GetInt("PalavrasDiagonais", 0) == 1)
        {
            diagonalWordsAreValid = true;
        }
        else
        {
            diagonalWordsAreValid = false;
        }
        inpFController.PassWords(words);
        wordsCopy = new List<string>(words);
        for (int i=0; i< wordsCopy.Count; i++)
        {
            wordsCopy[i] = Changecharacters(wordsCopy[i]);
           
        }
        objController.SetNumberOfWords(words.Count);


        //Filtrar palavrões e etc..
        if (filterBadWords)
        {
            List<string> badWords = badWordsSource.text.Split(',').ToList();
            for (int i = 0; i < badWords.Count(); i++)
            {
                if(words.Contains(badWords[i])){
                    words.Remove(badWords[i]);
                    print("palavra ofensiva <b>" + badWords[i] + "</b> <color=red> removida</color>");
                }
            }
        }


        //Filtrar as palavras que cabem na grid
        int maxGridDimension = Mathf.Max((int)gridSize.x, (int)gridSize.y);

        //Que palavras da lista cabem no grid
        words.RemoveAt(words.Count-1);
        words = words.Where(x => x.Length <= maxGridDimension).ToList();
    }

    private void InitializeGrid()
    {

        //Inicializar o tamanho dos arrays bidimensionais
        lettersGrid = new string[(int)gridSize.x, (int)gridSize.y];
        lettersTransforms = new Transform[(int)gridSize.x, (int)gridSize.y];


        //Passar por todos os elementos x e y da grid
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {

                lettersGrid[x, y] = "";
             
                GameObject letter = Instantiate(letterPrefab, transform.GetChild(0));
               
                letter.name = x.ToString() + "-" + y.ToString();
             
                lettersTransforms[x, y] = letter.transform;
                
            }
        }
  
        ApplyGridSettings();
     
    }

    void ApplyGridSettings()
    {
        GridLayoutGroup gridLayout = gridTransform.GetComponent<GridLayoutGroup>();

        gridLayout.cellSize = cellSize;
        gridLayout.spacing = cellSpacing;

        int cellSizeX = (int)gridLayout.cellSize.x + (int)gridLayout.spacing.x;

        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(cellSizeX * gridSize.x, 0);


    }

    void InsertWordsOnGrid()
    {
        contDica = 0;
        
        for (int i = 0; i < (words.Count); i++)
        {
            
            word = words[i];
            System.Random rn = new System.Random();

            bool inserted = false;
            int tryAmount = 0;

            do
            {
                int row;
                //int safetyFlag = 0;
                row = rn.Next((int)gridSize.x);
               // do
                //{
                  //  safetyFlag++;
                   // row = rn.Next((int)gridSize.x);
                //} while (row + word.Length > gridSize.x && row - word.Length < 0 && safetyFlag < 50); //garantir que as palavras grandes caibam na horizontal
                
                int column = rn.Next((int)gridSize.y);

                int dirX = 0; int dirY = 0;

                while (dirX == 0 && dirY == 0)
                {
                    
                    if (invertedWordsAreValid)
                    {
                        dirX = rn.Next(3) - 1;
                        dirY = rn.Next(3) - 1;
                    }
                    else
                    {
                        dirX = rn.Next(2);
                        dirY = rn.Next(2);
                    }

                    if (diagonalWordsAreValid == false)
                    {
                        if (rn.Next(2) == 0)
                        {
                            dirX = 0;
                        }
                        else
                        {
                            dirY = 0;
                        }
                    }

                    if (word.Length > gridSize.y)
                    {
                        dirY = 0;
                        if(dirX == 0)
                        {
                            dirX = 1;
                        }
                    }
                }

                inserted = InsertWord(word, row, column, dirX, dirY);
                tryAmount++;

            } while (!inserted && tryAmount < 100);


            int countLine = 0;
            while(tryAmount == 100 && countLine < gridSize.y)
            {
                inserted = InsertWord(word, countLine, 0, 1, 0);
                countLine++;
            }
            int countColumn = 0;
            while (tryAmount == 100 && countColumn < gridSize.y)
            {
                inserted = InsertWord(word, 0, countColumn, 1, 0);
                countColumn++;               
            }


            if (inserted) { 
                insertedWords.Add(Changecharacters(word));
          
            }
        }
    }

    private bool InsertWord(string word, int row, int column, int dirX, int dirY)
    {

        if (!CanInsertWordOnGrid(word, row, column, dirX, dirY))
            return false;

        matrizDicaX.Add(row);
        matrizDicaY.Add(column);
        checkPaint.Add(false);
        contDica++;
        string cleanWord = Changecharacters(word);

        for (int i = 0; i < word.Length; i++)
        {
            lettersGrid[(i * dirX) + row, (i * dirY) + column] = cleanWord[i].ToString();
            Transform t = lettersTransforms[(i * dirX) + row, (i * dirY) + column];
            t.GetComponentInChildren<Text>().text = cleanWord[i].ToString().ToUpper();
            //t.GetComponent<Image>().color = Color.grey;
        }

        return true;
    }

    private bool CanInsertWordOnGrid(string word, int row, int column, int dirX, int dirY)
    {
        if (dirX > 0)
        {
            if (row + word.Length > gridSize.x)
            {
                return false;
            }
        }
        if (dirX < 0)
        {
            if (row - word.Length < 0)
            {
                return false;
            }
        }
        if (dirY > 0)
        {
            if (column + word.Length > gridSize.y)
            {
                return false;
            }
        }
        if (dirY < 0)
        {
            if (column - word.Length < 0)
            {
                return false;
            }
        }

        for (int i = 0; i < word.Length; i++)
        {
            string currentCharOnGrid = (lettersGrid[(i * dirX) + row, (i * dirY) + column]);
            string currentCharOnWord = (word[i].ToString());

            if (currentCharOnGrid != String.Empty && currentCharOnWord != currentCharOnGrid)
            {
                return false;
            }
        }

        return true;
    }

    private void RandomizeEmptyCells()
    {

        System.Random rn = new System.Random();

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (lettersGrid[x, y] == string.Empty)
                {
                    lettersGrid[x, y] = alphabet[rn.Next(alphabet.Length)].ToString();
                    lettersTransforms[x, y].GetComponentInChildren<Text>().text = lettersGrid[x, y].ToUpper();
                }
            }
        }
    }

    public void LetterClick(int x, int y, bool state)
    {
        activated = state;
        orig = state ? new Vector2(x, y) : orig;
        dir = state ? dir : new Vector2(-1, -1);

        if (!state)
        {
            ValidateWord();
        }

    }

        private void ValidateWord()
    {
        word = string.Empty;

        foreach (Transform t in highlightedObjects)
        {
            word += t.GetComponentInChildren<Text>().text.ToLower();
        }

        bool isCorrect = insertedWords.Contains(word) || insertedWords.Contains(Reverse(word));

        if (isCorrect)
        {
            foreach (Transform h in highlightedObjects)
            {
                h.GetComponent<Image>().color = new Color32(128, 255, 128, 255);
                h.transform.DOPunchScale(-Vector3.one, 0.2f, 10, 1);
                h.GetComponent<LetterObjectScript>().hasPainted = true;
                molduraLetra.SetActive(false);
            }

            objController.CountObjective(word);
            int pos = wordsCopy.Contains(word)
                ? wordsCopy.FindIndex(str => str.Contains(word))
                : wordsCopy.FindIndex(str => str.Contains(Reverse(word)));

            checkPaint[pos] = true;

            audioManager.RightAnswer();
            insertedWords.Remove(word);
            insertedWords.Remove(Reverse(word));

            if (TutControl.tutId == 0)
            {
                TutControl.nextStepTutorial();
            }

            if (insertedWords.Count <= 0)
            {
                Finish();
            }
        }
        else
        {
            foreach (Transform h in highlightedObjects)
            {
                h.GetComponent<Image>().color = new Color32(255, 128, 128, 255);
            }

            audioManager.WrongAnswer();
            StartCoroutine(ColorDelay());
        }

        // Enviar mensagem de validação de palavra
        WordValidationMessage validationMessage = new WordValidationMessage(word, isCorrect);

        StartCoroutine(MessageSender.Instance.Send(validationMessage));
    }                                                                                                                                               


    public IEnumerator ColorDelay()
    {
        yield return new WaitForSeconds(0.5f);
        ClearWordSelection();
        CheckErrors();


    }


    public void LetterHover(int x, int y)
    {
        if (activated)
        {
            dir = new Vector2(x, y);
            if (IsLetterAligned(x, y))
            {
                HighlightSelectedLetters(x,y);
            }
        }
    }

    private void HighlightSelectedLetters(int x, int y)
    {

        ClearWordSelection();

        Color selectColor = new Color32(150,145,190,255);

        if (x == orig.x)
        {
            int min = (int)Math.Min(y, orig.y);
            int max = (int)Math.Max(y, orig.y);

            for (int i = min; i <= max; i++)
            {
                
              lettersTransforms[x, i].GetComponent<Image>().color = selectColor;
              highlightedObjects.Add(lettersTransforms[x, i]);
                
            }
        }
        else if (y == orig.y)
        {
            int min = (int)Math.Min(x, orig.x);
            int max = (int)Math.Max(x, orig.x);

            for (int i = min; i <= max; i++)
            {
                lettersTransforms[i, y].GetComponent<Image>().color = selectColor;
                highlightedObjects.Add(lettersTransforms[i, y]);
            }
        }
        else
        {

            // Increment according to direction (left and up decrement)
            int incX = (orig.x > x) ? -1 : 1;
            int incY = (orig.y > y) ? -1 : 1;
            int steps = (int)Math.Abs(orig.x - x);

            // Paints from (orig.x, orig.y) to (x, y)
            for (int i = 0, curX = (int)orig.x, curY = (int)orig.y; i <= steps; i++, curX += incX, curY += incY)
            {
                lettersTransforms[curX, curY].GetComponent<Image>().color = selectColor;
                highlightedObjects.Add(lettersTransforms[curX, curY]);
            }
        }

    }

    private void ClearWordSelection()
    {

        foreach (Transform  h in highlightedObjects)
        {

            if (h.GetComponent<LetterObjectScript>().hasPainted == true)
            {
                h.GetComponent<Image>().color = new Color32(128, 255, 128, 255);
                //h.GetComponent<bool>().hasPainted

            }
            else
            {
                h.GetComponent<Image>().color = Color.white;  

            }

        }

        highlightedObjects.Clear();
    }

    public bool IsLetterAligned(int x, int y)
    {
        return (orig.x == x || orig.y == y || Math.Abs(orig.x - x) == Math.Abs(orig.y - y));
    }



    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public void DicaLetra()
    {
        Color selectColor = new Color32(255, 255, 102, 255);
        for (int i = 0; i< checkPaint.Count; i++)
        {
            
            if(checkPaint[i] == false)
            {
                
                lettersTransforms[matrizDicaX[i], matrizDicaY[i]].GetComponent<Image>().color = selectColor;
                molduraLetra.SetActive(true);
                Debug.Log("Antes");
                if (PlayerPrefs.GetInt("LoadCaseId") == 99 && TutControl.tutId == 0)
                {
                    StartCoroutine(StartDelay(i));
                }
                else
                {
                    molduraLetra.transform.position = new Vector3(lettersTransforms[matrizDicaX[i], matrizDicaY[i]].position.x, lettersTransforms[matrizDicaX[i], matrizDicaY[i]].position.y, 0);
                    
                }
                return;
            }
        }
    }

    public IEnumerator StartDelay(int i)
    {
        yield return new WaitForSeconds(0.005f);
        molduraLetra.transform.position = new Vector3(lettersTransforms[matrizDicaX[i], matrizDicaY[i]].position.x, lettersTransforms[matrizDicaX[i], matrizDicaY[i]].position.y, 0);
        Debug.Log(lettersTransforms[matrizDicaX[i], matrizDicaY[i]].position.x + " " + lettersTransforms[matrizDicaX[i], matrizDicaY[i]].position.y);


    }

    void CheckErrors()
    {
        countErrors++;
        if((PlayerPrefs.GetInt("PrecoAjuda") == 0))
        {
            if (countErrors == 10)
            {
                avisoFree.SetActive(true);
                canvasPrincipal.SetActive(false);
            }
        }
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

    void Read()
    {

        data_string = palavrasFile.text;
        eachLine = new List<string>();
        eachLine.AddRange(data_string.Split("|"[0]));
        words = eachLine[PlayerPrefs.GetInt("LoadCaseId", 0)].Split(';').ToList();
        //casewords = eachLine[PlayerPrefs.GetInt("LoadCaseId", 0)].Split(';');

    }

}
