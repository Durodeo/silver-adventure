using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipes
{

    /// <summary>
    /// Root object that holds <see cref="Ingredients"/> and <see cref="Recipes"/>.
    /// </summary>
    public class KitchenContext
    {
        readonly IngredientCollection _ingredients;
        readonly RecipeCollection _recipes;

        /// <summary>
        /// Initializes a new empty <see cref="KitchenContext"/> instance.
        /// </summary>
        public KitchenContext()
        {
            _recipes = new RecipeCollection(this);
            _ingredients = new IngredientCollection(this);
        }

        /// <summary>
        /// Gets the collection of ingredients.
        /// </summary>
        public IngredientCollection Ingredients
        {
            get { return _ingredients;  }
        }

        /// <summary>
        /// Gets the collection of recipes.
        /// </summary>
        public RecipeCollection Recipes
        {
            get { return _recipes; }
        }
    }
}
