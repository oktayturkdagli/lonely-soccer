using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform ballCamera;
    [SerializeField] private Transform footballer;
    [SerializeField] private Transform ball;
    
    void Start()
    {
        EventManager.current.onStartGame += OnStartGame;
        EventManager.current.onFinishGame += OnFinishGame;
        EventManager.current.onWinGame += OnWinGame;
        EventManager.current.onLoseGame += OnLoseGame;
        EventManager.current.onKickTheBall += OnKickTheBall;
        EventManager.current.onStopTheBall += OnStopTheBall;
        EventManager.current.OnStartGame();
    }

    void OnStartGame()
    {
        // Debug.Log("Game is START!");
        Invoke(nameof(LateStart), 1f); // Game starts 1 second late to wait for all classes to load correctly
    }

    void OnFinishGame()
    {
        // Debug.Log("Game is OVER!");
    }
    
    void OnWinGame()
    {
        
    }
    
    void OnLoseGame()
    {
        
    }
    
    void OnStopTheBall()
    {
        bool isGoal = ball.GetComponent<Ball>().isGoal;
        if (isGoal)
        {
            Debug.Log("Goal!");
            EventManager.current.OnWinGame();
        }
        else
        {
            Debug.Log("Not Goal!");
            EventManager.current.OnLoseGame();
        }
        EventManager.current.OnFinishGame();
    }
    
    void OnKickTheBall()
    {
        ballCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
    }
    
    void LateStart()
    {
        
    }

    void OnDestroy()
    {
        EventManager.current.onStartGame -= OnStartGame;
        EventManager.current.onFinishGame -= OnFinishGame;
        EventManager.current.onWinGame += OnWinGame;
        EventManager.current.onLoseGame += OnLoseGame;
        EventManager.current.onKickTheBall -= OnKickTheBall;
        EventManager.current.onStopTheBall -= OnStopTheBall;
    }
}
