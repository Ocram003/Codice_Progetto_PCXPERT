using PlayFab.EventsModels;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;



public class RobotManager : MonoBehaviour
{
    public Canvas robotCanvas;
    public RectTransform robotImage; // L'immagine del robottino
    public GameObject HoCapito;      // bottone per chiudere il messsaggio

    public Image Fumetto;//immagine del fumetto
    public float fadeDuration = 1.0f;  // Durata della dissolvenza fumetto

    private Vector3 offScreenPosition; // Posizione iniziale fuori schermo robot
    private Vector3 onScreenPosition; // Posizione finale sullo schermo robot
    private float moveSpeed = 900f; // Velocità del movimento robot

    private Coroutine currentMovementCoroutine; // Per tenere traccia della coroutine attuale se no se schiaccio i tasti velocemente trigghera tutto

    public TextMeshProUGUI InfoText;//messaggio del robottino
    public float typingSpeed = 0.05f; // Velocità di scrittura (in secondi per carattere)

    private string fullText="";  // Testo completo da scrivere

    private string currentText = ""; // Testo che si sta scrivendo

    public GameObject ruota; //ruota nel caso in cui mi trovo nel quiz

    private bool isPaused;

    void Start()
    {
        robotCanvas.gameObject.SetActive(false);
        // Configura posizioni
        offScreenPosition = new Vector3(-1132, robotImage.anchoredPosition.y, -281);
        onScreenPosition = new Vector3(-780, robotImage.anchoredPosition.y, -271);

        // Inizializza posizioni e visibilità
        robotImage.anchoredPosition = offScreenPosition;

        // Assicurati che l'immagine parta invisibile all'inizio
        Color color = Fumetto.color;
        color.a = 0f;
        Fumetto.color = color;

        //per nascondere il bottone all'avvio
        HoCapito.gameObject.SetActive(false);
     }
    
    public void ShowRobot() //mostra a vidoe il robot, metodo inserito nell'OnClick del pulsante di help
    {
       
        robotCanvas.gameObject.SetActive(true);
        if (currentMovementCoroutine != null)
            {
                StopCoroutine(currentMovementCoroutine);
            }
        
        currentMovementCoroutine = StartCoroutine(MoveRobot(onScreenPosition, () =>
            {
           
            }));

        if (ruota != null) //serve per nascondere la ruota nel gioco riguardante essa, non è istanziata sempre la ruota
        {
            ruota.SetActive(false);
        }
        else
        {
            UnityEngine.Debug.Log("non ci troviamo nel gioco della ruota");
        }
        StartCoroutine(FadeIn());//per immagine del fumetto
        HoCapito.gameObject.SetActive(true);//per mostrare il bottone per chiudere il messaggio
        
        //per lo scorrimento del testo
        StartCoroutine(ShowText());
       
        if (SceneManager.GetActiveScene().name == "PersonalArea") //testo da mostrare in base alla scena in cui ci si trova
        {
            fullText = "Questa è la tua aria personale. Qui potrai visualizzare le tue informazioni quali username e punteggio. Inoltre potrai vedere nella tabella i 10 " +
       "migliori giocatori. Premedo il tasto LA MIA POSIZIONE vedrai la tua posizione nella classifica generale. Premendo il tasto LOGOUT uscirai dal tuo profilo"; 
        }
        else if (SceneManager.GetActiveScene().name == "HomePage")
        {
            fullText = "Benvenuto nella home page, da qui potrai accedere alle sezioni di gioco e apprendimento, quali due giochi, una sezione di assemblaggio e una sezione contenente schede informative riguardanti "+
                "le varie parti di un computer. Inoltre attraverso il pulsante in alto a sinistra potrai accedere all'area personale, contenente i tuoi dati. Premendo CONTATTACI potrai inviare un tuo feedback " +
                "sul gioco. Inizialmente il punteggio viene visualizzato in Byte(B).Ogni 1000 punti scalerai di livello passando da Byte a kilobyte(KB) poi da kilobyte a Megabyte(MB) e infine" +
                " da Megabyte a Gigabyte(GB)" +
                "Buon divertimento!";
        }
        else if (SceneManager.GetActiveScene().name == "Gioco_ruota")
        {
            fullText = "L'obiettivo di questo gioco è completare i pezzi della scheda madre, cintenuti nei 6 quadratini. Per farlo girate la ruota e a ogni spin vi verranno fatte 5 domande sul pezzo "+
                "che vi sarà uscito. Se risponderete bene a almeno 4 domande su 5 sbloccherete il pezzo, in caso contrario vi dovrà capirare a un altro giro per poterlo sbloccare. Per ogni pezzo sbloccato vi verranno assegnati dei punti, se completate tutti i pezzi ci sarà inoltre un punteggio bonus";
        }
        else if (SceneManager.GetActiveScene().name == "SchedeInformative")
        {
            fullText = "Qui puoi trovare le schede informative riguardanti le componenti del computer. Leggile prima di fare i quiz per rispondere bene a tutte le domande!";
                }
        else if (SceneManager.GetActiveScene().name == "VeroFalsoScene")
        {
            fullText = "L'obiettivo di questo gioco è riuscire a rispondere bene a tutte le domande sbloccando il premio finale. Ogni due domande passerete al premio successivo ma attenzione, otterrete i punti solo una volta arrivati a uno dei 3 checkpoint in arancione! "+
                "Avrete a disposizione 30 secondi per domanda e due aiuti, l'aiuto dal pubblico che vi potrà dire la risposta corretta (se sono abbastanza bravi :) ) e lo switch che vi permetterà di cambiare la domanda, schegliete bene quando usarli perchè una volta fatto dovrete fare tutto con le vostre forze! ";
        }
        else if (SceneManager.GetActiveScene().name == "Assemblaggio")
        {
            fullText = "In questa sezione potrete assemblare virtualmente una scheda madre e il case del Pc. Nella prima parte assemblerete una scheda madre con i vari componenti. Una volta terminata, attraverso la freccia sul monitor, potrete poi passare alla seconda sezione riguardante il case del computer ";
        }

        InfoText.text = "<alpha=#00>" + fullText; // Testo completo invisibile
        
        StartCoroutine(ShowText());
        
       
    }
   
    public void HideRobot() //metodo inserito nell'onclick di "ho capito" presente nel funmetto, sere per far uscire di scena il robot e il messaggio
    {
        // Ferma eventuali coroutine attive
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
        }
        
        StopAllCoroutines(); // Ferma tutte le coroutine attive, incluso ShowText()
        InfoText.text = ""; // Resetta il contenuto del testo
        HoCapito.gameObject.SetActive(false);//per nascondere il bottone per chiudere il messaggio

        // Nascondi il fumetto
        StartCoroutine(FadeOut());

        // Avvia la nuova coroutine per nascondere il robot
        currentMovementCoroutine = StartCoroutine(MoveRobot(offScreenPosition, () =>
         {             
             }));
        if (ruota != null) //rimostra la ruota se serve
        {
            ruota.SetActive(true);
        }
        else
        {
            UnityEngine.Debug.Log("non ci troviamo nel gioco della ruota");
        }

        
        robotCanvas.gameObject.SetActive(false);

    }
    

    IEnumerator MoveRobot(Vector3 targetPosition, System.Action onComplete) //metodo per inserire dinamicamente il robot 
    {
        while (Vector3.Distance(robotImage.anchoredPosition, targetPosition) > 0.01f)
            {
                robotImage.anchoredPosition = Vector3.MoveTowards(robotImage.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
        robotImage.anchoredPosition = targetPosition;
        onComplete?.Invoke();
    }

    // Funzione per la dissolvenza in ingresso del fumetto
    IEnumerator FadeIn()
    {
        Color color = Fumetto.color;
        float startAlpha = color.a;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, 1f, normalizedTime);
            Fumetto.color = color;
            yield return null;
        }

        // Assicura che l'immagine sia completamente visibile
        color.a = 1f;
        Fumetto.color = color;
    }

    // Funzione per la dissolvenza in uscita del fumetto
    IEnumerator FadeOut()
    {
        Color color = Fumetto.color;
        float startAlpha = color.a;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, 0f, normalizedTime);
            Fumetto.color = color;
            yield return null;
        }

        // Assicura che l'immagine sia completamente invisibile
        color.a = 0f;
        Fumetto.color = color;
    }

        
    //codice per lo scorrimento del testo
    IEnumerator ShowText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i); // Prendi i primi "i" caratteri del testo completo
            InfoText.text = currentText; // Aggiorna il testo di TextMeshPro
            yield return new WaitForSeconds(typingSpeed); // Aspetta il tempo definito
        }
        InfoText.text = currentText; // Mostra solo il testo scritto
    }
  







   

    
}

