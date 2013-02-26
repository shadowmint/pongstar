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
using System.IO;

namespace n.Test.Writers
{
  /** Write a simple human readable log format */
  public class nSimpleTestWriter : nTestWriter
  {
    private StreamWriter _w;

    public void Write (string path, string message)
    {
      using (_w = new StreamWriter(path)) {
        Log (message);
      }
    }

    public void Write (string path, nTestSuite suite)
    {
      using (_w = new StreamWriter(path))
      {
        int failed = 0;

        Log("Test results:\n============\n");
        Log("Raw test results:\n----------------");
        foreach (var testset in suite.Results) {
          PrintClassInfo(testset);
          foreach (var test in testset.Results) {
            PrintMethodInfo(testset, test, true);
          }
        }
        
        Log("\nSummary test results:\n--------------------");
        foreach (var testset in suite.Results) {
          PrintClassInfo(testset, true);
          foreach (var test in testset.Results) {
            PrintMethodInfo(testset, test);
            if (!test.Success) 
              ++failed;
          }
        }      
        
        Log("\nSummary results:\n---------------") ;
        foreach (var testset in suite.Results) {
          PrintClassInfo(testset);
        }      
        
        Log ("- Total: {0} tests failed", failed);

        if (failed > 0) {
          Log("\nFAILING TESTS:\n-------------") ;
          foreach (var testset in suite.Results) {
            foreach(var test in testset.Results) {
              if (!test.Success)
                PrintMethodInfo(testset, test);
            }
          }
        }
      }
    }

    private void PrintClassInfo (nTestResultSet item, bool withLeader = false)
    {
      if (withLeader)
        Log ("\n- {0}: {1}/{2} passed", item.Name, item.Passed, item.Passed + item.Failed);
      else
        Log ("- {0}: {1}/{2} passed", item.Name, item.Passed, item.Passed + item.Failed);
    }

    private void PrintMethodInfo (nTestResultSet parent, nTestResult item, bool withDebugAndErrors = false)
    {
      var success = item.Success ? "    " : "fail";
      Log ("-- {0} - {1}::{2}", success, parent.Name, item.Name);
      if (withDebugAndErrors) {
        if (item.Log.Length > 0) {
          Log ("------------------ DEBUG BLOCK -----------------");
          Log ("{0}", item.Log.ToString ());
          Log ("------------------------------------------------");
        }
        if (!item.Success) {
          Log ("------------------ ERROR BLOCK -----------------");
          Log ("{0}", item.Error.ToString ());
          Log ("------------------------------------------------");
        }
      }
    }
    
    private void Log(string fmt, params object[] values) 
    {
      var msg = String.Format(fmt, values);
      _w.WriteLine(msg);
    }
  }
}

