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

namespace n.Test
{
  /** Assertion helper class */
  public static class nAssert
  {
    public static void ShouldBe(this bool actual, bool expected)
    {
      if (actual != expected) {
        throw new Exception("Invalid boolean condition: " + actual + " should be " + expected);
      }
    }
    
    public static void ShouldBe(this int actual, int expected)
    {
      if (actual != expected) {
        throw new Exception("Invalid integer condition: " + actual + " should be " + expected);
      }
    }

    public static void ShouldBe(this long actual, long expected)
    {
      if (actual != expected) {
        throw new Exception("Invalid long condition: " + actual + " should be " + expected);
      }
    }

    public static void ShouldBe(this double actual, double expected)
    {
      if (actual != expected) {
        throw new Exception("Invalid double condition: " + actual + " should be " + expected);
      }
    }

    public static void ShouldBe(this float actual, float expected)
    {
      if (actual != expected) {
        throw new Exception("Invalid float condition: " + actual + " should be " + expected);
      }
    }

    public static void ShouldBe(this object actual, object expected)
    {
      if (actual != expected) {
        string aname = actual == null ? "null" : actual.ToString();
        string ename = expected == null ? "null" : expected.ToString();
        throw new Exception("Invalid object condition: " + aname + " should be " + ename);
      }
    }
    
    public static void ShouldBe(this string actual, string expected)
    {
      if (actual != expected) {
        throw new Exception("Invalid string condition: \"" + actual + "\" should be \"" + expected + "\"");
      }
    }
    
    public static void ShouldNotBe (this bool actual, bool expected)
    {
      if (actual == expected) {
        throw new Exception("Invalid boolean condition: " + actual + " should not be " + expected);
      }
    }
    
    public static void ShouldNotBe (this object actual, object expected)
    {
      if (actual == expected) {
        string aname = actual == null ? "null" : actual.ToString();
        string ename = expected == null ? "null" : expected.ToString();
        throw new Exception("Invalid object condition: " + aname + " should not be " + ename);
      }
    }

    public static void ShouldNotBe(this long actual, long expected)
    {
      if (actual == expected) {
        throw new Exception("Invalid long condition: " + actual + " should not be " + expected);
      }
    }

    public static void ShouldNotBe(this double actual, double expected)
    {
      if (actual == expected) {
        throw new Exception("Invalid double condition: " + actual + " should not be " + expected);
      }
    }

    public static void ShouldNotBe(this float actual, float expected)
    {
      if (actual == expected) {
        throw new Exception("Invalid float condition: " + actual + " should not be " + expected);
      }
    }

    public static void ShouldNotBe(this int actual, int expected)
    {
      if (actual == expected) {
        throw new Exception("Invalid integer condition: " + actual + " should not be " + expected);
      }
    }
    
    public static void ShouldNotBe(this string actual, string expected)
    {
      if (actual == expected) {
        throw new Exception("Invalid string condition: \"" + actual + "\" should not be \"" + expected + "\"");
      }
    }
  }
}

