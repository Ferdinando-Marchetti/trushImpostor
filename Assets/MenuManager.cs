using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public GameObject MenuPrincipale;
    public GameObject pannelloOpzioni;
    public AudioSource musicaMenu;
    public GameObject pannelloCrediti;


    public void AvviaGioco()
    {
        if (musicaMenu != null)
            StartCoroutine(FadeOutMusicaEAvviaScena());
        else
            SceneManager.LoadScene("SampleScene");
    }

    public void ApriOpzioni()
    {
        if (pannelloOpzioni != null)
            pannelloOpzioni.SetActive(true);

        if (MenuPrincipale != null)
            MenuPrincipale.SetActive(false);
    }

    public void ChiudiOpzioni()
    {
        if (pannelloOpzioni != null)
            pannelloOpzioni.SetActive(false);

        if (MenuPrincipale != null)
            MenuPrincipale.SetActive(true);
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

    private IEnumerator FadeOutMusicaEAvviaScena()
    {
        float durata = 1f;
        float volumeIniziale = musicaMenu.volume;

        float t = 0f;
        while (t < durata)
        {
            t += Time.deltaTime;
            musicaMenu.volume = Mathf.Lerp(volumeIniziale, 0f, t / durata);
            yield return null;
        }

        musicaMenu.Stop();
        musicaMenu.volume = volumeIniziale;

        SceneManager.LoadScene("SampleScene");
    }
    public void ApriCrediti()
    {
        pannelloCrediti.SetActive(true);
        MenuPrincipale.SetActive(false);
    }

    public void ChiudiCrediti()
    {
        pannelloCrediti.SetActive(false);
        MenuPrincipale.SetActive(true);
    }
}
