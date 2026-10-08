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

public class Slice : Attack 
{ 
    public bool done; 
    public Vector2 slicePos, sliceDim; 
    float sliceRotation; 
     
     
    public Slice(string Model, Vector2 POS , Vector2 DIM ,Vector2 TARGET) : base(Model, POS ,DIM, TARGET) 
    { 
        direction = TARGET - POS; 
        sliceDim = DIM; 
        done = false; 
        speed = 300f; 
        direction.Normalize(); 
        slicePos = POS + direction * 100; 
    } 
    public static Slice swing(string Model, Vector2 POS , Vector2 DIM,Vector2 TARGET) 
    { 
        return new Slice(Model,  POS ,  DIM, TARGET); 
    } 
    public override void Update() 
    { 
 
        slicePos += direction * speed * (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
        time += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
         
 
        if (time > 0.5) 
        { 
            done = true; 
            time = 0; 
        } 
        sliceRotation = (float)Math.Atan2(direction.X,-direction.Y); 
        // calculates the angle in radians. 
 
    } 
    public override void Draw() 
    { 
        Globals._spriteBatch.Draw(texture, new Rectangle((int)(slicePos.X), (int)(slicePos.Y), (int)(sliceDim.X), (int)(sliceDim.Y)),null,Color.White, sliceRotation , new Vector2(texture.Bounds.Width/2, texture.Bounds.Height/2), new SpriteEffects() , 0);
        base.Draw(); 
    } 
    public Rectangle sliceBounds() 
    { 
        return new Rectangle((int)(slicePos.X - sliceDim.X / 2), (int)(slicePos.Y - sliceDim.Y / 2),(int)(sliceDim.X),(int)(sliceDim.Y)); 
    } 
     
} 
 