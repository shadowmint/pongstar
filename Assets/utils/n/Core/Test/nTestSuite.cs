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
  /** A set of tests to run */
  public class nTestSuite
  {
    private IList<nTestResultSet> _results = new List<nTestResultSet>();
    
    /** Offset into the result set */
    private int offset = 0;
    
    /** Shortcut for attach */
    public Type type {
      set {
        Attach(value);
      }
    }
    
    /** Add a test to this suite */
    public void Attach (Type t)
    {
      if (typeof(nTestBase).IsAssignableFrom (t)) {
        var duplicate = from rs in _results where rs.Target == t select rs;
        if (!duplicate.Any()) {
          var r = new nTestResultSet(t);
          _results.Add(r);
        }
      } 
      else {
        throw new Exception(String.Format("Invalid binding: {0} does not extend nTestBase", t.Name));
      }
    }
    
    /** Returns the next result set or null */
    public nTestResultSet Next ()
    {
      nTestResultSet rtn = null;
      if (_results.Count () > offset) {
        rtn = _results.ElementAt(offset);
        ++offset;
      }
      return rtn;
    }
    
    /** Return the result set */
    public IEnumerable<nTestResultSet> Results {
      get {
        return _results;
      }
    }
  }
}

