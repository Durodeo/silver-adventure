using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.SimpleRecipesV2.Tests
{
    [TestFixture]
    public class T5TrackingRemovedObjects
    {
        [Test]
        public void t1_removed_recipe_has_null_context()
        {
            IKitchenContext c = KitchenFactory.Create();
            IIngredient salade = c.Ingredients.Create( "Salade" );
            IIngredient poulet = c.Ingredients.Create( "Aiguilletes de poulet" );

            IRecipe r = c.Recipes.Find( "Salade de poulet", true );
            Assert.That( r.Context, Is.SameAs( c ) );
            c.Recipes.Remove( r );
            Assert.That( r.Context, Is.Null );
        }

        [Test]
        public void t2_removed_ingredient_in_recipe_has_null_recipe_and_access_to_its_quantity_throws_an_invalid_operation_exception()
        {
            var c = KitchenFactory.Create();
            var salade = c.Ingredients.Create( "Salade" );
            var r = c.Recipes.Find( "Salade de poulet", true );
            var inRecipe = r.AddIngredient( salade );
            Assert.That( inRecipe.Ingredient, Is.SameAs( salade ) );
            Assert.That( inRecipe.Recipe, Is.SameAs( r ) );
            Assert.DoesNotThrow( () => inRecipe.Quantity = inRecipe.Quantity + 1 );

            r.RemoveIngredient( salade );
            Assert.That( inRecipe.Ingredient, Is.SameAs( salade ) );
            Assert.That( inRecipe.Recipe, Is.Null );
            Assert.Throws<InvalidOperationException>( () => Console.Write( inRecipe.Quantity ) );
            Assert.Throws<InvalidOperationException>( () => inRecipe.Quantity = 3 );
        }

        [Test]
        public void t3_removing_an_ingredient_from_the_kitchen_removes_it_from_all_the_recipes_that_used_it()
        {
            IKitchenContext c = KitchenFactory.Create();
            IIngredient salade = c.Ingredients.Create( "Salade" );
            IIngredient poulet = c.Ingredients.Create( "Aiguilletes de poulet" );
            IIngredient olive = c.Ingredients.Create( "Olive" );

            IRecipe r1 = c.Recipes.Find( "Salade de poulet", true );
            IIngredientInRecipe s1 = r1.AddIngredient( salade );
            IIngredientInRecipe p1 = r1.AddIngredient( poulet, 2 );

            IRecipe r2 = c.Recipes.Find( "Salade de poulet aux olives", true );
            IIngredientInRecipe s2 = r2.AddIngredient( salade );
            IIngredientInRecipe p2 = r2.AddIngredient( poulet, 2 );
            IIngredientInRecipe o2 = r2.AddIngredient( olive, 15 );

            IRecipe r3 = c.Recipes.Find( "Poulet aux olives", true );
            IIngredientInRecipe p3 = r3.AddIngredient( poulet );
            IIngredientInRecipe o3 = r3.AddIngredient( olive, 25 );

            Assert.That( r1.IngredientCount, Is.EqualTo( 2 ) );
            Assert.That( s1.Recipe, Is.SameAs( r1 ) );
            Assert.That( p1.Recipe, Is.SameAs( r1 ) );

            Assert.That( r2.IngredientCount, Is.EqualTo( 3 ) );
            Assert.That( s2.Recipe, Is.SameAs( r2 ) );
            Assert.That( p2.Recipe, Is.SameAs( r2 ) );
            Assert.That( o2.Recipe, Is.SameAs( r2 ) );

            Assert.That( r3.IngredientCount, Is.EqualTo( 2 ) );
            Assert.That( p3.Recipe, Is.SameAs( r3 ) );
            Assert.That( o3.Recipe, Is.SameAs( r3 ) );

            c.Ingredients.Remove( "Salade" );

            Assert.That( r1.IngredientCount, Is.EqualTo( 1 ) );
            Assert.That( s1.Recipe, Is.Null );
            Assert.That( p1.Recipe, Is.SameAs( r1 ) );

            Assert.That( r2.IngredientCount, Is.EqualTo( 2 ) );
            Assert.That( s2.Recipe, Is.Null );
            Assert.That( p2.Recipe, Is.SameAs( r2 ) );
            Assert.That( o2.Recipe, Is.SameAs( r2 ) );

            Assert.That( r3.IngredientCount, Is.EqualTo( 2 ) );
            Assert.That( p3.Recipe, Is.SameAs( r3 ) );
            Assert.That( o3.Recipe, Is.SameAs( r3 ) );

            c.Ingredients.Remove( "Olive" );
            Assert.That( r1.IngredientCount, Is.EqualTo( 1 ) );
            Assert.That( s1.Recipe, Is.Null );
            Assert.That( p1.Recipe, Is.SameAs( r1 ) );

            Assert.That( r2.IngredientCount, Is.EqualTo( 1 ) );
            Assert.That( s2.Recipe, Is.Null );
            Assert.That( p2.Recipe, Is.SameAs( r2 ) );
            Assert.That( o2.Recipe, Is.Null );

            Assert.That( r3.IngredientCount, Is.EqualTo( 1 ) );
            Assert.That( p3.Recipe, Is.SameAs( r3 ) );
            Assert.That( o3.Recipe, Is.Null );
        }

    }
}
