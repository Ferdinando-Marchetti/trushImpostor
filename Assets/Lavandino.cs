using UnityEngine;

public class Lavandino : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        // Verifica se l'oggetto ha il componente RichiedePulizia
        RichiedePulizia rifiuto = other.GetComponent<RichiedePulizia>();

        // Se l'oggetto è sporco e il giocatore preme E
        if (rifiuto != null && rifiuto.DeveEssereLavato() && Input.GetKeyDown(KeyCode.E))
        {
            rifiuto.Lava();

            // Messaggio a schermo (se hai UIManager)
            UIManager ui = FindObjectOfType<UIManager>();
            if (ui != null)
                ui.MostraMessaggio("🧼 Hai lavato l'oggetto!");
        }
    }
}
