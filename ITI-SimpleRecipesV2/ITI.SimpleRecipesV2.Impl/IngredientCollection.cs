using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipesV2.Impl
{
    class IngredientCollection: IIngredientCollection
    {
        readonly IKitchenContext _context;
        List<IIngredient> mesIngredients = new List<IIngredient>();
        internal IngredientCollection(IKitchenContext context)
        {
            Debug.Assert(context != null);
            _context = context;
        }
        /// <summary>
        /// Gets the kitchen context to which this collection of ingredients belongs.
        /// </summary>
        public IKitchenContext Context { get { return _context; } }

        public int Count
        {
            get
            {
                return mesIngredients.Count();
            }
        }

        /// <summary>
        /// Creates a new ingredient in this collection.
        /// This throws an <see cref="ArgumentException"/> if the ingredient already exists with this name.
        /// </summary>
        /// <param name="name">Name of the new ingredient. This name must be unique otherwise an <see cref="ArgumentException"/> is thrown.</param>
        /// <returns>The newly created ingredient.</returns>
        public IIngredient Create(string name)
        {
            if (name == null || name == "")
            {
                throw new ArgumentException("the ingredient must have a name ");
            }

            if (FindByName(name) != null)
            {
                throw new ArgumentException("the ingredient already exists with this name");
            }

            IIngredient ig = new Ingredient(name);
            mesIngredients.Add(ig);
            return ig;

        }

        /// <summary>
        /// Finds an ingredient by name. Returns null if it does not exist.
        /// </summary>
        /// <param name="name">Name of the ingredient to find.</param>
        /// <returns>The ingredient or null if not found.</returns>
        public IIngredient FindByName(string name)
        {
            if (name != null || name != "")
            {
                foreach (IIngredient elem in mesIngredients)
                {
                    if (elem.Name == name)
                    {
                        return elem;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Finds an ingredient by name and removes it if it exists.
        /// </summary>
        /// <param name="name">Name of the ingredient to find.</param>
        /// <returns>True if the ingredient was found and removed, false otherwise.</returns>
        public bool Remove(string name)
        {
            int i;
            IIngredient ig = FindByName(name);
            if (ig != null)
            {
                i = mesIngredients.IndexOf(ig);
                if (i >= 0)
                {
                    mesIngredients.RemoveAt(i);
                    // cherche ingrédient à supprimer dans les autres recettes de la cuisine
                    foreach (IRecipe listRecipes in _context.Recipes)
                    {
                        if (listRecipes.FindIngredient(ig) != null)
                        {
                            listRecipes.RemoveIngredient(ig);
                        }
                    }

                        return true;
                }
            }
            return false;
        }

        public IEnumerator<IIngredient> GetEnumerator()
        {
            return this.mesIngredients.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
