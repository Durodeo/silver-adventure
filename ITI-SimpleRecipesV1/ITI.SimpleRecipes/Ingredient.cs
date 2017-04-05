using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipes
{
    /// <summary>
    /// Simple model of an ingredient: a name and a unit price.
    /// </summary>
    public class Ingredient
    {
        String _name;
        Double _unitPrice;
        /// <summary>
        /// The minimal possible unit price is 0.01 (1 cent).
        /// </summary>
        public static readonly double MinimalPrice = 0.01;

        /// <summary>
        /// This constructor may need parameters!!!
        /// </summary>
        internal Ingredient(String name)
        {
            _name = name;
            _unitPrice = MinimalPrice;
        }

        /// <summary>
        /// Gets the name of the ingredient. Never null nor empty.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the price of this ingredient. Always greater or equal to <see cref="MinimalPrice"/>.
        /// </summary>
        public double UnitPrice
        {
            get { return _unitPrice; }
            set
            {
                if (value < MinimalPrice)
                {
                    throw new ArgumentException("the unitPrice must be greater or equal minimalPrice");
                }
                _unitPrice = value;
            }
        }

    }
}
