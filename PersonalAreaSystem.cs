using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

public class PersonalAreaSystem : MonoBehaviour

{
    [SerializeField] TextMeshProUGUI MessageText;
    [SerializeField] GameObject PersonalAreaPage;
    [SerializeField] TextMeshProUGUI UsernameText;
    [SerializeField] GameObject RowPrefab;
    [SerializeField] Transform RowParent;


    // Start is called before the first frame update
    void Start()// Moatra all'apertura della scena l'username dell'utente e la classifica dei giocatori
    {
        UsernameText.text=LoginPagePlayfab.LoggedInUsername;

        GetLeaderboard();
    }

    //per uscire dal profilo e dare la possibilità ad un altro utente di accedere 
    public void OpennLoginPage()
    {
        SceneManager.LoadScene("LoginPlayFab");   
    }
    
    void OnLeaderboardGet(GetLeaderboardResult result) // metodo che mi restituisce la classifica 
    {
        string Punteggio ="";

        foreach (Transform item in RowParent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in result.Leaderboard)
        {
            if (item.StatValue < 1000) //cambia a video il punteggio quando si passa da un livello all'altro, quindi da Byte a Kilobyte, da Kilobyte a MegaByte e così via. il punteggio cambia ogni 1000 punti (lo sappiamo che 1KB=1024 Byte e non 1000)
            {
                Punteggio = item.StatValue.ToString()+" B";
            }
            else if(item.StatValue>=1000 && item.StatValue < 2000){
                Punteggio = (item.StatValue - 999).ToString() + " KB";
            }
            else if (item.StatValue >= 2000 && item.StatValue < 3000)
            {
                Punteggio = (item.StatValue - 1999).ToString() + " MB";
            }
            else if (item.StatValue >= 3000 )
            {
                Punteggio = (item.StatValue - 2999).ToString() + " GB";
            }

            GameObject newGo = Instantiate(RowPrefab, RowParent);
            TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = (item.Position +1 ).ToString();
            texts[1].text = item.DisplayName;   //per ottenere username del giocatore 
            texts[2].text = Punteggio;         //per ottenere il punteggio 

            Debug.Log(string.Format("Place:{0} | ID:{1} | VALUE: {2}",
                item.Position, item.PlayFabId, item.StatValue));
        }
    }
    public void GetLeaderboardAroundPlayer()//richiesta della tua posizione in classifica
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "PlatformScore",
            MaxResultsCount = 1 // può essere modificato in base a qunati utenti vuoi vedere tra quelli "vicino" al giocatore loggato
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnError);
    }
    
    public void OnError(PlayFab.PlayFabError error)//messaggio di errore nel caso di classifica non trovata
    {
        MessageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
    void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)// metodo associato all'onClick del bottone "dammi la mia posizione" che restituisce posizione dell'utente nella classifica
    {
        string Punteggio = ""; 
        
        foreach (Transform item in RowParent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard)
        {
            if (item.StatValue < 1000)
            {
                Punteggio = item.StatValue.ToString() + " B";
            }
            else if (item.StatValue >= 1000 && item.StatValue < 2000)
            {
                Punteggio = (item.StatValue - 999).ToString() + " KB";
            }
            else if (item.StatValue >= 2000 && item.StatValue < 3000)
            {
                Punteggio = (item.StatValue - 1999).ToString() + " MB";
            }
            else if (item.StatValue >= 3000)
            {
                Punteggio = (item.StatValue - 2999).ToString() + " GB";
            }
            GameObject newGo = Instantiate(RowPrefab, RowParent);
            TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;                 //per ottenere username del giocatore 
            texts[2].text = Punteggio;        //per ottenere il punteggio 

            if (item.PlayFabId == LoginPagePlayfab.loggedInPlayfabId)
            {
                texts[0].color = Color.red;
                texts[1].color = Color.red;
                texts[2].color = Color.red;
            }

            Debug.Log(string.Format("Place:{0} | ID:{1} | VALUE: {2}",
                item.Position, item.PlayFabId, item.StatValue));
        }
    }
    public void TornaHomePage()
    {
        SceneManager.LoadScene("HomePage");
    }

    public void GetLeaderboard() //richiesta della classifica 
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }
}
