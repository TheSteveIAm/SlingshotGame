using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    bool dragging, flying, launched; //maybe change to state enum?
    float maxDrag = 3f;
    float forceMultiplier = 4f;
    Vector3 startPos, mouseStart, lastPos;
    Camera cam;
    Rigidbody body;
    Transform fireLine, lastPlatform;

    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
        body = GetComponent<Rigidbody>();
        fireLine = GetComponentInChildren<PlayerFire>().transform;
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

            if (body.velocity != Vector3.zero && !launched)
            {
                launched = true;
            }

            if (launched && body.velocity == Vector3.zero)
            {
                launched = false;
                flying = false;
                lastPos = new Vector3(lastPlatform.position.x, transform.position.y, lastPlatform.position.z);
                transform.position = lastPos;
                transform.rotation = Quaternion.Euler(Vector3.forward);
            }
        }

        if(transform.position.y <= -10f)
        {
            transform.position = lastPos;
            body.velocity = Vector3.zero;
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
        }
    }

    void OnMouseUp()
    {
        if (!flying)
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
        body.AddTorque(transform.right * 0.3f, ForceMode.Impulse);
        flying = true;
    }

    void OnCollisionEnter(Collision col)
    {
        lastPlatform = col.transform;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(startPos, deltaPos);
    //    Gizmos.DrawWireSphere(startPos, 1f);
    //    Gizmos.DrawWireSphere(deltaPos, 2f);
    //}
}
