using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipesV2
{

    /// <summary>
    /// Defines an ingredient in a recipe with a given <see cref="Quantity"/>.
    /// </summary>
    public interface IIngredientInRecipe
    {
        /// <summary>
        /// Gets the ingredient.
        /// This property is never null, even when the ingredient is no more in the <see cref="IRecipe"/>.
        /// </summary>
        IIngredient Ingredient { get; }

        /// <summary>
        /// Gets the <see cref="IRecipe"/> to which the <see cref="IIngredient"/> belongs.
        /// It is null, when this ingredient is no more in the recipe.
        /// </summary>
        IRecipe Recipe { get; }

        /// <summary>
        /// Gets or sets the quantity of the ingredient in the <see cref="IRecipe"/>.
        /// Both getter and setter throw an <see cref="InvalidOperationException"/> if this <see cref="IIngredientInRecipe"/>
        /// has been removed from its recipe.
        /// </summary>
        int Quantity { get; set; }

    }
}
