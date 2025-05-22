using System.Collections;
using UnityEngine;
using TMPro;

namespace SojaExiles
{
    public class opencloseDoor : MonoBehaviour
    {
        public Animator openandclose;
        public bool open;
        public Transform Player;
        public float interactionDistance = 3f;
        public TMP_Text interactionText;
        public Camera playerCamera;

        private static opencloseDoor doorCurrentlyLookedAt;

        void Start()
        {
            open = false;
            if (interactionText != null)
                interactionText.enabled = false;
        }

        void Update()
        {
            if (Player == null || playerCamera == null || interactionText == null)
                return;

            float dist = Vector3.Distance(Player.position, transform.position);
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            bool isLookingAtThis = false;

            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                opencloseDoor hitDoor = hit.collider.GetComponentInParent<opencloseDoor>();

                if (hitDoor == this && dist < interactionDistance)
                {
                    isLookingAtThis = true;
                    doorCurrentlyLookedAt = this;
                }
            }

            // Mostra il messaggio SOLO se stai guardando questa porta
            if (doorCurrentlyLookedAt == this && isLookingAtThis)
            {
                interactionText.enabled = true;
                interactionText.text = open ? "Premi E per chiudere" : "Premi E per aprire";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!open)
                        StartCoroutine(opening());
                    else
                        StartCoroutine(closing());
                }
            }
            else
            {
                if (interactionText.enabled && doorCurrentlyLookedAt == this)
                {
                    interactionText.enabled = false;
                }
            }
        }

        IEnumerator opening()
        {
            Debug.Log("You are opening the door: " + gameObject.name);
            openandclose.Play("Opening");
            open = true;
            yield return new WaitForSeconds(0.5f);
        }

        IEnumerator closing()
        {
            Debug.Log("You are closing the door: " + gameObject.name);
            openandclose.Play("Closing");
            open = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
