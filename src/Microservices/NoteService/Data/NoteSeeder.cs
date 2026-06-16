using MongoDB.Driver;
using NoteService.Entities;

namespace NoteService.Data
{
    public static class NoteSeeder
    {
        public static async Task SeedAsync(IMongoDatabase database)
        {
            var collection = database.GetCollection<Note>("Notes");

            if (await collection.CountDocumentsAsync(FilterDefinition<Note>.Empty) > 0) return;

            await collection.InsertManyAsync(new List<Note>
            {
                new() { PatientId = 1, PatientName = "TestNone", Content = "Le patient déclare qu'il 'se sent très bien' Poids égal ou inférieur au poids recommandé" },
                new() { PatientId = 2, PatientName = "TestBorderline", Content = "Le patient déclare qu'il ressent beaucoup de stress au travail Il se plaint également que son audition est anormale dernièrement" },
                new() { PatientId = 2, PatientName = "TestBorderline", Content = "Le patient déclare avoir fait une réaction aux médicaments au cours des 3 derniers mois Il remarque également que son audition continue d'être anormale" },
                new() { PatientId = 3, PatientName = "TestInDanger", Content = "Le patient déclare qu'il fume depuis peu" },
                new() { PatientId = 3, PatientName = "TestInDanger", Content = "Le patient déclare qu'il est fumeur et qu'il a cessé de fumer l'année dernière Il se plaint également de crises d'apnée respiratoire anormales Tests de laboratoire indiquant un taux de cholestérol LDL élevé" },
                new() { PatientId = 4, PatientName = "TestEarlyOnset", Content = "Le patient déclare qu'il lui est devenu difficile de monter les escaliers Il se plaint également d'être essoufflé Tests de laboratoire indiquant que les anticorps sont élevés Réaction aux médicaments" },
                new() { PatientId = 4, PatientName = "TestEarlyOnset", Content = "Le patient déclare qu'il a mal au dos lorsqu'il reste assis pendant longtemps" },
                new() { PatientId = 4, PatientName = "TestEarlyOnset", Content = "Le patient déclare avoir commencé à fumer depuis peu Hémoglobine A1C supérieure au niveau recommandé" },
                new() { PatientId = 4, PatientName = "TestEarlyOnset", Content = "Taille, Poids, Cholestérol, Vertige et Réaction" }
            });
        }
    }
}