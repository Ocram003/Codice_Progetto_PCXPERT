using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class Barra_manager : MonoBehaviour
{
    // questo script gestisce i 6 quadratini presenti nel gioco della ruota

    [Header("CAtegorie")]
    public GameObject panelCPU;
    public GameObject panelSSD;
    public GameObject panelVENTOLE;
    public GameObject panelGPU;
    public GameObject panelRAM;
    public GameObject panelSCHEDAMADRE;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panelCPU.SetActive(false);
        panelSSD.SetActive(false);  
        panelVENTOLE.SetActive(false);
        panelGPU.SetActive(false);
        panelRAM.SetActive(false);
        panelSCHEDAMADRE.SetActive(false);
        
    }

    // Update is called once per frame
    //dipendentemete dal valore del PlayerPrefs passato dallo script "Question_categories_manager" attiva il quadratino se l'oggetto è stato sbloccato
    void Update()
    {
      if(PlayerPrefs.GetInt("0", 0) == 1)
        {
            panelCPU.SetActive(true);
        }
      if (PlayerPrefs.GetInt("3", 0) == 1)
        {
            panelGPU.SetActive(true);
        }
      if (PlayerPrefs.GetInt("5", 0) == 1)
        {
            panelSCHEDAMADRE.SetActive(true);
        }
      if (PlayerPrefs.GetInt("1", 0) == 1)
        {
            panelSSD.SetActive(true);
        }
      if (PlayerPrefs.GetInt("4", 0) == 1)
        {
            panelRAM.SetActive(true);
        }
      if (PlayerPrefs.GetInt("2", 0) == 1)
        {
            panelVENTOLE.SetActive(true);
        }



    }

}
