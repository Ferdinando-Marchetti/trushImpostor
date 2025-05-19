using UnityEngine;

public class PickupThrow : MonoBehaviour
{
    public Transform holdPoint; // Oggetto vuoto davanti alla camera (es: "HandHold")
    public float pickupRange = 3f;
    public float throwForce = 500f;

    private GameObject heldObject;
    private Rigidbody heldRb;

    void Update()
    {
        // Raccolta e rilascio con E
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
                TryPickup();
            else
                DropObject();
        }

        // Lancio con click sinistro
        if (heldObject != null && Input.GetMouseButtonDown(0))
        {
            ThrowObject();
        }

        // Separazione oggetti composti con Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (heldObject != null)
            {
                RichiedeSeparazione separabile = heldObject.GetComponent<RichiedeSeparazione>();
                if (separabile != null && !separabile.separato)
                {
                    separabile.Separa();

                    // Rilascia l’oggetto composito dopo separazione
                    heldObject = null;
                    heldRb = null;
                }
            }
        }

        // Mantieni l'oggetto davanti alla telecamera
        if (heldObject != null)
        {
            heldObject.transform.position = holdPoint.position;
            heldObject.transform.rotation = holdPoint.rotation;
        }
    }

    void TryPickup()
    {
        Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.CompareTag("Trash"))
            {
                heldObject = hit.collider.gameObject;

                if (heldObject != null)
                {
                    heldRb = heldObject.GetComponent<Rigidbody>();

                    if (heldRb != null)
                    {
                        heldRb.isKinematic = true;
                        heldObject.transform.SetParent(holdPoint);
                        heldObject.transform.localPosition = Vector3.zero;
                        heldObject.transform.localRotation = Quaternion.identity;
                    }
                    else
                    {
                        Debug.LogWarning("❗ L'oggetto raccolto non ha un Rigidbody!");
                    }
                }
            }
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);
            if (heldRb != null)
            {
                heldRb.isKinematic = false;
            }

            heldObject = null;
            heldRb = null;
        }
    }

    void ThrowObject()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);

            if (heldRb != null)
            {
                heldRb.isKinematic = false;
                heldRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                heldRb.linearVelocity = Vector3.zero;
                heldRb.angularVelocity = Vector3.zero;
                heldRb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);

                Debug.Log($"[Lancio] isKinematic: {heldRb.isKinematic}");
            }

            heldObject = null;
            heldRb = null;
        }
    }
}
