using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public Text scoreText, multiText;
    int score, multi = 1;
    public SimpleHelvetica text;

    void OnEnable()
    {
        Player.OnLanded += AddScore;
        Player.OnMiss += Miss;

        multiText.text = "x" + multi.ToString();
    }

    void OnDisable()
    {
        Player.OnLanded -= AddScore;
        Player.OnMiss -= Miss;
    }

    void AddScore(Transform platform)
    {

        score += 1 * multi;

        if (multi < 10)
        {
            multi++;
        }

        scoreText.text = score.ToString();

        multiText.text = "x" + multi.ToString();
    }

    void Miss()
    {
        multi = 1;

        multiText.text = "x" + multi.ToString();
    }
}
