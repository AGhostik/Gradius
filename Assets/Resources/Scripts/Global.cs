using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    #region main
    public static int levelIndex;
    public static int randomSeed;
    #endregion

    #region input
    public static float inputHorizontal;
    public static float inputVertical;
    #endregion

    #region
    public static int scores;
    #endregion


    public static void ResetGameFields()
    {
        scores = 0;
    }
}