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
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using System.IO;
using System.Reflection;

namespace n.Platform.Db
{
  public class RecordSerializer
  {
    /** Serialize an object of type t */
    public string Serialize<T> (T record)
    {
      MethodInfo item = Generic ("SerializeXml", typeof(T));
      if (item != null) {
        var xml = (string)item.Invoke (this, new object[] { record });
        var block = Encrypt(xml);
        return block;
      }
      else
        return null;
    }

    /** Deserialize into an object of type t */
    public T Deserialize<T> (string record)
    {
      var xml = Decrypt(record);
      MethodInfo item = Generic ("DeserializeXml", typeof(T));
      if (item != null) {
        var rtn = (T)item.Invoke (this, new object[] { xml });
        return rtn;
      } 
      else 
        return default(T);
    }

    /** Fetch a method and magic it into the appropriate generic type */
    private MethodInfo Generic(string name, Type t) {
      MethodInfo rtn = null;
      var methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
      foreach (var m in methods) {
        if (m.Name == name) {
          rtn = m;
          break;
        }
      }
      if (rtn != null)
        rtn = rtn.MakeGenericMethod(new Type[] { t });
      return rtn;
    }

    private string Encrypt (string raw)
    {
      var c = new Crypto();
      var rtn = c.Encrypt(raw);
      return rtn;
    }

    private string Decrypt (string raw)
    {
      var c = new Crypto();
      var rtn = c.Decrypt(raw);
      return rtn;
      
    }

    private string SerializeXml<T>(T value) {
      if(value == null) {
        return null;
      }
      
      XmlSerializer serializer = new XmlSerializer(typeof(T));
      
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
      settings.Indent = false;
      settings.OmitXmlDeclaration = false;
      
      using(StringWriter textWriter = new StringWriter()) {
        using(XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings)) {
          serializer.Serialize(xmlWriter, value);
        }
        return textWriter.ToString();
      }
    }
    
    private T DeserializeXml<T>(string xml) {
      if(string.IsNullOrEmpty(xml)) {
        return default(T);
      }
      
      XmlSerializer serializer = new XmlSerializer(typeof(T));
      
      XmlReaderSettings settings = new XmlReaderSettings();

      using(StringReader textReader = new StringReader(xml)) {
        using(XmlReader xmlReader = XmlReader.Create(textReader, settings)) {
          return (T) serializer.Deserialize(xmlReader);
        }
      }
    }
  }
}

