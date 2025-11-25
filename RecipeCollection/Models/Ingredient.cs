using MongoDB.Bson;

namespace RecipeCollection.Models
{
    public class Ingredient
    {
        public ObjectId Id { get; set; }
        public string IngredientName { get; set; }
        public string Taste {  get; set; }
        public bool IsChecked { get; set; }
    }

    public class IngredientModel
    {
        public ObjectId Id { get; set; }
        public string RecepieName { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
