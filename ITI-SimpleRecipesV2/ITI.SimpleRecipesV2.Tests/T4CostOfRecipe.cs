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
    public class T4CorstOfRecipe
    {
        [Test]
        public void t1_cost_of_a_recipe_is_the_sum_of_the_ingredient_costs_with_their_quantities()
        {
            IKitchenContext c = KitchenFactory.Create();
            IIngredient salade = c.Ingredients.Create( "Salade" );
            IIngredient poulet = c.Ingredients.Create( "Aiguilletes de poulet" );

            IRecipe r = c.Recipes.Find( "Salade de poulet", true );
            IIngredientInRecipe s = r.AddIngredient( salade );
            IIngredientInRecipe p = r.AddIngredient( poulet, 2 );

            Assert.That( r.Cost, Is.EqualTo( KitchenConstant.MinimalIngredientPrice * 3 ) );
        }

        [Test]
        public void t2_cost_of_a_recipe_depends_on_the_cost_of_the_ingredients_and_their_quantities()
        {
            Random random = new Random();
            var c = KitchenFactory.Create();
            var generator = Enumerable.Range( 0, 10 );
            var r = c.Recipes.Find( "Test", true );
            var ingredients = generator.Select( i => c.Ingredients.Create( "n°" + i ) ).ToArray();
            var costs = generator.Select( i => random.NextDouble()*100 + KitchenConstant.MinimalIngredientPrice ).ToArray();
            var theoricalCost = 0.0;
            for( int i = 0; i < ingredients.Length; ++i )
            {
                ingredients[i].UnitPrice = costs[i];
                r.AddIngredient( ingredients[i] );
                theoricalCost += costs[i];
                Assert.That( r.Cost, Is.EqualTo( theoricalCost ).Within( 5 ).Ulps );
            }

            var newCosts = generator.Select( i => random.NextDouble() * 100 + KitchenConstant.MinimalIngredientPrice ).ToArray();
            for( int i = 0; i < ingredients.Length; ++i ) ingredients[i].UnitPrice = newCosts[i];
            theoricalCost = newCosts.Sum();
            Assert.That( r.Cost, Is.EqualTo( theoricalCost ) );

            var quantities = generator.Select( i => random.Next() * 10 ).ToArray();
            for( int i = 0; i < ingredients.Length; ++i )
            {
                var deltaQ = quantities[i] - 1;
                if( deltaQ <= 0 )
                {
                    r.RemoveIngredient( ingredients[i] );
                    theoricalCost -= ingredients[i].UnitPrice;
                }
                else
                {
                    r.AddIngredient( ingredients[i], deltaQ );
                    theoricalCost += ingredients[i].UnitPrice * deltaQ;
                }
                Assert.That( r.Cost, Is.EqualTo( theoricalCost ).Within( 5 ).Ulps );
            }
        }

    }
}
