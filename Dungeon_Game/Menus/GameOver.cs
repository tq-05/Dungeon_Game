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
 
public class gameOverMenu 
{ 
    private myMouse mouse; 
    private Buttons button; 
    private Font2d font; 
    private List<Buttons> buttons; 
    private List<Font2d> text; 
    private myFile file; 
    public bool isMenu,finishButtonPressed; 
    private int score; 
     
 
    public gameOverMenu(myMouse MOUSE) 
    { 
        finishButtonPressed = false; 
        file = new myFile(); 
        text = new List<Font2d>(); 
        buttons = new List<Buttons>(); 
        mouse = MOUSE; 
        score = 0; 
        calculateScore(); 
        createGameOverMenu(); 
 
    } 
    public virtual void Update() 
    { 
        foreach (var button in buttons) 
        { 
           if(button.isClicked(mouse.mousePos,mouse.IsLeftMouseButtonPressed)) 
           { 
            finishButtonPressed = true; 
           } 
        } 
    } 
    public virtual void Draw() 
    { 
        foreach(var sentence in text) 
        { 
            sentence.Draw(); 
        } 
        foreach (var button in buttons) 
        { 
            button.Draw(); 
        } 
    } 
    public bool isFinishButtonPressed() 
    { 
        return finishButtonPressed; 
    } 
    public void createGameOverMenu() 
    { 
        font = new Font2d("mediumFont", new Vector2 ((float)(Globals._graphics.PreferredBackBufferWidth*0.41),(float)(Globals._graphics.PreferredBackBufferHeight*0.3)), "GAME OVER!" ); 
        text.Add(font); 
        font = new Font2d("gameOverFont", new Vector2 ((float)(Globals._graphics.PreferredBackBufferWidth*0.46),(float)(Globals._graphics.PreferredBackBufferHeight*0.4)), "Time: " + Globals.totalTime); 
        text.Add(font); 
        font = new Font2d("gameOverFont", new Vector2 ((float)(Globals._graphics.PreferredBackBufferWidth*0.46),(float)(Globals._graphics.PreferredBackBufferHeight*0.45)), "Kills: " + Globals.totalKills); 
        text.Add(font); 
        font = new Font2d("gameOverFont", new Vector2 ((float)(Globals._graphics.PreferredBackBufferWidth*0.46),(float)(Globals._graphics.PreferredBackBufferHeight*0.5)), "Score: " + score); 
        text.Add(font); 
        button = new Buttons("finishGame",new Vector2 ((float)(Globals._graphics.PreferredBackBufferWidth*0.5),(float)(Globals._graphics.PreferredBackBufferHeight*0.6)), new Vector2 (300,80)); 
        buttons.Add(button); 
    } 
    public void calculateScore() 
    { 
        if(Player.playerHP > 0 ) 
        { 
            if (Globals.totalTime <30) 
            { 
                score += 100; 
            } 
            else if (Globals.totalTime < 70) 
            { 
                score+= 50; 
            } 
            else if (Globals.totalTime <90) 
            { 
                score += 10; 
            } 
            else if (Globals.totalTime >999) 
            {
                 score += 0; 
            } 
            else 
            { 
                score += 5; 
            } 
        } 
        else 
        { 
            score+=0; 
        } 
         
        score += Globals.totalKills*2; 
        file.writeToFile("u",score.ToString()); 
    } 
} 