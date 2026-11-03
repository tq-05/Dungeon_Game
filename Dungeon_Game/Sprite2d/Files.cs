using Microsoft.Xna.Framework; 
using Microsoft.Xna.Framework.Graphics; 
using Microsoft.Xna.Framework.Input; 
using System.Xml.Linq; 
using System.IO; 
using System.Runtime.CompilerServices; 
using System.Runtime.InteropServices; 
using System; 
 
public class Files : Sprite2d 
{ 
    private Vector2 fileDim,filePos; 
    private Texture2D fileTexture;
     
 
    public Files(string type, Vector2 POS , Vector2 DIM) : base(type,POS,DIM) 
    { 
        fileDim = DIM; 
        filePos = POS; 
        setFileType(type);
        loadModel(fileTexture);
         
    } 

    public void setFileType(string type)
    {
        switch(type)
        {
            case "file":
            fileTexture = new Texture2D(Globals._graphicsDevice,1,1);
            fileTexture.SetData(new[] {Color.Gray});
            break;      
        }
    }
     
    public override void Draw() 
    { 
        if (texture != null) 
        { // ensures that there is a value which can be loaded 
            Globals._spriteBatch.Draw(this.texture, new Rectangle( (int)(filePos.X - fileDim.X / 2), (int)(filePos.Y - fileDim.Y / 2),(int)(fileDim.X),(int)(fileDim.Y)),null,Color.White, 0.0f, new Vector2(texture.Bounds.Width/2,texture.Bounds.Height/2), new SpriteEffects() , 1); 
        } 
 
    } 
}