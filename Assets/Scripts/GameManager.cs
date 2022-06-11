using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform ballCamera;

    void Start()
    {
        EventManager.current.onStartGame += OnStartGame;
        EventManager.current.onFinishGame += OnFinishGame;
        EventManager.current.onShootBall += OnShootBall;
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
    
    void OnShootBall()
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
        EventManager.current.onShootBall -= OnShootBall;
    }
}
