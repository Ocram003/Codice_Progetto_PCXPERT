using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class ruota_manager : MonoBehaviour
{
    public float RotatePower;
    public int stopPower;

    private Rigidbody2D rbody;
    int inRotate;

    [Header("rigira")]
    public GameObject rigiraPanel;
    public Button gira;
    
    [Header("punteggio")]
    public int scoreTot = 0;
    public TextMeshProUGUI scoreText;

    [Header("Game over")]
    public GameObject GiocofinitoPanel;
    public TextMeshProUGUI GiocoFinitoText;
    public GameObject ruota;
    public GameObject barra;


    private string B = "B";


    private HashSet<int> usedCategories = new HashSet<int>(); // Categorie già scelte
    private void Start()
    {
        rigiraPanel.SetActive(false);
        ruota.SetActive(true);
        barra.SetActive(true);
        GiocofinitoPanel.SetActive(false);
        rbody = GetComponent<Rigidbody2D>();
        scoreText.text = "PUNTEGGIO:\n" + AggiornaPuneggioRuota().ToString();
        
        if (PlayerPrefs.GetInt("counter") == 6) //se si risponde bene alle 6 categorie mostra il panel di fine gioco
        {
            ruota.SetActive(false);
            barra.SetActive(false);
            GiocofinitoPanel.SetActive(true); //panel di fine gioco
            GiocoFinitoText.text = "Bravissimo, hai completato tutte le categorie ottenendo 2000 punti" + ". \nClicca su \"Torna alla home\" e continua a giocare per diventare un esperto del Computer!";
            usedCategories.Clear(); //resetta il gioco
            Debug.Log("Tutte le categorie sono state giocate! Ripristino.");
            
        }

    }

    float t;
    private void Update()
    {

        // con questo if decrementiamo la velocità della ruota col tempo
        if (rbody.angularVelocity > 0)
        {
            rbody.angularVelocity -= stopPower * Time.deltaTime;
            rbody.angularVelocity = Mathf.Clamp(rbody.angularVelocity, 0, 1440);
        }

        if (rbody.angularVelocity == 0 && inRotate == 1)
        {
            t += 1 * Time.deltaTime;
            if (t >= 0.5f)
            {
                SelezionaCategoria();

                inRotate = 0;
                t = 0;
            }
        }
    }

    public void Rotate() //metodo ch epermette la rotazione della ruota, inserito nell'onClick del bottone "gira"
    {
        stopPower = Random.Range(200, 601);//sceglie a caso quando fermarsi tra questi valori per variarlo da giro in giro
        RotatePower = Random.Range(1500, 3000); //sceglie a caso la potenza di rotazione tra questi valori per variarla da giro in giro
        if (inRotate == 0)
        {
            rbody.AddTorque(RotatePower);
            inRotate = 1;
        }
    }

    public void OnCategorySelected(int categoryIndex) //metodo per cambaire la categoria selezionata e passarla al playerPrefs in base a dove si è fermata la ruota
    {
        PlayerPrefs.SetInt("selectedCategory", categoryIndex);

        SceneManager.LoadScene("Quiz");
    }

  
    public void SelezionaCategoria() //metodo per le azioni da compiere quabndo si comincia la rotazione
    {
        float rot = transform.eulerAngles.z; //salva l'angolo al quale la rotazione si è fermata
        int selectedCategory = -1;
        int categoryCounter = 0;
        selectedCategory = GetCategoryFromRotation(rot); //associa la rotazione alla categoria

        if (PlayerPrefs.GetInt(selectedCategory.ToString(), 0) != 0) //mostra il panel che permette di rigirare la ruota nel caso in cui sia selezionata una categoria già scelta
        {
            rigiraPanel.SetActive(true);
            ruota.SetActive(false);
            barra.SetActive(false);
            gira.GetComponent<Image>().enabled = false;
            return;

        }

        categoryCounter++;
        // Aggiungi la categoria alla lista e prosegui
        usedCategories.Add(selectedCategory);
        OnCategorySelected(selectedCategory);
    }
        public int AggiornaPuneggioRuota() //aggiorna il punteggio della sessione di gioco corrente in base alle categorie sbloccate
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

            default:
                UnityEngine.Debug.Log("Errore");
                break;
        }
        return PlayerPrefs.GetInt("score");
    }



    private int GetCategoryFromRotation(float rotation) //metodo che associa la categoria selezionata alla rotazione
    {
        if (rotation > 0f && rotation <= 60f)
            return 0;
        else if (rotation > 60f && rotation <= 120f)
            return 1;
        else if (rotation > 120f && rotation <= 180f)
            return 2;
        else if (rotation > 180f && rotation <= 240f)
            return 3;
        else if (rotation > 240f && rotation <= 300f)
            return 4;
        else if (rotation > 300f && rotation <= 360f)
            return 5;

        return -1; // Categoria non valida
    }

   public void SetActive() //metodo che attiva alcuni elementi in caso si trovino in setActive(false), inserito nell'onClick del pannello che permette di rigirare la ruota
    {
        rigiraPanel.SetActive(false);
        ruota.SetActive(true);
        barra.SetActive(true);
        gira.GetComponent<Image>().enabled = true;

    }

}