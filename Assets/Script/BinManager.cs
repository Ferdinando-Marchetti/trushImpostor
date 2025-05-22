using UnityEngine;

public class BinManager : MonoBehaviour
{
    public string tipoAccettato; // esempio: "plastica", "carta", "vetro", "umido"

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se è un rifiuto valido
        TrashItem item = other.GetComponent<TrashItem>();
        if (item == null)
        {
            Debug.Log("❗ Oggetto senza componente TrashItem.");
            return;
        }

        // Controlla se va separato
        RichiedeSeparazione daSeparare = other.GetComponent<RichiedeSeparazione>();
        if (daSeparare != null && !daSeparare.separato)
        {
            Debug.Log("⚠️ Oggetto composto! Devi separarlo prima.");
            FindObjectOfType<UIManager>()?.MostraMessaggio("⚠️ Prima separa gli oggetti!");
            RespingeOggetto(other);
            return;
        }

        // Controlla se è sporco
        RichiedePulizia lavabile = other.GetComponent<RichiedePulizia>();
        if (lavabile != null && lavabile.DeveEssereLavato())
        {
            Debug.Log("❌ Oggetto sporco! Non può essere buttato.");
            FindObjectOfType<UIManager>()?.MostraMessaggio("❌ Questo oggetto è sporco! Lavalo prima.");
            RespingeOggetto(other);
            return;
        }

        // Controlla se è il tipo giusto
        if (item.trashType == tipoAccettato)
        {
            Debug.Log("✅ Oggetto corretto, accettato.");
            FindObjectOfType<UIManager>()?.MostraMessaggio("✅ Oggetto corretto! Bravo.");
            GameManager.Instance?.RifiutoSmaltito();
            Destroy(other.gameObject);
        }
        else
        {
            Debug.Log("❌ Oggetto nel bidone sbagliato.");
            FindObjectOfType<UIManager>()?.MostraMessaggio("❌ Questo oggetto non va in questo bidone!");
            RespingeOggetto(other);
        }
    }

    private void RespingeOggetto(Collider oggetto)
    {
        Rigidbody rb = oggetto.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Vector3 direzione = (oggetto.transform.position - transform.position).normalized + Vector3.up * 0.5f;
            rb.AddForce(direzione * 5f, ForceMode.Impulse);
        }
    }
}
