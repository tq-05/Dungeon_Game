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
public class Bullet : Attack 
{ 
    public bool done; 
    private Vector2 bulletPos, bulletDim; 
    float bulletRotation; 
     
     
    public Bullet(string MODEL, Vector2 POS , Vector2 DIM ,Vector2 TARGET) : base(MODEL, POS ,DIM, TARGET) 
    { 
        direction = TARGET - POS; 
        bulletDim = DIM; 
        bulletPos = POS; 
        done = false; 
        speed = 800f; 
        direction.Normalize(); 
        base.Update(); 
    } 
    public static Bullet shoot(string Model, Vector2 POS , Vector2 DIM,Vector2 TARGET) 
    { 
        return new Bullet(Model,  POS ,  DIM, TARGET); 
    } 
    public override void Update() 
    { 
 
        bulletPos += direction * speed * (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
        time += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
         
 
        if (time > 1) 
        { 
            done = true; 
            time = 0; 
        } 
        bulletRotation = (float)Math.Atan2(direction.X,-direction.Y); 
        // calculates the angle in radians. 
        base.Update(); 
 
    } 
    public override void Draw() 
    { 
        Globals._spriteBatch.Draw(texture, new Rectangle( (int)(bulletPos.X), (int)(bulletPos.Y), (int)(bulletDim.X), (int)(bulletDim.Y)),null,Color.White, bulletRotation , new Vector2(texture.Bounds.Width/2, texture.Bounds.Height/2), new SpriteEffects() , 0); 
        base.Draw(); 
    } 
    public Rectangle bulletBounds() 
    { 
        return new Rectangle((int)(bulletPos.X - bulletDim.X / 2), 
        (int)(bulletPos.Y - bulletDim.Y / 2),(int)(bulletDim.X),(int)(bulletDim.Y)); 
    } 
}