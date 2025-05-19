using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class BidoneGeneratoreEditor : MonoBehaviour
{
    [Header("Dimensioni interne del bidone")]
    public Vector3 dimensioniInterno = new Vector3(1f, 1f, 1f);
    public float spessoreParete = 0.05f;
    public float altezzaPareti = 1f;

    [ContextMenu("▶ Genera Pareti in Scena")]
    public void GeneraPareti()
    {
        // Elimina eventuali pareti esistenti (con lo stesso nome)
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Parete") || child.name == "Fondo")
                DestroyImmediate(child.gameObject);
        }

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

    private void CreaParete(string nome, Vector3 localPos, Vector3 localScale)
    {
        GameObject parete = new GameObject(nome);
        parete.transform.SetParent(transform);
        parete.transform.localPosition = localPos;
        parete.transform.localRotation = Quaternion.identity;
        parete.transform.localScale = localScale;

        var collider = parete.AddComponent<BoxCollider>();
        collider.isTrigger = false;

        // Aggiungi anche un MeshRenderer visibile per editor se vuoi
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        DestroyImmediate(cube.GetComponent<BoxCollider>());
        cube.transform.SetParent(parete.transform, false);
        cube.transform.localScale = Vector3.one;
        cube.GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Standard")) { color = new Color(1, 1, 1, 0.05f) };
    }
}
