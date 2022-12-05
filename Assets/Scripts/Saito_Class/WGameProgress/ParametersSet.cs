using System;
using UnityEngine;

[Serializable] public class PARAM
{
    public Parameter param;
}

[Serializable] public class Paramater
{
    public FIELD field;
    public PLAYER player;
    public SUBMARINE submaline;
    public YADOKAROCK yadokarock;
}

[Serializable] public class FIELD
{
    public float x_clip;
    public float y_clip;
    public float z_clip;
}

[Serializable] public class PLAYER
{
    public float pos_x;
    public float pos_y;
    public float pos_z;
    public float move_speed;
    public float move_boost;
    public float oxy_max;
    public float oxy_cost_boost;
    public float oxy_cost;
    public float damage;
    public float knockback_power;
    public float knockback_power_up;
    public float knockback_stan_;
}

[Serializable] public class SUBMARINE
{
    public float pos_x;
    public float pos_y;
    public float pos_z;
}

[Serializable] public class YADOKAROCK
{
    public float speed;
}


public class ParametersSet
{
    Paramater param;

    public void SetParameter()
    {
        string _inputString = Resources.Load<TextAsset>("InputData/ParameterData").ToString();
        param = JsonUtility.FromJson<Paramater>(_inputString);
    }

    public Paramater GetParameter()
    {
        return param;
    }
}
