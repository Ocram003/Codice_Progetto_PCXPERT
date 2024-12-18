using UnityEngine;
using UnityEngine.SceneManagement;


public class SchedeInfoManager : MonoBehaviour
{
    //panels relativi alle schede informative e comntebnenti i testi
    public GameObject firstPage;
    public GameObject ram;
    public GameObject cpu;
    public GameObject gpu;
    public GameObject alimentazione;
    public GameObject torre;
    public GameObject sistemaRaffreddamento;
    public GameObject ssd;
    public GameObject schedaMadre;
    public GameObject altro;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TornaHome();
    }
    //metodi per aprire le schede, si trovano nell'onClick di ogni bottone
    public void OpenRam()
    {
        firstPage.SetActive(false);
        ram.SetActive(true);
    }

    public void OpenCpu() {
        firstPage.SetActive(false);
        cpu.SetActive(true);
    }
    public void OpenGpu()
    {
        firstPage.SetActive(false);
        gpu.SetActive(true);
    }

    public void OpenAlimentazione()
    {
        firstPage.SetActive(false);
        alimentazione.SetActive(true);
    }
    public void OpenSchedaMadre()
    {
        firstPage.SetActive(false);
        schedaMadre.SetActive(true);
    }
    public void OpenSsd()
    {
        firstPage.SetActive(false);
        ssd.SetActive(true);
    }
    public void OpenCase()
    {
        firstPage.SetActive(false);
        torre.SetActive(true);
    }

    public void OpenSistemaRAffreddamento() { 
        firstPage.SetActive(false);
        sistemaRaffreddamento.SetActive(true);
    }
    public void OpenAltro()
    {
        firstPage.SetActive(false);
        altro.SetActive(true);
    }

    public void TornaHome() //torna alla lista di tutte le componenti 
    {
        firstPage.SetActive(true);
        ram.SetActive(false);
        cpu.SetActive(false);
        gpu.SetActive(false);
        alimentazione.SetActive(false);
        torre.SetActive(false);
        sistemaRaffreddamento.SetActive(false);
        ssd.SetActive(false);
        schedaMadre.SetActive(false);
        altro.SetActive(false);
    }

    public void TornaHomePage()
    {
        SceneManager.LoadScene("HomePage");
    }
}
