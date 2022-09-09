using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    Text m_score;

    GameManager _gameManager;

    void Start()
    {
        _gameManager = GameManager.GetInstance;
    }

    private void OnEnable()
    {
        m_score.text = _gameManager._gold.ToString();
    }
}
