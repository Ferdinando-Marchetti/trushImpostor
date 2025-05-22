using System.Collections;
using UnityEngine;
using TMPro;

namespace SojaExiles
{
    public class ClosetopencloseDoor : MonoBehaviour
    {
        public Animator Closetopenandclose;
        public bool open;
        public Transform Player;
        public float interactionDistance = 3f;
        public TMP_Text interactionText;
        public Camera playerCamera;

        private static ClosetopencloseDoor closetCurrentlyLookedAt;

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
                ClosetopencloseDoor hitCloset = hit.collider.GetComponentInParent<ClosetopencloseDoor>();

                if (hitCloset == this && dist < interactionDistance)
                {
                    isLookingAtThis = true;
                    closetCurrentlyLookedAt = this;
                }
            }

            if (closetCurrentlyLookedAt == this && isLookingAtThis)
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
                if (interactionText.enabled && closetCurrentlyLookedAt == this)
                {
                    interactionText.enabled = false;
                }
            }
        }

        IEnumerator opening()
        {
            Debug.Log("You are opening the closet: " + gameObject.name);
            Closetopenandclose.Play("ClosetOpening");
            open = true;
            yield return new WaitForSeconds(0.5f);
        }

        IEnumerator closing()
        {
            Debug.Log("You are closing the closet: " + gameObject.name);
            Closetopenandclose.Play("ClosetClosing");
            open = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
