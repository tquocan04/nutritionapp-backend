using ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.FoodItems.Exceptions
{
    internal class MealNotFoundException : NotFoundException
    {
        public MealNotFoundException(Guid id)
            : base($"MealId: {id} not found in database") { }
    }
}
