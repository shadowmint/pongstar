using System;
using System.Collections.Generic;
using UnityEngine;

namespace n.Utils
{
  public class nAudio
  {
    private List<AudioSource> _known = new List<AudioSource>();

    private GameObject _parent;

    /** Set of channels */
    private IDictionary<int, nAudioChannel> _channels = new Dictionary<int, nAudioChannel>();

    public nAudio(GameObject origin) {
      _parent = origin;
    }

    public AudioSource Source ()
    {
      AudioSource rtn = null;
      _parent.AddComponent<AudioSource> ();
      var items = _parent.GetComponents<AudioSource> ();
      foreach (var i in items) {
        if (!_known.Contains(i)) {
          _known.Add(i);
          rtn = i;
          break;
        }
      }

      return rtn;
    }

    /** Create a channel with a set of workers */
    public void Channel(int id, int workers) {
      if (_channels.ContainsKey(id)) 
        throw new Exception("Duplicate channel id: " + id);
      _channels[id] = new nAudioChannel(this, workers);
    }

    /** Register a sound asset to an ID */
    public void Register(int channelId, int resourceId, string resourcePath) {
      var channel = _channels[channelId];
      if (channel.Resources.ContainsKey(resourceId))
        throw new Exception("Duplicate resource id: " + resourceId);
      var asset = (AudioClip) Resources.Load(resourcePath);
      if (asset == null)
        throw new Exception("Invalid resource path: " + resourcePath);
      channel.Resources[resourceId] = asset;
    }

    /** Reload channels; eg. You've changed scene and want to load audio again */
    public void Reload() {
      var keys = _channels.Keys;
      foreach (var k in keys) {
        _channels[k].Reload(this);
      }
    }

    /** Play a sound on a given channel; if there are no free workers, this request is ignored. */
    public void Play(int channelId, int resourceId, float volume) {
      var channel = _channels [channelId];
      var clip = channel.Resources[resourceId];
      var player = channel.Next();
      if (player != null) {
        player.clip = clip;
        player.loop = false;
        player.volume = volume;
        player.Play();
      }
    }

    /** Play a sound on a given channel forever */
    public void Repeat(int channelId, int resourceId, float volume) {
      var channel = _channels [channelId];
      var clip = channel.Resources[resourceId];
      var player = channel.Next();
      if (player != null) {
        player.clip = clip;
        player.loop = true;
        player.volume = volume;
        player.Play();
      }
    }

    /** Check if a given channel is ready */
    public bool ChannelExists(int id) {
      return _channels.ContainsKey(id);
    }
  }

  /** Channel type */
  internal class nAudioChannel {

    /** Loaded assets */
    public IDictionary<int, AudioClip> Resources = new Dictionary<int, AudioClip>();

    /** Audio players */
    private AudioSource[] _workers;

    /** Reload using a new audio parent */
    public void Reload(nAudio parent) {
      for (var i = 0; i < _workers.Length; ++i) {
        _workers[i] = parent.Source();
      }
    }

    /** Create with worker count */
    public nAudioChannel(nAudio parent, int workers) {
      _workers = new AudioSource[workers];
      Reload(parent);
    }

    /** Get next free worker or null */
    public AudioSource Next() {
      AudioSource rtn = null;
      for (var i = 0; i < _workers.Length; ++i) {
        if (!_workers [i].isPlaying) {
          rtn = _workers [i];
          break;
        }
      }
      return rtn;
    }
  }
}

