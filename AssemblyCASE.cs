using UnityEngine;

public class ComponentCASE : MonoBehaviour
{
    public Transform initialPosition; // Posizione iniziale del componente
    public Transform targetPosition; // Posizione target dove il componente deve andare
    private bool isDragging = false;
    private Vector3 offset;

    public GameObject associatedText; // Testo associato
    public GameObject targetPiece; // Target che deve essere posizionato correttamente

    private Vector3 initialPos; // Memorizza la posizione iniziale
    private Quaternion initialRotation; // Memorizza la rotazione iniziale
    private Vector3 initialScale; // Memorizza la scala iniziale

    public float scaleMultiplier = 1.599411f; // Quanto ingrandire l'oggetto durante il trascinamento
    public float RotazioneOggettoQuandoVienePreso = -42.837f; // Rotazione sull'asse Y quando il pezzo viene preso
    

  

    [SerializeField]
    [Range(10.0f, 100.0f)]
    private float tolerance = 50.0f; // Tolleranza per la posizione del componente

    void Start()
    {
        // Nascondi il target inizialmente
        targetPiece.SetActive(false);

        // Memorizza posizione, scala e rotazione iniziali
        initialPos = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    public void OnMouseDown()
    {
        if (associatedText != null)
        {
            associatedText.SetActive(false); // Nasconde il testo associato
        }

        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();

        // Ingrandisci l'oggetto e imposta la rotazione specificata
        transform.localScale = initialScale * scaleMultiplier;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, RotazioneOggettoQuandoVienePreso, transform.eulerAngles.z);
    }

    public void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    public void OnMouseUp()
    {
        isDragging = false;

        // Ripristina scala e rotazione originali
        transform.localScale = initialScale;
        transform.rotation = initialRotation;

        // Verifica se il pezzo è posizionato correttamente
        float distance = Vector3.Distance(transform.position, targetPosition.position);

        if (distance <= tolerance) // Se posizionato correttamente
        {
            // Nascondi il pezzo e mostra il target
            gameObject.SetActive(false);
            targetPiece.SetActive(true);
        }
        else
        {
            // Torna alla posizione iniziale
            transform.position = initialPos;

            // Mostra di nuovo il testo associato
            if (associatedText != null)
            {
                associatedText.SetActive(true);
            }
        }

    }

    private Vector3 GetMouseWorldPosition()
    {
        // Ottieni la posizione del mouse nel mondo 3D
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

        
}