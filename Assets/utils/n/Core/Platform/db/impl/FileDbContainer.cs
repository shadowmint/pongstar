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

#if !UNITY_WEBPLAYER
namespace n.Platform.Db.Impl
{
  /** Saves items as files in a data folder */
  public class FileDbContainer : IDbContainer
  {
    private string Path (string key)
    {
      return @"/tmp/" + key + ".txt";
    }
  
    public void Set (string key, string value, DbContainerAction<bool> cb)
    {
      System.IO.File.WriteAllText(Path(key), value);
      cb.Invoke(true);
    }
      
    public void Get (string key, DbContainerAction<string> cb)
    {
      Exists (key, delegate (bool exists) {
        var value = exists ? File.ReadAllText(Path(key)) : "";
        cb.Invoke(value);
      });
    }
      
    public void Exists (string key, DbContainerAction<bool> cb)
    {
      var rtn = File.Exists (Path (key));
      cb.Invoke(rtn);
    }

    public void Clear (string key, DbContainerAction<bool> cb)
    {
      Exists (key, delegate (bool exists) {
        if (exists) 
          File.Delete(Path (key));
        cb.Invoke(true);
      });
    }
  }
}
#endif

