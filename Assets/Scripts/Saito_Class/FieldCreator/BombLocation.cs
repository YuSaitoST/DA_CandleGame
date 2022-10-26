using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLocation : MonoBehaviour
{
    [SerializeField] bool isInstalled_ = false; // 設置状態


    // Start is called before the first frame update
    void Start()
    {
        isInstalled_ = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    // プレイヤーが入力した時、これを呼び出す
    public void ItemsInPlace()
    {
        // ゲーム終了後は処理をしない
        if (BombManager.instance_.IsAllInstalled())
        {
            return;
        }

        isInstalled_ = true;
        BombManager.instance_.Check_AllInstalled();   // 全ての設置エリアを調べる
    }

    public bool IsInstalled()
    {
        return isInstalled_;
    }
}
