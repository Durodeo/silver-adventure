using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipes
{

    /// <summary>
    /// A recipe contains ingredients, each of them being associated to a quantity: the <see cref="IngredientInRecipe"/> modelizes
    /// the occurence of an ingredient in a recipe.
    /// </summary>
    public class Recipe
    {
        KitchenContext _context;
        double _cost;
        int _ingredientCount;
        String _name;
        
        List<IngredientInRecipe> mesIngredients = new List<IngredientInRecipe>();

        /// <summary>
        /// This constructor may need parameters!!!
        /// </summary>
        internal Recipe(KitchenContext context, String name)
        {

            Debug.Assert(context != null);
            _context = context;
            _name = name;
            _cost = 0;
            _ingredientCount = 0;
        }

        /// <summary>
        /// Gets the name of the recipe.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets the kitchen context to which this collection of ingredients belongs.
        /// This is null if this recipe has been removed from the kitchen's <see cref="KitchenContext.Recipes"/> collection.
        /// </summary>
        public KitchenContext Context
        {
            get
            {
                if (_context.Recipes.FindByName(_name) != null) { return _context; }
                else { return null; }               
            }
        }

        /// <summary>
        /// Gets the number of ingredients in this recipe.
        /// </summary>
        public int IngredientCount
        { get
            {
                foreach (IngredientInRecipe elem in mesIngredients)
                {
                    if (_context.Ingredients.FindByName(elem.Ingredient.Name)== null)
                    {
                       // RemoveIngredient(elem.Ingredient);
                    }
                }



                return _ingredientCount;
            }
        }

        /// <summary>
        /// Adds an ingredient to the recipe with a quantity.
        /// If the ingredient already exists, its <see cref="IngredientInRecipe.Quantity"/> is updated.
        /// An <see cref="ArgumentException"/> is thrown if the added ingredient does not belong to the same 
        /// kitchen context as this recipe.
        /// If this recipe has been removed from its collection, an <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        /// <param name="i">The ingredient to add.</param>
        /// <param name="quantity">Optional quantity to add. Must be greater or equal to 1.</param>
        /// <returns>The <see cref="IngredientInRecipe"/> instance with an updated quantity.</returns>
        public IngredientInRecipe AddIngredient( Ingredient i, int quantity = 1 )
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("the ingredient quantity must be positive");
            }
            IngredientInRecipe ig = FindIngredient(i);
            if (ig != null)
            {
                ig.Quantity += quantity;
            }
            else
            {
                _ingredientCount++;

                ig = new IngredientInRecipe(i, this, quantity);
                mesIngredients.Add(ig);
            }

           /* double cout = 0;
            foreach (IngredientInRecipe elem in mesIngredients)
            {
                cout += elem.Ingredient.UnitPrice * elem.Quantity;
            }
            _cost = cout;*/

            return ig;
        }

        /// <summary>
        /// Removes the given ingredient if it exists.
        /// </summary>
        /// <param name="i">The ingredient to remove.</param>
        /// <returns>True if the ingredient has been found and removed. False otherwise.</returns>
        public bool RemoveIngredient( Ingredient i )
        {
            int ind;
            IngredientInRecipe ig = FindIngredient(i);
            if (ig != null)
            {
                ind = mesIngredients.IndexOf(ig);
                if (ind >= 0)
                {
                  //  ig.Quantity= ig.Quantity - 1;
                    
                    mesIngredients.RemoveAt(ind);
                    _ingredientCount--;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the <see cref="IngredientInRecipe"/> if it exists in this recipe, null otherwise.
        /// </summary>
        /// <param name="i">The ingredient.</param>
        /// <returns>The found ingredient or null.</returns>
        public IngredientInRecipe FindIngredient( Ingredient i )
        {
            foreach (IngredientInRecipe elem in mesIngredients)
            {
                if (elem.Ingredient.Equals(i))
                {
                    return elem;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the total cost of the recipe: this is the sum of the cost of the ingredients (with their respective quantities).
        /// </summary>
        public double Cost
        {
            get
            {
                   double cout = 0;
                   foreach (IngredientInRecipe elem in mesIngredients)
                   {
                       cout += (elem.Ingredient.UnitPrice * elem.Quantity);
                   }
                   return cout;
              //  return _cost;
            }
        }

    }
}
