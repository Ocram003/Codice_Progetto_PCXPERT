using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginPagePlayfab : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TopText;
    [SerializeField] TextMeshProUGUI MessageText;
    [Header("Login")]// dati da inserire nel panel del login
    [SerializeField] TMP_InputField EmailLoginInput;
    [SerializeField] TMP_InputField PasswordLoginInput;
    [SerializeField] GameObject LoginPage;
    [Header("Register")]// dati da inserire nel panel per regostrarsi
    [SerializeField] TMP_InputField UsernameRegisterInput;
    [SerializeField] TMP_InputField EmailRegisterInput;
    [SerializeField] TMP_InputField PasswordRegisterInput;
    [SerializeField] GameObject RegisterPage;
    [Header("Recovery")]// dati da inserire nel panel se si è dimenticata la password
    [SerializeField] TMP_InputField EmailRecoveryInput;
    [SerializeField] GameObject RecoverPage;

    //Panel di benvenuto che appare quando si  effettua correttamente il login
    [SerializeField]
    private GameObject WelcomeObject;
    [SerializeField]
    private Text WelcomeText;

    public int punteggio;


    //variabili per memorizzare dati utili all'area personale
    public static string LoggedInUsername;
    public static int PlayerScore;
    public static  string loggedInPlayfabId;// mi serve per la classifica nell'area personale 

    //regione sui metodi associati ai bottoni
    #region Buttom Functions
    public void RegisterUser()//metodo associato all'onClick del bottone "registrati"
    {
        if (PasswordRegisterInput.text.Length < 6) // controllo sulla password impostata 
        {
            MessageText.text = "Attenzione la password deve contenere almeno 6 caratteri";
            return;
        }
        var request = new RegisterPlayFabUserRequest // richiesta di registrazione a PlayFab
        {
            DisplayName = UsernameRegisterInput.text,
            Email = EmailRegisterInput.text,
            Password = PasswordRegisterInput.text,

            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnregisterSucces, OnError);
    }
    public void RecoverUser()//metodo associato all'onClick del bottone "recupera password"
    {
        var request = new SendAccountRecoveryEmailRequest // richiesta di invio dell'email contenente link per impostare nuova password all'utente
        {
            Email = EmailRecoveryInput.text,
            TitleId = "91DBC",
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request,OnRecoverySucces,OnErrorRecovery);
    }

    private void OnErrorRecovery(PlayFabError result) //messaggio di errore nel caso di email inserita errata
    {
        MessageText.text= "Email inserita errata";
    }

    private void OnRecoverySucces(SendAccountRecoveryEmailResult result) //messaggio di invio eseguito
    {
        OpenLoginPage();
        MessageText.text = "Recovery Mail Sent";
    }

    public void Login()//metodo associato all'onClick del bottone "login"
    {

        var request = new LoginWithEmailAddressRequest //richiesta a PlayFab 
        {
            Email = EmailLoginInput.text,
            Password = PasswordLoginInput.text,

            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams     
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSucces, OnError);
    }
 
    private void OnLoginSucces(LoginResult result)
    {
        //per memorizzare dati per la classiica attorno alla ima posizione 
        loggedInPlayfabId =result.PlayFabId;
        string name = null;
        
        if(result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
           
        }
        //per memorizzare i dati utili all'area personale
        LoggedInUsername = name;
        

       //Panel di benvenuto 
        WelcomeObject.SetActive(true);
        WelcomeText.text = "Welcome!! " + name;
       StartCoroutine(LoadNextScene());
    }

    
    
    public void OnError(PlayFabError error)
    {
        MessageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
    private void OnregisterSucces(RegisterPlayFabUserResult result)// messaggio mostrato a video quando avviene una nuova registrazione
    {
        MessageText.text = "NUovo Account Creato";
        OpenLoginPage();
    }
    

    public void OpenLoginPage()
    {
        LoginPage.SetActive(true);
        RegisterPage.SetActive(false);
        RecoverPage.SetActive(false);
        TopText.text = "ACCEDI";
     }
    public void OpenRegisterPage()
    {
        LoginPage.SetActive(false);
        RegisterPage.SetActive(true);
        RecoverPage.SetActive(false);
        TopText.text = "REGISTRATI";
    }
    public void OpenRecoveryPage()
    {
        LoginPage.SetActive(false);
        RegisterPage.SetActive(false);
        RecoverPage.SetActive(true);
        TopText.text = "PASSWORD DIMENTICATA";
    }
    #endregion 

    IEnumerator LoadNextScene()// Mostra il Panel di benvenuto dopo 4 secondi dal Login
    {
        yield return new WaitForSeconds(4);
        MessageText.text = "Login Eseguito";
        SceneManager.LoadScene("HomePage");
    }

    public void SendLeaderboard(int score)// metodi per inserire punteggi nella classifica
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "PlatformScore",
                    Value = score
                }

            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }
    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)//verifica dell'invio del punteggio alla classifica 
    {
        Debug.Log("successful leaderboard sent");
    }
    

  



}
