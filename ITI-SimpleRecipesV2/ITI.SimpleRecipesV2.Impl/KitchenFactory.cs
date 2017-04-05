using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ITI.SimpleRecipesV2
{
    /// <summary>
    /// Entry point to obtain root <see cref="IKitchenContext"/> objects.
    /// </summary>
    public static class KitchenFactory
    {
        /// <summary>
        /// Creates a new, empty, <see cref="IKitchenContext"/> object.
        /// </summary>
        /// <returns>A new kitchen context.</returns>
        public static IKitchenContext Create()
        {            
            return new ITI.SimpleRecipesV2.Impl.KitchenContext();
        }

        /// <summary>
        /// Loads a <see cref="IKitchenContext"/> from a stream, previously saved thanks 
        /// to <see cref="IKitchenContext.Save(Stream)"/>.
        /// </summary>
        /// <param name="s">The stream to read from.</param>
        /// <returns>A kitchen context.</returns>
        public static IKitchenContext LoadFrom( Stream s )
        {
            IKitchenContext cLoad = KitchenFactory.Create();
            String collectionBase = "";
            String ElementBase = "";
            String nomIngredient = "";
            String valueIngredient = "";
            String unIngredient = "";
            string nomRecette = "";
            String CoutRecette = "";
            String NbIngredient = "";
            List<string> listIngred = new List<string>();
            bool inTheRecipe = false;
            List<String> KitchenIng = new List<string>();
            List<String> kitchenRecipe = new List<String>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = false;
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(s, settings);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                         switch (reader.Name)
                         {
                            case "KITCHEN":
                                break;
                            case "INGREDIENTS":
                                collectionBase = reader.Name;
                                break;
                            case "RECIPES":
                                collectionBase = reader.Name;
                               break;
                            case "Ingredient": // initialisation ingredient //
                                unIngredient = "";
                                nomIngredient = "";
                                valueIngredient = "";
                                break;
                            case "Recipe": // initialisation recette //
                                nomRecette = "";
                                CoutRecette = "";
                                NbIngredient = "";
                                break;
                            case "name":
                                ElementBase = reader.Name;
                                break;
                            case "UnitPrice":
                                ElementBase = reader.Name;
                                break;
                            case "cost":
                                ElementBase = reader.Name;
                                break;
                            case "ingredientCount":
                                ElementBase = reader.Name;
                                break;
                            case "ingredientsInRecipe":
                                inTheRecipe = true;
                                break;
                            case "Quantity":
                                ElementBase = reader.Name;
                                break;
                         }
                       break;

                    case XmlNodeType.Text:
                        if (collectionBase == "INGREDIENTS")// traitement des ingrédients//
                        {
                            if (ElementBase == "name") //recuperation du nom de l'ingredient 
                            {
                                nomIngredient = reader.Value;
                            }
                            else if (ElementBase == "UnitPrice")// recuperation du prix de l'ingredient
                            {
                                valueIngredient = reader.Value;
                            }

                        }
                        else if (collectionBase == "RECIPES")// traitement des recettes//
                        {
                            if (inTheRecipe == false)
                            {
                                if (ElementBase == "name")// recuperation du nom de la recette
                                {
                                    nomRecette = reader.Value;
                                }
                                else if (ElementBase == "cost")// recuperation du cout de la recette
                                {
                                    CoutRecette = reader.Value;
                                }
                                else if (ElementBase == "ingredientCount")// recuperation du nombre d'ingredient contenue dans la recette
                                {
                                    NbIngredient = reader.Value;
                                }
                            }
                            else // traitement des ingredient dans la recette
                            {
                                if (ElementBase == "name") // recuperation du nom de l'ingredient
                                {
                                    nomIngredient = reader.Value;
                                }
                                else if (ElementBase == "Quantity")// recuperation du nombre d'ingrédient dans la recette 
                                {
                                    valueIngredient = reader.Value;
                                }
                            }

                        }
                        break;
                    case XmlNodeType.XmlDeclaration:
                    case XmlNodeType.ProcessingInstruction:
                        break;
                    case XmlNodeType.Comment:
                        break;
                    case XmlNodeType.EndElement:
                        if (collectionBase == "INGREDIENTS") //enregistrement des ingredients 
                        {
                            switch (reader.Name)
                            {
                                case "Ingredient":
                                    IIngredient ingred= cLoad.Ingredients.Create(nomIngredient);
                                    ingred.UnitPrice = Convert.ToDouble(valueIngredient);
                                    break;
                            }
                        }
                        else if (collectionBase == "RECIPES")//enregistrement des recettes
                        {
                            switch (reader.Name)
                            {
                                case "Recipe":
                                    IRecipe recip = cLoad.Recipes.Find(nomRecette, true);
                                    foreach (string res in listIngred)
                                    {
                                        string[] elem = res.Split(',');
                                        recip.AddIngredient(cLoad.Ingredients.FindByName(elem[0]),Convert.ToInt32(elem[1]));
                                    }
                                    listIngred.Clear();
                                    break;
                                case "ingredientsInRecipe":
                                    inTheRecipe = false;
                                    
                                    break;
                                case "Ingredient":
                                    unIngredient = nomIngredient + "," + valueIngredient;
                                    listIngred.Add(unIngredient);
                                    break;
                            }
                        }
                        ElementBase = "";
                        break;
                }
            }
            return cLoad;
        }
    }
}
