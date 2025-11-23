using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    // ボタンが押されたらscene移動
    public void OnStartCheck()
    {
        SceneManager.LoadScene("GameScene");
    }
}