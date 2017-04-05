using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipesV2
{

    /// <summary>
    /// A recipe contains ingredients, each of them being associated to a quantity: the <see cref="IngredientInRecipe"/> modelizes
    /// the occurence of an ingredient in a recipe.
    /// </summary>
    public interface IRecipe
    {
        /// <summary>
        /// Gets the name of the recipe.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the kitchen context to which this collection of ingredients belongs.
        /// This is null if this recipe has been removed from the kitchen's <see cref="KitchenContext.Recipes"/> collection.
        /// </summary>
        IKitchenContext Context { get; }

        /// <summary>
        /// Gets the number of ingredients in this recipe.
        /// </summary>
        int IngredientCount { get; }

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
        IIngredientInRecipe AddIngredient( IIngredient i, int quantity = 1 );

        /// <summary>
        /// Removes the given ingredient if it exists.
        /// </summary>
        /// <param name="i">The ingredient to remove.</param>
        /// <returns>True if the ingredient has been found and removed. False otherwise.</returns>
        bool RemoveIngredient( IIngredient i );

        /// <summary>
        /// Gets the <see cref="IngredientInRecipe"/> if it exists in this recipe, null otherwise.
        /// </summary>
        /// <param name="i">The ingredient.</param>
        /// <returns>The found ingredient or null.</returns>
        IIngredientInRecipe FindIngredient( IIngredient i );

        /// <summary>
        /// Gets the total cost of the recipe: this is the sum of the cost of the ingredients (with 
        /// their respective quantities).
        /// </summary>
        double Cost { get; }

    }
}
