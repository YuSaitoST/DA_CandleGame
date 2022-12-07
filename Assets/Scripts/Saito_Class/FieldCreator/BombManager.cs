#define DEBUG_ON

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombManager : MonoBehaviour
{
    public static BombManager instance_;

    [SerializeField] GameObject prefab_location_    = null;
    [SerializeField] Text       txt_gameClear_      = null;

    BombLocation[] bombLocations_ = null;
    bool isAllInstalled_ = false;


    private void Awake()
    {
        if(instance_ == null)
        {
            instance_ = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        instance_.txt_gameClear_.text   = "";
        instance_.isAllInstalled_       = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //// ファイル読み込み
        //List<string>    _data   = ResourceReader.GetCSVReadData("BombLocationPos");
        //string[]        _strPos;

        //// プレハブ生成
        //GameObject _obj;
        //for(int i = 0; i < 4; ++i)
        //{
        //    _strPos = _data[i].Split(',');
        //    _obj    = Instantiate(prefab_location_, new Vector3(short.Parse(_strPos[0]), 0, short.Parse(_strPos[1])), Quaternion.identity);
        //    _obj.transform.parent = transform;
        //}
    }

    /// <summary>
    /// 全ての設置箇所に爆弾が置かれているかをチェックする
    /// </summary>
    public void Check_AllInstalled()
    {
        bool _isInstalled = false;
        foreach(var location in bombLocations_){
            _isInstalled = location.IsInstalled();
        }

        isAllInstalled_     = _isInstalled;
        txt_gameClear_.text = "Game Clear !";
    }

    /// <summary>
    /// 全ての設置箇所に置かれているかを取得する
    /// </summary>
    /// <returns>設置状態</returns>
    public bool IsAllInstalled()
    {
        return isAllInstalled_;
    }
}
