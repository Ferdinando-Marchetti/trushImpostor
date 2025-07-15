using UnityEngine;

public class BinManager : MonoBehaviour
{
    public string tipoAccettato;
    public float co2Risparmiata = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        TrashItem item = other.GetComponent<TrashItem>();
        if (item == null)
        {
            Debug.Log("❗ Oggetto senza componente TrashItem.");
            return;
        }

        RichiedeSeparazione daSeparare = other.GetComponent<RichiedeSeparazione>();
        if (daSeparare != null && !daSeparare.separato)
        {
            Debug.Log("⚠️ Oggetto composto! Devi separarlo prima.");
            UIManager.Instance?.MostraMessaggio(" Prima separa gli oggetti!");
            GameManager.Instance?.RifiutoSmaltitoErrato(other.gameObject); // ✅ Aggiunto
            RespingeOggetto(other);
            return;
        }

        RichiedePulizia lavabile = other.GetComponent<RichiedePulizia>();
        if (lavabile != null && lavabile.DeveEssereLavato())
        {
            Debug.Log("❌ Oggetto sporco! Non può essere buttato.");
            UIManager.Instance?.MostraMessaggio(" Questo oggetto è sporco! Lavalo prima.");
            GameManager.Instance?.RifiutoSmaltitoErrato(other.gameObject); // ✅ Aggiunto
            RespingeOggetto(other);
            return;
        }

        if (item.trashType == tipoAccettato)
        {
            // ✅ Oggetto corretto
            Debug.Log("✅ Oggetto corretto, accettato.");
            UIManager.Instance?.MostraMessaggio(" Oggetto corretto! Bravo.");
            switch (tipoAccettato)
            {
                case "plastica":
                    co2Risparmiata += 0.1f;
                    break;
                case "carta":
                    co2Risparmiata += 0.15f;
                    break;
                case "vetro":
                    co2Risparmiata += 0.25f;
                    break;
                case "umido":
                    co2Risparmiata += 0.05f;
                    break;
                case "indifferenziato":
                    co2Risparmiata += 0.02f;
                    break;
                case "esplosivo":
                    co2Risparmiata += 0.5f;
                    break;
                default:
                    Debug.LogWarning($"Tipo di rifiuto sconosciuto: {tipoAccettato}");
                    break;
            }

            GameManager.Instance?.AggiungiCO2(co2Risparmiata);
            GameManager.Instance?.RifiutoSmaltitoCorretto();
            Destroy(other.gameObject);
        }
        else
        {
            // ❌ Oggetto nel bidone sbagliato
            Debug.Log("❌ Oggetto nel bidone sbagliato.");
            UIManager.Instance?.MostraMessaggio(" Questo oggetto non va in questo bidone!");

            if (item.èEsplosivo)
            {
                Debug.Log("💣 Era esplosivo! Esplode.");
                GameManager.Instance?.Esplodi();
            }
            else
            {
                GameManager.Instance?.RifiutoSmaltitoErrato(other.gameObject);
                RespingeOggetto(other);
            }
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
