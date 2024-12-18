using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomePauseButtonManager : MonoBehaviour
{
    public GameObject AvvisoPanel;
    private bool isPaused = false; // Stato della pausa
    public Button pauseButton; // Riferimento al bottone
    public Sprite ResumeImage; // Immagine da mostrare quando il gioco è in pausa
    private Sprite originalImage; // Immagine originale del bottone
    public GameObject ruota;
    public GameObject barra;
    public int scoreTot=0;

   
    private void Start()
    {
        // Memorizziamo l'immagine originale del bottone
        if (pauseButton != null)
        {
            originalImage = pauseButton.GetComponent<Image>().sprite;
        }
        else
        {
            UnityEngine.Debug.Log("Il riferimento al pulsante non è stato assegnato!");
            
        }
        //controllo sulla presenza delle variabili rota barra e AvvisoPanel, fatto perchè non sono sempre presenti dove lo script è inserito
        if (AvvisoPanel==null||ruota==null||barra==null)
        {
            UnityEngine.Debug.Log("non servono questi elementi");
        }

        try { AvvisoPanel.SetActive(false); 
        }
        catch
        {
            UnityEngine.Debug.Log("ignora errore");
        }
    }

    //metodo inserito nel'onClick nel button di pausa del gioco vero o falso
    public void OnClickPause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0; // Blocca il tempo di gioco
        isPaused = true;

        // Cambia l'immagine del pulsante
        if (pauseButton != null && ResumeImage != null)
        {
            pauseButton.GetComponent<Image>().sprite = ResumeImage;
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1; // Ripristina il tempo di gioco
        isPaused = false;

        // Ripristina l'immagine originale del pulsante
        if (pauseButton != null && originalImage != null)
        {
            pauseButton.GetComponent<Image>().sprite = originalImage;
        }
    }

    // metodo inserito nel'onClick nel button chew serve per tornare alla home per far apperire "Avvisopanel"
    public void onClickHome()
    {
        Time.timeScale = 0; // Blocca il tempo di gioco
        isPaused = true;
        try { AvvisoPanel.SetActive(true);  // in alcuni metodi non serve il panel Avvisopanel ma tornano direttamente alla Home
        }
        catch {
            UnityEngine.Debug.Log("AvvisoPanel non serve");
        }
        if (ruota != null && barra != null) // nel gioco della ruota serve nascondere questi elementi
        {
            ruota.SetActive(false);
            barra.SetActive(false);
        }
        else
        {
            UnityEngine.Debug.Log("non servono altri elementi");
        }
        
    }

    //metodo che serve per tornare alla scene HomePage, inserito o nel bottone di conferma di uscita dal gioco presente in AvvisoPanel o in alcue scene direttamente nel bottone della Home
    public void OnClickYes()
    {
        try
        {
            AvvisoPanel.SetActive(false);
        }
        catch
        {
            UnityEngine.Debug.Log("AvvisoPanel non serve");
        }
        Time.timeScale = 1; // Ripristina il tempo di gioco
        isPaused = false;
        
        SceneManager.LoadScene("HomePage");


    }

    //metodo inserito nell'OnClick del button di uscita dal gioco della ruota, serve per uscire dal gioco e aggiornare il database col punteggio
    public void OnClickYesRuota()
    {
        
        AvvisoPanel.SetActive(false);
        Time.timeScale = 1; // Ripristina il tempo di gioco
        isPaused = false;
        AvvisoPanel.SetActive(false);
        if (ruota != null && barra != null)
        {
            ruota.SetActive(true);
            barra.SetActive(true);
        }
        else
        {
            UnityEngine.Debug.Log("non servono questi elementi");
        }
         
        SceneManager.LoadScene("HomePage");

        SendLeaderboard(AggiornaPuneggio()); // qua assegno il punteggio ottenuto all'uscita del gioco allo score totale, nel caso al posto di scoreTot inserire PLAYFAB
        PlayerPrefs.SetInt("counter", 0); //assegno il PlayerPrefs del counter delle categorie sbloccate a 0 all'uscita dalla ruota
    }

    public int AggiornaPuneggio() //aggiorna il PlayerPrefs del punteggio in base al numero di categorie che hai sbloccato
    {
        switch (PlayerPrefs.GetInt("counter"))
        {
            case 0:
                PlayerPrefs.SetInt("score", 0);
                PlayerPrefs.Save();
                break;

            case 1:
                PlayerPrefs.SetInt("score", 200);
                PlayerPrefs.Save();
                break;

            case 2:
                PlayerPrefs.SetInt("score", 400);
                PlayerPrefs.Save();
                break;

            case 3:
                PlayerPrefs.SetInt("score", 600);
                PlayerPrefs.Save();
                break;

            case 4:
                PlayerPrefs.SetInt("score", 800);
                PlayerPrefs.Save();
                break;

            case 5:
                PlayerPrefs.SetInt("score", 1000);
                PlayerPrefs.Save();
                break;
            case 6:
                PlayerPrefs.SetInt("score", 2000);
                PlayerPrefs.Save();
                break;

            default:
                UnityEngine.Debug.Log("Errore");
                break;
        }

        return PlayerPrefs.GetInt("score");
    }

        public void OnClickNo() //fa riprendere il gioco in caso di risposta negativa alla domanda presente nell'Avvisopanel
    {
        AvvisoPanel.SetActive(false);
        Time.timeScale = 1; // Ripristina il tempo di gioco
        isPaused = false;
        if (ruota != null && barra != null)
        {
            ruota.SetActive(true);
            barra.SetActive(true);
        }
        else
        {
            UnityEngine.Debug.Log("non servono questi elementi");
        }
    }
    public void SendLeaderboard(int score)
    {
        // Recupera il valore attuale della statistica prima di aggiornarla
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
        {
            // Cerca la statistica specifica
            int currentScore = 0;
            foreach (var stat in result.Statistics)
            {
                if (stat.StatisticName == "PlatformScore")
                {
                    currentScore = stat.Value; // Valore attuale
                    break;
                }
            }

            // Calcola il nuovo valore sommando lo score
            int newScore = currentScore + score;

            // Aggiorna la statistica con il nuovo valore
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "PlatformScore",
                    Value = newScore // Invio il totale calcolato
                }
            }
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
        },
        OnError);
    }
    
    //metodo per la classifica PlayFab
    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        UnityEngine.Debug.Log("successful leaderboard sent");
    }
    public void OnError(PlayFabError error)
    {
        UnityEngine.Debug.Log(error.ErrorMessage);
        UnityEngine.Debug.Log(error.GenerateErrorReport());
    }
}

