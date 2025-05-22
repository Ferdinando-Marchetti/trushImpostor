using UnityEngine;

public class RichiedePulizia : MonoBehaviour
{
    public bool èPulito = false;

    [Header("Effetto visivo")]
    public GameObject prefabEffettoLavaggio; // ← assegna prefab schiuma
    public Transform puntoEffetto;           // ← dove farlo apparire (es: transform stesso)

    public void Lava()
    {
        if (èPulito) return;

        èPulito = true;
        Debug.Log("🧽 Oggetto lavato!");

        // Avvia effetto visivo se presente
        if (prefabEffettoLavaggio != null && puntoEffetto != null)
        {
            GameObject fx = Instantiate(prefabEffettoLavaggio, puntoEffetto.position, Quaternion.identity);
            Destroy(fx, 2f); // autodistruggi dopo 2 secondi
        }

        // Altre azioni (es: cambiare materiale) le puoi lasciare qui
    }

    public bool DeveEssereLavato()
    {
        return !èPulito;
    }
}
