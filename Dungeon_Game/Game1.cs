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
 
namespace Dungeon_Game; 
 
public class Game1 : Game 
{ 
    WorldState world; 
    myMouse mouseState; 
    private GraphicsDeviceManager _graphics; 
 
    public Game1() 
    { 
        _graphics = new GraphicsDeviceManager(this); 
        Content.RootDirectory = "Content"; 
        Globals._graphics = _graphics; 
        Globals.content = Content; 
        IsMouseVisible = true; 
    } 
 
    protected override void Initialize() 
    { 
        // TODO: Add your initialization logic here 
         
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; 
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; 
        _graphics.IsFullScreen = true; 
        // window size adapts to current device resolution and is in full screen 
 
        _graphics.ApplyChanges();
        mouseState = new myMouse(); 
        world = new WorldState(mouseState); 
        base.Initialize(); 
    } 
 
    protected override void LoadContent() 
    { 
        Globals._spriteBatch = new SpriteBatch(GraphicsDevice); 
 
        // TODO: use this.Content to load your game content hered 
 
    } 
 
 
    protected override void Update(GameTime gameTime) 
    { 
 
        // TODO: Add your update logic here 
        Globals.gameTime = gameTime; 
        mouseState.Update(); 
        world.Update(); 
        if (world.exit) 
        { 
            Exit(); 
        } 
             
     
        // ensures the player does not go outside the window 
         
 
         
 
        base.Update(gameTime); 
    } 
 
 
    protected override void Draw(GameTime gameTime) 
    { 
        GraphicsDevice.Clear(Color.Black); 
        Globals._spriteBatch.Begin(); 
        world.Draw(); 
        Globals._spriteBatch.End(); 
        // TODO: Add your drawing code here
        base.Draw(gameTime); 
    } 
 
    static void Main(string[] args) 
    { 
        using var game = new Dungeon_Game.Game1(); 
        game.Run(); 
    } 
     
}