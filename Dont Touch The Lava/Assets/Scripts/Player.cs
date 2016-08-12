using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    bool dragging, flying, launched; //maybe change to state enum?
    float maxDrag = 3f;
    [SerializeField]
    float forceMultiplier = 4f;

    float landTimer = 0f, landTime = 0.25f;
    Vector3 startPos, mouseStart, lastPos;
    Camera cam;
    Rigidbody body;
    Transform fireLine, lastPlatform, lastLanded;
    LineRenderer line;
    Collider col;

    public delegate void LandDelegate(Transform platform);
    public static event LandDelegate OnLanded;

    public delegate void MissDelegate();
    public static event MissDelegate OnMiss;

    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
        body = GetComponent<Rigidbody>();
        fireLine = GetComponentInChildren<PlayerFire>().transform;
        line = GetComponent<LineRenderer>();
        col = GetComponent<Collider>();
    }

    Vector3 deltaPos;

    // Update is called once per frame
    void Update()
    {
        if (dragging && !flying)
        {
            deltaPos = startPos - Vector3.ClampMagnitude(cam.ScreenToWorldPoint(mouseStart) - cam.ScreenToWorldPoint(Input.mousePosition), maxDrag);
            deltaPos.y = transform.position.y;
            transform.position = deltaPos;
            deltaPos.y = transform.position.y - 2f;
            transform.LookAt(startPos + new Vector3(0, 2, 0));
            //Vector3 fireDrawLine = startPos - deltaPos;
            //fireDrawLine.y = 2f;
            //fireLine.LookAt(fireDrawLine, Vector3.up);
        }
        else if (flying)
        {
            //transform.LookAt(transform.position + body.velocity);

            //init launch
            if (body.velocity != Vector3.zero && !launched)
            {
                launched = true;
                line.enabled = false;
            }

            //landed
            if (launched && body.velocity == Vector3.zero)
            {
                if(landTimer <= landTime)
                {
                    landTimer += Time.smoothDeltaTime;
                    return;
                }

                GameObject toDestroy = null;

                if (lastPlatform != lastLanded)
                {
                    toDestroy = lastLanded.gameObject;
                }

                landTimer = 0f;
                lastLanded = lastPlatform;
                launched = false;
                flying = false;
                lastPos = new Vector3(lastLanded.position.x, transform.position.y, lastLanded.position.z);
                transform.position = lastPos;
                transform.rotation = Quaternion.Euler(Vector3.forward);
                line.enabled = true;

                if (OnLanded != null && toDestroy != null)
                {
                    OnLanded(lastLanded);

                    Destroy(toDestroy);
                }
            }
        }

        if (transform.position.y <= -10f)
        {
            transform.position = lastPos;
            body.velocity = Vector3.zero;

            if(OnMiss != null)
            {
                OnMiss();
            }
        }
    }

    void OnMouseDown()
    {
        if (!flying)
        {
            startPos = transform.position;
            mouseStart = Input.mousePosition;
            dragging = true;
            body.isKinematic = true;
            col.enabled = false;
        }
    }

    void OnMouseUp()
    {
        if (!flying && dragging)
        {
            dragging = false;
            //startPos = transform.position;
            //mouseStart = transform.position;
            body.isKinematic = false;
            Launch();
        }
    }

    void Launch()
    {
        Vector3 shotForce = -deltaPos + startPos;
        //shotForce.y = -1000f;
        body.AddForce(shotForce * forceMultiplier, ForceMode.Impulse);
        body.AddTorque(transform.right * 1.5f, ForceMode.Impulse);
        flying = true;
        StartCoroutine(ColliderDelay(0.2f));
    }

    void OnCollisionEnter(Collision col)
    {
        lastPlatform = col.transform;

        //initial landing
        if (lastLanded == null)
        {
            lastLanded = lastPlatform;
        }

        body.velocity *= 0.25f;

    }

    IEnumerator ColliderDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        col.enabled = true;
    }
}
