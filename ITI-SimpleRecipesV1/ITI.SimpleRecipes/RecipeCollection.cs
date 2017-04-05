using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipes
{
    /// <summary>
    /// Holds a collection of recipes.
    /// </summary>
    public class RecipeCollection
    {
        readonly KitchenContext _context;
        List<Recipe> mesRecipes = new List<Recipe>();

        internal RecipeCollection( KitchenContext context )
        {
            Debug.Assert( context != null );
            _context = context;
        }

        /// <summary>
        /// Gets the kitchen context to which this collection of ingredients belongs.
        /// </summary>
        public KitchenContext Context { get { return _context; } }

        /// <summary>
        /// Gets the number of recipes in this collection.
        /// </summary>
        public int Count { get { return mesRecipes.Count; } }
        
        /// <summary>
        /// Creates a new named <see cref="Recipe"/> or find an exisitng one.
        /// </summary>
        /// <param name="name">Name of the recipe.</param>
        /// <returns>A new or existing recipe.</returns>
        public Recipe FindOrCreateRecipe( string name )
        {
            if (name == null || name == "")
            {
                throw new ArgumentException("the recipe must have a name");
            }

            Recipe rec = FindByName(name);
            if (rec == null)
            {
                Recipe r = new Recipe(_context, name);
                mesRecipes.Add(r);
                return r;
            }
            else
            {
                return rec;
            }
        }
 
        /// <summary>
        /// Finds a recipe by its name or null if not found.
        /// </summary>
        /// <param name="name">Name to find.</param>
        /// <returns>Null or the recipe with the given name.</returns>
        public Recipe FindByName( string name )
        {
            if (name != null || name != "")
            {
                foreach (Recipe elem in mesRecipes)
                {
                    if (elem.Name == name)
                    {
                        return elem;
                    }
                }
            }
            return null; ;
        }

        /// <summary>
        /// Removes a recipe from this collection.
        /// This throws an <see cref="ArgumentException"/> if the recipe does not actually belong to this collection.
        /// </summary>
        /// <param name="r">The receipe to remove.</param>
        public void Remove( Recipe r )
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
            r= null;  
        }

    }
}
