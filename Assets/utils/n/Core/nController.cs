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
using n.Platform;

namespace n.Core
{
	/** Extend this class for application controllers */
	public abstract class nController
	{
		/** Dispatch requests to open a new scene */
		private nDispatcher _dispatch;

		/** Raw copy of the global application state */
		protected nStateFactory _stateFactory;

		public nController() {
			var r = new nResolver();
			_dispatch = r.Resolve<nDispatcher>();
			_stateFactory = r.Resolve<nStateFactory>();
		}

    /** Launch a different scene */
    protected void Launch<T>() {
      _dispatch.Dispatch(typeof(T));
    }
	}
}

