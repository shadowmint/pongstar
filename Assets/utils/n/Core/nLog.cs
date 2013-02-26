using System;
using n.Platform;

namespace n.Core
{
	public class nLog
	{
		private static nLogWriter _writer = null;

		private static nLogWriter Instance() {
			if (_writer == null) {
				var r = new nResolver();
				_writer = r.Resolve<nLogWriter>();
			}
			return _writer;
		}

    /** Use this for tests, etc. */
    public static void ForceWriterUpdate(nLogWriter writer) {
      _writer = writer;
    }

    public static void Debug (object o)
    {
      Debug("" + o);
    }

    public static void Debug (string fmt, params object[] args) {
      var message = String.Format(fmt, args);
      Debug(message);
    }
    
		public static void Debug(string message) {
			Instance().Trace(message);
		}

    public static void Info (string fmt, params object[] args) {
      var message = String.Format(fmt, args);
      Info(message);
    }
    
		public static void Info(string message) {
			Instance().Trace(message);
		}

    public static void Error (string fmt, Exception e, params object[] args) {
      var message = String.Format(fmt, args);
      Error(message, e);
    }
    
		public static void Error(string message, Exception e) {
			Instance().Trace(message + ": " + e.ToString());
		}

    public static void DebugArray(float[] args) {
      string msg;
      if (args == null)
        msg = "(NULL)";
      else if (args.Length == 0) 
        msg = "[]";
      else {
        msg = "";
        for (var i = 0; i < args.Length; ++i) {
          msg = msg + args[i].ToString();
          if (i != (args.Length - 1)) 
            msg += ", ";
        }
        msg = "[ " + msg + " ]";
      }
      Debug(msg);
    }
	}
}

