using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipesV2.Impl
{
    class RecipeCollection: IRecipeCollection
    {
        readonly IKitchenContext _context;
        List<IRecipe> mesRecipes = new List<IRecipe>();

        internal RecipeCollection(KitchenContext context)
        {
            Debug.Assert(context != null);
            _context = context;
        }
        /// <summary>
        /// Gets the kitchen context to which this collection of ingredients belongs.
        /// </summary>
        public IKitchenContext Context { get { return _context; } }

        public int Count => mesRecipes.Count();

        /// <summary>
        /// Finds an existing <see cref="IRecipe"/> by ots name or creates a new one.
        /// </summary>
        /// <param name="name">Name of the recipe.</param>
        /// <param name="createIfNotFound">True to create a new recipe.</param>
        /// <returns>
        /// A new or existing recipe, or null if <paramref name="createIfNotFound"/> is 
        /// false and there is no such recipe.
        /// </returns>
        public IRecipe Find(string name, bool createIfNotFound = false)
        {
            if (name == null || name == "")
            {
                throw new ArgumentException("the recipe must have a name");
            }

            IRecipe rec = FindByName(name);
            if (createIfNotFound == true && rec == null)
            {
                IRecipe r = new Recipe(_context, name);
                mesRecipes.Add(r);
                return r;
            }
            else
            {
                return rec;
            }
        }


        /// <summary>
        /// Removes a recipe from this collection.
        /// This throws an <see cref="ArgumentException"/> if the recipe does not actually belong to this collection.
        /// </summary>
        /// <param name="r">The receipe to remove.</param>
        public void Remove(IRecipe r)
        {
            {
                int i;
                if (FindByName(r.Name) == null)
                {
                    throw new ArgumentException("the recipe does not actually belong to this collection.");
                }

                i = mesRecipes.IndexOf(r);
                if (i >= 0)
                {
                    mesRecipes.RemoveAt(i);
                }
                r = null;
            }
        }

        /// <summary>
        /// Finds a recipe by its name or null if not found.
        /// </summary>
        /// <param name="name">Name to find.</param>
        /// <returns>Null or the recipe with the given name.</returns>
        public IRecipe FindByName(string name)
        {
            if (name != null || name != "")
            {
                foreach (IRecipe elem in mesRecipes)
                {
                    if (elem.Name == name)
                    {
                        return elem;
                    }
                }
            }
            return null; ;
        }

        public IEnumerator<IRecipe> GetEnumerator()
        {
            return this.mesRecipes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
