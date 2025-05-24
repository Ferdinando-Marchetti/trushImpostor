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

}
