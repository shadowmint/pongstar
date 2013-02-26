using System;

namespace n.Platform
{
	public interface nLogWriter
	{
    /** Log a single message */
		void Trace(string message);
	}
}

