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
    public class T4CorstOfRecipe
    {
        [Test]
        public void t1_cost_of_a_recipe_is_the_sum_of_the_ingredient_costs_with_their_quantities()
        {
            KitchenContext c = new KitchenContext();
            Ingredient salade = c.Ingredients.CreateIngredient( "Salade" );
            Ingredient poulet = c.Ingredients.CreateIngredient( "Aiguilletes de poulet" );

            Recipe r = c.Recipes.FindOrCreateRecipe( "Salade de poulet" );
            IngredientInRecipe s = r.AddIngredient( salade );
            IngredientInRecipe p = r.AddIngredient( poulet, 2 );

            Assert.That( r.Cost, Is.EqualTo( Ingredient.MinimalPrice * 3 ) );
        }

        [Test]
        public void t2_cost_of_a_recipe_depends_on_the_cost_of_the_ingredients_and_their_quantities()
        {
            Random random = new Random();
            var c = new KitchenContext();
            var generator = Enumerable.Range( 0, 10 );
            var r = c.Recipes.FindOrCreateRecipe( "Test" );
            var ingredients = generator.Select( i => c.Ingredients.CreateIngredient( "n°" + i ) ).ToArray();
            var costs = generator.Select( i => random.NextDouble()*100 + Ingredient.MinimalPrice ).ToArray();
            var theoricalCost = 0.0;
            for( int i = 0; i < ingredients.Length; ++i )
            {
                ingredients[i].UnitPrice = costs[i];
                r.AddIngredient( ingredients[i] );
                theoricalCost += costs[i];
                Assert.That( r.Cost, Is.EqualTo( theoricalCost ).Within( 5 ).Ulps );
            }

            var newCosts = generator.Select( i => random.NextDouble() * 100 + Ingredient.MinimalPrice ).ToArray();
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
