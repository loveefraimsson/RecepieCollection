using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RecipeCollection.Models;
using System.Net;

namespace RecipeCollection.Controllers
{
    public class IngredientController : Controller
    {
        public IActionResult Index()
        {
            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("recepie_collection");
            var collection = database.GetCollection<Ingredient>("ingredient");

            List<Ingredient> ingredients = collection.Find(i => true).ToList();

            return View(ingredients);
        }

        /* SHOW THE CHOSEN INDREDIENT */
        public IActionResult ShowIngredient(string Id)
        {
            ObjectId ingredientId = new ObjectId(Id);

            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("recepie_collection");
            var collection = database.GetCollection<Ingredient>("ingredient");

            Ingredient ingredient = collection.Find(i => i.Id == ingredientId).FirstOrDefault();

            return View(ingredient);
        }

        /* CREATE AN INGREDIENT */
        public IActionResult CreateIngredient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Ingredient ingredient)
        {
            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("recepie_collection");
            var collection = database.GetCollection<Ingredient>("ingredient");

            collection.InsertOne(ingredient);

            return Redirect("/Ingredient");
        }

        /* EDIT AN INGREDIENT */
        public IActionResult EditIngredient(string Id)
        { 
            ObjectId ingredientId = new ObjectId(Id);

            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("recepie_collection");
            var collection = database.GetCollection<Ingredient>("ingredient");

            Ingredient ingredient = collection.Find(i => i.Id == ingredientId).FirstOrDefault();

            return View(ingredient);
        }

        [HttpPost]
        public IActionResult Edit(string Id, Ingredient ingredient)
        {
            ObjectId ingredientId = new ObjectId(Id);

            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("recepie_collection");
            var collection = database.GetCollection<Ingredient>("ingredient");

            ingredient.Id = ingredientId;

            collection.ReplaceOne(i => i.Id == ingredientId, ingredient);

            return Redirect("/Ingredient");
        }

        /* DELETES AN INGREDIENT */
        [HttpPost]
        public IActionResult DeleteIngredient(string Id)
        {
            ObjectId ingredientId = new ObjectId(Id);

            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("recepie_collection");
            var collection = database.GetCollection<Ingredient>("ingredient");
            
            collection.DeleteOne(i => i.Id == ingredientId);

            return Redirect("/Ingredient");
        }
    }
}
