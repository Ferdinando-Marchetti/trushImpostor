using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Gestione Rifiuti")]
    public int totaleRifiuti = 0;
    private int rifiutiSmaltiti = 0;
    public int RifiutiSmaltiti => rifiutiSmaltiti;

    [Header("Interazione")]
    public float interactionDistance = 3f;
    public Camera playerCamera;
    public TMP_Text interactionText;

    private IInteractable currentTarget;

    // AGGIUNTO: Riferimento diretto al QuizManager
    public QuizManager quizManager;

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

    public void RifiutoSmaltito()
    {
        rifiutiSmaltiti++;

        Debug.Log("✅ Rifiuto smaltito.");
        Debug.Log($"📊 Smaltiti: {rifiutiSmaltiti} / {totaleRifiuti}");

        if (rifiutiSmaltiti >= totaleRifiuti)
        {
            Vittoria();
        }
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
}
