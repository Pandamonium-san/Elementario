using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

static class KeyMouseReader
{
	public static KeyboardState keyState, oldKeyState = Keyboard.GetState();
	public static MouseState mouseState, oldMouseState = Mouse.GetState();
    public static Point mousePos = new Point(-100, -100);
    public static Vector2 mousePosV2 = new Vector2(-100, -100);
    public static Point LeftClickPos = new Point(-100, -100);
    public static Point RightClickPos = new Point(-100, -100);

	public static bool KeyPressed(Keys key) {
		return keyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
	}
	public static bool LeftClick() {
		return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
	}
	public static bool RightClick() {
		return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
	}

	//Should be called at beginning of Update in Game
	public static void Update() {
		oldKeyState = keyState;
		keyState = Keyboard.GetState();
		oldMouseState = mouseState;
		mouseState = Mouse.GetState();
        mousePos = new Point((int)(KeyMouseReader.mouseState.X), (int)(KeyMouseReader.mouseState.Y));
        mousePosV2 = new Vector2(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y);

        LeftClickPos = new Point(-10000, -10000);         //Moves the mouseclick point outside the screen
        if (KeyMouseReader.LeftClick())
            LeftClickPos = mousePos;   //Creates point at mouse location for collision test
        RightClickPos = new Point(-10000, -10000);
        if (KeyMouseReader.RightClick())
            RightClickPos = mousePos;
        
	}
}