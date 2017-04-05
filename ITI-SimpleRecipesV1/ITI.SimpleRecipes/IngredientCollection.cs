using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipes
{

    /// <summary>
    /// Holds a collection of <see cref="Ingredient"/>.
    /// One can only add new ingredients to this collection thanks to <see cref="CreateIngredient"/>.
    /// </summary>
    public class IngredientCollection
    {
        readonly KitchenContext _context;
        List<Ingredient> mesIngredients = new List<Ingredient>();
        /// <summary>
        /// This constructor may need parameters!!!
        /// </summary>
        internal IngredientCollection(KitchenContext context)
        {
            Debug.Assert(context != null);
            _context = context;
        }

        /// <summary>
        /// Gets the kitchen context to which this collection of ingredients belongs.
        /// </summary>
        public KitchenContext Context { get { return _context; } }

        /// <summary>
        /// Gets the number of ingredients in this collection.
        /// </summary>
        public int Count { get { return mesIngredients.Count; } }

        /// <summary>
        /// Creates a new ingredient in this collection.
        /// This throws an <see cref="ArgumentException"/> if the ingredient already exists with this name.
        /// </summary>
        /// <param name="name">Name of the new ingredient. This name must be unique otherwise an <see cref="ArgumentException"/> is thrown.</param>
        /// <returns>The newly created ingredient.</returns>
        public Ingredient CreateIngredient( string name )
        {
            if (name == null || name == "")
            {
                throw new ArgumentException("the ingredient must have a name ");
            }

            if (FindByName(name) != null)
            {
                throw new ArgumentException("the ingredient already exists with this name");
            }

            Ingredient ig = new Ingredient(name);
            mesIngredients.Add(ig);
            return ig;
        }

        /// <summary>
        /// Finds an ingredient by name. Returns null if it does not exist.
        /// </summary>
        /// <param name="name">Name of the ingredient to find.</param>
        /// <returns>The ingredient or null if not found.</returns>
        public Ingredient FindByName( string name )
        {
            if (name != null || name != "")
            {
                foreach (Ingredient elem in mesIngredients)
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
        public bool RemoveByName( string name )
        {
            int i;
            Ingredient ig = FindByName(name);
            if (ig != null)
            {
                i = mesIngredients.IndexOf(ig);
                if (i >= 0)
                {
                    mesIngredients.RemoveAt(i);

                    // cherche ingrédient à supprimer dans les autres recettes de la cuisine
                    //????///              
                    
                    return true;
                }
            }
            return false;
        }

    }
}
