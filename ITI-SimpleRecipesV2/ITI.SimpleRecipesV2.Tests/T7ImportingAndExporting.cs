using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;

namespace ITI.SimpleRecipesV2.Tests
{
    [TestFixture]
    public class T7ImportingAndExporting
    {
        [Test]
        public void t1_importing_ingredients_only_from_text_file()
        {
            IKitchenContext c = KitchenFactory.Create();
            string idExistAlready1 = Guid.NewGuid().ToString();
            string idExistAlready2 = Guid.NewGuid().ToString();
            c.Ingredients.Create( idExistAlready1 );
            c.Ingredients.Create( idExistAlready2 );
            Assert.That( c.Ingredients.Count, Is.EqualTo( 2 ) );
            c.ImportIngredientsOnly( Path.Combine( GetImportDataPath(), "Ingredients.txt" ) );
            Assert.That( c.Ingredients.Count, Is.EqualTo( 2 + 535 ) );
            Assert.That( c.Ingredients.FindByName( "Pousses d'épinard" ).UnitPrice, Is.EqualTo( 9.53 ) );
            // When duplicate ingredients appear, the first one wins.
            Assert.That( c.Ingredients.FindByName( "Ratatouille" ).UnitPrice, Is.EqualTo( 8.90 ) );
            Assert.That( c.Ingredients.FindByName( "Semoule fine" ).UnitPrice, Is.EqualTo( 9.20 ) );
        }

        [Test]
        public void t2_importing_recipes_from_text_file()
        {
            IKitchenContext c = KitchenFactory.Create();
            c.ImportRecipes( Path.Combine( GetImportDataPath(), "Ingredients.txt" ), Path.Combine( GetImportDataPath(), "Recipes.txt" ) );
            Assert.That( c.Ingredients.Count, Is.EqualTo( 535 ) );
            Assert.That( c.Recipes.Count, Is.EqualTo( 576 ) );
            Assert.That( c.Recipes.Find( "Tomates aux crevettes" ).Cost, Is.EqualTo( 77268.0 ) );
            Assert.That( c.Recipes.Find( "Risotto au safran" ).Cost, Is.EqualTo( 93616.340000000011 ) );
        }

        [Test]
        public void t3_importing_recipes_from_text_file_that_has_some_incorrect_data()
        {
            IKitchenContext c = KitchenFactory.Create();
            c.ImportRecipes( Path.Combine( GetImportDataPath(), "Ingredients.txt" ), Path.Combine( GetImportDataPath(), "BuggyRecipes.txt" ) );
            Assert.That( c.Ingredients.Count, Is.EqualTo( 535 ) );
            Assert.That( c.Recipes.Count, Is.EqualTo( 576 ) );
            Assert.That( c.Recipes.Find( "Tomates aux crevettes" ).Cost, Is.EqualTo( 77268.0 ) );
            Assert.That( c.Recipes.Find( "Risotto au safran" ).Cost, Is.EqualTo( 93616.340000000011 ) );
        }


        string GetImportDataPath()
        {
            string path = new Uri( Assembly.GetExecutingAssembly().CodeBase ).AbsolutePath;
            // Removes file name, debug or release and bin folder.
            path = Path.GetDirectoryName( Path.GetDirectoryName( Path.GetDirectoryName( path ) ) );
            return Path.Combine( path, "ImportData" );
        }
    }
}
