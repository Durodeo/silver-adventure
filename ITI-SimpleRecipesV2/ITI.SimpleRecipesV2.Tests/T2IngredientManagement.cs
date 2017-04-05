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
    public class T2IngredientManagement
    {
        [Test]
        public void t1_creating_ingredients()
        {
            IKitchenContext c = KitchenFactory.Create();
            IIngredient i1 = c.Ingredients.Create( "Salade" );
            Assert.That( i1, Is.Not.Null );
            Assert.That( i1.Name, Is.EqualTo( "Salade" ) );
            Assert.That( i1.UnitPrice, Is.EqualTo( KitchenConstant.MinimalIngredientPrice ) );

            IIngredient i2 = c.Ingredients.Create( "Aiguilletes de poulet" );
            Assert.That( i2, Is.Not.Null );
            Assert.That( i2.Name, Is.EqualTo( "Aiguilletes de poulet" ) );
            Assert.That( i2.UnitPrice, Is.EqualTo( KitchenConstant.MinimalIngredientPrice ) );

            Assert.Throws<ArgumentException>( () => c.Ingredients.Create( "Salade" ) );
            Assert.Throws<ArgumentException>( () => c.Ingredients.Create( "Aiguilletes de poulet" ) );
        }

        [Test]
        public void t2_ingredient_names_must_be_valid()
        {
            IKitchenContext c = KitchenFactory.Create();
            Assert.Throws<ArgumentException>( () => c.Ingredients.Create( null ) );
            Assert.Throws<ArgumentException>( () => c.Ingredients.Create( "" ) );
        }

        [Test]
        public void t3_ingredient_prices_must_be_greater_than_MinimalPrice()
        {
            IKitchenContext c = KitchenFactory.Create();
            IIngredient i = c.Ingredients.Create( "Roquefort" );
            Assert.Throws<ArgumentException>( () => i.UnitPrice = 0.0 );
            Assert.Throws<ArgumentException>( () => i.UnitPrice = KitchenConstant.MinimalIngredientPrice / 2 );
            Assert.Throws<ArgumentException>( () => i.UnitPrice = -KitchenConstant.MinimalIngredientPrice );

            Assert.DoesNotThrow( () => i.UnitPrice = 54546 );
            Assert.That( i.UnitPrice, Is.EqualTo( 54546 ) );
            Assert.DoesNotThrow( () => i.UnitPrice = KitchenConstant.MinimalIngredientPrice );
            Assert.That( i.UnitPrice, Is.EqualTo( KitchenConstant.MinimalIngredientPrice ) );
        }

        [Test]
        public void t4_ingredients_can_be_found_by_their_names()
        {
            IKitchenContext c = KitchenFactory.Create();
            var names = Enumerable.Range( 0, 10 ).Select( _ => Guid.NewGuid().ToString() ).ToArray();

            foreach( var n in names )
            {
                c.Ingredients.Create( n );
            }
            Assert.That( c.Ingredients.Count, Is.EqualTo( names.Length ), "Since names are different." );
            foreach( var n in names )
            {
                IIngredient i = c.Ingredients.FindByName( n );
                Assert.That( i.Name, Is.EqualTo( n ) );
            }
        }

        [Test]
        public void t5_ingredients_can_be_removed_from_kitchen()
        {
            IKitchenContext c = KitchenFactory.Create();
            var names = Enumerable.Range( 0, 10 ).Select( _ => Guid.NewGuid().ToString() ).ToArray();
            foreach( var n in names ) c.Ingredients.Create( n );

            int count = c.Ingredients.Count;
            Assert.That( count, Is.EqualTo( names.Length ), "Since names are different." );

            foreach( var n in names )
            {
                Assert.That( c.Ingredients.Remove( n ) );
                Assert.That( c.Ingredients.Count, Is.EqualTo( --count ) );
            }
            Assert.That( count, Is.EqualTo( 0 ) );
        }

        [Test]
        public void t6_ingredient_collection_works_as_a_standard_collection()
        {
            IKitchenContext c = KitchenFactory.Create();
            var names = Enumerable.Range( 0, 60 ).Select( _ => Guid.NewGuid().ToString() ).ToArray();
            foreach( var n in names ) c.Ingredients.Create( n );
            Assert.That( string.Join( ", ", c.Ingredients.Select( r => r.Name ) ), Is.EqualTo( string.Join( ", ", names ) ) );
            // Challenging the non-generic IEnumerable implementation.
            IEnumerable goodOldOne = c.Ingredients;
            // Use OfType instead of Cast: Cast is optimized and directly uses the IEnumerable<T> whenever T matches!
            Assert.That( string.Join( ", ", goodOldOne.OfType<IIngredient>().Select( r => r.Name ) ), Is.EqualTo( string.Join( ", ", names ) ) );
        }

    }
}
