using UnityEngine;

public class RichiedePulizia : MonoBehaviour
{
    public bool èPulito = false;

    public void Lava()
    {
        èPulito = true;
        Debug.Log("🧽 Oggetto lavato!");
    }

    public bool DeveEssereLavato()
    {
        return !èPulito;
    }
}
