using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipesV2
{

    /// <summary>
    /// Holds a collection of <see cref="Ingredient"/>.
    /// One can only add new ingredients to this collection thanks to the <see cref="Create"/> method.
    /// </summary>
    public interface IIngredientCollection : IReadOnlyCollection<IIngredient>
    {
        /// <summary>
        /// Gets the kitchen context to which this collection of ingredients belongs.
        /// </summary>
        IKitchenContext Context { get; }

        /// <summary>
        /// Creates a new ingredient in this collection.
        /// This throws an <see cref="ArgumentException"/> if the ingredient already exists with this name.
        /// </summary>
        /// <param name="name">Name of the new ingredient. This name must be unique otherwise an <see cref="ArgumentException"/> is thrown.</param>
        /// <returns>The newly created ingredient.</returns>
        IIngredient Create( string name );

        /// <summary>
        /// Finds an ingredient by name. Returns null if it does not exist.
        /// </summary>
        /// <param name="name">Name of the ingredient to find.</param>
        /// <returns>The ingredient or null if not found.</returns>
        IIngredient FindByName( string name );

        /// <summary>
        /// Finds an ingredient by name and removes it if it exists.
        /// </summary>
        /// <param name="name">Name of the ingredient to find.</param>
        /// <returns>True if the ingredient was found and removed, false otherwise.</returns>
        bool Remove( string name );

    }
}
