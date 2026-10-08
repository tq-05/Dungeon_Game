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
  public class weaponMenu 
{    
    private int numberOfButtons; 
    private string selectedWeapon; 
    private float drawcd; 
    private Sprite2d weaponUI; 
    private myMouse mouse; 
    private Buttons button; 
    private myFile file; 
    private List<Sprite2d> weaponInterfaces; 
    private List<Buttons> buttons; 
    public bool isDone; 
     
 
    public weaponMenu(myMouse MOUSE) 
    { 
        selectedWeapon = ""; 
        drawcd = 0; 
        numberOfButtons = 0; 
        file = new myFile(); 
        weaponInterfaces = new List<Sprite2d>(); 
        buttons = new List<Buttons>(); 
        mouse = MOUSE; 
        #region User Interface stuff 
        for (numberOfButtons = 0; numberOfButtons < 4; numberOfButtons++ ) 
        { 
            switch(numberOfButtons) 
            { 
                case 0: 
                button = new Buttons("select",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.4), (float)(Globals._graphics.PreferredBackBufferHeight*0.3)), new Vector2 (200,50)); 
                buttons.Add(button); 
                weaponUI = new Sprite2d("sword",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.4), (float)(Globals._graphics.PreferredBackBufferHeight*0.3)), new Vector2 (200,200)); 
                weaponInterfaces.Add(weaponUI); 
                Console.WriteLine("button one has been added to the list"); 
                break; 
                 
                case 1: 
                button = new Buttons("select",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.4),(float)(Globals._graphics.PreferredBackBufferHeight*0.6) ), new Vector2 (200,50)); 
                buttons.Add(button); 
                weaponUI = new Sprite2d("sword",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.4), (float)(Globals._graphics.PreferredBackBufferHeight*0.6)), new Vector2 (200,200)); 
                weaponInterfaces.Add(weaponUI); 
                Console.WriteLine("button two has been added to the list"); 
                break; 
 
                case 2: 
                button = new Buttons("select",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.6), (float)(Globals._graphics.PreferredBackBufferHeight*0.3)), new Vector2 (200,50)); 
                buttons.Add(button); 
                weaponUI = new Sprite2d("gun",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.6), (float)(Globals._graphics.PreferredBackBufferHeight*0.3)), new Vector2 (200,200)); 
                weaponInterfaces.Add(weaponUI); 
                Console.WriteLine("button three has been added to the list"); 
                break; 
 
                case 3: 
                button = new Buttons("select",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.6), (float)(Globals._graphics.PreferredBackBufferHeight*0.6)), new Vector2 (200,50)); 
                buttons.Add(button); 
                weaponUI = new Sprite2d("gun",new Vector2((float)(Globals._graphics.PreferredBackBufferWidth*0.6), (float)(Globals._graphics.PreferredBackBufferHeight*0.6)), new Vector2 (200,200)); 
                weaponInterfaces.Add(weaponUI); 
                Console.WriteLine("button four has been added to the list"); 
                break; 
                 
            } 
 
        } 
        #endregion 
 
    } 
    public virtual void Update() 
    { 
        drawcd += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
        selectWeaponMenu(); 
         
    } 
 
    public virtual void Draw() 
    {     
        drawInterface(); 
    } 
    public void selectWeaponMenu() 
    { 
        if (drawcd > 0.5) 
        { 
            if( mouse != null && buttons.Count >= 4) 
            { 
                if(buttons[0].isClicked(mouse.mousePos , mouse.IsLeftMouseButtonPressed)) 
                { 
                    Console.WriteLine("sword selected"); 
                    selectedWeapon = "Sword"; 
                    isDone = true; 
                    file.writeToFile(selectedWeapon, "0"); 
                    return; 
                } 
 
                if(buttons[1].isClicked(mouse.mousePos , mouse.IsLeftMouseButtonPressed)) 
                { 
                    Console.WriteLine("gsword selected"); 
                    selectedWeapon = "Gsword"; 
                    isDone = true; 
                    file.writeToFile(selectedWeapon, "0"); 
                    return; 
                } 
                 
                if(buttons[2].isClicked(mouse.mousePos , mouse.IsLeftMouseButtonPressed)) 
                { 
                    Console.WriteLine("gun selected"); 
                    selectedWeapon = "Gun"; 
                    isDone = true;  
                    file.writeToFile(selectedWeapon, "0"); 
                    return; 
                }
                  if(buttons[3].isClicked(mouse.mousePos , mouse.IsLeftMouseButtonPressed)) 
                { 
                    Console.WriteLine("rifle selected"); 
                    selectedWeapon = "Rifle"; 
                    isDone = true;  
                    file.writeToFile(selectedWeapon, "0"); 
                    return; 
                } 
                 
            } 
        } 
    } 
    public bool weaponSelected() 
    { 
        return isDone; 
    } 
    public void drawInterface() 
    { 
        foreach(var fileInterface in weaponInterfaces) 
        { 
            fileInterface.Draw(); 
        } 
         
        foreach(var button in buttons) 
        { 
            button.Draw(); 
        } 
    } 
 
} 