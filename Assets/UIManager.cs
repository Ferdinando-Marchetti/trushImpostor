using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("🟢 Messaggi normali")]
    public GameObject notificaPanel;
    public TextMeshProUGUI notificaTesto;
    public float durataMessaggio = 2f;

    [Header("🏆 Messaggio di vittoria")]
    public GameObject vittoriaPanel;
    public TextMeshProUGUI vittoriaTesto;

    private float timer = 0f;

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

    public void MostraVittoria(string testo)
    {
        vittoriaTesto.text = testo;
        vittoriaPanel.SetActive(true);
    }
}
