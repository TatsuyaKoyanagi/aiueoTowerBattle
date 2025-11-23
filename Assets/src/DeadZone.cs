using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{
    [Header("設定")]
    public GameObject gameOverUI; // ゲームオーバー画像
    public GameObject restartButton; // リスタートボタン
    public GameManager gameManager; // GameManagerとの連携用

    // ボタンが押されたときの動き
    public void OnRetryButtonClick()
    {
        // 時間を動かす
        Time.timeScale = 1f;
        // シーンを読み込み直す
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ゲームオーバー！");

        // GameManagerに反映
        if (gameManager != null)
        {
            gameManager.isGameOver = true;
        }

        // 画像とボタンの表示
        gameOverUI.SetActive(true);
        restartButton.SetActive(true); 
    }
}