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
public class Attack : Sprite2d 
{ 
    protected float speed, time, count; 
    public Vector2 direction; 
    public Characters Origin; 
    public Player player; 
     
    public Attack(string Model, Vector2 POS , Vector2 DIM, Vector2 TARGET) : 
base(Model,POS, DIM) 
    { 
        speed = 1f; 
    } 
    public override void Update() 
    { 
    } 
    public override void Draw() 
    { 
    } 
} 