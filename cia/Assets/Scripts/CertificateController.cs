using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CertificateController : MonoBehaviour
{
    [SerializeField] GameObject certificateButton;
    [SerializeField] GameObject certificate;
    [SerializeField] TMP_Text countOuroText;
    [SerializeField] TMP_Text countPrataText;
    [SerializeField] TMP_Text countBronzeText;
    private float[] mTimes = new float[] { 60, 90, 120, 90, 120, 150, 120, 150, 180 };
    private float[] mTimesExtra = new float[] { 40, 60, 90, 60, 90, 120, 90, 120, 150 };
    private PresetsController presets;
    private int countOuro=0;
    private int countPrata=0;
    private int countBronze=0;
    private CaseController caseController;

    // Start is called before the first frame update
    void Start()
    {
        presets = GameObject.Find("PresetsController").GetComponent<PresetsController>();
        caseController = GameObject.Find("CaseController").GetComponent<CaseController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableCertifificate()
    {
        certificateButton.SetActive(true);
        if (!CarregaDados.conf.endGame) {
            TimeStatsMessage timeStatsMessage = new TimeStatsMessage(TimeEvent.Game, TimeType.End);  
            StartCoroutine(MessageSender.Instance.Send(timeStatsMessage));
            CarregaDados.conf.endGame = true;
        }
    } 
    public void DisableCertifificate()
    {
        certificateButton.SetActive(false);
    }

    public void CountMedals()
    {
        countOuro = 0;
        countPrata = 0;
        countBronze = 0;
        certificate.SetActive(true);
        for (int i = 0; i < caseController.caseDetails.Count; i++) {
            string caseIDString = "RecordeCaso" + i + presets.presetTempo.ToString() + presets.presetPreco.ToString() + presets.presetInvertida.ToString() + presets.presetDiagonal.ToString();
            float recordecaso = PlayerPrefs.GetFloat(caseIDString, 3000);
            if (i < caseController.mainCasesNumber)
            {
                switch (caseController.caseSize[i])
                {
                    case "P":
                        if (recordecaso <= mTimes[0])
                        {
                            countOuro++;
                            countPrata++;
                            countBronze++;

                        }
                        else if (recordecaso <= mTimes[1])
                        {
                            countPrata++;
                            countBronze++;

                        }
                        else if (recordecaso <= mTimes[2])
                        {
                            countBronze++;

                        }



                        break;
                    case "M":
                        if (recordecaso <= mTimes[3])
                        {
                            countOuro++;
                            countPrata++;
                            countBronze++;

                        }
                        else if (recordecaso <= mTimes[4])
                        {
                            countPrata++;
                            countBronze++;

                        }
                        else if (recordecaso <= mTimes[5])
                        {
                            countBronze++;

                        }


                        break;
                    case "G":
                        if (recordecaso <= mTimes[6])
                        {
                            countOuro++;
                            countPrata++;
                            countBronze++;
                        }
                        else if (recordecaso <= mTimes[7])
                        {

                            countPrata++;
                            countBronze++;
                        }
                        else if (recordecaso <= mTimes[8])
                        {
                            countBronze++;

                        }


                        break;
                }

            }
            else
            {
                switch (caseController.caseSize[i])
                {
                    case "P":
                        if (recordecaso <= mTimesExtra[0])
                        {
                            countOuro++;
                            countPrata++;
                            countBronze++;

                        }
                        else if (recordecaso <= mTimesExtra[1])
                        {
                            countPrata++;
                            countBronze++;

                        }
                        else if (recordecaso <= mTimesExtra[2])
                        {

                            countBronze++;
                        }



                        break;
                    case "M":
                        if (recordecaso <= mTimesExtra[3])
                        {
                            countOuro++;
                            countPrata++;
                            countBronze++;

                        }
                        else if (recordecaso <= mTimesExtra[4])
                        {
                            countPrata++;
                            countBronze++;

                        }
                        else if (recordecaso <= mTimesExtra[5])
                        {

                            countBronze++;
                        }


                        break;
                    case "G":
                        if (recordecaso <= mTimesExtra[6])
                        {
                            countOuro++;
                            countPrata++;
                            countBronze++;

                        }
                        else if (recordecaso <= mTimesExtra[7])
                        {
                            countPrata++;
                            countBronze++;

                        }
                        else if (recordecaso <= mTimesExtra[8])
                        {
                            countBronze++;

                        }


                        break;
                }
            }
        
    }
        countOuroText.text = "Ouro " + countOuro;
        countPrataText.text = "Prata " + countPrata;
        countBronzeText.text = "Bronze " + countBronze;


    }

}
