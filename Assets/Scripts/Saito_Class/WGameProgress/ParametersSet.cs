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
    public BOB bob;
    public NIC nic;
    public SPENCER spencer;
    public ALAN alan;
    public CATHERINE catherine;
    public GENERAL_FELLOW[] general_fellow;
    public YADOKAROCK yadokarock;
    public YADEKAROCK yadekarock;
    public RESULT result;
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
    public float rot_y;
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

[Serializable] public class BOB
{
    public float pos_x;
    public float pos_y;
    public float pos_z;
    public float rot_y;
}

[Serializable] public class NIC
{
    public float pos_x;
    public float pos_y;
    public float pos_z;
    public float rot_y;
}

[Serializable] public class SPENCER
{
    public float pos_x;
    public float pos_y;
    public float pos_z;
    public float rot_y;
}

[Serializable] public class GENERAL_FELLOW
{
    public int id;
    public int kind;
    public float pos_x;
    public float pos_y;
    public float pos_z;
    public float rot_y;
}

[Serializable] public class ALAN
{
    public float pos_x;
    public float pos_y;
    public float pos_z;
    public float rot_y;
}

[Serializable] public class CATHERINE
{
    public float pos_x;
    public float pos_y;
    public float pos_z;
    public float rot_y;
}

[Serializable] public class YADOKAROCK
{
    public float speed;
}

[Serializable] public class YADEKAROCK
{
    public float speed;
}

[Serializable] public class RESULT
{
    public int c_high;
    public int b_high;
    public int a_high;
    public int s_high;
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
