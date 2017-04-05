using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipes
{

    /// <summary>
    /// Defines an ingredient in a recipe with a given <see cref="Quantity"/>.
    /// </summary>
    public class IngredientInRecipe
    {
        Ingredient _ingredient;
        Recipe _recipe;
        int _quantity;
        /// <summary>
        /// This constructor may need parameters!!!
        /// </summary>
       internal IngredientInRecipe(Ingredient ingredient, Recipe recipe,int quantity)
        {
            _ingredient = ingredient;
            _recipe = recipe;
            _quantity = quantity;
        }

        /// <summary>
        /// Gets the ingredient.
        /// This property is never null, even when the ingredient is no more in the <see cref="Recipe"/>.
        /// </summary>
        public Ingredient Ingredient
        {
            get { return _ingredient; }
        }

        /// <summary>
        /// Gets the <see cref="Recipe"/> to which the <see cref="Ingredient"/> belongs.
        /// It is null, when this ingredient is no more in the recipe.
        /// </summary>
        public Recipe Recipe
        {
            get
            {
                if (_recipe.FindIngredient(_ingredient) != null)
                {
                    return _recipe;
                }
                else return null;
            }
        }

        /// <summary>
        /// Gets or sets the quantity of the ingredient in the <see cref="Recipe"/>.
        /// Both getter and setter throw an <see cref="InvalidOperationException"/> if this <see cref="IngredientInRecipe"/>
        /// has been removed from its recipe.
        /// </summary>
        public int Quantity
        {
            get
            {
               if(_recipe.FindIngredient(_ingredient) != null)

                {
                    return _quantity;
                }
				else
				{
                    throw new InvalidOperationException("The ingredient has been removed from its recipe");
                }
            }
            set
            {
                if (_recipe.FindIngredient(_ingredient) != null)
                {
                    if (value <= 0)
                    {
                        throw new ArgumentException("The ingredient quantity must be positive");
                    }
                    _quantity = value;
                }
                else
                {
                    throw new InvalidOperationException("The ingredient has been removed from its recipe");
                }
            }
        }
    }
}
