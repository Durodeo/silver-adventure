using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.SimpleRecipesV2
{
    /// <summary>
    /// Simple model of an ingredient: a name and a unit price.
    /// </summary>
    public interface IIngredient
    {
        /// <summary>
        /// Gets the name of the ingredient. Never null nor empty.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the price of this ingredient. Always greater or equal to <see cref="MinimalPrice"/>.
        /// </summary>
        double UnitPrice { get; set; }

    }
}
