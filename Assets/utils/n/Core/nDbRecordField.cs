using System;
using System.Collections.Generic;

namespace n.Core
{
  /** Helper for custom ORM like functionality */
	public class nDbRecordField
	{
		public string Name { get; set; }

		public nDbRecordFieldType Type { get; set; }
    
    public object Value { get; set; }
    
    public T As<T>() {
      return (T) Value;
    }
	}
}

