using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    [Header("Punteggio")]
    public int punteggio = 0;
    public int punteggioMassimo = 100;

    public static GameManager Instance;

    [Header("Gestione Rifiuti")]
    public int totaleRifiuti = 0;
    private int rifiutiSmaltiti = 0;
    public int RifiutiSmaltiti => rifiutiSmaltiti;

    [Header("Interazione")]
    public float interactionDistance = 3f;
    public Camera playerCamera;
    public TMP_Text interactionText;

    [Header("📊 Punteggio")]
    public TextMeshProUGUI punteggioText;


    private IInteractable currentTarget;

    // AGGIUNTO: Riferimento diretto al QuizManager
    public QuizManager quizManager;

    private void Start()
    {
        // Sblocca movimento ogni volta che la scena viene caricata
        Movement.inputBloccato = false;

        // (opzionale) Nascondi il cursore se sei in gioco
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        GestisciInterazione();
    }

    void GestisciInterazione()
    {
        if (playerCamera == null || interactionText == null)
            return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            currentTarget = hit.transform.GetComponent<IInteractable>();

            if (currentTarget != null)
            {
                interactionText.text = currentTarget.GetInteractionText();
                interactionText.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentTarget.Interact();
                }
            }
            else
            {
                interactionText.enabled = false;
            }
        }
        else
        {
            interactionText.enabled = false;
        }
    }

    public void RifiutoCreato()
    {
        totaleRifiuti++;
        Debug.Log("🆕 Rifiuto creato! Totale ora: " + totaleRifiuti);
    }

    // ... codice esistente invariato

    public void RifiutoSmaltitoCorretto()
    {
        rifiutiSmaltiti++;
        AggiungiPunti(10);
        Debug.Log("✅ Rifiuto corretto smaltito.");
        Debug.Log($"📊 Smaltiti: {rifiutiSmaltiti} / {totaleRifiuti}");

        UIManager.Instance.AggiornaPunteggioUI(punteggio);

        if (rifiutiSmaltiti >= totaleRifiuti)
        {
            Vittoria();
        }
    }

    public void RifiutoSmaltitoErrato()
    {
        TogliPunti(5);
        Debug.Log("❌ Rifiuto smaltito nel bidone sbagliato.");
        UIManager.Instance.AggiornaPunteggioUI(punteggio);
    }



    void Vittoria()
    {
        // BLOCCO movimento giocatore
        Movement.inputBloccato = true;

        Debug.Log($"🎉 VITTORIA! Smaltiti: {rifiutiSmaltiti} / Totale: {totaleRifiuti}");

        UIManager ui = FindObjectOfType<UIManager>();
        if (ui != null)
        {
            // Uso entrambe le coroutine del primo file
            StartCoroutine(ui.MostraVittoria("Hai smaltito tutti i rifiuti! Hai vinto!"));
            StartCoroutine(ui.AvviaQuiz());
        }
        else
        {
            Debug.LogWarning("⚠️ UIManager non trovato!");
        }
    }

    public void AggiungiPunti(int punti)
    {
        punteggio += punti;
        UIManager.Instance.AggiornaPunteggioUI(punteggio);
    }

    public void TogliPunti(int punti)
    {
        punteggio = Mathf.Max(0, punteggio - punti);
        UIManager.Instance.AggiornaPunteggioUI(punteggio);
    }

}
