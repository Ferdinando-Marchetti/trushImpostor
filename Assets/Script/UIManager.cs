using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Pannelli Messaggi")]
    public GameObject messaggioPanel;
    public TMP_Text messaggioText;

    [Header("Messaggi specifici")]
    public GameObject messaggioLavabilePanel;
    public TMP_Text messaggioLavabileText;

    public GameObject messaggioCompostoPanel;
    public TMP_Text messaggioCompostoText;

    public GameObject messaggioEsplosivoPanel;
    public TMP_Text messaggioEsplosivoText;

    [Header("Timer UI")]
    public TMP_Text timerText;

    [Header("Punteggio UI")]
    public TMP_Text punteggioText;

    [Header("Conteggio Rifiuti UI")]
    public TMP_Text conteggioRifiutiText;

    [Header("Panel Tempo Scaduto")]
    public GameObject tempoScadutoPanel;

    private Coroutine messaggioRoutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Assicurati che tutti i pannelli siano nascosti all'avvio
        if (messaggioPanel != null) messaggioPanel.SetActive(false);
        if (messaggioLavabilePanel != null) messaggioLavabilePanel.SetActive(false);
        if (messaggioCompostoPanel != null) messaggioCompostoPanel.SetActive(false);
        if (messaggioEsplosivoPanel != null) messaggioEsplosivoPanel.SetActive(false);
        if (tempoScadutoPanel != null) tempoScadutoPanel.SetActive(false);
    }

    public void MostraMessaggio(string testo, float durata = 3f)
    {
        if (messaggioRoutine != null)
            StopCoroutine(messaggioRoutine);

        messaggioText.text = testo;
        messaggioPanel.SetActive(true);
        messaggioRoutine = StartCoroutine(ChiudiDopoSecondi(messaggioPanel, durata));
    }

    public void MostraMessaggioLavabile(float durata = 8f)
    {
        if (messaggioRoutine != null)
            StopCoroutine(messaggioRoutine);

        if (messaggioLavabileText != null)
            messaggioLavabileText.text = "I rifiuti sporchi non possono essere riciclati così come sono.\r\nNel mondo reale, vanno ripuliti per bene prima di finire nel bidone giusto!\r\n\r\nTrova il contenitore giusto solo dopo aver fatto un salto in un posto dove scorre l'acqua ";

        messaggioLavabilePanel.SetActive(true);
        messaggioRoutine = StartCoroutine(ChiudiDopoSecondi(messaggioLavabilePanel, durata));
    }

    public void MostraMessaggioRifiutoComposto(float durata = 8f)
    {
        if (messaggioRoutine != null)
            StopCoroutine(messaggioRoutine);

        if (messaggioCompostoText != null)
            messaggioCompostoText.text = "I rifiuti composti non possono essere buttati così come sono.\r\nNel mondo reale vanno divisi per materiali prima del riciclo!\r\n\r\nTrova il bidone giusto solo dopo averli smontati pezzo per pezzo \r\nE ricorda: la “Q” non è solo una lettera… è anche un’azione fondamentale!";

        messaggioCompostoPanel.SetActive(true);
        messaggioRoutine = StartCoroutine(ChiudiDopoSecondi(messaggioCompostoPanel, durata));
    }

    public void MostraMessaggioEsplosivo(float durata = 4f)
    {
        if (messaggioRoutine != null)
            StopCoroutine(messaggioRoutine);

        if (messaggioEsplosivoText != null)
            messaggioEsplosivoText.text = "Attenzione! Hai raccolto un rifiuto esplosivo!";

        messaggioEsplosivoPanel.SetActive(true);
        messaggioRoutine = StartCoroutine(ChiudiDopoSecondi(messaggioEsplosivoPanel, durata));
    }

    IEnumerator ChiudiDopoSecondi(GameObject panel, float secondi)
    {
        yield return new WaitForSeconds(secondi);
        panel.SetActive(false);
    }

    public void AggiornaTimerUI(float tempo)
    {
        if (timerText != null)
        {
            int minuti = Mathf.FloorToInt(tempo / 60);
            int secondi = Mathf.FloorToInt(tempo % 60);
            timerText.text = $"{minuti:00}:{secondi:00}";
        }
    }

    public void AggiornaPunteggioUI(int punteggio)
    {
        if (punteggioText != null)
        {
            punteggioText.text = $"Punteggio: {punteggio}";
        }
    }

    public void AggiornaConteggioRifiuti(int smaltiti, int totali)
    {
        if (conteggioRifiutiText != null)
        {
            conteggioRifiutiText.text = $"Rifiuti smaltiti: {smaltiti} / {totali}";
        }
    }

    public IEnumerator MostraVittoria(string messaggio)
    {
        MostraMessaggio(messaggio, 5f);
        yield return new WaitForSeconds(5f);
    }

    public IEnumerator AvviaQuiz()
    {
        // Implementa il quiz qui
        yield break;
    }

    public void MostraMessaggioCO2()
    {
        // Implementa il messaggio CO2
    }
}
