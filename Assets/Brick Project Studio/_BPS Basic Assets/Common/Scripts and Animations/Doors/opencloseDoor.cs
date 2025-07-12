using System.Collections;
using UnityEngine;
using TMPro;



namespace SojaExiles
{
    public class opencloseDoor : MonoBehaviour
    {
        public Animator openandclose;
        public TMP_Text interactionText;
        public bool open = false;
        public float interactionDistance = 3f;
        public KeyCode interactKey = KeyCode.E;
        

        private Camera playerCamera;

        void Start()
        {
            playerCamera = Camera.main;

            if (interactionText != null)
                interactionText.text = ""; // nascondi il testo all'inizio
        }

        void Update()
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                if (hit.transform == transform)
                {
                    if (interactionText != null)
                    {
                        interactionText.text = open ? "PREMI E PER CHIUDERE" : "PREMI E PER APRIRE";
                    }

                    if (Input.GetKeyDown(interactKey))
                    {
                        if (!open)
                            StartCoroutine(opening());
                        else
                            StartCoroutine(closing());
                    }
                }
                else
                {
                    HideText();
                }
            }
            else
            {
                HideText();
            }
        }

        void HideText()
        {
            if (interactionText != null)
                interactionText.text = "";
        }

        IEnumerator opening()
        {
            Debug.Log("Apertura porta");
            openandclose.Play("Opening");
            open = true;
            yield return new WaitForSeconds(0.5f);
        }

        IEnumerator closing()
        {
            Debug.Log("Chiusura porta");
            openandclose.Play("Closing");
            open = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
