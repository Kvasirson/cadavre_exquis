using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    Text m_score;

    GameManager _gameManager;

    private void OnEnable()
    {
        _gameManager = GameManager.GetInstance;
        m_score.text = "Score : " + _gameManager.Gold.ToString();
    }
}
