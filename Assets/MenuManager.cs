using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject pannelloOpzioni;

    public void AvviaGioco()
    {
        SceneManager.LoadScene("SampleScene"); // sostituisci con il nome reale della scena
    }

    public void ApriOpzioni()
    {
        if (pannelloOpzioni != null)
            pannelloOpzioni.SetActive(true);
    }

    public void ChiudiOpzioni()
    {
        if (pannelloOpzioni != null)
            pannelloOpzioni.SetActive(false);
    }

    public void MostraCrediti()
    {
        Debug.Log("🧑‍💻 Realizzato da Paolo Paradiso, 2025");
    }

    public void EsciGioco()
    {
        Application.Quit();
        Debug.Log("🏁 Hai chiuso il gioco");
    }
}
