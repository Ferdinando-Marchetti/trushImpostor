

using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    // Singleton
    public static UIManager Instance;

    [Header("🟢 Messaggi normali")]
    public GameObject notificaPanel;
    public TextMeshProUGUI notificaTesto;
    public float durataMessaggio = 2f;

    [Header("🏆 Messaggio di vittoria")]
    public GameObject vittoriaPanel;
    public TextMeshProUGUI vittoriaTesto;

    // 🔽 NUOVO: Risparmio ambientale (CO2)
    [Header("🌱 Risparmio ambientale")]
    public GameObject messaggioCO2Panel;
    public TextMeshProUGUI messaggioCO2Text;

    private float timer = 0f;

    private void Awake()
    {
        // Inizializza il singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
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
        notificaTesto.text = testo;
        notificaPanel.SetActive(true);
        timer = durataMessaggio;
    }

    public IEnumerator MostraVittoria(string testo)
    {
        vittoriaTesto.text = testo;
        vittoriaPanel.SetActive(true);

        yield return new WaitForSeconds(5f); // aspetta 5 secondi

        vittoriaPanel.SetActive(false);
    }

    public IEnumerator AvviaQuiz()
    {
        yield return new WaitForSeconds(5f); // aspetta 5 secondi

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
    [Header("📊 Punteggio")]
    public TextMeshProUGUI punteggioText;

    public void AggiornaPunteggioUI(int punteggio)
    {
        if (punteggioText != null)
            punteggioText.text = $"Punteggio: {punteggio}";
        else
            Debug.LogWarning("⚠️ punteggioText non è assegnato!");
    }
    // 🔽 NUOVO metodo per mostrare il risparmio ambientale
    public void MostraMessaggioCO2(float valore)
    {
        if (messaggioCO2Panel != null && messaggioCO2Text != null)
        {
            messaggioCO2Panel.SetActive(true);
            messaggioCO2Text.text = $"🌱 Hai risparmiato {valore:F1} kg di CO₂ smaltendo correttamente i rifiuti!";
        }
        else
        {
            Debug.LogWarning("⚠️ messaggioCO2Panel o messaggioCO2Text non sono assegnati!");
        }
    }
}






 