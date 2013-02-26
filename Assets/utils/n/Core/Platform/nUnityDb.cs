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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using n.Platform.Db.Impl;
using n.Core;

namespace n.Platform.Db
{
  public class nUnityDb : nDb
  {
    private IDbContainer _dbc;
    private RecordSerializer _rs;
    
    public nUnityDb (IDbContainer container)
    {
      _dbc = container;
      _rs = new RecordSerializer ();
    }

    private string Key (Type t)
    {
      return "nUnityDbRecordSet." + t.FullName;
    }

    /** Converts the method into a generic of return type K and returns a K */
    private object InvokeGenericMethod (Type itype, Type rtype, object instance, string method, params object[] args)
    {
      /* default return */
      object rtn;
      if (rtype.IsValueType)
        rtn = Activator.CreateInstance (rtype);
      else
        rtn = null;
      
      /* invoke */
      var m = itype.GetMethod (method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
      if (m != null) {
        var gm = m.MakeGenericMethod (rtype);
        rtn = gm.Invoke (instance, args);
      }
      
      return rtn;
    }

    private void Load<T> (nDbRecordCb<DbTable<T>> cb)
    {
      var key = Key (typeof(T));

      /* Create an instance if one doesn't exist */
      _dbc.Exists (key, delegate (bool exists) {
        if (!exists) {
          var mdata = new DbTable<T> ();
          mdata.LastKey = 1;
          mdata.Records = new List<T> ();
          Save<T> (mdata, delegate (bool success) {
            cb.Invoke (mdata);
          });
        }
        
        /* otherwise load it */ else {
          _dbc.Get (key, delegate (string value) {
            var records = (DbTable<T>)InvokeGenericMethod (typeof(RecordSerializer), typeof(DbTable<T>), _rs, "Deserialize", value);
            cb.Invoke (records);
          });
        }
      });
    }

    private void Save<T> (DbTable<T> table, nDbRecordCb<bool> cb)
    {
      var key = Key (typeof(T));
      nLog.Debug ("Found records to save: " + table.Records.Count ());
      var block = (string)InvokeGenericMethod (typeof(RecordSerializer), typeof(DbTable<T>), _rs, "Serialize", table);
      nLog.Debug ("Block to write was: " + block);
      _dbc.Set (key, block, delegate {
        nLog.Debug ("Set callback was invoked");
        cb.Invoke (true);
       });
    }

    public void Count<T> (nDbRecordCb<int> cb) where T : nDbRecord
    {
      Load<T> (delegate (DbTable<T> all) {
        nLog.Debug ("Loaded all the records and found " + all.Records.Count());
        cb.Invoke (all.Records.Count ());
      });
    }

    public void Setup<T> (nDbRecordCb<bool> cb) where T : nDbRecord
    {
      cb.Invoke (true);
    }

    public void Insert<T> (T record, nDbRecordCb<bool> cb) where T : nDbRecord
    {
      if (record.Id == -1) {
        Load<T> (delegate (DbTable<T> all) {
          nLog.Debug ("Loaded all the records");
          record.Id = all.LastKey + 1;
          all.LastKey += 1;
          all.Records.Add (record);
          Save<T> (all, delegate (bool value) {
            nLog.Debug ("Saved all the records");
            cb.Invoke (true);
          });
        });
      } else {
        nLog.Debug ("Update???");
        Update<T> (record, cb);
      }
    }

    public void Update<T> (T record, nDbRecordCb<bool> cb) where T : nDbRecord
    {
      Load<T> (delegate (DbTable<T> all) {
        if (!all.Records.Contains (record)) 
          all.Records.Add (record);
        else {
          all.Records.Remove (record);
          all.Records.Add (record);
        }
        Save<T> (all, delegate (bool value) {
          cb.Invoke (true);
        });
      });
    }

    public void Delete<T> (T record, nDbRecordCb<bool> cb) where T : nDbRecord
    {
      Load<T> (delegate (DbTable<T> all) {
        if (all.Records.Contains (record)) {
          all.Records.Remove (record);
          Save<T> (all, delegate (bool value) {
            cb.Invoke (true);
          });
        } else
          cb.Invoke (true);
      });
    }

    public void Get<T> (long id, nDbRecordCb<T> cb) where T : nDbRecord
    {
      Load<T> (delegate (DbTable<T> all) {
        var items = from a in all.Records where a.Id == id select a;
        var rtn = items.FirstOrDefault ();
        cb.Invoke (rtn);
      });
    }

    public void All<T> (long offset, long limit, nDbRecordCb<IEnumerable<T>> cb) where T : nDbRecord
    {
      Load<T> (delegate (DbTable<T> all) {
        var rtn = (from a in all.Records select a).Skip ((int)offset).Take ((int)limit);
        cb.Invoke ((IEnumerable<T>)rtn);
      });
    }

    public void Clear<T> (nDbRecordCb<bool> cb) where T : nDbRecord
    {
      var mdata = new DbTable<T> ();
      mdata.LastKey = 1;
      mdata.Records = new List<T> ();
      Save<T> (mdata, delegate (bool value) {
        cb.Invoke (true);
      });
    }
  }
}

