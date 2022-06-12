using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dayText;
    [SerializeField] TextMeshProUGUI goalText;
    [SerializeField] TextMeshProUGUI levelCompleteText;
    [SerializeField] private Button nextButton;
    [SerializeField] TextMeshProUGUI missText;
    [SerializeField] TextMeshProUGUI tryAgainText;
    [SerializeField] private Button restartButton;
    [SerializeField] private ParticleSystem[] goalEffects;

    void Start()
    {
        EventManager.current.onStartGame += OnStartGame;
        EventManager.current.onFinishGame += OnFinishGame;
        EventManager.current.onWinGame += OnWinGame;
        EventManager.current.onLoseGame += OnLoseGame;
    }
    
    void OnStartGame()
    {
        dayText.DOFade(0f, 3f);
    }
    
    void OnFinishGame()
    {
        
    }
    
    void OnWinGame()
    {
        goalText.transform.gameObject.SetActive(true);
        goalText.transform.localPosition = new Vector3(transform.localPosition.x - 500, transform.position.y, transform.position.z);
        goalText.transform.DOMoveX(500, 1f).OnComplete(() =>
        {
            goalText.DOFade(0f, 1f).OnComplete(() =>
            {
                nextButton.transform.gameObject.SetActive(true);
                levelCompleteText.transform.gameObject.SetActive(true);
                levelCompleteText.DOFade(100f, 1f).OnComplete(() =>
                {
                    EventManager.current.OnFinishGame();
                });
            });
        });
        for (int i = 0; i < goalEffects.Length; i++)
        {
            goalEffects[i].transform.gameObject.SetActive(true);
            goalEffects[i].Play();
        }
    }
    
    void OnLoseGame()
    {
        missText.transform.gameObject.SetActive(true);
        missText.transform.localPosition = new Vector3(transform.localPosition.x - 500, transform.position.y, transform.position.z);
        missText.transform.DOMoveX(500, 1f).OnComplete(() =>
        {
            missText.DOFade(0f, 1f).OnComplete(() =>
            {
                restartButton.transform.gameObject.SetActive(true);
                tryAgainText.transform.gameObject.SetActive(true);
                tryAgainText.DOFade(100f, 1f).OnComplete(() =>
                {
                    EventManager.current.OnFinishGame();
                });
            });
        });
    }
    
    public void OnPressNextButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void OnPressRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void OnDestroy()
    {
        EventManager.current.onStartGame -= OnStartGame;
        EventManager.current.onFinishGame -= OnFinishGame;
        EventManager.current.onWinGame -= OnWinGame;
        EventManager.current.onLoseGame -= OnLoseGame;
    }
}
