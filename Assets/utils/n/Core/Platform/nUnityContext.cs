using System;

namespace n.Platform
{
  /** Unity doesn't really need a context; this is a just a dummy class */
	public class nUnityContext : nContext
	{
    /** Return a platform specific context object */
    public T Get<T>() {
      return default(T);
    }
	}
}

