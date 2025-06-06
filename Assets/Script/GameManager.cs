using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameManager : MonoBehaviour
{
    public AudioClip suonoEsplosione;
    public Animator effettoMorteAnimator; // Un animatore su uno UI panel
    public float ritardoMorte = 2f;

    public float co2Risparmiata = 0f;

    [Header("⏱️ Timer")]
    public float tempoTrascorso = 0f;
    public bool timerAttivo = false;

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

    public QuizManager quizManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        AvviaTimer(120f); // Inizia il conto alla rovescia da 2 minuti
    }

    void Update()
    {
        GestisciInterazione();

        if (timerAttivo && tempoTrascorso > 0f)
        {
            tempoTrascorso -= Time.deltaTime;
            UIManager.Instance?.AggiornaTimerUI(tempoTrascorso);

            if (tempoTrascorso <= 0f)
            {
                timerAttivo = false;
                TempoScaduto();
            }
        }
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
        AggiornaUIRifiuti();
    }

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

        AggiornaUIRifiuti();
    }

    public void RifiutoSmaltitoErrato(GameObject rifiuto)
    {
        TrashItem item = rifiuto.GetComponent<TrashItem>();
        if (item != null && item.èEsplosivo)
        {
            Esplodi();
            return;
        }

        TogliPunti(5);
        Debug.Log("❌ Rifiuto smaltito nel bidone sbagliato.");
        UIManager.Instance.AggiornaPunteggioUI(punteggio);
    }


    void Vittoria()
    {
        timerAttivo = false;
        Movement.inputBloccato = true;

        Debug.Log($"🎉 VITTORIA! Smaltiti: {rifiutiSmaltiti} / Totale: {totaleRifiuti}");

        UIManager ui = Object.FindFirstObjectByType<UIManager>();
        if (ui != null)
        {
            StartCoroutine(ui.MostraVittoria("Hai smaltito tutti i rifiuti! Hai vinto!"));
            StartCoroutine(ui.AvviaQuiz());

            ui.MostraMessaggioCO2();

            int bonusCO2 = Mathf.RoundToInt(co2Risparmiata * 10);
            AggiungiPunti(bonusCO2);
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

    private void AggiornaUIRifiuti()
    {
        UIManager.Instance?.AggiornaConteggioRifiuti(rifiutiSmaltiti, totaleRifiuti);
    }

    public void AvviaTimer(float durataInSecondi)
    {
        tempoTrascorso = durataInSecondi;
        timerAttivo = true;
        UIManager.Instance?.AggiornaTimerUI(tempoTrascorso);
    }

    void TempoScaduto()
    {
        Debug.Log("⏰ Tempo scaduto!");
        Movement.inputBloccato = true;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.tempoScadutoPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("⚠️ UIManager non trovato!");
        }
    }

    public void AggiungiCO2(float quantità)
    {
        co2Risparmiata += quantità;
        Debug.Log($"🌱 CO2 Risparmiata Totale: {co2Risparmiata}");
    }
    public void Esplodi()
    {
        timerAttivo = false;
        Movement.inputBloccato = true;

        Debug.Log("💥 Esplosione! Il giocatore è morto.");

        // 🔊 Riproduci suono
        if (suonoEsplosione != null)
            AudioSource.PlayClipAtPoint(suonoEsplosione, Camera.main.transform.position);

        // 🎞️ Attiva animazione UI morte
        if (effettoMorteAnimator != null)
            effettoMorteAnimator.SetTrigger("Esploso");

        // ⏱️ Mostra menu dopo un ritardo
        StartCoroutine(MostraMorteDopoRitardo());
    }

    private IEnumerator MostraMorteDopoRitardo()
    {
        yield return new WaitForSeconds(ritardoMorte);

        GameObject panel = GameObject.Find("EsplosionePanel");
        if (panel != null)
        {
            panel.SetActive(true);
        }


        if (UIManager.Instance != null)
        {
            UIManager.Instance.tempoScadutoPanel.SetActive(true);
            UIManager.Instance.MostraMessaggio("💥 BOOM! Sei esploso!");
        }
        else
        {
            Debug.LogWarning("⚠️ UIManager non trovato!");
        }
    }



}
