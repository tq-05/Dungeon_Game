using Microsoft.Xna.Framework; 
using Microsoft.Xna.Framework.Graphics; 
using Microsoft.Xna.Framework.Input; 
using System.Xml.Linq; 
using System.IO; 
using System.Runtime.CompilerServices; 
using System.Runtime.InteropServices; 
using System; 
using System.Linq; 
using System.Collections.Generic; 
using System.Net.Mime; 
public class myMouse 
{ 
    public Vector2 mousePos; 
    public float mouseRotation; 
    public bool IsLeftMouseButtonPressed = false; 
    MouseState mouseState = new MouseState(); 
     
     
    public myMouse() 
    { 
    } 
    public virtual void Update() 
    { 
        mouseState = Mouse.GetState(); 
        // gets the state of the mouse every frame 
        mousePos = new Vector2(mouseState.X, mouseState.Y); 
         
        if (mouseState.LeftButton == ButtonState.Pressed) 
        { 
            IsLeftMouseButtonPressed = true; 
        } 
        else 
        { 
            IsLeftMouseButtonPressed = false; 
        } 
        // the IsLeftMouseButtonPressed becomes false after a new frame has been generated automatically 
        // this is to check if the user has clicked so an attack can be  performed. 
         
    } 

    public Vector2 getMousePos()
    {
        return mousePos;
    }
} 
 
