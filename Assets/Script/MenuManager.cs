using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public GameObject MenuPrincipale;
    public GameObject pannelloOpzioni;
    public AudioSource musicaMenu;
    public GameObject pannelloCrediti;
    public GameObject schermataIntro; // Pannello con la spiegazione
    public GameObject pannelloLivelli; // Pannello con i livelli
    private bool inAttesaInvio = false;



    public void AvviaGioco()
    {
        // Mostra la schermata introduttiva
        schermataIntro.SetActive(true);
        MenuPrincipale.SetActive(false);
        inAttesaInvio = true;
    }
    void Update()
    {
        if (inAttesaInvio && Input.GetKeyDown(KeyCode.Return)) // Invio
        {
            inAttesaInvio = false;

            if (musicaMenu != null)
            {
                StartCoroutine(FadeOutMusicaEAvviaScena("CASA"));
            }
            else
            {
                schermataIntro.SetActive(false);
                MusicManager.Instance?.PlayMusic();
                SceneManager.LoadScene("CASA");
            }
        }
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

    public void ApriLivelli()
    {
        if (pannelloLivelli!= null)
            pannelloLivelli.SetActive(true);

        if (MenuPrincipale != null)
            MenuPrincipale.SetActive(false);
    }

    public void ChiudiLivelli()
    {
        if (pannelloLivelli != null)
            pannelloLivelli.SetActive(false);
        if (MenuPrincipale != null)
            MenuPrincipale.SetActive(true);
    }

    public void SelezionaLivello(string nomeLivello)
    {
        Debug.Log($"🔍 Hai selezionato il livello: {nomeLivello}");
        if (musicaMenu != null)
        {
            StartCoroutine(FadeOutMusicaEAvviaScena(nomeLivello));
        }
        else
        {
            pannelloLivelli.SetActive(false);
            MusicManager.Instance?.PlayMusic();
            SceneManager.LoadScene(nomeLivello);
        }
    }

    public void EsciGioco()
    {
        Application.Quit();
        Debug.Log("🏁 Hai chiuso il gioco");
    }

    private IEnumerator FadeOutMusicaEAvviaScena(string nomeLivello)
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

        yield return new WaitForSeconds(0.1f); // Piccola attesa di sicurezza

        // AVVIA la musica del gioco PRIMA di cambiare scena
        MusicManager.Instance?.PlayMusic();

        SceneManager.LoadScene(nomeLivello);
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

    public void CambiaScena(string nomeScena)
    {
        SceneManager.LoadScene(nomeScena);
    }
}
