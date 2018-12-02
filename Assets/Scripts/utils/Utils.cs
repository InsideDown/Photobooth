using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : Singleton<Utils> {

    protected Utils() {}

    public void CheckRequired(object thing, string name = null)
    {
        if (thing == null)
        {
            if (!string.IsNullOrEmpty(name))
            {
                throw new System.Exception(String.Format("A {0} is required to run this scene.", name));
            }else {
                throw new System.Exception(String.Format("A {0} is required to run this scene.", thing.ToString()));
            }
        }
    }
}
