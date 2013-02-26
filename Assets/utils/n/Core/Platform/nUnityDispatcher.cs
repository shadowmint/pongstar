using System;
using UnityEngine;
using n.Core;

namespace n.Platform
{
	/** The domain specific implementation should use a View to navigate to a new activity */
	public class nUnityDispatcher : nDispatcher
	{
    public void Dispatch (Type t)
    {
      var id = t.FullName;
      Application.LoadLevel(id);
    }
	}
}

