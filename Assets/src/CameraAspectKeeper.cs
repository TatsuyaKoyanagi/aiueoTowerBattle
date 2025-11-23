using UnityEngine;

public class CameraAspectKeeper : MonoBehaviour
{
    // 想定している解像度（縦持ちスマホなら 9:16 が一般的）
    public Vector2 targetAspect = new Vector2(9.0f, 16.0f);

    private Camera cam;
    private float initialSize;

    void Start()
    {
        cam = GetComponent<Camera>();
        // 最初に設定されているカメラのサイズ（高さの半分）を基準として記憶する
        initialSize = cam.orthographicSize;
    }

    void Update()
    {
        // 画面サイズが変わったときに計算し直す
        UpdateCameraSize();
    }

    void UpdateCameraSize()
    {
        // 目標のアスペクト比（例：9 ÷ 16 = 0.5625）
        float targetRatio = targetAspect.x / targetAspect.y;

        // 現在の画面のアスペクト比
        float currentRatio = (float)Screen.width / (float)Screen.height;

        // もし現在の画面が、目標より「細長い（横幅が狭い）」なら
        if (currentRatio < targetRatio)
        {
            // 横幅が収まるようにカメラを引く（サイズを大きくする）
            // 倍率 = 目標比率 / 現在比率
            float scaleHeight = targetRatio / currentRatio;
            cam.orthographicSize = initialSize * scaleHeight;
        }
        else
        {
            // 横幅が十分あるなら、基準のサイズ（高さ固定）に戻す
            cam.orthographicSize = initialSize;
        }
    }
}