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

public class Font2d 
{ 
 
    public Vector2 pos;  
    private string sentence; 
    protected SpriteFont font;  
    public Font2d(string Model, Vector2 POS , string SENTENCE) 
    { 
        pos=POS;  
        sentence=SENTENCE;  
        font = Globals.content.Load<SpriteFont>(Model);  
 
    } 
    public virtual void Update() 
    { 
 
    } 
    public virtual void Draw() 
    { 
        if (font != null) 
        { // ensures that there is a value which can be loaded 
            Globals._spriteBatch.DrawString(font, sentence ,pos, Color.White); 
        } 
    } 
 
     
}