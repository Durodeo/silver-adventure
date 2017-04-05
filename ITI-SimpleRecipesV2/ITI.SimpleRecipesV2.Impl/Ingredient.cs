using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipesV2.Impl
{
    class Ingredient:IIngredient
    {

        String _name;
        Double _unitPrice;

        internal Ingredient(String name)
        {
            _name = name;
            _unitPrice = KitchenConstant.MinimalIngredientPrice;
        }
        /// <summary>
         /// Gets the name of the ingredient. Never null nor empty.
         /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets or sets the price of this ingredient. Always greater or equal to <see cref="MinimalPrice"/>.
        /// </summary>
        public double UnitPrice
        {
            get { return _unitPrice; }
            set
            {
                if (value < KitchenConstant.MinimalIngredientPrice)
                {
                    throw new ArgumentException("the unitPrice must be greater or equal minimalPrice");
                }
                _unitPrice = value;
            }
        }
    }
}
