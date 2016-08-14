using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour
{

    public Vector3 center, size, halfSize;
    public GameObject pillarPrefab;
    public Transform startPillar;
    public float scaleRange = 50f;
    public AnimationCurve difficultyCurve;

    void OnEnable()
    {
        Player.OnLanded += CreatePillar;
    }

    void OnDisable()
    {
        Player.OnLanded -= CreatePillar;
    }

    // Use this for initialization
    void Start()
    {
        halfSize = size / 2;
        CreatePillar(startPillar);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreatePillar(Transform platform)
    {

        float difficultyModifier = (scaleRange * 2) * difficultyCurve.Evaluate(0.1f);

        Vector3 randomPos = new Vector3(center.x + platform.position.x + Random.Range(-halfSize.x, halfSize.x),
                                        center.y + Random.Range(-halfSize.y, halfSize.y),
                                        center.z + platform.position.z + Random.Range(-halfSize.z, halfSize.z));

        GameObject o = Instantiate(pillarPrefab, randomPos, Quaternion.Euler(90, Random.Range(0, 360), 0)) as GameObject;
        o.transform.localScale += new Vector3(Random.Range(-scaleRange, scaleRange), Random.Range(-scaleRange, scaleRange), 0);
        Vector3 v = Vector3.one * difficultyModifier;
        v.z = 0;
        o.transform.localScale += v;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, 0, transform.position.z) + center, size);
    }
}
