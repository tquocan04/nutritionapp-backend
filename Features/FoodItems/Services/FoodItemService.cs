using AutoMapper;
using Domains;
using Features.FoodItems.DTOs;
using Features.FoodItems.Exceptions;
using Features.FoodItems.Requests;

namespace Features.FoodItems.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public FoodItemService(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public async Task CreateNewFoodItemAsync(Guid userId, List<FoodItemRequest> requests)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            DateTime now = DateTime.Now;

            var dailyPlan = await _manager.FoodItem.GetDailyPlanAsync(userId, today, true)
                ?? throw new DailyPlanNotFoundException(today);

            if (now.Hour >= 0 && now.Hour <= 8)
            {
                var breakfast = await _manager.FoodItem.GetBreakfastAsync(dailyPlan.Breakfast_id, true)
                    ?? throw new MealNotFoundException(dailyPlan.Breakfast_id);

                foreach (var req in requests)
                {
                    var item = _mapper.Map<ItemBreakfast>(req);

                    item.Calories = req.Calories / 100 * req.Grams;
                    item.Carb = req.Carbs / 100 * req.Grams;
                    item.Fat = req.Fat / 100 * req.Grams;
                    item.Protein = req.Protein / 100 * req.Grams;

                    item.Breakfast_id = breakfast.Id;
                    await _manager.FoodItem.AddNewBreakfastItemAsync(item);

                    breakfast.TotalCalories += item.Calories;
                    breakfast.TotalFats += item.Fat;
                    breakfast.TotalCarbs += item.Carb;
                    breakfast.TotalProteins += item.Protein;

                    dailyPlan.TotalCalories += item.Calories;
                    dailyPlan.TotalFats += item.Fat;
                    dailyPlan.TotalCarbs += item.Carb;
                    dailyPlan.TotalProteins += item.Protein;
                }
            }
            else if (now.Hour >= 9 && now.Hour <= 13)
            {
                var lunch = await _manager.FoodItem.GetLunchAsync(dailyPlan.Lunch_id, true)
                    ?? throw new MealNotFoundException(dailyPlan.Lunch_id);

                foreach (var req in requests)
                {
                    var item = _mapper.Map<ItemLunch>(req);

                    item.Calories = req.Calories / 100 * req.Grams;
                    item.Carb = req.Carbs / 100 * req.Grams;
                    item.Fat = req.Fat / 100 * req.Grams;
                    item.Protein = req.Protein / 100 * req.Grams;

                    item.Lunch_id = lunch.Id;
                    await _manager.FoodItem.AddNewLunchItemAsync(item);

                    lunch.TotalCalories += item.Calories;
                    lunch.TotalFats += item.Fat;
                    lunch.TotalCarbs += item.Carb;
                    lunch.TotalProteins += item.Protein;

                    dailyPlan.TotalCalories += item.Calories;
                    dailyPlan.TotalFats += item.Fat;
                    dailyPlan.TotalCarbs += item.Carb;
                    dailyPlan.TotalProteins += item.Protein;
                }
            }
            else
            {
                var dinner = await _manager.FoodItem.GetDinnerAsync(dailyPlan.Dinner_id, true)
                    ?? throw new MealNotFoundException(dailyPlan.Dinner_id);

                foreach (var req in requests)
                {
                    var item = _mapper.Map<ItemDinner>(req);

                    item.Calories = req.Calories / 100 * req.Grams;
                    item.Carb = req.Carbs / 100 * req.Grams;
                    item.Fat = req.Fat / 100 * req.Grams;
                    item.Protein = req.Protein / 100 * req.Grams;

                    item.Dinner_id = dinner.Id;
                    await _manager.FoodItem.AddNewDinnerItemAsync(item);

                    dinner.TotalCalories += item.Calories;
                    dinner.TotalFats += item.Fat;
                    dinner.TotalCarbs += item.Carb;
                    dinner.TotalProteins += item.Protein;

                    dailyPlan.TotalCalories += item.Calories;
                    dailyPlan.TotalFats += item.Fat;
                    dailyPlan.TotalCarbs += item.Carb;
                    dailyPlan.TotalProteins += item.Protein;
                }
            }

            await _manager.SaveAsync();
        }

        public async Task<MealsDTO> GetMealAsync(Guid userId, DateOnly date)
        {
            var dailyPlan = await _manager.FoodItem.GetDailyPlanAsync(userId, date, true)
                ?? throw new DailyPlanNotFoundException(date);

            var breakfast = await _manager.FoodItem.GetBreakfastAsync(dailyPlan.Breakfast_id, false)
                ?? throw new MealNotFoundException(dailyPlan.Breakfast_id);

            var lunch = await _manager.FoodItem.GetLunchAsync(dailyPlan.Lunch_id, false)
                ?? throw new MealNotFoundException(dailyPlan.Lunch_id);

            var dinner = await _manager.FoodItem.GetDinnerAsync(dailyPlan.Dinner_id, false)
                ?? throw new MealNotFoundException(dailyPlan.Dinner_id);

            MealsDTO meals = new()
            {
                Breakfast = _mapper.Map<BreakfastDTO>(breakfast),
                Lunch = _mapper.Map<LunchDTO>(lunch),
                Dinner = _mapper.Map<DinnerDTO>(dinner),
            };

            meals.Breakfast.Items = _mapper.Map<IEnumerable<ItemDTO>>(await _manager.FoodItem.GetBreakfastItemListAsync(breakfast.Id, false));
            meals.Lunch.Items = _mapper.Map<IEnumerable<ItemDTO>>(await _manager.FoodItem.GetLunchItemListAsync(lunch.Id, false));
            meals.Dinner.Items = _mapper.Map<IEnumerable<ItemDTO>>(await _manager.FoodItem.GetDinnerItemListAsync(dinner.Id, false));

            return meals;
        }
    }
}
