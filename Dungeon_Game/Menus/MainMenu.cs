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
 public class mainMenu 
{ 
    private myMouse mouse; 
    private Buttons button; 
    private Font2d font; 
    private List<Buttons> buttons; 
    private List<Font2d> text; 
    public bool isMenu; 
     
 
    public mainMenu(myMouse MOUSE) 
    { 
         text = new List<Font2d>(); 
         buttons = new List<Buttons>(); 
         mouse = MOUSE; 
 
    } 
    public virtual void Update() 
    { 
        if( mouse != null && buttons.Count > 0) 
        { 
            if(buttons[0].isClicked(mouse.mousePos , mouse.IsLeftMouseButtonPressed)) 
            { 
                isMenu = true;  
                buttons.RemoveAt(0); 
            } 
             
        } 
        else 
        { 
            button = new Buttons("PLAY",new Vector2(Globals._graphics.PreferredBackBufferWidth / 2, Globals._graphics.PreferredBackBufferHeight / 2), new Vector2 (400,100)); 
            buttons.Add(button); 
            font = new Font2d("bigFont", new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.5),(float)(Globals._graphics.PreferredBackBufferHeight *0.3)), "DUNGEON GAME"); 
            text.Add(font);
            font = new Font2d("mediumFont", new Vector2(Globals._graphics.PreferredBackBufferWidth / 2, Globals._graphics.PreferredBackBufferHeight / 2), "PLAY"); 
            text.Add(font);  
        } 
         
 
    } 
    public bool menuOn() 
    { 
        return isMenu; 
    } 
    public virtual void Draw() 
    { 
        if(buttons.Count > 0) 
        { 
            buttons[0].Draw(); 
        } 
        if (text.Count > 0) 
        { 
            for(int i = 0; i < text.Count; i++)
            {
                text[i].Draw(); 
            }
        } 
         
    } 
}