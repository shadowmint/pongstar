using UnityEngine;
using System.Collections;
using n.Test;
using System.Linq;
using System;
using System.Collections.Generic;
using n.Platform.Db;
using n.Core;

namespace Tests
{
  public class TestLogWriterTests : nTestBase 
  {
    [nTest]
    public void test_can_write_log_messages()
    {
      nLog.Debug("Debug message");
      nLog.Info("Info message");
      nLog.Error("Error message", new Exception("Hello World"));
      
      nLog.Debug("Formatted debug {0} message", 100);
      nLog.Info("Info message {0} with format", "example");
      nLog.Error("Error code {0} with message", new Exception("Hello World"), 99);
    }
  }
}
