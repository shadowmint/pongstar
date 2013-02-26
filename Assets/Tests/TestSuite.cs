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
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using n.Test;
using n.Test.Writers;
using n.Platform.Db.Impl;

// #define TEST_ALL
using Tests;

namespace Ps.Tests
{
  public class TestSuite : nTestRunner
  {
    protected override void Setup(nTestSuite tests)
    {
      #if TEST_ALL
        var assembly = typeof(Pongstar).Assembly;
        foreach (var c in assembly.GetTypes()) {
          if (c.IsSubclassOf(typeof(nTestBase)))
            tests.type = c;
        }
      #else
        tests.type = typeof(nQuadTests);
      #endif
    }
    
    public static void RunTests() {
      new TestSuite().Run<nSimpleTestWriter, nDebugTestWriter>();
    }
  }
}