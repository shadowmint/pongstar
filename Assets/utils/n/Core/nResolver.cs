using System;
using n.External.TinyIoC;
using System.Reflection;
using UnityEngine;

namespace n.Core
{
	/** Wrapper around the IOC so we can swap the implementation out easily */
	public class nResolver
	{
		/** Resolve instance of type T */
		public T Resolve<T>() where T : class {
			var rtn = (T) TinyIoCContainer.Current.Resolve<T>();
			return rtn;
		}

		/** 
     * Bind interface T to type K 
     * <p>
     * This is complex because the crappy .Net 3.5 runtime in unity 
     * is rubbish and doesn't support constraints properly.
     */
		public void Bind<T, K>() {
      MethodInfo method = typeof(nResolver).GetMethod ("Resolve");
      MethodInfo item = method.MakeGenericMethod (new Type[] { typeof(K) });
      var instance = (K) item.Invoke (this, null);
      TinyIoCContainer.Current.Register(typeof(T), instance);
		}
	}
}

