using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class GameManager : MonoBehaviour
{
    [Header("設定")]
    public GameObject[] irasutotati; // 落ちてくるひらがな
    public Transform spawnPoint; // 出現位置
    public float rotateSpeed = 100f; // 回転スピード

    [Header("状態確認用")]
    public bool isGameOver = false; // ゲームオーバー判定

    private GameObject currentBlock; // 今操作しているブロック
    private bool isWaiting = false; // 次の生成待機中かどうか

    void Start()
    {
        SpawnBlock(); // ゲーム開始時に最初の1個を出す
    }

    void Update()
    {
        // ゲームオーバー時や、次の生成待機中、またはブロックがない時は操作しない
        if (isGameOver || isWaiting || currentBlock == null) return;

        // UIを触っている時は操作を拒否
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // 右クリックを押している間回転する
        if (Input.GetMouseButton(1)) // 0が左、1が右、2が中ホイール
        {
            // 時計回りに回転
            currentBlock.transform.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);
        }

        // 左クリックを押している間横移動する 
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // 現在のブロックの位置を取得
            Vector3 newPos = currentBlock.transform.position;
            
            // X座標だけマウスに合わせる
            newPos.x = mousePos.x;

            // 位置を更新
            currentBlock.transform.position = newPos;
        }

        // 左クリックを離した瞬間落下する
        if (Input.GetMouseButtonUp(0))
        {
            DropBlock();
        }
    }

    // ブロックを生成する機能
    void SpawnBlock()
    {
        if (isGameOver) return;

        // ランダムに選ぶ
        int number = Random.Range(0, irasutotati.Length);
        
        // SpawnPointの場所に生成する
        currentBlock = Instantiate(irasutotati[number], spawnPoint.position, Quaternion.identity);

        float randomScale = Random.Range(0.5f, 1.0f);
        currentBlock.transform.localScale = new Vector3(randomScale, randomScale, 1f);

        // 落とすまでは物理演算を無効にする
        Rigidbody2D rb = currentBlock.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; 
            rb.velocity = Vector2.zero; //慣性の削除
            rb.angularVelocity = 0f;
        }
    }

    // ブロックを落とす機能
    void DropBlock()
    {
        // 物理演算を有効にする
        Rigidbody2D rb = currentBlock.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        // 現在のブロックの手を離す
        currentBlock = null;

        // 次のブロックが出るまで少し待つ処理を開始
        StartCoroutine(WaitAndSpawnNext());
    }

    // 1秒待ってから次を出すコルーチン
    IEnumerator WaitAndSpawnNext()
    {
        isWaiting = true;
        yield return new WaitForSeconds(1.0f); // 1秒待機
        isWaiting = false;
        SpawnBlock();
    }
}