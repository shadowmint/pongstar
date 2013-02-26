/**
 * Copyright 2012 Douglas Linder
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using System.Collections;
using n.Core;
using System.Collections.Generic;
using n.Gfx;
using System;
using Ps.Model.Events;
using Ps.Controllers;
using n.Utils;

namespace Ps.Model.Actions
{
  public class Game
  {
    private static bool _soundReady = false;

    private static AudioClip _bop;
    private static AudioClip _boop;
    private static AudioSource _playerAudio = null;
    private static AudioSource _aiAudio = null;
    private static AudioSource _wallAudio = null;

    /* Sound effect ids for stars */
    private const int COLLECT_STAR_CHANNEL = 1;
    private const int COLLECT_STAR_BING1 = 1;
    private const int COLLECT_STAR_BING2 = 2;
    private const int COLLECT_STAR_BING3 = 3;
    private const int COLLECT_STAR_BING4 = 4;

    /* Sound effect ids for win lose */
    public static int WIN_LOSE_CHANNEL = 2;
    public static int WIN_SOUND = 1;
    public static int LOSE_SOUND = 2;

    /* Music channel */
    public static int MUSIC_CHANNEL = 3;
    public static int MUSIC_0 = 1;

    /* Bounce channel */
    public static int BOUNCE_CHANNEL = 4;
    public static int WALL_BOUNCE = 1;
    public static int PLAYER_BOUNCE = 2;
    public static int AI_BOUNCE = 3;

    public static void SetupAudio(nAudio a) {
      if (!_soundReady) {
        _bop = (AudioClip)Resources.Load("audio/bop");
        _boop = (AudioClip)Resources.Load("audio/boop");
        _playerAudio = a.Source();
        _playerAudio.loop = false;
        _playerAudio.clip = _boop;
        _playerAudio.volume = 0.5f;
        _aiAudio = a.Source();
        _aiAudio.loop = false;
        _aiAudio.clip = _bop;
        _aiAudio.volume = 0.5f;
        _wallAudio = a.Source();
        _wallAudio.loop = false;
        _wallAudio.clip = _bop;
        _wallAudio.pitch = 1.5f;
        _wallAudio.volume = 0.5f;

        /* Setup channel for stars */
        a.Channel(COLLECT_STAR_CHANNEL, 6);
        a.Register(COLLECT_STAR_CHANNEL, COLLECT_STAR_BING1, "audio/star1");
        a.Register(COLLECT_STAR_CHANNEL, COLLECT_STAR_BING2, "audio/star2");
        a.Register(COLLECT_STAR_CHANNEL, COLLECT_STAR_BING3, "audio/star3");
        a.Register(COLLECT_STAR_CHANNEL, COLLECT_STAR_BING4, "audio/star4");

        /* Setup channel for win lose sounds */
        a.Channel(WIN_LOSE_CHANNEL, 1);
        a.Register(WIN_LOSE_CHANNEL, WIN_SOUND, "audio/win");
        a.Register(WIN_LOSE_CHANNEL, LOSE_SOUND, "audio/lose");

        /* Music */
        a.Channel(MUSIC_CHANNEL, 1);
        a.Register(MUSIC_CHANNEL, MUSIC_0, "audio/theme");
        a.Repeat(MUSIC_CHANNEL, MUSIC_0, 0.1f);

        _soundReady = true;
      }
    }

    /** Check if nothing is playing */
    private static bool NoBounce() {
      return !_playerAudio.isPlaying && !_aiAudio.isPlaying && !_wallAudio.isPlaying;
    }

    /** Reset sound state */
    public static void SoundReset() {
      _soundReady = false;
    }

    public static void EndGame(IEventData raw) {
      var data = (WallHit)raw;
      SetupAudio(data.State.Audio);
      if (data.Target == WallHitTarget.WALL_TOP) {
        ++data.State.Score.Player;
        data.State.NewGame();
        data.State.Flare.Show();
        raw.State.Score.Update(Config.PointsPerWin, "Won a round!");
        data.State.Audio.Play(WIN_LOSE_CHANNEL, WIN_SOUND, 1.0f);
        if (raw.State.Score.Player == Config.WinScore) 
          Pongstar.Get<GameController>().Win();
      } else if (data.Target == WallHitTarget.WALL_BOTTOM) {
        ++data.State.Score.Ai;
        data.State.NewGame();
        data.State.Flare.Show();
        raw.State.Score.Update(Config.PointsPerLoss, "Lost a round");
        data.State.Audio.Play(WIN_LOSE_CHANNEL, LOSE_SOUND, 1.0f);
        if (raw.State.Score.Ai == Config.WinScore) 
          Pongstar.Get<GameController>().Lose();
      } 
      else {
        raw.State.Score.Update(Config.PointsPerBounce, "Bounce!");
        if (_wallAudio == null) SetupAudio(data.State.Audio);
        if (NoBounce())
          _wallAudio.Play();
      }
    }

    public static void PlayerTouch(IEventData raw) {
      var data = (PaddleHit)raw;
      if (data.Target == data.State.PlayerPaddle) {
        raw.State.Score.Update(Config.PointsPerPaddleBounce, "Deflected!");
        if (_playerAudio == null) SetupAudio(data.State.Audio);
        if (NoBounce())
          _playerAudio.Play();
      } 
      else {
        if (_aiAudio == null) SetupAudio(data.State.Audio);
        if (NoBounce())
          _aiAudio.Play();
      }
    }

    public static void PlayerMove(IEventData raw) {
      var data = (PlayerInput)raw;
      if (data.Request == PlayerInputType.LEFT) {
        var delta = data.Distance < data.Target.Speed ? data.Distance : data.Target.Speed;
        data.Target.MoveLeft(delta);
      } 
      else if (data.Request == PlayerInputType.RIGHT) {
        var delta = data.Distance < data.Target.Speed ? data.Distance : data.Target.Speed;
        data.Target.MoveRight(delta);
      }
    }

    public static void UpdateScore(IEventData raw) {
      var data = (CollectableHit) raw;
      raw.State.Score.Update(data.Points, "Collected a star!");

      /* Play a random star noise~ */
      SetupAudio(data.State.Audio);
      var id = nRand.Int(1, 3, 0);
      data.State.Audio.Play(COLLECT_STAR_CHANNEL, id, 0.6f);
    }
  }
}
