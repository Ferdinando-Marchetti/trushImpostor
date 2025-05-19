using UnityEngine;

public class CoperchioAnimatore : MonoBehaviour
{
    public Transform coperchio; // Riferimento al GameObject del coperchio
    public Transform player;    // Riferimento al Transform del giocatore

    [Header("Impostazioni apertura")]
    public Vector3 angoloAperto = new Vector3(-60f, 0f, 0f); // Rotazione di apertura
    public float distanzaAttivazione = 2.5f; // Distanza minima per aprire
    public float velocitàApertura = 3f;

    private Quaternion rotazioneOriginale;
    private Quaternion rotazioneFinale;
    private bool aperto = false;

    void Start()
    {
        if (coperchio == null || player == null)
        {
            Debug.LogError("⚠️ Assegna i riferimenti a coperchio e player!");
            enabled = false;
            return;
        }

        rotazioneOriginale = coperchio.localRotation;
        rotazioneFinale = Quaternion.Euler(angoloAperto);
    }

    void Update()
    {
        float distanza = Vector3.Distance(transform.position, player.position);
        aperto = distanza <= distanzaAttivazione;

        coperchio.localRotation = Quaternion.Slerp(
            coperchio.localRotation,
            aperto ? rotazioneFinale : rotazioneOriginale,
            Time.deltaTime * velocitàApertura );
    }
}
