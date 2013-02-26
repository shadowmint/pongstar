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

namespace n.Platform.Db
{
  /** Callback to indicate if the action was successful */
  public delegate void DbContainerAction<T>(T value);

  /** Stores a datablock in a location that can be read again */
  public interface IDbContainer
  {
    void Set(string key, string value, DbContainerAction<bool> cb);
    void Get(string key, DbContainerAction<string> cb);
    void Exists(string key, DbContainerAction<bool> cb);
    void Clear(string key, DbContainerAction<bool> cb);
  }
}

