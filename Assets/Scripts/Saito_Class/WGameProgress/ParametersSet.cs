using System;
using UnityEngine;

[Serializable]
public class PARAM
{
    public Parameter param;
}

[Serializable]
public class Paramater
{
    public PLAYER player;
    public SUBMARINE submaline;

    // 調整後削除する、エラー回避用変数
    public float player_pos_x;
    public float player_pos_y;
    public float player_pos_z;
    public float sbmarine_pos_x;
    public float sbmarine_pos_y;
    public float sbmarine_pos_z;
}

[Serializable]
public class FIELD
{
    public float x_clip;
    public float y_clip;
    public float z_clip;
}

[Serializable]
public class PLAYER
{
    public float pos_x;
    public float pos_y;
    public float pos_z;
    public float move_speed;
    public float move_boost;
    public float oxy_max;
    public float oxy_cost_boost;
    public float oxy_cost;
}

[Serializable]
public class SUBMARINE
{
    public float pos_x;
    public float pos_y;
    public float pos_z;
}

public class ParametersSet
{
    Paramater param;

    public void SetParameter()
    {
        string inputString = Resources.Load<TextAsset>("InputData/TestParameterData").ToString();
        param = JsonUtility.FromJson<Paramater>(inputString);
    }

    public Paramater GetParameter()
    {
        return param;
    }
}
