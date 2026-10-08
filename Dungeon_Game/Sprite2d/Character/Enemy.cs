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

public class Enemy : Characters 
{ 
    #region 
    public Vector2 enemyPos,enemyDim,allyDirection,playerDirection; 
    private myFile file; 
    public string model; 
    private float enemySpeed; 
    private List<string> plan, actions; 
    protected Texture2D hitbox; 
    private List<Bullet> ammo; 
    private List<Slice> slices; 
    private Slice slice; 
    private Bullet bullet; 
    Dictionary<string,int> actionWeights; 
    private string goal; 
    private float hurtDuration, rageDuration, shootCD, swingCD,overlapDistance,minimumDistanceBetweenEnemies,distanceBetweenEnemies; 
    public bool isGuarding,checkedType,isdead,isAggresive,isPassive,isRanged,isMelee,isHurt,isEnraged; // STATE part of GOAP AI 
    public int enemyID, allyID; 
    #endregion 
    public Enemy(string Model, Vector2 POS, Vector2 DIM, int ENEMYNUMBER) : base(Model, new Vector2(200,200), new Vector2(200,200)) 
    { 
        loadModel(Model);
        file = new myFile(); 
        plan = new List<string>(); 
        actionWeights = new Dictionary<string, int>(); 
        ammo = new List<Bullet>(); 
        slices = new List<Slice>(); 
        hitbox = Globals.content.Load<Texture2D>("box"); 
        Slice slice = new Slice("Square",new Vector2(0,0), new Vector2(0,0),new Vector2(0,0)); 
        enemyID = ENEMYNUMBER; 
        model = Model; 
        enemyPos = POS; 
        enemyDim = DIM; 
        enemySpeed = speed*4; 
        isdead = false; 
        isGuarding = false; 
        checkedType = false; 
        getHealth(); 
    } 
    public override void Update() 
 { 
        if(Player.playerHP > 0) 
        { 
            //Console.WriteLine("UPDATING ENEMYID: " + enemyID + "\n I AM " + model); 
            getWorldData(); 
            defineGoals(); 
            assignWeightsToActions(); 
            Planner(); 
            updateAttacks(); 
            checkIfAttackHitPlayer(); 
            overlapCollisions(); 
            stayInZone(); 
            lockedIn(); 
        } 
        else 
        { 
            ammo = new List<Bullet>(); 
            slices = new List<Slice>(); 
        } 
 
    } 
    public override void Draw() 
    { 
        if (!isdead) 
        { 
            if (texture != null) 
            {  
                Globals._spriteBatch.Draw(texture, new Rectangle( (int)(enemyPos.X), (int)(enemyPos.Y),(int)(enemyDim.X),(int)(enemyDim.Y)),null,Color.White, 0.0f, new Vector2(texture.Bounds.Width/2,texture.Bounds.Height/2), new SpriteEffects() , 0); 
            } 
        } 
        foreach(var bullet in ammo) 
        { 
             bullet.Draw(); 
        } 
        foreach(var slice in slices) 
        { 
             slice.Draw(); 
        } 
    } 
    #region DATA OF THE ENEMY
    public Rectangle enemyBounds() // gets the Agent's sprite hitbox 
    { 
        return new Rectangle((int)(enemyPos.X - enemyDim.X / 2), (int)(enemyPos.Y - enemyDim.Y / 2),(int)(enemyDim.X),(int)(enemyDim.Y)); 
    } 
    public Rectangle getRange() // creates a box around the agent to detect other allies or the player 
    { 
        return new Rectangle((int)(enemyPos.X - (enemyDim.X * 6)/2), (int)(enemyPos.Y - (enemyDim.Y * 6)/2),(int)(enemyDim.X * 6),(int)(enemyDim.Y * 6)); 
    } 
    public void lockedIn() // prevents enemy from leaving the screen 
    { 
        if (enemyPos.X > Globals._graphics.PreferredBackBufferWidth - enemyDim.X / 2) 
        { 
        enemyPos.X = Globals._graphics.PreferredBackBufferWidth - enemyDim.X / 2; 
        } 
        else if (enemyPos.X <  enemyDim.X/ 2) 
        { 
        enemyPos.X = enemyDim.X / 2; 
        } 
        if (enemyPos.Y> Globals._graphics.PreferredBackBufferHeight - enemyDim.Y / 2) 
        { 
        enemyPos.Y = Globals._graphics.PreferredBackBufferHeight - enemyDim.Y / 2; 
        } 
        else if (enemyPos.Y < enemyDim.Y / 2) 
        { 
        enemyPos.Y = enemyDim.Y / 2; 
        } 
    }     
    public void guardZone(Enemy enemy) 
    { 
        Rectangle zone = enemy.getRange(); 
        if (enemyPos.X > zone.X + zone.Width - enemyDim.X / 2) 
        { 
            enemyPos.X = zone.X + zone.Width - enemyDim.X / 2; 
        } 
        else if (enemyPos.X < zone.X + enemyDim.X/ 2) 
        { 
            enemyPos.X = zone.X + enemyDim.X / 2; 
        } 
        if (enemyPos.Y> zone.Y + zone.Height - enemyDim.Y / 2) 
        { 
            enemyPos.Y = zone.Y + zone.Height - enemyDim.Y / 2; 
        } 
        else if (enemyPos.Y < zone.Y+ enemyDim.Y / 2) 
        { 
            enemyPos.Y = zone.Y + enemyDim.Y / 2; 
        } 
    } 
    public void stayInZone() 
    { 
        if(isGuarding) 
        { 
            foreach(var enemy in Player.enemies) 
            { 
                if (enemy.enemyID == allyID) 
                { 
                    guardZone(enemy); 
                } 
            } 
        } 
    } 
    #endregion 
    #region AGENT part of GOAP AI 
    public bool checkIfPlayerInRange() 
    { 
        if (getRange().Contains(Player.playerBounds()) || getRange().Intersects(Player.playerBounds())) 
        { 
            return true; 
        } 
        return false; 
    } 
    public void getHealth() // gets the health of the Agent 
    { 
        if (model == "meleeEnemy") 
        { 
            isMelee = true; 
            isRanged = false; 
            healthPoints = 3; 
            getMeleeType(); 
        } 
        else 
        { 
            isPassive = true;
            isAggresive = false; 
            isRanged = true; 
            isMelee = false; 
            healthPoints = 2; 
        } 
    } 
    public void getMeleeType() // checks if the agent is passive or aggressive 
    { 
        int chance = Globals.random.Next(1,11); 
        bool canBePassive = false; 
        if(Player.enemies.Count > 1) 
        { 
            foreach(var enemy in Player.enemies) 
            { 
                if (enemy.enemyID != enemyID && enemy.model == "rangedEnemy") 
                { 
                    canBePassive = true; 
                } 
            } 
             
        } 
        if (canBePassive && !checkedType) 
        { 
            if(chance <= 5) 
            { 
                isAggresive = false; 
                isPassive = true; 
            } 
            else 
            { 
                isAggresive = true; 
                isPassive = false; 
            }  
        } 
        else 
        { 
            isAggresive = true; 
            isPassive = false; 
        } 
    } 
    public void checkIfAlive() // checks if the agent is alive 
    { 
        if (healthPoints == 0) 
        { 
            isdead = true;
            } 
    } 
    public void isHit() // checks if the Agent has been hit 
    { 
        if(!isEnraged) 
        { 
            if(!isHurt) 
            { 
                hurtDuration += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
                foreach(var bullet in Player.playerAmmo) 
                { 
                    if (enemyBounds().Intersects(bullet.bulletBounds()) || enemyBounds().Contains(bullet.bulletBounds())) 
                    { 
                        isHurt = true; 
                    } 
                    else 
                    { 
                        isHurt = false; 
                    } 
                     
                } 
                foreach(var slice in Player.playerSlices) 
                { 
                    if (enemyBounds().Intersects(slice.sliceBounds()) || enemyBounds().Contains(slice.sliceBounds())) 
                    { 
                        isHurt = true; 
                    } 
                    else 
                    { 
                        isHurt = false; 
                    } 
                } 
            } 
            else 
            { 
                if(hurtDuration > 2) 
                { 
                    hurtDuration = 0; 
                    isHurt = false; 
                    isEnraged = true; 
                } 
            } 
 } 
        else 
        { 
            rageDuration += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
            if (rageDuration <3) 
            { 
                isHurt = false; 
                isEnraged = false; 
                rageDuration = 0; 
            } 
        } 
    } 
    public void getWorldData() // gets data which are used by the AI  
    { 
        checkIfAlive(); 
        if(isMelee) 
        { 
            getMeleeType(); 
            checkedType = true; 
        } 
        isHit(); 
    } 
    public void updateAttacks() // updates the attacks created by the agent 
    { 
        for(int numberOfSlices=slices.Count-1;numberOfSlices>= 0;numberOfSlices--) 
            { 
                slices[numberOfSlices].Update(); 
                if(slices[numberOfSlices].done) 
                { 
                    slices.Remove(slices[numberOfSlices]); 
                } 
            } 
        for(int numberOfBullets=ammo.Count-1;numberOfBullets>= 0;numberOfBullets--) 
        { 
            ammo[numberOfBullets].Update(); 
            if(ammo[numberOfBullets].done) 
            { 
                ammo.Remove(ammo[numberOfBullets]); 
            } 
        } 
} 
    #endregion  
    #region ACTIONS part of GOAP AI
     public bool GROUPUP() // gets a random ranged enemy to meet up with OR gets a random passive melee enemy to meet up with 
    { 
        if(isRanged) 
        { 
            for(int enemyIndex = Player.enemies.Count - 1; enemyIndex>=0;enemyIndex--) 
            { 
                if(enemyID != Player.enemies[enemyIndex].enemyID && Player.enemies[enemyIndex].model == "meleeEnemy" && Player.enemies[enemyIndex].isPassive) 
                { 
                    allyID = Player.enemies[enemyIndex].enemyID; 
                    getAllyDirectionFromAgent(Player.enemies[enemyIndex].enemyPos); 
                    if(!Player.enemies[enemyIndex].getRange().Contains(enemyBounds())) 
                    { 
                        enemyPos += allyDirection * enemySpeed * (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
                        return true; 
                    } 
                    return false; 
                } 
            } 
        } 
        if(isMelee) 
        { 
            for(int enemyIndex = Player.enemies.Count - 1; enemyIndex>=0;enemyIndex--) 
            { 
                if(enemyID != Player.enemies[enemyIndex].enemyID && Player.enemies[enemyIndex].model == "rangedEnemy") 
                { 
                    allyID = Player.enemies[enemyIndex].enemyID; 
                    getAllyDirectionFromAgent(Player.enemies[enemyIndex].enemyPos); 
                    if(!Player.enemies[enemyIndex].getRange().Contains(enemyBounds())) 
                    { 
                        enemyPos += allyDirection * enemySpeed * (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
                        return true; 
                    } 
                    else
                     { 
                        isGuarding = true; 
                    } 
                    return false; 
                } 
            } 
        } 
        return false; 
    } 
    public bool ATTACK() // instantiates a new instance of either a slice or bullet to be spawned in 
    { 
        if(isRanged) 
        { 
            shootCD += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
            float randomAttackCD = (float)Globals.random.Next(60,120)/100; 
            if (shootCD > randomAttackCD) 
            { 
                bullet = new Bullet("bullet", enemyPos, new Vector2(50,75) , Player.playerPos); 
                ammo.Add(bullet); 
                shootCD=0; 
            } 
        } 
        else 
        { 
            swingCD += (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
            if (swingCD > 0.3) 
            { 
                slice = new Slice("Slice", enemyPos, new Vector2(300,100), Player.playerPos); 
                slices.Add(slice); 
                swingCD = 0; 
            } 
        } 
        return false; 
    } 
    public bool APPROACH() // increments the enemies position towards the player position 
    { 
        //Console.WriteLine("ATTEMPTING TO APPROACH PLAYER"); 
        if(!getRange().Contains(Player.playerBounds()) || !getRange().Intersects(Player.playerBounds())) 
        { 
            getPlayerDirectionFromAgent(); 
            enemyPos += playerDirection * enemySpeed * (float)Globals.gameTime.ElapsedGameTime.TotalSeconds; 
            return true; 
        } 
        else 
        { 
            return false; 
        } 
    } 
    public void getPlayerDirectionFromAgent() // gets the direction of agent from the player 
    { 
        playerDirection = Player.playerPos - enemyPos; 
        playerDirection.Normalize(); 
    } 
    public void getAllyDirectionFromAgent(Vector2 allyPos) // gets the direction towards an ally 
    { 
        allyDirection = allyPos - enemyPos; 
        allyDirection.Normalize(); 
    } 
    public void getAgentDirectionFromAlly(Vector2 allyPos) // gets the direction from ally from ally 
    { 
        allyDirection = enemyPos - allyPos; 
        allyDirection.Normalize(); 
    } 
    public void overlapCollisions() // checks if there are any overlaps and pushes position back if overlap is detected 
    { 
        foreach(var enemy in Player.enemies) 
        { 
            if (enemy.enemyID != enemyID) 
            { 
                if(enemy.enemyBounds().Intersects(enemyBounds())) 
                { 
                    getAgentDirectionFromAlly(enemy.enemyPos); 
                    getOverlap(enemy); 
                    if(overlapDistance > 0) 
                    { 
                        enemyPos += overlapDistance * allyDirection; 
                    } 
                } 
            } 
        } 
 } 
    public void getOverlap(Enemy ally) // calculates the overlap amount 
    { 
        distanceBetweenEnemies = Vector2.Distance(enemyPos ,ally.enemyPos); 
        minimumDistanceBetweenEnemies = (enemyDim.X + ally.enemyDim.X)/2; 
        overlapDistance = minimumDistanceBetweenEnemies - distanceBetweenEnemies; 
    } 
    public void getAgentDirectionFromPlayer() // gets the direction of player to Agent 
    { 
        playerDirection = enemyPos - Player.playerPos; 
        playerDirection.Normalize(); 
    } 
    public bool PROTECT() // stays within an area around the chosen ally and moves towards to player in the zone while attacking if they are near 
    { 
        if(!getRange().Contains(Player.playerBounds()) || !getRange().Intersects(Player.playerBounds())) 
        { 
            foreach(var enemy in Player.enemies) 
            { 
                if (enemy.enemyID == allyID) 
                { 
                    isGuarding= true; 
                    if(APPROACH()) 
                    { 
                        ATTACK(); 
                    } 
                    else 
                    { 
                        return false; 
                    } 
                } 
            } 
            return false; 
        } 
        isGuarding = false; 
        return false; 
    } 
    public void checkIfAttackHitPlayer() // checks if an attack has hit the player 
    { 
        for(int numberOfBullets=ammo.Count-1;numberOfBullets>= 0;numberOfBullets--) 
        {
             if(ammo[numberOfBullets].bulletBounds().Contains(Player.playerBounds()) || ammo[numberOfBullets].bulletBounds().Intersects(Player.playerBounds())) 
            { 
                Player.playerHP--; 
                ammo.Remove(ammo[numberOfBullets]); 
            } 
        } 
        foreach (var slice in  slices) 
        { 
            if(slice.sliceBounds().Contains(Player.playerBounds()) || slice.sliceBounds().Intersects(Player.playerBounds())) 
            { 
                Player.playerHP--; 
            } 
        } 
    } 
    public void assignWeightsToActions() // assigns values by a dictionary to actions  
    { 
        if (goal == "survive") 
        { 
            actionWeights = new Dictionary<string,int> 
            { 
                {"GROUPUP" , 0},{"ATTACK" , 0},{"PROTECT" , 0},{"APPROACH" , 0} 
            }; 
        } 
        else if (goal == "kill") 
        { 
            if (isPassive) 
            { 
                if (isMelee) 
                { 
                    actionWeights = new Dictionary<string,int> 
                    { 
                        {"GROUPUP" , 2},{"ATTACK" , 0},{"PROTECT" , 1},{"APPROACH" , 0} 
                    }; 
                } 
                else // for ranged 
                { 
                    actionWeights = new Dictionary<string,int> 
                    { 
                        {"GROUPUP" , 2},{"ATTACK" , 1},{"PROTECT" , 0},{"APPROACH" , 0} 
                    };
                     } 
            } 
            else // melee 
            { 
                actionWeights = new Dictionary<string,int> 
                { 
                    {"GROUPUP" , 0},{"ATTACK" , 1},{"PROTECT" , 0},{"APPROACH" , 2} 
                };    
            } 
 
        } 
    } 
    #endregion 
    #region PLANNER part of GOAP AI 
    public void Planner() // creates a plan for the AI and executes it 
    { 
        actions = new List<string>{"ATTACK","PROTECT","GROUPUP","APPROACH"}; 
        if(plan != null) 
        { 
            if(plan.Count == 0) 
            { 
                for (int index = actions.Count - 1; index != 0; index--) // checks for unused actions (actions which return a 0) 
                { 
                    if(actionWeights[actions[index]] == 0) 
                    { 
                        actions.RemoveAt(index); 
                    } 
                } 
                if(actions.Count >1) 
                { 
                    plan = new List<string>(sortPlan(actions)); 
                } 
                else 
                { 
                    plan = actions; 
                } 
            } 
            else 
            { 
                executePlan(plan); 
            } 
             
        } 
 } 
    public List<string> sortPlan(List<string> availableActions) // bubble sort for plan priority queue 
    { 
        //Console.WriteLine("SORTING"); 
        List<string> oldAvailableActions = new List<string>(availableActions); 
        for(int currentAction = 0; currentAction < availableActions.Count; currentAction++ ) 
        { 
            if(currentAction+1 < availableActions.Count) 
            { 
                if(actionWeights[availableActions[currentAction]] < actionWeights[availableActions[currentAction+1]]) 
                { 
                    string temp = availableActions[currentAction]; 
                    availableActions[currentAction] = availableActions[currentAction+1]; 
                    availableActions[currentAction+1] = temp; 
                } 
            } 
        } 
        if(oldAvailableActions.SequenceEqual(availableActions)) 
        { 
            return availableActions; 
        } 
        else 
        { 
            availableActions = sortPlan(availableActions); 
            if(oldAvailableActions.SequenceEqual(availableActions)) 
            { 
                return availableActions; 
            } 
        } 
        return availableActions; 
     
    } 
    public void executePlan(List<string> plan) // executes the given plan as a queue structure 
    { 
        bool isActionStillTrue = false; 
        switch (plan[0]) 
        { 
            case "ATTACK": 
            //Console.WriteLine("executing ATTACK"); 
            isActionStillTrue = ATTACK();
             break; 
            case "PROTECT": 
            //Console.WriteLine("executing PROTECT"); 
            isActionStillTrue = PROTECT(); 
            break; 
            case "GROUPUP": 
            //Console.WriteLine("executing GROUPUP"); 
            isActionStillTrue = GROUPUP(); 
            break; 
            case "APPROACH": 
            //Console.WriteLine("executing APPROACH"); 
            isActionStillTrue = APPROACH(); 
            break; 
        } 
        if (!isActionStillTrue) 
        { 
            plan.RemoveAt(0); 
        } 
    } 
    #endregion 
    public void defineGoals() // GOALS part of GOAP AI 
    { 
        if(!isHurt) 
        { 
            goal = "kill"; 
        } 
        else if (isHurt && isAggresive) 
        { 
            goal = "survive"; 
        } 
    } 
 
} 