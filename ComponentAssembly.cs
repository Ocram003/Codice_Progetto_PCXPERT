using UnityEngine;

public class ComponentAssembly : MonoBehaviour
{
    public Transform initialPosition; // Posizione iniziale del componente
    public Transform targetPosition; // Posizione target dove il componente deve andare
    private bool isDragging = false;
    private Vector3 offset;

    public GameObject associatedText; // Riferimento al testo associato al componente
    public GameObject targetPiece; // Il target che deve essere posizionato correttamente

    private Vector3 initialPos; // Variabile per memorizzare la posizione iniziale


    [SerializeField]
    [Range(10.0f, 100.0f)]
    private float tolerance = 50.0f; // Tolleranza per la posizione del componente

    void Start()
    {
        // Inizialmente, nascondi il target
        targetPiece.SetActive(false);
        // Memorizza la posizione iniziale dell'oggetto al momento dell'avvio
        initialPos = transform.position;
    }

    public void OnMouseDown()
    {
        // Verifica se l'oggetto è interattivo
        if (associatedText != null)
        {
            associatedText.SetActive(false); // Nasconde il testo quando inizia a essere trascinato
        }

        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();
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

        // Verifica se il pezzo è stato posizionato correttamente con una tolleranza
        float distance = Vector3.Distance(transform.position, targetPosition.position);

        if (distance <= tolerance) // Se la distanza è inferiore alla tolleranza, il pezzo è posizionato correttamente
        {
            // Nascondi il pezzo e mostra il target
            gameObject.SetActive(false); // Nasconde il pezzo attuale
            targetPiece.SetActive(true); // Mostra il targetPiece

            // Nascondi il testo associato
            if (associatedText != null)
            {
                associatedText.SetActive(false);
            }
        }
        else
        {
            // Se non è posizionato correttamente, torna alla posizione iniziale
            transform.position = initialPos; // Usa la variabile che memorizza la posizione iniziale


            // Mostra di nuovo il testo (ad esempio "Errore, riprova!")
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
