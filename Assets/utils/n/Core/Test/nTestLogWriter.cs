//
//  Copyright 2012  douglasl
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Collections.Generic;
using n.Platform;
using n.Core;

namespace n.Test
{
  /** 
   * Setup and run tests by extending this class 
   * <p>
   * Notice this is a simple and stupid test runner; it does *not*
   * support running tests in parallel.
   */

  /** Writes std logs to the test logs */
  public class nTestLogWriter : nLogWriter {

    public nTestResult _results;

    public nTestLogWriter(nTestResult results) {
      _results = results;
    }

    public void Trace(string message) {
      _results.Log.Append(message + "\n");
      UnityEngine.Debug.Log(message);
    }
  }
}
