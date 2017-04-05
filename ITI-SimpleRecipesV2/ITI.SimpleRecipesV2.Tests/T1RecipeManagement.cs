using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections;

namespace ITI.SimpleRecipesV2.Tests
{
    [TestFixture]
    public class T1RecipeManagement
    {
        [Test]
        public void t1_creating_recipes()
        {
            IKitchenContext c = KitchenFactory.Create();
            IRecipe r1 = c.Recipes.Find( "Boeuf strogonoff", createIfNotFound: true );
            Assert.That( r1 != null );
            Assert.That( r1.Context == c );
            Assert.That( r1.Name == "Boeuf strogonoff" );

            IRecipe r2 = c.Recipes.Find( "Oeuf en gelée", true );
            Assert.That( r2 != null );
            Assert.That( r2.Context == c );
            Assert.That( r2.Name == "Oeuf en gelée" );

            IRecipe r3 = c.Recipes.Find( "Oeuf en gelée" );
            Assert.That( r2 == r3 );

            IRecipe r4 = c.Recipes.Find( "Boeuf strogonoff", true );
            Assert.That( r4 == r1 );
        }

        [Test]
        public void t2_recipes_names_must_be_valid()
        {
            IKitchenContext c = KitchenFactory.Create();
            Assert.Throws<ArgumentException>( () => c.Recipes.Find( null, false ) );
            Assert.Throws<ArgumentException>( () => c.Recipes.Find( "", false ) );
            Assert.Throws<ArgumentException>( () => c.Recipes.Find( null, true ) );
            Assert.Throws<ArgumentException>( () => c.Recipes.Find( "", true ) );
        }

        [Test]
        public void t3_recipes_can_be_found_by_their_names()
        {
            IKitchenContext c = KitchenFactory.Create();
            var names = Enumerable.Range( 0, 10 ).Select( _ => Guid.NewGuid().ToString() ).ToArray();

            foreach( var n in names )
            {
                c.Recipes.Find( n, true );
            }
            Assert.That( c.Recipes.Count, Is.EqualTo( names.Length ), "Since names are different." );
            foreach( var n in names )
            {
                IRecipe r = c.Recipes.Find( n );
                Assert.That( r.Name, Is.EqualTo( n ) );
                Assert.That( r.Context, Is.SameAs( c ) );
            }
        }

        [Test]
        public void t4_recipes_can_be_removed_from_kitchen()
        {
            IKitchenContext c = KitchenFactory.Create();
            var names = Enumerable.Range( 0, 10 ).Select( _ => Guid.NewGuid().ToString() ).ToArray();
            foreach( var n in names )
            {
                var r = c.Recipes.Find( n, true );
                Assert.That( r.Context == c );
            }

            int count = c.Recipes.Count;
            Assert.That( count, Is.EqualTo( names.Length ), "Since names are different." );

            foreach( var n in names )
            {
                c.Recipes.Remove( c.Recipes.Find( n ) );
                Assert.That( c.Recipes.Count, Is.EqualTo( --count ) );
            }
            Assert.That( count, Is.EqualTo( 0 ) );
        }

        [Test]
        public void t5_recipe_collection_works_as_a_standard_collection()
        {
            IKitchenContext c = KitchenFactory.Create();
            var names = Enumerable.Range( 0, 60 ).Select( _ => Guid.NewGuid().ToString() ).ToArray();
            foreach( var n in names ) c.Recipes.Find( n, true );

            Assert.That( string.Join( ", ", c.Recipes.Select( r => r.Name ) ), Is.EqualTo( string.Join( ", ", names ) ) );
            // Challenging the non-generic IEnumerable implementation.
            IEnumerable goodOldOne = c.Recipes;
            // Use OfType instead of Cast: Cast is optimized and directly uses the IEnumerable<T> whenever T matches!
            Assert.That( string.Join( ", ", goodOldOne.OfType<IRecipe>().Select( r => r.Name ) ), Is.EqualTo( string.Join( ", ", names ) ) );
        }
    }
}
