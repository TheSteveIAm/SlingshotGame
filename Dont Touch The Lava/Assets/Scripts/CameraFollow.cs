using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    Vector3 targetPos, previousPos;

    Vector3 refVelocity = Vector3.zero;


    void OnEnable()
    {
        Player.OnLanded += SetTargetPos;
    }

    void OnDisable()
    {
        Player.OnLanded -= SetTargetPos;
    }
	
	// Update is called once per frame
	void Update () {
	    if(targetPos != previousPos)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref refVelocity , 0.3f);
        }
	}

    void SetTargetPos(Transform platform)
    {
        targetPos = platform.position;
        targetPos.y = transform.position.y;
    }
}
