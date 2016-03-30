using UnityEngine;
using System.Collections;
using System;

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class SpawnFunctionAssembly: Attribute
{
    private readonly monsterToSpawn mFunctionType;

    public SpawnFunctionAssembly(monsterToSpawn functionType)
    {
        mFunctionType = functionType;
    }

    public monsterToSpawn FunctionType
    {
        get { return mFunctionType; }
    }
}