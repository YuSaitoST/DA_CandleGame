using System.Collections.Generic;
using UnityEngine;

public class RandomNumberTool// : MonoBehaviour
{
    List<int> randomNunber_ = new List<int>();
    int min_;
    int max_;


    public RandomNumberTool(int min, int max)
    {
        min_ = min;
        max_ = max;

        for (int i = 0; i < max_; ++i)
        {
            randomNunber_.Add(i);
        }
    }

    //void Start()
    //{
    //    min_ = 0;
    //    max_ = 0;
    //}

    /*
     * @brief d•¡‚µ‚È‚¢—”‚ðŽæ“¾‚·‚é
     * @return —”
     */
    public int GetNoDuplicatesRN()
    {
        if (randomNunber_.Count == 0)
        {
            return -1;
        }

        int index;

        do
        {
            index = Random.Range(0, max_ - 1);
            if (randomNunber_.Count == 1) { 
                index = randomNunber_[0]; 
            }

        } while (randomNunber_.IndexOf(index) == -1);

        randomNunber_.Remove(index);

        return index;
    }
}
