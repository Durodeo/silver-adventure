using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.SimpleRecipes.Tests
{
    [TestFixture]
    public class T3RecipeWithIngredient
    {
        [Test]
        public void t1_adding_ingredients_to_recipes()
        {
            KitchenContext c = new KitchenContext();
            Ingredient salade = c.Ingredients.CreateIngredient( "Salade" );
            Ingredient poulet = c.Ingredients.CreateIngredient( "Aiguilletes de poulet" );

            Recipe r = c.Recipes.FindOrCreateRecipe( "Salade de poulet" );
            IngredientInRecipe s = r.AddIngredient( salade );
            Assert.That( s != null );
            Assert.That( s.Ingredient, Is.SameAs( salade ) );
            Assert.That( s.Recipe, Is.SameAs( r ) );
            Assert.That( s.Quantity, Is.EqualTo( 1 ) );

            IngredientInRecipe p = r.AddIngredient( poulet, 2 );
            Assert.That( p != null );
            Assert.That( p.Ingredient, Is.SameAs( poulet ) );
            Assert.That( p.Recipe, Is.SameAs( r ) );
            Assert.That( p.Quantity, Is.EqualTo( 2 ) );

            Assert.That( r.FindIngredient( salade ), Is.SameAs( s ) );
            Assert.That( r.FindIngredient( poulet ), Is.SameAs( p ) );
        }

        [Test]
        public void t2_ingredients_quantity_must_be_positive()
        {
            KitchenContext c = new KitchenContext();
            Ingredient salade = c.Ingredients.CreateIngredient( "Salade" );
            Recipe r = c.Recipes.FindOrCreateRecipe( "Salade de poulet" );
            Assert.Throws<ArgumentException>( () => r.AddIngredient( salade, 0 ) );
            Assert.Throws<ArgumentException>( () => r.AddIngredient( salade, -1 ) );
            Assert.Throws<ArgumentException>( () => r.AddIngredient( salade, -100 ) );

            IngredientInRecipe s = r.AddIngredient( salade );
            Assert.Throws<ArgumentException>( () => s.Quantity = 0 );
            Assert.Throws<ArgumentException>( () => s.Quantity = -1 );
            Assert.Throws<ArgumentException>( () => s.Quantity = -20 );

            Assert.DoesNotThrow( () => s.Quantity = 45057 );
            Assert.That( s.Quantity, Is.EqualTo( 45057 ) );
        }

        [Test]
        public void t3_ingredients_can_be_removed_from_a_recipe()
        {
            KitchenContext c = new KitchenContext();
            var names = Enumerable.Range( 0, 10 ).Select( _ => Guid.NewGuid().ToString() ).ToArray();
            var ingredients = names.Select( n => c.Ingredients.CreateIngredient( n ) ).ToArray();

            Recipe r = c.Recipes.FindOrCreateRecipe( "Test" );
            foreach( var i in ingredients ) r.AddIngredient( i );

            foreach( var i in ingredients )
            {
                Assert.That( r.FindIngredient( i ).Ingredient, Is.SameAs( i ) );
            }
            int count = r.IngredientCount;
            Assert.That( count, Is.EqualTo( names.Length ), "Since names are different." );

            foreach( var i in ingredients )
            {
                Assert.That( r.RemoveIngredient( i ) );
                Assert.That( r.IngredientCount, Is.EqualTo( --count ) );
                Assert.That( r.RemoveIngredient( i ), Is.False );
            }
            Assert.That( count, Is.EqualTo( 0 ) );
        }


    }
}
