using n.Gfx.Old;
using UnityEngine;

namespace n.UI
{
  /** A simple helper for buttons */
  public class nButton {

    private nProp _buttonDown;

    private nProp _buttonOver;

    private nProp _button;

    private nProp _text;

    private bool _visible = false;

    public string Texture { get; set; }

    public string OverTexture { get; set; }

    public string DownTexture { get; set; }

    public string Text { get; set; }

    public string Font { get; set; }

    public float FontSize { get; set; }

    public float Depth { get; set; }

    public UnityEngine.Vector2 Position { get; set; }

    public UnityEngine.Vector2 Size { get; set; }

    public nPropAction Action { get; set; }

    public Color Color { get; set; }

    public n.Gfx.nCamera Camera { get; set; }

    public void Destroy() {
      if (_visible) {
        _visible = false;
        _button.Visible = false;
        _button = null;
        _text.Visible = false;
        _text = null;
      }
    }


    public bool Hidden {
      set {
        if (value) {
          _button.Depth = _button.Depth - 10f;
          if (_buttonOver != null) _buttonOver.Depth = _buttonOver.Depth - 10f;
          if (_buttonDown != null) _buttonDown.Depth = _buttonDown.Depth - 10f;
          if (_text != null) _text.Depth = _text.Depth - 10f;
        }
        else {
          _button.Depth = _button.Depth + 10f;
          if (_buttonOver != null) _buttonOver.Depth = _buttonOver.Depth + 10f;
          if (_buttonDown != null) _buttonDown.Depth = _buttonDown.Depth + 10f;
          if (_text != null) _text.Depth = _text.Depth + 10f;
        }
      }
    }


    public void Manifest () {
      if (!_visible) {
        _visible = true;

        _button = new nProp (Texture, Size);
        _button.Visible = true;
        _button.Position = Position;
        _button.Depth = Depth + 1f;

        if (OverTexture != null) {
          _buttonOver = new nProp (OverTexture, Size);
          _buttonOver.Visible = true;
          _buttonOver.Position = Position;
          _buttonOver.Depth = Depth + 2f;
          _button.Listen(Camera, nInputEvent.ENTER, delegate {
            _button.Depth = Depth + 3;
            _buttonOver.Depth = Depth + 2f;
          });
          _buttonOver.Listen(Camera, nInputEvent.EXIT, delegate {
            _button.Depth = Depth + 1;
            _buttonOver.Depth = Depth + 3f;
          });
        }

        if (DownTexture != null) {
          _buttonDown = new nProp (DownTexture, Size);
          _buttonDown.Visible = true;
          _buttonDown.Position = Position;
          _buttonDown.Depth = Depth + 2f;
          _button.Listen(Camera, nInputEvent.DOWN, delegate {
            _buttonDown.Depth = Depth + 1f;
          });
          _buttonDown.Listen(Camera, nInputEvent.UP, delegate {
            _buttonDown.Depth = Depth + 4f;
          });
          _buttonDown.Listen(Camera, nInputEvent.EXIT, delegate {
            _buttonDown.Depth = Depth + 4f;
          });
        }

        if ((Text != null) && (Text != "")) {
          var text = new nText (Size);
          text.Text = Text;
          text.Font = (Font) Resources.Load(Font);
          text.FontSize = FontSize;
          text.Color = Color;
          _text = new nProp (text);
          _text.Visible = true;
          _text.Position = new UnityEngine.Vector2(Position[0] - Size[0] / 3.0f, Position[1] + Size[1] / 3.3f);
          _text.Depth = Depth + 0f;
        }

        _button.Listen(Camera, nInputEvent.UP, Action);
      }
    }
  }
}