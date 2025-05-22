using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;

[System.Serializable]
public class Domanda
{
    public string testo;
    public string[] opzioni;
    public int rispostaCorretta;
}

public class QuizManager : MonoBehaviour
{
    public GameObject canvasQuiz;
    public TMP_Text testoDomanda;
    public Button[] pulsantiRisposte; // Bottoni per la selezione
    public TMP_Text[] testoOpzioni;   // Testi delle risposte
    public Button bottoneProssima;
    int nCorrette = 0;
    public GameObject pannelloFine;



    public Domanda[] domande;
    private int indiceCorrente = 0;
    private int sceltaUtente = -1;

    void Start()
    {
        bottoneProssima.onClick.AddListener(ControllaRisposta);
        canvasQuiz.SetActive(false);
    }

    public void MostraQuiz()
    {
        canvasQuiz.SetActive(true);
        indiceCorrente = 0;
        MostraDomanda();
    }

    void MostraDomanda()
    {
        Domanda d = domande[indiceCorrente];
        testoDomanda.text = d.testo;
        sceltaUtente = -1;

        for (int i = 0; i < pulsantiRisposte.Length; i++)
        {
            // Imposta il testo della risposta
            testoOpzioni[i].text = d.opzioni[i];

            // Pulisce la "X" dal bottone
            TMP_Text testoBottone = pulsantiRisposte[i].GetComponentInChildren<TMP_Text>();
            if (testoBottone != null)
                testoBottone.text = "";

            int index = i;
            pulsantiRisposte[i].onClick.RemoveAllListeners();
            pulsantiRisposte[i].onClick.AddListener(() =>
            {
                SelezionaRisposta(index);
            });
        }
    }

    void SelezionaRisposta(int index)
    {
        sceltaUtente = index;

        // Aggiunge la "X" al bottone selezionato e la rimuove dagli altri
        for (int i = 0; i < pulsantiRisposte.Length; i++)
        {
            TMP_Text testoBottone = pulsantiRisposte[i].GetComponentInChildren<TMP_Text>();
            if (testoBottone != null)
                testoBottone.text = (i == index) ? "X" : "";
        }
    }

    void ControllaRisposta()
    {
        if (sceltaUtente == -1)
        {
            Debug.Log("⚠️ Seleziona una risposta prima di continuare.");
            return;
        }
        
        bool corretta = sceltaUtente == domande[indiceCorrente].rispostaCorretta;
        if (corretta)
        {
            Debug.Log("✅ Risposta corretta!");
            nCorrette++;
        }
        else
        {
            Debug.Log("❌ Risposta sbagliata.");
        }
        

        indiceCorrente++;
        if (indiceCorrente < domande.Length)
        {
            MostraDomanda();
        }
        else
        {
            FineQuiz();
        }
    }

    void FineQuiz()
    {
        
        Debug.Log("🎯 Quiz completato!");
        canvasQuiz.SetActive(false);
        

        // Puoi aggiungere qui la logica per passare al livello successivo o mostrare un messaggio finale
        pannelloFine.SetActive(true);
        TMP_Text testoPannello = pannelloFine.GetComponentInChildren<TMP_Text>();
        if (testoPannello != null)
        {
            testoPannello.text = $"Hai risposto correttamente a {nCorrette} domande su {domande.Length}.";
        }
        else
        {
            Debug.LogWarning("⚠️ Testo del pannello non trovato!");
        }
    }
}
