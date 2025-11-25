using MongoDB.Bson;
using System.Runtime.Serialization;

namespace RecipeCollection.Models
{
    public class Recepie
    {
        public ObjectId Id {  get; set; }
        public string RecepieName { get; set; }
        
        public List<Ingredient> RecepieIngredients { get; set; }
    }
}
