using System.Collections;
using UnityEngine;

public class AssemblaggioButtonManager : MonoBehaviour
{
    public GameObject PanelSchedaMadre; 
    public GameObject BaseSchedaMadre;
    public GameObject OggettiSchedaMadre;
    public GameObject PanelCase;
    public GameObject BaseCase;
    public GameObject OggettiCase;
    void Start()
    {
        PanelSchedaMadre.SetActive(false);
       
    }

    // metodo per aprire l'aiuto per 'assemblaggio della scheda madre
    public void ApriSchedaMadre()
    {
        PanelSchedaMadre.SetActive(true);
        BaseSchedaMadre.SetActive(false);
        OggettiSchedaMadre.SetActive(false);
    }

    // metodo per chiudere l'aiuto per 'assemblaggio della scheda madre
    public void ChiudiSchedaMadre()
    {
        PanelSchedaMadre.SetActive(false);
        BaseSchedaMadre.SetActive(true);
        OggettiSchedaMadre.SetActive(true);
    }
    // metodo per aprire l'aiuto per 'assemblaggio del case
    public void ApriCase()
    {
        PanelCase.SetActive(true);
        BaseCase.SetActive(false);
        OggettiCase.SetActive(false);
    }

    // metodo per chiudere l'aiuto per 'assemblaggio del case
    public void ChiudiCase()
    {
        PanelCase.SetActive(false);
        BaseCase.SetActive(true);
        OggettiCase.SetActive(true);
    }

}

