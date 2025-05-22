using UnityEngine;

public class BidoneCostruttore : MonoBehaviour
{
    [Header("Dimensioni interne del bidone")]
    public Vector3 dimensioniInterno = new Vector3(1f, 1f, 1f); // larghezza, altezza, profondità
    public float spessoreParete = 0.05f;
    public float altezzaPareti = 1f;

    private Transform centroInterno;

    void Start()
    {
        // Crea automaticamente il centro interno invisibile
        GameObject centro = new GameObject("CentroBidone_Automatico");
        centro.transform.SetParent(transform);
        centro.transform.localPosition = Vector3.zero; // Centro del bidone
        centro.transform.localRotation = Quaternion.identity;
        centroInterno = centro.transform;

        CreaStruttura();
    }

    void CreaStruttura()
    {
        // Fondo
        CreaParete("Fondo", new Vector3(0, -dimensioniInterno.y / 2f, 0), new Vector3(dimensioniInterno.x, spessoreParete, dimensioniInterno.z));

        // Parete Sinistra
        CreaParete("PareteSinistra", new Vector3(-dimensioniInterno.x / 2f + spessoreParete / 2f, 0, 0), new Vector3(spessoreParete, altezzaPareti, dimensioniInterno.z));

        // Parete Destra
        CreaParete("PareteDestra", new Vector3(dimensioniInterno.x / 2f - spessoreParete / 2f, 0, 0), new Vector3(spessoreParete, altezzaPareti, dimensioniInterno.z));

        // Parete Fronte
        CreaParete("PareteFronte", new Vector3(0, 0, dimensioniInterno.z / 2f - spessoreParete / 2f), new Vector3(dimensioniInterno.x, altezzaPareti, spessoreParete));

        // Parete Dietro
        CreaParete("PareteDietro", new Vector3(0, 0, -dimensioniInterno.z / 2f + spessoreParete / 2f), new Vector3(dimensioniInterno.x, altezzaPareti, spessoreParete));
    }

    void CreaParete(string nome, Vector3 localPos, Vector3 localScale)
    {
        GameObject parete = new GameObject(nome);
        parete.transform.SetParent(centroInterno);
        parete.transform.localPosition = localPos;
        parete.transform.localRotation = Quaternion.identity;
        parete.transform.localScale = localScale;

        BoxCollider collider = parete.AddComponent<BoxCollider>();
        collider.isTrigger = false;
    }
}
