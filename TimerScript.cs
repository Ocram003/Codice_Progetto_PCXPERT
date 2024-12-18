using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour //metodo per la gestione del timer del gioco del vero o falso
{
    public float TimeLeft;
    private bool TimerOn = false;
    public bool timerFinito = false;
    public GameObject FineTimerPanel;
    public Text TimerTxt;
    public VeroFalsoManager Manager;
    

    public void StartTimer()
    {
        TimerOn = true;
        FineTimerPanel.SetActive(false);
        timerFinito = false;

    }

    void Update()
    {
        if (TimerOn)
        {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else
            {
                Debug.Log("Time is UP!");

                TimeLeft = 0;
                TimerOn = false;
                timerFinito=true;
                StartCoroutine(Finetempo());
                StartCoroutine(Manager.FineGioco()); //richiama il metodo di VeroFalsoManager per la fine del tempo
            }
        }
    }


    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer(float newTime)
    {
        TimeLeft = newTime;
        TimerOn = false; // Timer si ferma dopo il reset
        updateTimer(TimeLeft); // Aggiorna il testo del timer
        timerFinito = false;
    }

    public void StopTimer()
    {
        TimerOn = false;
    }

    IEnumerator Finetempo()
    {
        FineTimerPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        FineTimerPanel.SetActive(false);

    }
}
