using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipesV2
{

    /// <summary>
    /// Root object that holds <see cref="Ingredients"/> and <see cref="Recipes"/>.
    /// </summary>
    public interface IKitchenContext
    {
        /// <summary>
        /// Gets the collection of ingredients.
        /// </summary>
        IIngredientCollection Ingredients { get; }

        /// <summary>
        /// Gets the collection of recipes.
        /// </summary>
        IRecipeCollection Recipes { get; }

        /// <summary>
        /// Saves this kitchen context with all its ingredients and recipes into a stream.
        /// The format is not specified, it can be any format, provided that the kitchen factory
        /// id able to reload it.
        /// </summary>
        /// <param name="s">The stream to save to.</param>
        void Save( Stream s );

        /// <summary>
        /// Imports ingredients from a text file that respects the following format:
        /// - An ingredient per line 
        /// - Each line starts with an integer (an identifier) that is ignored by this method.
        /// - The ingredient's name is after the identifier and a white space.
        /// - The unit price appears between brackets at the end of the line in cents.
        /// </summary>
        /// <param name="filePath">Full path of the file to import.</param>
        /// <remarks>
        /// Existing ingredients are preserved and duplicates are ignored.
        /// </remarks>
        void ImportIngredientsOnly( string filePath );

        /// <summary>
        /// Imports ingredients and recipes from two text files that respect the following format:
        /// - Ingredient file follows the same format as the one described in <see cref="ImportIngredientsOnly"/>.
        /// - The recipe file:
        ///    - A recipe is expressed on one line.
        ///    - Each line starts with the title followed by its ingredients.
        ///    - Ingredients are enclosed between brackets, identifiers and quantities appear between parentheses:
        ///         The recipe name [(id:99999;Quantity:100),(id:77777;Quantity:12000)]
        /// </summary>
        /// <param name="ingredientFilePath">Full path of the ingredient file to import.</param>
        /// <param name="recipeFilePath">Full path of the recipe file to import.</param>
        /// <remarks>
        /// Existing ingredients and/or recipes are preserved and duplicates are ignored.
        /// When importing recipes, any recipe that uses an ingredient that does not exist should be ignored.
        /// A contrario, a 0 quantity should be automatically corrected to 1. 
        /// </remarks>
        void ImportRecipes( string ingredientFilePath, string recipeFilePath );
    }
}
