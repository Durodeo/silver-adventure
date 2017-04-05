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
    public class T5TrackingRemovedObjects
    {
        [Test]
        public void t1_removed_recipe_has_null_context()
        {
            KitchenContext c = new KitchenContext();
            Ingredient salade = c.Ingredients.CreateIngredient( "Salade" );
            Ingredient poulet = c.Ingredients.CreateIngredient( "Aiguilletes de poulet" );

            Recipe r = c.Recipes.FindOrCreateRecipe( "Salade de poulet" );
            Assert.That( r.Context, Is.SameAs( c ) );
            c.Recipes.Remove( r );
            Assert.That( r.Context, Is.Null );
        }

        [Test]
        public void t2_removed_ingredient_in_recipe_has_null_recipe_and_access_to_its_quantity_throws_an_invalid_operation_exception()
        {
            var c = new KitchenContext();
            var salade = c.Ingredients.CreateIngredient( "Salade" );
            var r = c.Recipes.FindOrCreateRecipe( "Salade de poulet" );
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
            KitchenContext c = new KitchenContext();
            Ingredient salade = c.Ingredients.CreateIngredient( "Salade" );
            Ingredient poulet = c.Ingredients.CreateIngredient( "Aiguilletes de poulet" );
            Ingredient olive = c.Ingredients.CreateIngredient( "Olive" );

            Recipe r1 = c.Recipes.FindOrCreateRecipe( "Salade de poulet" );
            IngredientInRecipe s1 = r1.AddIngredient( salade );
            IngredientInRecipe p1 = r1.AddIngredient( poulet, 2 );

            Recipe r2 = c.Recipes.FindOrCreateRecipe( "Salade de poulet aux olives" );
            IngredientInRecipe s2 = r2.AddIngredient( salade );
            IngredientInRecipe p2 = r2.AddIngredient( poulet, 2 );
            IngredientInRecipe o2 = r2.AddIngredient( olive, 15 );

            Recipe r3 = c.Recipes.FindOrCreateRecipe( "Poulet aux olives" );
            IngredientInRecipe p3 = r3.AddIngredient( poulet );
            IngredientInRecipe o3 = r3.AddIngredient( olive, 25 );

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

            c.Ingredients.RemoveByName( "Salade" );

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

            c.Ingredients.RemoveByName( "Olive" );
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
