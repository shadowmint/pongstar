using UnityEngine;
using System.Collections;

namespace n.Test 
{
  /** Writes a test result set to file */
  public interface nTestWriter {
    void Write(string path, nTestSuite suite);
    void Write(string path, string message);
  }
}

