using UnityEngine;
using System.Collections.Generic;
using System;
using Scripts;

public class SkillComparer : IComparer<SOffensive> {

    public int Compare(SOffensive x, SOffensive y)
    {
        if (x == null)
        {
            if (y == null)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            if (y == null)
            {
                return 1;
            }
            else
            {
                if (x.tier == 0)
                {
                    return 1;
                }
                if(y.tier == 0)
                {
                    return -1;
                }
                int retval = (y.tier + y.skillLevel).CompareTo(x.tier + x.skillLevel);
                if (retval != 0)
                {
                    return retval;
                }
                else
                {
                    retval = x.skillLevel.CompareTo(y.skillLevel);
                    if (retval != 0)
                    {
                        return retval;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }
}
