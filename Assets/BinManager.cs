using UnityEngine;

public class BinManager : MonoBehaviour
{
    public string tipoAccettato; // esempio: "plastica", "carta", "vetro", "umido"

    private void OnTriggerEnter(Collider other)
    {
        // 🔒 Controlla se l'oggetto deve essere separato
        RichiedeSeparazione daSeparare = other.GetComponent<RichiedeSeparazione>();
        if (daSeparare != null && !daSeparare.separato)
        {
            Debug.Log("⚠️ Oggetto composto! Devi separarlo prima di buttarlo.");
            FindObjectOfType<UIManager>()?.MostraMessaggio("⚠️ Prima separa gli oggetti!");
            return;
        }

        // ✅ Verifica se ha TrashItem
        TrashItem item = other.GetComponent<TrashItem>();

        if (item != null)
        {
            // 🧼 Verifica se è sporco
            RichiedePulizia lavabile = other.GetComponent<RichiedePulizia>();
            if (lavabile != null && !lavabile.èPulito)
            {
                Debug.Log("❌ Oggetto sporco! Devi lavarlo prima.");
                FindObjectOfType<UIManager>()?.MostraMessaggio("❌ Questo oggetto è sporco! Lavalo prima.");
                return;
            }

            // 🗑️ Controllo tipo corretto
            if (item.trashType == tipoAccettato)
            {
                Debug.Log("✅ Oggetto corretto, accettato.");
                FindObjectOfType<UIManager>()?.MostraMessaggio("Oggetto buttato nel bidone giusto! Bravo.");

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.RifiutoSmaltito();
                }
                else
                {
                    Debug.LogWarning("⚠️ GameManager.Instance è nullo!");
                }

                Destroy(other.gameObject);
            }
            else
            {
                Debug.Log("❌ Oggetto nel bidone sbagliato.");
                FindObjectOfType<UIManager>()?.MostraMessaggio("Questo oggetto non va in questo bidone!");
            }
        }
        else
        {
            Debug.Log("❗ Oggetto senza componente TrashItem.");
        }
    }
}
