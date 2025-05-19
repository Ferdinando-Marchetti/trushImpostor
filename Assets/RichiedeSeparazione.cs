using UnityEngine;

public class RichiedeSeparazione : MonoBehaviour
{
    public bool separato = false;
    public GameObject[] partiDaSeparare; // Es. mela, lisca, ecc.

    public void Separa()
    {
        if (separato) return;

        foreach (GameObject parte in partiDaSeparare)
        {
            // Scollega la parte dal contenitore
            parte.transform.parent = null;

            // Aggiungi Rigidbody se non presente
            if (parte.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = parte.AddComponent<Rigidbody>();
                rb.mass = 50f;
                rb.linearDamping = 1f;
                rb.angularDamping = 2f;
            }

            // Aggiungi Collider se non presente
            if (parte.GetComponent<Collider>() == null)
            {
                parte.AddComponent<BoxCollider>(); // Puoi usare un altro tipo se preferisci
            }

            // Aggiungi TrashItem se non presente
            TrashItem tipo = parte.GetComponent<TrashItem>();
            if (tipo == null)
            {
                tipo = parte.AddComponent<TrashItem>();
                tipo.trashType = "umido"; // ✅ correzione applicata qui
            }

            // Tag per raccolta
            parte.tag = "Trash";

            // Registra nel GameManager
            GameManager.Instance?.RifiutoCreato();
            Debug.Log($"🧩 Separato: {parte.name} | Tipo: {tipo.trashType}");
        }

        separato = true;

        // Elimina contenitore composito (es. piatto invisibile)
        Destroy(gameObject);
    }
}
