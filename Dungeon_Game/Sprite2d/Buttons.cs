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
     
 
    public Buttons(string type, Vector2 POS , Vector2 DIM) : base(type,POS,DIM) 
    { 
        buttonDim = DIM; 
        buttonPos = POS; 
        setButtonColour(type);
        loadModel(buttonTexture);
         
    } 

    public void setButtonColour(string type)
    {
        switch(type)
        {
            case "PLAY":
            buttonTexture = new Texture2D(Globals._graphicsDevice,1,1);
            buttonTexture.SetData(new[] {Color.Green});
            break;
            case "DELETE":
            buttonTexture = new Texture2D(Globals._graphicsDevice,1,1);
            buttonTexture.SetData(new[] {Color.Red});
            break;
            case "LOAD":
            buttonTexture = new Texture2D(Globals._graphicsDevice,1,1);
            buttonTexture.SetData(new[] {Color.White});
            break;
            

        }
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