using UnityEngine;
using UnityEngine.UI;
using Tomino;

public class ScoreView : MonoBehaviour
{
    public Text scoreText;
    public Game game;

    void Update()
    {
        var padLength = ConstantGame.ScoreFormat.Length;
        var padCharacter = ConstantGame.ScoreFormat.PadCharacter;
        scoreText.text = game.Score.Value.ToString().PadLeft(padLength, padCharacter);
    }
}
