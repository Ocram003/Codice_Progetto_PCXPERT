using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Question //tipo delle domande dei due giochi, entrambi divisi per categorie
{
    public string questionText;
    public string[] replies;
    public int correctReplyInedex;
}

[CreateAssetMenu(fileName = "New category", menuName = "Quiz/Question Data")]
public class QtsData : ScriptableObject
{
    public Question[] question;
    public string category;

    
}
