using System;
using n.Platform;

namespace n.Core
{
	public class nRand
	{
		private static Random _r = null;

		private static Random R {
      get {
        if (_r == null) 
          _r = new Random();
        return _r;
      }
		}

    /** Roughly return a value of value even rate guesses */
    public static bool Bool(bool value, int rate) {
      var variance = (int) Math.Floor(R.NextDouble() * (double) rate);
      var rtn = (variance % rate) == 0;
      return rtn;
    }

    public static float Float(float value, float variation) {
      var variance = (float) R.NextDouble() * variation * 2.0f;
      var rtn = value - variation + variance;
      return rtn;
    }

    public static float Float(float value, float up, float down) {
      if (down < 0)
        down = -down;
      if (up < 0)
        up = -up;
      var variance = (float) R.NextDouble() * (up + down) * 2.0f;
      var rtn = value - down + variance;
      return rtn;
    }

    public static int Int(int value, int up, int down) {
      if (down < 0)
        down = -down;
      if (up < 0)
        up = -up;
      var variance = (int) (R.NextDouble() * ((float) (up + down)) + 1f);
      var rtn = value - down + variance;
      return rtn;
    }

    public static int Int(int value, int variation) {
      var variance = (int) Math.Floor(R.NextDouble() * (double) variation * 2.0);
      var rtn = value - variation + variance;
      return rtn;
    }
	}
}

