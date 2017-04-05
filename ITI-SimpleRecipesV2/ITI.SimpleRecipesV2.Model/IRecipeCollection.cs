using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipesV2
{
    /// <summary>
    /// Holds a collection of recipes.
    /// </summary>
    public interface IRecipeCollection : IReadOnlyCollection<IRecipe>
    {
        /// <summary>
        /// Gets the kitchen context to which this collection of ingredients belongs.
        /// </summary>
        IKitchenContext Context { get; }

        /// <summary>
        /// Finds an existing <see cref="IRecipe"/> by ots name or creates a new one.
        /// </summary>
        /// <param name="name">Name of the recipe.</param>
        /// <param name="createIfNotFound">True to create a new recipe.</param>
        /// <returns>
        /// A new or existing recipe, or null if <paramref name="createIfNotFound"/> is 
        /// false and there is no such recipe.
        /// </returns>
        IRecipe Find( string name, bool createIfNotFound = false );

        /// <summary>
        /// Removes a recipe from this collection.
        /// This throws an <see cref="ArgumentException"/> if the recipe does not actually belong to this collection.
        /// </summary>
        /// <param name="r">The receipe to remove.</param>
        void Remove( IRecipe r );

    }
}
