using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
public class Globals : Game
{ 
    public static ContentManager content; 
    
    public static GraphicsDevice _graphicsDevice;
    public static SpriteBatch _spriteBatch; 
    public static GraphicsDeviceManager _graphics; 
    public static GameTime gameTime; 
    public static Random random = new Random(); 
    public static int selectedFileNumber,totalKills = 0; 
    public static bool isLoaded; 
    public static float totalTime = 0; 
} 
