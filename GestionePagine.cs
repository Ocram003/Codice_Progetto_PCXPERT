using UnityEngine;

public class GestionePagine : MonoBehaviour
{
    public Canvas PaginaBenvenuto;        // Canvas per la pagina di benvenuto
    public Canvas PaginaAssemblaggioPC;   // Canvas per la pagina di assemblaggio PC
    public Canvas PaginaMenuPrincipale;   // Canvas per il menu principale
    public Canvas CanvasSchedaMadre;      // Canvas per la scheda madre

    private Canvas paginaCorrente;        // Variabile per la pagina corrente

    // Inizializza la pagina corrente (di default, la pagina di benvenuto)
    void Start()
    {
        // Disattiva tutti i canvas
        DisattivaTuttiICanvas();

        // Imposta la pagina corrente alla PaginaBenvenuto
        paginaCorrente = PaginaBenvenuto;

        // Mostra la pagina corrente (PaginaBenvenuto)
        MostraPaginaCorrente();
    }

    // Metodo per andare alla pagina successiva
    public void VaiAvanti()
    {
        if (paginaCorrente == PaginaBenvenuto)
        {
            paginaCorrente = CanvasSchedaMadre; // Passa alla scheda madre
        }

        MostraPaginaCorrente(); // Mostra la nuova pagina corrente
    }

    // Metodo per tornare al menu principale
    public void TornaAlMenu()
    {
        paginaCorrente = PaginaMenuPrincipale;  // Imposta la pagina corrente al menu principale
        MostraPaginaCorrente();  // Mostra il menu principale
    }

    // Funzione per disattivare tutti i canvas
    private void DisattivaTuttiICanvas()
    {
        PaginaBenvenuto.gameObject.SetActive(false);
        PaginaAssemblaggioPC.gameObject.SetActive(false);
        PaginaMenuPrincipale.gameObject.SetActive(false);
        CanvasSchedaMadre.gameObject.SetActive(false);
    }

    // Funzione per attivare la pagina corrente
    private void MostraPaginaCorrente()
    {
        // Disattiva tutti i canvas prima di attivare la pagina corrente
        DisattivaTuttiICanvas();

        // Attiva la pagina corrente
        paginaCorrente.gameObject.SetActive(true);
    }
}