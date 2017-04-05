using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NUnit.Framework;

namespace ITI.SimpleRecipes.Tests
{
    [TestFixture]
    public class PublicModelChecker
    {
        [Test]
        public void write_current_public_API_to_console_with_double_quotes()
        {
            Console.WriteLine( GetPublicAPI( typeof( Recipe ).Assembly ).ToString().Replace( "\"", "\"\"" ) );
        }

        [Test]
        public void public_API_is_not_modified()
        {
            var model = XElement.Parse( @"<Assembly Name=""ITI.SimpleRecipes"">
  <Types>
    <Type Name=""ITI.SimpleRecipes.Ingredient"">
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Method"" Name=""get_Name"" />
      <Member Type=""Method"" Name=""get_UnitPrice"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Field"" Name=""MinimalPrice"" />
      <Member Type=""Property"" Name=""Name"" />
      <Member Type=""Method"" Name=""set_UnitPrice"" />
      <Member Type=""Method"" Name=""ToString"" />
      <Member Type=""Property"" Name=""UnitPrice"" />
    </Type>
    <Type Name=""ITI.SimpleRecipes.IngredientCollection"">
      <Member Type=""Property"" Name=""Context"" />
      <Member Type=""Property"" Name=""Count"" />
      <Member Type=""Method"" Name=""CreateIngredient"" />
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Method"" Name=""FindByName"" />
      <Member Type=""Method"" Name=""get_Context"" />
      <Member Type=""Method"" Name=""get_Count"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Method"" Name=""RemoveByName"" />
      <Member Type=""Method"" Name=""ToString"" />
    </Type>
    <Type Name=""ITI.SimpleRecipes.IngredientInRecipe"">
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Method"" Name=""get_Ingredient"" />
      <Member Type=""Method"" Name=""get_Quantity"" />
      <Member Type=""Method"" Name=""get_Recipe"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Property"" Name=""Ingredient"" />
      <Member Type=""Property"" Name=""Quantity"" />
      <Member Type=""Property"" Name=""Recipe"" />
      <Member Type=""Method"" Name=""set_Quantity"" />
      <Member Type=""Method"" Name=""ToString"" />
    </Type>
    <Type Name=""ITI.SimpleRecipes.KitchenContext"">
      <Member Type=""Constructor"" Name="".ctor"" />
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Method"" Name=""get_Ingredients"" />
      <Member Type=""Method"" Name=""get_Recipes"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Property"" Name=""Ingredients"" />
      <Member Type=""Property"" Name=""Recipes"" />
      <Member Type=""Method"" Name=""ToString"" />
    </Type>
    <Type Name=""ITI.SimpleRecipes.Recipe"">
      <Member Type=""Method"" Name=""AddIngredient"" />
      <Member Type=""Property"" Name=""Context"" />
      <Member Type=""Property"" Name=""Cost"" />
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Method"" Name=""FindIngredient"" />
      <Member Type=""Method"" Name=""get_Context"" />
      <Member Type=""Method"" Name=""get_Cost"" />
      <Member Type=""Method"" Name=""get_IngredientCount"" />
      <Member Type=""Method"" Name=""get_Name"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Property"" Name=""IngredientCount"" />
      <Member Type=""Property"" Name=""Name"" />
      <Member Type=""Method"" Name=""RemoveIngredient"" />
      <Member Type=""Method"" Name=""ToString"" />
    </Type>
    <Type Name=""ITI.SimpleRecipes.RecipeCollection"">
      <Member Type=""Property"" Name=""Context"" />
      <Member Type=""Property"" Name=""Count"" />
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Method"" Name=""FindByName"" />
      <Member Type=""Method"" Name=""FindOrCreateRecipe"" />
      <Member Type=""Method"" Name=""get_Context"" />
      <Member Type=""Method"" Name=""get_Count"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Method"" Name=""Remove"" />
      <Member Type=""Method"" Name=""ToString"" />
    </Type>
  </Types>
</Assembly>
" );
            var current = GetPublicAPI( typeof( Recipe ).Assembly );
            if( !XElement.DeepEquals( model, current ) )
            {
                string m = model.ToString( SaveOptions.DisableFormatting );
                string c = current.ToString( SaveOptions.DisableFormatting );
                Assert.That( c, Is.EqualTo( m ) );
            }
        }

        XElement GetPublicAPI( Assembly a )
        {
            return new XElement( "Assembly",
                                  new XAttribute( "Name", a.GetName().Name ),
                                  new XElement( "Types",
                                                AllNestedTypes( a.GetExportedTypes() )
                                                 .OrderBy( t => t.FullName )
                                                 .Select( t => new XElement( "Type",
                                                                                new XAttribute( "Name", t.FullName ),
                                                                                t.GetMembers()
                                                                                 .OrderBy( m => m.Name )
                                                                                 .Select( m => new XElement( "Member",
                                                                                                              new XAttribute( "Type", m.MemberType ),
                                                                                                              new XAttribute( "Name", m.Name ) ) ) ) ) ) );
        }

        IEnumerable<Type> AllNestedTypes( IEnumerable<Type> types )
        {
            foreach( Type t in types )
            {
                yield return t;
                foreach( Type nestedType in AllNestedTypes( t.GetNestedTypes() ) )
                {
                    yield return nestedType;
                }
            }
        }
    }
}
