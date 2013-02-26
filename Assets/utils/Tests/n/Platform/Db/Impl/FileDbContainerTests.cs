using UnityEngine;
using System.Collections;
using n.Test;
using System.Linq;
using System;
using System.Collections.Generic;
using n.Platform.Db;
using n.Platform.Db.Impl;

namespace Tests
{
  public class FileDbContainerTests : nTestBase
  {
    #if !UNITY_WEBPLAYER
    private FileDbContainer setup ()
    {
      return new FileDbContainer ();
    }
    
    [nTest]
    public void test_can_create_instance ()
    {
      var instance = setup ();
      instance.ShouldNotBe (null);
    }

    [nTest]
    public void test_can_insert_value_and_read_it ()
    {
      var instance = setup ();
      instance.Set ("example.key", "example.value", delegate {});
      instance.Set ("example.2.key", "example.2.value", delegate {});

      string value1;
      instance.Get("example.key", delegate (string value) { value1 = value; });
      value1.ShouldNotBe(null);
      value1.ShouldBe("example.value");

      string value2;
      instance.Get("example.2.key", delegate (string value) { value2 = value; });
      value2.ShouldNotBe(null);
      value2.ShouldBe("example.2.value");
    }

    [nTest]
    public void test_can_check_value_exists ()
    {
      var instance = setup ();
      instance.Set ("example.key", "example.value", delegate {});
      instance.Set ("example.2.key", "example.2.value", delegate {});
      instance.Set ("example.3.key", "example.3.value", delegate {});
      instance.Clear ("example.3.key", delegate {});
      instance.Clear ("example.4.key", delegate {});

      instance.Exists("example.key", delegate (bool value) { value.ShouldBe(true); });
      instance.Exists("example.2.key", delegate (bool value) { value.ShouldBe(true); });
      instance.Exists("example.3.key", delegate (bool value) { value.ShouldBe(false); });
      instance.Exists("example.4.key", delegate (bool value) { value.ShouldBe(false); });
    }
    #endif
  }
}
