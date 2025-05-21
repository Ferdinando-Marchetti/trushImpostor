using UnityEngine;

public class RichiedePulizia : MonoBehaviour
{
    [Header("Stato dell'oggetto")]
    public bool èPulito = false;

    /// <summary>
    /// Chiama questo metodo per lavare l'oggetto.
    /// </summary>
    public void Lava()
    {
        èPulito = true;
        Debug.Log($"🧽 {gameObject.name} è stato lavato!");
        // Qui puoi aggiungere effetti visivi o suoni se vuoi
    }

    /// <summary>
    /// Ritorna true se l'oggetto deve ancora essere lavato.
    /// </summary>
    public bool DeveEssereLavato()
    {
        return !èPulito;
    }
}
