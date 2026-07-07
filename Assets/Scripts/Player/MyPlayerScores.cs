using UnityEngine;
using UnityEngine.UI;

public class MyPlayerScores : MonoBehaviour
{
    public static int Scores = 0;
    public Text ScoresText;

    // Update is called once per frame
    void Update()
    {
        ScoresText.text = $"Score:{Scores}";
    }
}
