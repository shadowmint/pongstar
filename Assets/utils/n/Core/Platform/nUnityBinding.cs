using System;
using n.Platform.Db;
using n.Platform.Db.Impl;
using n.Core;

namespace n.Platform
{
  public class nUnityBinding
	{
    public static void Bind(nResolver resolver) {
      resolver.Bind<nDispatcher, nUnityDispatcher>();
      resolver.Bind<nLogWriter, nUnityLogWriter>();
      #if UNITY_WEBPLAYER
        resolver.Bind<IDbContainer, CookieDbContainer>();
      #else
        resolver.Bind<IDbContainer, FileDbContainer>();
      #endif
    }
	}
}

