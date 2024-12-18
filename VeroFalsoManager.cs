using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using PlayFab.ClientModels;
using PlayFab;

//commento 
public class VeroFalsoManager : MonoBehaviour
{
    public QtsData[] categories;
    private QtsData selectedCategory;
    private int currentQuestionIndex = 0;
    public TMP_Text questiontext;
    public Button[] replyButtons;
    private int counterCategory=0;
    private int numDomande = 6;
    public int score;
    

    public GameObject[] Panels;
    private int panelAttivoIndex=-1;
    
    private Color biancoTrasparente = new Color32(255, 255, 255, 129);
    Color colorInactive = new Color32(255, 255, 255, 180);
    Color colorbottone = new Color32(255, 255, 255, 255);
    Color checkpoint = new Color32(255, 145, 0, 255);

    [Header("correctReplyIndex")]
    public int correctReplyIndex;
    int correctReplies;

    [Header("Fine Gioco")]
    public GameObject fineGiocoPanel;
    public TextMeshProUGUI finetext;

    [Header("bottoni")]
    public Button SwitchBottone;
    public Button aiutoPubblicoBottone;
    public GameObject aiutoPubblicoPanel;
    public TextMeshProUGUI aiutoPubblcotext;

    [Header("Aiuto immagini")]
    public GameObject[] immaginiGruppo0; // Immagini da mostrare per correctReplyIndex == 0
    public GameObject[] immaginiGruppo1; // Immagini da mostrare per correctReplyIndex == 1

    [Header("Timer")]
    public GameObject timer;

 
    public LoginPagePlayfab LoginPagePlayfab;
    
    
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SelectCategory(0); //comincia il gioco con la  categoria di domande di difficltà 1
        fineGiocoPanel.SetActive(false);
        SwitchBottone.GetComponent<Image>().color = colorbottone; //imposta i bottoni al colore originale
        aiutoPubblicoBottone.GetComponent<Image>().color = colorbottone;
        aiutoPubblicoPanel.SetActive(false);
        timer.SetActive(true);
        score = 0;
        
        

    }

    public void SelectCategory(int categoryIndex) //metodo che gestisce le odmand ein base alla c ategoria selezionata
    {
        selectedCategory = categories[categoryIndex];
        currentQuestionIndex = 0;
        ShuffleQuestions(selectedCategory.question); //importa un ordine casuale tra le domande della categoria selezionata
        DisplayQuestion(categoryIndex); 
    }

    public void DisplayQuestion(int cetegoryIndex) // metodo che mostra la domanda in base alla categoria selezionata
    {
        for (int i = 0; i < replyButtons.Length; i++)
        {
            replyButtons[i].GetComponent<Image>().color = Color.white;
        }
        if (selectedCategory == null) return;

        var question = selectedCategory.question[currentQuestionIndex];

        questiontext.text = question.questionText;

        timer.SetActive(true);
        timer.GetComponent<TimerScript>().StartTimer();
        
        

        foreach (Button r in replyButtons) //mostra vero o falso sui buttoni
        {
            r.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < replyButtons.Length; i++)
        {
            TMP_Text buttonText = replyButtons[i].GetComponentInChildren<TMP_Text>();
            buttonText.text = question.replies[i];
            int replyIndex = i;

            replyButtons[i].onClick.AddListener(() =>
            {
                OnReplySelected(replyIndex, cetegoryIndex);
            });

        }

    }

    public void OnReplySelected(int replyIndex, int categoryIndex) //metodo che gestisce la riposta dell'utente
    {
        var question = selectedCategory.question[currentQuestionIndex];

        timer.GetComponent<TimerScript>().StopTimer();
        timer.GetComponent<TimerScript>().ResetTimer(30);

        if (replyIndex == selectedCategory.question[currentQuestionIndex].correctReplyInedex) //gestisce la risposta corretta
        {
            correctReplies++;

            StartCoroutine(RispostaCorretta(replyIndex, categoryIndex));
            UnityEngine.Debug.Log("risposta corretta");

        }
        else //gestisce la risposta sbagliata
        {

            StartCoroutine(CambiaColoreConPausa(replyIndex, Color.red));
            UnityEngine.Debug.Log("risposta sbagliata");
            StartCoroutine(FineGioco());

        }
       
    }

    public void NextQuestion(int categoryIndex) //metodo per passare alla domanda successiva
    {
        currentQuestionIndex++;


        if (currentQuestionIndex < numDomande) //rimane nella stessa categoria per 6 domande 
        {
            DisplayQuestion(categoryIndex);
        }
        else
        {
            counterCategory++;
            numDomande = 6;
            if (counterCategory < 5) //rimane nel gioco per 30 domande (6 per 5 categorie)
            {

                SelectCategory(counterCategory); 
            }
            else
            {
                StartCoroutine(FineGioco());
                UnityEngine.Debug.Log("gioco finito");
            }
        }
    }
    // Algoritmo Fisher-Yates per mescolare un array
    private void ShuffleQuestions(Question[] questions)
    {
        System.Random rng = new System.Random();
        int n = questions.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Question value = questions[k];
            questions[k] = questions[n];
            questions[n] = value;
        }
    }

    public void ChangePanelColour() //gestisce il cambio colore del panel contenente la griglia dei punteggi e i checkpoint
    {
        panelAttivoIndex++;

        if (panelAttivoIndex < Panels.Length) // Verifica che l'indice sia valido
        {
            // Pannelli speciali (4, 9, 14) diventano rossi e poi tornano blu
            if (panelAttivoIndex == 4 || panelAttivoIndex == 9 || panelAttivoIndex == 14)
            {
                Panels[panelAttivoIndex].GetComponent<Image>().color = Color.red; // Cambia colore in rosso
                
            }
            else
            {
                // Cambia il colore del pannello successivo in verde
                Panels[panelAttivoIndex].GetComponent<Image>().color = Color.green;
            }

            // Cambia il colore del pannello corrente in bianco trasparente, se non è il primo
            if (panelAttivoIndex > 0)
            {
                Panels[panelAttivoIndex - 1].GetComponent<Image>().color = biancoTrasparente;
            }
            if (panelAttivoIndex == 5 || panelAttivoIndex == 10 )
            {
                Panels[panelAttivoIndex - 1].GetComponent<Image>().color = checkpoint;
            }


        }
        else
        {
            UnityEngine.Debug.LogWarning("Indice del pannello fuori dai limiti!");
        }
    }
   
    
    IEnumerator CambiaColoreConPausa(int replyIndex, Color color)
    {
        replyButtons[replyIndex].GetComponent<Image>().color = color;
        yield return new WaitForSeconds(1f); // Aspetta un secondo
        replyButtons[replyIndex].GetComponent<Image>().color = Color.white;
    }

    IEnumerator RispostaCorretta(int replyIndex, int categoryIndex) //metodo che gestisce la grafica in caso di risposte corrette 
    {
        StartCoroutine(CambiaColoreConPausa(replyIndex, Color.green)); // Colora il bottone
        yield return new WaitForSeconds(1f); // Aspetta la durata della coroutine
        if (correctReplies % 2 == 0)
        {
            ChangePanelColour();
        }
        NextQuestion(categoryIndex); // Passa alla domanda successiva dopo il ritardo
    }

    public IEnumerator FineGioco() //metodo che gestisce la fine del gioco, sia che tu l'abbia completato sia ch etu abbia sbagliato
    {
        yield return new WaitForSeconds(2f); // Aspetta un secondo
        string testofine="";
        fineGiocoPanel.SetActive(true);
       

        if (correctReplies < 10) testofine = "0 punti. Peccato ritenta la prossima volta!";
        else if (correctReplies >= 10 && correctReplies < 20) 
        { 
            testofine = " 300 punti"+"! Bravo/a ma puoi fare meglio, ritenta e cerca di sbloccare il premio finale!";
            score = 300 ;
        }
        else if (correctReplies >= 20 && correctReplies < 30) {
            testofine = " 900 punti"+"! Bravissimo/a hai quasi raggunto il premio finale, ritenta e prova a reggiungerlo!";
            score = 900;
        }
        else if (correctReplies == 30){ 
            testofine = " 2000 punti"+"! Complimenti, hai finito il gioco col massimo punteggio! Rigioca per guadagnarne altri e imparare ancora sul computer";
            score = 2000;
        }
        
        finetext.text = "GiocoFinito! Hai risposto bene a "+ correctReplies +" domande, guadagnando " + testofine;

       SendLeaderboard(score); //invia lo score alla classifica
    }

    

    public void SwitchButton() //metodo inserito nell'OnClick dello switch per cambiare la domanda
    {
        
        currentQuestionIndex++;
        numDomande = 7; //fatto per cambiare la domanda con una della stessa categoria
        timer.GetComponent<TimerScript>().ResetTimer(30);
        DisplayQuestion(counterCategory);
        SwitchBottone.interactable = false; //si può usare solo una volta lo switch
        SwitchBottone.GetComponent<Image>().color = colorInactive;
    }

    public void AiutoPubblicoButton() //metodo inserito nell'OnClick dell'aiuto dal pubblico per farsi dire la risposta corretta
    {

        StartCoroutine(MostraAiutoPanel());
        aiutoPubblicoBottone.interactable = false;
        aiutoPubblicoBottone.GetComponent<Image>().color = colorInactive;
    }

    IEnumerator MostraAiutoPanel() //fa uscire per 4 secondi l'aiuto dal pubblico
    {
        aiutoPubblicoPanel.SetActive(true);
        aiutoPubblcotext.text = "secondo il pubblico la risposta giusta è:";
        // Nascondi tutte le immagini all'inizio
        foreach (var img in immaginiGruppo0) img.SetActive(false);
        foreach (var img in immaginiGruppo1) img.SetActive(false);

        // Seleziona un'immagine casuale tra quelle contenute nel vettore di immagini di vero o falso in base a correctReplyIndex
        if (selectedCategory.question[currentQuestionIndex].correctReplyInedex == 0)
        {
            int randomIndex = Random.Range(0, immaginiGruppo0.Length);
            immaginiGruppo0[randomIndex].SetActive(true);
        }
        else if (selectedCategory.question[currentQuestionIndex].correctReplyInedex == 1)
        {
            int randomIndex = Random.Range(0, immaginiGruppo1.Length);
            immaginiGruppo1[randomIndex].SetActive(true);
        }

        yield return new WaitForSeconds(4f);
        
        aiutoPubblicoPanel.SetActive(false);

    }

    public void TornaHome()
    {
        SceneManager.LoadScene("HomePage");
    }

    //i successivi metodi servono per inviare lo score alla classifica e al database
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




