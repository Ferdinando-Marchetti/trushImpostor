using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Tempo Scaduto")]
    public GameObject tempoScadutoPanel;

    [Header("🟢 Messaggi normali")]
    public GameObject notificaPanel;
    public TextMeshProUGUI notificaTesto;
    public float durataMessaggio = 2f;

    [Header("🏆 Messaggio di vittoria")]
    public GameObject vittoriaPanel;
    public TextMeshProUGUI vittoriaTesto;

    [Header("🌱 Risparmio ambientale")]
    public GameObject messaggioCO2Panel;
    public TextMeshProUGUI messaggioCO2Text;

    [Header("📘 Tutorial")]
    public GameObject pannelloTutorial;
    private bool tutorialAperto = false;

    [Header("♻ Conteggio Rifiuti")]
    public TextMeshProUGUI rifiutiText;

    [Header("📊 Punteggio")]
    public TextMeshProUGUI punteggioText;

    [Header("⏱ Timer UI")]
    public TextMeshProUGUI timerText;

    [Header("💥 Esplosivi")]
    public GameObject messaggioEsplosivoPanel;
    public TMP_Text messaggioEsplosivoText;
    private bool haMostratoMessaggioEsplosivo = false; // 🔥 flag

    [Header("🧽 Messaggio lavabile")]
    public GameObject messaggioLavabilePanel;
    public TMP_Text messaggioLavabileText;
    private bool haMostratoMessaggioLavabile = false;

    [Header("🧽 Messaggio composti")]
    public GameObject messaggioCompostoPanel;
    public TMP_Text messaggioCompostoText;
    private bool haMostratoMessaggioComposto = false;

    private Coroutine messaggioRoutine;
    private float timer = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (pannelloTutorial != null)
        {
            pannelloTutorial.SetActive(true);
            tutorialAperto = true;
            Movement.inputBloccato = true;
        }

        if (messaggioEsplosivoPanel != null)
            messaggioEsplosivoPanel.SetActive(false); // 🔥 Nascondi a inizio
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (pannelloTutorial != null)
            {
                tutorialAperto = !tutorialAperto;
                pannelloTutorial.SetActive(tutorialAperto);
                Movement.inputBloccato = tutorialAperto;
            }
        }

        if (notificaPanel.activeSelf)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                notificaPanel.SetActive(false);
            }
        }
    }

    public void MostraMessaggioEsplosivo(float durata = 4f)
    {
        if (haMostratoMessaggioEsplosivo != false)
        {
            Debug.LogWarning("⚠ Messaggio esplosivo già mostrato!");
            return; // Non mostrare di nuovo
        }
        else
        {
            haMostratoMessaggioEsplosivo = true; // 🔥 Imposta flag
            if (messaggioRoutine != null)
                StopCoroutine(messaggioRoutine);

            if (messaggioEsplosivoText != null)
                messaggioEsplosivoText.text = "ATTENZIONE! \r\nI rifiuti pericolosi non vanno nei bidoni normali.\r\nNel mondo reale vanno portati in isole ecologiche.\r\n\r\nTrova il bidone nascosto per buttarlo...\r\nE fai attenzione a non... saltare in aria!";

            messaggioEsplosivoPanel.SetActive(true);
            messaggioRoutine = StartCoroutine(ChiudiDopoSecondi(messaggioEsplosivoPanel, durata));
        }
       
    }

    public void MostraMessaggioLavabile(float durata = 4f)
    {
        if (haMostratoMessaggioLavabile != false)
        {
            Debug.LogWarning("⚠ Messaggio lavabile già mostrato!");
            return; // Non mostrare di nuovo
        }
        else
        {
            haMostratoMessaggioLavabile = true; // Imposta flag
            if (messaggioRoutine != null)
                StopCoroutine(messaggioRoutine);

            if (messaggioLavabileText != null)
                messaggioLavabileText.text = "ATTENZIONE!\r\nI rifiuti sporchi non possono essere riciclati così come sono.\r\nNel mondo reale, vanno... ripuliti per bene prima di finire nel bidone giusto!\r\n\r\nTrova il contenitore giusto solo dopo aver fatto un salto in un posto dove  scorre l'acqua!";

            messaggioLavabilePanel.SetActive(true);
            messaggioRoutine = StartCoroutine(ChiudiDopoSecondi(messaggioLavabilePanel, durata));
        }
        
    }

    public void MostraMessaggioRifiutoComposto(float durata = 4f)
    {
        if (haMostratoMessaggioComposto != false)
        {
            Debug.LogWarning("⚠ Messaggio composto già mostrato!");
            return; // Non mostrare di nuovo
        }
        else
        {
            haMostratoMessaggioComposto = true; // Imposta flag
            if (messaggioRoutine != null)
                StopCoroutine(messaggioRoutine);

            if (messaggioCompostoText != null)
                messaggioCompostoText.text = "I rifiuti composti non possono essere buttati così come sono.\r\nNel mondo reale vanno... divisi per materiali prima del riciclo!\r\n\r\nTrova il bidone giusto solo dopo averli smontati pezzo per pezzo \r\nE ricorda: la “Q” non è solo una lettera… è anche un’azione fondamentale! ";

            messaggioCompostoPanel.SetActive(true);
            messaggioRoutine = StartCoroutine(ChiudiDopoSecondi(messaggioCompostoPanel, durata));
        }
    }

    IEnumerator ChiudiDopoSecondi(GameObject panel, float secondi)
    {
        yield return new WaitForSeconds(secondi);
        panel.SetActive(false);
    }


    public void MostraMessaggio(string testo)
    {
        if (notificaTesto != null)
        {
            notificaTesto.text = testo;
            notificaTesto.alignment = TextAlignmentOptions.Center;
        }

        if (notificaPanel != null)
        {
            notificaPanel.SetActive(true);
            timer = durataMessaggio;
        }
    }

    public IEnumerator MostraVittoria(string testo)
    {
        if (vittoriaTesto != null)
            vittoriaTesto.text = testo;

        if (vittoriaPanel != null)
            vittoriaPanel.SetActive(true);

        yield return new WaitForSeconds(5f);

        if (vittoriaPanel != null)
            vittoriaPanel.SetActive(false);
    }

    public IEnumerator AvviaQuiz()
    {
        yield return new WaitForSeconds(5f);

        QuizManager quiz = FindFirstObjectByType<QuizManager>();
        if (quiz != null)
        {
            quiz.MostraQuiz();
        }
        else
        {
            Debug.LogWarning("⚠ QuizManager non trovato!");
        }

        Debug.Log("🧠 Quiz iniziato!");
    }

    public void AggiornaPunteggioUI(int punteggio)
    {
        if (punteggioText != null)
            punteggioText.text = $"Punteggio: {punteggio}";
        else
            Debug.LogWarning("⚠ punteggioText non è assegnato!");
    }

    public void AggiornaConteggioRifiuti(int smaltiti, int totali)
    {
        if (rifiutiText != null)
            rifiutiText.text = $"Rifiuti smaltiti correttamente: {smaltiti} / {totali}";
        else
            Debug.LogWarning("⚠ rifiutiText non è assegnato!");
    }

    public void AggiornaTimerUI(float tempo)
    {
        if (timerText != null)
        {
            tempo = Mathf.Max(0f, tempo);
            int minuti = Mathf.FloorToInt(tempo / 60f);
            int secondi = Mathf.FloorToInt(tempo % 60f);
            timerText.text = $"Tempo: {minuti:D2}:{secondi:D2}";
        }
        else
        {
            Debug.LogWarning("⚠ timerText non assegnato!");
        }
    }

    public void MostraMessaggioCO2()
    {
        if (messaggioCO2Panel != null && messaggioCO2Text != null)
        {
            float risparmio = GameManager.Instance.co2Risparmiata;
            messaggioCO2Panel.SetActive(true);
            messaggioCO2Text.text = $" Hai risparmiato {risparmio:F1} kg di CO₂ smaltendo correttamente i rifiuti!";
        }
        else
        {
            Debug.LogWarning("⚠ messaggioCO2Panel o messaggioCO2Text non sono assegnati!");
        }
    }

    public void ChiudiTutorial()
    {
        if (pannelloTutorial != null)
        {
            pannelloTutorial.SetActive(false);
            tutorialAperto = false;
            Movement.inputBloccato = false;
        }
    }

    public void RiprovaLivello()
    {
        MusicManager.Instance?.PlayMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TornaAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}