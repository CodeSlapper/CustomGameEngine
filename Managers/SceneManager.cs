﻿using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Scenes;
// NEW for Audio
using OpenTK.Audio;
namespace OpenGL_Game.Managers
{
    class SceneManager : GameWindow
    {
        Scene scene;
        public static int width = 800, height = 450;
        public static int windowXPos = 200, windowYPos = 200;

        public delegate void SceneDelegate(FrameEventArgs e);
        public SceneDelegate renderer;
        public SceneDelegate updater;

        public delegate void KeyboardDelegate(KeyboardKeyEventArgs e);
        public KeyboardDelegate keyboardDownDelegate;
        public KeyboardDelegate keyboardUpDelegate;

        public delegate void MouseDelegate(MouseButtonEventArgs e);
        public MouseDelegate mouseDelegate;

        AudioContext audioContext;
        public SceneManager() : base(width, height, new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(8, 8, 8, 8), 16))
        {
            this.X = windowXPos;
            this.Y = windowYPos;
            // NEW for Audio
            audioContext = new AudioContext();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Escape) Exit();
            if (keyboardDownDelegate != null) keyboardDownDelegate.Invoke(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (keyboardUpDelegate != null) keyboardUpDelegate.Invoke(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if(mouseDelegate != null) mouseDelegate.Invoke(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            //Load the GUI
            GUI.SetUpGUI(width, height);

            StartMenu();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            updater(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            renderer(e);

            GL.Flush();
            SwapBuffers();
        }

        public void StartNewGame()
        {
            if(scene != null) scene.Close();
            scene = new GameScene(this);
        }

        public void StartMenu()
        {
            //the if statement means whatever scene is open it gets closed
            if (scene != null) scene.Close();
            scene = new MainMenuScene(this);
        }
        //changing the game scene
        public void ChangeScene(SceneType Scene_Change)
        {
            if (scene != null) scene.Close();
            if (Scene_Change == SceneType.SCENE_GAME)
            {
                scene = new GameScene(this);
            }

            if (Scene_Change == SceneType.SCENE_MAIN_MENU)
            {
                scene = new MainMenuScene(this);
            }

            if (Scene_Change == SceneType.GAME_OVER)
            {
                scene = new GameOverScene(this);
            }
            if (Scene_Change == SceneType.KEYS_MENU)
            {
                scene = new KeysMenu(this);
            }

        }

        public static int WindowWidth
        {
            get { return width; }
        }

        public static int WindowHeight
        {
            get { return height; }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            SceneManager.width = Width;
            SceneManager.height = Height;

            //Load the GUI
            GUI.SetUpGUI(Width, Height);
        }
    }

}

