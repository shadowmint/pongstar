using System;
using System.Collections.Generic;
using System.Linq;

namespace n.Core
{
  /** Set of errors for a database record */
	public class nDbRecordErrors
	{
		/** Set of actual errors */
		private IList<nDbRecordError> _errors = new List<nDbRecordError>();

		/** If there are any errors */
		public bool Any { 
			get {
				return _errors.Any();
			}
		}

		/** The set of error messages */
		public IEnumerable<string> Messages { 
			get {
				var messages = from e in _errors select e.ToString();
				return messages;
			}
		}

		/** Generates a single summary message */
		public string Summary {
			get {
				var summary = "";
				foreach (var s in Messages) {
					summary += s.ToString () + "\n";
				}
				return summary;
			}
		}

		/** Add an error message */
		public void Add(string type, string message)
		{
			_errors.Add(new nDbRecordError(type, message, null));
        }

		/** Add an error message */
		public void Add(string type, string message, Exception e)
		{
			_errors.Add(new nDbRecordError(type, message, e));
		}

		/** Clear error messages */
		public void Clear() {
			_errors.Clear();
		}
	}
}

