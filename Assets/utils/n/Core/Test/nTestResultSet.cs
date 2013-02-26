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
using System.Linq;
using System.Collections.Generic;

namespace n.Test
{
  /** Results from a single test class */
  public class nTestResultSet
  {
    public Type Target { get; set; }
    
    public IList<nTestResult> Results = new List<nTestResult>();
    
    public string Name {
      get {
        return Target.FullName;
      }
    }
    
    public int Failed {
      get {
        var fails = from r in Results where !r.Success select r;
        return fails.Count();
      }
    }
    
    public int Passed {
      get {
        var ok = from r in Results where r.Success select r;
        return ok.Count();
      }
    }
    
    public bool Success {
      get {
        var fails = from r in Results where !r.Success select r;
        return !Results.Any() || fails.Any();
      }
    }
    
    public nTestResultSet (Type target)
    {
      Target = target;
      var methods = Target.GetMethods ();
      foreach (var m in methods) {
        var testAttribs = from a in m.GetCustomAttributes(typeof(nTest), false) select a;
        if (testAttribs.Any()) {
          var r = new nTestResult(m);
          Results.Add(r);
        }
      }
    }
  }
}

