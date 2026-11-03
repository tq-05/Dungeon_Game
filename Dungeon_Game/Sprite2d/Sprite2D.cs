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
public class Sprite2d 
{ 
 
    #region  
    public Vector2 pos, dim; // allows dimensions and position to be recieved. 
    protected Texture2D texture; // allows the selected model to be loaded. 
    #endregion 
    public Sprite2d(string Model, Vector2 POS , Vector2 DIM) 
    { 
        pos=POS; // gets the models's position 
        dim=DIM; // gets the models's dimensions 
    } 
    public virtual void Update() 
    { 
    } 
    public virtual void Draw() 
    { 
        if (texture != null) 
        { // ensures that there is a value which can be loaded 
            Globals._spriteBatch.Draw(this.texture, new Rectangle( (int)(pos.X), (int)(pos.Y),(int)(dim.X),(int)(dim.Y)),null,Color.White, 0.0f, new Vector2(texture.Bounds.Width/2,texture.Bounds.Height/2), new SpriteEffects() , 0); 
        } 
    } 
    public Rectangle spriteBounds() 
    { 
        return new Rectangle((int)(pos.X - dim.X / 2), (int)(pos.Y - dim.Y / 2),(int)(dim.X),(int)(dim.Y)); 
    } 

    public virtual void loadModel(string Model)
    {
        texture = Globals.content.Load<Texture2D>(Model); //Loads the model which can be drawn later 
    }

    public virtual void loadModel(Texture2D texture)
    {
        this.texture = texture;
    }
 
     
} 