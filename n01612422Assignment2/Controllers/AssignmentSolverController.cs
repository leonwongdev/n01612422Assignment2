using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using WebGrease.Extensions;
using static System.Net.Mime.MediaTypeNames;

namespace n01612422Assignment2.Controllers
{
    public class AssignmentSolverController : ApiController
    {
        /// <summary>
        /// Adapted J1 - The New CCC (Canadian Calorie Counting)
        /// Original Source : https://cemc.math.uwaterloo.ca/contests/computing/2006/stage1/juniorEn.pdf
        /// </summary>
        /// <param name="burger">Integer representing the index burger choice</param>
        /// <param name="drink">Integer representing the index drink choice</param>
        /// <param name="side">Integer representing the index side choice</param>
        /// <param name="dessert">Integer representing the index dessert choice</param>
        /// <returns>Total calories calculated from the calories menu</returns>
        [HttpGet]
        [Route("api/J1/Menu/{burger}/{drink}/{side}/{dessert}")]
        public string solveJ1(int burger, int drink, int side, int dessert)
        {
            // Creating menu using dictionary, key will be option index, value will be calories.
            Dictionary<int, int> burgerCalories = new Dictionary<int, int>
            {
                { 1, 461 }, // Cheeseburger
                { 2, 431 }, // Fish Burger
                { 3, 420 }, // Veggie Burger
                { 4, 0 }    // No Burger
            };

            Dictionary<int, int> drinkCalories = new Dictionary<int, int>
            {
                { 1, 130 }, // Soft Drink
                { 2, 160 }, // Orange Juice
                { 3, 118 }, // Milk
                { 4, 0 }    // No Drink
            };

            Dictionary<int, int> sideOrderCalories = new Dictionary<int, int>
            {
                { 1, 100 }, // Fries
                { 2, 57 },  // Baked Potato
                { 3, 70 },  // Chef Salad
                { 4, 0 }    // No Side Order
            };
            Dictionary<int, int> dessertCalories = new Dictionary<int, int>
            {
                { 1, 167 }, // Apple Pie
                { 2, 266 }, // Sundae
                { 3, 75 },  // Fruit Cup
                { 4, 0 }    // No Dessert
            };


            int caloriesCount = burgerCalories[burger] + drinkCalories[drink] + sideOrderCalories[side] + dessertCalories[dessert];
            return "Your total calorie count is " + caloriesCount;
        }

        /// <summary>
        /// Adapted J2 - Roll the Dice
        /// Original Source : https://cemc.math.uwaterloo.ca/contests/computing/2006/stage1/juniorEn.pdf
        /// </summary>
        /// <param name="m">number of sides of the first dice</param>
        /// <param name="n">number of sides of the second dice</param>
        /// <returns>number of ways to get sum of 10</returns>
        [HttpGet]
        [Route("api/J2/DiceGame/{m}/{n}")]
        public string solveJ2(int m, int n)
        {
            // initialize dice value;
            // ignore side that is greater than 10 because adding it to any value will always greater than 10
            int[] dice1 = new int[m];
            for (int i = 0; i < m && i < 10; i++)
            {
                dice1[i] = i+1;
            }

            int[] dice2 = new int[n];
            for (int i = 0; i < n && i < 10; i++)
            {
                dice2[i] = i + 1;
            }
            int numOfWays = 0;
            for (int i = 0; i < dice1.Length; i++)
            {
                for (int j = 0; j < dice2.Length; j++)
                {
                    if (dice1[i] + dice2[j] == 10)
                    {
                        numOfWays++;
                    }
                }
            }
            return "There are " + numOfWays + " total ways to get sum 10.";
        }
        /// <summary>
        /// Original question: 2022 J3
        /// Source: https://cemc.math.uwaterloo.ca/contests/computing/past_ccc_contests/2022/ccc/juniorEF.pdf
        /// Modification made: In order to take input that has '+' and '-' sign, 
        /// I made this action to accept post request and take the instructions as plain text from the request body
        /// Sample curl command: curl -d "AFB+8SC-4H-2GDPE+9" http://localhost:50530/api/harptuning
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/HarpTuning")]
        
        public string[] solveJ3()
        {
            
            // read instructions as plain text from the request body
            string instructions = Request.Content.ReadAsStringAsync().Result;

            Debug.WriteLine(instructions);
                
            string instructionPattern = @"[A-Z]+[\+\-]\d+"; // regex for extracting each instruction

            MatchCollection matches = Regex.Matches(instructions, instructionPattern);

            // Loop thru each sub instruction e.g AFB+8
            List<string> result = new List<string>();
            foreach (Match match in matches)
            {
                
                string instruction = match.Value;
                string letter;
                string turns;
                
                if (instruction.Contains("+"))
                {
                    string[] instructionParts = instruction.Split('+');
                    letter = instructionParts[0];
                    turns = instructionParts[1];
                    result.Add(letter + " tighten " + turns);
                } else if (instruction.Contains("-"))
                {
                    string[] instructionParts = instruction.Split('-');
                    letter = instructionParts[0];
                    turns = instructionParts[1];
                    result.Add(letter + " losen " + turns);
                }
            }

            
            return result.ToArray();
        }
    }
}
