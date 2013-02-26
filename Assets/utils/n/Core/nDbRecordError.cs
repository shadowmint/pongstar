using System;
using System.Collections.Generic;

namespace n.Core
{
	public class nDbRecordError
	{
		public string Field { get; set; }

		public string Message { get; set; }

		public Exception Error { get; set; }

		public nDbRecordError(string field, string message, Exception e) {
			Field = field;
			Message = message;
			Error = e;
		}

		public override string ToString ()
		{
			var rtn = "";
			if (Error != null) 
				rtn = string.Format ("[nDbRecordError: Field={0}, Message={1}, Error={2}]", Field, Message, Error);
			else
				rtn = string.Format ("[nDbRecordError: Field={0}, Message={1}", Field, Message);
			return rtn;
		}
	}
}

