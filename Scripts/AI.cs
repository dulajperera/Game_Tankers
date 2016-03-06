using UnityEngine;
using System.Collections;
using Assets.Scripts.bean;
using System.Drawing;
using System.Threading;
using System;
using System.Linq;

public class AI{

    long[,] values=null;
    Game gameInsatance;

    public AI(Game game)
    {
        this.gameInsatance = game;
    }

   public void processCoins()
    {
      foreach(Coin c in this.gameInsatance.getCoins())
        {
            int i0 = c.Pos.X;
            int j0 = c.Pos.Y;

            for(int i=0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    long d1 = (long)Mathf.Pow((i - i0),2);
                    long d2 = (long)Mathf.Pow((j - j0),2);
                    long d3 = d1 + d2;
                    long d4 = (long)Mathf.Sqrt((float)d3);
                    long d5;
                    if (d4 == 0)
                    {
                        d5 = 10000000;
                    }
                    else
                    {
                        d5 = 10000000 / (5 * d4);
                        
                    }
                    if (values != null)
                    {
                        values[i, j] += d5;
                    }

                    //Debug.Log("d4 value " +d4);
                    //Debug.Log("d3 value" +d3);
                }
            }
            //Debug.Log("COin values: "+values[3,5]);
           

        }
      
    }


    public void processBricks()
    {
        foreach (BrickWall b in this.gameInsatance.getBricks())
        {
            int i = b.Pos.X;
            int j = b.Pos.Y;

            if (values != null)
            {
                values[i, j] = -1;
            }
           //Debug.Log("brick values :"+values[i,j]);
        }
        
    }

    public void processWater()
    {
        foreach (Point w in this.gameInsatance.getWater())
        {
            int i = w.X;
            int j = w.Y;

            if (values != null)
            {
                values[i, j] = -1;
            }
            //Debug.Log("water values :" + values[i, j]);
        }

    }

    public void processStones()
    {
        foreach (Point s in this.gameInsatance.getStones())
        {
            int i = s.X;
            int j = s.Y;

            if (values != null)
            {
                values[i, j] = -1;
            }
           // Debug.Log("stones values :" + values[i, j]);
        }

    }

    public void generateMap()
    {
        values = new long[10, 10];
        processCoins();
        processBricks();
        processStones();
        processWater();
        showValues();
    }


     public void showValues()
    {
        string s = "";
        for (int i=0;i<10;i++)
        {
            for (int j=0;j<10;j++)
            {
                s+= (values[i, j]-values[i,j] % 1) + " \t";
            }
            s+="\n";
        }
        Debug.Log(s);
    }

    public void goNextPosition(object stateInfo)
    {
        Debug.Log("ai is running..");
        while (!navigator.gameInstance.GameFinished) {
            Thread.Sleep(1200);
            if (!navigator.gameInstance.GameStarted)
                continue;

            generateMap();

            Debug.Log("Thinking..");

            Point pos = navigator.gameInstance.PlayerSelf.CurrentP;
            Debug.Log("bb");
            Point down = new Point(pos.X, (pos.Y + 1));
            Point up = new Point(pos.X, (pos.Y - 1));
            Point left = new Point((pos.X - 1), pos.Y);
            Point right = new Point((pos.X + 1), pos.Y);

            Debug.Log("a");
            long upValue = 0;
            long downValue = 0;
            long leftValue = 0;
            long rightValue = 0;

            if (up.X * up.Y >= 0 && up.X < 10 && up.Y < 10)
            {
                upValue = values[up.X, up.Y];
            }
            if (left.X * left.Y >= 0 && left.X < 10 && left.Y < 10)
            {
                leftValue = values[left.X, left.Y];
            }
            if (right.X * right.Y >= 0 && right.X < 10 && right.Y < 10)
            {
                rightValue = values[right.X, right.Y];
            }
            if (down.X * down.Y >= 0 && down.X < 10 && down.Y < 10)
            {
                downValue = values[down.X, down.Y];
            }

            Debug.Log("cc");

            long[] val= { upValue, downValue, leftValue, rightValue };
            long maxVal = val.Max();
            Debug.Log("u "+upValue+"\td "+ downValue+"\tl "+ leftValue+"\tr "+ rightValue+"\tmax "+ maxVal);

            if (upValue == maxVal)
            {
                debugCMD("UP#");
            }
            else if (downValue == maxVal)
            {
                debugCMD("DOWN#");
            }
            else if (leftValue == maxVal)
            {
                debugCMD("LEFT#");
            }
            else if (rightValue == maxVal)
            {
                debugCMD("RIGHT#");
            }
            else {
                Debug.Log("No point to move.");
            }
        }
    }

    public void debugCMD(string cmd)
    {
        if (navigator.gameInstance != null)
        {
            Debug.Log(cmd);
            navigator.gameInstance.GetClient.Send(cmd);
        }
        else
        {
            Debug.Log("Not send " + cmd);
        }

    }

}
