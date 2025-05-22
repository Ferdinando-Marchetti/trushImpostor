using UnityEngine;

public class Lavandino : MonoBehaviour
{
    public float distanzaLavaggio = 2f; // distanza massima per lavare

    private Transform playerCamera;

    private void Start()
    {
        playerCamera = Camera.main.transform;
    }

    private void Update()
    {
        // Rileva se il giocatore ha un oggetto in mano
        PickupThrow pickup = FindObjectOfType<PickupThrow>();

        if (pickup != null && pickup.heldObject != null)
        {
            GameObject oggetto = pickup.heldObject;
            float distanza = Vector3.Distance(oggetto.transform.position, transform.position);

            if (distanza <= distanzaLavaggio && Input.GetKeyDown(KeyCode.E))
            {
                RichiedePulizia pulizia = oggetto.GetComponent<RichiedePulizia>();
                if (pulizia != null && pulizia.DeveEssereLavato())
                {
                    pulizia.Lava();
                    FindObjectOfType<UIManager>()?.MostraMessaggio("🧼 Oggetto lavato!");
                }
            }
        }
    }
}
