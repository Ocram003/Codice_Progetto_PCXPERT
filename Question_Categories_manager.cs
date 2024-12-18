using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

public class Question_Categories_manager : MonoBehaviour
{

    public QtsData[] categories; //vettore delle categorie
    private QtsData selectedCategory; 
    public int currentQuestionIndex = 0;
    public TMP_Text questiontext;
    public Button[] replyButtons;


    [Header("Score")]
    public Score_manager score; //punteggio non visualizzato, solo counter di tipo score
    public TextMeshProUGUI punteggioText; 

    [Header("correctReplyIndex")]
    public int correctReplyIndex; 
    int correctReplies;

    [Header("MostraPunteggioPanel")]
    public GameObject MostraPunteggio; //panel che viene attivato quando le 5 domande finiscono

    [Header("MostraRispostePanel")]
    public GameObject MostraRisposte; //panel contenente la scroll view
    public TextMeshProUGUI risposteText; //testo scroll view

    [Header("User Answers Data")]
    public List<UserAnswer> userAnswers = new List<UserAnswer>(); //Arraylist con le domande uscite

    [Header("testo in uscita")]
    public TextMeshProUGUI messaggioUscita;

    [Header("Timer")]
    public GameObject timer; //timer

    private int counterCategories = 0; //counter categorie per condizione di uscita


    [System.Serializable]
    public class UserAnswer //classe per le domande che appaiono nello scrollable text
    {
        public string questionText;   // La domanda
        public string givenAnswer;    // Risposta fornita dall'utente
        public string correctAnswer;  // Risposta corretta
    }

    void Start()
    {

        MostraPunteggio.SetActive(false); 
        int SelectedCategoryIndex = PlayerPrefs.GetInt("selectedCategory", 0); //categoria selezionata passata dal PlayerPrefs dello script della ruota
        SelectCategory(SelectedCategoryIndex);
        counterCategories = PlayerPrefs.GetInt("counter"); //passa counterCategories al playerPrefs per punteggio in caso di uscita dal gioco



    }
    public void SelectCategory(int categoryIndex)
    {
        selectedCategory = categories[categoryIndex];
        currentQuestionIndex = 0;
        ShuffleQuestions(selectedCategory.question); //selezione casuale delle 5 domande dal database di damnde
        DisplayQuestion(categoryIndex); 
    }

    public void DisplayQuestion(int categoryIndex) //metodo per mostrare la domanda
    {
        if (selectedCategory == null) return;

        if (currentQuestionIndex >= selectedCategory.question.Length)
        {
            EndQuiz(categoryIndex); //esce dal ciclo se si superano le 5 dmande
            return;
        }

        var question = selectedCategory.question[currentQuestionIndex]; 

        questiontext.text = question.questionText; //viene creato il testo da mostrare

        // attivazione e reset timer per ogni domanda
        timer.SetActive(true);
        timer.GetComponent<TimerRuotaScript>().ResetTimer(15);
        timer.GetComponent<TimerRuotaScript>().StartTimer();

        // Assegna e resetta i Listeners del bottone per le risposte
        foreach (Button r in replyButtons)
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
                OnReplySelected(replyIndex, categoryIndex);
            });
        }
    }

    public void OnTimerFinished() //metodo che gestisce la fine del timer per domanda
    {
        currentQuestionIndex++;

        if (currentQuestionIndex >= 5)
        {
            EndQuiz(0); // Assumi 0 come categoria predefinita
        }
        else
        {
            DisplayQuestion(0);
        }
    }

    private void EndQuiz(int categoryIndex) //metodo che gestisce la fine delle 5 domande
    {
        timer.SetActive(false);
        MostraPunteggioPanel(); //mostra il panel di fine gioco
        MostraRispostePanel(); //mostra le risposte date e il punteggio

        // Determina il messaggio di uscita
        if (score.score >= 4) //sblocchi l'oggetto se rispondi bene a 4 domande giuste su 5
        {
            messaggioUscita.text = "Bravo, hai sbloccato questo oggetto!";
            PlayerPrefs.SetInt(categoryIndex.ToString(), 1); //setta il playerPrefs a 1 indicando che l'oggetto è stato sbloccato
            PlayerPrefs.Save(); //salva il playerPrefs
        }
        else
        {
            messaggioUscita.text = "Peccato, ritenta!";
        }
    }

    public void OnReplySelected(int replyIndex, int categoryIndex) //metodo che gestisce la risposta dell'utente
    {
        var question = selectedCategory.question[currentQuestionIndex];
        //stoppo e resetto timer
        timer.GetComponent<TimerRuotaScript>().StopTimer();
        timer.GetComponent<TimerRuotaScript>().ResetTimer(15); 

        // Salva la domanda, risposta fornita e risposta corretta
        userAnswers.Add(new UserAnswer
        {
            questionText = question.questionText,
            givenAnswer = question.replies[replyIndex],
            correctAnswer = question.replies[question.correctReplyInedex] 
        });

        if (replyIndex == selectedCategory.question[currentQuestionIndex].correctReplyInedex) //aggiorna lo score che serve per capire se l'oggetto è sbloccato o meno
        {
            score.Addscore(1);
            correctReplies++;
            UnityEngine.Debug.Log("risposta corretta");
        }
        else
        {
            UnityEngine.Debug.Log("risposta sbagliata");
        }

        currentQuestionIndex++; //aggiorno il question index 
        if (currentQuestionIndex < 5)
        {
            DisplayQuestion(categoryIndex);
        }
        else
        {
            if (score.score >= 4) //setta a 1 
            {
                counterCategories++;
                PlayerPrefs.SetInt("counter", counterCategories);
                switch (categoryIndex)
                {
                    case 0:
                        PlayerPrefs.SetInt("0", 1);
                        PlayerPrefs.Save();
                        break;

                    case 1:
                        PlayerPrefs.SetInt("1", 1);
                        PlayerPrefs.Save();
                        break;

                    case 2:
                        PlayerPrefs.SetInt("2", 1);
                        PlayerPrefs.Save();
                        break;

                    case 3:
                        PlayerPrefs.SetInt("3", 1);
                        PlayerPrefs.Save();
                        break;

                    case 4:
                        PlayerPrefs.SetInt("4", 1);
                        PlayerPrefs.Save();
                        break;

                    case 5:
                        PlayerPrefs.SetInt("5", 1);
                        PlayerPrefs.Save();
                        break;

                    default:
                        UnityEngine.Debug.Log("Errore");
                        break;
                }

                messaggioUscita.text = "Bravo, hai sbloccato questo oggetto!";
            }
            else
            {
                messaggioUscita.text = "Peccato, ritenta!";
            }
            currentQuestionIndex = 0;
            timer.SetActive(false); 
            MostraPunteggioPanel();
            MostraRispostePanel(); // Mostra le risposte alla fine




        }
    }


    public void cambiascena() //passa alla scena contenente la ruota una volta finite le 5 domande
    {
        SceneManager.LoadScene("gioco_ruota");
    }

    // Algoritmo Fisher-Yates per mescolare un array
    private void ShuffleQuestions(Question[] questions) //metodo per mettere le domande in ordine casuale
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

    public void MostraPunteggioPanel() //mostra a qunate domande hai risposto bene una volta terminato il quiz da 5 domande
    {
        MostraPunteggio.SetActive(true);
        punteggioText.text = "Hai risposto bene a " + correctReplies + " domande su 5";
    }

    public void MostraRispostePanel() //metodo che mostra le risposte date e quelle corrette una volta terminate le 5 domande del quiz
    {
        MostraRisposte.SetActive(true);

        // Costruisci il contenuto del testo
        string contenuto = "";
        foreach (var answer in userAnswers)
        {
            contenuto += $"<b>Domanda:</b> {answer.questionText}\n";
            contenuto += $"<color=black><b>Risposta data:</b> {answer.givenAnswer}</color>\n";
            contenuto += $"<color=green><b>Risposta corretta:</b> {answer.correctAnswer}</color>\n\n";
        }

        // Imposta il contenuto nel TextMeshPro
        risposteText.text = contenuto;
    }

}

