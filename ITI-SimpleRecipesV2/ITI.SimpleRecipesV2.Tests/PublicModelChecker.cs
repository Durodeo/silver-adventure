using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NUnit.Framework;

namespace ITI.SimpleRecipesV2.Tests
{
    [TestFixture]
    public class PublicModelChecker
    {
        static readonly Assembly _modelAssembly = typeof( IKitchenContext ).Assembly;
        static readonly Assembly _implementationAssembly = typeof( KitchenFactory ).Assembly;

        [Explicit]
        [Test]
        public void write_current_public_model_API_to_console_with_double_quotes()
        {
            Console.WriteLine( GetPublicAPI( _modelAssembly ).ToString().Replace( "\"", "\"\"" ) );
        }

        [Explicit]
        [Test]
        public void write_current_public_implementation_to_console_with_double_quotes()
        {
            Console.WriteLine( GetPublicAPI( _implementationAssembly ).ToString().Replace( "\"", "\"\"" ) );
        }

        [Test]
        public void the_implementation_must_only_expose_the_KitchenFactory()
        {
            var model = XElement.Parse( @"
<Assembly Name=""ITI.SimpleRecipesV2.Impl"">
  <Types>
    <Type Name=""ITI.SimpleRecipesV2.KitchenFactory"">
      <Member Type=""Method"" Name=""Create"" />
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Method"" Name=""LoadFrom"" />
      <Member Type=""Method"" Name=""ToString"" />
    </Type>
  </Types>
</Assembly>
" );
            CheckPublicAPI( model, _implementationAssembly );
        }

        [Test]
        public void the_public_API_of_the_model_must_not_be_modified()
        {
            var model = XElement.Parse( @"
<Assembly Name=""ITI.SimpleRecipes.Model"">
  <Types>
    <Type Name=""ITI.SimpleRecipesV2.IIngredient"">
      <Member Type=""Method"" Name=""get_Name"" />
      <Member Type=""Method"" Name=""get_UnitPrice"" />
      <Member Type=""Property"" Name=""Name"" />
      <Member Type=""Method"" Name=""set_UnitPrice"" />
      <Member Type=""Property"" Name=""UnitPrice"" />
    </Type>
    <Type Name=""ITI.SimpleRecipesV2.IIngredientCollection"">
      <Member Type=""Property"" Name=""Context"" />
      <Member Type=""Method"" Name=""Create"" />
      <Member Type=""Method"" Name=""FindByName"" />
      <Member Type=""Method"" Name=""get_Context"" />
      <Member Type=""Method"" Name=""Remove"" />
    </Type>
    <Type Name=""ITI.SimpleRecipesV2.IIngredientInRecipe"">
      <Member Type=""Method"" Name=""get_Ingredient"" />
      <Member Type=""Method"" Name=""get_Quantity"" />
      <Member Type=""Method"" Name=""get_Recipe"" />
      <Member Type=""Property"" Name=""Ingredient"" />
      <Member Type=""Property"" Name=""Quantity"" />
      <Member Type=""Property"" Name=""Recipe"" />
      <Member Type=""Method"" Name=""set_Quantity"" />
    </Type>
    <Type Name=""ITI.SimpleRecipesV2.IKitchenContext"">
      <Member Type=""Method"" Name=""get_Ingredients"" />
      <Member Type=""Method"" Name=""get_Recipes"" />
      <Member Type=""Method"" Name=""ImportIngredientsOnly"" />
      <Member Type=""Method"" Name=""ImportRecipes"" />
      <Member Type=""Property"" Name=""Ingredients"" />
      <Member Type=""Property"" Name=""Recipes"" />
      <Member Type=""Method"" Name=""Save"" />
    </Type>
    <Type Name=""ITI.SimpleRecipesV2.IRecipe"">
      <Member Type=""Method"" Name=""AddIngredient"" />
      <Member Type=""Property"" Name=""Context"" />
      <Member Type=""Property"" Name=""Cost"" />
      <Member Type=""Method"" Name=""FindIngredient"" />
      <Member Type=""Method"" Name=""get_Context"" />
      <Member Type=""Method"" Name=""get_Cost"" />
      <Member Type=""Method"" Name=""get_IngredientCount"" />
      <Member Type=""Method"" Name=""get_Name"" />
      <Member Type=""Property"" Name=""IngredientCount"" />
      <Member Type=""Property"" Name=""Name"" />
      <Member Type=""Method"" Name=""RemoveIngredient"" />
    </Type>
    <Type Name=""ITI.SimpleRecipesV2.IRecipeCollection"">
      <Member Type=""Property"" Name=""Context"" />
      <Member Type=""Method"" Name=""Find"" />
      <Member Type=""Method"" Name=""get_Context"" />
      <Member Type=""Method"" Name=""Remove"" />
    </Type>
    <Type Name=""ITI.SimpleRecipesV2.KitchenConstant"">
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Method"" Name=""get_MinimalIngredientPrice"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Property"" Name=""MinimalIngredientPrice"" />
      <Member Type=""Method"" Name=""ToString"" />
    </Type>
  </Types>
</Assembly>
" );
            CheckPublicAPI( model, _modelAssembly );
        }

        void CheckPublicAPI( XElement model, Assembly assembly )
        {
            XElement current = GetPublicAPI( assembly );
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
