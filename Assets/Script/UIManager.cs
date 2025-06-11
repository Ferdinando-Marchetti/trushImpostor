using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Singleton
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

    [Header("♻️ Conteggio Rifiuti")]
    public TextMeshProUGUI rifiutiText;

    [Header("📊 Punteggio")]
    public TextMeshProUGUI punteggioText;

    [Header("⏱️ Timer UI")]
    public TextMeshProUGUI timerText;

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
    }

    void Update()
    {
        // T per aprire/chiudere tutorial
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

    public void MostraMessaggio(string testo)
    {
        if (notificaTesto != null)
        {
            notificaTesto.text = testo;
            notificaTesto.alignment = TextAlignmentOptions.Center; // ⬅️ Allinea al centro
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
            Debug.LogWarning("⚠️ QuizManager non trovato!");
        }

        Debug.Log("🧠 Quiz iniziato!");
    }

    public void AggiornaPunteggioUI(int punteggio)
    {
        if (punteggioText != null)
            punteggioText.text = $"Punteggio: {punteggio}";
        else
            Debug.LogWarning("⚠️ punteggioText non è assegnato!");
    }

    public void AggiornaConteggioRifiuti(int smaltiti, int totali)
    {
        if (rifiutiText != null)
            rifiutiText.text = $"Rifiuti smaltiti correttamente: {smaltiti} / {totali}";
        else
            Debug.LogWarning("⚠️ rifiutiText non è assegnato!");
    }

    public void AggiornaTimerUI(float tempo)
    {
        if (timerText != null)
        {
            tempo = Mathf.Max(0f, tempo); // Evita numeri negativi
            int minuti = Mathf.FloorToInt(tempo / 60f);
            int secondi = Mathf.FloorToInt(tempo % 60f);
            timerText.text = $"Tempo: {minuti:D2}:{secondi:D2}";
        }
        else
        {
            Debug.LogWarning("⚠️ timerText non assegnato!");
        }
    }


    public void MostraMessaggioCO2()
    {
        if (messaggioCO2Panel != null && messaggioCO2Text != null)
        {
            BinManager binManager = FindFirstObjectByType<BinManager>();
            if (binManager != null)
            {
                float risparmio = binManager.co2Risparmiata;
                messaggioCO2Panel.SetActive(true);
                messaggioCO2Text.text = $" Hai risparmiato {risparmio:F1} kg di CO₂ smaltendo correttamente i rifiuti!";
            }
            else
            {
                Debug.LogWarning("⚠️ BinManager non trovato nella scena!");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ messaggioCO2Panel o messaggioCO2Text non sono assegnati!");
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TornaAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
