using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Score_manager : MonoBehaviour //classe score per il punteggio della ruota
{
    public int score=0;
    public TextMeshProUGUI scoretext;


    void Start()
    {
        score = 0;
        UpdateScoreText();
    }

    public void Addscore(int points) 
    { 
        score = score + points;
        UpdateScoreText();
    }

    public void SubtractScore(int points)
    {
        score -= points;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoretext.text = "" + score.ToString();
    }


}
