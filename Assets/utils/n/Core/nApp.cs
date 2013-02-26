/**
 * Copyright 2012 Douglas Linder
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using n.Platform;
using System.Reflection;
using UnityEngine;
using n.Core;

namespace n.Core
{

	/** The target application needs to implement and create an instance of this to use. */
	public abstract class nApp : MonoBehaviour 
	{
    public nApp() {
      nUnityBinding.Bind(_resolver);
      setup(_resolver);
    }

    /** So we dont need this on the implementation */
    public void Update() {
    }
    
		/** Bind service details for the application. */
		protected abstract void setup(nResolver resolver);

		/** The resolver to use for everything */
		protected nResolver _resolver = new nResolver();

		/** Return a controller instance */
		public T Controller<T>() 
    {
      MethodInfo method = typeof(nResolver).GetMethod ("Resolve");
      MethodInfo item = method.MakeGenericMethod (new Type[] { typeof(T) });
      var rtn = (T) item.Invoke (_resolver, null);
			return rtn;
		}
	}
}

