using UnityEngine;

public class TrashItem : MonoBehaviour
{
    public string trashType; // plastica, carta, vetro, umido
    public bool ËEsplosivo = false;
    public bool ËLavabile = false;
    public bool ËComposto = false;  // Flag aggiunto
    public bool ËStatoRaccolto = false;  // Per messaggi al primo pickup
}
