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
using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using n.Core;

namespace n.Platform.Db.Impl
{
  /** Callback type */
  public delegate void AsyncBrowserCallback(string value);

  /** Reads and writes to the browser */
  public class AsyncBrowserComms {
  
    /** Id key */
    private int key = 0;

    /** Unique name for messenger; change this if it conflicts (uses md5(cookiedbcontainer)) */
    private const string NAME = "CookieDbContainer__f2c5c6ea33ca033089cbc787eddffacd";

    /** The name of the game object to use for this handler */
    private static GameObject _mobj = null;

    /** The messenger behaviour instance */
    private static AsyncBrowserMessenger _messenger = null;

    /** Create the messenger if it doesnt exist */
    private void InitMessenger ()
    {
      if (_mobj == null) {
        _mobj = (GameObject) new GameObject(NAME);
        _mobj.AddComponent<AsyncBrowserMessenger>();
        _messenger = _mobj.GetComponent<AsyncBrowserMessenger>();
      }
    }

    /** 
     * Request a callback from the browser.
     * <p>
     * The returned result will be the result of running eval on the js code 
     * in the parent browser.
     * <p>
     * The return is: var value = js; return value;
     * <p>
     * A typical js call might be: 
     * <pre>
     *    (function() { return "hello world"; })();
     * </pre>
     */
    public void Invoke(string js, AsyncBrowserCallback callback) {
      InitMessenger();
      var id = key;
      ++key;
      _messenger.Add(id, callback);
      var cmd = String.Format("var rvalue = {0}; var u = GetUnity(); u.SendMessage('{1}', 'Message', '{2}!'+rvalue);", js, NAME, id);
      Application.ExternalEval(cmd);
    }
  }

  public class AsyncBrowserMessenger : MonoBehaviour {

    /** Set of callbacks */
    private Dictionary<int, AsyncBrowserCallback> _callbacks = new Dictionary<int, AsyncBrowserCallback>();

    /** Add a callback to invoke when it happens */
    public void Add(int id, AsyncBrowserCallback callback) {
      _callbacks[id] = callback;
    }

    public void Message (string value)
    {
      var offset = value.IndexOf ("!");
      var id = value.Substring (0, offset);
      int real_id; 
      if (!int.TryParse (id, out real_id)) 
        nLog.Debug ("Malformed message: {0}. Should start like: 324324!mymessagehere", value);
      else {
        var real_value = value.Substring(offset + 1);
        if (_callbacks.ContainsKey(real_id)) {
          var cb = _callbacks[real_id];
          _callbacks.Remove(real_id);
          cb.Invoke(real_value);
        }
        else
          nLog.Debug("Requested a callback for id {0} but no match was found", real_id);
      }
    }
  }
}

