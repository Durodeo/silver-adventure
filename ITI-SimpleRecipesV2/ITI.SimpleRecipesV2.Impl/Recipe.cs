using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipesV2.Impl
{
    class Recipe :IRecipe
    {

        IKitchenContext _context;
        double _cost;
        int _ingredientCount;
        String _name;

       List<IIngredientInRecipe> mesIngredients = new List<IIngredientInRecipe>();

        internal Recipe(IKitchenContext context, String name)
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
       public  string Name { get { return _name; } }

        /// <summary>
        /// Gets the kitchen context to which this collection of ingredients belongs.
        /// This is null if this recipe has been removed from the kitchen's <see cref="KitchenContext.Recipes"/> collection.
        /// </summary>
        public IKitchenContext Context
        {
            get
            {
                if (_context.Recipes.Find(_name) != null) { return _context; }
                else { return null; }
            }
        }

        /// <summary>
        /// Gets the number of ingredients in this recipe.
        /// </summary>
        public int IngredientCount
        {
            get
            {
                foreach (IIngredientInRecipe elem in mesIngredients)
                {
                    if (_context.Ingredients.FindByName(elem.Ingredient.Name) == null)
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
        public IIngredientInRecipe AddIngredient(IIngredient i, int quantity = 1)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("the ingredient quantity must be positive");
            }
            IIngredientInRecipe ig = FindIngredient(i);
            if (ig != null)
            {
                ig.Quantity += quantity;
            }
            else
            {
                _ingredientCount++;
                Ingredient ing = i as Ingredient;
                ig = new IngredientInRecipe(ing, this, quantity);
                mesIngredients.Add(ig);
            }
            return ig;
        }

        /// <summary>
        /// Removes the given ingredient if it exists.
        /// </summary>
        /// <param name="i">The ingredient to remove.</param>
        /// <returns>True if the ingredient has been found and removed. False otherwise.</returns>
        public bool RemoveIngredient(IIngredient i)
        {
            int ind;
            IIngredientInRecipe ig = FindIngredient(i);
            if (ig != null)
            {
                ind = mesIngredients.IndexOf(ig);
                if (ind >= 0)
                {
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
        public IIngredientInRecipe FindIngredient(IIngredient i)
        {
            foreach (IIngredientInRecipe elem in mesIngredients)
            {
                if (elem.Ingredient.Equals(i))
                {
                    return elem;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the total cost of the recipe: this is the sum of the cost of the ingredients (with 
        /// their respective quantities).
        /// </summary>
        public double Cost
        {
            get
            {
                double cout = 0;
                foreach (IIngredientInRecipe elem in mesIngredients)
                {
                    cout += (elem.Ingredient.UnitPrice * elem.Quantity);
                }
                return cout;
                //  return _cost;
            }
        }
    }
}
