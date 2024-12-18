using UnityEngine;

public class Avanti : MonoBehaviour
{
    public GameObject canvasAttuale; // Il canvas attualmente attivo
    public GameObject canvasSuccessivo; // Il canvas verso cui andare

    public void VaiAvanti()
    {
        if (canvasAttuale != null && canvasSuccessivo != null)
        {
            // Disattiva il canvas attuale
            canvasAttuale.SetActive(false);

            // Attiva il canvas successivo
            canvasSuccessivo.SetActive(true);
        }
        else
        {
            Debug.LogError("Canvas attuale o successivo non assegnato!");
        }
    }
}