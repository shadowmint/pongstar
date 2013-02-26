using UnityEngine;
using System.Collections;
using n.Test;
using System.Linq;
using System;
using System.Collections.Generic;
using n.Platform.Db;
using n.Platform.Db.Impl;
using n;
using n.Core;

namespace Tests
{
  public class nUnityDbTests : nTestBase 
  {
    private MyDbRecordType data ()
    {
      var rtn = new MyDbRecordType () {
        Ref = null,
        Value1 = 1,
        Value2 = 1,
        Value3 = true,
        Value4 = 1,
        Value5 = "1",
        Value6 = "first",
        Value7 = DateTime.Now
      };
      return rtn;
    }

    private nUnityDb setup () {
      #if UNITY_WEBPLAYER
        var c = new CookieDbContainer();
      #else
        var c = new FileDbContainer();
      #endif
      return new nUnityDb(c);
    }
    
    [nTest]
    public void test_can_create_instance() {
      var instance = setup();
      instance.ShouldNotBe(null);
    }

    [nTest]
    public void test_can_save_object() {
      #if !UNITY_WEBPLAYER
        var instance = setup();
        var item = data();
        var item2 = data();
        item.Id.ShouldBe(-1);
        item2.Id.ShouldBe(-1);

        instance.Setup<MyDbRecordType>(delegate (bool value) { value.ShouldBe(true); });
        instance.Clear<MyDbRecordType>(delegate (bool value) { value.ShouldBe(true); });
        instance.Insert<MyDbRecordType>(item, delegate (bool value) { value.ShouldBe(true); });
        instance.Insert<MyDbRecordType>(item2, delegate (bool value) { value.ShouldBe(true); });
        instance.Count<MyDbRecordType>(delegate (int value) { value.ShouldBe(2); });

        item.Id.ShouldNotBe(-1);
        nLog.Debug("New item id was: " +item.Id);

        item2.Id.ShouldNotBe(-1);
        nLog.Debug("New item id was: " +item2.Id);

        item.Id.ShouldNotBe(item2.Id);
      #endif
    }

    [nTest]
    public void test_can_get_object() {
      var instance = setup();
      var item = data();
      var item2 = data();
      item.Id.ShouldBe(-1);
      item.Value6 = "Item";
      
      instance.Setup<MyDbRecordType>(delegate {});
      instance.Clear<MyDbRecordType>(delegate {});
      instance.Insert<MyDbRecordType>(item, delegate {});
      instance.Insert<MyDbRecordType>(item2, delegate {});

      instance.Get<MyDbRecordType>(item.Id, delegate (MyDbRecordType value) {
        value.Value6.ShouldBe("Item");
        value.ShouldNotBe(null);
      });

      instance.Count<MyDbRecordType>(delegate (int count) {
        instance.All<MyDbRecordType>(0, count, delegate (IEnumerable<MyDbRecordType> value) { 
          value.Count().ShouldBe(2);
        });
      });
    }

    [nTest]
    public void test_can_delete_objects() {
      var instance = setup();
      var item = data();
      var item2 = data();
      item.Value6 = "Item";
      item2.Value6 = "Item2";
      
      instance.Setup<MyDbRecordType>(delegate {});
      instance.Clear<MyDbRecordType>(delegate {});
      instance.Insert<MyDbRecordType>(item, delegate {});
      instance.Insert<MyDbRecordType>(item2, delegate {});
      
      instance.Get<MyDbRecordType>(item.Id, delegate (MyDbRecordType value) {
        value.ShouldNotBe(null);
      });

      instance.Delete(item, delegate {
        instance.Get<MyDbRecordType>(item.Id, delegate (MyDbRecordType value) {
          value.ShouldBe(null);
        });
      });

      instance.Count<MyDbRecordType>(delegate (int count) {
        instance.All<MyDbRecordType>(0, count, delegate (IEnumerable<MyDbRecordType> value) { 
          value.Count().ShouldBe(1);
          var vitem = value.First();
          vitem.Value6.ShouldBe("Item2");
        });
      });
    }

    [nTest]
    public void test_can_update_objects() {
      var instance = setup();
      var item = data();
      item.Id.ShouldBe(-1);
      item.Value6 = "Item";
      
      instance.Setup<MyDbRecordType>(delegate {});
      instance.Clear<MyDbRecordType>(delegate {});
      instance.Insert<MyDbRecordType>(item, delegate {});
      instance.Get<MyDbRecordType>(item.Id, delegate (MyDbRecordType value) {
        value.Value6.ShouldBe("Item");
      });

      item.Value6 = "Item2";
      instance.Update(item, delegate {
        instance.Get<MyDbRecordType>(item.Id, delegate (MyDbRecordType value) {
          value.Value6.ShouldBe("Item2");
        });
      });
    }
  }
}
