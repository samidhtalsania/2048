using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;
using System.Security.Permissions;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        //array that stores the values displayed in the grid. Empty spaces are 0.
        int[,] num = new int[4, 4];

        Label[,] labelArray = new Label[4, 4];
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
           
            //Generate 16 labels 
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Label text = new Label();
                    text.Height = 75;
                    text.Width = 75;
                    text.BackColor = System.Drawing.Color.Snow;
                    text.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                    text.TabStop = false;
                    text.TabIndex = 0;
                    text.AutoSize = false;
                    text.AutoEllipsis = false;
                    text.Location = new System.Drawing.Point(j * 80 + 5, i * 80 + 5);
                    
                    
                    text.TextAlign = ContentAlignment.MiddleCenter;

                    if ((i + j) % 2 == 0)
                    {
                        text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(89)))), ((int)(((byte)(152)))));
                    }
                    else 
                    {
                        text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(157)))), ((int)(((byte)(195)))));
                    }

                    text.ForeColor = System.Drawing.Color.WhiteSmoke;
                    
                    this.Controls.Add(text);
                    labelArray[i, j] = text;
                }
            }

            //randomly fill 4 empty spaces with either 1 or 2
            Random rand = new Random();
            for (int i = 0; i < 4; i++)
            {
                int x = rand.Next(4);
                int y = rand.Next(4);

                num[x, y] = rand.Next(1, 3) * 2;
                labelArray[x, y].Text = num[x, y].ToString();
            }

        }

    

        /// <summary>
        /// Handle Key Down event. This function is trigerred by all key downs on keyboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            string previousState = string.Empty;
            string currentState = string.Empty;

            //determine state before keypress
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    previousState += num[i, j];
                }
            }
            //Handle up down left right key presses
            switch (e.KeyCode.ToString()) 
            {
                case "Up":
                    MoveUp();
                    break;
                case "Down":
                    MoveDown();
                    break;
                case "Left":
                    MoveLeft();
                    break;
                case "Right":
                    MoveRight();
                    break;
                default:
                    break;
            }

            //determine state after keypress
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    currentState += num[i, j];
                }
            }

            //Check if the user can continue
            if (checkGameState())
            {
                DialogResult dr = MessageBox.Show("No More Tiles Left", "Game Over", MessageBoxButtons.OK);
                if (dr.Equals(DialogResult.OK))
                {
                    Application.Restart();
                }
            }

            //Dont fill empty spaces if the move didnt result in any changes
            if(previousState != currentState)
                fillEmpty();
            
            //Display the grid
            reDisplay();
        }

        /// <summary>
        /// Check if the user can continue with the game or not.
        /// This method checks if there are any transaction that the user can do or if there are any empty spaces in the grid.
        /// </summary>
        /// <returns>bool</returns>
        private bool checkGameState()
        {
            

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (num[j, i] == num[ j + 1 , i])
                    {
                        return false;
                    }
                    else if (num[j, i] == 0 || num[j + 1, i] == 0)
                    {
                        return false;
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (num[i, j] == num[i, j + 1])
                    {
                        return false;
                    }
                    else if (num[j, i] == 0 || num[j + 1, i] == 0)
                    {
                        return false;
                    }
                }
            }


            return true;

            
        }

        /// <summary>
        /// Randomly fill the empty space in the grid with either 2 or 4.
        /// Either fills 2 spaces or 1 space.
        /// </summary>
        private void fillEmpty()
        {
            int count = 0;
            Random rand = new Random();
            int limit = rand.Next(1,3);
            for (int i = 0; i < 16; i++)
            {
                int x = rand.Next(0, 4);
                int y = rand.Next(0, 4);
                if (num[x,y] == 0)
                {
                    num[x,y] = rand.Next(1, 3) * 2;
                    count++;
                }

                if (count == limit)
                    break;
            }
        }

        /// <summary>
        /// Handle right button key press
        /// </summary>
        private void MoveRight()
        {
            for (int i = 0; i < 4; i++)
            {
                int[] arr = Merge(num[i, 0], num[i, 1], num[i, 2], num[i, 3]);
                int count = 3;
                for (int j = 3; j >= 0 ; j--)
                {
                    if (arr[j] != 0)
                    {
                        num[i, count] = arr[j];
                        count--;
                    }
                }

                for (int k = count; k >= 0; k--)
                {
                    num[i, k] = 0;
                }
            }
            
        }

        /// <summary>
        /// method that redisplays the grid 
        /// </summary>
        private void reDisplay()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (num[i, j] != 0)
                    {
                        labelArray[i, j].Text = num[i, j].ToString();
                    }
                    else 
                    {
                        labelArray[i, j].Text = string.Empty;
                    }
                    
                }
            }
        }

        /// <summary>
        /// Handle left button key press
        /// </summary>
        private void MoveLeft()
        {
            for (int i = 0; i < 4; i++)
            {
                
                int[] arr = Merge(num[i, 3], num[i, 2], num[i, 1], num[i, 0]);
                int count = 0;
                for (int j = 3; j >= 0; j--)
                {
                    if (arr[j] != 0)
                    {
                        num[i, count] = arr[j];
                        count++;
                    }
                }

                for (int k = count; k < 4; k++)
                {
                    num[i, k] = 0;
                }

               
            }
           
        }

        //p1->p2->p3->p4
        //0422
        //0220
        //
        private int[] Merge(int p1, int p2, int p3, int p4)
        {
            int[] ans = new int[4]{p1,p2,p3,p4};

            for (int i = 3; i > 0 ; i--)
            {
                if (ans[i] == 0)
                {
                    continue;
                }
                else
                {
                    int temp = isAvail(i, ans);
                    if (temp >= 0)
                    {
                        ans[i] *= 2;
                        ans[temp] = 0;
                    }
                }
            }
            return ans;
        }

        private int isAvail(int i, int[] ans)
        {
            int temp = ans[i];

            for (int k = i - 1; k >= 0; k--)
            {
                if (ans[k] == 0)
                    continue;
                else if (ans[k] == temp)
                    return k;
                else
                    return -1;
            }
            return -1;
        }


       
        /// <summary>
        /// Handle down button key press
        /// </summary>
        private void MoveDown()
        {
            for (int i = 0; i < 4; i++)
            {
                int[] arr = Merge(num[0,i],num[1,i],num[2,i],num[3,i]);
                //4020
                //display as 0042
                //just need to remove 0s
                int[] temp = new int[4];
                int count = 3 ;
                for (int j = 3; j >= 0; j--)
                {
                    if (arr[j] != 0)
                    {
                        temp[count--] = arr[j];
                    }
                }

                count = 3;
                for (int j = 3; j >= 0; j--)
                {
                    num[j, i] = temp[count--];
                }
            }
        }

        /// <summary>
        /// Handle up button key press
        /// </summary>
        private void MoveUp()
        {
            for (int i = 0; i < 4; i++)
            {
                int[] arr = Merge(num[0, i], num[1, i], num[2, i], num[3, i]);
                //4020
                //want 4200
                int[] temp = new int[4];
                int count = 0;
                for (int j = 0; j < arr.Length ; j++)
                {
                    if (arr[j] != 0)
                    {
                        temp[count++] = arr[j];
                    }
                }

                for (int j = 0; j < temp.Length; j++)
                {
                    num[j, i] = temp[j];
                }


            }
        }




    }



}
