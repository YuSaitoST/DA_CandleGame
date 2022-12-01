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
        string inputString = Resources.Load<TextAsset>("InputData/ParameterData").ToString();
        param = JsonUtility.FromJson<Paramater>(inputString);
    }

    public Paramater GetParameter()
    {
        return param;
    }
}
