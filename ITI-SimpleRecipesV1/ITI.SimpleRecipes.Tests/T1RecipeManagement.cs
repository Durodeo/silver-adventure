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
    public class T1RecipeManagement
    {
        [Test]
        public void t1_creating_recipes()
        {
            KitchenContext c = new KitchenContext();
            Recipe r1 = c.Recipes.FindOrCreateRecipe( "Boeuf strogonoff" );
            Assert.That( r1 != null );
            Assert.That( r1.Context == c );
            Assert.That( r1.Name == "Boeuf strogonoff" );

            Recipe r2 = c.Recipes.FindOrCreateRecipe( "Oeuf en gelée" );
            Assert.That( r2 != null );
            Assert.That( r2.Context == c );
            Assert.That( r2.Name == "Oeuf en gelée" );

            Recipe r3 = c.Recipes.FindOrCreateRecipe( "Oeuf en gelée" );
            Assert.That( r2 == r3 );

            Recipe r4 = c.Recipes.FindOrCreateRecipe( "Boeuf strogonoff" );
            Assert.That( r4 == r1 );
        }

        [Test]
        public void t2_recipes_names_must_be_valid()
        {
            KitchenContext c = new KitchenContext();
            Assert.Throws<ArgumentException>( () => c.Recipes.FindOrCreateRecipe( null ) );
            Assert.Throws<ArgumentException>( () => c.Recipes.FindOrCreateRecipe( "" ) );
        }

        [Test]
        public void t3_recipes_can_be_found_by_their_names()
        {
            KitchenContext c = new KitchenContext();
            var names = Enumerable.Range( 0, 10 ).Select( _ => Guid.NewGuid().ToString() ).ToArray();

            foreach( var n in names )
            {
                c.Recipes.FindOrCreateRecipe( n );
            }
            Assert.That( c.Recipes.Count, Is.EqualTo( names.Length ), "Since names are different." );
            foreach( var n in names )
            {
                Recipe r = c.Recipes.FindByName( n );
                Assert.That( r.Name, Is.EqualTo( n ) );
            }
        }

        [Test]
        public void t4_recipes_can_be_removed_from_kitchen()
        {
            KitchenContext c = new KitchenContext();
            var names = Enumerable.Range( 0, 10 ).Select( _ => Guid.NewGuid().ToString() ).ToArray();
            foreach( var n in names )
            {
                var r = c.Recipes.FindOrCreateRecipe( n );
                Assert.That( r.Context == c );
            }

            int count = c.Recipes.Count;
            Assert.That( count, Is.EqualTo( names.Length ), "Since names are different." );

            foreach( var n in names )
            {
                c.Recipes.Remove( c.Recipes.FindByName( n ) );
                Assert.That( c.Recipes.Count, Is.EqualTo( --count ) );
            }
            Assert.That( count, Is.EqualTo( 0 ) );
        }


    }
}
