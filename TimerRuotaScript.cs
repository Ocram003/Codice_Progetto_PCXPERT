using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerRuotaScript : MonoBehaviour //classe usata per gestire il timer dell ruota
{
    public float TimeLeft;
    private bool TimerOn = false;
    public bool timerFinito = false;
    public GameObject FineTimerPanel;
    public Text TimerTxt;
    public Question_Categories_manager Manager;


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
                timerFinito = true;
                StartCoroutine(Finetempo()); //attiva il panel di tempo terminato
            }
        }
    }



    void updateTimer(float currentTime) //fa "scorrere il tempo"
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer(float newTime) 
    {
        TimeLeft = newTime;
        TimerOn = false;
        updateTimer(TimeLeft);
        timerFinito = false;
    }

    public void StopTimer()
    {
        TimerOn = false;
    }

    IEnumerator Finetempo() //chiamato quando finisce il tempo
    {
        FineTimerPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        FineTimerPanel.SetActive(false);

        // Notifica al manager che il tempo è scaduto
        Manager.OnTimerFinished(); //richiama metodo presente il Question_categories_manager
    }
}

