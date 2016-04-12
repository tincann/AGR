using System;
using System.Drawing;
using System.Threading;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using RayTracer.Helpers;

namespace RayTracer
{
    public class OpenTKApp : GameWindow
    {
        private static int _screenId;
        private static Game _game;
        private static CancellationTokenSource _exited = new CancellationTokenSource();

        protected override void OnLoad(EventArgs e)
        {
            // called upon app init
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            ClientSize = new Size(512, 512);
            //ClientSize = new Size(1024, 1024);
            _game = new Game();
            _game.Screen = new Surface(Width, Height);
            Sprite.Target = _game.Screen;
            _screenId = _game.Screen.GenTexture();
            _game.Init(this, _exited.Token);
        }

        protected override void OnUnload(EventArgs e)
        {
            // called upon app close
            GL.DeleteTextures(1, ref _screenId);
        }

        protected override void OnResize(EventArgs e)
        {
            // called upon window resize
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            _game.TraceRay(e.X, e.Y, new RNG());
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Pause)
            {
                lockKeys = !lockKeys;
                Console.WriteLine($"Keys locked: {lockKeys}");   
            }


            if (e.Key == Key.KeypadPlus)
            {
                _game.Antialiasing(1);
            }

            if (e.Key == Key.KeypadMinus)
            {
                _game.Antialiasing(-1);
            }

            if (e.Key == Key.Keypad7)
            {
                _game.GammaCorrection(true);
            }

            if (e.Key == Key.Keypad8)
            {
                _game.GammaCorrection(false);
            }
            base.OnKeyUp(e);
        }

        private bool lockKeys = false;
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            
            // called once per frame; app logic
            var keyboard = OpenTK.Input.Keyboard.GetState();
            if (keyboard[Key.Escape])
            {
                _exited.Cancel();
                Exit();
            }

            if (lockKeys)
            {
                return;
            }

            var moveVector = Vector3.Zero;
            if (keyboard[Key.A])
            {
                moveVector += -Vector3.UnitX;
            }else if (keyboard[Key.D])
            {
                moveVector += Vector3.UnitX;
            }
            if (keyboard[Key.W])
            {
                moveVector += Vector3.UnitZ;
            }else if (keyboard[Key.S])
            {
                moveVector += -Vector3.UnitZ;
            }

            if (keyboard[Key.Space])
            {
                moveVector += Vector3.UnitY;
            }else if (keyboard[Key.ControlLeft])
            {
                moveVector += -Vector3.UnitY;
            }

            if (moveVector != Vector3.Zero)
            {
                _game.MoveCamera(moveVector.Normalized());
            }

            var rotVector = Vector2.Zero;
            if (keyboard[Key.Left])
            {
                rotVector += -Vector2.UnitX;
            }else if (keyboard[Key.Right])
            {
                rotVector += Vector2.UnitX;
            }

            if (keyboard[Key.Up])
            {
                rotVector += Vector2.UnitY;
            }else if (keyboard[Key.Down])
            {
                rotVector += -Vector2.UnitY;
            }

            if (rotVector != Vector2.Zero)
            {
                _game.RotateCamera(rotVector);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // called once per frame; render
            _game.Tick();
            GL.BindTexture(TextureTarget.Texture2D, _screenId);
            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                _game.Screen.Width,
                _game.Screen.Height,
                0,
                PixelFormat.Bgra,
                PixelType.UnsignedByte,
                _game.Screen.Pixels
                );
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.BindTexture(TextureTarget.Texture2D, _screenId);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex2(-1.0f, -1.0f);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex2(1.0f, -1.0f);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex2(1.0f, 1.0f);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex2(-1.0f, 1.0f);
            GL.End();
            _game.Render();
            SwapBuffers();
        }

        [STAThread]
        public static void Main()
        {
            // entry point
            using (var app = new OpenTKApp())
            {
                app.Run(30.0, 0.0);
            }
        }
    }
}