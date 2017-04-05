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
    public class T6SavingAndLoading
    {
        [Test]
        public void t1_saving_and_reloading_kitchen()
        {
            var rnd = new Random();
            IKitchenContext c = KitchenFactory.Create();
            FillKitchenWithRandomRecipesAndIngredients( rnd, c );

            IKitchenContext c2 = WriteAndRead( c );
            AreEqualKitchen( c, c2 );
        }

        static void FillKitchenWithRandomRecipesAndIngredients( Random rnd, IKitchenContext c )
        {
            int ingCount = rnd.Next( 30 ) + 20;
            for( int i = 0; i < ingCount; i++ )
            {
                var ing = c.Ingredients.Create( Guid.NewGuid().ToString() );
                ing.UnitPrice = KitchenConstant.MinimalIngredientPrice + rnd.NextDouble() * 20;
            }
            int recCount = rnd.Next( 20 ) + 10;
            for( int i = 0; i < ingCount; i++ )
            {
                var rec = c.Recipes.Find( Guid.NewGuid().ToString(), true );
                int nbIngInRec = rnd.Next( 10 ) + 5;
                for( int j = 0; j < nbIngInRec; j++ )
                {
                    int iIngToAdd = rnd.Next( c.Ingredients.Count );
                    var ing = c.Ingredients.Skip( iIngToAdd ).First();
                    rec.AddIngredient( ing, rnd.Next( 3 ) + 1 );
                }
            }
        }

        static void AreEqualKitchen( IKitchenContext c1, IKitchenContext c2 )
        {
            var c1Ing = c1.Ingredients.Select( i => i.Name + '|' + i.UnitPrice );
            var c2Ing = c2.Ingredients.Select( i => i.Name + '|' + i.UnitPrice );
            Assert.That( c1Ing, Is.EqualTo( c2Ing ) );
            var c1Rec = c1.Recipes.Select( r => r.Name + '|' + r.Cost + '|' + r.IngredientCount );
            var c2Rec = c2.Recipes.Select( r => r.Name + '|' + r.Cost + '|' + r.IngredientCount );
            Assert.That( c1Rec, Is.EqualTo( c2Rec ) );
        }

        static IKitchenContext WriteAndRead( IKitchenContext c )
        {
            using( MemoryStream s = new MemoryStream() )
            {
                c.Save( s );
                s.Position = 0;
                return KitchenFactory.LoadFrom( s );
            }
        }
    }
}
