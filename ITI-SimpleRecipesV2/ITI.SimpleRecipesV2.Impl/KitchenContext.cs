using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
namespace ITI.SimpleRecipesV2.Impl
{
    class KitchenContext :IKitchenContext
    {
        readonly IIngredientCollection _ingredients;
        readonly IRecipeCollection _recipes;

        public KitchenContext()
        {
            _recipes = new RecipeCollection(this);
            _ingredients = new IngredientCollection(this);
        }

        /// <summary>
        /// Gets the collection of ingredients.
        /// </summary>
        public IIngredientCollection Ingredients { get { return _ingredients; } }

        /// <summary>
        /// Gets the collection of recipes.
        /// </summary>
        public IRecipeCollection Recipes { get { return _recipes; } }

        /// <summary>
        /// Saves this kitchen context with all its ingredients and recipes into a stream.
        /// The format is not specified, it can be any format, provided that the kitchen factory
        /// id able to reload it.
        /// </summary>
        /// <param name="s">The stream to save to.</param>
        public void Save(Stream s)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.CloseOutput = true;
                      
            XmlWriter writer = XmlWriter.Create(s, settings);
            writer.WriteStartElement("KITCHEN");

            writer.WriteStartElement("INGREDIENTS");
            
            foreach (IIngredient listIngredients in _ingredients)
            {                
                writer.WriteStartElement("Ingredient");
                writer.WriteElementString("name", listIngredients.Name);
                writer.WriteElementString("UnitPrice", listIngredients.UnitPrice.ToString("G17"));
             //   writer.WriteElementString("UnitPrice", ((decimal)listIngredients.UnitPrice).ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();///INGREDIENTS
            writer.WriteStartElement("RECIPES");
            foreach (IRecipe listRecipes in _recipes)
            {
                writer.WriteStartElement("Recipe");
                writer.WriteElementString("name", listRecipes.Name);
                writer.WriteElementString("cost", listRecipes.Cost.ToString());
                writer.WriteElementString("ingredientCount", listRecipes.IngredientCount.ToString());

                writer.WriteStartElement("ingredientsInRecipe");
                foreach (IIngredient listIngredients in _ingredients)
                {
                    IIngredientInRecipe moningredient = listRecipes.FindIngredient(listIngredients);
                    if (moningredient != null)
                    {
                        writer.WriteStartElement("Ingredient");
                        writer.WriteElementString("name", moningredient.Ingredient.Name);
                        writer.WriteElementString("Quantity", moningredient.Quantity.ToString());
                        writer.WriteEndElement();///Ingredient
                    }
                }
                writer.WriteEndElement();///ingredients in recipe
                writer.WriteEndElement();///Recipe
            }      
            writer.WriteEndElement();///RECIPES
            writer.WriteEndElement();///KITCHEN
            writer.Flush();            
        }

        /// <summary>
        /// Imports ingredients from a text file that respects the following format:
        /// - An ingredient per line 
        /// - Each line starts with an integer (an identifier) that is ignored by this method.
        /// - The ingredient's name is after the identifier and a white space.
        /// - The unit price appears between brackets at the end of the line in cents.
        /// </summary>
        /// <param name="filePath">Full path of the file to import.</param>
        /// <remarks>
        /// Existing ingredients are preserved and duplicates are ignored.
        /// </remarks>
        public void ImportIngredientsOnly(string filePath)
        {
            string ligne;
            StreamReader fic = new StreamReader(filePath);

            while ((ligne =fic.ReadLine()) != null)
            {
                string myString = ligne.Substring(5);
                string[] elemIng = myString.Split('[');
                double price = Convert.ToDouble(elemIng[1].Trim(']'));
                price = price / 100;
                if (_ingredients.FindByName(elemIng[0].TrimEnd(' '))==null)
                {
                    IIngredient ing = _ingredients.Create(elemIng[0].TrimEnd(' '));
                    ing.UnitPrice = price;
                }                
            }
            fic.Close();
        }

        /// <summary>
        /// Imports ingredients and recipes from two text files that respect the following format:
        /// - Ingredient file follows the same format as the one described in <see cref="ImportIngredientsOnly"/>.
        /// - The recipe file:
        ///    - A recipe is expressed on one line.
        ///    - Each line starts with the title followed by its ingredients.
        ///    - Ingredients are enclosed between brackets, identifiers and quantities appear between parentheses:
        ///         The recipe name [(id:99999;Quantity:100),(id:77777;Quantity:12000)]
        /// </summary>
        /// <param name="ingredientFilePath">Full path of the ingredient file to import.</param>
        /// <param name="recipeFilePath">Full path of the recipe file to import.</param>
        /// <remarks>
        /// Existing ingredients and/or recipes are preserved and duplicates are ignored.
        /// When importing recipes, any recipe that uses an ingredient that does not exist should be ignored.
        /// A contrario, a 0 quantity should be automatically corrected to 1. 
        /// </remarks>
        public void ImportRecipes(string ingredientFilePath, string recipeFilePath)
        {
            string ligne;           
            Dictionary<string, string> listIng = new Dictionary<string, string>();
            bool flagLeastOneIngMissing;
            // traitement des ingredients
            StreamReader fic = new StreamReader(ingredientFilePath);
            while ((ligne = fic.ReadLine()) != null)
            {
                string indiceIng = ligne.Substring(0, 4);
                string myString = ligne.Substring(5);
                string[] elementIng = myString.Split('[');
                double price = Convert.ToDouble(elementIng[1].Trim(']'));
                price = price / 100;
                if (_ingredients.FindByName(elementIng[0].TrimEnd(' ')) == null)
                {
                    IIngredient ing = _ingredients.Create(elementIng[0].TrimEnd(' '));
                    ing.UnitPrice = price;
                    listIng.Add(indiceIng, elementIng[0].TrimEnd(' '));                
                }
            }
            fic.Close();
            // triatement des recettes
            string recipeName;
            Dictionary<string, string[]> listRecipe = new Dictionary<string, string[]>();
            fic = new StreamReader(recipeFilePath);
            while ((ligne = fic.ReadLine()) != null)
            {
                string[] elementRec = ligne.Split('[');
                recipeName = elementRec[0].TrimEnd(' ');
                if (listRecipe.ContainsKey(recipeName) == false)
                {
                    string[] listIngredient = elementRec[1].TrimEnd(']').Split(',');
                    string[] listIngredientforRecipe = new string[listIngredient.Count()];
                    if (listIngredient[0] != "")
                    {                        
                        flagLeastOneIngMissing = false;
                        for (int i = 0; i < listIngredient.Count(); i++)
                        {
                            string[] unIngredient = listIngredient[i].Trim(new char[] { '(', ')' }).Split(';');
                            string[] unIngredientID = unIngredient[0].Split(':');
                            string[] unIngredientQuantite = unIngredient[1].Split(':');
                            if (listIng.ContainsKey(unIngredientID[1]) == true)
                            {
                                listIngredientforRecipe[i] = listIng[unIngredientID[1]] + ":" + unIngredientQuantite[1];
                            }
                            else
                            {
                                flagLeastOneIngMissing=true;
                            }                           
                        }
                        if (flagLeastOneIngMissing == false)
                        {
                            listRecipe.Add(recipeName, listIngredientforRecipe);
                        }
                    }
                    
                }
            }
            fic.Close();
            foreach (KeyValuePair<string, string[]> elem in listRecipe)
            {
                IRecipe rec = _recipes.Find(elem.Key, true);               
                foreach (string valeur in elem.Value)
                {
                    string[] moningredient = valeur.Split(':');
                    int quantite = Convert.ToInt32(moningredient[1]);
                    if (quantite == 0) quantite = 1;
                    rec.AddIngredient(_ingredients.FindByName(moningredient[0]), quantite);
                }
            }
        }
    }
}

