using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System;
using UnityEditor.SearchService;
public class MenuManager : MonoBehaviour
{
    public GameObject giocaPanel;

    public TextMeshProUGUI scoreText;

    public int currentScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        giocaPanel.SetActive(false);

        // Recupera le statistiche del giocatore da PlayFab
        GetPlayerStatistics();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickAssemblaggio() 
    {
        
        SceneManager.LoadScene("Assemblaggio");
    }

    public void OnClickGioca() //attiva il panel per la scelta del gioco da fare, inserito nell'onClik del button nella Homepage dei giochi
    {
        giocaPanel.SetActive(true);
    }
    public void OnClickRuota() //apre la scena gioco_ruota inserito nell'onClik del button per l'apertura del gioco della ruota
    {
        setPlayerPrefsTo0();
        SceneManager.LoadScene("gioco_ruota");
    }

    public void OnClickVerofalso() //apre la scena VeroFalsoScene inserito nell'onClik del button per l'apertura del gioco del vero o falso
    {
        SceneManager.LoadScene("VeroFalsoScene");
    }

    public void OnClickSchede() //apre la scena SchedeInformative inserito nell'onClik del button presente nell'homepage per l'apertura delle schede informative
    {
        SceneManager.LoadScene("SchedeInformative");
    }

    public void onClickAreapersonale() //apre la scena PersonalArea inserito nell'onClik del button presente nell'homepage per l'apertura dell'area personale
    {
        SceneManager.LoadScene("PersonalArea");
    }


    private void setPlayerPrefsTo0() //metodo che serve per settare a 0 i PlayerPrefs del gioco della ruota quando si comincia il gioco
    {
        PlayerPrefs.SetInt("0", 0);
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("4", 0);
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("3", 0);
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("5", 0);
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("2", 0);
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("1", 0);
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("counter", 0);
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.Save();
    }

    //i seguenti 3 metodi servono per ottenere il punteggio della statistica del punteggio dal PlayFab, per poi inserirla a video nell'HomePage
    private void GetPlayerStatistics() 
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatisticsSuccess,
            OnGetStatisticsFailure
        );
    }

    private void OnGetStatisticsSuccess(GetPlayerStatisticsResult result)
    {
        string Punteggio = "";
        // Cerca lo score tra le statistiche
        foreach (var stat in result.Statistics)
        {
            currentScore = stat.Value;
            if (stat.Value < 1000) //cambia a video il punteggio quando si passa da un livello all'altro, quindi da Byte a Kilobyte, da Kilobyte a MegaByte e così via. il punteggio cambia ogni 1000 punti (lo sappiamo che 1KB=1024 Byte e non 1000)
            {
                Punteggio = stat.Value.ToString() + " B";
            }
            else if (stat.Value >= 1000 && stat.Value < 2000)
            {
                Punteggio = (stat.Value - 999).ToString() + " KB";
            }
            else if (stat.Value >= 2000 && stat.Value < 3000)
            {
                Punteggio = (stat.Value - 1999).ToString() + " MB";
            }
            else if (stat.Value >= 3000)
            {
                Punteggio = (stat.Value - 2999).ToString() + " GB";
            }
            // Mostra lo score nel TextMeshPro
            scoreText.text =  Punteggio;
                return;
            
        }
        Debug.Log("Statistiche recuperate con successo!");

        // Se non trovi la statistica "Score"
        scoreText.text = "0";
    }

    private void OnGetStatisticsFailure(PlayFabError error) // in caso di errore nella statistica
    {
        Debug.LogError($"Errore nel recupero delle statistiche: {error.GenerateErrorReport()}");
        scoreText.text = "Errore nel caricamento dello Score!";
    }

    public void ChangePanel() //per chiudere il manel che mostra i giochi
    {
        giocaPanel.SetActive(false);
    }


    public void SendEmail() //metodo inserito nel button "contattaci"
    {
        // Indirizzo email e contenuti preimpostati
        string email = "marcocap.2003@gmail.com";
        string subject = "Feedback sul gioco";
        string body = "Scrivici le tue perplessità ";

        // Crea il link mailto
        string mailto = $"mailto:{email}?subject={Uri.EscapeUriString(subject)}&body={Uri.EscapeUriString(body)}";

        // Apre il client email
        Application.OpenURL(mailto);
    }
}

