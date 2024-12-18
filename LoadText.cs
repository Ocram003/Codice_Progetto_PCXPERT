using UnityEngine;
using TMPro;
using System.IO;

public class LoadText : MonoBehaviour //serve per far apparire i testi nei panel delle schede tecniche poichè si trovano in file .txt
{
    public string fileName;
    public TMP_Text uiText; // Riferimento al componente TextMeshPro nell'interfaccia

    void Start()
    {
        Debug.Log("Start eseguito");
        LoadTextFromResources();
    }
    void LoadTextFromResources()
    {
        // Carica il file come TextAsset dalla cartella Resources
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);

        if (textAsset != null)
        {
            uiText.text = textAsset.text; // Aggiorna il testo nell'UI
        }
        else
        {
            Debug.LogError("File non trovato nella cartella Resources: " + fileName);
        }
    }

}