using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RecipeCollection.Models;
using System.Diagnostics;

namespace RecipeCollection.Controllers
{
    public class RecepieController : Controller
    {

        /* SHOWS ALL RECEPIES */
        public IActionResult Index()
        {
            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("recepie_collection");
            var collection = database.GetCollection<Recepie>("recepie");

            List<Recepie> recepies = collection.Find(r => true).ToList();

            return View(recepies);
        }

        /* CREATES RECEPIES */
        public IActionResult CreateRecepie()
        {
            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("recepie_collection");
            var collection = database.GetCollection<Ingredient>("ingredient");

            List<Ingredient> ingredients = collection.Find(i => true).ToList();

            IngredientModel ingredient = new IngredientModel();
            ingredient.Ingredients = ingredients;

            return View(ingredient);
        }

        [HttpPost]
        public IActionResult Create(IngredientModel model)
        {
            List<Ingredient> RecepieIngredient = new List<Ingredient>();

            //Creates a new list of ingredients
            for (int i = 0; i < model.Ingredients.Count; i++)
            {
                if (model.Ingredients[i].IsChecked == true)
                {
                    RecepieIngredient.Add(model.Ingredients[i]);
                }
            }

            //Creates a new recepie model
            Recepie recepie = new Recepie();
            recepie.RecepieName = model.RecepieName;
            recepie.RecepieIngredients = RecepieIngredient;

            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("recepie_collection");
            var collection = database.GetCollection<Recepie>("recepie");

            collection.InsertOne(recepie);

            return Redirect("/Recepie");
        }

        /* SHOWS THE CHOOSEN RECEPIES */
        public IActionResult ShowRecepie(string Id)
        {
            ObjectId recepieId = new ObjectId(Id);

            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("recepie_collection");
            var collection = database.GetCollection<Recepie>("recepie");

            Recepie recepie = collection.Find(r => r.Id == recepieId).FirstOrDefault();

            return View(recepie);
        }

        /* EDITS A RECEPIE */
        public IActionResult EditRecepie(string Id)
        {
            List<EditRecepieModel> viewModel = new List<EditRecepieModel>();

            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("recepie_collection");
            
            //Gets all ingredients
            var IngredientCollection = database.GetCollection<Ingredient>("ingredient");

            //Gets the choosen recepie
            ObjectId recepieId = new ObjectId(Id); 
            var recepieCollection = database.GetCollection<Recepie>("recepie");
            Recepie choosenRecepie = recepieCollection.Find(r => r.Id == recepieId).FirstOrDefault();

            List<Ingredient> newListOfIngredienses = new List<Ingredient>();
            
            List<Ingredient> allIngredients = IngredientCollection.Find(b => true).ToList();
            var choosenRecepieIngredients = choosenRecepie.RecepieIngredients.ToList();

            //Loops through all ingredients in database and if it doesn't exist in the list of recepie ingredients, it adds it to a new list
            foreach (var item in allIngredients)
            {
                var id = choosenRecepieIngredients.Find(i => i.Id == item.Id);

                if (id == null)
                {
                    newListOfIngredienses.Add(item);
                }
            }

            //Adds the ingredients in the recepie to the list
            foreach (var item in choosenRecepieIngredients)
            {
                newListOfIngredienses.Add(item);
            }

            //Creates a ingredient model
            IngredientModel ingredient = new IngredientModel();
            ingredient.Ingredients = newListOfIngredienses;
            ingredient.RecepieName = choosenRecepie.RecepieName;
            ingredient.Id = choosenRecepie.Id;

            return View(ingredient);

        }

        [HttpPost]
        public IActionResult Edit(IngredientModel model, string Id)
        {
            ObjectId recepieId = new ObjectId(Id);

            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("recepie_collection");
            var collection = database.GetCollection<Recepie>("recepie");

            //Creates a list of the edit of ingredients to add to the database
            List<Ingredient> RecepieIngredient = new List<Ingredient>();

            for (int i = 0; i < model.Ingredients.Count; i++)
            {
                if (model.Ingredients[i].IsChecked == true)
                {
                    RecepieIngredient.Add(model.Ingredients[i]);
                }
            }

            Recepie recepie = new Recepie();
            recepie.RecepieName = model.RecepieName;
            recepie.RecepieIngredients = RecepieIngredient;
            recepie.Id = recepieId;

            collection.ReplaceOne(r => r.Id == recepieId, recepie);

            return Redirect("/Recepie");
        }

        /* DELETES A RECEPIE */
        public IActionResult DeleteRecepie(string Id)
        {
            ObjectId recepieId = new ObjectId(Id);

            MongoClient dbClient = new MongoClient();
            var database = dbClient.GetDatabase("recepie_collection");
            var collection = database.GetCollection<Recepie>("recepie");

            collection.DeleteOne(i => i.Id == recepieId);

            return Redirect("/Recepie");
        }
    }
}
