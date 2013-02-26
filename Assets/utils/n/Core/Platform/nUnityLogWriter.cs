using System;
using UnityEngine;

namespace n.Platform
{
	public class nUnityLogWriter : nLogWriter
	{
    public void Trace (string message)
    {
      Debug.Log(message);
    }
	}
}

