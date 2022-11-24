using UnityEngine;

public class Parameters {
    public float player_pos_x;
    public float player_pos_y;
    public float player_pos_z;
    public float sbmarine_pos_x;
    public float sbmarine_pos_y;
    public float sbmarine_pos_z;
}


public class ParametersSet
{
    Parameters parameters_;

    public void SetParameter()
    {
        string inputString = Resources.Load<TextAsset>("input").ToString();
        parameters_ = JsonUtility.FromJson<Parameters>(inputString);


        Debug.Log(parameters_.player_pos_x);
        Debug.Log(parameters_.player_pos_y);
        Debug.Log(parameters_.player_pos_z);
    }

    public Parameters GetParameter()
    {
        return parameters_;
    }
}