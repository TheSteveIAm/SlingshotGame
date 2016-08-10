using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    bool dragging;
    float maxDrag = 3f;
    float forceMultiplier = 4f;
    Vector3 startPos, mouseStart;
    Camera cam;
    Rigidbody body;
    Transform fireLine;

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
        if (dragging)
        {
            deltaPos = startPos - Vector3.ClampMagnitude(cam.ScreenToWorldPoint(mouseStart) - cam.ScreenToWorldPoint(Input.mousePosition), maxDrag);
            deltaPos.y = transform.position.y;
            transform.position = deltaPos;
            deltaPos.y = transform.position.y - 2f;
            //Vector3 fireDrawLine = startPos - deltaPos;
            //fireDrawLine.y = 2f;
            //fireLine.LookAt(fireDrawLine, Vector3.up);
        }
        //else
        //{
        //    deltaPos = transform.position;
        //    startPos = deltaPos;
        //    mouseStart = deltaPos;
        //}
    }

    void OnMouseDown()
    {
        startPos = transform.position;
        mouseStart = Input.mousePosition;
        dragging = true;
        body.isKinematic = true;
    }

    void OnMouseUp()
    {
        dragging = false;
        //startPos = transform.position;
        //mouseStart = transform.position;
        body.isKinematic = false;
        Launch();
    }

    void Launch()
    {
        Vector3 shotForce = -deltaPos + startPos;
        //shotForce.y = -1000f;
        body.AddForce(shotForce * forceMultiplier, ForceMode.Impulse);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos, deltaPos);
        Gizmos.DrawWireSphere(startPos, 1f);
        Gizmos.DrawWireSphere(deltaPos, 2f);
    }
}
