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
    public class T1IngredientManagement
    {
        [Test]
        public void t1_creating_ingredients()
        {
            KitchenContext c = new KitchenContext();
            Ingredient i1 = c.Ingredients.CreateIngredient( "Salade" );
            Assert.That( i1, Is.Not.Null );
            Assert.That( i1.Name, Is.EqualTo( "Salade" ) );
            Assert.That( i1.UnitPrice, Is.EqualTo( Ingredient.MinimalPrice ) );

            Ingredient i2 = c.Ingredients.CreateIngredient( "Aiguilletes de poulet" );
            Assert.That( i2, Is.Not.Null );
            Assert.That( i2.Name, Is.EqualTo( "Aiguilletes de poulet" ) );
            Assert.That( i2.UnitPrice, Is.EqualTo( Ingredient.MinimalPrice ) );

            Assert.Throws<ArgumentException>( () => c.Ingredients.CreateIngredient( "Salade" ) );
            Assert.Throws<ArgumentException>( () => c.Ingredients.CreateIngredient( "Aiguilletes de poulet" ) );
        }

        [Test]
        public void t2_ingredient_names_must_be_valid()
        {
            KitchenContext c = new KitchenContext();
            Assert.Throws<ArgumentException>( () => c.Ingredients.CreateIngredient( null ) );
            Assert.Throws<ArgumentException>( () => c.Ingredients.CreateIngredient( "" ) );
        }

        [Test]
        public void t3_ingredient_prices_must_be_greater_than_MinimalPrice()
        {
            KitchenContext c = new KitchenContext();
            Ingredient i = c.Ingredients.CreateIngredient( "Roquefort" );
            Assert.Throws<ArgumentException>( () => i.UnitPrice = 0.0 );
            Assert.Throws<ArgumentException>( () => i.UnitPrice = Ingredient.MinimalPrice / 2 );
            Assert.Throws<ArgumentException>( () => i.UnitPrice = -Ingredient.MinimalPrice );

            Assert.DoesNotThrow( () => i.UnitPrice = 54546 );
            Assert.That( i.UnitPrice, Is.EqualTo( 54546 ) );
            Assert.DoesNotThrow( () => i.UnitPrice = Ingredient.MinimalPrice );
            Assert.That( i.UnitPrice, Is.EqualTo( Ingredient.MinimalPrice ) );
        }

        [Test]
        public void t4_ingredients_can_be_found_by_their_names()
        {
            KitchenContext c = new KitchenContext();
            var names = Enumerable.Range( 0, 10 ).Select( _ => Guid.NewGuid().ToString() ).ToArray();

            foreach( var n in names )
            {
                c.Ingredients.CreateIngredient( n );
            }
            Assert.That( c.Ingredients.Count, Is.EqualTo( names.Length ), "Since names are different." );
            foreach( var n in names )
            {
                Ingredient i = c.Ingredients.FindByName( n );
                Assert.That( i.Name, Is.EqualTo( n ) );
            }
        }

        [Test]
        public void t5_ingredients_can_be_removed_from_kitchen()
        {
            KitchenContext c = new KitchenContext();
            var names = Enumerable.Range( 0, 10 ).Select( _ => Guid.NewGuid().ToString() ).ToArray();
            foreach( var n in names ) c.Ingredients.CreateIngredient( n );

            int count = c.Ingredients.Count;
            Assert.That( count, Is.EqualTo( names.Length ), "Since names are different." );

            foreach( var n in names )
            {
                Assert.That( c.Ingredients.RemoveByName( n ) );
                Assert.That( c.Ingredients.Count, Is.EqualTo( --count ) );
            }
            Assert.That( count, Is.EqualTo( 0 ) );
        }


    }
}
