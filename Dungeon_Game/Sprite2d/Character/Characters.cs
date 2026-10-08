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
public class Characters : Sprite2d 
{ 
    protected float speed; 
    public int healthPoints; 
 
    public Characters(string Model, Vector2 POS , Vector2 DIM) : base(Model,new Vector2(Globals._graphics.PreferredBackBufferWidth / 2, Globals._graphics.PreferredBackBufferHeight / 2), new Vector2 (200,200)) 
    { 
        healthPoints = 5; 
        speed = 40f; 
    } 
    public override void Update() 
    { 
 
    } 
    public override void Draw() 
    { 
 
    } 
} 