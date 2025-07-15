using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public AudioClip suonoEsplosione;
    public GameObject filtroRossoPanel;
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
    private int rifiutiGestiti = 0; // ✅ Aggiunto per conteggio completo

    public int RifiutiSmaltiti => rifiutiSmaltiti;

    [Header("Interazione")]
    public float interactionDistance = 3f;
    public Camera playerCamera;
    public TMP_Text interactionText;

    [Header("📊 Punteggio")]
    public TextMeshProUGUI punteggioText;

    public QuizManager quizManager;

    private IInteractable currentTarget;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        AvviaTimer(240f);
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
        rifiutiGestiti++; // ✅ Conta anche qui
        AggiungiPunti(10);
        Debug.Log("✅ Rifiuto corretto smaltito.");
        Debug.Log($"📊 Smaltiti correttamente: {rifiutiSmaltiti} / {totaleRifiuti}");

        UIManager.Instance.AggiornaPunteggioUI(punteggio);

        if (rifiutiGestiti >= totaleRifiuti)
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

        rifiutiGestiti++; // ✅ Conta anche quelli sbagliati
        TogliPunti(5);
        Debug.Log("❌ Rifiuto smaltito nel bidone sbagliato.");
        UIManager.Instance.AggiornaPunteggioUI(punteggio);

        if (rifiutiGestiti >= totaleRifiuti)
        {
            Vittoria();
        }

        AggiornaUIRifiuti();
    }

    void Vittoria()
    {
        timerAttivo = false;
        Movement.inputBloccato = true;

        Debug.Log($"🎉 VITTORIA! Rifiuti gestiti: {rifiutiGestiti} / Totale: {totaleRifiuti}");

        UIManager ui = Object.FindFirstObjectByType<UIManager>();
        if (ui != null)
        {
            StartCoroutine(ui.MostraVittoria("Hai finito di smaltire i rifiuti!"));
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
        if (suonoEsplosione != null)
            AudioSource.PlayClipAtPoint(suonoEsplosione, Camera.main.transform.position);

        if (MusicManager.Instance != null)
            MusicManager.Instance.StopMusic();

        timerAttivo = false;
        Movement.inputBloccato = true;

        Debug.Log("💥 Esplosione! Il giocatore è morto.");

        if (filtroRossoPanel != null)
            filtroRossoPanel.SetActive(true);

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
            UIManager.Instance.MostraMessaggio("BOOM! Sei esploso!");
        }
        else
        {
            Debug.LogWarning("⚠️ UIManager non trovato!");
        }
    }
}
