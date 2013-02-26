using System;
using n.Core;

namespace n.Core
{
	/** Return a global instance of the application state */
	public interface nStateFactory
	{
		/** Return the global application state */
		nModel State { get; }

    /** Reset the state */
    void Reset();
	}
}

