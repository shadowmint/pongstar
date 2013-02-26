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

namespace n.Platform.Db.Impl
{
  /** Saves items as cookies in the browser & reads them */
  public class CookieDbContainer : IDbContainer 
  {
    /** Reads and writes to the browser */
    private AsyncBrowserComms _comms = new AsyncBrowserComms();

    public void Set (string key, string value, DbContainerAction<bool> cb)
    {
      var request = @"
        (function(key, value) { 
          if(typeof(Storage)!=='undefined') { 
            localStorage[key] = value; 
            return 'done';
          } 
          else { 
            alert('Your browser does not support local storage.'); 
            return 'fail';
          } 
        })('" + key + "', '" + value + "');";
      
      _comms.Invoke(request, delegate (string response) {
        var rtn = response == "done" ? true : false;
        cb.Invoke(rtn);
      });
    }

    public void Get (string key, DbContainerAction<string> cb)
    {
      var request = @"
        (function(key) { 
          if(typeof(Storage)!=='undefined') { 
            return localStorage[key];
          } 
          else { 
            alert('Your browser does not support local storage.'); 
            return '';
          } 
        })('" + key + "');";
      
      _comms.Invoke(request, delegate (string response) {
        cb.Invoke(response);
      });
    }

    public void Exists (string key, DbContainerAction<bool> cb)
    {
      var request = @"
        (function(key) { 
          if(typeof(Storage)!=='undefined') { 
            if (localStorage[key]) 
              return 'ok';
            else
              return 'fail';
          } 
          else { 
            alert('Your browser does not support local storage.'); 
            return '';
          } 
        })('" + key + "');";

      _comms.Invoke(request, delegate (string response) {
        var rtn = response == "ok" ? true : false;
        cb.Invoke(rtn);
      });
    }

    public void Clear (string key, DbContainerAction<bool> cb)
    {
      var request = @"
        (function(key) { 
          if(typeof(Storage)!=='undefined') { 
            localStorage[key] = null; 
            return 'done';
          } 
          else { 
            alert('Your browser does not support local storage.'); 
            return 'fail';
          } 
        })('" + key + "');";

      _comms.Invoke(request, delegate (string response) {
        var rtn = response == "done" ? true : false;
        cb.Invoke(rtn);
      });
    }
  }
}

