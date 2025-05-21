using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public QuizManager quizManager; // AGGIUNTO: Riferimento diretto al QuizManager

    public int totaleRifiuti = 0;
    private int rifiutiSmaltiti = 0;

    public int RifiutiSmaltiti => rifiutiSmaltiti;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
        Debug.Log($"🎉 VITTORIA! Smaltiti: {rifiutiSmaltiti} / Totale: {totaleRifiuti}");
 
       

        UIManager ui = Object.FindFirstObjectByType<UIManager>();
        if (ui != null)
        {
            StartCoroutine(ui.MostraVittoria("Hai smaltito tutti i rifiuti! Hai vinto!"));
        }
        else
        {
            Debug.LogWarning("⚠️ UIManager non trovato!");
        }

        if (ui != null)
        {
            StartCoroutine(ui.AvviaQuiz());
        }
        else
        {
            Debug.LogWarning("⚠️ UIManager non trovato!");
        }
      
    }
}
