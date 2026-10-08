using Microsoft.Xna.Framework; 
using Microsoft.Xna.Framework.Graphics; 
using Microsoft.Xna.Framework.Input; 
using System.Xml.Linq; 
using System.IO; 
using System.Runtime.CompilerServices; 
using System.Runtime.InteropServices; 
using System; 
 
public class Buttons : Sprite2d 
{ 
    private Vector2 buttonDim,buttonPos; 
    private Texture2D buttonTexture;
     
 
    public Buttons(string Model, Vector2 POS , Vector2 DIM) : base(Model,POS,DIM) 
    { 
        buttonDim = DIM; 
        buttonPos = POS; 
        buttonTexture = new Texture2D(Globals._graphicsDevice,1,1);
        buttonTexture.SetData(new[] {Color.Green});
        loadModel(buttonTexture);
         
    } 
     
    public bool isClicked(Vector2 mousePos, bool mouseClicked) 
    { 
        return buttonBounds().Contains(mousePos) && mouseClicked; 
    } 
    public Rectangle buttonBounds() 
    { 
        return new Rectangle((int)(buttonPos.X - buttonDim.X / 2), (int)(buttonPos.Y - buttonDim.Y / 2),(int)(buttonDim.X),(int)(buttonDim.Y)); 
    } 
 
    public override void Draw() 
    { 
        if (texture != null) 
        { // ensures that there is a value which can be loaded 
            Globals._spriteBatch.Draw(this.texture, new Rectangle( (int)(buttonPos.X - buttonDim.X / 2), (int)(buttonPos.Y - buttonDim.Y / 2),(int)(buttonDim.X),(int)(buttonDim.Y)),null,Color.White, 0.0f, new Vector2(texture.Bounds.Width/2,texture.Bounds.Height/2), new SpriteEffects() , 1); 
        } 
 
    } 
}