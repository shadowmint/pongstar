using System;

namespace n.Platform
{
  /** Generic platform specific handle to hold context information */
	public interface nContext
	{
    /** Return a platform specific context object */
    T Get<T>();
	}
}
